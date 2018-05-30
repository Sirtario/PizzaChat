using System;
using System.Threading;
using PIZZA.Hub.Interface;
using ACX.ViciOne.TCPLibrary;


namespace PIZZA.Hub
{
    class Program
    {

        static void Main(string[] args)
        {
            HubTerminal term = new HubTerminal();
            Thread listener = new Thread(term.RunListener);
            byte[] Data = new byte[256];
            TCPServer server = new TCPServer(IsPackageCompleteHb);

            term.Cout(ConsoleColor.Gray, "PizzaHub");

            term.CommandAdd("test", HubTerminalCommands.testCmd);
            
            listener.Start();
        }

        private static bool IsPackageCompleteHb(byte[] currentBytes)
        {
           
        }
    }
}
