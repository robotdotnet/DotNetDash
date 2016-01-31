using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetDash.CameraViews
{
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
