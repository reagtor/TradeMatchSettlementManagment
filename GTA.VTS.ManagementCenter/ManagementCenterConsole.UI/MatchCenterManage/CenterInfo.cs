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
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 描述：撮合中心信息配置页
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class CenterInfo :DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CenterInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region 单例
        /// <summary>
        /// 单例
        /// </summary>
        private static CenterInfo instance = null;
        public static CenterInfo Instance
        {
            get
            {
                if (instance == null) instance = new CenterInfo();
                return instance;
            }
            set { instance = value; }
        }
        #endregion

        /// <summary>
        /// 撮合中心表实体
        /// </summary>
        public ManagementCenter.Model.RC_MatchCenter MatchCenter;

        #region  检测输入信息
        /// <summary>
        /// 检测输入信息
        /// </summary>
        /// <returns></returns>
        public bool CheckInPut()
        {
            try
            {
                if (MatchCenter == null) MatchCenter = new RC_MatchCenter();
                if (this.txt_CenterName.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入撮合中心名称!");
                    return false;
                }
                MatchCenter.MatchCenterName = this.txt_CenterName.Text;
                if (this.txt_CenterIP.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入IP地址!");
                    return false;
                }
                if (!InputTest.IPTest(this.txt_CenterIP.Text.Trim()))
                {
                    ShowMessageBox.ShowInformation("IP地址,输入有误，请重新输入!");
                    return false;
                }
                MatchCenter.IP = this.txt_CenterIP.Text;
                if (!InputTest.intTest(this.txt_Port.Text))
                {
                    ShowMessageBox.ShowInformation("端口输入有误，请重新输入!");
                    return false;
                }
                MatchCenter.Port = int.Parse(this.txt_Port.Text);
                if (this.txt_cuoheService.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入撮合服务的名称!");
                    return false;
                }
                MatchCenter.CuoHeService = this.txt_cuoheService.Text;
                if (this.txt_xiadanService.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入下单服务的名称!");
                    return false;
                }
                MatchCenter.XiaDanService = this.txt_xiadanService.Text;
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-2051";
                string errMsg = "检测输入信息失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }

        }
        #endregion

        #region 窗体加载事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterInfo_Load(object sender, EventArgs e)
        {
            this.txt_CenterIP.Text = "127.0.0.1";
            this.txt_Port.Text = "9281";
            txt_cuoheService.Text = "OrderDealRpt";
            this.txt_xiadanService.Text = "DoOrderService";

        }
        #endregion

    }
}
