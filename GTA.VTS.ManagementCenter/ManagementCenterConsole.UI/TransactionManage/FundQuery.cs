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
using ManagementCenter.BLL;
using ManagementCenter.Model.UserManage;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 追加资金的历史查询 错误编码范围0341-0350
    /// 作者：程序员：熊晓凌
    /// 日期：2008-12-11
    /// 描述:调整界面资金相关数据显示格式为字符串格式
    /// 修改作者：刘书伟
    /// 日期：2010-05-06
    /// </summary>
    public partial class FundQuery :DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FundQuery()
        {
            InitializeComponent();
        }
        #endregion

        #region 变量

        /// <summary>
        /// 表格当前行号
        /// </summary>
        private int m_cutRow = -100;

        /// <summary>
        /// 当前页
        /// </summary>
        private int m_pageNo = int.MaxValue;

        /// <summary>
        /// 一页展示记录数
        /// </summary>
        private int m_pageSize = ParameterSetting.PageSize;

        /// <summary>
        /// 总记录数
        /// </summary>
        private int m_rowCount = int.MaxValue;

        /// <summary>
        /// 查询实体
        /// </summary>
        private ManagementCenter.Model.UserManage.FundAddQueryEntity fundAddQueryEntity;

        /// <summary>
        /// 用户管理
        /// </summary>
        private ManagementCenter.BLL.UM_FundAddInfoBLL FundAddInfoBLL;

        private bool isFirstInit = true;

        /// <summary>
        /// 提示信息变量
        /// </summary>
        private ToolTip m_tip=new ToolTip();

        #endregion

        #region 查询事件
        private void btn_Query_Click(object sender, EventArgs e)
        {
            if(SetFundQueryEntity())
            Init();
        }
        #endregion

        #region 追加资金按纽事件
        private void btn_AddFund_Click(object sender, EventArgs e)
        {
            if(new AddFundForm().ShowDialog()==DialogResult.OK)
            {
                Init();
            }
        }
        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FundQuery_Load(object sender, EventArgs e)
        {
            try
            {
                FundAddInfoBLL = new UM_FundAddInfoBLL();
                SetFundQueryEntity();
                Init();
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("窗体加载失败!");
                string errCode = "GL-0341";
                string errMsg = "追加资金窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
        }
        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadFundList();
                //根据获取的页数初始化分页控件
                this.ucPageNavigation1.PageIndexChanged -=
                    new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(
                        ucPageNavigation1_PageIndexChanged);
                if (m_rowCount == 0)
                {
                    this.ucPageNavigation1.PageCount = 0;
                }
                else
                {
                    if (m_rowCount % this.m_pageSize == 0)
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize);
                    }
                    else
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize) + 1;
                    }
                }
                this.ucPageNavigation1.CurrentPage = this.m_pageNo;
                this.ucPageNavigation1.PageIndexChanged +=
                    new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(
                        ucPageNavigation1_PageIndexChanged);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0342";
                string errMsg = "初始化表格失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                throw exception;
            }
        }

        #endregion

        #region 页码改变触发事件

        /// <summary>
        /// 页码改变触发事件
        /// </summary>
        /// <param name="page">查询的页数</param>
        private void ucPageNavigation1_PageIndexChanged(int page)
        {
            this.m_pageNo = page;
            LoadFundList();
        }

        #endregion

        #region 加载追加资金历史列表

        /// <summary>
        /// 加载追加资金历史列表
        /// </summary>
        private void LoadFundList()
        {
            try
            {
                DataSet ds = FundAddInfoBLL.GetPagingFund(fundAddQueryEntity, this.m_pageNo, this.m_pageSize,
                                                          out this.m_rowCount);
                DataTable Funddt;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    Funddt = new DataTable();
                    if (!isFirstInit) ShowMessageBox.ShowInformation("不存在记录！");
                }
                else
                {
                    Funddt = ds.Tables[0];
                }
                this.gridFund.DataSource = Funddt;
            }
            catch (Exception ex)
            {
                //写日志
                ShowMessageBox.ShowInformation("加载历史列表失败");
                string errCode = "GL-0343";
                string errMsg = "加载历史列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
            isFirstInit = false;
        }

        #endregion

        #region 设置查询实体对象

        /// <summary>
        /// 设置查询实体对象
        /// </summary>
        private bool SetFundQueryEntity()
        {
            if (fundAddQueryEntity == null)
            {
                fundAddQueryEntity = new FundAddQueryEntity();
            }
            
            if (this.txt_TransID.Text.Trim() == string.Empty)
            {
                fundAddQueryEntity.UserID = int.MaxValue;
            }
            else
            {
                try
                {
                    fundAddQueryEntity.UserID = int.Parse(this.txt_TransID.Text.Trim());
                }
                catch 
                {
                    ShowMessageBox.ShowInformation("请输入正确的交易员编号！");
                    return false;
                }
            }
            //if (this.txt_ManangerID.Text.Trim() == string.Empty)
            //{           
            //    fundAddQueryEntity.ManagerID = int.MaxValue;
            //}
            //else
            //{
            //    try
            //    {
            //        fundAddQueryEntity.ManagerID = int.Parse(this.txt_ManangerID.Text.Trim());
            //    }
            //    catch (Exception ex)
            //    {
            //        ShowMessageBox.ShowInformation("请输入正确的管理员编号！");
            //        return false;
            //    }
            //}
            //登陆名称(根据要求把按管理员编号查询改成按管理员登陆名称查询)
            fundAddQueryEntity.loginName = this.txt_LoginName.Text.Trim() != string.Empty
                                         ? this.txt_LoginName.Text.Trim()
                                         : string.Empty;
            return true;  
        }

        #endregion

        #region 表格双击事件
        /// <summary>
        /// 表格双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridFund_DoubleClick(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi =
                this.ViewFund.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

            if (this.ViewFund != null && this.ViewFund.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewFund.FocusedRowHandle;

                AddFundForm AddFundForm = new AddFundForm();
                DataRow dw = ViewFund.GetDataRow(m_cutRow);
                int UserID = int.Parse(dw["UserID"].ToString());
                AddFundForm.UserID = UserID;
                if (AddFundForm.ShowDialog() == DialogResult.OK)
                {
                    Init();
                }
            }
        }
        #endregion

        #region 鼠标移到gridFund_MouseMove控件上的任一地方时，显示提示 gridFund_MouseMove事件
        /// <summary>
        ///鼠标移到gridFund_MouseMove控件上的任一地方时，显示提示 gridFund_MouseMove事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridFund_MouseMove(object sender, MouseEventArgs e)
        {
            m_tip.SetToolTip(this.gridFund,"双击查看详细信息");
            m_tip.Active = true;
        }
        #endregion

    }
}
