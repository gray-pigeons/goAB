using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            Console.WriteLine(username);
            Console.WriteLine(passworld);

            if (username.Length>6&&passworld.Length>6)
            {
                MessageBox.Show(string.Format("用户{0}登录成功,密码为{1}",username,passworld),"登录结果",MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
            }

        }

    }
}
