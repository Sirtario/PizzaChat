using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIZZAChatFrontend
{
    public class PIZZAApp
    {
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
