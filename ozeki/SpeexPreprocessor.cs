using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ozeki
{
    public class SpeexPreprocessor
    {
        public bool AGC
        {
            get
            {
                return this._info.AGC;
            }
            set
            {
                if (this._info.AGC != value)
                {
                    int value2 = value ? 1 : 0;
                    NativeAPI.SetSPPParam(this._instance, 1, value2);
                    this._info.AGC = value;
                }
            }
        }

        public bool Denoise
        {
            get
            {
                return this._info.Denoise;
            }
            set
            {
                if (this._info.Denoise != value)
                {
                    int value2 = value ? 1 : 0;
                    NativeAPI.SetSPPParam(this._instance, 4, value2);
                    this._info.Denoise = value;
                }
            }
        }

        public int NoiseSuppress
        {
            get
            {
                return this._info.NoiseSuppress;
            }
            private set
            {
                if (this._info.NoiseSuppress != value)
                {
                    NativeAPI.SetSPPParam(this._instance, 5, value);
                    this._info.NoiseSuppress = value;
                }
            }
        }

        public int AGCMaxGain
        {
            get
            {
                return this._info.AGC_MaxGain;
            }
            set
            {
                if (this._info.AGC_MaxGain != value)
                {
                    NativeAPI.SetSPPParam(this._instance, 2, value);
                    this._info.AGC_MaxGain = value;
                }
            }
        }

        public int AGCSpeed
        {
            get
            {
                return this._info.AGC_Speed;
            }
            set
            {
                if (this._info.AGC_Speed != value)
                {
                    NativeAPI.SetSPPParam(this._instance, 3, value);
                    this._info.AGC_Speed = value;
                }
            }
        }

        // Token: 0x06000D45 RID: 3397 RVA: 0x00048CAC File Offset: 0x00046EAC
        public SpeexPreprocessor(int frameSize)
        {
            if (NativeAPI.CreateSPP(out this._instance, frameSize) != 0)
            {
                throw new Exception("Preprocessor creation failed.");
            }
            this._info = NativeAPI.GetSPPInfo(this._instance);
        }

        // Token: 0x06000D46 RID: 3398 RVA: 0x00048D00 File Offset: 0x00046F00
        public byte[] Filter(byte[] data)
        {
            if (this._disposed == 1)
            {
                throw new Exception("SPP is disposed.");
            }
            short[] frame = data.ToShortArray();
            return this.Filter(frame).ToByteArray();
        }

        internal short[] Filter(short[] frame)
        {
            NativeAPI.FilterSPP(this._instance, frame, out this._outputPtr);
            short[] array = new short[frame.Length];
            Marshal.Copy(this._outputPtr, array, 0, array.Length);
            return array;
        }

        public NoiseReductionLevel GetNoiseReductionLevel()
        {
            NoiseReductionLevel result;
            if (!this._info.Denoise)
            {
                result = NoiseReductionLevel.NoReduction;
            }
            else if (this._info.NoiseSuppress <= -70)
            {
                result = NoiseReductionLevel.High;
            }
            else if (this._info.NoiseSuppress >= -15)
            {
                result = NoiseReductionLevel.Low;
            }
            else
            {
                result = NoiseReductionLevel.Medium;
            }
            return result;
        }

        // Token: 0x06000D49 RID: 3401 RVA: 0x00048E08 File Offset: 0x00047008
        public void SetNoiseReductionLevel(NoiseReductionLevel level)
        {
            int noiseSuppress = 0;
            if (level == NoiseReductionLevel.NoReduction)
            {
                this.Denoise = false;
            }
            else
            {
                this.Denoise = true;
                switch (level)
                {
                    case NoiseReductionLevel.Low:
                        noiseSuppress = -15;
                        break;
                    case NoiseReductionLevel.Medium:
                        noiseSuppress = -40;
                        break;
                    case NoiseReductionLevel.High:
                        noiseSuppress = -70;
                        break;
                }
                this.NoiseSuppress = noiseSuppress;
            }
        }

        // Token: 0x06000D4A RID: 3402 RVA: 0x00048E7C File Offset: 0x0004707C
        private void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref this._disposed, 1) == 0)
            {
                if (disposing)
                {
                }
                NativeAPI.ReleaseSPP(this._instance);
            }
        }

        // Token: 0x06000D4B RID: 3403 RVA: 0x00048EC4 File Offset: 0x000470C4
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Token: 0x06000D4C RID: 3404 RVA: 0x00048ED8 File Offset: 0x000470D8
        ~SpeexPreprocessor()
        {
            this.Dispose(false);
        }

        // Token: 0x04000657 RID: 1623
        private readonly IntPtr _instance;

        // Token: 0x04000658 RID: 1624
        private int _disposed;

        // Token: 0x04000659 RID: 1625
        private IntPtr _outputPtr;

        // Token: 0x0400065A RID: 1626
        private SpeexPPInfo _info;
    }
}
