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
using Types=ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 描述：冻结解冻UI 错误编码范围0351-0370
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public partial class FreezeManage : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FreezeManage()
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
        private UM_UserInfo QueryUserInfo;

        /// <summary>
        /// 帐户管理类
        /// </summary>
        private ManagementCenter.BLL.UM_DealerAccountBLL DealerAccountBLL;

        private UM_AccountTypeBLL AccountTypeBLL;

        private bool isFirstInit = true;

        private List<UM_AccountType> L_AccountType;

        #endregion

        #region 加载页面
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreezeManage_Load(object sender, EventArgs e)
        {
            try
            {
                AccountTypeBLL=new UM_AccountTypeBLL();
                L_AccountType = AccountTypeBLL.GetListArray(string.Empty);
                DealerAccountBLL = new UM_DealerAccountBLL();
                SetQueryUserInfo();
                InitUserList();
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("窗体加载失败!");
                string errCode = "GL-0351";
                string errMsg = "帐号冻结解冻管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
        }
        #endregion

        #region 初始化

        /// <summary>
        /// 初始化页面信息
        /// </summary>
        private void InitUserList()
        {
            try
            {
                this.m_pageNo = 1;
                //加载第一页数据
                LoadUser();
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
                    if (m_rowCount%this.m_pageSize == 0)
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount/this.m_pageSize);
                    }
                    else
                    {
                        this.ucPageNavigation1.PageCount = Convert.ToInt32(m_rowCount/this.m_pageSize) + 1;
                    }
                }
                this.ucPageNavigation1.CurrentPage = this.m_pageNo;
                this.ucPageNavigation1.PageIndexChanged +=
                    new ManagementCenterConsole.UI.CommonControl.PageIndexChangedCallBack(
                        ucPageNavigation1_PageIndexChanged);
            }
            catch (Exception ex)
            {
                string errCode = "GL-0352";
                string errMsg = "初始化交易员表格失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                throw exception;
            }
        }

        #endregion

        #region 加载用户列表

        /// <summary>
        /// 加载用户列表
        /// </summary>
        private void LoadUser()
        {
            try
            {
                ManagementCenter.BLL.UM_UserInfoBLL UserInfoBLL = new UM_UserInfoBLL();
                DataSet ds = UserInfoBLL.GetPagingUser(QueryUserInfo, this.m_pageNo, this.m_pageSize,
                                                       out this.m_rowCount);
                DataTable allUser;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    allUser = new DataTable();
                    if (!isFirstInit) ShowMessageBox.ShowInformation("不存在记录!");
                }
                else
                {
                    allUser = ds.Tables[0];
                }
                this.gridUserInfo.DataSource = allUser;

                if (ViewUserInfo.RowCount > 0) 
                {
                    int userid=int.Parse(ViewUserInfo.GetDataRow(0)["UserID"].ToString());
                    InitAccountListByUserID(userid,0);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0353";
                string errMsg = "读取交易员列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
            isFirstInit = false;
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
            LoadUser();
        }

        #endregion

        #region 设置查询实体对象

        /// <summary>
        /// 设置查询实体对象
        /// </summary>
        private bool SetQueryUserInfo()
        {
            if (QueryUserInfo == null)
            {
                QueryUserInfo = new UM_UserInfo();
            }
            if (this.txt_UserID.Text.Trim() == string.Empty)
            {
                QueryUserInfo.UserID = int.MaxValue;
            }
            else
            {
                if (InputTest.intTest(this.txt_UserID.Text.Trim()))
                {
                    QueryUserInfo.UserID = int.Parse(this.txt_UserID.Text.Trim());
                }
                else
                {
                    ShowMessageBox.ShowInformation("请输入正确的交易员编号!");
                    return false;
                }
            }

            QueryUserInfo.UserName = this.txt_Name.Text.Trim() != string.Empty
                                         ? this.txt_Name.Text.Trim()
                                         : string.Empty;

            //if (this.txt_CounterID.Text.Trim() == string.Empty)
            //{
            //    QueryUserInfo.CouterID = int.MaxValue;
            //}
            //else
            //{
            //    if (InputTest.intTest(this.txt_CounterID.Text.Trim()))
            //    {
            //        QueryUserInfo.CouterID = int.Parse(this.txt_CounterID.Text.Trim());
            //    }
            //    else
            //    {
            //        ShowMessageBox.ShowInformation("请输入正确的柜台编号!");
            //        return false;
            //    }
            //}
            //柜台名称
            QueryUserInfo.Name = this.txt_CounterName.Text.Trim() != string.Empty
                                         ? this.txt_CounterName.Text.Trim()
                                         : string.Empty;
            QueryUserInfo.LoginName = string.Empty;
            QueryUserInfo.RoleID = (int) Types.RoleTypeEnum.Transaction;
            return true;
        }

        #endregion

        #region 用户点击gridUserInfo行事件
        /// <summary>
        /// 用户点击gridUserInfo行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridUserInfo_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.ViewUserInfo.CalcHitInfo(((Control) sender).PointToClient(Control.MousePosition));

            if (this.ViewUserInfo != null && this.ViewUserInfo.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewUserInfo.FocusedRowHandle;
                
                if (m_cutRow < 0) return;

                DataRow dw = ViewUserInfo.GetDataRow(m_cutRow);
                 int userID = int.Parse(dw["UserID"].ToString());
                InitAccountListByUserID(userID,0); 
            }
        }
        #endregion

        #region 加载帐户列表
        /// <summary>
        /// 加载帐户列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="viewRow"></param>
        private void InitAccountListByUserID(int userID,int viewRow)
        {
            try
            {
                DataSet ds = DealerAccountBLL.GetList(string.Format(" UserID={0}", userID));
                DataTable allAccount;
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    allAccount = new DataTable();
                }
                else
                {
                   allAccount = ds.Tables[0];
                }
                this.gridAccount.DataSource = allAccount;
                this.ViewAccount.FocusedRowHandle = viewRow;
                SetControl(viewRow);
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-0354";
                string errMsg = "加载用户帐号列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
        }
        #endregion

        #region gridAccount的单击事件gridAccount_Click
        /// <summary>
        /// gridAccount的单击事件gridAccount_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridAccount_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.ViewAccount.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

            if (this.ViewAccount != null && this.ViewAccount.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
            {
                m_cutRow = this.ViewAccount.FocusedRowHandle;
                
                if (m_cutRow < 0) return;
                SetControl(m_cutRow);
            }

        }
        #endregion

        #region 根据当前行设置按钮显示方式
        /// <summary>
        /// 根据当前行设置按钮显示方式
        /// </summary>
        /// <param name="m_Row"></param>
        private void SetControl(int m_Row)
        {
            if(ViewAccount.RowCount<1) 
            {
                this.txt_TranscationID.Text = this.txt_Account.Text = string.Empty;
                this.btn_UnFreeze.Enabled = this.btn_Freeze.Enabled = this.chk_IsAutoRelieve.Enabled = false;
                return;
            }
            DataRow dw = ViewAccount.GetDataRow(m_Row);

            int userID = int.Parse(dw["UserID"].ToString());
            string AccountID = dw["DealerAccoutID"].ToString();
            this.txt_TranscationID.Text = userID.ToString();
            this.txt_Account.Text = AccountID;
            object obj = dw["IsDo"];
            bool IsDo = (bool)obj;

            //没有冻结的帐号
            if (IsDo)
            {
                this.btn_UnFreeze.Enabled = false;
                this.btn_Freeze.Enabled = true;
                this.chk_IsAutoRelieve.Enabled = true;
            }
            //冻结冻结的帐号
            else
            {
                this.btn_UnFreeze.Enabled = true;
                this.btn_Freeze.Enabled = false;
                this.chk_IsAutoRelieve.Enabled = false;
            }
        }
        #endregion

        #region 冻结按纽事件
        /// <summary>
        /// 冻结按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Freeze_Click(object sender, EventArgs e)
        {
            string mess;
            try
            {
                string AccountID = this.txt_Account.Text.Trim();
                UM_DealerAccount DealerAccount = DealerAccountBLL.GetModel(AccountID);
                if (DealerAccount==null) return;
                UM_DealerAccount Account =
                    DealerAccountBLL.GetModelByUserIDAndType(int.Parse(this.txt_TranscationID.Text.Trim()),
                                                             (int) GTA.VTS.Common.CommonObject.Types.AccountAttributionType.BankAccount);
                if (DealerAccount.DealerAccoutID == Account.DealerAccoutID)
                {
                    ShowMessageBox.ShowInformation("银行帐号不允许冻结!");
                    return;
                }
                if (DealerAccount.UserID == int.Parse(this.txt_TranscationID.Text.Trim()) && DealerAccount.IsDo == true)
                {
                    DealerAccount.IsDo = false;
                    
                    UM_FreezeReason FreezeReason = new UM_FreezeReason();
                    FreezeReason.DealerAccoutID = DealerAccount.DealerAccoutID;
                    FreezeReason.FreezeReason = this.txt_Reason.Text.Trim();
                    FreezeReason.FreezeReasonTime = System.DateTime.Now;
                    if (chk_IsAutoRelieve.Enabled == true && this.chk_IsAutoRelieve.Checked == true )
                    {
                        if (this.dateEdit1.DateTime > System.DateTime.Now)
                        {
                            FreezeReason.IsAuto = (int)Types.IsAutoUnFreezeEnum.Auto;
                            FreezeReason.ThawReasonTime = this.dateEdit1.DateTime;
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("时间设置有误，必须大于当前时间!");
                            return;
                        }
                        
                    } 

                    ManagementCenter.BLL.UserManage.TransactionManage tm=new ManagementCenter.BLL.UserManage.TransactionManage();
                    if(!tm.FreezeAccount(DealerAccount,FreezeReason,out mess))
                    {
                        ShowMessageBox.ShowInformation(mess);
                        return;
                    }
                   
                    InitAccountListByUserID(DealerAccount.UserID,ViewAccount.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0355";
                string errMsg = "冻结失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                ShowMessageBox.ShowInformation(exception.ToString());
                return;
            }
        }
        #endregion

        #region 解冻按纽事件
        /// <summary>
        /// 解冻按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_UnFreeze_Click(object sender, EventArgs e)
        {
            string mess;
            try
            {
                string AccountID = this.txt_Account.Text.Trim();
                if(!string.IsNullOrEmpty(AccountID))
                {
                   UM_DealerAccount DealerAccount = DealerAccountBLL.GetModel(string.Format(AccountID));

                  if (DealerAccount.UserID == int.Parse(this.txt_TranscationID.Text.Trim()) && DealerAccount.IsDo == false)
                  {
                    DealerAccount.IsDo = true;

                    UM_ThawReason ThawReason = new UM_ThawReason();
                    ThawReason.DealerAccoutID = DealerAccount.DealerAccoutID;
                    ThawReason.ThawReason = this.txt_Reason.Text.Trim();
                    ThawReason.ThawReasonTime = System.DateTime.Now;

                    ManagementCenter.BLL.UserManage.TransactionManage tm = new ManagementCenter.BLL.UserManage.TransactionManage();
                    if (!tm.ThawAccount(DealerAccount, ThawReason, out mess))
                    {
                        ShowMessageBox.ShowInformation(mess);
                        return;
                    } 

                    InitAccountListByUserID(DealerAccount.UserID, ViewAccount.FocusedRowHandle);
                  }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0356";
                string errMsg = "解冻失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                ShowMessageBox.ShowInformation(exception.ToString());
                return;
            }
        }
        #endregion

        #region 查询按纽事件
        /// <summary>
        /// 查询按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Query_Click(object sender, EventArgs e)
        {
            if(SetQueryUserInfo())
            {InitUserList();}
        }
        #endregion

        
        #region 用户重画单元格
        private void ViewAccount_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

            if (e.Column == this.gridCol_AccountTypeID)
            {
                if (L_AccountType==null) return;
                foreach (UM_AccountType AccountType  in L_AccountType)
                {
                    if(AccountType.AccountTypeID==(int)e.CellValue)
                    {
                        e.DisplayText = AccountType.AccountName;
                        break;
                    }
                }
            }
            if(e.Column==this.gridCol_IsDo)
            {
                if((bool)e.CellValue==true)
                {
                    e.DisplayText = "正常";
                }
                else
                {
                    e.DisplayText = "冻结";
                }
            }
        }
        #endregion

      
    }
}