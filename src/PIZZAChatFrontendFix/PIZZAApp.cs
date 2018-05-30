using ACX.ViciOne.TCPLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIZZA.Client
{
    public class PIZZAApp
    {
        private TCPClient _tcpClientChat;
        private TCPClient _tcpClientHub;

        public string HubHostName { get; set; }

        private List<Tuple<string, string, string>> _servers;

        public void Prepare(IPIZZAFrontend frontend)
        {
            frontend.Connect += Frontend_Connect;
            frontend.Disconnect += Frontend_Disconnect;
            frontend.EnterRoom += Frontend_EnterRoom;
            frontend.GetServers += Frontend_GetServers;
            frontend.SendMessage += Frontend_SendMessage;
            frontend.WhisperMessage += Frontend_WhisperMessage;
        }

        private void Frontend_WhisperMessage(string arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        private void Frontend_SendMessage(string obj)
        {
            throw new NotImplementedException();
        }

        private void Frontend_GetServers()
        {
            if(_tcpClientHub == null || !_tcpClientHub.IsAlive)
            {
                _tcpClientHub = new TCPClient()
            }

            _t
        }

        private void Frontend_EnterRoom(string obj)
        {
            throw new NotImplementedException();
        }

        private void Frontend_Disconnect()
        {
            throw new NotImplementedException();
        }

        private void Frontend_Connect(int obj)
        {
            if(_servers == null)
            {
                return;
            }
        }
    }
}
