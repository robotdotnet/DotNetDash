using FRC.CameraServer;
using FRC.CameraServer.Interop;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash.CameraViews
{
    public class BitmapSink : ImageSink
    {
        private CS_RawFrame frame = new CS_RawFrame();
        Mat tmpMat;
        unsafe byte* dataPtr;
        int width;
        int height;
        int pixelFormat;

        private MatType GetCvFormat(PixelFormat format)
        {
            MatType type = MatType.CV_8UC1;
            switch (format)
            {
                case PixelFormat.BGR:
                    type = MatType.CV_8UC3;
                    break;
                case PixelFormat.YUYV:
                case PixelFormat.RGB565:
                    type = MatType.CV_8UC2;
                    break;
            }
            return type;
        }

        public BitmapSink(string name)
            : base(CsCore.CreateRawSink(name.AsSpan()))
        {

        }

        public long GrabFrame(ref Bitmap image)
        {
            return GrabFrame(ref image, .225);
        }

        public unsafe long GrabFrame(ref Bitmap image, double timeout)
        {
            frame.width = 0;
            frame.height = 0;
            frame.pixelFormat = (int)PixelFormat.BGR;

            ulong rv = CsCore.GrabRawSinkFrameTimeout(Handle, ref frame, timeout);
            if (rv == 0) return 0;

            var format = GetCvFormat((PixelFormat)frame.pixelFormat);

            if (dataPtr != frame.data || width != frame.width || height != frame.height || pixelFormat != frame.pixelFormat)
            {
                dataPtr = frame.data;
                width = frame.width;
                height = frame.height;
                pixelFormat = frame.pixelFormat;
                tmpMat = new Mat(frame.height, frame.width, format, (IntPtr)frame.data);
            }

            if (image == null || image.Width != width || image.Height != height || image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }

            BitmapConverter.ToBitmap(tmpMat, image);
            return (long)rv;
        }
    }
}
