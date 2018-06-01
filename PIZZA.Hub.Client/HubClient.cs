using ACX.ViciOne.TCPLibrary;
using System;

namespace PIZZA.Hub.Client
{
    public class HubClient
    {
        private TcpConnection _tcpConnection;

        public void Connect(string host, int port)
        {
            _tcpConnection?.Disconnect();

            _tcpConnection = new TcpConnection()
        }
    }
}
