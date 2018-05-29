using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using PIZZA.Core;

namespace PIZZA.Hub.Core
{
    class HubServerunlistreqPayLoad : HubPayLoad
    {
        private PIZZAString _hostname;
        private PIZZAString _friendlyname;
        private PIZZAString _description;
        private byte   _flags;

        private HubServerunlistreqPayLoad(byte[] bytes)
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


        public static HubServerunlistreqPayLoad FromBytes(byte[] bytes)
        {
            HubServerunlistreqPayLoad result = new HubServerunlistreqPayLoad(bytes);
            return result;
        }

        public byte[] GetBytes()
        {
            List<byte> result =  ;
            

            return 
        }
    }
}
