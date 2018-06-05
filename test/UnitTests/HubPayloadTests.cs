using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Hub.Core.PayLoads;

namespace Tests
{
    [TestClass]
    public class HubPayloadTests
    {
        [TestMethod]
        public void TestGetBytesHubClientEnlistReqPayLoad()
        {
            var payload = new HubClientEnlistReqPayLoad();

            payload.Description = "test";
            payload.Friendlyname = "Payload";
            payload.Hostname = "bla.com";
            payload.PasswordRequired = false;

            var res = payload.GetBytes();

            var exp = new byte[] {  0, 7, 0x62, 0x6c, 0x61, 0x2e, 0x63, 0x6f, 0x6d, 0, 7, 0x50, 0x61, 0x79, 0x6c, 0x6f, 0x61, 0x64, 0, 4, 0x74, 0x65, 0x73, 0x74, 0 };
            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestFromBytesHubClientEnlistReqPayLoad()
        {
            var payload = HubClientEnlistReqPayLoad.FromBytes(new byte[] { 0, 7, 0x62, 0x6c, 0x61, 0x2e, 0x63, 0x6f, 0x6d, 0, 7, 0x50, 0x61, 0x79, 0x6c, 0x6f, 0x61, 0x64, 0, 4, 0x74, 0x65, 0x73, 0x74, 0 });

            Assert.AreEqual("test", payload.Description);
            Assert.AreEqual("Payload", payload.Friendlyname);
            Assert.AreEqual("bla.com", payload.Hostname);
            Assert.IsFalse(payload.PasswordRequired);
        }

        [TestMethod]
        public void TestGetBytesHubServerenlistreqPayLoad()
        {
            var payload = new HubServerenlistreqPayLoad();

            payload.Description = "test";
            payload.Friendlyname = "Payload";
            payload.Hostname = "bla.com";
            payload.PasswordRequired = false;

            var res = payload.GetBytes();

            var exp = new byte[] { 0, 7, 0x62, 0x6c, 0x61, 0x2e, 0x63, 0x6f, 0x6d, 0, 7, 0x50, 0x61, 0x79, 0x6c, 0x6f, 0x61, 0x64, 0, 4, 0x74, 0x65, 0x73, 0x74, 0 };
            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestFromBytesHubServerenlistreqPayLoad()
        {
            var payload = HubServerenlistreqPayLoad.FromBytes(new byte[] { 0, 7, 0x62, 0x6c, 0x61, 0x2e, 0x63, 0x6f, 0x6d, 0, 7, 0x50, 0x61, 0x79, 0x6c, 0x6f, 0x61, 0x64, 0, 4, 0x74, 0x65, 0x73, 0x74, 0 });

            Assert.AreEqual("test", payload.Description);
            Assert.AreEqual("Payload", payload.Friendlyname);
            Assert.AreEqual("bla.com", payload.Hostname);
            Assert.IsFalse(payload.PasswordRequired);
        }
    }
}
