using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FRC.CameraServer;
using FRC.NetworkTables;

namespace DotNetDash.CameraViews
{
    class WebCameraView : CsCameraView
    {
        public WebCameraView(INetworkTablesInterface ntInterface)
        {
            CameraServerCameras = GetCameraServerCameras(ntInterface);
            CameraServerView = new CollectionViewSource();
            CameraServerView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(CameraStream.CameraName)));
            CameraServerView.Source = CameraServerCameras;

            var layout = new StackPanel { Orientation = Orientation.Horizontal };
            layout.Children.Add(new TextBlock { Text = "Camera URL:" });
            var urlBox = new ComboBox { MinWidth = 300, Margin = new Thickness(10, 0, 0, 0) };
            urlBox.IsEditable = true;
            urlBox.IsReadOnly = false;

            urlBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Path = new PropertyPath(nameof(WebcamUrl)),
                Mode = BindingMode.OneWayToSource
            });
            urlBox.SetBinding(ComboBox.ItemsSourceProperty, new Binding
            {
                Source = CameraServerView
            });

            urlBox.GroupStyle.Add(GroupStyle.Default);
            urlBox.DisplayMemberPath = nameof(CameraStream.Stream);

            layout.Children.Add(urlBox);

            CameraSelector.Content = layout;
        }

        private static ObservableCollection<CameraStream> GetCameraServerCameras(INetworkTablesInterface ntInterface)
        {
            var cache = new Dictionary<string, IEnumerable<CameraStream>>();
            var collection = new ObservableCollection<CameraStream>();
            
            var cameraTable = ntInterface.GetTable("CameraPublisher");
            var context = SynchronizationContext.Current;

            Action<string, NotifyFlags> subTableCallback = (name, flags) =>
            {
                if (flags == NotifyFlags.New || flags == NotifyFlags.Immediate)
                {
                    cameraTable.GetSubTable(name).AddEntryListener("streams", (NetworkTable subTable, ReadOnlySpan<char> key, in NetworkTableEntry notification, in RefNetworkTableValue value, NotifyFlags valueFlags) =>
                    {
                        var streamsArray = value.GetStringArray().ToArray();
                        context.Post(state =>
                        {
                            for (int i = 0; i < streamsArray.Length; i++)
                            {
                                if(streamsArray[i].StartsWith("mjpeg:", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    streamsArray[i] = streamsArray[i].Substring("mjpeg:".Length);
                                }
                            }

                            var streams = streamsArray.Select(stream => new CameraStream { CameraName = name, Stream = stream });

                            valueFlags &= ~NotifyFlags.Local;
                            switch (valueFlags)
                            {
                                case NotifyFlags.Immediate:
                                case NotifyFlags.New:
                                    cache[name] = streams;
                                    foreach (var stream in streams)
                                    {
                                        collection.Add(stream);
                                    }
                                    break;
                                case NotifyFlags.Delete:
                                    foreach (var stream in cache[name])
                                    {
                                        collection.Remove(stream);
                                    }
                                    break;
                                case NotifyFlags.Update:
                                    foreach (var stream in cache[name])
                                    {
                                        collection.Remove(stream);
                                    }
                                    cache[name] = streams;
                                    foreach (var stream in streams)
                                    {
                                        collection.Add(stream);
                                    }
                                    break;
                            }
                        }, null);
                    }, NotifyFlags.Update | NotifyFlags.Immediate);
                }
                else if (flags == NotifyFlags.Delete)
                {
                    cache.Remove(name);
                    var toRemove = collection.Where(stream => stream.CameraName == name).ToList();
                    foreach (var item in toRemove)
                    {
                        collection.Remove(item);
                    }
                }
            };

            cameraTable.AddSubTableListener((table, name, flags) => subTableCallback(name.ToString(), flags), true);

            foreach(var subTable in cameraTable.GetSubTables())
            {
                subTableCallback(subTable, NotifyFlags.Immediate);
            }

            return collection;
        }

        public string WebcamUrl
        {
            set
            {
                try
                {

                    HttpCamera camera = new HttpCamera("Http", value);
                    BitmapSink sink = new BitmapSink("Streamer");
                    sink.Source = camera;

                    CurrentDevice = new CsVideoSource(sink, camera);
                }
                catch (ArgumentException)
                {
                    //Purposefully swallow the error.  This comes from an invalid video source being passed in as the webcam url.
                    CurrentDevice = null;
                    //And disable the video device
                }
            }
        }

        private ObservableCollection<CameraStream> CameraServerCameras { get; }

        public CollectionViewSource CameraServerView { get; }
    }

    struct CameraStream
    {
        public string CameraName { get; set; }
        public string Stream { get; set; }

        public override string ToString() => Stream;
    }

    sealed class WebcamViewProcessor : IViewProcessor
    {
        private INetworkTablesInterface ntInterface;

        public WebcamViewProcessor(INetworkTablesInterface ntInterface)
        {
            this.ntInterface = ntInterface;
            View = new WebCameraView(ntInterface);
        }

        public FrameworkElement View { get; }
    }

    [CustomViewFactory(Name = "RoboRIO/MJPEG Camera")]
    public class WebcamViewProcessorFactory : IViewProcessorFactory
    {
        private INetworkTablesInterface ntInterface;
        
        [ImportingConstructor]
        public WebcamViewProcessorFactory(INetworkTablesInterface ntInterface)
        {
            this.ntInterface = ntInterface;
        }

        public IViewProcessor Create()
        {
            return new WebcamViewProcessor(ntInterface);
        }
    }
}
