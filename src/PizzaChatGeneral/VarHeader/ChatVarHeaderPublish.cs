using PIZZA.Chat.Core;
using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIZZA.Chat.Core
{
    public class ChatVarHeaderPublish : ChatVariableHeader
    {
        private PIZZAString _whisperTarget;
        private PIZZAInt _datatype;
        private PIZZAString _sendername;

        public ChatVarHeaderPublish()
        {
        }

        private ChatVarHeaderPublish(byte[] bytes)
        {
            var list = bytes.ToList();

            Lenght = PIZZAInt5.FromBytes(list.ToArray());
            list.RemoveRange(0, 5);

            _datatype = PIZZAInt.FromBytes(list.ToArray());
            list.RemoveRange(0, 2);

            _sendername = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _sendername.Length.Value);

            _whisperTarget = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _whisperTarget.Length.Value);

            if (list.Count !=0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// datatype of the payload
        /// </summary>
        public ChatPayloadDatatypes Datatype
        {
            get { return (ChatPayloadDatatypes)_datatype.Value; }
            set { _datatype.Value = (int)value; }
        }

        /// <summary>
        /// target client name for whispering if not wantet set to string.empty
        /// </summary>
        public string WhisperTarget
        {
            get { return _whisperTarget.Value;}

            set { _whisperTarget.Value = value; }
        }

        /// <summary>
        /// name of the Sender Has to be the same as ClientID
        /// </summary>
        public string SenderName
        {
            get { return _sendername.Value; }
            set { _sendername.Value = value; }
        }

        /// <summary>
        /// Returns the variable header from th byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatVarHeaderPublish FromBytes(byte[] bytes)
        {
            return new ChatVarHeaderPublish(bytes);
        }

        /// <summary>
        /// Returns a byte[] that resembles the Var Header
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Lenght.GetBytes());
            bytes.AddRange(_datatype.GetBytes());
            bytes.AddRange(_sendername.GetBytes());
            bytes.AddRange(_whisperTarget.GetBytes());

            return bytes.ToArray();
        }
    }
}
