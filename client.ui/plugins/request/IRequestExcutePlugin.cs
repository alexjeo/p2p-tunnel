using System;
using System.Collections.Generic;
using System.Text;

namespace client.ui.plugins.request
{
    public interface IRequestExcutePlugin
    {
        RequestExcuteTypes Type { get; }

        byte[] Excute(OnTcpRequestMsessageEventArg arg);
    }
}
