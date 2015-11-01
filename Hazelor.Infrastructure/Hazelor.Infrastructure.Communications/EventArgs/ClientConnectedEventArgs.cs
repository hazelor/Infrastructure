using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Communications.Events
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public TcpClient tcpClient { get; set; }

        public ClientConnectedEventArgs(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }
    }
}