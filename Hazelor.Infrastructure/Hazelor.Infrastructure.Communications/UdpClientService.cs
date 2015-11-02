using System.Net;
using Hazelor.Infrastructure.Communications.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Hazelor.Infrastructure.Communications
{
    public class UdpClientService : IUdpClientService
    {
        private IPEndPoint _ipEndPoint;
        private IPEndPoint _remoteEP;
        private int _localPort;
        private int _remotePort;
        //private UdpClient _listenClient;
        private UdpClient _sendClient;

        private const int BUFFERSIZE = 1024;
        private const int BUFCOUNT = 10;
        private byte[][] _buffer;
        private byte[] _tempBuffer = null;
        private int _bufferContentLength;
        private int[] _bufferContentLengthArray;
        private int _bufferIndex = 0;
        private bool _isStarted = false;
        
        //private ManualResetEvent sendDone = new ManualResetEvent(false);
        //private ManualResetEvent receiveDone = new ManualResetEvent(false);

        private event EventHandler<DataReceivedEventArgs> DataReceivedEvent;

        public UdpClientService(string RemoteIpAddress, int RemotePort, int LocalPort)
        {
            InitializeBuffer();
            InitializeConfiguration(RemoteIpAddress, RemotePort, LocalPort);
        }

        public void InitializeConfiguration(string RemoteIpAddress, int RemotePort, int LocalPort)
        {
            _localPort = LocalPort;
            _remotePort = RemotePort;
            _ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), LocalPort);
            _remoteEP = new IPEndPoint(IPAddress.Parse(RemoteIpAddress), RemotePort);
            if (_isStarted == true)
            {
                StopService();
            }
            //try
            //{
            //    //_listenClient = new UdpClient(_ipEndPoint);
            //    //_sendClient = new UdpClient(_remoteEP);
            //    _sendClient = new UdpClient(_ipEndPoint);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            //_sendClient.Connect(_remoteEP);
            //_sendClient.Send(_buffer[1], _bufferContentLengthArray.Length);
        }

        private void InitializeBuffer()
        {
            _buffer = new byte[BUFCOUNT][];
            for (int i = 0; i < BUFCOUNT; i++)
            {
                _buffer[i] = new byte[BUFFERSIZE];
            }
            _bufferContentLengthArray = new int[BUFCOUNT];
        }

        public void Register(EventHandler<DataReceivedEventArgs> eventHandler)
        {
            DataReceivedEvent += eventHandler;
        }

        public void Unregister(EventHandler<DataReceivedEventArgs> eventHandler)
        {
            DataReceivedEvent -= eventHandler;
        }

        public void StartService()
        {
            if (_isStarted)
            {
                StopService();
            }
            // TODO: Implement this method
            try
            {
                _sendClient = new UdpClient(_ipEndPoint);
                _sendClient.Connect(_remoteEP);
                _isStarted = true;
                StartListening();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void StopService()
        {
            _isStarted = false;

            // TODO: Implement this method
            //throw new NotImplementedException();
            if (_sendClient != null)
            {
                try
                {
                    //_listenClient.Close();
                    _sendClient.Close();
                    _sendClient = null;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private void StartListening()
        {
            if (_sendClient != null)
            {
                _sendClient.BeginReceive(StopListening, _sendClient);     
            }
        }

        private void StopListening(IAsyncResult AsyncResult)
        {
            try
            {
                UdpClient tcpClient = (UdpClient)AsyncResult.AsyncState;
                _tempBuffer = tcpClient.EndReceive(AsyncResult, ref _remoteEP);
                if (_tempBuffer.Length != 0)
                {
                    _bufferContentLength = _tempBuffer.Length;
                    _bufferContentLengthArray[_bufferIndex] = _bufferContentLength;
                    Buffer.BlockCopy(_tempBuffer, 0, _buffer[_bufferIndex], 0, _bufferContentLength);
                    OnDataReceived(new DataReceivedEventArgs() { Content = _tempBuffer, BufferIndex = _bufferIndex });
                    _bufferIndex = (++_bufferIndex) % BUFCOUNT;
                }
                StartListening();
            }
            catch (Exception e)
            {
                return;
            }
        }

        public byte[] ReadBytes(int bufIndex, out int readbufSize)
        {
            readbufSize = _bufferContentLengthArray[bufIndex];
            return _buffer[bufIndex];
        }

        public void SendData(byte[] data)
        {
            if (_sendClient != null)
            {
                _sendClient.Send(data, data.Length);
            }
        }

        private void OnDataReceived(DataReceivedEventArgs eventArgs)
        {
            if (DataReceivedEvent != null)
            {
                //DataReceivedEvent(this, e);
                ////System.Diagnostics.Debug.Assert();
                Delegate[] delArray = DataReceivedEvent.GetInvocationList();
                foreach (Delegate del in delArray)
                {
                    try
                    {
                        EventHandler<DataReceivedEventArgs> method = (EventHandler<DataReceivedEventArgs>)del;
                        method.BeginInvoke(this, eventArgs, OnDataReceivedProcessComplete, null);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        private void OnDataReceivedProcessComplete(IAsyncResult asyncResult)
        {
            AsyncResult result = (AsyncResult)asyncResult;
            EventHandler<DataReceivedEventArgs> del = (EventHandler<DataReceivedEventArgs>)result.AsyncDelegate;
            del.EndInvoke(asyncResult);
        }
    }
}