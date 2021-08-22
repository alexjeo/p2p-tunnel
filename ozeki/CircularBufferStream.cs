using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ozeki
{
	public class CircularBufferStream : Stream
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x000701D0 File Offset: 0x0006E3D0
		public int ReadPosition
		{
			get
			{
				return this.readPosition;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x000701F0 File Offset: 0x0006E3F0
		public int WritePosition
		{
			get
			{
				return this.writePosition;
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00070210 File Offset: 0x0006E410
		public CircularBufferStream(int size)
		{
			this.buffer = new byte[size];
			this.lockObject = new object();
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00070234 File Offset: 0x0006E434
		public void SetBufferSize(int size)
		{
			lock (this.lockObject)
			{
				this.buffer = new byte[size];
			}
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x0007027C File Offset: 0x0006E47C
		public override void Write(byte[] data, int offset, int count)
		{
			lock (this.lockObject)
			{
				int num = 0;
				if (count > this.buffer.Length - this.byteCount)
				{
					count = this.buffer.Length - this.byteCount;
				}
				int num2 = Math.Min(this.buffer.Length - this.writePosition, count);
				Array.Copy(data, offset, this.buffer, this.writePosition, num2);
				this.writePosition += num2;
				this.writePosition %= this.buffer.Length;
				num += num2;
				if (num < count)
				{
					Array.Copy(data, offset + num, this.buffer, this.writePosition, count - num);
					this.writePosition += count - num;
					num = count;
				}
				this.byteCount += num;
			}
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x00070394 File Offset: 0x0006E594
		public override int Read(byte[] data, int offset, int count)
		{
			int result;
			lock (this.lockObject)
			{
				if (count > this.byteCount)
				{
					count = this.byteCount;
				}
				int num = 0;
				int num2 = Math.Min(this.buffer.Length - this.readPosition, count);
				Array.Copy(this.buffer, this.readPosition, data, offset, num2);
				num += num2;
				this.readPosition += num2;
				this.readPosition %= this.buffer.Length;
				if (num < count)
				{
					Array.Copy(this.buffer, this.readPosition, data, offset + num, count - num);
					this.readPosition += count - num;
					num = count;
				}
				this.byteCount -= num;
				result = num;
			}
			return result;
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x000704B0 File Offset: 0x0006E6B0
		public int MaxLength
		{
			get
			{
				return this.buffer.Length;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x000704D4 File Offset: 0x0006E6D4
		public int Count
		{
			get
			{
				return this.byteCount;
			}
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000704F4 File Offset: 0x0006E6F4
		public void Reset()
		{
			lock (this.lockObject)
			{
				this.byteCount = 0;
				this.readPosition = 0;
				this.writePosition = 0;
			}
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00070548 File Offset: 0x0006E748
		public void Advance(int count)
		{
			if (count >= this.byteCount)
			{
				this.Reset();
			}
			else
			{
				this.byteCount -= count;
				this.readPosition += count;
				this.readPosition %= this.MaxLength;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x000705AC File Offset: 0x0006E7AC
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x000705C8 File Offset: 0x0006E7C8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x000705E4 File Offset: 0x0006E7E4
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00070600 File Offset: 0x0006E800
		public override void Flush()
		{
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00070604 File Offset: 0x0006E804
		public override long Length
		{
			get
			{
				return (long)this.byteCount;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x00070624 File Offset: 0x0006E824
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x00070644 File Offset: 0x0006E844
		public override long Position
		{
			get
			{
				return (long)this.readPosition;
			}
			set
			{
			}
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00070648 File Offset: 0x0006E848
		public override long Seek(long offset, SeekOrigin origin)
		{
			return (long)this.readPosition;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00070668 File Offset: 0x0006E868
		public override void SetLength(long value)
		{
		}

		// Token: 0x04000F4F RID: 3919
		private byte[] buffer;

		// Token: 0x04000F50 RID: 3920
		private int writePosition;

		// Token: 0x04000F51 RID: 3921
		private int readPosition;

		// Token: 0x04000F52 RID: 3922
		private int byteCount;

		// Token: 0x04000F53 RID: 3923
		private readonly object lockObject;
	}
}
