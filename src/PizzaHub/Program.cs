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

            TCPServer server = new TCPServer(IsPackageCompleteHb);
            HubTerminal term = new HubTerminal(server);
            Thread listener = new Thread(term.RunListener);

            HubServerMessageHandler messagehandler = new HubServerMessageHandler(server, ServerList, ClientList, respondinghosts);

            HubResponseTimer timer = new HubResponseTimer(respondinghosts, ClientList, ServerList);

            term.CommandAdd("test", HubTerminalCommands.testCmd);
            
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
