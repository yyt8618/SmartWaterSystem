using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public delegate void PortDataReceivedEventHandle(object sender, PortDataReciveEventArgs e);

    public class PortDataReciveEventArgs : EventArgs
    {
        private byte[] data;

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public PortDataReciveEventArgs()
        {
            this.data = null;
        }

        public PortDataReciveEventArgs(byte[] data)
        {
            this.data = data;
        }


    }
}
