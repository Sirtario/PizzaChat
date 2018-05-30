using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core.PayLoads
{
    class HubHostlistDatPayLoad : HubPayLoad
    {
        private List<HubHostInfo> _hosts;

        private HubHostlistDatPayLoad(byte[] bytes)
        {
            List<byte> tmp = bytes.ToList();

            while (tmp.Count != 0)
            {
                HubHostInfo _current = new HubHostInfo();

                _current.Hostname = PIZZAString.FromBytes(tmp.ToArray());

                tmp.RemoveRange(0, 2);

                _current.Friendlyname = PIZZAString.FromBytes(tmp.ToArray());

                tmp.RemoveRange(0, 2);

                _current.Description = PIZZAString.FromBytes(tmp.ToArray());

                tmp.RemoveRange(0, 2);

                _current.RequiresPassword = tmp[0];

                tmp.RemoveAt(0);

                _hosts.Add(_current);
            }

        }

        public HubHostlistDatPayLoad FromBytes(byte[] bytes)
        {
            HubHostlistDatPayLoad result = new HubHostlistDatPayLoad(bytes);

            return result;
        }

        public byte[] GetBytes()
        {
            List<byte> result = new List<byte>();

            foreach(HubHostInfo i in _hosts)
            {
                byte[] tmp = i.Hostname.GetBytes();
                tmp.Concat(i.Friendlyname.GetBytes()).Concat(i.Description.GetBytes()).Concat(new byte[] { i.RequiresPassword });

                foreach (byte b in tmp)
                {
                    result.Add(b);
                }

            }

            return result.ToArray();
        }
    }
}
