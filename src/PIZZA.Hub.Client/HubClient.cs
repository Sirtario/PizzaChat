using ACX.ViciOne.TCPLibrary;
using PIZZA.Hub.Core;
using PIZZA.Hub.Core.Enumerationen;
using PIZZA.Hub.Core.PayLoads;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PIZZA.Hub.Client
{
    public class HubClient
    {
        public event Action PingExceeded;

        private TcpConnection _tcpConnection;

        private ManualResetEventSlim _receivedHostlistDat;
        private HubMessage _hostlistDat;

        private ManualResetEventSlim _receivedHostavailableDat;
        private HubMessage _hostavailableDat;

        private ManualResetEventSlim _receivedEnlistack;
        private HubMessage _enlistAck;

        private ManualResetEventSlim _receivedUnlistack;

        private ManualResetEventSlim _receivedPingack;

        private int _hostIdentifier;
        private int _interval;

        private System.Timers.Timer _pingTimer;

        public void Connect(string host, int port)
        {
            _tcpConnection?.Disconnect();

            _tcpConnection = new TcpConnection(TcpDelegate.IsPIZZAHubMessageComplete);

            _tcpConnection.TCPMessageReceived += TcpConnection_TCPMessageReceived;

            _tcpConnection.Connect(host, port);
        }

        public void Stop()
        {
            _pingTimer?.Dispose();
            _tcpConnection?.Dispose();
        }

        private void TcpConnection_TCPMessageReceived(object sender, TcpMessageReceivedEventArgs e)
        {
            var message = HubMessageFactory.GetMessage(e.Message);

            switch (message.Header.PacketType)
            {
                //case HubPacketTypes.SERVERENLISTREQ:
                //    break;
                //case HubPacketTypes.CLIENTENLISTREQ:
                //    break;
                case HubPacketTypes.ENLISTACK:
                    _enlistAck = message;
                    _receivedEnlistack?.Set();
                    break;
                //case HubPacketTypes.HOSTLISTREQ:
                //    break;
                case HubPacketTypes.HOSTLISTDAT:
                    _hostlistDat = message;
                    _receivedHostlistDat?.Set();
                    break;
                //case HubPacketTypes.HOSTAVAILABLEREQ:
                //    break;
                case HubPacketTypes.HOSTAVAILABLEDAT:
                    _hostavailableDat = message;
                    _receivedHostlistDat?.Set();
                    break;
                //case HubPacketTypes.UNLISTREQ:
                //    break;
                case HubPacketTypes.UNLISTACK:
                    _receivedUnlistack?.Set();
                    break;
                //case HubPacketTypes.PING:
                //    break;
                case HubPacketTypes.PINGACK:
                    _receivedPingack?.Set();
                    break;
                default:
                    break;
            }
        }

        public void StartPing(int hostIdentifier, int intervalSek)
        {
            if(_pingTimer != null && _pingTimer.Enabled)
            {
                _pingTimer.Stop();
            }

            _hostIdentifier = hostIdentifier;
            _interval = intervalSek;

            _pingTimer = new System.Timers.Timer();

            _pingTimer.Interval = intervalSek * 1000;

            _pingTimer.Elapsed += PingTimer_Elapsed;

            _pingTimer.Start();
        }

        private void PingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var pingMessage = HubMessageFactory.GetMessage(HubPacketTypes.PING);
            var paylaod = pingMessage.PayLoad as HubPingPayLoad;

            paylaod.Hostidentifier = _hostIdentifier;

            _tcpConnection.Send(pingMessage.GetBytes());

            _receivedPingack = new ManualResetEventSlim();

            if(!_receivedPingack.Wait(Constants.Timeout))
            {
                PingExceeded?.Invoke();
            }
        }

        public void UnlistHost(int hostidentifier)
        {
            var message = HubMessageFactory.GetMessage(HubPacketTypes.UNLISTACK);
            var payload = message.PayLoad as HubUnlistReqPayLoad;

            payload.Hostidentifier = hostidentifier;

            _tcpConnection.Send(message.GetBytes());
        }

        public EnlistResult ListHost(string hostname, bool isServer, string password, string description, string friendlyname, bool passwordRequired)
        {
            HubMessage message;

            if(isServer)
            {
                message = HubMessageFactory.GetMessage(HubPacketTypes.SERVERENLISTREQ);

                var payload = message.PayLoad as HubServerenlistreqPayLoad;

                payload.Description = description;
                payload.Friendlyname = friendlyname;
                payload.Hostname = hostname;
                payload.PasswordRequired = passwordRequired;
            }
            else
            {
                message = HubMessageFactory.GetMessage(HubPacketTypes.CLIENTENLISTREQ);

                var payload = message.PayLoad as HubClientEnlistReqPayLoad;

                payload.Description = description;
                payload.Friendlyname = friendlyname;
                payload.Hostname = hostname;
                payload.PasswordRequired = passwordRequired;
            }

            _receivedEnlistack = new ManualResetEventSlim();

            _tcpConnection.Send(message.GetBytes());

            if(!_receivedEnlistack.Wait(Constants.Timeout))
            {
                throw new TimeoutException($"Hub didn't answer in {Constants.Timeout}ms");
            }

            var datPayload = _enlistAck.PayLoad as HubEnlistAckPayLoad;

            return new EnlistResult(datPayload.ReturnCode, datPayload.HostIdentifier, datPayload.PingInterval);
        }

        public IEnumerable<HubHostInfo> GetHostlist(bool getServers, bool getClients, bool ignorePasswordProtected)
        {
            var message = HubMessageFactory.GetMessage(HubPacketTypes.HOSTLISTREQ);
            var payload = message.PayLoad as HubHostlistReqPayLoad;

            payload.IgnoreRequiredPassword = ignorePasswordProtected;
            payload.RequestClients = getClients;
            payload.RequestServers = getServers;

            _receivedHostlistDat = new ManualResetEventSlim();

            _tcpConnection.Send(message.GetBytes());

            if(!_receivedHostlistDat.Wait(Constants.Timeout /*90000*/))
            {
                throw new TimeoutException($"Hub didn't answer in {Constants.Timeout}ms");
            }

            var datPayload = _hostlistDat.PayLoad as HubHostlistDatPayLoad;

            return datPayload.Hosts;
        }

        public bool IsHostAvaliable(string hostname)
        {
            var message = HubMessageFactory.GetMessage(HubPacketTypes.HOSTAVAILABLEREQ);
            var payload = message.PayLoad as HubHostavailableReqPayLoad;

            payload.Hostname = hostname;

            _receivedHostavailableDat = new ManualResetEventSlim();

            _tcpConnection.Send(message.GetBytes());

            if(!_receivedHostavailableDat.Wait(Constants.Timeout))
            {
                throw new TimeoutException($"Hub didn't answer in {Constants.Timeout}ms");
            }

            var datPayload = _hostavailableDat.PayLoad as HubHostavailableDatPayLoad;

            return datPayload.IsAvailable;
        }
    }
}
