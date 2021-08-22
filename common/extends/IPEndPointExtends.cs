using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace common.extends
{
    public static class IPEndPointExtends
    {
        /// <summary>
        /// ip转long  4个段，分别一个字节， 255 255 255 255 ，再拼上端口 4字节
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long ToInt64(this IPEndPoint ip)
        {
            byte[] byteIp = ip.Address.GetAddressBytes();
            byte[] bytePort = BitConverter.GetBytes(ip.Port);

            byte[] dist = new byte[byteIp.Length + bytePort.Length];

            Array.Copy(byteIp, 0, dist, 0, byteIp.Length);
            Array.Copy(bytePort, 0, dist, 4, bytePort.Length);

            return BitConverter.ToInt64(dist);
        }
    }
}
