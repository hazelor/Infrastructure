using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Communications.Events
{
    public class DataReceivedEventArgs : EventArgs
    {
        public byte[] Content { get; set; }

        public int BufferIndex { get; set; }
    }
}
