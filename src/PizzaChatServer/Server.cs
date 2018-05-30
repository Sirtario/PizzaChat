using ACX.ViciOne.TCPLibrary;
using PIZZA.Chat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PIZZA.Chat.Server
{
    public class Server :IDisposable
    {
        private ITCPServer _tcpServer;
        private ConnectManager _connectManager;
        private PublishManager _publishManager;
        private ChannelManager _channelmanager;
        private StatusManager _statusManager;

        public event ChatClientConnectionApprovalEventHandler ClientConnecting;

        public Server(ITCPServer server)
        {
            _tcpServer = server ?? throw new ArgumentNullException();

            _tcpServer.TCPMessagereceived += _tcpServer_TCPMessagereceived;
            _connectManager = new ConnectManager();
            _connectManager.ClientConnected += _connectManager_ClientConnected;
            _connectManager.SendMessage += SendMessage;
            _connectManager.ClientConnectionApprove += _connectManager_ClientConnectionApprove;

            _statusManager.SendMessage += SendMessage;
        }


        /// <summary>
        /// conntains all PIZZAChat Connections
        /// </summary>
        public List<ChatClientConnection> Connections { get; private set; }
       
        /// <summary>
        /// The Channel All new clients connect to
        /// </summary>
        public string DefaultChannel { get; set; }
       
        /// <summary>
        /// List of all Channels
        /// </summary>
        public List<PIZZAChannel> Channels { get; set; }

        //passes the Appoval event through
        private void _connectManager_ClientConnectionApprove(object sender, ChatConnectApprovalEventArgs e)
        {
            ClientConnecting.Invoke(this, e);
        }

        private void _connectManager_ClientConnected(ChatClientConnection obj)
        {
            obj.CourentChannel = DefaultChannel;
            obj.PingTimer.Elapsed += PingTimer_Elapsed;
            Connections.Add(obj);

            _statusManager.SendStatus(obj, Connections.FindAll(c=>c.CourentChannel== obj.CourentChannel),Channels );
        }

        private void PingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _tcpServer.DisconnectClient((sender as ChatClientConnection).ClientIP);
        }

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

            // check message header
            if (message.FixedHeader.Protokollname != Encoding.UTF8.GetBytes("PIZZAC")) 
            {
                throw new Exception("False Protokoll or malformed Packet!");
            }

            if (message.FixedHeader.ProtokollVersion != 1)
            {
                throw new Exception("False Protokoll or malformed Packet!");
            }

            if (message.FixedHeader.RemainingLength != message.GetBytes().Length -13)
            {
                throw new Exception("False Protokoll or malformed Packet!");
            }

            //dipatches the message to the managers 
            switch (message.FixedHeader.PacketType)
            {
                case Packettypes.CONNECT:
                    _connectManager.RecieveConnect(message, e.Sender);
                    break;
                case Packettypes.GETSTATUS:

                    var myconnection = Connections.Find(prop => prop.ClientIP == e.Sender);
                    var usersInMyChannel = Connections.FindAll(prop => prop.CourentChannel == myconnection.CourentChannel);

                    _statusManager.RecieveGetStatus( myconnection, usersInMyChannel, Channels);
                    break;
                case Packettypes.PING:
                    ResetPingTimer(Connections.Find(prop => prop.ClientIP == e.Sender));
                    SendPingRESP(Connections.Find(prop => prop.ClientIP == e.Sender).ClientIP);
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

        private void SendPingRESP(IPEndPoint clientIP)
        {
            _tcpServer.Send(clientIP,new PizzaChatMessage(Packettypes.PINGRESP).GetBytes());
        }

        private void ResetPingTimer(ChatClientConnection chatClientConnection)
        {
            chatClientConnection.PingTimer.Stop();
            chatClientConnection.PingTimer.Start();
        }

        private void SendMessage(PizzaChatMessage message, IPEndPoint endPoint)
        {
            _tcpServer.Send(endPoint, message.GetBytes());
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
