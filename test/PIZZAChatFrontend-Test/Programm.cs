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
        private static List<Tuple<string, string, string>> _exampleServers = new List<Tuple<string, string, string>>()
        {
            new Tuple<string, string, string>("server1", "Beschreibung von Server1", "127.0.0.1"),
            new Tuple<string, string, string>("server2", "Beschreibung von Server2", "127.0.0.2"),
            new Tuple<string, string, string>("server3", "Beschreibung von Server3", "127.0.0.3"),
            new Tuple<string, string, string>("server4", "Beschreibung von Server4", "127.0.0.4"),
        };

        [STAThread]
        public static void Main()
        {
            var a = new System.Windows.Application();
            var window = new MainWindow();

            window.GetServers += () =>
            {
                window.ShowServerlist(_exampleServers);
            };

            window.SendMessage += (message) =>
            {
                window.ReceiveMessage(message, "you", false);
            };

            a.Run(window);
        }
    }
}
