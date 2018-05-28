using System.Collections.Generic;
using System.Net;

namespace ACX.ViciOne.TCPLibrary
{
    public interface ITCPServer
    {
        IEnumerable<IPEndPoint> ConnectedClients { get; }

        event TcpServerMessageReceivedEventHandler TCPMessagereceived;

        void DisconnectClient(IPEndPoint sender);
        void Send(IPEndPoint target, byte[] message);
        void ServerStart();
        void ServerStart(int port);
        void Stop();
    }
}