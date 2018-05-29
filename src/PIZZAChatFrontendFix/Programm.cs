using PIZZAChatFrontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIZZAChatFrontendFix
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var a = new Application();
            var window = new MainWindow();
            var pizzaApp = new PIZZAApp();

            pizzaApp.Prepare(window);

            a.Run(window);
        }
    }
}
