using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PIZZAChatFrontend
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var window = new MainWindow();

            window.Show();
        }
    }
}
