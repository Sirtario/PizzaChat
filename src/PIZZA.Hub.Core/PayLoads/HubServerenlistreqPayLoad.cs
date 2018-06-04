using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubServerenlistreqPayLoad : HubPayLoad
    {
        private PIZZAString _hostname = new PIZZAString();
        private PIZZAString _friendlyname = new PIZZAString();
        private PIZZAString _description = new PIZZAString();
        private byte   _flags = new byte();

        public HubServerenlistreqPayLoad()
        { }

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

        public bool PasswordRequired
        {
            get
            {
                return (_flags & 1) == 1;
            }
            set
            {
                _flags = (byte)(_flags & 254);
                _flags |= (byte)(value ? 1 : 0);
            }
        }

        private HubServerenlistreqPayLoad(byte[] bytes)
        {
             List<byte> tmp = bytes.ToList();
            _hostname = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0,_hostname.Length.Value + 2 );

            _friendlyname = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, _friendlyname.Length.Value + 2);

            _description = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, _description.Length.Value + 2);

            _flags = tmp[0];

            tmp.Clear();
        }


        public static HubServerenlistreqPayLoad FromBytes(byte[] bytes)
        {
            HubServerenlistreqPayLoad result = new HubServerenlistreqPayLoad(bytes);
            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = _hostname.GetBytes();
            
            return result.Concat(_friendlyname.GetBytes()).Concat(_description.GetBytes()).Concat(new byte[] { _flags }).ToArray();
        }
    }
}
