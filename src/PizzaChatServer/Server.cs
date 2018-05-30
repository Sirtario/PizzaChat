using ACX.ViciOne.TCPLibrary;
using PIZZA.Chat.Core;
using System;

namespace PIZZA.Chat.Server
{
    public class Server
    {
        private ITCPServer _tcpServer;

        public Server()
        {
        }

        public Server(ITCPServer server)
        {
            _tcpServer = server ?? throw new ArgumentNullException();

            _tcpServer.TCPMessagereceived += _tcpServer_TCPMessagereceived;
        }

        private void _tcpServer_TCPMessagereceived(object sender, TcpServerMessageReceivedEventArgs e)
        {
            var message = PizzaChatMessage.FromBytes(e.Message);
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
