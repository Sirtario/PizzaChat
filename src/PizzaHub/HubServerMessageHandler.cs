using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ACX.ViciOne.TCPLibrary;
using PIZZA.Core;
using PIZZA.Hub.Core;
using PIZZA.Hub.Core.PayLoads;
using PIZZA.Hub.Core.Enumerationen;
using PIZZA.Hub.Interface;

namespace PIZZA.Hub
{
    class HubServerMessageHandler
    {
        private HubPizzaServerList _serverlist;
        private int _serverMaxID = 1;
        private int _clientMaxID = 1;
        private TCPServer _server;
        private HubPizzaClientList _clientlist;

        private bool IsBitSet(byte b, int bit)
        { return (1 & (b >> bit)) == 1; }

        public HubServerMessageHandler(TCPServer server, HubPizzaServerList serverlist, HubPizzaClientList clientlist)
        {
            _server = server;
            _server.TCPMessagereceived += RecievedMessageEventHandler;
            _serverlist = serverlist;
            _clientlist = clientlist;
        }

        private void RecievedMessageEventHandler(object sender, TcpServerMessageReceivedEventArgs e)
        {
            HubMessage message = HubMessageFactory.GetMessage(e.Message);

            switch(message.Header.PacketType)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    {
                       // HubMessage inmessage = HubMessageFactory.GetMessage(e.Message);
                        byte[] payload;
                        HubEnlistAckPayLoad pl;
                        HubMessage anser;

                        PIZZAString ServerAdress = PIZZAString.FromBytes(message.PayLoad.GetBytes());

                        if (ServerAdress.Value == String.Empty)
                        {
                            ServerAdress.Value = e.Sender.ToString();
                            HubTerminal.Cout(ConsoleColor.Yellow, $"[Hub Info] Hostname was empty using EndPoint: {e.Sender.ToString()} as Hostname...");
                        }

                        if (!_serverlist.ContainsHostname(ServerAdress))
                        {
                            if (_serverlist.ContainsServerID(_serverMaxID))
                                _serverMaxID++;

                                _serverlist.AddServer(_serverMaxID, ServerAdress);
                            

                            payload = new byte[] { (byte)HubReturnCodes.ACCEPTED };
                            payload = payload.Concat(new byte[] { 20 }).Concat(new byte[] { (byte)_serverMaxID }).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);

                            HubTerminal.Cout(ConsoleColor.Green, $"[Server Added] Server with EndPoint {e.Sender} has been added as ID: { _serverMaxID } Hostname: { ServerAdress }");

                            _serverMaxID++;
                        }
                        else
                        {
                            payload = new byte[] { (byte)HubReturnCodes.DENIED_ALREDY_LISTED };
                            payload = payload.Concat(new byte[] { 0 }).Concat(new byte[] { 0 }).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);

                            HubTerminal.Cout(ConsoleColor.Red, $"Server with Hostname: { ServerAdress } could not be added because it is alredy listed...");
                        }

                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
                case HubPacketTypes.CLIENTENLISTREQ:
                    {

                        byte[] payload;
                        HubEnlistAckPayLoad pl;
                        HubMessage anser;

                        PIZZAString ClientAddress = PIZZAString.FromBytes(e.Message);

                        if (ClientAddress.Value == String.Empty)
                            ClientAddress.Value = e.Sender.ToString();

                        if (!_clientlist.ContainsHostname(ClientAddress))
                        {
                            if (_clientlist.ContainsClientID(_clientMaxID))
                            _clientMaxID++;
 
                            _clientlist.AddClient(_clientMaxID, ClientAddress);

                            payload = new byte[] { (byte)HubReturnCodes.ACCEPTED };
                            payload = payload.Concat(new byte[] { 20 }).Concat(new byte[] {(byte)_clientMaxID}).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);
                        }
                        else
                        {
                            payload = new byte[] { (byte)HubReturnCodes.DENIED_ALREDY_LISTED };
                            payload = payload.Concat(new byte[] { 0 }).Concat(new byte[] { 0 }).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);
                        }


                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
               // case HubPacketTypes.ENLISTACK:
                 //   break;
                case HubPacketTypes.HOSTLISTREQ:
                    {
                        byte flags;
                        byte[] payload;
                        HubMessage anser;

                        flags = message.PayLoad.GetBytes()[0];



                    }
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
                    break;
                case HubPacketTypes.PINGACK:
                    break;
                default:
                    throw new NotImplementedException($"The packet type {message.Header.PacketType} is unknown...");
            }
        }


    }
}
