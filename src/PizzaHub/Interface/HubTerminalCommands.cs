using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;


namespace PIZZA.Hub.Interface
{
    static class HubTerminalCommands
    {
        public static int testCmd(string[] str)
        {
            Console.WriteLine(str[0]);

            return 0;
        }


    }
}
