using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts.Msg
{
    public class ReqLogin
    {
        public string Username;
        public string Password;
        public ReqLogin(string name, string pass)
        {
            Username = name;
            Password = pass;
        }
    }

    public class RspLogin
    {
        public string State { get; set; }
        public string Text { get; set; }
    }
}
