using System;
using System.Linq;
using Hazelor.Infrastructure.Communications.Events;
using Hazelor.Infrastructure.Communications.Interface;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Hazelor.Infrastructure.Communications;

namespace InsTest
{
    class MainWindowVMTcpClient : BindableBase
    {
        private string _receivetext;

        public string receivetext
        {
            get
            {
                return _receivetext;
            }
            set
            {
                SetProperty(ref this._receivetext, value);
            }
        }

        private string _sendtext;

        public string sendtext
        {
            get
            {
                return _sendtext;
            }
            set
            {
                SetProperty(ref this._sendtext, value);
            }
        }

        private bool _isrunning;

        public bool isrunning
        {
            get
            {
                return _isrunning;
            }
            set
            {
                SetProperty(ref this._isrunning, value);
            }
        }

        public DelegateCommand Start { get; set; }

        //private Random random;
        public DelegateCommand Stop { get; set; }

        public DelegateCommand Send { get; set; }

        private ITcpClientService tcpListenService = new TcpClientService(11000);
        
        private void start()
        {
            isrunning = tcpListenService.Connect("127.0.0.1", 60000);
            //isrunning = true;
        }

        private void stop()
        {
            //tcpListenService.Unregister(OnDataReceived);
            isrunning = !tcpListenService.Disconnect();
            //isrunning = false;
            //tcpListenService.Dispose();
        }

        private void send()
        {
            if (isrunning)
            {
                if (sendtext != "")
                {
                    byte[] sendytes = System.Text.Encoding.Default.GetBytes(sendtext);
                    tcpListenService.SendData(sendytes);

                }
            }
            //tcpListenService.Unregister(OnDataReceived);
            //tcpListenService.StopService();
            //isrunning = false;
            //tcpListenService.Dispose();
        }

        private void OnDataReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            byte[] content = e.datagram;
            string str = System.Text.Encoding.Default.GetString(content);
            //Encoding encoding = Encoding.UTF8;
            //string contentstring = encoding.GetString(content, 0, content.Length);
            this.receivetext = str;
            //byte[] sendbackdata = new byte[] { 0xeb, 0x90 };
        }

        public MainWindowVMTcpClient()
        {
            tcpListenService.Register(OnDataReceived);
            this.isrunning = false;
            this.receivetext = "";
            this.sendtext = "";
            Start = new DelegateCommand(this.start);
            Stop = new DelegateCommand(this.stop);
            Send = new DelegateCommand(this.send);
        }
    }
}