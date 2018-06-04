using System.Collections.Generic;

namespace Pizza.Server
{
    internal class PIZZAChatConfig
    {
        private readonly List<PizzaChatConfigChannel> _channels;
        private readonly string _defaultChannel;
        private readonly List<PIZZAChatConfigUser> _users;

        public string DefaultChannel => _defaultChannel;
        public List<PizzaChatConfigChannel> Channels => _channels;
        public List<PIZZAChatConfigUser> Users => _users;
    }
}