using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hazelor.Infrastructure.Communications.Models;
using System.Net.Sockets;

namespace Hazelor.Infrastructure.Communications.Events
{
    public class TcpDatagramReceivedEventArgs<T> : EventArgs
    {
        public TcpClient tcpClient { get; set; }

        public T datagram { get; set; }

        public TcpDatagramReceivedEventArgs(TcpClient tcpClient, T datagram)
        {
            this.tcpClient = tcpClient;
            this.datagram = datagram;
        }
    }
}