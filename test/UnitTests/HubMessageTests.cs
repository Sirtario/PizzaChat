using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Hub.Core;
using PIZZA.Hub.Core.PayLoads;

namespace Tests
{
    [TestClass]
    public class HubMessageTests
    {
        [TestMethod]
        public void TestGetBytesHubCLIENTENLISTREQMessage()
        {
            var message = HubMessageFactory.GetMessage(HubPacketTypes.CLIENTENLISTREQ);

            var payload = message.PayLoad as HubClientEnlistReqPayLoad;

            payload.Description = "test";
            payload.Friendlyname = "Payload";
            payload.Hostname = "bla.com";
            payload.PasswordRequired = false;

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48, 1, 2, 0, 0, 25,0,7 ,0x62,0x6c,0x61,0x2e,0x63,0x6f,0x6d,0,7,0x50,0x61,0x79,0x6c,0x6f,0x61,0x64,0,4,0x74,0x65,0x73,0x74,0};
            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestGetBytesServerEnlistReqMessage()
        {
            var message = HubMessageFactory.GetMessage(HubPacketTypes.SERVERENLISTREQ);
            var payload = message.PayLoad as HubServerenlistreqPayLoad;

            payload.Description = "test";
            payload.Friendlyname = "Payload";
            payload.Hostname = "bla.com";
            payload.PasswordRequired = false;

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48, 1, 1, 0, 0, 25, 0, 7, 0x62, 0x6c, 0x61, 0x2e, 0x63, 0x6f, 0x6d, 0, 7, 0x50, 0x61, 0x79, 0x6c, 0x6f, 0x61, 0x64, 0, 4, 0x74, 0x65, 0x73, 0x74, 0 };
            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestFromBytesServerEnlistReqMessage()
        {
            var message = HubMessageFactory.GetMessage(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48, 1, 1, 0, 0, 25, 0, 7, 0x62, 0x6c, 0x61, 0x2e, 0x63, 0x6f, 0x6d, 0, 7, 0x50, 0x61, 0x79, 0x6c, 0x6f, 0x61, 0x64, 0, 4, 0x74, 0x65, 0x73, 0x74, 0 });

            var payload = message.PayLoad as HubServerenlistreqPayLoad;

            Assert.IsTrue(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48 }.CompareMenberwise(message.Header.ProtocollName));
            Assert.AreEqual(1, message.Header.ProtocollVersion);
            Assert.AreEqual(HubPacketTypes.SERVERENLISTREQ, message.Header.PacketType);
            Assert.AreEqual(25, message.Header.PayloadLength);

            Assert.AreEqual("test",payload.Description);
            Assert.AreEqual("Payload", payload.Friendlyname);
            Assert.AreEqual( "bla.com", payload.Hostname);
            Assert.IsFalse(payload.PasswordRequired);
        }
    }
}
