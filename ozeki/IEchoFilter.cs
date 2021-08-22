using System;
using System.Collections.Generic;
using System.Text;

namespace ozeki
{
	internal interface IEchoFilter
	{
		// Token: 0x06000D10 RID: 3344
		byte[] Filter(byte[] localFrame, byte[] remoteFrame);

		// Token: 0x06000D11 RID: 3345
		void Dispose();
	}
}
