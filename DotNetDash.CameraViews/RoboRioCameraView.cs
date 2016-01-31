using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetDash.CameraViews
{
    class RoboRioCameraView : CameraView
    {
        public RoboRioCameraView()
        {
            CurrentDevice = new FrcMJpegStream(Properties.Settings.Default.TeamNumber);
        }
    }

    sealed class RoboRioCameraViewProcessor : IViewProcessor
    {
        public FrameworkElement View { get; } = new RoboRioCameraView();
    }

    [CustomViewFactory(Name = "USB Camera")]
    public class RoboRioCameraViewProcessorFactory : IViewProcessorFactory
    {
        public IViewProcessor Create()
        {
            return new RoboRioCameraViewProcessor();
        }
    }
}
