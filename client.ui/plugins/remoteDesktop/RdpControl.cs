using System;
using System.Collections.Generic;
using System.Text;

namespace client.ui.plugins.remoteDesktop
{
    public class RdpControl : AxMSTSCLib.AxMsRdpClient9NotSafeForScripting
    {
        public RdpControl() : base() { }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x0021)
            {
                if (!this.ContainsFocus)
                {
                    this.Focus();
                }
            }

            base.WndProc(ref m);
        }
    }

}
