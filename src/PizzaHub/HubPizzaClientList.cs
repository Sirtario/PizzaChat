using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub
{
    class HubPizzaClientList
    {
        private Dictionary<int, PIZZAString> _clist;

        public void AddClient(int ID, PIZZAString Hostname) => _clist.Add(ID, Hostname);

        public bool ContainsClientID(int ID) => _clist.ContainsKey(ID);

        public bool ContainsHostname(PIZZAString Hostname) => _clist.ContainsValue(Hostname);

        public PIZZAString GetHostname(int ID) => _clist[ID];

        public void RemoveClient(int ID) => _clist.Remove(ID);

        public HubPizzaClientList()
        {
            _clist = new Dictionary<int, PIZZAString>();
        }

    }
}
