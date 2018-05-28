using System.Net;

namespace ACX.ViciOne.TCPLibrary
{
    public delegate void TcpServerMessageReceivedEventHandler(object sender, TcpServerMessageReceivedEventArgs e);

    public class TcpServerMessageReceivedEventArgs : TcpMessageReceivedEventArgs
    {
        public TcpServerMessageReceivedEventArgs(byte[] message, IPEndPoint sender) : base(message)
        {
            Sender = sender;
        }

        public IPEndPoint Sender { get; private set; }
    }
}