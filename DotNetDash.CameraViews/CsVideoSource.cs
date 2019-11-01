using FRC.CameraServer;
using FRC.CameraServer.OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDash.CameraViews
{
    public class CsVideoSource
    {
        private Thread readThread;
        private volatile bool isRunning = true;
        private BitmapSink cvSink;
        private Bitmap bitmap;
        private VideoSource source;

        public event EventHandler<Bitmap> NewFrame;

        public CsVideoSource(BitmapSink sink, VideoSource source)
        {
            cvSink = sink;
            this.source = source;
        }

        public void Start()
        {
            readThread = new Thread(ThreadMain);
            readThread.IsBackground = true;
            readThread.Start();
        }

        private void ThreadMain()
        {
            while (isRunning)
            {
                var ret = cvSink.GrabFrame(ref bitmap);
                if (!isRunning) return;
                if (ret == 0) continue;

                NewFrame?.Invoke(this, bitmap);
            }
        }

        public void Stop()
        {
            isRunning = false;
            if (readThread != null)
            {
                readThread.Join();
            }
        }
    }
}
