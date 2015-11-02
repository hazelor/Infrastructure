using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Communications.Models
{
    public class TcpClientState
    {
        private const int _bufferSize = 1024;

        public TcpClient tcpClient { get; set; }

        public NetworkStream networkStream { get; set; }

        public byte[] buffer { get; set; }

        public TcpClientState(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.networkStream = this.tcpClient.GetStream();
            this.buffer = new byte[_bufferSize];
        }
    }
}