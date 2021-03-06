using ACX.ViciOne.TCPLibrary;
using PIZZA.Chat.Core;
using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PIZZA.Chat.Server
{
    public class ServerLib : IDisposable, IServer
    {
        private ITCPServer _tcpServer;
        private ConnectManager _connectManager;
        private PublishManager _publishManager;
        private ChannelManager _channelmanager;
        private StatusManager _statusManager;

        public event ChatClientConnectionApprovalEventHandler ClientConnecting;
        public event ChatClientEnteringChannelEventHandler ClientEnteringChannel;

        public ServerLib(ITCPServer server)
        {
            _tcpServer = server ?? throw new ArgumentNullException();

            DefaultChannel = "home";
            Connections = new List<ChatClientConnection>();
            Channels = new List<PIZZAChannel>();
            Channels.Add(new PIZZAChannel(new PIZZA.Core.PIZZAString() { Value = "home" }, 0));

            _tcpServer.TCPMessagereceived += _tcpServer_TCPMessagereceived;
            _connectManager = new ConnectManager();
            _connectManager.ClientConnected += _connectManager_ClientConnected;
            _connectManager.SendMessage += SendMessage;
            _connectManager.ClientConnectionApprove += _connectManager_ClientConnectionApprove;

            _statusManager = new StatusManager();
            _statusManager.SendMessage += SendMessage;

            _publishManager = new PublishManager();
            _publishManager.SendMessage += SendMessage;

            _channelmanager = new ChannelManager();
            _channelmanager.SendMessage += SendMessage;
            _channelmanager.ClientEnteringChannel += _channelmanager_ClientEnteringChannel;
            _channelmanager.ChannelChanged += _channelmanager_ChannelChanged;
        }

        private void _channelmanager_ChannelChanged(string arg1, string arg2)
        {
            var connectionsInArg1 = Connections.FindAll(c => c.CourentChannel == arg1);

            foreach (var connection in Connections.FindAll(c => c.CourentChannel == arg1))
            {
                _statusManager.SendStatus(connection, connectionsInArg1, Channels);
            }

            var connectionsInArg2 = Connections.FindAll(c => c.CourentChannel == arg2);

            foreach (var connection in Connections.FindAll(c => c.CourentChannel == arg2))
            {
                _statusManager.SendStatus(connection, connectionsInArg2, Channels);
            }
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

        private void _channelmanager_ClientEnteringChannel(object sender, ChatClientEnteringChannelEventArgs e)
        {
            ClientEnteringChannel.Invoke(this, e);
        }

        //passes the Appoval event through
        private void _connectManager_ClientConnectionApprove(object sender, ChatConnectApprovalEventArgs e)
        {
            ClientConnecting.Invoke(this, e);
        }

        private void _connectManager_ClientConnected(ChatClientConnection obj)

        {
            obj.CourentChannel = DefaultChannel;
            //obj.PingTimer.Elapsed += (s,e) =>{
            //    if (_tcpServer.ConnectedClients.Any(c => c == obj.ClientIP))
            //    {
            //        Console.WriteLine($"{obj.ClientID} got disconnected because of Ping");
            //        _tcpServer.DisconnectClient(obj.ClientIP);
            //    }
            //};
            Connections.Add(obj);

            _statusManager.SendStatus(obj, Connections.FindAll(c=>c.CourentChannel== obj.CourentChannel),Channels );
        }

        //does the main logic
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
            if (!message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }) )
            {
                throw new Exception("False Protokoll or malformed Packet!");
            }

            if (message.FixedHeader.ProtokollVersion != 1)
            {
                throw new Exception("False Protokoll or malformed Packet!");
            }

            var myconnection = Connections.Find(prop => prop.ClientIP == e.Sender);
            
            //dispatches the message to the managers 
            switch (message.FixedHeader.PacketType)
            {
                case Packettypes.CONNECT:
                    _connectManager.RecieveConnect(message, e.Sender);
                    break;
                case Packettypes.GETSTATUS:
                    var usersInMyChannel = Connections.FindAll(prop => prop.CourentChannel == myconnection.CourentChannel);
                    _statusManager.RecieveGetStatus( myconnection, usersInMyChannel, Channels);
                    break;
                case Packettypes.PING:
                    //ResetPingTimer(Connections.Find(prop => prop.ClientIP == e.Sender));
                    SendPingRESP(Connections.Find(prop => prop.ClientIP == e.Sender).ClientIP);
                    break;
                case Packettypes.ENTERCHANNEL:
                    _channelmanager.RecieveEnterChannel(message, myconnection, Channels);
                    break;
                case Packettypes.DISCONNECT:
                    (Connections.Find(prop => prop.ClientIP == e.Sender)).PingTimer.Dispose();

                    Connections.Remove(Connections.Find(prop => prop.ClientIP == e.Sender));
                    _tcpServer.DisconnectClient(e.Sender);
                    break;
                case Packettypes.PUBLISH:
                    usersInMyChannel = Connections.FindAll(prop => prop.CourentChannel == myconnection.CourentChannel);
                    _publishManager.Publish(message,myconnection,usersInMyChannel);
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
        /// Starts the server on Default port
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
