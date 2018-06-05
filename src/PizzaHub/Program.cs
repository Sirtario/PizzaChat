using System;
using System.Threading;
using PIZZA.Hub.Interface;
using PIZZA.Hub.Core;
using ACX.ViciOne.TCPLibrary;


namespace PIZZA.Hub
{
    class Program
    {
        

        static void Main(string[] args)
        {
            HubTerminal.Cout(ConsoleColor.Gray, "PizzaHub");
            HubPizzaServerList ServerList = new HubPizzaServerList();
            HubPizzaClientList ClientList = new HubPizzaClientList();
            HubRespondingHosts respondinghosts = new HubRespondingHosts();
            HubServerAuthentication ServerAuthentication = new HubServerAuthentication();
            HubTerminalCommands Commands = new HubTerminalCommands(ServerList, ClientList,respondinghosts);

            TCPServer server = new TCPServer(IsPackageCompleteHb);
            HubTerminal term = new HubTerminal(server);
            Thread listener = new Thread(term.RunListener);

            HubServerMessageHandler messagehandler = new HubServerMessageHandler(server, ServerList, ClientList, respondinghosts);

            HubResponseTimer timer = new HubResponseTimer(respondinghosts, ClientList, ServerList);

            term.CommandAdd("setpass", ServerAuthentication.SetPassword);
            term.CommandAdd("showpass", ServerAuthentication.ShowPassword);
            term.CommandAdd("serverlist", Commands.ListServers);
            term.CommandAdd("clientlist", Commands.ListClients);
            term.CommandAdd("removehost", Commands.RemoveHost);
            term.CommandAdd("help", Commands.help);

            listener.Start();

        }

        private static bool IsPackageCompleteHb(byte[] currentBytes)
        {
            HubMessage message;

            try
            {
                message = HubMessageFactory.GetMessage(currentBytes);
            }
            catch
            {
                return false;
            }

            return message.Header.PayloadLength.Value >= message.PayLoad.GetBytes().Length;
        }
    }
}
