using System.Net;

namespace ACX.ViciOne.TCPLibrary
{
    public class TCPClient
    {
        private TcpConnection _connection;

        private delegate void Listen();

        /// <summary>
        /// Gets thrown when a Message is received on TCP Level
        /// </summary>
        public event TcpMessageReceivedEventHandler TCPMessageReceived;

        public TCPClient(IsPackageComplete isPackageComplete)
        {
            _connection = new TcpConnection(isPackageComplete);

            _connection.TCPMessageReceived += Connection_TCPMessageReceived;
        }

        public IPEndPoint RemoteEndpoint => _connection.Socket.RemoteEndPoint as IPEndPoint;

        public IPEndPoint LocalEndpoint => _connection.Socket.LocalEndPoint as IPEndPoint;

        public bool IsAlive => _connection.IsAlive;

        private void Connection_TCPMessageReceived(object sender, TcpMessageReceivedEventArgs e)
        {
            TCPMessageReceived?.Invoke(this, new TcpMessageReceivedEventArgs(e.Message));
        }

        /// <summary>
        /// connects the TCPClient To the TCP Server
        /// </summary>
        /// <param name="hostName">Servername</param>
        public void Connect(string hostName, int port)
        {
            _connection.Connect(hostName, port);
        }

        public void Connect(string hostname) { Connect(hostname, 1883); }

        /// <summary>
        /// Disconnects The TCPClient from the server 
        /// </summary>
        public void Disconnect()
        {
            _connection.Disconnect();
        }

        /// <summary>
        /// Send the specified Byte[] to the server
        /// </summary>
        /// <param name="buffer">byte[] to be sent to the server</param>
        public void Send(byte[] message)
        {
            _connection.Send(message);
        }
    }
}