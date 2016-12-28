using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace DotNetDash.CameraViews
{
    class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [SecurityCritical]
        public SafeHBitmapHandle(IntPtr preexistingHandle, bool ownsHandle)
            : base(ownsHandle)
        {
            SetHandle(preexistingHandle);
        }

        protected override bool ReleaseHandle()
        {
            return DeleteObject(handle);
        }
    }
}
