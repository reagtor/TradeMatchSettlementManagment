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
    /// 撮合中心配置 异常编码2000-2015
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class CenterManage :DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CenterManage()
        {
            InitializeComponent();

        }
        #endregion
        #region 变量

        /// <summary>
        /// 表格当前行号
        /// </summary>
        private int m_cutRow = -100;

        ///// <summary>
        ///// 当前页
        ///// </summary>
        //private int m_pageNo = int.MaxValue;

        /// <summary>
        /// 一页展示记录数
        /// </summary>
        private int m_pageSize = ParameterSetting.PageSize;

        ///// <summary>
        ///// 总记录数
        ///// </summary>
        //private int m_rowCount = int.MaxValue;

        /// <summary>
        /// 用户管理
        /// </summary>
        private ManagementCenter.BLL.RC_MatchCenterBLL MatchCenterBLL;

        private ManagementCenter.Model.RC_MatchCenter MatchCenter;

        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CenterManage_Load(object sender, EventArgs e)
        {
            try
            {
                MatchCenterBLL = new RC_MatchCenterBLL();
                LoadCenterList();
                SetTextValue(null);
                SetControlEnAbled();
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("窗体加载失败!");
                string errCode = "GL-2000";
                string errMsg = "撮合中心管理页面加载失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }
        #endregion

        #region 加载撮合中心列表

        /// <summary>
        /// 加载撮合中心列表
        /// </summary>
        private void LoadCenterList()
        {
            try
            {
                DataSet ds = MatchCenterBLL.GetList(string.Empty); 
                DataTable Centerdt;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    Centerdt = new DataTable();
                }
                else
                {
                    Centerdt = ds.Tables[0];
                }
                this.gridCenter.DataSource = Centerdt;
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-2001";
                string errMsg = "加载撮合中心列表失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }
        }

        #endregion

        #region 表格单击事件
        /// <summary>
        /// 表格单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCenter_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
               this.ViewCenter.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

            if (this.ViewCenter != null && this.ViewCenter.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewCenter.FocusedRowHandle;
               
                DataRow dw = ViewCenter.GetDataRow(m_cutRow);
                int MatchCenterID = int.Parse(dw["MatchCenterID"].ToString());
                MatchCenter = MatchCenterBLL.GetModel(MatchCenterID);
                SetTextValue(MatchCenter);
                SetControlEnAbled();
            } 
        }
        #endregion

        #region 设置控件的值
        /// <summary>
        /// 设置控件的值
        /// </summary>
        /// <param name="matchcenter"></param>
        private void SetTextValue(RC_MatchCenter matchcenter)
        {
            if(matchcenter==null)
            {
                m_cutRow = this.ViewCenter.FocusedRowHandle;
                DataRow dw = ViewCenter.GetDataRow(m_cutRow);
                if (dw == null) return;
                int MatchCenterID = int.Parse(dw["MatchCenterID"].ToString());
                matchcenter = MatchCenterBLL.GetModel(MatchCenterID);
                if(matchcenter==null) return;
            }
            this.txt_CenterIP.Text = matchcenter.IP;
            this.txt_CenterName.Text = matchcenter.MatchCenterName;
            this.txt_cuoheService.Text = matchcenter.CuoHeService;
            this.txt_Port.Text = matchcenter.Port.ToString();
            this.txt_xiadanService.Text = matchcenter.XiaDanService;
            this.txt_CenterID.Text = matchcenter.MatchCenterID.ToString();
        }
        #endregion

        #region 设置控件是否可用
        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        private void SetControlEnAbled()
        {
            if(edit==0)
            {
                this.btn_OK.Enabled = false;
                this.btn_Add.Enabled = this.btn_Modify.Enabled = this.btn_Del.Enabled = true;
                this.txt_CenterIP.Enabled =
                    this.txt_CenterName.Enabled =
                    this.txt_cuoheService.Enabled = this.txt_Port.Enabled = this.txt_xiadanService.Enabled = txt_CenterID.Enabled=false;
               
            }
            else if(edit==1)
            {
                this.btn_OK.Enabled =this.btn_Add.Enabled= true;
                this.btn_Modify.Enabled = this.btn_Del.Enabled = false;

                this.txt_CenterIP.Enabled =this.txt_CenterName.Enabled =this.txt_Port.Enabled =  true;
                this.txt_cuoheService.Enabled = this.txt_xiadanService.Enabled = false;
                this.txt_CenterName.Text =this.txt_CenterID.Text = string.Empty;
                //添加时的默认值
                this.txt_CenterIP.Text = "127.0.0.1";
                this.txt_Port.Text = "9281";
                this.txt_cuoheService.Text = "OrderDealRpt";
                this.txt_xiadanService.Text = "DoOrderService";
            }
            else if(edit==2)
            {
                this.btn_OK.Enabled = this.btn_Modify.Enabled = true;
                this.btn_Add.Enabled = this.btn_Del.Enabled = false;

                this.txt_CenterIP.Enabled =this.txt_CenterName.Enabled =this.txt_Port.Enabled =  true;
                this.txt_cuoheService.Enabled = this.txt_xiadanService.Enabled = false;
            }
        }
        #endregion

        /// <summary>
        /// 编辑类型 0 查看 1 添加 2修改
        /// </summary>
        private static int edit=0;

        #region 添加按纽事件
        /// <summary>
        /// 添加按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if(this.btn_Add.Text=="添加")
            {
                this.btn_Add.Text = "取消";
                edit = 1;
                SetControlEnAbled();
            }
            else if (this.btn_Add.Text == "取消")
            {
                this.btn_Add.Text = "添加";
                edit = 0;
                SetTextValue(null);
                SetControlEnAbled();
            }  
        }
        #endregion

        #region 修改按纽事件
        /// <summary>
        /// 修改按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, EventArgs e)
        {
            if (this.btn_Modify.Text == "修改")
            {
                this.btn_Modify.Text = "取消";
                edit = 2;
                SetControlEnAbled();
            }
            else if (this.btn_Modify.Text == "取消")
            {
                this.btn_Modify.Text = "修改";
                edit = 0;
                SetTextValue(null);
                SetControlEnAbled();
            }
        }
        #endregion

        #region 删除按纽事件
        /// <summary>
        /// 删除按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
              this.ViewCenter.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));
                if (this.ViewCenter != null && this.ViewCenter.FocusedRowHandle >= 0)
                {
                    m_cutRow = this.ViewCenter.FocusedRowHandle;

                    DataRow dw = ViewCenter.GetDataRow(m_cutRow);
                    int MatchCenterID = int.Parse(dw["MatchCenterID"].ToString());
                    ManagementCenter.BLL.RC_MatchMachineBLL  _MatchMachineBLL=new RC_MatchMachineBLL();
                    DataSet ds= _MatchMachineBLL.GetList(string.Format("MatchCenterID={0}", MatchCenterID));
                    if(ds!=null && ds.Tables[0].Rows.Count>0)
                    {
                        ShowMessageBox.ShowInformation("该中心下存在撮合机，不允许删除!");
                        return;
                    }
                    MatchCenterBLL.Delete(MatchCenterID);
                    ShowMessageBox.ShowInformation("删除成功!");
                    SetTextValue(null);
                    SetControlEnAbled();
                    LoadCenterList();
                } 
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("删除失败!");
                string errCode = "GL-2002";
                string errMsg = "删除失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
            SetTextValue(null);
            SetControlEnAbled();
        }
        #endregion

        #region 确定按纽事件
        /// <summary>
        /// 确定按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckInPut())
                {
                    if(edit==1)
                    {
                        int i = MatchCenterBLL.Add(MatchCenter);
                        if(i>0)
                        {
                            ShowMessageBox.ShowInformation("添加成功!");
                            this.btn_Add.Text = "添加";
                            this.btn_Modify.Text = "修改";
                            edit = 0;
                            SetTextValue(null);
                            SetControlEnAbled();
                            LoadCenterList();
                            return;
                        }
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                    else if(edit==2)
                    {
                        MatchCenter.MatchCenterID =Convert.ToInt32(this.txt_CenterID.Text);
                        MatchCenterBLL.Update(MatchCenter);
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.btn_Add.Text = "添加";
                        this.btn_Modify.Text = "修改";
                        edit = 0;
                        SetTextValue(null);
                        SetControlEnAbled();
                        LoadCenterList();
                        return;
                    }
                } 
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("保存失败!");
                string errCode = "GL-2003";
                string errMsg = "保存失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }
        #endregion

        #region 检测输入
        /// <summary>
        /// 检测输入
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
                string errCode = "GL-2004";
                string errMsg = "检测输入失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }
        #endregion

        #region 重画事件
        private void ViewCenter_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == this.gridCol_State)
            {
                if ((object)e.CellValue == (object)System.DBNull.Value)
                {
                    e.DisplayText =string.Empty;
                }
                else
                {
                    if ((int)e.CellValue == (int)ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess)
                    {
                        e.DisplayText = "连接正常";
                    }
                    else if ((int)e.CellValue == (int)ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat)
                    {
                        e.DisplayText = "连接断开";
                    }
                }
                
            }
        }
        #endregion

        #region 刷新状态
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                MatchCenterBLL.CenterTestConnection();
                LoadCenterList();
            }
            catch (Exception ex)
            {
                string errCode = "GL-2005";
                string errMsg = "刷新状态失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }
        #endregion


    }

}
