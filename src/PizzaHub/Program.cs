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
            HubTerminal term = new HubTerminal();
            Thread listener = new Thread(term.RunListener);
            TCPServer server = new TCPServer(IsPackageCompleteHb);

            HubTerminal.Cout(ConsoleColor.Gray, "PizzaHub");

            HubTerminal.Cout(ConsoleColor.Yellow, "[TCP] Server Gestarted...");
            server.ServerStart();

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
