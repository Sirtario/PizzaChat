using System;

namespace ACX.ViciOne.TCPLibrary
{
    public delegate void TcpMessageReceivedEventHandler(object sender, TcpMessageReceivedEventArgs e);

    public class TcpMessageReceivedEventArgs : EventArgs
    {
        public TcpMessageReceivedEventArgs(byte[] message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public byte[] Message { get; private set; }
    }
}