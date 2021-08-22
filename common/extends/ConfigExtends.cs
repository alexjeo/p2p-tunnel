using ProtoBuf;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace common.extends
{
    public static class ConfigExtends
    {
        public static bool ProtobufSerializeFileSave(this object obj, string fileName)
        {
            try
            {
                string json = Helper.JsonSerializer(obj);
                File.WriteAllText(fileName, json);
            }
            catch (System.Exception)
            {
            }
            return true;
        }
        public static T ProtobufDeserializeFileRead<T>(this string fileName)
        {
            T result = default;
            if (File.Exists(fileName))
            {
                try
                {
                    result = Helper.DeJsonSerializer<T>(File.ReadAllText(fileName));
                }
                catch (System.Exception)
                {
                }
            }
            return result;
        }

    }

    public interface IFileConfig
    {

    }
}
