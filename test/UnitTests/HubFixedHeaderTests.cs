using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Hub.Core;

namespace Tests
{
    [TestClass]
    public class HubFixedHeaderTests
    {
        [TestMethod]
        public void TestGetBytesHubHeader1()
        {
            var header = new HubHeader(HubPacketTypes.CLIENTENLISTREQ);

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48, 1, 2, 0, 0, 0 };

            var res = header.GetBytes(0);

            Assert.IsTrue(res.CompareMenberwise(exp));
        }
        [TestMethod]
        public void TestGetBytesHubHeader2()
        {
            var header = new HubHeader(HubPacketTypes.CLIENTENLISTREQ);

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48, 1, 2, 255, 255, 0 };

            var res = header.GetBytes(16776960);

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestFromBytesHubHeader1()
        {
            var header = HubHeader.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48, 1, 2, 255, 255, 0 });

            Assert.AreEqual(1, header.ProtocollVersion);
            Assert.AreEqual(16776960, header.PayloadLength);
            Assert.IsTrue(header.ProtocollName.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x48 }));
            Assert.AreEqual(HubPacketTypes.CLIENTENLISTREQ, header.PacketType);
        }
    }
}
