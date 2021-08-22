using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ozeki
{
    public class CodecSpeex : ICodec
    {
        public CodecSpeex(int sampleRate)
        {
            if (sampleRate != 8000 && sampleRate != 16000 && sampleRate != 32000)
            {
                throw new ArgumentException("Sample rate must be 8000 or 16000 or 32000");
            }
            this.SampleRate = sampleRate;
            if (NativeAPI.CreateSpeexCodec(out this.instance, sampleRate) != 0)
            {
                throw new Exception("Speex codec initialization has failed.");
            }
            int sampleRate2 = this.SampleRate;
            if (sampleRate2 != 8000)
            {
                if (sampleRate2 != 16000)
                {
                    if (sampleRate2 == 32000)
                    {
                        this.Description = "Speex UltraWideband";
                        this.PayloadType = CodecPayloadType.Speex_Ultrawideband;
                    }
                }
                else
                {
                    this.Description = "Speex Wideband";
                    this.PayloadType = CodecPayloadType.Speex_Wideband;
                }
            }
            else
            {
                this.Description = "Speex Narrowband";
                this.PayloadType = CodecPayloadType.Speex_Narrowband;
            }
            this.encSync = new object();
            this.decSync = new object();
        }

        ~CodecSpeex()
        {
            this.Dispose(false);
        }

        public byte[] Encode(byte[] data)
        {
            byte[] result;
            lock (this.encSync)
            {
                short[] inData = data.ToShortArray();
                IntPtr source;
                int num = NativeAPI.SpeexEncode(this.instance, inData, out source);
                if (num != 0)
                {
                    byte[] array = new byte[num];
                    Marshal.Copy(source, array, 0, num);
                    result = array;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public byte[] Decode(byte[] data)
        {
            byte[] result;
            lock (this.decSync)
            {
                IntPtr source;
                int num = NativeAPI.SpeexDecode(this.instance, data, data.Length, out source);
                if (num != 0)
                {
                    short[] array = new short[num];
                    Marshal.Copy(source, array, 0, array.Length);
                    result = array.ToByteArray();
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public string Description { get; private set; }

        public int SampleRate { get; private set; }

        public CodecPayloadType PayloadType { get; private set; }

        public MediaType MediaType
        {
            get
            {
                return MediaType.Audio;
            }
        }

        public string EncodingName
        {
            get
            {
                return "SPEEX";
            }
        }

        public int Channels
        {
            get
            {
                return 1;
            }
        }

        public int PacketizationTime
        {
            get
            {
                return 20;
            }
        }

        public int Bitrate
        {
            get
            {
                return 0;
            }
        }

        public string GetFmtp()
        {
            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref this.disposed, 1) == 0)
            {
                if (disposing)
                {
                }
                NativeAPI.ReleaseSpeexCodec(this.instance);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IntPtr instance;

        private object encSync;

        private object decSync;

        private int disposed;
    }
}
