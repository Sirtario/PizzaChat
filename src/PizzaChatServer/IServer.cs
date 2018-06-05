using System.Collections.Generic;
using PIZZA.Chat.Core;

namespace PIZZA.Chat.Server
{
    public interface IServer
    {
        List<PIZZAChannel> Channels { get; set; }
        List<ChatClientConnection> Connections { get; }
        string DefaultChannel { get; set; }

        event ChatClientConnectionApprovalEventHandler ClientConnecting;
        event ChatClientEnteringChannelEventHandler ClientEnteringChannel;

        void Dispose();
        void Start();
        void Start(int port);
        void Stop();
    }
}