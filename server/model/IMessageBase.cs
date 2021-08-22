using System;
using System.Collections.Generic;
using System.Text;

namespace server.model
{
    public interface IMessageModelBase
    {
        MessageTypes MsgType { get;}
    }
}
