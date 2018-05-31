using ACX.ViciOne.TCPLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    public class TCPMock : ITCPServer
    {
        public IEnumerable<IPEndPoint> ConnectedClients => null;

        public event TcpServerMessageReceivedEventHandler TCPMessagereceived;

        /// <summary>
        /// gets invoked everytime the tcp should send someting
        /// </summary>
        public event Action<IPEndPoint, byte[]> SendMessageRecieved;

        public void DisconnectClient(IPEndPoint sender)
        {
            
        }

        public void Send(IPEndPoint target, byte[] message)
        {
            SendMessageRecieved.Invoke(target, message);
        }

        public void ServerStart()
        {
            
        }

        public void ServerStart(int port)
        {
            
        }

        public void Stop()
        {
            
        }

        /// <summary>
        /// invokes a TCPmessagerecievedEvent
        /// </summary>
        /// <param name="message"></param>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        public void SendMessage(byte[] message, string IP, int port)
        {
            TCPMessagereceived.Invoke(this, new TcpServerMessageReceivedEventArgs(message, new IPEndPoint(IPAddress.Parse(IP), port)));
        }
    }
}
