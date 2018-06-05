using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Timers;
using System.Text;
using PIZZA.Hub;

namespace PIZZA.Hub
{
    public class HubResponseTimer
    {
        private int _intervalTime = 15000;

        private HubRespondingHosts _respondingHosts;

        private HubPizzaClientList _clientlist;

        private HubPizzaServerList _serverlist;

        public int IntervallTime => _intervalTime;

        private Timer _timer;

        private Task HandleIntervals()
        {
            Task checkResponses = new Task(()=> {
                foreach(KeyValuePair<int,int> p in _respondingHosts.GetResponse())
                {
                    if (p.Value == 0)
                    {
                        if (_serverlist.ContainsServerID(p.Key))
                            _serverlist.RemoveServer(p.Key);
                        else _clientlist.RemoveClient(p.Key);

                        _respondingHosts.RemoveResponse(p.Key);

                    }
                    else _respondingHosts.DecrementInterval(p.Key);
                }
            });

            return checkResponses;
        }

        public HubResponseTimer(HubRespondingHosts respondingHosts, HubPizzaClientList clientlist, HubPizzaServerList serverlist)
        {
            _timer = new Timer(_intervalTime);
            _timer.Elapsed += async (sender, e) => await HandleIntervals();
            _timer.Start();
            _respondingHosts = respondingHosts;
            _clientlist = clientlist;
            _serverlist = serverlist;
        }

    }
}
