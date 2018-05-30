using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PIZZA.Client
{
    public interface IPIZZAFrontend
    {
        event Action<string> SendMessage;

        /// <summary>
        /// whisperTarget, message
        /// </summary>
        event Action<string, string> WhisperMessage;
        event Action<string> EnterRoom;
        event Action<int> Connect;
        event Action Disconnect;
        event Action GetServers;

        void ReceiveMessage(string Message, string sender, bool isWhispered);
        void ShowServerlist(List<Tuple<string, string, string>> servers);
        void RefreshStatus(List<string> usersInChannel, string channel);
    }
}