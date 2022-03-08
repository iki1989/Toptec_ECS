using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Util
{
    public static class PingTest
    {
        public static bool EnablePing(string ip)
        {
            PingReply reply = new Ping().Send(ip, 3);

            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }
    }
}
