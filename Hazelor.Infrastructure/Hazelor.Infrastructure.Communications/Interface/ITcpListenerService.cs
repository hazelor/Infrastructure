using Hazelor.Infrastructure.Communications.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Communications.Interface
{
    public interface ITcpListenerService
    {
        event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        void InitializeConfiguration(int LocalPort);

        void Register(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler);

        void Unregister(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler);

        void StartService();

        void StopService();

        void SendData(TcpClient tcpClient, byte[] datas);

        void SendDataToALL(byte[] datas);

        void Dispose();
    }
}
