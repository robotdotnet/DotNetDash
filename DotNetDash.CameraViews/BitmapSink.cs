using FRC.CameraServer;
using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsPixelFormat = FRC.CameraServer.PixelFormat;

namespace DotNetDash.CameraViews
{
    public class BitmapSink : ImageSink
    {
        private CS_RawFrame frame = new CS_RawFrame();
        unsafe byte* dataPtr;
        int width;
        int height;
        int pixelFormat;


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
            frame.pixelFormat = (int)CsPixelFormat.BGR;

            ulong rv = CsCore.GrabRawSinkFrameTimeout(Handle, ref frame, timeout);
            if (rv == 0) return 0;


            if (dataPtr != frame.data || width != frame.width || height != frame.height || pixelFormat != frame.pixelFormat)
            {
                dataPtr = frame.data;
                width = frame.width;
                height = frame.height;
                pixelFormat = frame.pixelFormat;
            }

            if (image == null || image.Width != width || image.Height != height || image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }

            ToBitmap(frame, image);

            //BitmapConverter.ToBitmap(tmpMat, image);
            return (long)rv;
        }

        private unsafe void ToBitmap(in CS_RawFrame src, Bitmap dst)
        {
            var pf = dst.PixelFormat;

            if (pf != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                throw new Exception("Unsupported pixel format");
            }

            int w = src.width;
            int h = src.height;

            Rectangle rect = new Rectangle(0, 0, w, h);
            BitmapData bd = null;

            try
            {
                bd = dst.LockBits(rect, ImageLockMode.WriteOnly, pf);

                IntPtr srcData = (IntPtr)src.data;
                byte* pSrc = (byte*)srcData.ToPointer();
                byte* pDst = (byte*)bd.Scan0.ToPointer();
                int ch = 3;
                int sstep = src.width * ch;
                int dstep = ((src.width * 3) + 3) / 4 * 4;
                int stride = bd.Stride;

                if (sstep == dstep)
                {
                    CopyMemory(pDst, pSrc, src.totalData);
                }
                else
                {
                    for (int y = 0; y < h; y++)
                    {
                        long offsetSrc = (long)y * sstep;
                        long offsetDst = (long)y * dstep;

                        CopyMemory(pDst + offsetDst, pSrc + offsetSrc, w * ch);
                    }
                }
            }
            finally
            {
                dst.UnlockBits(bd);
            }
        }

        private unsafe void CopyMemory(void* outDest, void* inSrc, int inNumOfBytes)
        {
            Buffer.MemoryCopy(inSrc, outDest, inNumOfBytes, inNumOfBytes);
        }
    }
}
