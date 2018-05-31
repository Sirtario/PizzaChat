using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;


namespace PIZZA.Hub
{
    class HubPizzaServerList
    {
        private Dictionary<int, PIZZAString> _serverList;

        public void AddServer(int id, PIZZAString newserver) => _serverList.Add(id, newserver);

        public bool ContainsServerID(int id) => _serverList.ContainsKey(id);

        public bool ContainsHostname(PIZZAString EndPoint) => _serverList.ContainsValue(EndPoint);

        public PIZZAString GetHostname(int id) => _serverList[id];

        public HubPizzaServerList()
        {
            _serverList = new Dictionary<int, PIZZAString>();
        }
    }
}
