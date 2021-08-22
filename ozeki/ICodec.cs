using System;
using System.Collections.Generic;
using System.Text;

namespace ozeki
{
	public interface ICodec
	{
		// Token: 0x06001916 RID: 6422
		byte[] Encode(byte[] data);

		// Token: 0x06001917 RID: 6423
		byte[] Decode(byte[] data);

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001918 RID: 6424
		string Description { get; }

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001919 RID: 6425
		int SampleRate { get; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600191A RID: 6426
		CodecPayloadType PayloadType { get; }

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600191B RID: 6427
		MediaType MediaType { get; }

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600191C RID: 6428
		string EncodingName { get; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x0600191D RID: 6429
		int Channels { get; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x0600191E RID: 6430
		int PacketizationTime { get; }

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x0600191F RID: 6431
		int Bitrate { get; }

		// Token: 0x06001920 RID: 6432
		string GetFmtp();
	}
}
