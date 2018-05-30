using System;
using System.Threading;
using PIZZA.Hub.Interface;

namespace PIZZA.Hub
{
    class Program
    {
        static void Main(string[] args)
        {
            HubTerminal term = new HubTerminal();
            Thread listener = new Thread(term.RunListener);

            term.Cout(ConsoleColor.Gray, "PizzaHub");

            term.CommandAdd("test", HubTerminalCommands.testCmd);
            
            listener.Start();
        }
    }
}
