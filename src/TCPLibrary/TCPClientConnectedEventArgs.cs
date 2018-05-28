using System;
using System.Net.Sockets;

namespace ACX.ViciOne.TCPLibrary
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TCPClientConnectedEventHandler(object sender, TCPClientConnectedEventArgs e);

    /// <summary>
    /// Event Arguments that are Needed to handle the TCPMessageRecieved Event
    /// </summary>
    public class TCPClientConnectedEventArgs
    {
        public TcpClient TcpClient { get; private set; }

        public TCPClientConnectedEventArgs(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }
    }
}