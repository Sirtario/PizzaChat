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
        private List<PIZZAString> _clientsInCurrentChannel;
        private List<PIZZAChannel> _channels;
        private int _clientcount;
        private int _channelcount;

        public ChatPayloadStatus()
        {
        }

        public ChatPayloadStatus(int clientcount, int channelcount)
        {
            _clientcount = clientcount;
            _channelcount = channelcount;
        }

        private ChatPayloadStatus(byte[] bytes)
        {
            _clientsInCurrentChannel = new List<PIZZAString>();
            _channels = new List<PIZZAChannel>();

            var list = bytes.ToList();

            for (int i = 0; i < _clientcount; i++)
            {
                _clientsInCurrentChannel.Add(PIZZAString.FromBytes(list.ToArray()));
                list.RemoveRange(0, _clientsInCurrentChannel[_clientsInCurrentChannel.Count - 1].Length.Value);
            }

            for (int i = 0; i < _channelcount; i++)
            {
                _channels.Add(PIZZAChannel.FromBytes(list.ToArray()));
                list.RemoveRange(0, _channels[_channels.Count - 1].Length);
            }

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        public IEnumerable<string> ClientsInCurrentChannel
        {
            get
            {
                return _clientsInCurrentChannel.Select(prop => prop.Value).ToList();
            }
        }

        public IEnumerable<string> Channels
        {
            get
            {
                return _channels.Select(prop => prop.Channelname.Value).ToList();
            }
        }

        public static ChatPayloadStatus FromBytes(byte[] bytes)
        {
            return new ChatPayloadStatus(bytes);
        }

        public byte[] GetBytes()
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
