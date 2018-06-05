using PIZZA.Chat.Core;
using PIZZA.Client;
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

namespace PIZZAChatFrontend
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPIZZAFrontend
    {
        private string _messages = string.Empty;
        private string _status = string.Empty;
        private string _members = string.Empty;
        private string _channels = string.Empty;
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
        //serverindex
        public event Action<int> Connect;
        public event Action Disconnect;
        public event Action GetServers;
        public event Action<PIZZAChannel> EnterRoom;

        public void ReceiveMessage(string message, string sender, bool isWhispered)
        {
            var htmlClass = "message";
            message = BeautifyText(message);

            if (isWhispered)
            {
                htmlClass = "whispered-message";
            }

            _messages += $"<p><div class=\"sender\">{sender}</div><div class=\"{htmlClass}\">{message}</div></p>";

            ShowInfo();
        }

        private string BeautifyText(string message)
        {
            message = message.Replace("Ü", "&Uuml;").Replace("Ä", "&Auml;").Replace("Ö", "&Ouml;").Replace("ä", "&auml;").Replace("ö", "&ouml;").Replace("ü", "&uuml;").Replace("ß", "&szlig;");

            return message;
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

        public void ShowServerlist(List<Tuple<string, string, string, bool>> servers)
        {
            var serverWindow = new ServerList(servers);

            serverWindow.ConnectTo += ServerWindow_ConnectTo;

            serverWindow.Show();
        }

        public void RefreshStatus(List<string> usersInChannel, List<PIZZAChannel> channels, string channel, string hostname)
        {
            foreach (var user in usersInChannel)
            {
                _members += $"<div class=\"member\">{user}</div>";
            }

            foreach (var currentChannel in channels)
            {
                _channels += $"<div class=\"channel\">{BeautifyText(currentChannel.Channelname.Value)}</div>";
            }

            _status = $"<div class=\"current-host\">{BeautifyText(hostname)}<div class=\"current-channel\">{BeautifyText(channel)}</div></div>";

            ShowInfo();
        }

        public string GetClientId(string hostname)
        {
            var window = new GetText($"ClientId:", $"server: {BeautifyText(hostname)}");

            window.ShowDialog();

            return window.Value;
        }

        public string GetPassword(string topic)
        {
            var window = new GetText($"Password:", topic);

            window.ShowDialog();

            return window.Value;
        }

        public void ShowReturncode(ChatConnectReturncode returncode, string hostname)
        {
            var text = $"connect to server: {BeautifyText(hostname)}";

            if (returncode != ChatConnectReturncode.ACCEPTED)
            {
                text = $"failed to connect: {BeautifyText(returncode.ToString())}";
            }

            _messages += $"<div class=\"system-message\">{text}</div>";

            ShowInfo();
        }

        public void ShowEnterChannelReturncode(ChatEnterChannelReturnCode returncode, string channel)
        {
            var text = $"enter channel {BeautifyText(channel)}";

            if (returncode != ChatEnterChannelReturnCode.Accepted)
            {
                text = $"failed to enter channel: {returncode.ToString()}";
            }

            _messages += $"<div class=\"system-message\">{text}</div>";

            ShowInfo();
        }
    }
}
