using Hazelor.Infrastructure.Communications.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Communications.Interface
{
    public interface ITcpClientService
    {
        void InitializeConfiguration(int LocalPort);

        void Register(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler);

        void Unregister(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler);

        bool Connect(string RemoteIpAddress, int RemotePort);

        bool Disconnect();

        void SendData(byte[] data);

        void Dispose();
    }
}