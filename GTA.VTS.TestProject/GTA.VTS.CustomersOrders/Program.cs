using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GTA.VTS.CustomersOrders.AppForm;
using GTA.VTS.CustomersOrders.BLL;
using System.Globalization;


namespace GTA.VTS.CustomersOrders
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

            //string txt = CultureInfo.InstalledUICulture.Name;
            //先初始化资源文件内容zh-CN
            ResourceOperate.Instanse.InitResourceLocal(CultureInfo.InstalledUICulture.Name);
            LoginForm login = new LoginForm();
            //mainForm = new MainForm();
            mainForm = new mdiMainForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(mainForm);
            }
            mainForm.Close();
        }

        //public static MainForm mainForm;
        public static mdiMainForm mainForm;
    }
}
