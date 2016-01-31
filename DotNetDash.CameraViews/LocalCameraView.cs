using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetDash.CameraViews
{
    class LocalCameraView : CameraView
    {
        public LocalCameraView()
        {
            CameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            CameraSelector.Content = new LocalCameraSelector();
        }

        public FilterInfoCollection CameraDevices { get; private set; }

        public FilterInfo SelectedFilter
        {
            set
            {
                CurrentDevice = value != null ? new VideoCaptureDevice(value.MonikerString) : null;
            }
        }
    }

    sealed class LocalCameraViewProcessor : IViewProcessor
    {
        public FrameworkElement View { get; } = new LocalCameraView();
    }

    [CustomViewFactory(Name = "Laptop Camera")]
    public class LocalCameraViewProcessorFactory : IViewProcessorFactory
    {
        public IViewProcessor Create()
        {
            return new LocalCameraViewProcessor();
        }
    }
}
