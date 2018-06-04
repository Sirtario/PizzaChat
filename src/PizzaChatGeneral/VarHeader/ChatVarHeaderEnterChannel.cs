﻿using PIZZA.Chat.Core;
using PIZZA.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PIZZA.Chat.Core
{
    public class ChatVarHeaderEnterChannel : ChatVariableHeader
    {
        private PIZZAString _channel;
        private PIZZAString _password;

        public ChatVarHeaderEnterChannel()
        {
        }

        private ChatVarHeaderEnterChannel(byte[] bytes)
        {
            var list = bytes.ToList();

            Lenght = PIZZAInt5.FromBytes(list.ToArray());
            list.RemoveRange(0, 5);

            _channel = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _channel.Length.Value);

            _password = PIZZAString.FromBytes(list.ToArray());
            list.RemoveRange(0, _password.Length.Value);

            if (list.Count != 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// password if any, NOTE: set password to string.empty if no password is required
        /// </summary>
        public string Password
        {
            get
            {
                return _password.Value;
            }
            set
            {
                _password.Value = value;
            }
        }

        /// <summary>
        /// Name of the channel to enter
        /// </summary>
        public string Channel
        {
            get
            {
                return _channel.Value;
            }
            set
            {
                _channel.Value = value;
            }
        }

        /// <summary>
        /// Returns a new variable header from the Byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ChatVarHeaderEnterChannel FromBytes(byte[] bytes)
        {
            return new ChatVarHeaderEnterChannel(bytes);
        }

        /// <summary>
        /// Gets a byte[] that contains the Variable header
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();

            bytes.AddRange(Lenght.GetBytes());
            bytes.AddRange(_channel.GetBytes());
            bytes.AddRange(_password.GetBytes());

            return bytes.ToArray();
        }
    }
}
