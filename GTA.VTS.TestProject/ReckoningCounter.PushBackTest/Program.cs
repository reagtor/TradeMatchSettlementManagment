using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ReckoningCounter.PushBackTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoginForm login = new LoginForm();
            MainForm = new Form1();

            string errMsg = "";
            bool isSuccess = MainForm.Initialize(ref errMsg);
            if(!isSuccess)
            {
                MessageBox.Show(errMsg);
                return;
            }

            login.ShowDialog();
            Application.Run(MainForm);
        }

        public static Form1 MainForm;
    }
}
