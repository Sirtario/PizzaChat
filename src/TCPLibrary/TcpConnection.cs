using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ACX.ViciOne.TCPLibrary
{
    public delegate bool IsPackageComplete(byte[] currentBytes);

    /// <summary>
    /// Holds Data relevant to a Connection from a client.
    /// </summary>
    public class TcpConnection : IDisposable
    {
        public event TcpMessageReceivedEventHandler TCPMessageReceived;

        private ThreadCancelationToken _cancelationToken;
        private Thread _listeningThread;
        private NetworkStream _stream;
        private TcpClient _client;
        private IsPackageComplete _isPackageComplete;

        public TcpConnection(IsPackageComplete isPackageComplete)
        {
            _isPackageComplete = isPackageComplete;
        }

        public TcpConnection(TcpClient tcpClient, IsPackageComplete isPackageComplete) : this(isPackageComplete)
        {
            _client = tcpClient;
            _stream = _client.GetStream();

            Start();
        }

        public bool IsAlive => _listeningThread != null && _listeningThread.IsAlive;

        public Socket Socket => _client.Client;

        public void Stop()
        {
            _cancelationToken.Cancel();
        }

        public void AwaitStop(int timeout)
        {
            _cancelationToken.AwaitCancel(timeout);
        }

        private void Start()
        {
            if (IsAlive)
            {
                throw new InvalidOperationException("cannot start; already runnning");
            }

            var parameterizedThreadStart = new ParameterizedThreadStart(ListenThread);

            _cancelationToken = new ThreadCancelationToken();
            _listeningThread = new Thread(parameterizedThreadStart);

            _listeningThread.Start(_cancelationToken);
        }

        private void ListenThread(object token)
        {
            if (!(token is ThreadCancelationToken cancelationToken))
            {
                throw new ArgumentException($"token has to be of type {typeof(ThreadCancelationToken)}");
            }

            while (!cancelationToken.IsCancelationRequested)
            {
                SpinWait.SpinUntil(() => _stream.DataAvailable || cancelationToken.IsCancelationRequested);

                var buffer = new List<byte>();

                while (_stream.DataAvailable && !_isPackageComplete(buffer.ToArray()))
                {
                    var nextByte = (byte)_stream.ReadByte();

                    buffer.Add(nextByte);
                }

                if (buffer.Count != 0)
                {
                    new Task(new Action(() => TCPMessageReceived?.Invoke(this, new TcpMessageReceivedEventArgs(buffer.ToArray())))).Start();
                }
            }

            cancelationToken.Canceled();
        }

        /// <summary>
        /// Disconnects the Connection And Releases all resources 
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        /// <summary>
        /// Connects to a Host
        /// </summary>
        /// <param name="HostName"></param>
        /// <param name="port"></param>
        public void Connect(string HostName, int port)
        {
            _client = new TcpClient(HostName, port);

            if (_client.Connected)
            {
                //Start Listening Threat
                _stream = _client.GetStream();

                Start();
            }
        }

        /// <summary>
        /// Closes the connection Releases all resources used by the connection
        /// </summary>
        public void Disconnect()
        {
            _cancelationToken.AwaitCancel(500);

            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
            _client?.Close();
            _client?.Dispose();
            _client = null;
        }

        public void Send(byte[] message)
        {
            _stream?.Write(message, 0, message.Length);
        }
    }
}