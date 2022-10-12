using goABClient.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goABClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new SocketCilent();
            LoginPage loginPage = new LoginPage();
            loginPage.ShowDialog();
            if (loginPage.DialogResult==DialogResult.OK)
            {
                Application.Run(new MainPage());
            }
        }
    }
}
