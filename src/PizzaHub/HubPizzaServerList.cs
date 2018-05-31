﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;
using PIZZA.Hub.Core;


namespace PIZZA.Hub
{
    class HubPizzaServerList
    {
        private Dictionary<int, HubHostInfo> _serverList;

        public void AddServer(int id, HubHostInfo newserver) => _serverList.Add(id, newserver);

        public bool ContainsServerID(int id) => _serverList.ContainsKey(id);

        public bool ContainsHostInfo(HubHostInfo EndPoint) => _serverList.ContainsValue(EndPoint);

        public HubHostInfo GetHostInfo(int id) => _serverList[id];

        public int Count => _serverList.Count;

        public void RemoveServer(int id) => _serverList.Remove(id);

        public bool ContainsHostname(PIZZAString hostname)
        {            
            foreach(KeyValuePair<int,HubHostInfo> hi in _serverList)
            {
                if (hi.Value.Hostname == hostname)
                 return true;
            }
            return false;
        }

        public int GetIdByHostname(PIZZAString hostname)
        {
            foreach(KeyValuePair<int,HubHostInfo> hi in _serverList)
            {
                if (hi.Value.Hostname == hostname)
                    return hi.Key;
            }
            return 0;
        }

        public HubPizzaServerList()
        {
            _serverList = new Dictionary<int, HubHostInfo>();
        }
    }
}
