using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class PizzaChatMessage
    {
        public PizzaChatMessage(Packettypes type)
        {
            switch (type)
            {
                case Packettypes.CONNECT:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVarHeaderConnect();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.CONNACK:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVarHeaderConnAck();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.GETSTATUS:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVariableHeader();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.STATUS:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVarHeaderStatus();
                    Payload = new ChatPayloadStatus();
                    break;
                case Packettypes.PING:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVariableHeader();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.PINGRESP:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVariableHeader();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.ENTERCHANNEL:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVarHeaderEnterChannel();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.ENTERCHANNELACK:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVarHeaderEnterChannelAck();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.DISCONNECT:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVariableHeader();
                    Payload = new ChatPayload();
                    break;
                case Packettypes.PUBLISH:
                    FixedHeader = new ChatFixedHeader(type);
                    VariableHeader = new ChatVarHeaderPublish();
                    Payload = new ChatPayloadPublish();
                    break;
                default:
                    throw new NotImplementedException($"unknown Packettype {type.ToString()}");
            }
        }

        private PizzaChatMessage(byte[] bytes)
        {
            var list = bytes.ToList();

            FixedHeader = ChatFixedHeader.FromBytes(list.GetRange(0,13).ToArray());

            list.RemoveRange(0, 13);

            switch (FixedHeader.PacketType)
            {
                case Packettypes.CONNECT:
                    VariableHeader = ChatVarHeaderConnect.FromBytes(list.ToArray());
                    list.RemoveRange(0, VariableHeader.GetBytes().Length);
                    break;
                case Packettypes.CONNACK:
                    VariableHeader = ChatVarHeaderConnAck.FromBytes(list.ToArray());
                    list.RemoveRange(0, VariableHeader.GetBytes().Length);
                    break;
                case Packettypes.GETSTATUS:
                    VariableHeader = new ChatVariableHeader();
                    break;
                case Packettypes.STATUS:
                    VariableHeader = ChatVarHeaderStatus.FromBytes(list.ToArray());
                    list.RemoveRange(0, VariableHeader.GetBytes().Length);
                    break;
                case Packettypes.PING:
                    VariableHeader = new ChatVariableHeader();
                    break;
                case Packettypes.PINGRESP:
                    VariableHeader = new ChatVariableHeader();
                    break;
                case Packettypes.ENTERCHANNEL:
                    VariableHeader = ChatVarHeaderEnterChannel.FromBytes(list.ToArray());
                    list.RemoveRange(0, VariableHeader.GetBytes().Length);
                    break;
                case Packettypes.ENTERCHANNELACK:
                    VariableHeader = ChatVarHeaderEnterChannelAck.FromBytes(list.ToArray());
                    list.RemoveRange(0, VariableHeader.GetBytes().Length);
                    break;
                case Packettypes.DISCONNECT:
                    VariableHeader = new ChatVariableHeader();
                    break;
                case Packettypes.PUBLISH:
                    VariableHeader = ChatVarHeaderPublish.FromBytes(list.ToArray());
                    list.RemoveRange(0, VariableHeader.GetBytes().Length);
                    break;
                default:
                    throw new NotImplementedException($"unknown Packettype {FixedHeader.PacketType.ToString()}");
            }

            switch (FixedHeader.PacketType)
            {
                case Packettypes.STATUS:
                    Payload = ChatPayloadStatus.FromBytes(list.ToArray(), VariableHeader as ChatVarHeaderStatus);
                    list.RemoveRange(0, Payload.GetBytes().Length);
                    break;
                case Packettypes.PUBLISH:
                    Payload = ChatPayloadPublish.FromBytes(list.ToArray());
                    list.RemoveRange(0, Payload.GetBytes().Length);
                    break;
                default:
                    Payload = new ChatPayload();
                    list.RemoveRange(0, Payload.GetBytes().Length);
                    break;
            }

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        public ChatFixedHeader FixedHeader { get; private set; }

        public ChatPayload Payload { get; private set; }

        public ChatVariableHeader VariableHeader { get; private set; }

        public static PizzaChatMessage FromBytes(byte[] bytes)
        {
            return new PizzaChatMessage(bytes);
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(VariableHeader.GetBytes());
            bytes.AddRange(Payload.GetBytes());

            FixedHeader.RemainingLength = bytes.Count;

            bytes.InsertRange(0, FixedHeader.GetBytes(bytes.Count));
            
            return bytes.ToArray();
        }
    }
}
