using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ozeki
{
	[DataContract]
	public enum CodecPayloadType
	{
		// Token: 0x04000F95 RID: 3989
		[EnumMember]
		Dynamic = -1,
		// Token: 0x04000F96 RID: 3990
		[EnumMember]
		PCMU,
		// Token: 0x04000F97 RID: 3991
		[EnumMember]
		GSM = 3,
		// Token: 0x04000F98 RID: 3992
		[EnumMember]
		G723,
		// Token: 0x04000F99 RID: 3993
		[EnumMember]
		PCMA = 8,
		// Token: 0x04000F9A RID: 3994
		[EnumMember]
		G722,
		// Token: 0x04000F9B RID: 3995
		[EnumMember]
		L16_44_2,
		// Token: 0x04000F9C RID: 3996
		[EnumMember]
		L16_44_1,
		// Token: 0x04000F9D RID: 3997
		[EnumMember]
		G728 = 15,
		// Token: 0x04000F9E RID: 3998
		[EnumMember]
		G729 = 18,
		// Token: 0x04000F9F RID: 3999
		[EnumMember]
		H263 = 34,
		// Token: 0x04000FA0 RID: 4000
		[EnumMember]
		Speex_Narrowband = 97,
		// Token: 0x04000FA1 RID: 4001
		[EnumMember]
		iLBC,
		// Token: 0x04000FA2 RID: 4002
		[EnumMember]
		H264,
		// Token: 0x04000FA3 RID: 4003
		[EnumMember]
		Speex_Wideband,
		// Token: 0x04000FA4 RID: 4004
		[EnumMember]
		Speex_Ultrawideband = 108,
		// Token: 0x04000FA5 RID: 4005
		[EnumMember]
		telephone_event = 101,
		// Token: 0x04000FA6 RID: 4006
		[EnumMember]
		H263_plus,
		// Token: 0x04000FA7 RID: 4007
		[EnumMember]
		L16,
		// Token: 0x04000FA8 RID: 4008
		[EnumMember]
		G726_16,
		// Token: 0x04000FA9 RID: 4009
		[EnumMember]
		G726_24,
		// Token: 0x04000FAA RID: 4010
		[EnumMember]
		G726_32,
		// Token: 0x04000FAB RID: 4011
		[EnumMember]
		G726_40
	}
}
