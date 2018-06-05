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

        int HubPort { get; }
        string HubHostname { get; }

        void ReceiveMessage(string Message, string sender, bool isWhispered);

        /// <summary>
        /// servername, server-description, hostname, isPasswordRequired
        /// </summary>
        /// <param name="servers"></param>
        void ShowServerlist(List<Tuple<string, string, string, bool>> servers);
        void RefreshStatus(List<string> usersInChannel, List<PIZZAChannel> channels, string channel, string hostname);
        string GetClientId(string hostname);
        string GetPassword(string topic);
        void ShowReturncode(ChatConnectReturncode returncode, string hostname);
        void ShowEnterChannelReturncode(ChatEnterChannelReturnCode returnCode, string channel);
    }
}