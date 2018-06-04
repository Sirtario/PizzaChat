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

        public string Channelname { get; }
        public string Password{get;}
    }
}
