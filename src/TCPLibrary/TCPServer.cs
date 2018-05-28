using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ACX.ViciOne.TCPLibrary
{
    /// <summary>
    /// handles the low level TCP connection of MQTT
    /// </summary>
    public class TCPServer : ITCPServer
    {
        private List<TcpConnection> _connections = new List<TcpConnection>();
        private TcpListener _listener;
        private ThreadCancelationToken _acceptThreadCancelationToken;
        private Thread _acceptThread;
        private readonly IsPackageComplete _isPackageComplete;

        public IEnumerable<IPEndPoint> ConnectedClients => _connections.Select(c => c.Socket.RemoteEndPoint as IPEndPoint);

        public TCPServer(IsPackageComplete isPackageComplete)
        {
            ClientConnected += TCPServer_Clientconnected;

            _isPackageComplete = isPackageComplete;
        }

        private event TCPClientConnectedEventHandler ClientConnected;

        /// <summary>
        /// Gets thrown Whenever a message is Received on TCP Level
        /// </summary>
        public event TcpServerMessageReceivedEventHandler TCPMessagereceived;

        /// <summary>
        /// Starts the TCP Server
        /// </summary>
        public void ServerStart(int port)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            _listener.Start();

            var threadStart = new ParameterizedThreadStart(AcceptConnections);

            _acceptThreadCancelationToken = new ThreadCancelationToken();
            _acceptThread = new Thread(threadStart);

            _acceptThread.Start(_acceptThreadCancelationToken);
        }

        public void ServerStart() => ServerStart(1883);

        public void Send(IPEndPoint target, byte[] message)
        {
            if (!_connections.Any(c => c.Socket.RemoteEndPoint.Equals(target)))
            {
                throw new ArgumentException("target is not in ListOfConnections");
            }

            var client = _connections.Find(c => c.Socket.RemoteEndPoint.Equals(target));

            client.Send(message);
        }

        private void AcceptConnections(object token)
        {

            if (!(token is ThreadCancelationToken cancelationToken))
            {
                throw new ArgumentException($"token has to be {typeof(ThreadCancelationToken)}");
            }

            while (!cancelationToken.IsCancelationRequested)
            {
                SpinWait.SpinUntil(() => _listener.Pending() || cancelationToken.IsCancelationRequested);

                if (cancelationToken.IsCancelationRequested)
                {
                    continue;
                }

                var client = _listener.AcceptTcpClient();

                ClientConnected?.Invoke(this, new TCPClientConnectedEventArgs(client));
            }

            cancelationToken.Canceled();
        }

        public void DisconnectClient(IPEndPoint sender)
        {
            var connection = _connections.Find(c => (c.Socket.RemoteEndPoint as IPEndPoint).EqualsAddress(sender));

            if (connection == null)
            {
                throw new Exception($"cannot disconnect client: {sender}. connection not found");
            }

            connection.Dispose();

            _connections.Remove(connection);
        }

        private void TCPServer_Clientconnected(object sender, TCPClientConnectedEventArgs e)
        {
            _connections.Add(new TcpConnection(e.TcpClient, _isPackageComplete));

            _connections[_connections.Count - 1].TCPMessageReceived += TCPConnection_TCPMessageReceived;
        }

        private void TCPConnection_TCPMessageReceived(object sender, TcpMessageReceivedEventArgs e)
        {
            var connection = sender as TcpConnection;

            TCPMessagereceived?.Invoke(this, new TcpServerMessageReceivedEventArgs(e.Message, connection.Socket.RemoteEndPoint as IPEndPoint));
        }

        /// <summary>
        /// Stop the TCP Server
        /// </summary>
        public void Stop()
        {
            _acceptThreadCancelationToken.AwaitCancel(8000);

            _listener.Stop();
            _listener = null;

            KillAllConnections();
        }

        private void KillAllConnections()
        {
            while (_connections.Count > 0)
            {
                var connection = _connections[0];

                if (connection.IsAlive)
                {
                    connection.Dispose();
                }

                _connections.Remove(connection);
            }

            _connections = null;
        }
    }
}