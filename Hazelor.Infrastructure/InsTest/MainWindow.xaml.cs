using Hazelor.Infrastructure.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hazelor.Infrastructure.Communications.Events;

namespace InsTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUdpClientService _cS;
        public MainWindow()
        {
            InitializeComponent();
            //_cS = new UdpClientService();
            //_cS.Register(test);

        }

        public void test(object sender, DataReceivedEventArgs e)
        {

        }

    }


}
