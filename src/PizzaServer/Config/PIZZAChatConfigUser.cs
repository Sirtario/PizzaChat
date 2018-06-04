using System;
using System.Collections.Generic;
using System.Text;

namespace Pizza.Server
{
    public class PIZZAChatConfigUser
    {
        public PIZZAChatConfigUser()
        {
            ClientID = "default";
            Password = string.Empty;
        }

        public string ClientID { get; set; }
        public string Password { get; set; }

    }
}
