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
        private byte   _pinginterval = new byte();
        private PIZZAInt _hostIdentifier = new PIZZAInt();

        public HubEnlistAckPayLoad()
        { }

        private HubEnlistAckPayLoad(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();

            _returncode = (HubReturnCodes)tmp[0];

            tmp.RemoveAt(0);

            _pinginterval = tmp[0];

            tmp.RemoveAt(0);

            _hostIdentifier = PIZZAInt.FromBytes(tmp.ToArray());

            tmp.Clear();
        }

        public int HostIdentifier
        {
            get
            {
                return _hostIdentifier.Value;
            }
            set
            {
                _hostIdentifier.Value = value;
            }
        }

        public int PingInterval
        {
            get
            {
                return _pinginterval;
            }
            set
            {
                _pinginterval = (byte)value;
            }
        }

        public HubReturnCodes ReturnCode
        {
            get
            {
                return _returncode;
            }
            set
            {
                _returncode = value;
            }
        }

        public static HubEnlistAckPayLoad FromBytes(byte[] bytes)
        {
            HubEnlistAckPayLoad result = new HubEnlistAckPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[] { (byte)_returncode };

            return result.Concat(new byte[] { _pinginterval }).Concat(_hostIdentifier.GetBytes()).ToArray();

        }
    }
}
