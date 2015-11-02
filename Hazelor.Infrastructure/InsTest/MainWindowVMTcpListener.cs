using Hazelor.Infrastructure.Communications;
using Hazelor.Infrastructure.Communications.Events;
using Hazelor.Infrastructure.Communications.Interface;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsTest
{
    class MainWindowVMTcpListener : BindableBase
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

        private ITcpListenerService tcpListenService = new TcpListenerService(11000);
        
        private void start()
        {
            tcpListenService.StartService();
            isrunning = true;
        }

        private void stop()
        {
            //tcpListenService.Unregister(OnDataReceived);
            tcpListenService.StopService();
            isrunning = false;
            //tcpListenService.Dispose();
        }


        private void OnDataReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            byte[] content = e.datagram;
            string str = System.Text.Encoding.Default.GetString(content);
            //Encoding encoding = Encoding.UTF8;
            //string contentstring = encoding.GetString(content, 0, content.Length);
            this.receivetext = str;
            byte[] sendbackdata = new byte[] { 0xeb, 0x90};
            tcpListenService.SendData(e.tcpClient, sendbackdata);
        }
        public MainWindowVMTcpListener()
        {
            tcpListenService.Register(OnDataReceived);
            this.isrunning = false;
            this.receivetext = "";
            Start = new DelegateCommand(this.start);
            Stop = new DelegateCommand(this.stop);
        }
    }
}
