using ACX.ViciOne.TCPLibrary;
using PIZZA.Hub.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIZZA.Hub.Core.PayLoads;

namespace PIZZA.Client
{
    public class PIZZAApp
    {
        private TCPClient _tcpClientChat;
        private TCPClient _tcpClientHub;

        public string HubHostName { get; set; }

        private List<Tuple<string, string, string>> _servers;
        private IPIZZAFrontend _frontend;

        public void Prepare(IPIZZAFrontend frontend)
        {
            frontend.Connect += Frontend_Connect;
            frontend.Disconnect += Frontend_Disconnect;
            frontend.EnterRoom += Frontend_EnterRoom;
            frontend.GetServers += Frontend_GetServers;
            frontend.SendMessage += Frontend_SendMessage;
            frontend.WhisperMessage += Frontend_WhisperMessage;

            _frontend = frontend;
        }

        private void Frontend_WhisperMessage(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        private void Frontend_SendMessage(string obj)
        {
            throw new NotImplementedException();
        }

        private void Frontend_GetServers()
        {
            if(_tcpClientHub == null || !_tcpClientHub.IsAlive)
            {
                _tcpClientHub = new TCPClient(TcpDelegate.IsPIZZAHubMessageComplete);
            }

            _tcpClientHub.TCPMessageReceived += _tcpClientHub_TCPMessageReceived;

            _tcpClientHub.Connect(HubHostName);

            var message = HubMessageFactory.GetMessage(HubPacketTypes.HOSTLISTREQ);

            _tcpClientHub.Send(message.GetBytes());
        }

        private void _tcpClientHub_TCPMessageReceived(object sender, TcpMessageReceivedEventArgs e)
        {
            var message = HubMessageFactory.GetMessage(e.Message);

            switch (message.Header.PacketType)
            {
                //case HubPacketTypes.SERVERENLISTREQ:
                //    break;
                //case HubPacketTypes.CLIENTENLISTREQ:
                //    break;
                //case HubPacketTypes.ENLISTACK:
                //    break;
                //case HubPacketTypes.HOSTLISTREQ:
                //    break;
                case HubPacketTypes.HOSTLISTDAT:
                    ShowServerlist(message.PayLoad as HubHostlistDatPayLoad);
                    break;
                //case HubPacketTypes.HOSTAVAILABLEREQ:
                //    break;
                //case HubPacketTypes.HOSTAVAILABLEDAT:
                //    break;
                //case HubPacketTypes.UNLISTREQ:
                //    break;
                //case HubPacketTypes.UNLISTTACK:
                //    break;
                //case HubPacketTypes.PING:
                //    break;
                //case HubPacketTypes.PINGACK:
                //    break;
                //default:
                //    break;
            }
        }

        private void ShowServerlist(HubHostlistDatPayLoad hubHostlistDatPayLoad)
        {
            var serverList = hubHostlistDatPayLoad.Hosts.Select(h => new Tuple<string, string, string>(h.Friendlyname, h.Description, h.Hostname));

            _frontend.ShowServerlist(serverList.ToList());
        }

        private void Frontend_EnterRoom(string obj)
        {
            throw new NotImplementedException();
        }

        private void Frontend_Disconnect()
        {
            throw new NotImplementedException();
        }

        private void Frontend_Connect(int obj)
        {
            if(_servers == null)
            {
                return;
            }
        }
    }
}
