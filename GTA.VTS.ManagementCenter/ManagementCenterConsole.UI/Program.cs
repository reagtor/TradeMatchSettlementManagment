using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using ManagementCenterConsole.UI;

namespace ManagementCenterConsole.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Money Twins");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LoginFrm());
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (CheckInstance())
            {
                CommonClass.ShowMessageBox.ShowInformation("您已经启动了此程序！");
                return;
            }
            if (new LoginFrm().ShowDialog() == DialogResult.OK)
            {
                Application.Run(MainForm.Instance);
            }    
        }

        #region 检查系统是否已启动过一次了

        private static bool CheckInstance()
        {
            Process instance = RunningInstance();
            if (instance != null)
            {
                HandleRunningInstance(instance);
                return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// 将进程的主窗体显示出来
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            const int SHOWMAXIMIZED = 2;
            ShowWindowAsync(instance.MainWindowHandle, SHOWMAXIMIZED);
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


        /// <summary>
        /// 取得应用程序的当前实例
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            LogHelper.WriteError("********************管理中心控制台Domain异常:*******************\r\n", ex);
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // LogHelper.WriteError(e.Exception.Message, e.Exception);

            LogHelper.WriteError("********************管理中心控制台Application异常:*******************\r\n", e.Exception);
        } 

    }
}