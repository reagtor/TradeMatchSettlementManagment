using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.Common.CommonUtility;
using System.Diagnostics;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 作者：叶振东
    /// 时间：2010-03-02
    /// 描述：柜台的连接与断开
    /// </summary>
    public partial class frmConnection : MdiFormBase, IMessageView
    {
        #region 变量
        WCFServices wcfLogic;
        #endregion 变量

        #region 构造函数
        public frmConnection()
        {
            wcfLogic = WCFServices.Instance;
            wcfLogic.wcfMsgView = this;
            InitializeComponent();
            LocalhostResourcesFormText();
        }
        #endregion  构造函数

        #region 窗体事件
        /// <summary>
        /// 连接按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (wcfLogic.IsServiceOk)
                return;
            string errorMsg = "";
            bool isSuccess = wcfLogic.Initialize(ServerConfig.TraderID, "", out errorMsg);
            if (isSuccess)
            {
                //StartTimer();
                wcfLogic.ServiceOk = true;
            }
            else
            {
                string errMsg = ResourceOperate.Instanse.GetResourceByKey("StartWCF");
                MessageBox.Show(errMsg);
            }
        }

        /// <summary>
        /// 关闭连接按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (wcfLogic.IsServiceOk)
            {
                wcfLogic.ShutDown();
                wcfLogic.IsServiceOk = false;
                wcfLogic.ServiceOk = false;
                //StopTimer();
            }
        }

        /// <summary>
        /// 注册通道按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegisterChannle_Click(object sender, EventArgs e)
        {
            bool ok = false;
            string Success = ResourceOperate.Instanse.GetResourceByKey("Success");
            try
            {
                List<string> list = new List<string>();
                foreach (var item in txtEnturstNo.Text.Split(','))
                {
                    list.Add(item);
                }
                ok = wcfLogic.ChangeEntrustChannel(list, txtChannelID.Text.Trim(), cbXH.Checked ? 1 : 2);
            }
            catch
            {
            }
            if (ok)
            {
                MessageBox.Show(Success);
            }
        }

        /// <summary>
        /// 清空连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            //this.btnUpdateApp.Text = ResourceOperate.Instanse.GetResourceByKey("Update");
            this.Text = ResourceOperate.Instanse.GetResourceByKey("menuConnection");
            #region 柜台连接
            this.btnConnect.Text = ResourceOperate.Instanse.GetResourceByKey("btnConnect");
            this.btnRegisterChannle.Text = ResourceOperate.Instanse.GetResourceByKey("btnRegisterChannle");
            this.btnDisConnect.Text = ResourceOperate.Instanse.GetResourceByKey("btnDisConnect");
            this.lblChannel.Text = ResourceOperate.Instanse.GetResourceByKey("lblChannel");
            this.lblEntrust.Text = ResourceOperate.Instanse.GetResourceByKey("lblEntrust");
            this.cbXH.Text = ResourceOperate.Instanse.GetResourceByKey("cbXH");
            #endregion 柜台连接
        }
        #endregion 窗体事件

        #region IMessageView 成员

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteMessage(string msg)
        {
            MessageDisplayHelper.Event(msg, listBox3);
        }

        #endregion
  

     
    }
}