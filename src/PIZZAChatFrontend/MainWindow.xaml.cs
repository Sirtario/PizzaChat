using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private string _messages = string.Empty;
        private string _status = "Hier koennte Ihr Status stehen!";
        private string _members = "Member";
        private string _channels = "Channel";
        private string _peoples = string.Empty;
        private string _mainHtml;

        public MainWindow()
        {
            InitializeComponent();

            _mainHtml = File.ReadAllText("Content.html");

            ShowInfo();
        }

        public event Action<string> SendMessage;
        //whisperTarget, message
        public event Action<string, string> WhisperMessage;
        public event Action<string> EnterRoom;
        //serverindex
        public event Action<int> Connect;
        public event Action Disconnect;
        public event Action GetServers;

        public void ReceiveMessage(string message, string sender, bool isWhispered)
        {
            var htmlClass = "message";
            message = BeautifullText(message);

            if (isWhispered)
            {
                htmlClass = "whispered-message";
            }

            _messages += $"<p><div class=\"sender\">{sender}</div><div class=\"{htmlClass}\">{message}</div></p>";

            ShowInfo();
        }

        private string BeautifullText(string message)
        {
            message = message.Replace("Ü", "&Uuml;").Replace("Ä", "&Auml;").Replace("Ö", "&Ouml;").Replace("ä", "&auml;").Replace("ö", "&ouml;").Replace("ü", "&uuml;").Replace("ß", "&szlig;");

            return message;
        }

        public void RefreshStatus(List<string> usersInChannel, string channel)
        {
            throw new NotImplementedException();
        }

        public void ShowServerlist(List<Tuple<string, string, string>> servers)
        {
            var serverWindow = new ServerList(servers);

            serverWindow.ConnectTo += ServerWindow_ConnectTo;

            serverWindow.Show();
        }

        private void ServerWindow_ConnectTo(int obj)
        {
            Connect?.Invoke(obj);
        }

        private void ShowInfo()
        {
            var html = _mainHtml.Replace("__MESSAGES__", _messages);
            html = html.Replace("__STATUS__", _status);
            html = html.Replace("__MEMBER__", _members);
            html = html.Replace("__CHANNEL__", _channels);

            _browserControl.NavigateToString(html);
        }

        private void ListServers_Click(object sender, RoutedEventArgs e)
        {
            GetServers?.Invoke();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //Connect?.Invoke();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect?.Invoke();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            SendMessageFromTextbox();
        }

        private void Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMessageFromTextbox();
            }
        }

        private void SendMessageFromTextbox()
        {
            if (string.IsNullOrEmpty(_textbox.Text))
                return;

            SendMessage?.Invoke(_textbox.Text);

            _textbox.Text = string.Empty;

            _textbox.Focus();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            _textbox.Focus();
        }

        private void BrowserControl_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var document = _browserControl.Document as mshtml.HTMLDocument;
            document.parentWindow.scroll(0, 10000000);
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
