using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Hub;
using PIZZA.Hub.Core;

namespace PIZZA.Hub.Interface
{
    class HubTerminalCommands
    {
        private HubPizzaServerList _serverlist;
        private HubPizzaClientList _clientlist;
        private HubRespondingHosts _respondingHosts;

        public HubTerminalCommands(HubPizzaServerList serverlist, HubPizzaClientList clientlist,HubRespondingHosts respondingHosts)
        {
            _serverlist = serverlist;
            _clientlist = clientlist;
            _respondingHosts = respondingHosts;
        }


        public int ListServers(string[] input)
        {
            if (_serverlist.Count > 0)
            {
                HubTerminal.Cout(ConsoleColor.White, "=================ServerList===============\n");

                foreach (KeyValuePair<int, HubHostInfo> p in _serverlist.GetAllServerInfos())
                {
                    HubTerminal.Cout(ConsoleColor.Yellow, $"================{p.Value.Hostname}================");
                    HubTerminal.Cout(ConsoleColor.White, $"Hostidentifier: {p.Key}");
                    HubTerminal.Cout(ConsoleColor.White, $"Friendlyname: {p.Value.Friendlyname}");
                    HubTerminal.Cout(ConsoleColor.White, $"Description: {p.Value.Description}\n");
                }
            }
            else HubTerminal.Cout(ConsoleColor.Red, "[ServerList] No servers found...");

            return 0;
        }

        public int ListClients(string[] input)
        {
            if (_clientlist.Count > 0)
            {
                HubTerminal.Cout(ConsoleColor.White, "=================ClientList===============\n");

                foreach (KeyValuePair<int, HubHostInfo> p in _clientlist.GetAllClientInfos())
                {
                    HubTerminal.Cout(ConsoleColor.Yellow, $"================{p.Value.Hostname}================");
                    HubTerminal.Cout(ConsoleColor.White, $"Hostidentifier: {p.Key}");
                    HubTerminal.Cout(ConsoleColor.White, $"Friendlyname: {p.Value.Friendlyname}");
                    HubTerminal.Cout(ConsoleColor.White, $"Description: {p.Value.Description}\n");
                }

            }
            else HubTerminal.Cout(ConsoleColor.Red, "[ClientList] No clients found...");

            return 0;
        }

        public int RemoveHost(string[] hostID)
        {
            int Hostidentifier = 0;

            if (int.TryParse(hostID[0], out Hostidentifier))
            {
                if (_serverlist.ContainsServerID(Hostidentifier))
                {
                    _serverlist.RemoveServer(Hostidentifier);
                    _respondingHosts.RemoveResponse(Hostidentifier);
                }
                else if (_clientlist.ContainsClientID(Hostidentifier))
                {
                    _clientlist.RemoveClient(Hostidentifier);
                    _respondingHosts.RemoveResponse(Hostidentifier); 
                }
                else HubTerminal.Cout(ConsoleColor.Red, $"There is no Host with ID: {Hostidentifier}");
            }
            else HubTerminal.Cout(ConsoleColor.Red, $"[RemoveError] The given parameter: {hostID[0]} is not an Integer...");

            return 0;
        }

        public int help(string[] input)
        {
            HubTerminal.Cout(ConsoleColor.Green, "#####################", false); HubTerminal.Cout(ConsoleColor.Yellow, "ConsoleCommands", false);HubTerminal.Cout(ConsoleColor.Green,"###################");
            HubTerminal.Cout(ConsoleColor.Green, "##", false);
            HubTerminal.Cout(ConsoleColor.White, " setpass {new_password}", false);HubTerminal.Cout(ConsoleColor.Green, " # ", false);HubTerminal.Cout(ConsoleColor.White," set server password     ",false);
            HubTerminal.Cout(ConsoleColor.Green, "##");
            HubTerminal.Cout(ConsoleColor.Green, "##---------------------------------------------------##");
            HubTerminal.Cout(ConsoleColor.Green, "##", false);
            HubTerminal.Cout(ConsoleColor.White, " showpass              ", false); HubTerminal.Cout(ConsoleColor.Green, " # ", false);HubTerminal.Cout(ConsoleColor.White," show password hash      ", false);
            HubTerminal.Cout(ConsoleColor.Green, "##");
            HubTerminal.Cout(ConsoleColor.Green, "##---------------------------------------------------##");
            HubTerminal.Cout(ConsoleColor.Green, "##", false);
            HubTerminal.Cout(ConsoleColor.White, " serverlist            ", false); HubTerminal.Cout(ConsoleColor.Green, " # ",false); HubTerminal.Cout(ConsoleColor.White,"list all servers         ", false);
            HubTerminal.Cout(ConsoleColor.Green, "##");
            HubTerminal.Cout(ConsoleColor.Green, "##---------------------------------------------------##");
            HubTerminal.Cout(ConsoleColor.Green, "##", false);
            HubTerminal.Cout(ConsoleColor.White, " clientlist            ", false); HubTerminal.Cout(ConsoleColor.Green, " # ",false); HubTerminal.Cout(ConsoleColor.White," list all clients        ", false);
            HubTerminal.Cout(ConsoleColor.Green, "##");
            HubTerminal.Cout(ConsoleColor.Green, "##---------------------------------------------------##");
            HubTerminal.Cout(ConsoleColor.Green, "##", false);
            HubTerminal.Cout(ConsoleColor.White, " setpass               ", false); HubTerminal.Cout(ConsoleColor.Green, " # ",false); HubTerminal.Cout(ConsoleColor.White," set server password     ", false);
            HubTerminal.Cout(ConsoleColor.Green, "##");
            HubTerminal.Cout(ConsoleColor.Green, "##---------------------------------------------------##");
            HubTerminal.Cout(ConsoleColor.Green, "##", false);
            HubTerminal.Cout(ConsoleColor.White, " removehost {id}       ", false); HubTerminal.Cout(ConsoleColor.Green, " # ", false); HubTerminal.Cout(ConsoleColor.White," remove a host by id     ", false);
            HubTerminal.Cout(ConsoleColor.Green, "##");
            HubTerminal.Cout(ConsoleColor.Green, "#######################################################");
            return 0;
        }

    }
}
