using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DotNetDash.CameraViews
{
    /// <summary>
    /// Interaction logic for BaseCameraView.xaml
    /// </summary>
    public partial class CsCameraView : UserControl, INotifyPropertyChanged
    {
        public CsCameraView()
        {
            DataContext = this;
            InitializeComponent();
            IsVisibleChanged += OnIsVisibleChanged;
        }

        volatile bool doUpdate = false;

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var isVisible = (bool)e.NewValue;
            doUpdate = isVisible;
        }

        public CsVideoSource currentDevice;

        public CsVideoSource CurrentDevice
        {
            get
            {
                return currentDevice;
            }
            set
            {
                if (currentDevice != null)
                {
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

        private void NewFrame(object sender, Bitmap bitmap)
        {
            if (!doUpdate) return;
            var previous = View;
            Stream stream = unusedStream;
            stream.Seek(0, SeekOrigin.Begin);
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Seek(0, SeekOrigin.Begin);

            Dispatcher.Invoke(() =>
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                View = image;
                unusedStream = previous != null ? previous.StreamSource : new MemoryStream();
            });
        }

        private Stream unusedStream = new MemoryStream();
        private BitmapImage cameraView;

        public BitmapImage View
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
