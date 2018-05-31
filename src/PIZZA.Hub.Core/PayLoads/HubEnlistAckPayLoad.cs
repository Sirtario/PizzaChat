using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;
using PIZZA.Hub.Core.Enumerationen;

namespace PIZZA.Hub.Core.PayLoads
{
    public class HubEnlistAckPayLoad : HubPayLoad
    {
        private HubReturnCodes _returncode = new HubReturnCodes();
        private byte   _pingintervall = new byte();
        private PIZZAInt _hostIdentifier = new PIZZAInt();

        public HubEnlistAckPayLoad()
        { }

        private HubEnlistAckPayLoad(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();

            _returncode = (HubReturnCodes)tmp[0];

            tmp.RemoveAt(0);

            _pingintervall = tmp[0];

            tmp.RemoveAt(0);

            _hostIdentifier = PIZZAInt.FromBytes(tmp.ToArray());

            tmp.Clear();
        }

        public static HubEnlistAckPayLoad FromBytes(byte[] bytes)
        {
            HubEnlistAckPayLoad result = new HubEnlistAckPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[] { (byte)_returncode };

            return result.Concat(new byte[] { _pingintervall }).Concat(_hostIdentifier.GetBytes()).ToArray();

        }
    }
}
