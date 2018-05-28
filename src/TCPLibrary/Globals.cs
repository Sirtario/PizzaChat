using System.Net;

namespace ACX.ViciOne.TCPLibrary
{
    public static class Globals
    {
        public static bool EqualsAddress(this IPEndPoint ipendpoint, IPEndPoint other)
        {
            return ipendpoint.Address.Equals(other.Address) && ipendpoint.Port == other.Port;
        }
    }
}
