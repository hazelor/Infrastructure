using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Hazelor.Infrastructure.Communications.Events;
using Hazelor.Infrastructure.Communications.Models;
using System.Reflection.Emit;
using Hazelor.Infrastructure.Communications.Interface;

namespace Hazelor.Infrastructure.Communications
{

    public class TcpListenerService : ITcpListenerService
    {
        private IPEndPoint _ipEndPoint;
        //private IPEndPoint _remoteEP;
        private int _listenPort;
        //private int _remotePort;
        private TcpListener _listenClient;
        //private TcpClient _sendClient;

        //private const int BUFFERSIZE = 1024;
        //private const int BUFCOUNT = 10;
        //private const int CONNECTCOUNT = 10;
        //private byte[][][] _buffer;
        //private byte[] _tempBuffer = null;
        //private int _bufferContentLength;
        //private int[][] _bufferContentLengthArray;
        private List<TcpClientState> clients = new List<TcpClientState>();
        //private int _bufferIndex = 0;
        //private int _connectionIndex = 0;
        private bool isrunning = false;
        private bool disposed = false;

        private event EventHandler<TcpDatagramReceivedEventArgs<byte[]>> DatagramReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        public TcpListenerService(int LocalPort)
        {
            InitializeBuffer();
            InitializeConfiguration(LocalPort);
        }

        public void InitializeConfiguration(int LocalPort)
        {
            _listenPort = LocalPort;
            _ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _listenPort);
            // TODO: Implement this method
            //throw new NotImplementedException();
            if (isrunning == true)
            {
                StopService();
            }
            _listenClient = new TcpListener(_ipEndPoint);
            _listenClient.AllowNatTraversal(true);
        }

        private void InitializeBuffer()
        {
            //_buffer = new byte[CONNECTCOUNT][][];
            //for (int i = 0; i < CONNECTCOUNT; i++)
            //{
            //    _buffer[i] = new byte[BUFCOUNT][];
            //    for (int j = 0; j < BUFCOUNT; j++)
            //    {
            //        _buffer[i][j] = new byte[BUFFERSIZE];
            //    }
            //}
            //_bufferContentLengthArray = new int[CONNECTCOUNT][];
            //for (int i = 0; i < BUFCOUNT; i++)
            //{
            //    _bufferContentLengthArray[i] = new int[BUFCOUNT];
            //}
        }

        public void Register(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler)
        {
            // TODO: Implement this method
            DatagramReceivedEvent += eventHandler;
        }

        public void Unregister(EventHandler<TcpDatagramReceivedEventArgs<byte[]>> eventHandler)
        {
            // TODO: Implement this method
            DatagramReceivedEvent -= eventHandler;
        }

        public void StartService()
        {
            if (isrunning != true)
            {
                if (_listenClient != null)
                {
                    if (isrunning)
                    {
                        StopService();
                    }
                    isrunning = true;
                    StartListening();
                }
            }
            // TODO: Implement this method
        }

        public void StopService()
        {
            // TODO: Implement this method
            //if (isrunning)
            //{
            //}
            isrunning = false;
            if (_listenClient != null)
            {
                _listenClient.Stop();                
            }
            lock (this.clients)
            {
                for (int i = 0; i < this.clients.Count; i++)
                {
                    this.clients[i].tcpClient.Client.Disconnect(false);
                }
                this.clients.Clear();
            }
        }

        private void StartListening()
        {
            _listenClient.Start();
            _listenClient.BeginAcceptTcpClient(HandleTcpClientAccepted, _listenClient);
        }

        private void HandleTcpClientAccepted(IAsyncResult AsyncResult)
        {
            if (isrunning)
            {
                TcpListener tcpListener = (TcpListener)AsyncResult.AsyncState;
                TcpClient tcpClient = tcpListener.EndAcceptTcpClient(AsyncResult);
                TcpClientState tcpClientState = new TcpClientState(tcpClient);
                lock (this.clients)
                {
                    this.clients.Add(tcpClientState);
                    RaiseClientConnected(tcpClient);
                    //_connectionIndex = this.clients.IndexOf(tcpClientState);
                    //_bufferIndex = (++_bufferIndex) % BUFCOUNT;
                }

                NetworkStream networkStream = tcpClientState.networkStream;
                networkStream.BeginRead(tcpClientState.buffer, 0, tcpClientState.buffer.Length, HandleDatagramReceived, tcpClientState);
                tcpListener.BeginAcceptTcpClient(HandleTcpClientAccepted, AsyncResult.AsyncState);
            }
        }

