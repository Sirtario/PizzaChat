using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ACX.ViciOne.TCPLibrary;
using PIZZA.Hub.Core;

namespace PIZZA.Hub.Interface
{
    public delegate int cmdFunc(string[] T);

   public class HubTerminal
    {

        private Dictionary<string,cmdFunc> _commands = new Dictionary<string,cmdFunc>();
        private TCPServer _server;

        public HubTerminal(TCPServer server)
        {
            _server = server;
        }

        public static void _Cout(ConsoleColor color, string txt, bool IsLine = true)
        {
            Console.ForegroundColor = color;

            if (IsLine)
                Console.WriteLine(txt);
            else Console.Write(txt);
        }

        public static void Cout(ConsoleColor color, string txt, bool IsLine = true)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = color;

            if (IsLine)
                Console.WriteLine(txt);
            else Console.Write(txt);

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("cmd>>");
        }

        public void CommandAdd(string cmd,cmdFunc cmdFunct)
        {
            _commands.Add(cmd, cmdFunct);
        }

        public void RunListener()
        {
            bool run = true;
            _server.ServerStart(Constants.HubStandardPort);
            Cout(ConsoleColor.Yellow, "[TCP] Server started...");
            while (run)
            {
               // Cout(ConsoleColor.Gray, "cmd>> ", false);
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
            Cout(ConsoleColor.Yellow, "[TCP] Server Stoped...");
            _server.Stop();
        }

    }
}
