using server.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace server.packet
{
    /// <summary>
    /// 自定义TCP数据包
    /// </summary>
    public class TcpPacket : IPacket
    {
        public byte[] Chunk { get; set; }

        public MessageTypes Type { get; set; }

        public TcpPacket(byte[] chunk, MessageTypes type)
        {
            Chunk = chunk;
            Type = type;
        }

        /// <summary>
        /// 包转 byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            byte[] typeArray = BitConverter.GetBytes((int)Type);
            byte[] lengthArray = BitConverter.GetBytes(Chunk.Length + typeArray.Length);

            byte[] result = new byte[lengthArray.Length + typeArray.Length + Chunk.Length];

            int distIndex = 0;
            Array.Copy(lengthArray, 0, result, distIndex, lengthArray.Length);
            distIndex += lengthArray.Length;

            Array.Copy(typeArray, 0, result, distIndex, typeArray.Length);
            distIndex += typeArray.Length;

            Array.Copy(Chunk, 0, result, distIndex, Chunk.Length);

            return result;
        }

        /// <summary>
        /// byte[]  转为包结构
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List<TcpPacket> FromArray(List<byte> buffer)
        {
            List<TcpPacket> result = new List<TcpPacket>();
            do
            {
                int packageLen = BitConverter.ToInt32(buffer.GetRange(0, 4).ToArray());
                if (packageLen > buffer.Count - 4)
                {
                    break;
                }

                byte[] rev = buffer.GetRange(4, packageLen).ToArray();
                lock (buffer)
                {
                    buffer.RemoveRange(0, packageLen + 4);
                }
                result.Add(ToPacket(rev));
            } while (buffer.Count > 4);

            return result;
        }

        private static TcpPacket ToPacket(byte[] array)
        {
            int offset = 0;
            int type = BitConverter.ToInt32(array.Skip(offset).Take(4).ToArray());
            offset += 4;
            byte[] chunk = array.Skip(offset).ToArray();

            return new TcpPacket(chunk, (MessageTypes)type);
        }
    }
}
