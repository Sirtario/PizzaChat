using ACX.ViciOne.TCPLibrary;
using PIZZA.Chat.Core;
using System;
using System.Collections.Generic;

namespace PIZZA.Chat.Server
{
    public class Server :IDisposable
    {
        private ITCPServer _tcpServer;
        private ConnectManager _connectManager;
        private PublishManager _publishManager;
        private ChannelManager _channelmanager;
        private StatusManager _statusManager;

        public Server(ITCPServer server)
        {
            _tcpServer = server ?? throw new ArgumentNullException();

            _tcpServer.TCPMessagereceived += _tcpServer_TCPMessagereceived;
            _connectManager = new ConnectManager();
            _connectManager.ClientConnected += _connectManager_ClientConnected;
            _connectManager.SendMessage += SendMessage;
        }

        private void _connectManager_ClientConnected(ChatClientConnection obj)
        {
            _statusManager.SendInitialStatus();
        }

        /// <summary>
        /// conntains all PIZZAChat Connections
        /// </summary>
        public List<ChatClientConnection> Connections { get; private set; }

        private void _tcpServer_TCPMessagereceived(object sender, TcpServerMessageReceivedEventArgs e)
        {
            PizzaChatMessage message;

            try
            {
                message = PizzaChatMessage.FromBytes(e.Message);
            }
            catch (Exception)
            {

                throw;
            }

            //dipatches the message to the managers 
            switch (message.FixedHeader.PacketType)
            {
                case Packettypes.CONNECT:
                    _connectManager.RecieveConnect(message, e.Sender);
                    break;
                case Packettypes.GETSTATUS:
                    _statusManager.RecieveGetStatus(message, e.Sender);
                    break;
                case Packettypes.PING:
                    _connectManager.ReceivePing(message, e.Sender);
                    break;
                case Packettypes.ENTERCHANNEL:
                    _channelmanager.RecieveEnterChannel(message, e.Sender);
                    break;
                case Packettypes.DISCONNECT:
                    _connectManager.ReceiveDisconnect(message,e.Sender);
                    break;
                case Packettypes.PUBLISH:
                    _publishManager.Publish(message);
                    break;
                default:
                    throw new NotSupportedException($"the Packet type is not supportet as recieving packet on the server {message.FixedHeader.PacketType}");
            }


        }

        private void SendMessage(PizzaChatMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the server on Default port (6666)
        /// </summary>
        public void Start()
        {
            _tcpServer.ServerStart(6666);
        }

        /// <summary>
        /// starts Server on specified Port
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {
            _tcpServer.ServerStart(port);
        }

        /// <summary>
        /// stops server
        /// </summary>
        public void Stop()
        {
            _tcpServer.Stop();
        }

        /// <summary>
        /// Stops server and releases all recources
        /// </summary>
        public void Dispose()
        {
            Stop();

            //events abhängen und alles nullen oder disposen
        }
    }
}
