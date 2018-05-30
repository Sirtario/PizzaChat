using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    public class HubHostInfo
    {
        private PIZZAString _hostname = new PIZZAString();
        private PIZZAString _friendlyname = new PIZZAString();
        private PIZZAString _description = new PIZZAString();
        private byte _requiresPassword;

        public string Hostname
        {
            get
            {
                return _hostname.Value;
            }
            set
            {
                _hostname.Value = value;
            }
        }

        public string Friendlyname
        {
            get
            {
                return _friendlyname.Value;
            }
            set
            {
                _friendlyname.Value = value;
            }
        }

        public string Description
        {
            get
            {
                return _description.Value;
            }
            set
            {
                _description.Value = value;
            }
        }

        public int Length => GetBytes().Length;

        private HubHostInfo(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();

            _hostname = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, 2);

            _friendlyname = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, 2);

            _description = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, 2);

            _requiresPassword = tmp[0];

            tmp.RemoveAt(0);
        }

        public static HubHostInfo FromBytes(byte[] bytes)
        {
            HubHostInfo result = new HubHostInfo(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = _hostname.GetBytes();

            return result.Concat(_friendlyname.GetBytes()).Concat(_description.GetBytes()).Concat(new byte[] { _requiresPassword }).ToArray();
        }
    }
}
