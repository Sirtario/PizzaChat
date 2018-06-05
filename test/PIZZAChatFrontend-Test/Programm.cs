using PIZZA.Chat.Core;
using PIZZA.Client;
using PIZZA.Core;
using PIZZAChatFrontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIZZAChatFrontend_Test
{
    public static class Programm
    {
        private static List<Tuple<string, string, string, bool>> _exampleServers = new List<Tuple<string, string, string, bool>>()
        {
            new Tuple<string, string, string, bool>("server1", "Beschreibung von Server1", "127.0.0.1", true),
            new Tuple<string, string, string, bool>("server2", "Beschreibung von Server2", "127.0.0.2", false),
            new Tuple<string, string, string, bool>("server3", "Beschreibung von Server3", "127.0.0.3", false),
            new Tuple<string, string, string, bool>("server4", "Beschreibung von Server4", "127.0.0.4", true),
        };

        [STAThread]
        public static void Main()
        {
            var a = new System.Windows.Application();
            IPIZZAFrontend window = new MainWindow();

            var channel = "test";
            var host = -1;

            var users = new List<string>()
            {
                "Hans",
                "Phillip",
                "Paul",
                "Ronny"
            };

            var channels = new List<PIZZAChannel>()
            {
                new PIZZAChannel()
                {
                    Channelname = new PIZZAString(){ Value = "channel1" },
                },
                new PIZZAChannel()
                {
                    Channelname = new PIZZAString(){ Value = "channel2" },
                },
                new PIZZAChannel()
                {
                    Channelname = new PIZZAString(){ Value = "channel3" },
                },
                new PIZZAChannel()
                {
                    Channelname = new PIZZAString(){ Value = "test" },
                },
                new PIZZAChannel()
                {
                    Channelname = new PIZZAString(){ Value = "estt" },
                }
            };

            window.GetServers += () =>
            {
                window.ShowServerlist(_exampleServers);
            };

            window.SendMessage += (message) =>
            {
                window.ReceiveMessage(message, "you", false);
            };

            window.Connect += (index) =>
            {
                host = index;

                window.RefreshStatus(users, channels, "test", _exampleServers[host].Item1);
            };

            window.EnterRoom += (pchannel) =>
            {
                window.ShowEnterChannelReturncode(ChatEnterChannelReturnCode.Accepted, pchannel.Channelname.Value);
                window.RefreshStatus(users, channels, pchannel.Channelname.Value, _exampleServers[host].Item1);
            };

            a.Run(window as MainWindow);
        }
    }
}
