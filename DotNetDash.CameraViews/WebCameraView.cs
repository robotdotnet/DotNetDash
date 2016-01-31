using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DotNetDash.CameraViews
{
    class WebCameraView : CameraView
    {
        private const int CameraServerPort = 1180;

        public WebCameraView()
        {
            var layout = new StackPanel { Orientation = Orientation.Horizontal };
            layout.Children.Add(new TextBlock { Text = "Webcam IP Address/URL with port:" });
            var urlBox = new TextBox { Margin = new Thickness(10, 0, 0, 0) };
            urlBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Path = new PropertyPath(nameof(WebcamUrl)),
                Mode = BindingMode.OneWayToSource
            });
            layout.Children.Add(urlBox);
            CameraSelector.Content = layout;
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
    }

    sealed class WebcamViewProcessor : IViewProcessor
    {
        public FrameworkElement View { get; } = new WebCameraView();
    }

    [CustomViewFactory(Name = "Webcam")]
    public class WebcamViewProcessorFactory : IViewProcessorFactory
    {
        public IViewProcessor Create()
        {
            return new WebcamViewProcessor();
        }
    }
}
