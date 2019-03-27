using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 撮合中心配置向导 异常编码 2041-2060 
    /// 作者：熊晓凌
    /// 日期：2008-12-12
    /// </summary>
    public partial class DispositionGuide : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DispositionGuide()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// 窗体名称
        /// </summary>
        public static string frmname = "Welcome";

        /// <summary>
        /// 配置向导页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispositionGuide_Load(object sender, EventArgs e)
        {
            frmname = "Welcome";
            this.labDescrip.Text = "欢迎使用撮合中心配置向导";
            this.panelMid.Controls.Clear();
            Welcome.Instance.Dock = DockStyle.Fill;
            this.panelMid.Controls.Add(Welcome.Instance);
            SetButton(frmname);
        }

        /// <summary>
        /// 下一步事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Next_Click(object sender, EventArgs e)
        {
            try
            {
                switch (frmname)
                {
                    case "Welcome":
                        this.labDescrip.Text = "请设置撮合中心参数";
                        this.panelMid.Controls.Clear();
                        CenterInfo.Instance.Dock = DockStyle.Fill;
                        this.panelMid.Controls.Add(CenterInfo.Instance);
                        frmname = "CenterInfo";
                        SetButton(frmname);
                        break;
                    case "CenterInfo":
                        if (CenterInfo.Instance.CheckInPut())
                        {
                            this.labDescrip.Text = "请设置对应交易所的撮合机个数";
                            this.panelMid.Controls.Clear();
                            Bourse_Machine.Instance.Dock = DockStyle.Fill;
                            this.panelMid.Controls.Add(Bourse_Machine.Instance);
                            frmname = "Bourse-Machine";
                            SetButton(frmname);
                        }
                        break;
                    case "Bourse-Machine":
                        if (Bourse_Machine.Instance.CheckData())
                        {
                            this.labDescrip.Text = "完成";
                            this.panelMid.Controls.Clear();
                            Complete.Instance.Dock = DockStyle.Fill;
                            this.panelMid.Controls.Add(Complete.Instance);
                            frmname = "Complete";
                            SetButton(frmname);
                        }
                        break;
                    case "Complete":

                        break;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-2041";
                string errMsg = "点击下一步事件失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        /// <summary>
        /// 上一步事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Back_Click(object sender, EventArgs e)
        {
            try
            {
                switch (frmname)
                {
                    case "Welcome":
                        break;
                    case "CenterInfo":
                        this.labDescrip.Text = "欢迎使用撮合中心配置向导";
                        this.panelMid.Controls.Clear();
                        Welcome.Instance.Dock = DockStyle.Fill;
                        this.panelMid.Controls.Add(Welcome.Instance);
                        frmname = "Welcome";
                        SetButton(frmname);
                        break;
                    case "Bourse-Machine":
                        this.labDescrip.Text = "请设置撮合中心参数";
                        this.panelMid.Controls.Clear();
                        CenterInfo.Instance.Dock = DockStyle.Fill;
                        this.panelMid.Controls.Add(CenterInfo.Instance);
                        frmname = "CenterInfo";
                        SetButton(frmname);
                        break;
                    case "Complete":
                        this.labDescrip.Text = "请设置对应交易所的撮合机个数";
                        this.panelMid.Controls.Clear();
                        Bourse_Machine.Instance.Dock = DockStyle.Fill;
                        this.panelMid.Controls.Add(Bourse_Machine.Instance);
                        frmname = "Bourse-Machine";
                        SetButton(frmname);
                        break;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-2042";
                string errMsg = "点击上一步事件失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        /// <summary>
        /// 设置按扭的值
        /// </summary>
        /// <param name="name"></param>
        private void SetButton(string name)
        {
            switch (name)
            {
                case "Welcome":
                    this.btn_Back.Enabled = false;
                    this.btn_CancelOrOk.Enabled = true;
                    this.btn_Next.Enabled = true;
                    this.btn_Back.Text = "上一步";
                    this.btn_CancelOrOk.Text = "取消";
                    this.btn_Next.Text = "下一步";
                    break;
                case "CenterInfo":
                    this.btn_Back.Enabled = true;
                    this.btn_CancelOrOk.Enabled = true;
                    this.btn_Next.Enabled = true;
                    this.btn_Back.Text = "上一步";
                    this.btn_CancelOrOk.Text = "取消";
                    this.btn_Next.Text = "下一步";
                    break;
                case "Bourse-Machine":
                    this.btn_Back.Enabled = true;
                    this.btn_CancelOrOk.Enabled = true;
                    this.btn_Next.Enabled = true;
                    this.btn_Back.Text = "上一步";
                    this.btn_CancelOrOk.Text = "取消";
                    this.btn_Next.Text = "下一步";
                    break;
                case "Complete":
                    this.btn_Back.Enabled = true;
                    this.btn_CancelOrOk.Enabled = true;
                    this.btn_Next.Enabled = false;
                    this.btn_Back.Text = "上一步";
                    this.btn_CancelOrOk.Text = "确定";
                    this.btn_Next.Text = "下一步";
                    break;
            }
        }

        /// <summary>
        /// 取消按扭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelOrOk_Click(object sender, EventArgs e)
        {
            if (frmname == "Complete")
            {
                if (Complete.Instance.SaveDate())
                {
                    ShowMessageBox.ShowInformation("保存成功!");
                }
                else
                {
                    ShowMessageBox.ShowInformation("保存失败!");
                }
            }
            Welcome.Instance = null;
            CenterInfo.Instance = null;
            Bourse_Machine.Instance = null;
            Complete.Instance = null;
            this.Close();
        }
    }
}