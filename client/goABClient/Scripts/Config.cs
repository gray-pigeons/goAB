using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts
{
    public static class Config
    {
        public static string IPAddress { get; set; } = "127.0.0.1";
        public static int Port { get; set; } = 8081;

        /// <summary>
        /// 连接状态
        /// </summary>
        public static ConnEnumStateCode ConnectState { get; set; }





    }
}
