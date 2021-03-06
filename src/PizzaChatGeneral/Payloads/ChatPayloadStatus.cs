using PIZZA.Chat.Core;
using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatPayloadStatus : ChatPayload
    {
        private List<PIZZAString> _clientsInCurrentChannel = new List<PIZZAString>();
        private List<PIZZAChannel> _channels = new List<PIZZAChannel>();

        public ChatPayloadStatus()
        {
        }

        private ChatPayloadStatus(byte[] bytes, ChatVarHeaderStatus varHeader)
        {
            _clientsInCurrentChannel = new List<PIZZAString>();
            _channels = new List<PIZZAChannel>();

            var list = bytes.ToList();

            for (int i = 0; i < varHeader.ClientCount; i++)
            {
                _clientsInCurrentChannel.Add(PIZZAString.FromBytes(list.ToArray()));
                list.RemoveRange(0, _clientsInCurrentChannel[_clientsInCurrentChannel.Count - 1].GetBytes().Length);
            }

            for (int i = 0; i < varHeader.ChannelCount; i++)
            {
                _channels.Add(PIZZAChannel.FromBytes(list.ToArray()));
                list.RemoveRange(0, _channels[_channels.Count - 1].Length);
            }

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        public void AddUser(string clientID)
        {
            _clientsInCurrentChannel.Add((new PIZZAString() { Value = clientID }));
        }

        public void AddChannel(PIZZAChannel channel)
        {
            _channels.Add(channel);
        }

        public IEnumerable<string> ClientsInCurrentChannel
        {
            get
            {
                return _clientsInCurrentChannel.Select(prop => prop.Value).ToList();
            }
        }

        public IEnumerable<PIZZAChannel> Channels
        {
            get
            {
                return _channels;
            }
        }

        public static ChatPayloadStatus FromBytes(byte[] bytes, ChatVarHeaderStatus varHeader)
        {
            return new ChatPayloadStatus(bytes, varHeader);
        }

        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();

            foreach (var client in _clientsInCurrentChannel)
            {
                bytes.AddRange(client.GetBytes());
            }

            foreach (var Channels in _channels)
            {
                bytes.AddRange(Channels.GetBytes());
            }

            return bytes.ToArray();
        }
    }
}
