using server.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.ui.plugins.chat
{
    public interface IP2PChatPlugin
    {
        P2PChatTypes Type { get; }

        void Excute(P2PChatModel arg);
    }
}
