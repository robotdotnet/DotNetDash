using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DotNetDash.CameraViews
{
    /// <summary>
    /// Interaction logic for LocalCameraView.xaml
    /// </summary>
    public partial class LocalCameraView : UserControl, INotifyPropertyChanged
    {
        public LocalCameraView()
        {
            DataContext = this;
            CameraDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
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

        public FilterInfoCollection CameraDevices { get; private set; }

        public FilterInfo SelectedFilter
        {
            set
            {
                CurrentDevice = value != null ? new VideoCaptureDevice(value.MonikerString) : null;
            }
        }

        public VideoCaptureDevice currentDevice;

        public VideoCaptureDevice CurrentDevice
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
            Dispatcher.BeginInvoke((Action)(() => CameraView = bitmap));
        }

        private System.Drawing.Bitmap cameraView;
        private System.Drawing.Bitmap processingView;

        public System.Drawing.Bitmap CameraView
        {
            get { return cameraView; }
            set
            {
                cameraView = value;
                processingView = (System.Drawing.Bitmap)(cameraView.Clone());
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
