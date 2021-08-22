using System;
using System.Collections.Generic;
using System.Text;

namespace client.service.serverPlugins.request
{
    public interface IRequestExcuteMessage
    {
        RequestExcuteTypes Type { get; }
    }
}