        private void HandleDatagramReceived(IAsyncResult AsyncResult)
        {
            if (isrunning)
            {
                TcpClientState tcpClientState = (TcpClientState)AsyncResult.AsyncState;
                NetworkStream networkStream = tcpClientState.networkStream;
                int numberOfReadBytes = 0;
                try
                {
                    numberOfReadBytes = networkStream.EndRead(AsyncResult);
                }
                catch
                {
                    numberOfReadBytes = 0;
                }
                if (numberOfReadBytes == 0)
                {
                    lock (this.clients)
                    {
                        this.clients.Remove(tcpClientState);
                        RaiseClientDisconnected(tcpClientState.tcpClient);
                        return;
                    }
                }
                byte[] receivedBytes = new byte[numberOfReadBytes];
                Buffer.BlockCopy(tcpClientState.buffer, 0, receivedBytes, 0, numberOfReadBytes);
                RaiseDatagramReceived(tcpClientState.tcpClient, receivedBytes);
                networkStream.BeginRead(tcpClientState.buffer, 0, tcpClientState.buffer.Length, HandleDatagramReceived, tcpClientState);
            }
        }

        private void RaiseDatagramReceived(TcpClient tcpClient, byte[] receivedBytes)
        {
            if (DatagramReceivedEvent != null)
            {
                Delegate[] delArray = DatagramReceivedEvent.GetInvocationList();
                foreach (Delegate del in delArray)
                {
                    try
                    {
                        EventHandler<TcpDatagramReceivedEventArgs<byte[]>> method = (EventHandler<TcpDatagramReceivedEventArgs<byte[]>>)del;
                        method.BeginInvoke(this, new TcpDatagramReceivedEventArgs<byte[]>(tcpClient, receivedBytes), OnReceivedDatagramProcessComplete, null);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        private void OnReceivedDatagramProcessComplete(IAsyncResult AsyncResult)
        {
            AsyncResult result = (AsyncResult)AsyncResult;
            EventHandler<TcpDatagramReceivedEventArgs<byte[]>> del = (EventHandler<TcpDatagramReceivedEventArgs<byte[]>>)result.AsyncDelegate;
            del.EndInvoke(AsyncResult);
        }

        private void RaiseClientConnected(TcpClient tcpClient)
        {
            if (ClientConnectedEvent != null)
            {
                ClientConnectedEvent(this, new ClientConnectedEventArgs(tcpClient));
            }
        }

        private void RaiseClientDisconnected(TcpClient tcpClient)
        {
            if (ClientDisconnectedEvent != null)
            {
                ClientDisconnectedEvent(this, new ClientDisconnectedEventArgs(tcpClient));
            }
        }

        //public byte[] ReadBytes(int bufIndex, out int readbufSize)
        //{
        //    // TODO: Implement this method
        //    readbufSize = 0;
        //    throw new NotImplementedException();
        //}

        public void SendData(TcpClient tcpClient, byte[] datas)
        {
            if (!isrunning)
            {
                throw new InvalidProgramException("This Tcp Server has not been started.");
            }
            if (tcpClient == null)
            {
                throw new ArgumentNullException("tcpClient");
            }
            if (datas == null)
            {
                throw new ArgumentNullException("datas");
            }
            tcpClient.GetStream().BeginWrite(datas, 0, datas.Length, DatagramWrittenCompleted, tcpClient);
            // TODO: Implement this method
        }

        private void DatagramWrittenCompleted(IAsyncResult AsyncResult)
        {
            ((TcpClient)AsyncResult.AsyncState).GetStream().EndWrite(AsyncResult);
        }

        public void SendDataToALL(byte[] datas)
        {
            if (!isrunning)
            {
                throw new InvalidProgramException("This Tcp Server has not been started.");
            }
            //if (tcpClient == null)
            //{
            //    throw new ArgumentNullException("tcpClient");
            //}
            if (datas == null)
            {
                throw new ArgumentNullException("datas");
            }
            for (int i = 0; i < this.clients.Count; i++)
            {
                SendData(this.clients[i].tcpClient, datas);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        StopService();
                        if (_listenClient != null)
                        {
                            _listenClient = null;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                disposed = true;
            }
        }
    }
}