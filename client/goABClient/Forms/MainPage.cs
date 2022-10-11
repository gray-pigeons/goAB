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
    public partial class MainPage : Form
    {



        public MainPage()
        {
            InitializeComponent();
        }

        private void Btn_send_Click(object sender, EventArgs e)
        {
            string userInput = richTextBox1_userInput.Text;
        }

        private void richTextBox1_showText_TextChanged(object sender, EventArgs e)
        {
            string showTxt = richTextBox1_showText.Text;

        }
    }
}
