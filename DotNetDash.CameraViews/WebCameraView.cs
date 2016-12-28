using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DotNetDash.CameraViews
{
    class WebCameraView : CameraView
    {
        public WebCameraView()
        {
            var layout = new StackPanel { Orientation = Orientation.Horizontal };
            layout.Children.Add(new TextBlock { Text = "Camera URL:" });
            var urlBox = new TextBox { MinWidth = 300, Margin = new Thickness(10, 0, 0, 0) };
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

    [CustomViewFactory(Name = "MJPEG Stream")]
    public class WebcamViewProcessorFactory : IViewProcessorFactory
    {
        public IViewProcessor Create()
        {
            return new WebcamViewProcessor();
        }
    }
}
