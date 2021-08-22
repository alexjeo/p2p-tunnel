using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace common.extends
{
    public static class StringExtends
    {
        public static string SubStr(this string str, int start, int maxLength)
        {
            if(maxLength + start > str.Length)
            {
                maxLength = str.Length - start;
            }
            return str.Substring(start, maxLength);
        }
    }
}
