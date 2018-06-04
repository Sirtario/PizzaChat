using System;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub
{
    class HubRespondingHosts
    {
        private Dictionary<int, int> _respondinghosts;

        public void AddResponse(int hostidentifier, int interval) => _respondinghosts.Add(hostidentifier, interval);

        public void RemoveResponse(int hostidentifier) => _respondinghosts.Remove(hostidentifier);

        public bool ContainsResponse(int hostidentifier) => _respondinghosts.ContainsKey(hostidentifier);

        public int Count => _respondinghosts.Count;

        public int GetIntervalCount(int hostidentifier) => _respondinghosts[hostidentifier];

        public void DecrementInterval(int hostidentifier) => _respondinghosts[hostidentifier]--;

        public void SetInterval(int hostidentifier, int NewInterval) => _respondinghosts[hostidentifier] = NewInterval;

        public Dictionary<int, int> GetResponse() => _respondinghosts;

        public HubRespondingHosts()
        {
            _respondinghosts = new Dictionary<int, int>();
        }

    }
}
