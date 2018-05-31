using System;
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

            var exp = new byte[] {0x50,0x49, 0x5a,0x5a, 0x41,0x43,1,1,/*hier remaining length*/13,0,0,0,0,/*hier varheader length*/13,0,4,0x74,0x65,0x73,0x74,0,0 };

            Assert.IsTrue(res.CompareMenberwise(exp));
        }
    }
}
