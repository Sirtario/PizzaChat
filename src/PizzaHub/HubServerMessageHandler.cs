using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ACX.ViciOne.TCPLibrary;
using PIZZA.Core;
using PIZZA.Hub.Core;

namespace PIZZA.Hub
{
    class HubServerMessageHandler
    {
        private HubPizzaServerList _slist;
        private int _serverMaxID = 1;
        private TCPServer _server;

        public HubServerMessageHandler(TCPServer server, HubPizzaServerList slist)
        {
            _server = server;
            _server.TCPMessagereceived += RecievedMessageEventHandler;
            _slist = slist;
        }

        private void RecievedMessageEventHandler(object sender, TcpServerMessageReceivedEventArgs e)
        {
            HubMessage message = HubMessageFactory.GetMessage(e.Message);

            switch(message.Header.PacketType)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    break;
                case HubPacketTypes.CLIENTENLISTREQ:
                    break;
                case HubPacketTypes.ENLISTACK:
                    break;
                case HubPacketTypes.HOSTLISTREQ:
                    break;
                case HubPacketTypes.HOSTLISTDAT:
                    break;
                case HubPacketTypes.HOSTAVAILABLEREQ:
                    break;
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    break;
                case HubPacketTypes.UNLISTREQ:
                    break;
                case HubPacketTypes.UNLISTTACK:
                    break;
                case HubPacketTypes.PING:

                    PIZZAString ServerAdress = PIZZAString.FromBytes(message.PayLoad.GetBytes());

                    if (!_slist.ContainsEndPoint(ServerAdress))
                    {
                        if (!_slist.ContainsServerID(_serverMaxID))
                        _slist.AddServer(_serverMaxID, ServerAdress);
                        else
                        {
                            _serverMaxID++;
                            _slist.AddServer(_serverMaxID, ServerAdress);
                        }
                        _server.Send(e.Sender, HubMessageFactory.GetMessage(HubPacketTypes.PINGACK).GetBytes());

                        _serverMaxID++;
                    }

                    break;
                case HubPacketTypes.PINGACK:
                    break;
                default:
                    throw new NotImplementedException($"The packet type {message.Header.PacketType} is unknown...");
            }
        }


    }
}
