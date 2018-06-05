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
    public class HubServerMessageHandler
    {
        private HubPizzaServerList _serverlist;
        private int _MaxID = 1;
        private ITCPServer _server;
        private HubPizzaClientList _clientlist;
        private HubRespondingHosts _respondingHost;
        private HubServerAuthentication _serverAuthentication;

        private bool IsBitSet(byte b, int bit)
        { return (1 & (b >> bit)) == 1; }

        public HubServerMessageHandler(ITCPServer server, HubPizzaServerList serverlist, HubPizzaClientList clientlist, HubRespondingHosts respondingHost /*,HubServerAuthentication authentication*/ )
        {
            _server = server;
            _server.TCPMessagereceived += RecievedMessageEventHandler;
            _serverlist = serverlist;
            _clientlist = clientlist;
            _respondingHost = respondingHost;
            HubTerminal.Cout(ConsoleColor.White, "[PizzaHub] HubMessageHandler Initialized...");
        }

        private void RecievedMessageEventHandler(object sender, TcpServerMessageReceivedEventArgs e)
        {
            HubMessage message = HubMessageFactory.GetMessage(e.Message);

            switch(message.Header.PacketType)
            {
                case HubPacketTypes.SERVERENLISTREQ:
                    {
                        byte[] payload;
                        HubEnlistAckPayLoad pl;
                        HubMessage anser;
                        PIZZAInt hostid;

                        HubHostInfo ServerInfo = HubHostInfo.FromBytes(message.PayLoad.GetBytes());

                        

                        if (ServerInfo.Hostname == String.Empty)
                        {
                            ServerInfo.Hostname = e.Sender.ToString();
                            HubTerminal.Cout(ConsoleColor.Yellow, $"[Hub Info] Hostname was empty using EndPoint: {e.Sender.ToString()} as Hostname...");
                        }

                        if (!_serverlist.ContainsHostInfo(ServerInfo))
                        {
                            if (_serverlist.ContainsServerID(_MaxID))
                                _MaxID++;

                                _serverlist.AddServer(_MaxID, ServerInfo);

                            hostid = new PIZZAInt() { Value = _MaxID };
                            payload = new byte[] { (byte)HubReturnCodes.ACCEPTED };
                            payload = payload.Concat(new byte[] { 20 }).Concat(hostid.GetBytes()).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);

                            HubTerminal.Cout(ConsoleColor.Green, $"[Server Added] Server with EndPoint {e.Sender} has been added as ID: { _MaxID } Hostname: { ServerInfo }");

                            _MaxID++;
                        }
                        else
                        {
                            hostid = new PIZZAInt() { Value = 0 };
                            payload = new byte[] { (byte)HubReturnCodes.DENIED_ALREDY_LISTED };
                            payload = payload.Concat(new byte[] { 0 }).Concat(hostid.GetBytes()).ToArray();

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
                        PIZZAInt hostid;

                        HubHostInfo ClientAddress = HubHostInfo.FromBytes(e.Message);

                        if (ClientAddress.Hostname == String.Empty)
                            ClientAddress.Hostname = e.Sender.ToString();

                        if (!_clientlist.ContainsHostInfo(ClientAddress))
                        {
                            if (_clientlist.ContainsClientID(_MaxID))
                            _MaxID++;
 
                            _clientlist.AddClient(_MaxID, ClientAddress);

                            hostid = new PIZZAInt() { Value = _MaxID };

                            payload = new byte[] { (byte)HubReturnCodes.ACCEPTED };
                            payload = payload.Concat(new byte[] { 20 }).Concat(hostid.GetBytes()).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);
                        }
                        else
                        {
                            hostid = new PIZZAInt() { Value = 0 };
                            payload = new byte[] { (byte)HubReturnCodes.DENIED_ALREDY_LISTED };
                            payload = payload.Concat(new byte[] { 0 }).Concat(hostid.GetBytes()).ToArray();

                            pl = HubEnlistAckPayLoad.FromBytes(payload);

                            anser = new HubMessage(HubPacketTypes.ENLISTACK, pl);
                        }


                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
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
                                        if (_serverlist.GetHostInfo(i).RequiresPassword == false)
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
                case HubPacketTypes.UNLISTREQ:
                    {
                        PIZZAString hostname = PIZZAString.FromBytes(message.PayLoad.GetBytes());
                        HubMessage anser;
                        HubUnlistAckPayLoad pl;
                        byte[] payload = new byte[] { };



                        if (_serverlist.ContainsHostname(hostname))
                            _serverlist.RemoveServer(_serverlist.GetIdByHostname(hostname));
                        else if (_clientlist.ContainsHostname(hostname))
                            _clientlist.RemoveClient(_clientlist.GetIdByHostname(hostname));

                        pl = HubUnlistAckPayLoad.FromBytes(payload);
                        anser = new HubMessage(HubPacketTypes.UNLISTACK, pl);

                        _server.Send(e.Sender, anser.GetBytes());
                    }
                    break;
                case HubPacketTypes.PING:
                    {
                        PIZZAInt hostidentifier = PIZZAInt.FromBytes(message.PayLoad.GetBytes());
                        HubMessage anser;
                        HubPingAckPayLoad pl;
                        byte[] payload = new byte[] { };

                        _respondingHost.SetInterval(hostidentifier.Value, 2);

                        pl = HubPingAckPayLoad.FromBytes(payload);

                        anser = new HubMessage(HubPacketTypes.PINGACK, pl);

                        _server.Send(e.Sender, anser.GetBytes());

                    }
                    break;
                default:
                    throw new NotImplementedException($"The packet type {message.Header.PacketType} is unknown...");
            }
        }


    }
}
