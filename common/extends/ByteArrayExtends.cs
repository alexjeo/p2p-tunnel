using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

namespace common.extends
{
    public static class ByteArrayExtends
    {
        public static Byte[] ProtobufSerialize<T>(this T obj)
        {
            using var memory = new MemoryStream();
            Serializer.Serialize(memory, obj);
            return memory.ToArray();
        }
        public static T ProtobufDeserialize<T>(this byte[] data)
        {
            using var memory = new MemoryStream(data);
            return Serializer.Deserialize<T>(memory);
        }

    }
}
