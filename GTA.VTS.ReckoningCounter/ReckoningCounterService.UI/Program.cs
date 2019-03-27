using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.AccountManagementAndFind.InterfaceBLL.AccountManagementAndFindService;
using System.Diagnostics;

namespace CounterServiceManager
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

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //Application.Run(new Form1());

            Application.Run(new mainfrm());
        }

        /// <summary>
        /// 在发生未捕获线程异常时发生事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

            LogHelper.WriteError("********************AppDomain.UnhandledException:*******************\r\n" + e.Exception.StackTrace, e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            LogHelper.WriteError("********************AppDomain.UnhandledException:*******************\r\n", ex);
        }
    }
}
