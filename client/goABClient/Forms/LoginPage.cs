using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using goABClient.Scripts;


namespace goABClient
{
    public  partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            string username = txtBox_username.Text;
            string passworld = txtBox_password.Text;

            Console.WriteLine("username="+username);

            if (Config.ConnectState == ConnEnumStateCode.ConnectFailed || Config.ConnectState == ConnEnumStateCode.DisConnect)
            {
                SocketCilent.Instance.InitClient();
            }
            int waitTime = 0;
            bool successState = true;
            while (successState)
            {
                if (Config.ConnectState == ConnEnumStateCode.Connected)
                {
                    successState = false;
                }

                Task.Delay(500); //毫秒
                waitTime += 500;
                if (waitTime >= 9000)
                {
                    if (Config.ConnectState != ConnEnumStateCode.Connected )
                    {
                        return;
                    }
                }
            }
                Console.WriteLine("pass="+passworld);


            //发送登录消息



            if (username.Length>6&&passworld.Length>6)
            {
                MessageBox.Show(string.Format("用户{0}登录成功,密码为{1}",username,passworld),"登录结果",MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
            }

        }

    }
}
