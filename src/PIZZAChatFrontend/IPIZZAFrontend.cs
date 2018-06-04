using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PIZZA.Chat.Core;

namespace PIZZA.Client
{
    public interface IPIZZAFrontend
    {
        event Action<string> SendMessage;

        /// <summary>
        /// whisperTarget, message
        /// </summary>
        event Action<string, string> WhisperMessage;
        event Action<PIZZAChannel> EnterRoom;
        event Action<int> Connect;
        event Action Disconnect;
        event Action GetServers;

        void ReceiveMessage(string Message, string sender, bool isWhispered);

        /// <summary>
        /// servername, server-description, hostname, isPasswordRequired
        /// </summary>
        /// <param name="servers"></param>
        void ShowServerlist(List<Tuple<string, string, string, bool>> servers);
        void RefreshStatus(List<string> usersInChannel, List<PIZZAChannel> channels, string channel);
        string GetClientId();
        string GetPassword(string topic);
        void ShowReturncode(ChatConnectReturncode returncode);
        void ShowEnterChannelReturncode(ChatEnterChannelReturnCode returnCode);
    }
}