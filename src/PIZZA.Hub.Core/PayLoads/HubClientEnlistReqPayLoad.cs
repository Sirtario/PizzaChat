using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubClientEnlistReqPayLoad : HubPayLoad
    {

        private PIZZAString _hostname = new PIZZAString();
        private PIZZAString _friendlyname = new PIZZAString();
        private PIZZAString _description = new PIZZAString();
        private byte      _flags = new byte();

        public HubClientEnlistReqPayLoad()
        { }

        private HubClientEnlistReqPayLoad(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();

            _hostname = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, _hostname.Length.Value + 2);

            _friendlyname = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, _friendlyname.Length.Value + 2);

            _description = PIZZAString.FromBytes(tmp.ToArray());

            tmp.RemoveRange(0, _description.Length.Value + 2);

            _flags = tmp[0];

            tmp.Clear();
        }

        public static HubClientEnlistReqPayLoad FromBytes(byte[] bytes)
        {
            HubClientEnlistReqPayLoad result = new HubClientEnlistReqPayLoad(bytes);
            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = _hostname.GetBytes();
            
            return result.Concat(_friendlyname.GetBytes()).Concat(_description.GetBytes()).Concat(new byte[] { _flags }).ToArray();
        }
    }
}
