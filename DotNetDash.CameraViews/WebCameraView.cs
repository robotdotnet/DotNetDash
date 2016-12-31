using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NetworkTables;

namespace DotNetDash.CameraViews
{
    class WebCameraView : CameraView
    {
        public WebCameraView()
        {
            var layout = new StackPanel { Orientation = Orientation.Horizontal };
            layout.Children.Add(new TextBlock { Text = "Camera URL:" });
            var urlBox = new ComboBox { MinWidth = 300, Margin = new Thickness(10, 0, 0, 0) };
            urlBox.IsEditable = true;
            CameraServerCameras = GetCameraServerCameras();
            urlBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Path = new PropertyPath(nameof(WebcamUrl)),
                Mode = BindingMode.OneWayToSource
            });
            urlBox.SetBinding(ComboBox.ItemsSourceProperty, new Binding
            {
                Path = new PropertyPath(nameof(CameraServerCameras))
            });
            layout.Children.Add(urlBox);
            CameraSelector.Content = layout;
        }

        private ObservableCollection<string> GetCameraServerCameras()
        {
            var cache = new Dictionary<string, string[]>();
            var collection = new ObservableCollection<string>();

            var ntInterface = (App.Current as App).Container.GetExport<INetworkTablesInterface>().Value;
            var cameraTable = ntInterface.GetTable("CameraProcessor");
            var context = SynchronizationContext.Current;

            Action<string, NotifyFlags> subTableCallback = (name, flags) =>
            {
                if (flags == NotifyFlags.NotifyNew || flags == NotifyFlags.NotifyImmediate)
                {
                    cameraTable.GetSubTable(name).AddTableListener("streams", (subTable, __, value, valueFlags) =>
                    {
                        context.Post(state =>
                        {
                            var streams = value.GetStringArray();
                            for (int i = 0; i < streams.Length; i++)
                            {
                                if(streams[i].StartsWith("mjpeg:", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    streams[i] = streams[i].Substring("mjpeg:".Length);
                                }
                            }
                            valueFlags &= ~NotifyFlags.NotifyLocal;
                            switch (valueFlags)
                            {
                                case NotifyFlags.NotifyImmediate:
                                case NotifyFlags.NotifyNew:
                                    cache[name] = streams;
                                    foreach (var stream in streams)
                                    {
                                        collection.Add(stream);
                                    }
                                    break;
                                case NotifyFlags.NotifyDelete:
                                    foreach (var stream in cache[name])
                                    {
                                        collection.Remove(stream);
                                    }
                                    break;
                                case NotifyFlags.NotifyUpdate:
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
                    }, true);
                }
            };

            cameraTable.AddSubTableListener((table, name, _, flags) => subTableCallback(name, flags), true);

            foreach(var subTable in cameraTable.GetSubTables())
            {
                subTableCallback(subTable, NotifyFlags.NotifyImmediate);
            }

            return collection;
        }

        public string WebcamUrl
        {
            set
            {
                try
                {

                    CurrentDevice = new AForge.Video.MJPEGStream(value);
                }
                catch (ArgumentException)
                {
                    //Purposefully swallow the error.  This comes from an invalid video source being passed in as the webcam url.
                    CurrentDevice = null;
                    //And disable the video device
                }
            }
        }

        public ObservableCollection<string> CameraServerCameras { get; }
    }

    sealed class WebcamViewProcessor : IViewProcessor
    {
        public FrameworkElement View { get; } = new WebCameraView();
    }

    [CustomViewFactory(Name = "RoboRIO/MJPEG Camera")]
    public class WebcamViewProcessorFactory : IViewProcessorFactory
    {
        public IViewProcessor Create()
        {
            return new WebcamViewProcessor();
        }
    }
}
