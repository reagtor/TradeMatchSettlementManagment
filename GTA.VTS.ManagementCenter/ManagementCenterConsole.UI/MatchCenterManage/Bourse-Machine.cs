using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 交易所撮合机配置页
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class Bourse_Machine : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Bourse_Machine()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 单例
        /// </summary>
        private static Bourse_Machine instance = null;

        public static Bourse_Machine Instance
        {
            get
            {
                if (instance == null) instance = new Bourse_Machine();
                return instance;
            }
            set { instance = value; }
        }
        /// <summary>
        /// 存放交易所类型信息
        /// </summary>
        public DataSet ds;

        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bourse_Machine_Load(object sender, EventArgs e)
        {
            try
            {
                ManagementCenter.BLL.CM_BourseTypeBLL BourseTypeBLL = new CM_BourseTypeBLL();

                string strWhere = "  DELETESTATE IS NOT NULL AND DELETESTATE<>1 "; 
                ds = BourseTypeBLL.GetList(strWhere);

                ds.Tables[0].Columns.Add("MachineNo");
                this.gridControl1.DataSource = ds.Tables[0];

                this.gridControl1.Focus();
                this.gridView1.FocusedColumn = this.gridCol_MachineNo;
                SendKeys.Send("{ENTER}");
                this.gridControl1.Refresh();
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("页面加载失败!");
                string errCode = "GL-2052";
                string errMsg = "页面加载失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        /// <summary>
        /// 数据检测
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            try
            {
                DataTable editData = ds.Tables[0];
                for (int i = 0; i < editData.Rows.Count; i++)
                {
                    if (!InputTest.intTest(editData.Rows[i]["MachineNo"].ToString()))
                    {
                        ShowMessageBox.ShowInformation("请为每个交易所分配正确的撮合机个数!");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-2053";
                string errMsg = "数据检测失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }
    }
}