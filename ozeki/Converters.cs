using System;
using System.Collections.Generic;
using System.Text;

namespace ozeki
{
	public static class Converters
	{
		// Token: 0x06001548 RID: 5448 RVA: 0x0006BFD0 File Offset: 0x0006A1D0
		public static byte[] ToByteArray(this short[] array)
		{
			byte[] array2 = new byte[array.Length * 2];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0006C004 File Offset: 0x0006A204
		public static short[] ToShortArray(this byte[] array)
		{
			short[] array2 = new short[array.Length / 2];
			Buffer.BlockCopy(array, 0, array2, 0, array.Length);
			return array2;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0006C038 File Offset: 0x0006A238
		public static float[] ToFloatArray(this byte[] array)
		{
			float[] array2 = new float[array.Length / 2];
			int num = 0;
			while (num + 2 <= array.Length)
			{
				short num2 = (short)((int)array[num + 1] << 8 | (int)array[num]);
				float num3 = (float)num2 / 32768f;
				array2[num / 2] = num3;
				num += 2;
			}
			return array2;
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0006C09C File Offset: 0x0006A29C
		public static byte[] ToByteArray(this float[] array)
		{
			byte[] array2 = new byte[array.Length * 4];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			return array2;
		}
	}
}
