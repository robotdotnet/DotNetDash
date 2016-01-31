using AForge.Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotNetDash.CameraViews
{
    /// <summary>
    /// Interaction logic for BaseCameraView.xaml
    /// </summary>
    public partial class CameraView : UserControl, INotifyPropertyChanged
    {
        public CameraView()
        {
            DataContext = this;
            InitializeComponent();
            IsVisibleChanged += OnIsVisibleChanged;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var isVisible = (bool)e.NewValue;
            if (isVisible)
                CurrentDevice?.Start();
            else
                CurrentDevice?.SignalToStop();
        }

        public IVideoSource currentDevice;

        public IVideoSource CurrentDevice
        {
            get
            {
                return currentDevice;
            }
            set
            {
                if (currentDevice != null)
                {
                    currentDevice.NewFrame -= NewFrame;
                    currentDevice.Stop();
                }
                currentDevice = value;
                if (value != null)
                {
                    value.NewFrame += NewFrame;
                    value.Start();
                }
            }
        }

        private void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var bitmap = eventArgs.Frame.Clone() as System.Drawing.Bitmap;
            Dispatcher.BeginInvoke((Action)(() => View = bitmap));
        }

        private System.Drawing.Bitmap cameraView;

        public System.Drawing.Bitmap View
        {
            get { return cameraView; }
            set
            {
                cameraView = value;
                NotifyProperyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyProperyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
