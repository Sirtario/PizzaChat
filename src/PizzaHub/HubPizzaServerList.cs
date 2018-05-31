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

        public void AddServer(int id, PIZZAString nserver) => _serverList.Add(id, nserver);

        public bool ContainsServerID(int ID) => _serverList.ContainsKey(ID);

        public bool ContainsEndPoint(PIZZAString EndPoint) => _serverList.ContainsValue(EndPoint);

        public PIZZAString GetEndPoint(int ID) => _serverList[ID];

        public HubPizzaServerList()
        {
            _serverList = new Dictionary<int, PIZZAString>();
        }
    }
}
