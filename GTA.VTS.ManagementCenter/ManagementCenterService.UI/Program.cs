using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.ServiceModel;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL;
using ManagementCenter.BLL.WCFService_Out;
using System.Threading;
using SoftDogInterface;

namespace ManagementCenterService.UI
{
    /// <summary>
    /// 描述：虚拟交易系统后台管理中心服务界面
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// 描述：添加日志
    /// 修改作者：刘书伟
    /// 修改日期：2010-04-23
    /// </summary>
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
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            //先检查加密狗
            //if (!CheckDogExist())
            //{
            //    Thread.CurrentThread.Abort();
            //    Application.Exit();
            //    return;
            //}

            Application.Run(new Form1());

        }

        #region 这只是为了加密后的异常捕捉     create by:李健华 create date:2009-06-17
        /// <summary>
         /// 线程捕捉相关异常
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //加载加密后的dll失败捕获
            if (e.Exception is System.IO.FileLoadException)
            {
                //Application.ExitThread();
                LogHelper.WriteError("********************管理中心服务Application异常:*******************\r\n", e.Exception);
                return;
            }
            //启动多个服务地址被使用不能正常启动异常捕获
            else if (e.Exception is System.ServiceModel.AddressAccessDeniedException
                     || e.Exception is System.ServiceModel.AddressAlreadyInUseException)
            {
                //MessageBox.Show(e.Exception.Message);
               // Application.ExitThread();
                LogHelper.WriteError("********************管理中心服务Application异常:*******************\r\n", e.Exception);
                return;
            }
            //其他目前不作处理有需要再作处理。
        }
        #endregion

        #region 检查加密狗是否存在  static bool CheckDogExist()  create by:李健华 create by:2009-06-09
        /// <summary>
        /// 检查加密狗是否存在，这里暂时先是检查是否可以打开得了密狗就让运行
        /// </summary>
        /// <returns>返回是否存在加密狗</returns>
        static bool CheckDogExist()
        {
            bool retCode = false;
            OperateDog od = null;
            try
            {
                od = new OperateDog();
                //打开产品名称密狗,此密狗模版已经做好
                //retCode = od.OpenDog("ManageCenter");   
                retCode = od.OpenDog("GrandDog");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请把RC_GrandDog.dll拷贝到程序根目录 !" + ex.Message, "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return retCode = false;
            }
            if (retCode)
            {
                //验证用户输入的用户名
                #region 先不加上用户密码检测
                //if (new FrmValidateUser(od).ShowDialog() != DialogResult.OK)
                //{
                //    retCode = false;
                //}
                #endregion
            }
            else
            {
                MessageBox.Show("对应的硬件狗不存在！", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return retCode;
        }
        #endregion
    }
}