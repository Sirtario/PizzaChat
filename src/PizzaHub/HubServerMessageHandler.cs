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

                        HubHostInfo ServerInfo = HubHostInfo.FromBytes(message.PayLoad.GetBytes());

                        if (ServerInfo.Hostname.Value == String.Empty)
                        {
                            ServerInfo.Hostname.Value = e.Sender.ToString();
                            HubTerminal.Cout(ConsoleColor.Yellow, $"[Hub Info] Hostname was empty using EndPoint: {e.Sender.ToString()} as Hostname...");
                        }

                        if (!_serverlist.ContainsHostInfo(ServerInfo))
                        {
                            if (_serverlist.ContainsServerID(_serverMaxID))
                                _serverMaxID++;

                                _serverlist.AddServer(_serverMaxID, ServerInfo);
                            

                            payload = new byte[] { (byte)HubReturnCodes.ACCEPTED };
                            payload = payload.Concat(new byte[] { 20 }).Concat(new byte[] { (byte)_serverMaxID }).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);

                            HubTerminal.Cout(ConsoleColor.Green, $"[Server Added] Server with EndPoint {e.Sender} has been added as ID: { _serverMaxID } Hostname: { ServerInfo }");

                            _serverMaxID++;
                        }
                        else
                        {
                            payload = new byte[] { (byte)HubReturnCodes.DENIED_ALREDY_LISTED };
                            payload = payload.Concat(new byte[] { 0 }).Concat(new byte[] { 0 }).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);

                            HubTerminal.Cout(ConsoleColor.Red, $"Server with Hostname: { ServerInfo } could not be added because it is alredy listed...");
                        }

                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
                case HubPacketTypes.CLIENTENLISTREQ:
                    {

                        byte[] payload;
                        HubEnlistAckPayLoad pl;
                        HubMessage anser;

                        HubHostInfo ClientAddress = HubHostInfo.FromBytes(e.Message);

                        if (ClientAddress.Hostname.Value == String.Empty)
                            ClientAddress.Hostname.Value = e.Sender.ToString();

                        if (!_clientlist.ContainsHostInfo(ClientAddress))
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
                        byte[] payload = new byte[0];
                        HubMessage anser;
                        HubHostlistDatPayLoad pl;

                        flags = message.PayLoad.GetBytes()[0];

                        if (IsBitSet(flags, 1))
                        {
                            
                            if (IsBitSet(flags, 0) && !IsBitSet(flags, 2))
                            {
                                try
                                {
                                    for (int i = 0; i <= _serverlist.Count; i++)
                                    { payload = payload.Concat(_serverlist.GetHostInfo(i).GetBytes()).ToArray(); }
                                }
                                catch { }
                            }
                            if (IsBitSet(flags, 0) && IsBitSet(flags, 2))
                            {

                                for (int i = 0; i <= _serverlist.Count; i++)
                                {
                                    try
                                    {
                                        if (_serverlist.GetHostInfo(i).RequiresPassword == 0)
                                            payload = payload.Concat(_serverlist.GetHostInfo(i).GetBytes()).ToArray();
                                    }
                                    catch { }
                                }

                            }
                        }
                        else
                        {
                            for (int i = 0; i <= _clientlist.Count; i++)
                            {
                                try
                                {
                                    payload = payload.Concat(_clientlist.GetHostInfo(i).GetBytes()).ToArray();
                                }
                                catch { }
                            }
                        }

                        pl = HubHostlistDatPayLoad.FromBytes(payload);
                        anser = new HubMessage(HubPacketTypes.HOSTLISTDAT, pl);

                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
               // case HubPacketTypes.HOSTLISTDAT:
                 //   break;
                case HubPacketTypes.HOSTAVAILABLEREQ:
                    {
                        PIZZAString hostname = PIZZAString.FromBytes(message.PayLoad.GetBytes());
                        HubMessage anser;
                        HubHostavailableDatPayLoad pl;
                        byte[] payload = new byte[] { 0 };

                        if (_serverlist.ContainsHostname(hostname) || _clientlist.ContainsHostname(hostname))
                            payload[0] = 1;

                        pl = HubHostavailableDatPayLoad.FromBytes(payload);

                        anser = new HubMessage(HubPacketTypes.HOSTAVAILABLEDAT, pl);

                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
              //  case HubPacketTypes.HOSTAVAILABLEDAT:
                //    break;
                case HubPacketTypes.UNLISTREQ:
                    {
                        PIZZAString hostname = PIZZAString.FromBytes(message.PayLoad.GetBytes());
                        HubMessage anser;
                        byte[] payload = new byte[] { };



                        if (_serverlist.ContainsHostname(hostname))
                            _serverlist.RemoveServer(_serverlist.GetIdByHostname(hostname));
                        else if (_clientlist.ContainsHostname(hostname))
                            _clientlist.RemoveClient(_clientlist.GetIdByHostname(hostname));


                    }
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
