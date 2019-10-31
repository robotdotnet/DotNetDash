using FRC.CameraServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetDash.CameraViews
{
    public class LocalCsCameraView : CameraView
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
                    CurrentDevice = null;
                    return;
                }
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
