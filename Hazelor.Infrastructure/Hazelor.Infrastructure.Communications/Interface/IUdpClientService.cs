using Hazelor.Infrastructure.Communications.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hazelor.Infrastructure.Communications
{

    public interface IUdpClientService
    {
        //event EventHandler<byte[]> DataReceivedEvent;
        //event EventHandler<DataReceivedEventArgs> DataReceivedEvent;
        void InitializeConfiguration(string RemoteIpAddress, int RemotePort, int LocalPort);
        void Register(EventHandler<DataReceivedEventArgs> eventHandler);
        void Unregister(EventHandler<DataReceivedEventArgs> eventHandler);
        void StartService();
        void StopService();
        byte[] ReadBytes(int bufIndex, out int readbufSize);
        void SendData(byte[] data);

    }
}