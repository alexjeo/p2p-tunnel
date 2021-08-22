using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ozeki
{
    internal class SpeexEchoFilter : IEchoFilter
    {
        public SpeexEchoFilter(int frameSize, int filterLength)
        {
            if (NativeAPI.CreateAEC(out _instance, frameSize, filterLength) != 0)
            {
                //throw new MediaException(20006, "AcousticEchoCanceller creation failed.", null);
            }
        }

        public byte[] Filter(byte[] localFrame, byte[] remoteFrame)
        {
            if (this._disposed == 1)
            {
                //throw new MediaException(20007, "AEC is disposed.", null);
            }
            short[] recFrame = localFrame.ToShortArray();
            short[] playFrame = remoteFrame.ToShortArray();
            return Filter(recFrame, playFrame).ToByteArray();
        }

        internal short[] Filter(short[] recFrame, short[] playFrame)
        {
            NativeAPI.CancelEcho(_instance, recFrame, playFrame, out _outputPtr);
            short[] array = new short[recFrame.Length];
            Marshal.Copy(_outputPtr, array, 0, array.Length);
            return array;
        }

        private void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                if (disposing)
                {
                }
                NativeAPI.ReleaseAEC(_instance);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SpeexEchoFilter()
        {
            this.Dispose(false);
        }

        private readonly IntPtr _instance;

        private int _disposed;

        private IntPtr _outputPtr;
    }
}
