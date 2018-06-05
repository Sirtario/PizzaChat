using System;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;
using PIZZA.Hub.Core;

namespace PIZZA.Hub
{
    public class HubPizzaClientList
    {
        private Dictionary<int, HubHostInfo> _clientlist;

        public void AddClient(int ID, HubHostInfo Hostname) => _clientlist.Add(ID, Hostname);

        public bool ContainsClientID(int ID) => _clientlist.ContainsKey(ID);

        public bool ContainsHostInfo(HubHostInfo Hostname) => _clientlist.ContainsValue(Hostname);

        public HubHostInfo GetHostInfo(int ID) => _clientlist[ID];

        public void RemoveClient(int ID) => _clientlist.Remove(ID);

        public int Count => _clientlist.Count;

        public Dictionary<int, HubHostInfo> GetAllClientInfos() => _clientlist;

        public bool ContainsHostname(PIZZAString hostname)
        {
            foreach(KeyValuePair<int,HubHostInfo> hi in _clientlist)
            {

                if (hi.Value.Hostname == hostname.Value)
                    return true;
            }
            return false;
        }

        public int GetIdByHostname(PIZZAString hostname)
        {
            foreach(KeyValuePair<int,HubHostInfo> hi in _clientlist)
            {
                if (hi.Value.Hostname == hostname.Value)
                    return hi.Key;
            }
            return 0;
        }

        public HubPizzaClientList()
        {
            _clientlist = new Dictionary<int, HubHostInfo>();
        }

    }
}
