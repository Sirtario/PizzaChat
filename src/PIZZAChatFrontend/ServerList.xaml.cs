using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PIZZAChatFrontend
{
    /// <summary>
    /// Interaktionslogik für ServerList.xaml
    /// </summary>
    public partial class ServerList : Window
    {
        private string _servers = string.Empty;
        private string _baseHtml;

        public event Action<int> ConnectTo;

        public ServerList(List<Tuple<string, string, string>> servers)
        {
            InitializeComponent();

            _baseHtml = File.ReadAllText("ServerList.html");

            SetItems(servers);
        }

        private void SetItems(List<Tuple<string, string, string>> servers)
        {
            _servers = string.Empty;

            foreach (var tuple in servers)
            {
                var index = servers.IndexOf(tuple);
                
                _servers += $"<tr title=\"{tuple.Item3}\"><td><a href=\"{index}\">{tuple.Item1}</a></td><td>&nbsp;:&nbsp;</td><td><a href=\"{index}\">{tuple.Item2}</a></td></tr>";
            }

            var html = _baseHtml.Replace("__SERVER__", _servers);

            _webbrowserServers.NavigateToString(html);
        }

        private void WebbrowserServers_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if(e.Uri != null && int.TryParse(e.Uri.AbsolutePath, out var index))
            {
                e.Cancel = true;

                ConnectTo?.Invoke(index);
            }
        }
    }
}
