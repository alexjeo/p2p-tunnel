using System;
using System.Collections.Generic;
using System.Text;

namespace client.ui.plugins.request
{
    public interface IRequestExcuteMessage
    {
        RequestExcuteTypes Type { get; }
    }
}
