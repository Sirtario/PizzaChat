using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIZZA.Chat.Core;

namespace Tests
{
    [TestClass]
    public class ChatMessageTests
    {
        [TestMethod]
        public void TestConnectGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.CONNECT);
            var varheader = message.VariableHeader as ChatVarHeaderConnect;

            varheader.ClientID = "test";
            varheader.Password = string.Empty;

            var res = message.GetBytes();

            var exp = new byte[] {0x50,0x49, 0x5a,0x5a, 0x41,0x43,1,1,0,0,0,0,13,0,0,0,0,13,0,4,0x74,0x65,0x73,0x74,0,0 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestConnectfromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 1, 0, 0, 0, 0, 13, 0, 0, 0, 0, 13, 0, 4, 0x74, 0x65, 0x73, 0x74, 0, 0 });
            var varheader = message.VariableHeader as ChatVarHeaderConnect;

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.CONNECT);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise( new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(13, message.FixedHeader.RemainingLength);
            Assert.AreEqual("test", varheader.ClientID);
            Assert.AreEqual("", varheader.Password);
        }

        [TestMethod]
        public void TestConnAckGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.CONNACK);
            var varheader = message.VariableHeader as ChatVarHeaderConnAck;

            varheader.CommunicationMode = 1;
            varheader.PingIntervall = 60;
            varheader.Returncode = ChatConnectReturncode.ACCEPTED;

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 2, 0, 0, 0, 0, 8, 0, 0, 0, 0, 8, 0, 1, 60 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestConnAckFromBytes()
        {
            var message =  PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 2, 0, 0, 0, 0, 8, 0, 0, 0, 0, 8, 0, 1, 60 });
            var varheader = message.VariableHeader as ChatVarHeaderConnAck;

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.CONNACK);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(8, message.FixedHeader.RemainingLength);
            Assert.AreEqual(ChatConnectReturncode.ACCEPTED, varheader.Returncode);
            Assert.AreEqual(60, varheader.PingIntervall);
            Assert.AreEqual(1, varheader.CommunicationMode);
        }

        [TestMethod]
        public void TestGetStatusGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.GETSTATUS);
            
            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 3, 0, 0, 0, 0, 0};

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestGetStatusFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 3, 0, 0, 0, 0, 0 });
            var varheader = message.VariableHeader as ChatVarHeaderConnAck;

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.GETSTATUS);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(0, message.FixedHeader.RemainingLength);
        }

        [TestMethod]
        public void TestStatusGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.STATUS);
            var varheader = message.VariableHeader as ChatVarHeaderStatus;
            var payload = message.Payload as ChatPayloadStatus;

            varheader.ChannelCount = 1;
            varheader.ChannelPower = 2;
            varheader.ClientCount = 1;
            varheader.CurrentChannel = "Default";

            payload.AddChannel(new PIZZAChannel(new PIZZA.Core.PIZZAString() { Value = "Default" }, 1));
            payload.AddUser("User");

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 4, 0, 0, 0, 0, 33,0,0,0,0,17,0,7, 0x44, 0x65,0x66,0x61,0x75,0x6c,0x74,2,1,1,0,4,0x55,0x73,0x65,0x72, 0, 7, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6c, 0x74 ,1};

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestPingGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.PING);

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 5, 0, 0, 0, 0, 0 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestPingFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 5, 0, 0, 0, 0, 0 });

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.PING);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(0, message.FixedHeader.RemainingLength);
        }

        [TestMethod]
        public void TestPingRESPGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.PINGRESP);

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 6, 0, 0, 0, 0, 0 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestPingRESPFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 6, 0, 0, 0, 0, 0 });

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.PINGRESP);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(0, message.FixedHeader.RemainingLength);
        }

        [TestMethod]
        public void TestEnterChannelGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.ENTERCHANNEL);
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannel;

            varheader.Channel = "Default";
            varheader.Password = string.Empty;

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 7, 0, 0, 0, 0, 16 ,0,0,0,0, 16 ,0, 7, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6c, 0x74,0,0 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestEnterChannelFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 7, 0, 0, 0, 0, 16, 0, 0, 0, 0, 16, 0, 7, 0x44, 0x65, 0x66, 0x61, 0x75, 0x6c, 0x74, 0, 0 });
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannel;

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.ENTERCHANNEL);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(16, message.FixedHeader.RemainingLength);
            Assert.AreEqual("Default", varheader.Channel);
            Assert.AreEqual(string.Empty, varheader.Password);
        }

        [TestMethod]
        public void TestEnterChannelAckGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.ENTERCHANNELACK);
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannelAck;

            varheader.ReturnCode = ChatEnterChannelReturnCode.Acceptded;

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 8, 0, 0, 0, 0, 6, 0, 0, 0, 0, 6, 0 };
        }

        [TestMethod]
        public void TestEnterChannelAckFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 8, 0, 0, 0, 0, 6, 0, 0, 0, 0, 6, 0 });
            var varheader = message.VariableHeader as ChatVarHeaderEnterChannelAck;

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.ENTERCHANNELACK);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(6, message.FixedHeader.RemainingLength);

            Assert.AreEqual(ChatEnterChannelReturnCode.Acceptded, varheader.ReturnCode);
        }

        [TestMethod]
        public void TestDisconnectGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.DISCONNECT);

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 9, 0, 0, 0, 0, 0 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestDisconnectFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 9, 0, 0, 0, 0, 0 });

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.DISCONNECT);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(0, message.FixedHeader.RemainingLength);
        }

        [TestMethod]
        public void TestPublishGetBytes()
        {
            var message = new PizzaChatMessage(Packettypes.PUBLISH);
            var varheader = message.VariableHeader as ChatVarHeaderPublish;
            var payload = message.Payload as ChatPayloadPublish;

            varheader.Datatype = ChatPayloadDatatypes.Text;
            varheader.SenderName = "Ben";
            varheader.WhisperTarget = string.Empty;

            payload.Payload = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var res = message.GetBytes();

            var exp = new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 10, 0, 0, 0, 0, 24, 0, 0, 0, 0, 14, 0, 0, 0, 3, 0x42, 0x65, 0x6e, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }

        [TestMethod]
        public void TestPublishFromBytes()
        {
            var message = PizzaChatMessage.FromBytes(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43, 1, 10, 0, 0, 0, 0, 24 ,0,0,0,0,14,0,0,0,3,0x42,0x65,0x6e,0,0,0,1,2,3,4,5,6,7,8,9});
            var varheader = message.VariableHeader as ChatVarHeaderPublish;
            var payload = message.Payload as ChatPayloadPublish;

            Assert.AreEqual(message.FixedHeader.PacketType, Packettypes.PUBLISH);
            Assert.IsTrue(message.FixedHeader.Protokollname.CompareMenberwise(new byte[] { 0x50, 0x49, 0x5a, 0x5a, 0x41, 0x43 }));
            Assert.AreEqual(1, message.FixedHeader.ProtokollVersion);
            Assert.AreEqual(24, message.FixedHeader.RemainingLength);

            Assert.AreEqual("Ben", varheader.SenderName);
            Assert.AreEqual(string.Empty, varheader.WhisperTarget);
            Assert.AreEqual(ChatPayloadDatatypes.Text, varheader.Datatype);

            Assert.IsTrue(payload.Payload.CompareMenberwise(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
        }
    }
}
