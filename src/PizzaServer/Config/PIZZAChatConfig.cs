using PIZZA.Hub.Client;
using System.Collections.Generic;
using System.Net;

namespace Pizza.Server
{
    public class PIZZAChatConfig
    {
        public PIZZAChatConfig()
        {
        }

        public PIZZAChatConfig(bool b)
        {
            DefaultChannel = "Home";
            Hostname = "127.0.0.1";
            Channels = new List<PizzaChatConfigChannel>();
            Users = new List<PIZZAChatConfigUser>();
            HubIPAdress = "127.0.0.1";
            HubPort = 56602;
            HubPass = string.Empty;

            Users.Add(new PIZZAChatConfigUser());
            Channels.Add(new PizzaChatConfigChannel("tests",string.Empty));
            Channels.Add(new PizzaChatConfigChannel("Home", string.Empty));
        }

        public string DefaultChannel { get; set; }
        public string Hostname { get;  set; }

        public List<PizzaChatConfigChannel> Channels { get; set; }
        public List<PIZZAChatConfigUser> Users { get; set; }

        public string HubIPAdress { get; set; }
        public int HubPort { get; set; }
        public string HubPass { get;  set; }
    }
}