﻿using System.Runtime.Remoting.Messaging;
using Hazelor.Infrastructure.Communications.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Hazelor.Infrastructure.Communications.Interface;
using Hazelor.Infrastructure.Communications.Models;

namespace Hazelor.Infrastructure.Communications
{
    public class TcpClientService : ITcpClientService
    {
        private IPEndPoint _localEP;
        private IPEndPoint _remoteEP;
        private int _localPort;
        private int _remotePort;
        private TcpClient _localClient;

        //private UdpClient _sendClient;

        //private const int BUFFERSIZE = 1024;
        //private const int BUFCONUT = 10;
        //private byte[][] _buffer;
        //private byte[] _tempBuffer = null;
        //private int _bufferContentLength;
        //private int[] _bufferContentLengthArray = new int[10];
        //private int _bufferIndex = 0;
        //private bool isconnecting = false;
        //private bool disposed = false;

        private event EventHandler<TcpDatagramReceivedEventArgs<byte[]>> DatagramReceivedEvent;

        public TcpClientService()
        {
            _localPort = 0;
            InitializeConfiguration(_localPort);
        }

        public TcpClientService(int LocalPort)
        {
            InitializeConfiguration(LocalPort);
        }

        public void InitializeConfiguration(int LocalPort)
        {
            _localPort = LocalPort;
            //_remotePort = RemotePort;
            // TODO: Implement this method
            //throw new NotImplementedException();
            //_localEP = new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[0], _localPort);
            _localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _localPort);
            if (_localClient != null)
            {
                try
                {
                    Disconnect();
                    Dispose();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            //_localClient = new TcpClient(_localEP);
        }

        public void Register(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler)
        {
            // TODO: Implement this method
            //throw new NotImplementedException();
            DatagramReceivedEvent += eventHandler;
        }

        public void Unregister(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler)
        {
            // TODO: Implement this method
            //throw new NotImplementedException();
            DatagramReceivedEvent -= eventHandler;
        }

        public bool Connect(string RemoteIpAddress, int RemotePort)
        {
            if (_localClient != null)
            {
                try
                {
                    Disconnect();
                    Dispose();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            _localClient = new TcpClient(_localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _localPort)); 
            _remotePort = RemotePort;
            _remoteEP = new IPEndPoint(IPAddress.Parse(RemoteIpAddress), _remotePort);
            //if (_localClient.Connected)
            //{
            //    Disconnect();
            //}
            try
            {
                _localClient.Connect(_remoteEP);
                //_localClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                StartListening();
                //return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return _localClient.Connected;
            //return false;
            // TODO: Implement this  
            //throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            if (_localClient != null)
            {
                if (_localClient.Connected)
                {
                    //_localClient.Client.Shutdown(SocketShutdown.Both);
                    _localClient.Client.Disconnect(true);
                    _localClient.Client.Close();
                    _localClient.Client.Dispose();
                    //return true;
                }
                return !_localClient.Connected;
            }
            return true;
            // TODO: Implement this method
            //throw new NotImplementedException();
        }

        private void StartListening()
        {
            NetworkStream networkStream = _localClient.GetStream();
            NetworkStreamState networkStreamState = new NetworkStreamState()
            {
                networkStream = networkStream,
                buffer = new byte[1024]
            };
            networkStream.BeginRead(networkStreamState.buffer, 0, networkStreamState.buffer.Length, OnDataReadCompleted, networkStreamState);
        }

        //public byte[] ReadBytes(int bufIndex, out int readbufSize)
        //{
        //    // TODO: Implement this method
        //    readbufSize = 0;
        //    throw new NotImplementedException();
        //}

        public void SendData(byte[] data)
        {
            // TODO: Implement this method
            //throw new NotImplementedException();
            if (_localClient != null)
            {
                if (_localClient.Connected)
                {
                    NetworkStream networkStream = _localClient.GetStream();
                    if (networkStream.CanWrite)
                    {
                        networkStream.BeginWrite(data, 0, data.Length, OnDataSendCompleted, networkStream);
                    }
                }
            }
        }

        private void OnDataSendCompleted(IAsyncResult AsyncResult)
        {
            NetworkStream networkStream = (NetworkStream)AsyncResult.AsyncState;
            //NetworkStreamState networkStreamState = new NetworkStreamState(){
            //    networkStream = networkStream,
            //    buffer = new byte[1024]
            //};
            networkStream.EndWrite(AsyncResult);
        }

        private void OnDataReadCompleted(IAsyncResult AsyncResult)
        {
            NetworkStreamState networkStreamState = (NetworkStreamState)AsyncResult.AsyncState;
            int numberOfReadBytes = 0;
            try
            {
                numberOfReadBytes = networkStreamState.networkStream.EndRead(AsyncResult);
            }
            catch
            {
                numberOfReadBytes = 0;
            }
            if (numberOfReadBytes == 0)
            {
                //networkStreamState.networkStream.Close(0);
                return;
            }
            //networkStreamState.networkStream.Close(0);
            byte[] receivedBytes = new byte[numberOfReadBytes];
            Buffer.BlockCopy(networkStreamState.buffer, 0, receivedBytes, 0, numberOfReadBytes);
            if (DatagramReceivedEvent != null)
            {
                Delegate[] delArray = DatagramReceivedEvent.GetInvocationList();
                foreach (Delegate del in delArray)
                {
                    try
                    {
                        EventHandler<TcpDatagramReceivedEventArgs<byte[]>> method = (EventHandler<TcpDatagramReceivedEventArgs<byte[]>>)del;
                        method.BeginInvoke(this, new TcpDatagramReceivedEventArgs<byte[]>(_localClient, receivedBytes), OnReceivedDatagramProcessComplete, null);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            networkStreamState.networkStream.BeginRead(networkStreamState.buffer, 0, networkStreamState.buffer.Length, OnDataReadCompleted, networkStreamState);
        }

        private void OnReceivedDatagramProcessComplete(IAsyncResult AsyncResult)
        {
            AsyncResult result = (AsyncResult)AsyncResult;
            EventHandler<TcpDatagramReceivedEventArgs<byte[]>> del = (EventHandler<TcpDatagramReceivedEventArgs<byte[]>>)result.AsyncDelegate;
            del.EndInvoke(AsyncResult);
        }

        public void Dispose()
        {
            if (_localClient != null)
            {
                try
                {
                    _localClient.Close();

                    if (_localClient != null)
                    {
                        _localClient = null;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}