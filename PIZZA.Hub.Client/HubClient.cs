using ACX.ViciOne.TCPLibrary;
using PIZZA.Hub.Core;
using PIZZA.Hub.Core.PayLoads;
using System;
using System.Threading;

namespace PIZZA.Hub.Client
{
    public class HubClient
    {
        private TcpConnection _tcpConnection;

        private ManualResetEventSlim _receivedHostlistDat;

        public void Connect(string host, int port)
        {
            _tcpConnection?.Disconnect();

            _tcpConnection = new TcpConnection(TcpDelegate.IsPIZZAHubMessageComplete);

            _tcpConnection.TCPMessageReceived += TcpConnection_TCPMessageReceived;

            _tcpConnection.Connect(host, port);
        }

        private void TcpConnection_TCPMessageReceived(object sender, TcpMessageReceivedEventArgs e)
        {
            var message = HubMessageFactory.GetMessage(e.Message);

            switch (message.Header.PacketType)
            {
                //case HubPacketTypes.SERVERENLISTREQ:
                //    break;
                //case HubPacketTypes.CLIENTENLISTREQ:
                //    break;
                case HubPacketTypes.ENLISTACK:
                    break;
                //case HubPacketTypes.HOSTLISTREQ:
                //    break;
                case HubPacketTypes.HOSTLISTDAT:
                    break;
                //case HubPacketTypes.HOSTAVAILABLEREQ:
                //    break;
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    break;
                //case HubPacketTypes.UNLISTREQ:
                //    break;
                case HubPacketTypes.UNLISTTACK:
                    break;
                //case HubPacketTypes.PING:
                //    break;
                case HubPacketTypes.PINGACK:
                    break;
                default:
                    break;
            }
        }

        public void GetHostlist(bool getServers, bool getClients, bool ignorePasswordProtected)
        {
            var message = HubMessageFactory.GetMessage(HubPacketTypes.HOSTLISTREQ);
            var payload = message.PayLoad as HubHostlistReqPayLoad;

            payload.IgnoreRequiredPassword = ignorePasswordProtected;
            payload.RequestClients = getClients;
            payload.RequestServers = getServers;

            _receivedHostlistDat = new ManualResetEventSlim();

            _tcpConnection.Send(message.GetBytes());

            _receivedHostlistDat.Wait(50);
        }
    }
}
