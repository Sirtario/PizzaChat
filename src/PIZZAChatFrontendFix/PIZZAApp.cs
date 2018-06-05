using ACX.ViciOne.TCPLibrary;
using PIZZA.Hub.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIZZA.Hub.Core.PayLoads;
using PIZZA.ChatCore;
using PIZZA.Chat.Core;
using System.Timers;
using PIZZA.Hub.Client;

namespace PIZZA.Client
{
    public class PIZZAApp
    {
        private TCPClient _tcpClientChat;
        private HubClient _hubClient;

        private Timer _pingTimer;

        private List<Tuple<string, string, string, bool>> _servers;
        private IPIZZAFrontend _frontend;
        private PIZZAChannel _enteringChannel;
        private int _hostIndex;

        public void Prepare(IPIZZAFrontend frontend)
        {
            frontend.Connect += Frontend_Connect;
            frontend.Disconnect += Frontend_Disconnect;
            frontend.EnterRoom += Frontend_EnterRoom;
            frontend.GetServers += Frontend_GetServers;
            frontend.SendMessage += Frontend_SendMessage;
            frontend.WhisperMessage += Frontend_WhisperMessage;
            frontend.ConnectDirectly += Frontend_ConnectDirectly;

            _frontend = frontend;
        }

        private void Frontend_ConnectDirectly(string obj)
        {
            _servers.Clear();
            _servers.Add(new Tuple<string, string, string, bool>(obj, obj, obj, false));

            Frontend_Connect(0);
        }

        private void Frontend_WhisperMessage(string arg1, string arg2)
        {
            var message = GetPublishMessage(arg1);
            var varheader = message.VariableHeader as ChatVarHeaderPublish;

            varheader.WhisperTarget = arg2;

            _tcpClientChat.Send(message.GetBytes());
        }

        private PizzaChatMessage GetPublishMessage(string str)
        {
            var message = new PizzaChatMessage(Packettypes.PUBLISH);
            var varheader = message.VariableHeader as ChatVarHeaderPublish;
            var payload = message.Payload as ChatPayloadPublish;

            varheader.Datatype = ChatPayloadDatatypes.Text;
            payload.Payload = Encoding.UTF8.GetBytes(str);

            return message;
        }

        private void Frontend_SendMessage(string obj)
        {
            var message = GetPublishMessage(obj);

            _tcpClientChat.Send(message.GetBytes());
        }

        private void Frontend_GetServers()
        {
            if(_hubClient != null)
            {
                _hubClient.Stop();
            }

            _hubClient = new HubClient();

            _hubClient.Connect(_frontend.HubHostname, _frontend.HubPort);
        }

        private void ShowServerlist(HubHostlistDatPayLoad hubHostlistDatPayLoad)
        {
            var serverList = hubHostlistDatPayLoad.Hosts.Select(h => new Tuple<string, string, string, bool>(h.Friendlyname, h.Description, h.Hostname, h.RequiresPassword));

            _frontend.ShowServerlist(serverList.ToList());
        }

        private void Frontend_EnterRoom(PIZZAChannel obj)
        {
            var message = new PizzaChatMessage(Packettypes.ENTERCHANNEL);
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannel;

            varheader.Channel = obj.Channelname.Value;
           
            if(obj.HasPassword >= 1)
            {
                varheader.Password = _frontend.GetPassword(obj.Channelname.Value);
            }

            _enteringChannel = obj;

            _tcpClientChat.Send(message.GetBytes());
        }

        private void Frontend_Disconnect()
        {
            var message = new PizzaChatMessage(Packettypes.DISCONNECT);

            _tcpClientChat.Send(message.GetBytes());
        }

        private void Frontend_Connect(int obj)
        {
            if(_servers == null)
            {
                return;
            }

            _hostIndex = obj;

            var hostname = _servers[obj].Item3;

            InitChatTcpConnection(hostname);

            var message = new PizzaChatMessage(Packettypes.CONNECT);
            var varheader = message.VariableHeader as ChatVarHeaderConnect;

            varheader.ClientID = _frontend.GetClientId(_servers[_hostIndex].Item1);

            if(_servers[obj].Item4)
            {
                varheader.Password = _frontend.GetPassword(_servers[obj].Item1);
            }

            _tcpClientChat.Send(message.GetBytes());
        }

        private void InitChatTcpConnection(string hostname)
        {
            if (_tcpClientChat == null)
            {
                _tcpClientChat = new TCPClient(Chat.Core.TcpDelegate.IsPIZZAChatMessageComplete);
            }
            
            _tcpClientChat.TCPMessageReceived += TcpClientChat_TCPMessageReceived;

            var port = ChatCore.Constants.DefaultPort;

            if (hostname.Contains(':'))
            {
                var hostnameSplit = hostname.Split(':');

                hostname = hostnameSplit[0];

                if (int.TryParse(hostnameSplit[1], out var hostnamePort))
                {
                    port = hostnamePort;
                }
            }

            _tcpClientChat.Connect(hostname, port);
        }

        private void TcpClientChat_TCPMessageReceived(object sender, TcpMessageReceivedEventArgs e)
        {
            var message = PizzaChatMessage.FromBytes(e.Message);

            switch (message.FixedHeader.PacketType)
            {
                //case Packettypes.CONNECT:
                //    break;
                case Packettypes.CONNACK:
                    ConnectionAccepted(message);
                    break;
                //case Packettypes.GETSTATUS:
                //    break;
                case Packettypes.STATUS:
                    ReceiveStatus(message);
                    break;
                //case Packettypes.PING:
                //    break;
                case Packettypes.PINGRESP:
                    break;
                //case Packettypes.ENTERCHANNEL:
                //    break;
                case Packettypes.ENTERCHANNELACK:
                    ReceiveEnterChannelAck(message);
                    break;
                //case Packettypes.DISCONNECT:
                //    break;
                //case Packettypes.PUBLISH:
                //    break;
                default:
                    break;
            }
        }

        private void ReceiveStatus(PizzaChatMessage message)
        {
            var varheader = message.VariableHeader as ChatVarHeaderStatus;
            var payload = message.Payload as ChatPayloadStatus;

            _frontend.RefreshStatus(payload.ClientsInCurrentChannel.ToList(), payload.Channels.ToList(), varheader.CurrentChannel, _servers[_hostIndex].Item1);
        }

        private void ReceiveEnterChannelAck(PizzaChatMessage message)
        {
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannelAck;

            _frontend.ShowEnterChannelReturncode(varheader.ReturnCode, _enteringChannel.Channelname.Value);
        }

        private void ConnectionAccepted(PizzaChatMessage message)
        {
            var varheader = message.VariableHeader as ChatVarHeaderConnAck;

            _frontend.ShowReturncode(varheader.Returncode, _servers[_hostIndex].Item1);

            if(varheader.Returncode != ChatConnectReturncode.ACCEPTED)
            {
                return;
            }

            if((_pingTimer?.Enabled).GetValueOrDefault())
            {
                _pingTimer.Stop();
            }

            if(varheader.PingIntervall == 0)
            {
                return;
            }

            _pingTimer = new Timer();

            _pingTimer.Interval = varheader.PingIntervall * 1000;

            _pingTimer.Elapsed += PingTimer_Elapsed;

            _pingTimer.Start();
        }

        private void PingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var message = new PizzaChatMessage(Packettypes.PING);

            _tcpClientChat.Send(message.GetBytes());
        }
    }
}
