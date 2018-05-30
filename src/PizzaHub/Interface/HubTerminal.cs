using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub.Interface
{
    delegate int cmdFunc(string[] T);

    class HubTerminal
    {

        private Dictionary<string,cmdFunc> _commands = new Dictionary<string,cmdFunc>();

        public static void Cout(ConsoleColor color, string txt, bool IsLine = true)
        {
            Console.ForegroundColor = color;

            if (IsLine)
                Console.WriteLine(txt);
            else Console.Write(txt);
        }

        public void CommandAdd(string cmd,cmdFunc cmdFunct)
        {
            _commands.Add(cmd, cmdFunct);
        }

        public void RunListener()
        {
            bool run = true;

            while (run)
            {
                Cout(ConsoleColor.Gray, "cmd>> ", false);
                string[] cmd = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (cmd[0] == "exit")
                { run = false; break; }

                if (_commands.ContainsKey(cmd[0]))
                {
                    if (cmd.Length > 1)
                    {
                        List<string> paras = cmd.ToList();
                        paras.RemoveAt(0);
                        _commands[cmd[0]](paras.ToArray());
                    }
                    else _commands[cmd[0]](new string[] { });
                }
                else Cout(ConsoleColor.Red, $"Command: {cmd[0]} not found.");
            }
        }

    }
}
