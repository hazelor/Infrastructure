using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Communications.Models
{
    public class NetworkStreamState
    {
        public NetworkStream networkStream { get; set; }

        public byte[] buffer { get; set; }
    }
}
