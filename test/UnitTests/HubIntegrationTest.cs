using System;
using ACX.ViciOne.TCPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Hub;
using PIZZA.Hub.Client;
using PIZZA.Hub.Core;
using Tests.Mocks;

namespace Tests
{
    [TestClass]
    public class HubIntegrationTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // making the HUb
            var hubtcpmock = new TCPServer(TcpDelegate.IsPIZZAHubMessageComplete);

            HubPizzaServerList hubserverlist = new HubPizzaServerList();
            HubPizzaClientList hubclientlist = new HubPizzaClientList();
            HubRespondingHosts hubresphost = new HubRespondingHosts();

            var hubserver = new HubServerMessageHandler(hubtcpmock, hubserverlist, hubclientlist, hubresphost);

            //making the Client

            var Pizzahubclient = new HubClient();

            Pizzahubclient.Connect("127.0.0.1", 56602);
            var res = Pizzahubclient.ListHost("bam", true, string.Empty, "pow", "bla", false);

            if (res.Returncode != PIZZA.Hub.Core.Enumerationen.HubReturnCodes.ACCEPTED)
            {
                throw new Exception(res.Returncode.ToString());
            }
            Pizzahubclient.StartPing(res.HostIdentifier, res.Pinginterval);
        }

        [TestMethod]
        public void Test()
        {
            // making the HUb
            var hubtcpmock = new TCPServer(TcpDelegate.IsPIZZAHubMessageComplete);

            HubPizzaServerList hubserverlist = new HubPizzaServerList();
            HubPizzaClientList hubclientlist = new HubPizzaClientList();
            HubRespondingHosts hubresphost = new HubRespondingHosts();

            var hubserver = new HubServerMessageHandler(hubtcpmock, hubserverlist, hubclientlist, hubresphost);

            var testcon = new TCPClient(TcpDelegate.IsPIZZAHubMessageComplete);
            testcon.Connect("127.0.0.1", 56602);
            Assert.AreEqual(56602, testcon.RemoteEndpoint.Port);
            Assert.AreEqual("127.0.0.1", testcon.RemoteEndpoint.Address.ToString());
        }

    }
}
