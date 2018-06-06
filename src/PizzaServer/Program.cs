using ACX.ViciOne.TCPLibrary;
using PIZZA.Chat.Core;
using PIZZA.Chat.Server;
using PIZZA.Core;
using PIZZA.Hub.Client;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Pizza.Server
{
    class ConsoleServer
    {
        private static ITCPServer _tcpServer;
        private static IServer _server;
        private static PIZZAChatConfig Config;
        private static HubClient _hub;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server!");
            Console.WriteLine("Loading config");

            try
            {
                Config = DeSerializeConfig("Config.xml");
            }
            catch (Exception)
            {
                Config = new PIZZAChatConfig();

                SerializeConfig(Config, "Config.xml");
            }

            //todo: namen aus config
            //Hub stuff
            Console.WriteLine("Enlisting in Hub");
            try
            {
                _hub = new HubClient();
                _hub.Connect(Config.HubIPAdress, Config.HubPort);
                var Hubres = _hub.ListHost(Config.Hostname, true, Config.HubPass, string.Empty, "Pedo", false);
                if (Hubres.Returncode!= PIZZA.Hub.Core.Enumerationen.HubReturnCodes.ACCEPTED)
                {
                    throw new Exception($"{Hubres.Returncode.ToString()}");
                }
                _hub.StartPing(Hubres.HostIdentifier,Hubres.Pinginterval);

            }
            catch (Exception ex)
            {

                Console.WriteLine($"unable to connect to hub: {ex.Message}");
            }
            
            Console.WriteLine("Starting Server");
            _tcpServer = new TCPServer(TcpDelegate.IsPIZZAChatMessageComplete);
            _server = new ServerLib(_tcpServer);

            _server.ClientConnecting += _server_ClientConnecting;
            _server.ClientEnteringChannel += _server_ClientEnteringChannel;

            SetBasicConfig();

            _server.Start();

            Console.ReadLine();

            //TODO: Stop hub connection

            _server?.Dispose();
            SerializeConfig(Config,"Config.xml");

        }

        private static PIZZAChatConfig DeSerializeConfig( string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(PIZZAChatConfig));

            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                return (PIZZAChatConfig)xmlSerializer.Deserialize(stream);

        }

        private static void SerializeConfig(PIZZAChatConfig config, string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(PIZZAChatConfig));

            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                xmlSerializer.Serialize(stream, config);
        }

        private static void SetBasicConfig()

        {
            _server.DefaultChannel = Config.DefaultChannel;

            _server.Channels = new System.Collections.Generic.List<PIZZAChannel>();

            foreach (var channel in Config.Channels)
            {
                _server.Channels.Add(new PIZZAChannel(new PIZZAString() { Value = channel.Channelname }, (byte)(channel.Password == string.Empty ? 0x00 : 0x01)));
            }
        }

        private static void _server_ClientEnteringChannel(object sender, ChatClientEnteringChannelEventArgs e)
        {
            if (Config.Channels.Find(s => s.Channelname == e.Channel).Password == e.Password)
            {
                e.ReturnCode = ChatEnterChannelReturnCode.Accepted;
            }
            else
            {
                e.ReturnCode = ChatEnterChannelReturnCode.Denied;
            }
        }

        private static void _server_ClientConnecting(object sender, ChatConnectApprovalEventArgs e)
        {
            //if (Config.Users.Any(c => c.ClientID == e.ClientID)
            //{

            //}

            Console.WriteLine($"client {e.ClientID} connects");

            e.CommunicationMode = 1;
            e.ConnectReturncode = ChatConnectReturncode.ACCEPTED;
            e.PingIntervall = 60;
        }
    }
}
