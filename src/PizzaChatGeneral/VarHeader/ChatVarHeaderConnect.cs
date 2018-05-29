using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatVarHeaderConnect : ChatVariableHeader
    {
        private PIZZAString _clientID;

        private PIZZAString _password;

        public ChatVarHeaderConnect()
        {
            _clientID = new PIZZAString();
            _password = new PIZZAString();
        }

        private ChatVarHeaderConnect(byte[] bytes)
        {
            var list = bytes.ToList();

            Lenght = PIZZAInt5.FromBytes(list.ToArray());
            list.RemoveRange(0, 5);

            _clientID = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _clientID.Length.Value);

            _password = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _password.Length.Value);

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        public string ClientID {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }

        public string Password
        {
            get { return _password.Value; }
            set { _password.Value = value; }
        }

        public static ChatVarHeaderConnect FromBytes(byte[] bytes)
        {
            return new ChatVarHeaderConnect(bytes);
        }

        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Lenght.GetBytes());
            bytes.AddRange(_clientID.GetBytes());
            bytes.AddRange(_password.GetBytes());

            return bytes.ToArray();
        }
    }
}
