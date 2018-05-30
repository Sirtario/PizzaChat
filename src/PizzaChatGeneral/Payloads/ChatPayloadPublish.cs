using System;

namespace PIZZA.Chat.Core
{
    public class ChatPayloadPublish : ChatPayload
    {
        public ChatPayloadPublish()
        {
        }
        private ChatPayloadPublish(byte[] bytes)
        {
            Payload = bytes;
        }

        public byte[] Payload { get; set; }

        public static ChatPayload FromBytes(byte[] v)
        {
            return new ChatPayloadPublish(v);
        }

        public override byte[] GetBytes()
        {
            return Payload;
        }
    }
}