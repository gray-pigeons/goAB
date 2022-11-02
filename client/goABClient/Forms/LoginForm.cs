using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using goABClient.Scripts;
using goABClient.Scripts.Msg;
using goABClient.Scripts.Tools;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace goABClient
{
    public  partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            //if (Config.ConnectState == ConnEnumStateCode.ConnectFailed || Config.ConnectState == ConnEnumStateCode.DisConnect)
            //{
            //    SocketCilent.Instance.InitClient();
            //    int waitTime = 0;
            //    bool successState = true;
            //    while (successState)
            //    {
            //        if (Config.ConnectState == ConnEnumStateCode.Connected)
            //        {
            //            successState = false;
            //        }

            //        Task.Delay(500); //毫秒
            //        waitTime += 500;
            //        if (waitTime >= 9000)
            //        {
            //            if (Config.ConnectState != ConnEnumStateCode.Connected)
            //            {
            //                return;
            //            }
            //        }
            //    }
            //    Console.WriteLine("初始化连接完成=" + Config.ConnectState);
            //}



            //发送登录消息
            string username = txtBox_username.Text.Trim();
            string passworld = txtBox_password.Text.Trim();

            Console.WriteLine("username=" + username + ",pass=" + passworld);

            if (username.Length < 6 && passworld.Length < 6)
            {
                MessageBox.Show("用户名或密码长度太短", "提示信息", MessageBoxButtons.OK);
                return;
            }


            string rsp = HttpTool.PostHttp(GetLoginUrl(),new ReqLogin(username,passworld));
            RspLogin rspLogin = JsonConvert.DeserializeObject<RspLogin>(rsp);
            Console.WriteLine(rspLogin.State);
            Console.WriteLine(rspLogin.Text);

            if (rspLogin==null)
            {
                MessageBox.Show("登录失败!!!","登录结果",MessageBoxButtons.OK);
                DialogResult = DialogResult.None;
            }

            if (username.Length > 6 && passworld.Length > 6)
            {
                MessageBox.Show(string.Format("用户{0}登录成功,密码为{1}", username, passworld), "登录结果", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
            }

        }



        string GetLoginUrl()
        {
            return string.Format("http://{0}:{1}/login", Config.IPAddress, Config.HttpPort);
        }

    }


}
