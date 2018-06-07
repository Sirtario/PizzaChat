using System;
using System.Collections.Generic;
using System.Text;

namespace Pizza.Server
{
    public class PizzaChatConfigChannel
    {
        public PizzaChatConfigChannel()
        {
            Channelname = "Home";
            Password = string.Empty;
        }

        public PizzaChatConfigChannel(string channelname, string password)
        {
            Channelname = channelname;
            Password = password;
        }

        public string Channelname { get; set; }
        public string Password{ get; set; }
    }
}
