using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Chat.Core;

namespace Tests
{
    [TestClass]
    public class ChatFixedHeaderTest
    {
        [TestMethod]
        public void IsGetBytesWorking()
        {
            var header = new ChatFixedHeader(Packettypes.CONNECT);

            var res = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 1, 0, 0, 0, 0, 0 };

            Assert.IsTrue(header.GetBytes(0).CompareMenberwise(res));
        }

        [TestMethod]
        public void IsFromBytesWorking()
        {
            var header = ChatFixedHeader.FromBytes(new byte[] { 0x51, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 1, 0, 0, 0, 0, 0 });

            Assert.IsTrue(header.Protokollname.CompareMenberwise(new byte[] { 0x51, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, header.ProtokollVersion);
            Assert.AreEqual(Packettypes.CONNECT, header.PacketType);
            Assert.AreEqual(0, header.RemainingLength);
        }
        [TestMethod]
        public void IsFromBytesWorking2()
        {
            var header = ChatFixedHeader.FromBytes(new byte[] { 0x51, 0x42, 0x5a, 0x5a, 0x41, 0x43, 3, 1, 0, 0, 0, 0, 0 });

            Assert.IsTrue(header.Protokollname.CompareMenberwise(new byte[] { 0x51, 0x42, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(3, header.ProtokollVersion);
            Assert.AreEqual(Packettypes.CONNECT, header.PacketType);
            Assert.AreEqual(0, header.RemainingLength);
        }

        [TestMethod]
        public void IsFromBytesWorking3()
        {
            var header = ChatFixedHeader.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 1, 0, 0, 0, 0, 0 });

            Assert.IsTrue(header.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, header.ProtokollVersion);
            Assert.AreEqual(Packettypes.CONNECT, header.PacketType);
            Assert.AreEqual(0, header.RemainingLength);
        }
        [TestMethod]
        public void IsFromBytesWorking4()
        {
            var header = ChatFixedHeader.FromBytes(new byte[] { 0x51, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 1, 0, 0, 0, 0, 255 });

            Assert.IsTrue(header.Protokollname.CompareMenberwise(new byte[] { 0x51, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, header.ProtokollVersion);
            Assert.AreEqual(Packettypes.CONNECT, header.PacketType);
            Assert.AreEqual(255, header.RemainingLength);
        }
        [TestMethod]
        public void IsFromBytesWorking5()
        {
            var header = ChatFixedHeader.FromBytes(new byte[] { 0x51, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 1, 255, 0, 0, 0, 255 });

            Assert.IsTrue(header.Protokollname.CompareMenberwise(new byte[] { 0x51, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, header.ProtokollVersion);
            Assert.AreEqual(Packettypes.CONNECT, header.PacketType);
            Assert.AreEqual(1095216660735, header.RemainingLength);
        }
    }
}
