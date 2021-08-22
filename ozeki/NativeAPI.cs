using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ozeki
{
    [DoNotObfuscateType]
    public static class NativeAPI
    {
        // Token: 0x060012FE RID: 4862
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        // Token: 0x060012FF RID: 4863
        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        // Token: 0x06001300 RID: 4864 RVA: 0x00063568 File Offset: 0x00061768
        public static IntPtr CreateH264Encoder()
        {
            if (NativeAPI.createH264Encoder == null)
            {
                NativeAPI.createH264Encoder = NativeAPI.GetDelegate<NativeAPI.CreateH264EncoderDelegate>("CreateH264Encoder");
            }
            return NativeAPI.createH264Encoder();
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x000635AC File Offset: 0x000617AC
        public static IntPtr CreateH264Decoder()
        {
            if (NativeAPI.createH264Decoder == null)
            {
                NativeAPI.createH264Decoder = NativeAPI.GetDelegate<NativeAPI.CreateH264DecoderDelegate>("CreateH264Decoder");
            }
            return NativeAPI.createH264Decoder();
        }

        // Token: 0x06001302 RID: 4866 RVA: 0x000635F0 File Offset: 0x000617F0
        //public static StatusCode H264EncoderInit(IntPtr instance, VideoStreamInfo videoInfo, int bitrate, int profileIdc, int levelIdc)
        //{
        //    if (NativeAPI.h264EncoderInit == null)
        //    {
        //        NativeAPI.h264EncoderInit = NativeAPI.GetDelegate<NativeAPI.H264EncoderInitDelegate>("H264EncoderInit");
        //    }
        //    return NativeAPI.h264EncoderInit(instance, ref videoInfo, bitrate, profileIdc, levelIdc);
        //}

        // Token: 0x06001303 RID: 4867 RVA: 0x0006363C File Offset: 0x0006183C
        //public static StatusCode H264DecoderInit(IntPtr instance, byte[] initData)
        //{
        //    if (NativeAPI.h264DecoderInit == null)
        //    {
        //        NativeAPI.h264DecoderInit = NativeAPI.GetDelegate<NativeAPI.H264DecoderInitDelegate>("H264DecoderInit");
        //    }
        //    return NativeAPI.h264DecoderInit(instance, initData, initData.Length);
        //}

        //// Token: 0x06001304 RID: 4868 RVA: 0x00063688 File Offset: 0x00061888
        //public static StatusCode H264Encode(IntPtr instance, byte[] inData, int inDataSize, out IntPtr outData, out int outDataSize)
        //{
        //    if (NativeAPI.h264Encode == null)
        //    {
        //        NativeAPI.h264Encode = NativeAPI.GetDelegate<NativeAPI.H264EncodeDelegate>("H264Encode");
        //    }
        //    return NativeAPI.h264Encode(instance, inData, inDataSize, out outData, out outDataSize);
        //}

        //// Token: 0x06001305 RID: 4869 RVA: 0x000636D4 File Offset: 0x000618D4
        //public static StatusCode H264Decode(IntPtr instance, byte[] inData, int inDataSize, out VideoPacket mediaPacket)
        //{
        //    if (NativeAPI.h264Decode == null)
        //    {
        //        NativeAPI.h264Decode = NativeAPI.GetDelegate<NativeAPI.H264DecodeDelegate>("H264Decode");
        //    }
        //    return NativeAPI.h264Decode(instance, inData, inDataSize, out mediaPacket);
        //}

        // Token: 0x06001306 RID: 4870 RVA: 0x0006371C File Offset: 0x0006191C
        public static void ReleaseBuffer(IntPtr buffer)
        {
            if (NativeAPI.releaseBuffer == null)
            {
                NativeAPI.releaseBuffer = NativeAPI.GetDelegate<NativeAPI.ReleaseBufferDelegate>("ReleaseBuffer");
            }
            NativeAPI.releaseBuffer(buffer);
        }

        // Token: 0x06001307 RID: 4871 RVA: 0x0006375C File Offset: 0x0006195C
        public static void ReleaseH264Encoder(IntPtr instance)
        {
            if (NativeAPI.releaseH264Encoder == null)
            {
                NativeAPI.releaseH264Encoder = NativeAPI.GetDelegate<NativeAPI.ReleaseH264EncoderDelegate>("ReleaseH264Encoder");
            }
            NativeAPI.releaseH264Encoder(instance);
        }

        // Token: 0x06001308 RID: 4872 RVA: 0x0006379C File Offset: 0x0006199C
        public static void ReleaseH264Decoder(IntPtr instance)
        {
            if (NativeAPI.releaseH264Decoder == null)
            {
                NativeAPI.releaseH264Decoder = NativeAPI.GetDelegate<NativeAPI.ReleaseH264DecoderDelegate>("ReleaseH264Decoder");
            }
            NativeAPI.releaseH264Decoder(instance);
        }

        // Token: 0x06001309 RID: 4873 RVA: 0x000637DC File Offset: 0x000619DC
        public static IntPtr CreateH263Encoder()
        {
            if (NativeAPI.createH263Encoder == null)
            {
                NativeAPI.createH263Encoder = NativeAPI.GetDelegate<NativeAPI.CreateH263EncoderDelegate>("CreateH263Encoder");
            }
            return NativeAPI.createH263Encoder();
        }

        // Token: 0x0600130A RID: 4874 RVA: 0x00063820 File Offset: 0x00061A20
        public static IntPtr CreateH263Decoder()
        {
            if (NativeAPI.createH263Decoder == null)
            {
                NativeAPI.createH263Decoder = NativeAPI.GetDelegate<NativeAPI.CreateH263DecoderDelegate>("CreateH263Decoder");
            }
            return NativeAPI.createH263Decoder();
        }

        // Token: 0x0600130B RID: 4875 RVA: 0x00063864 File Offset: 0x00061A64
        //public static StatusCode H263EncoderInit(IntPtr instance, int imgHeight, int imgWidth, double frameRate, double bitsPerPixel)
        //{
        //    if (NativeAPI.h263EncoderInit == null)
        //    {
        //        NativeAPI.h263EncoderInit = NativeAPI.GetDelegate<NativeAPI.H263EncoderInitDelegate>("H263EncoderInit");
        //    }
        //    return NativeAPI.h263EncoderInit(instance, imgHeight, imgWidth, frameRate, bitsPerPixel);
        //}

        //// Token: 0x0600130C RID: 4876 RVA: 0x000638B0 File Offset: 0x00061AB0
        //public static StatusCode H263DecoderInit(IntPtr instance, byte[] initData)
        //{
        //    if (NativeAPI.h263DecoderInit == null)
        //    {
        //        NativeAPI.h263DecoderInit = NativeAPI.GetDelegate<NativeAPI.H263DecoderInitDelegate>("H263DecoderInit");
        //    }
        //    return NativeAPI.h263DecoderInit(instance, initData, initData.Length);
        //}

        // Token: 0x0600130D RID: 4877 RVA: 0x000638FC File Offset: 0x00061AFC
        //public static StatusCode H263Encode(IntPtr instance, byte[] inData, int inDataSize, out IntPtr outData, out int outDataSize)
        //{
        //    if (NativeAPI.h263Encode == null)
        //    {
        //        NativeAPI.h263Encode = NativeAPI.GetDelegate<NativeAPI.H263EncodeDelegate>("H263Encode");
        //    }
        //    return NativeAPI.h263Encode(instance, inData, inDataSize, out outData, out outDataSize);
        //}

        //// Token: 0x0600130E RID: 4878 RVA: 0x00063948 File Offset: 0x00061B48
        //public static StatusCode H263Decode(IntPtr instance, byte[] inData, int inDataSize, out VideoPacket videoPacket)
        //{
        //    if (NativeAPI.h263Decode == null)
        //    {
        //        NativeAPI.h263Decode = NativeAPI.GetDelegate<NativeAPI.H263DecodeDelegate>("H263Decode");
        //    }
        //    return NativeAPI.h263Decode(instance, inData, inDataSize, out videoPacket);
        //}

        // Token: 0x0600130F RID: 4879 RVA: 0x00063990 File Offset: 0x00061B90
        public static void ReleaseH263Encoder(IntPtr instance)
        {
            if (NativeAPI.releaseH263Encoder == null)
            {
                NativeAPI.releaseH263Encoder = NativeAPI.GetDelegate<NativeAPI.ReleaseH263EncoderDelegate>("ReleaseH263Encoder");
            }
            NativeAPI.releaseH263Encoder(instance);
        }

        // Token: 0x06001310 RID: 4880 RVA: 0x000639D0 File Offset: 0x00061BD0
        public static void ReleaseH263Decoder(IntPtr instance)
        {
            if (NativeAPI.releaseH263Decoder == null)
            {
                NativeAPI.releaseH263Decoder = NativeAPI.GetDelegate<NativeAPI.ReleaseH263DecoderDelegate>("ReleaseH263Decoder");
            }
            NativeAPI.releaseH263Decoder(instance);
        }

        // Token: 0x06001311 RID: 4881 RVA: 0x00063A10 File Offset: 0x00061C10
        //public static StatusCode CreateReader(string path, out IntPtr instance)
        //{
        //    if (NativeAPI.createReader == null)
        //    {
        //        NativeAPI.createReader = NativeAPI.GetDelegate<NativeAPI.CreateReaderDelegate>("CreateReader");
        //    }
        //    return NativeAPI.createReader(path, out instance);
        //}

        //// Token: 0x06001312 RID: 4882 RVA: 0x00063A58 File Offset: 0x00061C58
        //public static StatusCode ReleaseReader(IntPtr instance)
        //{
        //    if (NativeAPI.releaseReader == null)
        //    {
        //        NativeAPI.releaseReader = NativeAPI.GetDelegate<NativeAPI.ReleaseReaderDelegate>("ReleaseReader");
        //    }
        //    return NativeAPI.releaseReader(instance);
        //}

        // Token: 0x06001313 RID: 4883 RVA: 0x00063AA0 File Offset: 0x00061CA0
        //public static StatusCode GetNextVideoData(IntPtr instance, out VideoPacket videoPacket)
        //{
        //    if (NativeAPI.getNextVideoData == null)
        //    {
        //        NativeAPI.getNextVideoData = NativeAPI.GetDelegate<NativeAPI.GetNextVideoDataDelegate>("GetNextVideoData");
        //    }
        //    return NativeAPI.getNextVideoData(instance, out videoPacket);
        //}

        //// Token: 0x06001314 RID: 4884 RVA: 0x00063AE8 File Offset: 0x00061CE8
        //public static StatusCode GetNextAudioData(IntPtr instance, out AudioPacket audioPacket)
        //{
        //    if (NativeAPI.getNextAudioData == null)
        //    {
        //        NativeAPI.getNextAudioData = NativeAPI.GetDelegate<NativeAPI.GetNextAudioDataDelegate>("GetNextAudioData");
        //    }
        //    return NativeAPI.getNextAudioData(instance, out audioPacket);
        //}

        // Token: 0x06001315 RID: 4885 RVA: 0x00063B30 File Offset: 0x00061D30
        //public static StatusCode GetVideoInfo(IntPtr instance, out VideoStreamInfo videoStreamInfo)
        //{
        //    if (NativeAPI.getVideoInfo == null)
        //    {
        //        NativeAPI.getVideoInfo = NativeAPI.GetDelegate<NativeAPI.GetVideoInfoDelegate>("GetVideoInfo");
        //    }
        //    return NativeAPI.getVideoInfo(instance, out videoStreamInfo);
        //}

        // Token: 0x06001316 RID: 4886 RVA: 0x00063B78 File Offset: 0x00061D78
        //public static StatusCode GetAudioInfo(IntPtr instance, out AudioStreamInfo audioStreamInfo)
        //{
        //    if (NativeAPI.getAudioInfo == null)
        //    {
        //        NativeAPI.getAudioInfo = NativeAPI.GetDelegate<NativeAPI.GetAudioInfoDelegate>("GetAudioInfo");
        //    }
        //    return NativeAPI.getAudioInfo(instance, out audioStreamInfo);
        //}

        // Token: 0x06001317 RID: 4887 RVA: 0x00063BC0 File Offset: 0x00061DC0
        public static int CreateVADDetector(out IntPtr instance)
        {
            if (NativeAPI.createVADDetector == null)
            {
                NativeAPI.createVADDetector = NativeAPI.GetDelegate<NativeAPI.CreateVADDetectorDelegate>("CreateVADDetector");
            }
            return NativeAPI.createVADDetector(out instance);
        }

        // Token: 0x06001318 RID: 4888 RVA: 0x00063C08 File Offset: 0x00061E08
        //public static int DetectVoice(IntPtr instance, byte[] data, out VADStatus vadStatus)
        //{
        //    if (NativeAPI.detectVoice == null)
        //    {
        //        NativeAPI.detectVoice = NativeAPI.GetDelegate<NativeAPI.DetectVoiceDelegate>("DetectVoice");
        //    }
        //    return NativeAPI.detectVoice(instance, data, data.Length, out vadStatus);
        //}

        // Token: 0x06001319 RID: 4889 RVA: 0x00063C54 File Offset: 0x00061E54
        public static void ReleaseVADDetector(IntPtr instance)
        {
            if (NativeAPI.releaseVADDetector == null)
            {
                NativeAPI.releaseVADDetector = NativeAPI.GetDelegate<NativeAPI.ReleaseVADDetectorDelegate>("ReleaseVADDetector");
            }
            NativeAPI.releaseVADDetector(instance);
        }

        // Token: 0x0600131A RID: 4890 RVA: 0x00063C94 File Offset: 0x00061E94
        public static int CreateG728(out IntPtr instance)
        {
            if (NativeAPI.createG728 == null)
            {
                NativeAPI.createG728 = NativeAPI.GetDelegate<NativeAPI.CreateSpeechCodecDelegate>("CreateG728");
            }
            return NativeAPI.createG728(out instance);
        }

        // Token: 0x0600131B RID: 4891 RVA: 0x00063CDC File Offset: 0x00061EDC
        public static int CreateG722(out IntPtr instance, int bitrate)
        {
            if (NativeAPI.createG722 == null)
            {
                NativeAPI.createG722 = NativeAPI.GetDelegate<NativeAPI.CreateG726CodecDelegate>("CreateG722");
            }
            return NativeAPI.createG722(out instance, bitrate);
        }

        // Token: 0x0600131C RID: 4892 RVA: 0x00063D24 File Offset: 0x00061F24
        public static int CreateG723(out IntPtr instance, int bitrate)
        {
            if (NativeAPI.createG723 == null)
            {
                NativeAPI.createG723 = NativeAPI.GetDelegate<NativeAPI.CreateG726CodecDelegate>("CreateG723");
            }
            return NativeAPI.createG723(out instance, bitrate);
        }

        // Token: 0x0600131D RID: 4893 RVA: 0x00063D6C File Offset: 0x00061F6C
        public static int CreateG726(out IntPtr instance, int bitrate)
        {
            if (NativeAPI.createG726 == null)
            {
                NativeAPI.createG726 = NativeAPI.GetDelegate<NativeAPI.CreateG726CodecDelegate>("CreateG726");
            }
            return NativeAPI.createG726(out instance, bitrate);
        }

        // Token: 0x0600131E RID: 4894 RVA: 0x00063DB4 File Offset: 0x00061FB4
        public static int CreateG729(out IntPtr instance)
        {
            if (NativeAPI.createG729 == null)
            {
                NativeAPI.createG729 = NativeAPI.GetDelegate<NativeAPI.CreateSpeechCodecDelegate>("CreateG729");
            }
            return NativeAPI.createG729(out instance);
        }

        // Token: 0x0600131F RID: 4895 RVA: 0x00063DFC File Offset: 0x00061FFC
        public static int CreateGSM(out IntPtr instance)
        {
            if (NativeAPI.createGSM == null)
            {
                NativeAPI.createGSM = NativeAPI.GetDelegate<NativeAPI.CreateSpeechCodecDelegate>("CreateGSM");
            }
            return NativeAPI.createGSM(out instance);
        }

        // Token: 0x06001320 RID: 4896 RVA: 0x00063E44 File Offset: 0x00062044
        public static int SpeechCodecEncode(IntPtr instance, byte[] inData, int inDataLength, out IntPtr outData, out int outDataLength)
        {
            if (NativeAPI.speechCodecEncode == null)
            {
                NativeAPI.speechCodecEncode = NativeAPI.GetDelegate<NativeAPI.SpeechCodecEncodeDelegate>("SpeechCodecEncode");
            }
            return NativeAPI.speechCodecEncode(instance, inData, inDataLength, out outData, out outDataLength);
        }

        // Token: 0x06001321 RID: 4897 RVA: 0x00063E90 File Offset: 0x00062090
        public static int SpeechCodecDecode(IntPtr instance, byte[] inData, int inDataLength, out IntPtr outData, out int outDataLength)
        {
            if (NativeAPI.speechCodecDecode == null)
            {
                NativeAPI.speechCodecDecode = NativeAPI.GetDelegate<NativeAPI.SpeechCodecDecodeDelegate>("SpeechCodecDecode");
            }
            return NativeAPI.speechCodecDecode(instance, inData, inDataLength, out outData, out outDataLength);
        }

        // Token: 0x06001322 RID: 4898 RVA: 0x00063EDC File Offset: 0x000620DC
        public static void ReleaseSpeechCodec(IntPtr instance)
        {
            if (NativeAPI.releaseSpeechCodec == null)
            {
                NativeAPI.releaseSpeechCodec = NativeAPI.GetDelegate<NativeAPI.ReleaseSpeechCodecDelegate>("ReleaseSpeechCodec");
            }
            NativeAPI.releaseSpeechCodec(instance);
        }

        // Token: 0x06001323 RID: 4899 RVA: 0x00063F1C File Offset: 0x0006211C
        public static int CreateIPPAEC(out IntPtr instance)
        {
            if (NativeAPI.createIPPAEC == null)
            {
                NativeAPI.createIPPAEC = NativeAPI.GetDelegate<NativeAPI.CreateIPPAECDelegate>("CreateIPPAEC");
            }
            return NativeAPI.createIPPAEC(out instance);
        }

        // Token: 0x06001324 RID: 4900 RVA: 0x00063F64 File Offset: 0x00062164
        public static void ReleaseIPPAEC(IntPtr instance)
        {
            if (NativeAPI.releaseIPPAEC == null)
            {
                NativeAPI.releaseIPPAEC = NativeAPI.GetDelegate<NativeAPI.ReleaseIPPAECDelegate>("ReleaseIPPAEC");
            }
            NativeAPI.releaseIPPAEC(instance);
        }

        // Token: 0x06001325 RID: 4901 RVA: 0x00063FA4 File Offset: 0x000621A4
        public static int CancelEcho(IntPtr instance, byte[] sendIn, byte[] recvIn, out IntPtr sendOut)
        {
            if (NativeAPI.cancelEchoIPP == null)
            {
                NativeAPI.cancelEchoIPP = NativeAPI.GetDelegate<NativeAPI.CancelEchoIPPDelegate>("CancelEchoIPP");
            }
            return NativeAPI.cancelEchoIPP(instance, sendIn, recvIn, out sendOut);
        }

        // Token: 0x06001326 RID: 4902 RVA: 0x00063FEC File Offset: 0x000621EC
        //public static int SetIPPAECParam(IntPtr instance, NativeAECParams param, int value)
        //{
        //    if (NativeAPI.setIPPAECParam == null)
        //    {
        //        NativeAPI.setIPPAECParam = NativeAPI.GetDelegate<NativeAPI.SetIPPAECParamDelegate>("SetIPPAECParam");
        //    }
        //    return NativeAPI.setIPPAECParam(instance, param, value);
        //}

        //// Token: 0x06001327 RID: 4903 RVA: 0x00064034 File Offset: 0x00062234
        //public static int GetIPPAECInfo(IntPtr instance, out USC_EC_Info info)
        //{
        //    if (NativeAPI.getIPPAECInfo == null)
        //    {
        //        NativeAPI.getIPPAECInfo = NativeAPI.GetDelegate<NativeAPI.GetIPPAECInfoDelegate>("GetIPPAECInfo");
        //    }
        //    return NativeAPI.getIPPAECInfo(instance, out info);
        //}

        // Token: 0x06001328 RID: 4904 RVA: 0x0006407C File Offset: 0x0006227C
        public static int CreateAEC(out IntPtr instance, int frameSize, int filterLength)
        {
            if (NativeAPI.createAEC == null)
            {
                NativeAPI.createAEC = NativeAPI.GetDelegate<NativeAPI.CreateAECDelegate>("CreateAEC");
            }
            return NativeAPI.createAEC(out instance, frameSize, filterLength);
        }

        // Token: 0x06001329 RID: 4905 RVA: 0x000640C4 File Offset: 0x000622C4
        public static void ReleaseAEC(IntPtr instance)
        {
            if (NativeAPI.releaseAEC == null)
            {
                NativeAPI.releaseAEC = NativeAPI.GetDelegate<NativeAPI.ReleaseAECDelegate>("ReleaseAEC");
            }
            NativeAPI.releaseAEC(instance);
        }

        // Token: 0x0600132A RID: 4906 RVA: 0x00064104 File Offset: 0x00062304
        public static void ResetAEC(IntPtr instance)
        {
            if (NativeAPI.resetAEC == null)
            {
                NativeAPI.resetAEC = NativeAPI.GetDelegate<NativeAPI.ResetAECDelegate>("ResetAEC");
            }
            NativeAPI.resetAEC(instance);
        }

        // Token: 0x0600132B RID: 4907 RVA: 0x00064144 File Offset: 0x00062344
        public static void CancelEcho(IntPtr instance, short[] sendIn, short[] recvIn, out IntPtr sendOut)
        {
            if (NativeAPI.cancelEcho == null)
            {
                NativeAPI.cancelEcho = NativeAPI.GetDelegate<NativeAPI.CancelEchoDelegate>("CancelEcho");
            }
            NativeAPI.cancelEcho(instance, sendIn, recvIn, out sendOut);
        }

        // Token: 0x0600132C RID: 4908 RVA: 0x00064188 File Offset: 0x00062388
        public static int CreateSPP(out IntPtr instance, int frameSize)
        {
            if (NativeAPI.createSPP == null)
            {
                NativeAPI.createSPP = NativeAPI.GetDelegate<NativeAPI.CreateSPPDelegate>("CreateSPP");
            }
            return NativeAPI.createSPP(out instance, frameSize);
        }

        // Token: 0x0600132D RID: 4909 RVA: 0x000641D0 File Offset: 0x000623D0
        public static void ReleaseSPP(IntPtr instance)
        {
            if (NativeAPI.releaseSPP == null)
            {
                NativeAPI.releaseSPP = NativeAPI.GetDelegate<NativeAPI.ReleaseSPPDelegate>("ReleaseSPP");
            }
            NativeAPI.releaseSPP(instance);
        }

        // Token: 0x0600132E RID: 4910 RVA: 0x00064210 File Offset: 0x00062410
        public static void FilterSPP(IntPtr instance, short[] frame, out IntPtr outFrame)
        {
            if (NativeAPI.filterSPP == null)
            {
                NativeAPI.filterSPP = NativeAPI.GetDelegate<NativeAPI.FilterSPPDelegate>("FilterSPP");
            }
            try
            {
                //outFrame = IntPtr.Zero;
               NativeAPI.filterSPP(instance, frame, out outFrame);
            }
            catch (Exception ex)
            {
                outFrame = IntPtr.Zero;
                Debug.WriteLine(ex.Message);
            }
        }

        // Token: 0x0600132F RID: 4911 RVA: 0x00064254 File Offset: 0x00062454
        public static void SetSPPParam(IntPtr instance, int param, int value)
        {
            if (NativeAPI.setSPPParam == null)
            {
                NativeAPI.setSPPParam = NativeAPI.GetDelegate<NativeAPI.SetSPPParamDelegate>("SetSPPParam");
            }
            NativeAPI.setSPPParam(instance, param, value);
        }

        public static SpeexPPInfo GetSPPInfo(IntPtr instance)
        {
            if (NativeAPI.getSPPInfo == null)
            {
                NativeAPI.getSPPInfo = NativeAPI.GetDelegate<NativeAPI.GetSPPInfoDelegate>("GetSPPInfo");
            }
            SpeexPPInfo result;
            NativeAPI.getSPPInfo(instance, out result);
            return result;
        }

        // Token: 0x06001331 RID: 4913 RVA: 0x000642E4 File Offset: 0x000624E4
        public static int CreateSpeexCodec(out IntPtr instance, int sampleRate)
        {
            if (NativeAPI.createSpeexCodec == null)
            {
                NativeAPI.createSpeexCodec = NativeAPI.GetDelegate<NativeAPI.CreateSpeexCodecDelegate>("CreateSpeexCodec");
            }
            return NativeAPI.createSpeexCodec(out instance, sampleRate);
        }

        // Token: 0x06001332 RID: 4914 RVA: 0x0006432C File Offset: 0x0006252C
        public static void ReleaseSpeexCodec(IntPtr instance)
        {
            if (NativeAPI.releaseSpeexCodec == null)
            {
                NativeAPI.releaseSpeexCodec = NativeAPI.GetDelegate<NativeAPI.ReleaseSpeexCodecDelegate>("ReleaseSpeexCodec");
            }
            NativeAPI.releaseSpeexCodec(instance);
        }

        // Token: 0x06001333 RID: 4915 RVA: 0x0006436C File Offset: 0x0006256C
        public static int SpeexEncode(IntPtr instance, short[] inData, out IntPtr outData)
        {
            if (NativeAPI.speexEncode == null)
            {
                NativeAPI.speexEncode = NativeAPI.GetDelegate<NativeAPI.SpeexEncodeDelegate>("SpeexEncode");
            }
            return NativeAPI.speexEncode(instance, inData, out outData);
        }

        // Token: 0x06001334 RID: 4916 RVA: 0x000643B4 File Offset: 0x000625B4
        public static int SpeexDecode(IntPtr instance, byte[] inData, int inDataLength, out IntPtr outData)
        {
            if (NativeAPI.speexDecode == null)
            {
                NativeAPI.speexDecode = NativeAPI.GetDelegate<NativeAPI.SpeexDecodeDelegate>("SpeexDecode");
            }
            return NativeAPI.speexDecode(instance, inData, inDataLength, out outData);
        }

        // Token: 0x06001335 RID: 4917 RVA: 0x000643FC File Offset: 0x000625FC
        public static int CreateiLBCCodec(out IntPtr instance, int sampleRate)
        {
            if (NativeAPI.createiLBCCodec == null)
            {
                NativeAPI.createiLBCCodec = NativeAPI.GetDelegate<NativeAPI.CreateiLBCCodecDelegate>("CreateiLBCCodec");
            }
            return NativeAPI.createiLBCCodec(out instance, sampleRate);
        }

        // Token: 0x06001336 RID: 4918 RVA: 0x00064444 File Offset: 0x00062644
        public static void ReleaseiLBCCodec(IntPtr instance)
        {
            if (NativeAPI.releaseiLBCCodec == null)
            {
                NativeAPI.releaseiLBCCodec = NativeAPI.GetDelegate<NativeAPI.ReleaseiLBCCodecDelegate>("ReleaseiLBCCodec");
            }
            NativeAPI.releaseiLBCCodec(instance);
        }

        // Token: 0x06001337 RID: 4919 RVA: 0x00064484 File Offset: 0x00062684
        public static int ILBCEncode(IntPtr instance, short[] inData, out IntPtr outData)
        {
            if (NativeAPI.iLBCEncode == null)
            {
                NativeAPI.iLBCEncode = NativeAPI.GetDelegate<NativeAPI.iLBCEncodeDelegate>("iLBCEncode");
            }
            return NativeAPI.iLBCEncode(instance, inData, out outData);
        }

        // Token: 0x06001338 RID: 4920 RVA: 0x000644CC File Offset: 0x000626CC
        public static int ILBCDecode(IntPtr instance, byte[] inData, int inDataLength, out IntPtr outData)
        {
            if (NativeAPI.iLBCDecode == null)
            {
                NativeAPI.iLBCDecode = NativeAPI.GetDelegate<NativeAPI.iLBCDecodeDelegate>("iLBCDecode");
            }
            return NativeAPI.iLBCDecode(instance, inData, inDataLength, out outData);
        }

        // Token: 0x06001339 RID: 4921 RVA: 0x00064514 File Offset: 0x00062714
        public static int CreateImgResizer(out IntPtr instance, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            if (NativeAPI.createImgResizer == null)
            {
                NativeAPI.createImgResizer = NativeAPI.GetDelegate<NativeAPI.CreateImgResizerDelegate>("CreateImgResizer");
            }
            return NativeAPI.createImgResizer(out instance, srcWidth, srcHeight, dstWidth, dstHeight);
        }

        // Token: 0x0600133A RID: 4922 RVA: 0x00064560 File Offset: 0x00062760
        public static int Resize(IntPtr instance, byte[] src, out IntPtr dst, out int dstSize)
        {
            if (NativeAPI.resize == null)
            {
                NativeAPI.resize = NativeAPI.GetDelegate<NativeAPI.ResizeDelegate>("Resize");
            }
            return NativeAPI.resize(instance, src, out dst, out dstSize);
        }

        // Token: 0x0600133B RID: 4923 RVA: 0x000645A8 File Offset: 0x000627A8
        public static void ReleaseImgResizer(IntPtr instance)
        {
            if (NativeAPI.releaseImgResizer == null)
            {
                NativeAPI.releaseImgResizer = NativeAPI.GetDelegate<NativeAPI.ReleaseImgResizerDelegate>("ReleaseImgResizer");
            }
        }

        // Token: 0x0600133C RID: 4924 RVA: 0x000645DC File Offset: 0x000627DC
        private static T GetDelegate<T>(string funcName) where T : class
        {
            NativeAPI.Init();
            IntPtr procAddress = NativeAPI.GetProcAddress(NativeAPI.hModule, funcName);
            if (procAddress == IntPtr.Zero)
            {
                NativeAPI.DeleteResources();
                NativeAPI.Init();
                procAddress = NativeAPI.GetProcAddress(NativeAPI.hModule, funcName);
                if (procAddress == IntPtr.Zero)
                {
                    // throw new VoIPException(10007, string.Format("Could not load native function. Function name: {0}", funcName), null);
                }
            }
            return Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T)) as T;
        }

        // Token: 0x0600133D RID: 4925 RVA: 0x00064680 File Offset: 0x00062880
        private static void Init()
        {
            if (!(NativeAPI.hModule != IntPtr.Zero))
            {
                lock (NativeAPI.sync)
                {
                    if (!(NativeAPI.hModule != IntPtr.Zero))
                    {
                        if (IntPtr.Size == 4)
                        {
                            NativeAPI.LoadLibrary("vcomp100.dll");
                            NativeAPI.hModule = NativeAPI.LoadLibrary("NativeLib.dll");
                        }
                        else
                        {
                            NativeAPI.LoadLibrary("vcomp100.dll");
                            NativeAPI.hModule = NativeAPI.LoadLibrary("NativeLib(x64).dll");
                        }
                    }
                }
            }
        }

        // Token: 0x0600133E RID: 4926 RVA: 0x00064780 File Offset: 0x00062980
        private static void DeleteResources()
        {
            lock (NativeAPI.sync)
            {

            }
        }

        // Token: 0x04000D6D RID: 3437
        private static NativeAPI.CreateH264EncoderDelegate createH264Encoder;

        // Token: 0x04000D6E RID: 3438
        private static NativeAPI.CreateH264DecoderDelegate createH264Decoder;

        // Token: 0x04000D6F RID: 3439
        //  private static NativeAPI.H264EncoderInitDelegate h264EncoderInit;

        // Token: 0x04000D70 RID: 3440
        //  private static NativeAPI.H264DecoderInitDelegate h264DecoderInit;

        // Token: 0x04000D71 RID: 3441
        // private static NativeAPI.H264EncodeDelegate h264Encode;

        // Token: 0x04000D72 RID: 3442
        // private static NativeAPI.H264DecodeDelegate h264Decode;

        // Token: 0x04000D73 RID: 3443
        private static NativeAPI.ReleaseBufferDelegate releaseBuffer;

        // Token: 0x04000D74 RID: 3444
        private static NativeAPI.ReleaseH264EncoderDelegate releaseH264Encoder;

        // Token: 0x04000D75 RID: 3445
        private static NativeAPI.ReleaseH264DecoderDelegate releaseH264Decoder;

        // Token: 0x04000D76 RID: 3446
        private static NativeAPI.CreateH263EncoderDelegate createH263Encoder;

        // Token: 0x04000D77 RID: 3447
        private static NativeAPI.CreateH263DecoderDelegate createH263Decoder;

        // Token: 0x04000D78 RID: 3448
        // private static NativeAPI.H263EncoderInitDelegate h263EncoderInit;

        // Token: 0x04000D79 RID: 3449
        // private static NativeAPI.H263DecoderInitDelegate h263DecoderInit;

        // Token: 0x04000D7A RID: 3450
        // private static NativeAPI.H263EncodeDelegate h263Encode;

        // Token: 0x04000D7B RID: 3451
        //private static NativeAPI.H263DecodeDelegate h263Decode;

        // Token: 0x04000D7C RID: 3452
        private static NativeAPI.ReleaseH263EncoderDelegate releaseH263Encoder;

        // Token: 0x04000D7D RID: 3453
        private static NativeAPI.ReleaseH263DecoderDelegate releaseH263Decoder;

        // Token: 0x04000D7E RID: 3454
        //private static NativeAPI.CreateReaderDelegate createReader;

        // Token: 0x04000D7F RID: 3455
        // private static NativeAPI.ReleaseReaderDelegate releaseReader;

        // Token: 0x04000D80 RID: 3456
        // private static NativeAPI.GetNextVideoDataDelegate getNextVideoData;

        // Token: 0x04000D81 RID: 3457
        // private static NativeAPI.GetNextAudioDataDelegate getNextAudioData;

        // Token: 0x04000D82 RID: 3458
        // private static NativeAPI.GetVideoInfoDelegate getVideoInfo;

        // Token: 0x04000D83 RID: 3459
        // private static NativeAPI.GetAudioInfoDelegate getAudioInfo;

        // Token: 0x04000D84 RID: 3460
        private static NativeAPI.CreateVADDetectorDelegate createVADDetector;

        // Token: 0x04000D85 RID: 3461
        //private static NativeAPI.DetectVoiceDelegate detectVoice;

        // Token: 0x04000D86 RID: 3462
        private static NativeAPI.ReleaseVADDetectorDelegate releaseVADDetector;

        // Token: 0x04000D87 RID: 3463
        private static NativeAPI.CreateSpeechCodecDelegate createG728;

        // Token: 0x04000D88 RID: 3464
        private static NativeAPI.CreateG726CodecDelegate createG722;

        // Token: 0x04000D89 RID: 3465
        private static NativeAPI.CreateG726CodecDelegate createG723;

        // Token: 0x04000D8A RID: 3466
        private static NativeAPI.CreateG726CodecDelegate createG726;

        // Token: 0x04000D8B RID: 3467
        private static NativeAPI.CreateSpeechCodecDelegate createG729;

        // Token: 0x04000D8C RID: 3468
        private static NativeAPI.CreateSpeechCodecDelegate createGSM;

        // Token: 0x04000D8D RID: 3469
        private static NativeAPI.SpeechCodecEncodeDelegate speechCodecEncode;

        // Token: 0x04000D8E RID: 3470
        private static NativeAPI.SpeechCodecDecodeDelegate speechCodecDecode;

        // Token: 0x04000D8F RID: 3471
        private static NativeAPI.ReleaseSpeechCodecDelegate releaseSpeechCodec;

        // Token: 0x04000D90 RID: 3472
        private static NativeAPI.CreateIPPAECDelegate createIPPAEC;

        // Token: 0x04000D91 RID: 3473
        private static NativeAPI.ReleaseIPPAECDelegate releaseIPPAEC;

        // Token: 0x04000D92 RID: 3474
        private static NativeAPI.CancelEchoIPPDelegate cancelEchoIPP;

        // Token: 0x04000D93 RID: 3475
        // private static NativeAPI.SetIPPAECParamDelegate setIPPAECParam;

        // Token: 0x04000D94 RID: 3476
        // private static NativeAPI.GetIPPAECInfoDelegate getIPPAECInfo;

        // Token: 0x04000D95 RID: 3477
        private static NativeAPI.CreateAECDelegate createAEC;

        // Token: 0x04000D96 RID: 3478
        private static NativeAPI.ReleaseAECDelegate releaseAEC;

        // Token: 0x04000D97 RID: 3479
        private static NativeAPI.ResetAECDelegate resetAEC;

        // Token: 0x04000D98 RID: 3480
        private static NativeAPI.CancelEchoDelegate cancelEcho;

        // Token: 0x04000D99 RID: 3481
        private static NativeAPI.CreateSPPDelegate createSPP;

        // Token: 0x04000D9A RID: 3482
        private static NativeAPI.ReleaseSPPDelegate releaseSPP;

        // Token: 0x04000D9B RID: 3483
        private static NativeAPI.FilterSPPDelegate filterSPP;

        // Token: 0x04000D9C RID: 3484
        private static NativeAPI.SetSPPParamDelegate setSPPParam;

        // Token: 0x04000D9D RID: 3485
        private static NativeAPI.GetSPPInfoDelegate getSPPInfo;

        // Token: 0x04000D9E RID: 3486
        private static NativeAPI.CreateSpeexCodecDelegate createSpeexCodec;

        // Token: 0x04000D9F RID: 3487
        private static NativeAPI.ReleaseSpeexCodecDelegate releaseSpeexCodec;

        // Token: 0x04000DA0 RID: 3488
        private static NativeAPI.SpeexEncodeDelegate speexEncode;

        // Token: 0x04000DA1 RID: 3489
        private static NativeAPI.SpeexDecodeDelegate speexDecode;

        // Token: 0x04000DA2 RID: 3490
        private static NativeAPI.CreateiLBCCodecDelegate createiLBCCodec;

        // Token: 0x04000DA3 RID: 3491
        private static NativeAPI.ReleaseiLBCCodecDelegate releaseiLBCCodec;

        // Token: 0x04000DA4 RID: 3492
        private static NativeAPI.iLBCEncodeDelegate iLBCEncode;

        // Token: 0x04000DA5 RID: 3493
        private static NativeAPI.iLBCDecodeDelegate iLBCDecode;

        // Token: 0x04000DA6 RID: 3494
        private static NativeAPI.CreateImgResizerDelegate createImgResizer;

        // Token: 0x04000DA7 RID: 3495
        private static NativeAPI.ResizeDelegate resize;

        // Token: 0x04000DA8 RID: 3496
        private static NativeAPI.ReleaseImgResizerDelegate releaseImgResizer;

        // Token: 0x04000DA9 RID: 3497
        private static object sync = new object();

        // Token: 0x04000DAA RID: 3498
        private static IntPtr hModule = IntPtr.Zero;

        // Token: 0x0200059B RID: 1435
        // (Invoke) Token: 0x060027BA RID: 10170
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode CreateReaderDelegate(string path, out IntPtr instance);

        // Token: 0x0200059C RID: 1436
        // (Invoke) Token: 0x060027BE RID: 10174
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode ReleaseReaderDelegate(IntPtr instance);

        //// Token: 0x0200059D RID: 1437
        //// (Invoke) Token: 0x060027C2 RID: 10178
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode GetVideoInfoDelegate(IntPtr instance, out VideoStreamInfo videoStreamInfo);

        //// Token: 0x0200059E RID: 1438
        //// (Invoke) Token: 0x060027C6 RID: 10182
        //[SuppressUnmanagedCodeSecurity]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate StatusCode GetAudioInfoDelegate(IntPtr instance, out AudioStreamInfo audioStreamInfo);

        //// Token: 0x0200059F RID: 1439
        //// (Invoke) Token: 0x060027CA RID: 10186
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode GetNextVideoDataDelegate(IntPtr instance, out VideoPacket mediaPacket);

        //// Token: 0x020005A0 RID: 1440
        //// (Invoke) Token: 0x060027CE RID: 10190
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode GetNextAudioDataDelegate(IntPtr instance, out AudioPacket audioPacket);

        // Token: 0x020005A1 RID: 1441
        // (Invoke) Token: 0x060027D2 RID: 10194
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr CreateH264EncoderDelegate();

        // Token: 0x020005A2 RID: 1442
        // (Invoke) Token: 0x060027D6 RID: 10198
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr CreateH264DecoderDelegate();

        // Token: 0x020005A3 RID: 1443
        // (Invoke) Token: 0x060027DA RID: 10202
        //[SuppressUnmanagedCodeSecurity]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate StatusCode H264EncoderInitDelegate(IntPtr instance, ref VideoStreamInfo videoInfo, int bitrate, int profileIdc, int levelIdc);

        //// Token: 0x020005A4 RID: 1444
        //// (Invoke) Token: 0x060027DE RID: 10206
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode H264DecoderInitDelegate(IntPtr instance, byte[] initData, int initDataSize);

        //// Token: 0x020005A5 RID: 1445
        //// (Invoke) Token: 0x060027E2 RID: 10210
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode H264EncodeDelegate(IntPtr instance, byte[] inData, int inDataSize, out IntPtr outData, out int outDataSize);

        //// Token: 0x020005A6 RID: 1446
        //// (Invoke) Token: 0x060027E6 RID: 10214
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode H264DecodeDelegate(IntPtr instance, byte[] inData, int inDataSize, out VideoPacket mediaPacket);

        // Token: 0x020005A7 RID: 1447
        // (Invoke) Token: 0x060027EA RID: 10218
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseBufferDelegate(IntPtr data);

        // Token: 0x020005A8 RID: 1448
        // (Invoke) Token: 0x060027EE RID: 10222
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void ReleaseH264EncoderDelegate(IntPtr instance);

        // Token: 0x020005A9 RID: 1449
        // (Invoke) Token: 0x060027F2 RID: 10226
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseH264DecoderDelegate(IntPtr instance);

        // Token: 0x020005AA RID: 1450
        // (Invoke) Token: 0x060027F6 RID: 10230
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr CreateH263EncoderDelegate();

        // Token: 0x020005AB RID: 1451
        // (Invoke) Token: 0x060027FA RID: 10234
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr CreateH263DecoderDelegate();

        // Token: 0x020005AC RID: 1452
        // (Invoke) Token: 0x060027FE RID: 10238
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode H263EncoderInitDelegate(IntPtr instance, int imgHeight, int imgWidth, double frameRate, double bitsPerPixel);

        //// Token: 0x020005AD RID: 1453
        //// (Invoke) Token: 0x06002802 RID: 10242
        //[SuppressUnmanagedCodeSecurity]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate StatusCode H263DecoderInitDelegate(IntPtr instance, byte[] initData, int initDataSize);

        //// Token: 0x020005AE RID: 1454
        //// (Invoke) Token: 0x06002806 RID: 10246
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate StatusCode H263EncodeDelegate(IntPtr instance, byte[] inData, int inDataSize, out IntPtr outData, out int outDataSize);

        //// Token: 0x020005AF RID: 1455
        //// (Invoke) Token: 0x0600280A RID: 10250
        //[SuppressUnmanagedCodeSecurity]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate StatusCode H263DecodeDelegate(IntPtr instance, byte[] inData, int inDataSize, out VideoPacket mediaPacket);

        // Token: 0x020005B0 RID: 1456
        // (Invoke) Token: 0x0600280E RID: 10254
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void ReleaseH263EncoderDelegate(IntPtr instance);

        // Token: 0x020005B1 RID: 1457
        // (Invoke) Token: 0x06002812 RID: 10258
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseH263DecoderDelegate(IntPtr instance);

        // Token: 0x020005B2 RID: 1458
        // (Invoke) Token: 0x06002816 RID: 10262
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CreateVADDetectorDelegate(out IntPtr instance);

        // Token: 0x020005B3 RID: 1459
        // (Invoke) Token: 0x0600281A RID: 10266
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseVADDetectorDelegate(IntPtr instance);

        // Token: 0x020005B4 RID: 1460
        // (Invoke) Token: 0x0600281E RID: 10270
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //[SuppressUnmanagedCodeSecurity]
        //private delegate int DetectVoiceDelegate(IntPtr instance, byte[] data, int dataLength, out VADStatus vadStatus);

        // Token: 0x020005B5 RID: 1461
        // (Invoke) Token: 0x06002822 RID: 10274
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CreateSpeechCodecDelegate(out IntPtr instance);

        // Token: 0x020005B6 RID: 1462
        // (Invoke) Token: 0x06002826 RID: 10278
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CreateG726CodecDelegate(out IntPtr instance, int bitrate);

        // Token: 0x020005B7 RID: 1463
        // (Invoke) Token: 0x0600282A RID: 10282
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SpeechCodecEncodeDelegate(IntPtr instance, byte[] inData, int inDataLength, out IntPtr outData, out int outDataLength);

        // Token: 0x020005B8 RID: 1464
        // (Invoke) Token: 0x0600282E RID: 10286
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int SpeechCodecDecodeDelegate(IntPtr instance, byte[] inData, int inDataLength, out IntPtr outData, out int outDataLength);

        // Token: 0x020005B9 RID: 1465
        // (Invoke) Token: 0x06002832 RID: 10290
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void ReleaseSpeechCodecDelegate(IntPtr instance);

        // Token: 0x020005BA RID: 1466
        // (Invoke) Token: 0x06002836 RID: 10294
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CreateIPPAECDelegate(out IntPtr instance);

        // Token: 0x020005BB RID: 1467
        // (Invoke) Token: 0x0600283A RID: 10298
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void ReleaseIPPAECDelegate(IntPtr instance);

        // Token: 0x020005BC RID: 1468
        // (Invoke) Token: 0x0600283E RID: 10302
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CancelEchoIPPDelegate(IntPtr instance, byte[] sendIn, byte[] recvIn, out IntPtr sendOut);

        // Token: 0x020005BD RID: 1469
        // (Invoke) Token: 0x06002842 RID: 10306
        //[SuppressUnmanagedCodeSecurity]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate int SetIPPAECParamDelegate(IntPtr instance, NativeAECParams param, int value);

        // Token: 0x020005BE RID: 1470
        // (Invoke) Token: 0x06002846 RID: 10310
        //[SuppressUnmanagedCodeSecurity]
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //private delegate int GetIPPAECInfoDelegate(IntPtr instance, out USC_EC_Info info);

        // Token: 0x020005BF RID: 1471
        // (Invoke) Token: 0x0600284A RID: 10314
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CreateAECDelegate(out IntPtr instance, int frameSize, int filterLength);

        // Token: 0x020005C0 RID: 1472
        // (Invoke) Token: 0x0600284E RID: 10318
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseAECDelegate(IntPtr instance);

        // Token: 0x020005C1 RID: 1473
        // (Invoke) Token: 0x06002852 RID: 10322
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ResetAECDelegate(IntPtr instance);

        // Token: 0x020005C2 RID: 1474
        // (Invoke) Token: 0x06002856 RID: 10326
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CancelEchoDelegate(IntPtr instance, short[] sendIn, short[] recvIn, out IntPtr sendOut);

        // Token: 0x020005C3 RID: 1475
        // (Invoke) Token: 0x0600285A RID: 10330
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CreateSPPDelegate(out IntPtr instance, int frameSize);

        // Token: 0x020005C4 RID: 1476
        // (Invoke) Token: 0x0600285E RID: 10334
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void ReleaseSPPDelegate(IntPtr instance);

        // Token: 0x020005C5 RID: 1477
        // (Invoke) Token: 0x06002862 RID: 10338
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FilterSPPDelegate(IntPtr instance, short[] frame, out IntPtr outFrame);

        // Token: 0x020005C6 RID: 1478
        // (Invoke) Token: 0x06002866 RID: 10342
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void SetSPPParamDelegate(IntPtr instance, int param, int value);

        // Token: 0x020005C7 RID: 1479
        // (Invoke) Token: 0x0600286A RID: 10346
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GetSPPInfoDelegate(IntPtr instance, out SpeexPPInfo info);

        // Token: 0x020005C8 RID: 1480
        // (Invoke) Token: 0x0600286E RID: 10350
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CreateSpeexCodecDelegate(out IntPtr instance, int sampleRate);

        // Token: 0x020005C9 RID: 1481
        // (Invoke) Token: 0x06002872 RID: 10354
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseSpeexCodecDelegate(IntPtr instance);

        // Token: 0x020005CA RID: 1482
        // (Invoke) Token: 0x06002876 RID: 10358
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SpeexEncodeDelegate(IntPtr instance, short[] inData, out IntPtr outData);

        // Token: 0x020005CB RID: 1483
        // (Invoke) Token: 0x0600287A RID: 10362
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SpeexDecodeDelegate(IntPtr instance, byte[] inData, int inDataLength, out IntPtr sendOut);

        // Token: 0x020005CC RID: 1484
        // (Invoke) Token: 0x0600287E RID: 10366
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [SuppressUnmanagedCodeSecurity]
        private delegate int CreateiLBCCodecDelegate(out IntPtr instance, int sampleRate);

        // Token: 0x020005CD RID: 1485
        // (Invoke) Token: 0x06002882 RID: 10370
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ReleaseiLBCCodecDelegate(IntPtr instance);

        // Token: 0x020005CE RID: 1486
        // (Invoke) Token: 0x06002886 RID: 10374
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int iLBCEncodeDelegate(IntPtr instance, short[] inData, out IntPtr outData);

        // Token: 0x020005CF RID: 1487
        // (Invoke) Token: 0x0600288A RID: 10378
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int iLBCDecodeDelegate(IntPtr instance, byte[] inData, int inDataLength, out IntPtr sendOut);

        // Token: 0x020005D0 RID: 1488
        // (Invoke) Token: 0x0600288E RID: 10382
        private delegate int CreateImgResizerDelegate(out IntPtr instance, int srcWidth, int srcHeight, int dstWidth, int dstHeight);

        // Token: 0x020005D1 RID: 1489
        // (Invoke) Token: 0x06002892 RID: 10386
        private delegate int ResizeDelegate(IntPtr instance, byte[] src, out IntPtr dst, out int dstSize);

        // Token: 0x020005D2 RID: 1490
        // (Invoke) Token: 0x06002896 RID: 10390
        private delegate void ReleaseImgResizerDelegate(IntPtr instance);
    }
}
