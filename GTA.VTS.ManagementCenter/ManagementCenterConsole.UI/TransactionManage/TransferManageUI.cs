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
using ManagementCenter.Model.CommonClass;
using ManagementCenter.Model.UserManage;
using ManagementCenterConsole.UI.CommonClass;
using Types = ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 描述：转账管理窗体  错误编码范围:0371-0385
    /// 作者：刘书伟
    /// 日期：2009-10-23
    /// 描述：添加转账金额判断
    /// 修改作者：刘书伟
    /// 修改日期：2010-05-12
    /// </summary>
    public partial class TransferManageUI : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        public TransferManageUI()
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

        /// <summary>
        /// 交易员信息管理类
        /// </summary>
        private ManagementCenter.BLL.UserManage.TransactionManage TransactionManageBLL;


        #endregion

        //================================  私有  方法 ================================

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

            QueryUserInfo.LoginName = string.Empty;
            QueryUserInfo.RoleID = (int)Types.RoleTypeEnum.Transaction;
            return true;
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
                string errCode = "GL-0372";
                string errMsg = "初始化交易员表格失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                throw exception;
            }
        }

        #endregion

        #region 根据查询条件，获取用户列表

        /// <summary>
        /// 根据查询条件，获取用户列表
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
                    string userid = ViewUserInfo.GetDataRow(0)["UserID"].ToString();
                    int counterID = int.Parse(ViewUserInfo.GetDataRow(0)["CouterID"].ToString());//柜台ID
                    InitAccountListByUserID(userid, counterID, 0);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0377";
                string errMsg = "根据查询条件，获取用户列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
            isFirstInit = false;
        }

        #endregion

        #region 根据查询条件，获取港股交易商品
        /// <summary>
        /// 根据查询条件，获取港股交易商品
        /// </summary>
        /// <returns></returns>
        private DataTable QueryHKCommodity(string adminId, string adminPassword, string traderId, int counterID)
        {
            try
            {
                string strMess = string.Empty;
                TransactionManageBLL = new ManagementCenter.BLL.UserManage.TransactionManage();
                DataTable _dtTransferResult = TransactionManageBLL.AdminFindTraderCapitalAccountInfoByID(
                                              adminId, adminPassword, traderId, counterID, out strMess);
                if (_dtTransferResult == null || _dtTransferResult.Rows.Count == 0)
                {
                    _dtTransferResult = new DataTable();
                }
                //账号类型
                ddlAccountType.DataSource = ComboBoxDataSource.GetAccountTypeList();
                ddlAccountType.ValueMember = "ValueIndex";
                ddlAccountType.DisplayMember = "TextTitleValue";

                //币种类型
                ddlCurrencyType.DataSource = ComboBoxDataSource.GetCurrencyTypeList();
                ddlCurrencyType.ValueMember = "ValueIndex";
                ddlCurrencyType.DisplayMember = "TextTitleValue";
                return _dtTransferResult;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0376";
                string errMsg = "根据查询条件，获取港股交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 根据用户ID显示此用户的资金账户信息
        /// <summary>
        /// 加载帐户列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="counterID">柜台ID</param>
        /// <param name="viewRow">当前行</param>
        private void InitAccountListByUserID(string userID, int counterID, int viewRow)
        {
            try
            {
                this.gdcTransferResult.DataSource = QueryHKCommodity(CommonClass.ParameterSetting.Mananger.UserID.ToString(), CommonClass.ParameterSetting.Mananger.Password, userID.ToString(), counterID);
            }
            catch (Exception ex)
            {
                //写日志
                string errCode = "GL-0375";
                string errMsg = "加载用户帐号列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
        }
        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            this.txtTransferMoney.Text = string.Empty;
        }

        #endregion

        //================================  事件 ================================

        #region  转账管理窗体 TransferManageUI_Load
        /// <summary>
        /// 转账管理窗体 TransferManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                AccountTypeBLL = new UM_AccountTypeBLL();
                //L_AccountType = AccountTypeBLL.GetListArray(string.Empty);
                DealerAccountBLL = new UM_DealerAccountBLL();
                SetQueryUserInfo();
                InitUserList();

                //转出账户类型
                cmbTransOut.Properties.Items.Clear();
                cmbTransOut.Properties.Items.AddRange(ComboBoxDataSource.GetAccountTypeList());
                cmbTransOut.SelectedIndex = 0;

                //转入账户类型(默认显示不包含银行资金账户)
                cmbTransIn.Properties.Items.Clear();
                cmbTransIn.Properties.Items.AddRange(ComboBoxDataSource.GetAccountTypeListByAccountTypeID((int)GTA.VTS.Common.CommonObject.Types.AccountType.BankAccount));
                cmbTransIn.SelectedIndex = 0;

                //币种类型
                cmbCurrencyType.Properties.Items.Clear();
                cmbCurrencyType.Properties.Items.AddRange(ComboBoxDataSource.GetCurrencyTypeList());
                cmbCurrencyType.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("窗体加载失败!");
                string errCode = "GL-0371";
                string errMsg = "转账管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
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
            LoadUser();
        }
        #endregion

        #region 查询按钮事件
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (SetQueryUserInfo())
                {
                    InitUserList();
                }

            }
            catch
            {
                return;
            }
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
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.ViewUserInfo.CalcHitInfo(((Control)sender).PointToClient(Control.MousePosition));

                if (this.ViewUserInfo != null && this.ViewUserInfo.FocusedRowHandle >= 0 && hi.RowHandle >= 0)
                {
                    m_cutRow = this.ViewUserInfo.FocusedRowHandle;

                    if (m_cutRow < 0)
                    {
                        return;
                    }

                    DataRow dw = ViewUserInfo.GetDataRow(m_cutRow);
                    string userID = dw["UserID"].ToString();//用户ID
                    int counterID = int.Parse(dw["CouterID"].ToString());//柜台ID
                    InitAccountListByUserID(userID, counterID, m_cutRow);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }
        #endregion

        #region 转出账户组合框改变事件
        /// <summary>
        /// 转出账户组合框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTransOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int _AccountTypeID = AppGlobalVariable.INIT_INT;//账号类型ID
                _AccountTypeID = ((UComboItem)cmbTransOut.SelectedItem).ValueIndex;
                if (_AccountTypeID != AppGlobalVariable.INIT_INT)
                {
                    //转入账户类型
                    cmbTransIn.Properties.Items.Clear();
                    cmbTransIn.Properties.Items.AddRange(ComboBoxDataSource.GetAccountTypeListByAccountTypeID(_AccountTypeID));
                    cmbTransIn.SelectedIndex = 0;

                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-0373";
                string errMsg = " 转出账户组合框改变事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                return;
            }
        }
        #endregion

        #region 自由转帐（同币种）
        /// <summary>
        /// 自由转帐（同币种）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                CT_CounterBLL CounterBLL = new CT_CounterBLL();

                if (!CounterBLL.TestCenterConnection())
                {
                    MessageBox.Show("柜台服务连接失败,请检查柜台服务是否开启!", "系统提示");
                    return;
                }
                if (this.ViewUserInfo != null && this.ViewUserInfo.FocusedRowHandle >= 0 && ViewUserInfo.RowCount > 0)
                {
                    m_cutRow = this.ViewUserInfo.FocusedRowHandle;

                    if (m_cutRow < 0)
                    {
                        return;
                    }

                    DataRow dw = ViewUserInfo.GetDataRow(m_cutRow);
                    int userID = Convert.ToInt32(dw["UserID"].ToString());//用户ID
                    int counterID = int.Parse(dw["CouterID"].ToString());//柜台ID
                    int FromCapitalAccountType = ((UComboItem)cmbTransOut.SelectedItem).ValueIndex;
                    int ToCapitalAccountType = ((UComboItem)cmbTransIn.SelectedItem).ValueIndex;
                    decimal TransferAmount = AppGlobalVariable.INIT_DECIMAL;
                    string _Money = string.Empty;//输入的金额
                    if (!string.IsNullOrEmpty(txtTransferMoney.Text))
                    {
                        if (InputTest.DecimalTest(this.txtTransferMoney.Text))
                        {
                            if (Convert.ToDecimal(txtTransferMoney.Text)<=0)
                            {
                                ShowMessageBox.ShowInformation("转出金额需大于0!");
                                return;
                            }
                            _Money = this.txtTransferMoney.Text;
                            string[] _lengthRMB = _Money.Split('.');
                            if (_lengthRMB[0].Length > 12)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(整数部分不能大于12位)!");
                                return;
                            }
                            if (_lengthRMB.Length > 1)
                            {
                                if (_lengthRMB[1].Length > 3)
                                {
                                    ShowMessageBox.ShowInformation("小数部分不能大于3位!");
                                    return;
                                }
                            }
                            if (this.txtTransferMoney.Text.Length > 16)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                                return;
                            }
                            TransferAmount = Convert.ToDecimal(txtTransferMoney.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("转出资金不能为空!");
                        return;
                    }
                    TransactionManageBLL = new ManagementCenter.BLL.UserManage.TransactionManage();
                    int currencyType = ((UComboItem)cmbCurrencyType.SelectedItem).ValueIndex;
                    string outMessage = string.Empty;
                    bool result = TransactionManageBLL.ConvertFreeTransferEntity(userID, FromCapitalAccountType, ToCapitalAccountType,
                                                                     TransferAmount, currencyType, counterID, out outMessage);
                    if (result)
                    {
                        ShowMessageBox.ShowInformation("成功转出资金:" + TransferAmount + "元");
                        InitAccountListByUserID(userID.ToString(), counterID, m_cutRow);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(outMessage))
                        {
                            ShowMessageBox.ShowInformation(outMessage + "!");//在柜台返回的异常信息后加!

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0374";
                string errMsg = " 自由转帐（同币种）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                return;
            }

        }

        #endregion

        #region 取消按钮事件 btnCancel_Click
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }
        #endregion

    }
}
