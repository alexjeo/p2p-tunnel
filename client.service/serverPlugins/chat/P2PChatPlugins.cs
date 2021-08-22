using common.extends;
using ProtoBuf;
using server.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.service.serverPlugins.chat
{

    //文本
    public class P2PChatTextPlugin : IP2PChatPlugin
    {
        public P2PChatTypes Type => P2PChatTypes.TEXT;

        public void Excute(P2PChatModel arg)
        {
            P2PChatTextModel textmodel = arg.Data.ProtobufDeserialize<P2PChatTextModel>();
            P2PChatEventHandles.Instance.OnTcpChatTextMessage(new P2PChatTextReceiveModel
            {
                Id = arg.FormId,
                Text = textmodel.Text
            });
        }
    }
    [ProtoContract]
    public class P2PChatTextModel
    {
        public P2PChatTextModel() { }

        [ProtoMember(1, IsRequired = true)]
        public P2PChatTypes ChatType { get; } = P2PChatTypes.TEXT;

        [ProtoMember(2)]
        public string Text { get; set; } = string.Empty;
    }

    public class P2PChatTextReceiveModel : P2PChatTextModel
    {
        public P2PChatTextReceiveModel() { }

        public long Id { get; set; } = 0;
    }



    //文件
    public class P2PChatFilePlugin : IP2PChatPlugin
    {
        public P2PChatTypes Type => P2PChatTypes.FILE;

        public void Excute(P2PChatModel arg)
        {
            P2PChatFileModel textmodel = arg.Data.ProtobufDeserialize<P2PChatFileModel>();
            P2PChatEventHandles.Instance.OnTcpChatFileMessage(new P2PChatFileReceiveModel
            {
                FromId = arg.FormId,
                Data = textmodel.Data,
                Name = textmodel.Name,
                Size = textmodel.Size,
                Md5 = textmodel.Md5,
            });
        }
    }
    [ProtoContract]
    public class P2PChatFileModel
    {
        public P2PChatFileModel() { }

        [ProtoMember(1, IsRequired = true)]
        public P2PChatTypes ChatType { get; } = P2PChatTypes.FILE;

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(3)]
        public byte[] Data { get; set; } = Array.Empty<byte>();

        [ProtoMember(4)]
        public string Md5 { get; set; } = string.Empty;

        [ProtoMember(5)]
        public long Size { get; set; } = 0;
    }

    public class P2PChatFileReceiveModel : P2PChatFileModel
    {
        public P2PChatFileReceiveModel() { }

        public long FromId { get; set; } = 0;
    }
}
