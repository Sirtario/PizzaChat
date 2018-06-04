using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    class HubHostlistDatPayLoad : HubPayLoad
    {
        private List<HubHostInfo> _hosts = new List<HubHostInfo>();

        public HubHostlistDatPayLoad()
        { }

        private HubHostlistDatPayLoad(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();

            while (tmp.Count != 0)
            {
                HubHostInfo _current = HubHostInfo.FromBytes(tmp.ToArray());

                tmp.RemoveRange(0, _current.Length);

                _hosts.Add(_current);
            }

        }

        public static HubHostlistDatPayLoad FromBytes(byte[] bytes)
        {
            HubHostlistDatPayLoad result = new HubHostlistDatPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            List<byte> result = new List<byte>();

            foreach(HubHostInfo i in _hosts)
            {
                result.Concat(i.GetBytes());
            }

            return result.ToArray();
        }
    }
}
