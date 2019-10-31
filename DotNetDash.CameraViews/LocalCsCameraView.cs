using FRC.CameraServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetDash.CameraViews
{
    public class LocalCsCameraView : CsCameraView
    {
        public LocalCsCameraView()
        {
            CameraSelector.Content = new LocalCameraSelector();
        }

        public UsbCameraInfo[] CameraDevices => UsbCamera.EnumerateUsbCameras();

        public UsbCameraInfo? SelectedFilter
        {
            set
            {
                if (value == null)
                {
                    CurrentDevice?.Stop();
                    CurrentDevice = null;
                    return;
                }

                UsbCamera camera = new UsbCamera("Usb", value.Value.Path);
                BitmapSink sink = new BitmapSink("Streamer");
                sink.Source = camera;

                CurrentDevice = new CsVideoSource(sink, camera);
                ;
            }
        }
    }

    sealed class LocalCsCameraViewProcessor : IViewProcessor
    {
        public FrameworkElement View { get; } = new LocalCsCameraView();
    }

    [CustomViewFactory(Name = "USB Camera")]
    public class LocalCCameraViewProcessorFactory : IViewProcessorFactory
    {
        public IViewProcessor Create()
        {
            return new LocalCsCameraViewProcessor();
        }
    }
}
