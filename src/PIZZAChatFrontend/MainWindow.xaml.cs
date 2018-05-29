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

namespace PIZZA.Client
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPIZZAFrontend
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public event Action<string> SendMessage;
        public event Action<string, string> WhisperMessage;
        public event Action<string> EnterRoom;
        public event Action<int> Connect;
        public event Action Disconnect;
        public event Action GetServers;

        public void ReceiveMessage(string Message, string sender, bool isWhispered)
        {
            throw new NotImplementedException();
        }

        public void RefreshStatus(List<string> usersInChannel, string channel)
        {
            throw new NotImplementedException();
        }

        public void ShowServerlist(List<Tuple<string, string, string>> servers)
        {
            throw new NotImplementedException();
        }
    }
}
