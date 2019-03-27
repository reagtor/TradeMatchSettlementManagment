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
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using ManagementCenterConsole.UI.CommonClass;
using ManagementCenter.BLL;
using Types = ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 描述:交易员信息管理页面 错误编码范围0300-0320
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// 描述:添加相关输入检验
    /// 修改作者：刘书伟
    /// 日期：2010-04-26
    /// </summary>
    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserInfoForm()
        {
            InitializeComponent();
        }
        #endregion

        #region 用户信息模型

        private UM_UserInfo m_currentUser = null;

        /// <summary>
        /// 用户信息模型
        /// </summary>
        public UM_UserInfo CurrentUser
        {
            get { return this.m_currentUser; }
            set
            {
                this.m_currentUser = new UM_UserInfo();
                this.m_currentUser = value;
            }
        }

        #endregion

        #region 初始资金

        /// <summary>
        /// 初始资金
        /// </summary>
        private InitFund m_InitFund = null;

        /// <summary>
        /// 初始资金属性
        /// </summary>
        public InitFund InitFund
        {
            get { return this.m_InitFund; }
            set
            {
                this.m_InitFund = new InitFund();
                this.m_InitFund = value;
            }
        }

        #endregion

        #region 操作类型　 1:添加,2:修改

        private int m_EditType = 1;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return this.m_EditType; }
            set { this.m_EditType = value; }
        }

        #endregion

        /// <summary>
        /// 提示判断的错误信息
        /// </summary>
        private string JudgmentMessage = string.Empty;

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_EditType == 1)
                {
                    m_InitFund = new InitFund();
                    m_currentUser = new UM_UserInfo();
                }
                SetControlsIsEnabled();
                BinData();
                InitTreeView();
                if (m_EditType == 2)
                {
                    InitUserInfo();
                    InitOriginationFund();
                }
                if (this.dll_CouterID.Properties.Items.Count < 1) ShowMessageBox.ShowInformation("请先添加柜台信息!");
            }
            catch (Exception ex)
            {
                string errCode = "GL-0300";
                string errMsg = "交易员信息页面加载失败。";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                ShowMessageBox.ShowInformation(vte.ToString());
            }
        }

        #endregion

        #region 确定事件

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string mess;
            try
            {
                CT_CounterBLL CounterBLL = new CT_CounterBLL();

                if (!CounterBLL.TestCenterConnection())
                {
                    MessageBox.Show("柜台服务连接失败,请检查柜台服务是否开启!", "系统提示");
                    return;
                }
                if (this.m_EditType == 1)
                {
                    if (CheckUserInfo())
                    {
                        DataTable table = GetModifyNodes(this.m_EditType);

                        ManagementCenter.BLL.UserManage.TransactionManage transactionManage =
                            new ManagementCenter.BLL.UserManage.TransactionManage();

                        if (transactionManage.AddTransaction(table, CurrentUser, InitFund, out mess))
                        {
                            ShowMessageBox.ShowInformation("添加成功!");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation(mess);
                        }
                    }
                }
                else
                {
                    if (this.m_currentUser.AddType ==
                        (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.FrontTaransaction)
                    {
                        ShowMessageBox.ShowInformation("此交易员为虚拟前台所开设用户,不允许修改!");
                        return;
                    }
                    if (CheckUserInfo())
                    {
                        DataTable addtable = GetModifyNodes(1);
                        DataTable deltable = GetModifyNodes(2);

                        ManagementCenter.BLL.UserManage.TransactionManage transactionManage =
                            new ManagementCenter.BLL.UserManage.TransactionManage();

                        if (transactionManage.UpdateTransaction(addtable, deltable, CurrentUser,
                                                                this.che_UpdatePass.Checked, out mess))
                        {
                            ShowMessageBox.ShowInformation("修改成功!");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation(mess);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0301";
                string errMsg = "保存失败!";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                ShowMessageBox.ShowInformation(vte.ToString());
            }
        }

        #endregion

        #region 初始化用户信息

        /// <summary>
        /// 初始化用户信息
        /// </summary>
        public void InitUserInfo()
        {
            try
            {
                if (this.m_currentUser != null)
                {
                    this.txt_Address.Text = this.m_currentUser.Address;
                    this.txt_Postalcode.Text = this.m_currentUser.Postalcode;
                    this.txt_CertificateNo.Text = this.m_currentUser.CertificateNo;
                    this.txt_Email.Text = this.m_currentUser.Email;
                    this.txt_Name.Text = this.m_currentUser.UserName;
                    this.txt_Password.Text = "******";
                    this.txt_PassAgain.Text = "******";
                    this.txt_Telephone.Text = this.m_currentUser.Telephone;
                    foreach (object item in this.dll_CouterID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == m_currentUser.CouterID)
                        {
                            this.dll_CouterID.SelectedItem = item;
                            break;
                        }
                    }
                    foreach (object item in this.dll_CertificateStyle.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == m_currentUser.CertificateStyle)
                        {
                            this.dll_CertificateStyle.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0302";
                string errMsg = "交易员信息初始化失败!";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw vte;
            }

        }

        #endregion

        #region 检测用户信息

        /// <summary>
        /// 检测用户信息
        /// </summary>
        /// <returns></returns>
        public bool CheckUserInfo()
        {
            try
            {
                JudgmentMessage = string.Empty;
                if (this.dll_CouterID.Properties.Items.Count < 1)
                {
                    ShowMessageBox.ShowInformation("请先添加柜台信息!");
                    return false;
                }
                if (this.txt_Name.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入交易员(对应真实姓名项)名称!");//("请输入交易员名称!");
                    return false;
                }
                else
                {
                    if (!IsSuperLenth(this.txt_Name.Text, 20))
                    {
                        ShowMessageBox.ShowInformation("姓名超出有效范围长度20!");
                        return false;
                    }
                    this.m_currentUser.UserName = this.txt_Name.Text.ToString();
                }
                if (this.txt_CertificateNo.Text.ToString() != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_CertificateNo.Text.ToString(), 50))
                    {
                        ShowMessageBox.ShowInformation("证件号码超出有效范围长度50!");
                        return false;
                    }
                    if (((UComboItem)this.dll_CertificateStyle.SelectedItem).ValueIndex == (int)ManagementCenter.Model.CommonClass.Types.CertificateStyleEnum.StatusCard)
                    {
                        if (!InputTest.IsStatusCard(this.txt_CertificateNo.Text))
                        {
                            ShowMessageBox.ShowInformation("证件号码是15位或18位的合法格式!");
                            return false;
                        }
                    }
                    this.m_currentUser.CertificateNo = this.txt_CertificateNo.Text.ToString();
                    this.m_currentUser.CertificateStyle = ((UComboItem)this.dll_CertificateStyle.SelectedItem).ValueIndex;
                }
                else
                {
                    this.m_currentUser.CertificateNo = null;
                    this.m_currentUser.CertificateStyle = null;
                }
                if (this.txt_Postalcode.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Postalcode.Text, 50))
                    {
                        ShowMessageBox.ShowInformation("邮政编码超出有效范围长度50!");
                        return false;
                    }
                    if (!InputTest.IsPostCode(this.txt_Postalcode.Text))
                    {
                        ShowMessageBox.ShowInformation("邮编是6个整数!");
                        return false;
                    }
                    this.m_currentUser.Postalcode = this.txt_Postalcode.Text.ToString();
                }
                else
                {
                    this.m_currentUser.Postalcode = null;
                }

                if (this.txt_Email.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Email.Text, 50))
                    {
                        ShowMessageBox.ShowInformation("邮箱超出有效范围长度50!");
                        return false;
                    }
                    if (!InputTest.emailTest(this.txt_Email.Text))
                    {
                        ShowMessageBox.ShowInformation("邮箱格式错误,请输入正确的邮箱!");
                        return false;
                    }
                    this.m_currentUser.Email = this.txt_Email.Text;
                }
                else
                {
                    this.m_currentUser.Email = null;
                }

                if (this.txt_Telephone.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Telephone.Text, 20))
                    {
                        ShowMessageBox.ShowInformation("电话号码超出有效范围长度20!");
                        return false;
                    }
                    if (!InputTest.IsMobileOrTelephone(this.txt_Telephone.Text))
                    {
                        ShowMessageBox.ShowInformation("电话号码格式不正确!");
                        return false;
                    }
                    this.m_currentUser.Telephone = this.txt_Telephone.Text.ToString();
                }
                else
                {
                    this.m_currentUser.Telephone = null;
                }

                if (this.txt_Address.Text != string.Empty)
                {
                    if (!IsSuperLenth(this.txt_Address.Text, 50))
                    {
                        ShowMessageBox.ShowInformation("地址超出有效范围长度50!");
                        return false;
                    }
                    this.m_currentUser.Address = this.txt_Address.Text.ToString();
                }
                else
                {
                    this.m_currentUser.Address = null;
                }

                this.m_currentUser.CouterID = ((UComboItem)this.dll_CouterID.SelectedItem).ValueIndex;
                this.m_currentUser.RoleID = (int)ManagementCenter.Model.CommonClass.Types.RoleTypeEnum.Transaction;
                this.m_currentUser.AddType = (int)ManagementCenter.Model.CommonClass.Types.AddTpyeEnum.BackTaransaction;

                if (EditType == 1)
                {
                    try
                    {
                        string _Money = string.Empty;//输入的金额
                        if (this.txt_RMB.Text != string.Empty)
                        {
                            _Money = this.txt_RMB.Text;
                            string[] _lengthRMB = _Money.Split('.');
                            if (_lengthRMB[0].Length > 12)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(整数部分不能大于12位)!");
                                return false;
                            }
                            if (_lengthRMB.Length > 1)
                            {
                                if (_lengthRMB[1].Length > 3)
                                {
                                    ShowMessageBox.ShowInformation("小数部分不能大于3位!");
                                    return false;
                                }
                            }
                            if (this.txt_RMB.Text.Length > 16)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                                return false;
                            }
                            m_InitFund.RMB = decimal.Parse(this.txt_RMB.Text);

                        }
                        if (this.txt_US.Text != string.Empty)
                        {
                            _Money = this.txt_US.Text;
                            string[] _lengthRMB = _Money.Split('.');
                            if (_lengthRMB[0].Length > 12)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(整数部分不能大于12位)!");
                                return false;
                            }
                            if (_lengthRMB.Length > 1)
                            {
                                if (_lengthRMB[1].Length > 3)
                                {
                                    ShowMessageBox.ShowInformation("小数部分不能大于3位!");
                                    return false;
                                }
                            }
                            if (this.txt_US.Text.Length > 16)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                                return false;
                            }
                            m_InitFund.US = decimal.Parse(this.txt_US.Text);

                        }
                        if (this.txt_HK.Text != string.Empty)
                        {
                            _Money = this.txt_HK.Text;
                            string[] _lengthRMB = _Money.Split('.');
                            if (_lengthRMB[0].Length > 12)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(整数部分不能大于12位)!");
                                return false;
                            }
                            if (_lengthRMB.Length > 1)
                            {
                                if (_lengthRMB[1].Length > 3)
                                {
                                    ShowMessageBox.ShowInformation("小数部分不能大于3位!");
                                    return false;
                                }
                            }
                            if (this.txt_HK.Text.Length > 16)
                            {
                                ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                                return false;
                            }
                            m_InitFund.HK = decimal.Parse(this.txt_HK.Text);

                        }
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox.ShowInformation("请输入正确的金额!");
                        LogHelper.WriteError(ex.Message, ex);
                        return false;
                    }
                }
                //登陆密码检测
                if (EditType == 1 || (EditType == 2 && this.che_UpdatePass.Checked == true))
                {
                    if (this.txt_Password.Text.ToString() == string.Empty || this.txt_PassAgain.Text.ToString() == string.Empty)
                    {
                        ShowMessageBox.ShowInformation("密码不能为空!");
                        return false;
                    }
                    if (!IsSuperLenth(this.txt_Password.Text.ToString(), 12))
                    {
                        MessageBox.Show("密码超出有效范围长度12!");
                        return false;
                    }
                    if (!InputTest.LoginTest(this.txt_Password.Text.ToString()))
                    {
                        MessageBox.Show("密码中包含非法字符!");
                        return false;
                    }
                    if (this.txt_Password.Text.ToString() != this.txt_PassAgain.Text.ToString())
                    {
                        MessageBox.Show("两次输入的密码不一致!");
                        return false;
                    }
                    this.m_currentUser.Password = this.txt_Password.Text.ToString();
                    if ((EditType == 2 && this.che_UpdatePass.Checked == true))
                    {
                        this.m_currentUser.Password =
                            ManagementCenter.Model.CommonClass.UtilityClass.DesEncrypt(
                                this.txt_Password.Text.ToString(), string.Empty);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("用户输入检测失败!");
                string errCode = "GL-0303";
                string errMsg = "用户输入检测失败!";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }

        #endregion

        #region 绑定界面下拉框的数据

        /// <summary>
        /// 绑定界面下拉框的数据
        /// </summary>
        private void BinData()
        {
            try
            {
                this.dll_CertificateStyle.Properties.Items.Clear();
                this.dll_CertificateStyle.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetCertificateStyleList());
                dll_CertificateStyle.SelectedIndex = 0;

                this.dll_CouterID.Properties.Items.Clear();
                this.dll_CouterID.Properties.Items.AddRange(CommonClass.ComboBoxDataSource.GetAllCounterList());
                dll_CouterID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string errCode = "GL-0304";
                string errMsg = "用户输入检测失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                throw exception;
            }
        }

        #endregion

        #region 判断字符串是否为空或者长度大于指定长度

        /// <summary>
        /// 判断字符串是否为空或者长度大于指定长度
        /// </summary>
        /// <param name="Content">字段内容</param>
        /// <param name="Length">字段长度</param>
        /// <returns></returns>
        private bool LengthLessOrNull(string Content, int Length)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return true;
            }
            if (Length > 0)
            {
                if (Content.Length > Length)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断输入长度
        /// </summary>
        /// <param name="Content">输入内容</param>
        /// <param name="Lenth">输入的内容长度</param>
        /// <returns></returns>
        public bool IsSuperLenth(string Content, int Lenth)
        {
            if (Content == null) return true;
            if (Content == string.Empty) return true;
            if (Content.Length <= Lenth) return true;
            return false;
        }

        #endregion

        #region 初始化权限树

        /// <summary>
        /// 初始化权限树
        /// </summary>
        public void InitTreeView()
        {
            try
            {
                UM_DealerTradeBreedClassBLL DealerTradeBreedClassBLL = new UM_DealerTradeBreedClassBLL();

                int ID = int.MinValue;
                if (this.m_EditType == 2) ID = this.m_currentUser.UserID;
                DataSet ds = DealerTradeBreedClassBLL.GetUserBreedClassRight(ID);
                if (ds == null) return;

                this.RightTree.Nodes.Clear();

                int PreBourseTypeID = 0;
                TreeNode node = null;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int BourseTypeID = int.Parse(ds.Tables[0].Rows[i]["BourseTypeID"].ToString());

                    TreeNode treeNode;
                    //为不同交易所时，先添加交易所，再添加品种
                    if (PreBourseTypeID != BourseTypeID)
                    {
                        node = new TreeNode();
                        PreBourseTypeID = BourseTypeID;

                        node.Text = ds.Tables[0].Rows[i]["BourseTypeName"].ToString();

                        node.Tag = GetTreeTag(int.MinValue, int.MinValue, false, false);

                        this.RightTree.Nodes.Add(node);

                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["BreedClassName"].ToString()))
                        {
                            treeNode = new TreeNode();
                            treeNode.Text = ds.Tables[0].Rows[i]["BreedClassName"].ToString();

                            bool falg = false;
                            int DealerTradeBreedClassID = int.MaxValue;
                            int BreedClassID = int.Parse(ds.Tables[0].Rows[i]["BreedClassID"].ToString());
                            //初始化时判断有无该品种的交易权限
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UserID"].ToString()))
                            {
                                treeNode.Checked = true;
                                DealerTradeBreedClassID =
                                    int.Parse(ds.Tables[0].Rows[i]["DealerTradeBreedClassID"].ToString());
                                falg = true;
                            }
                            treeNode.Tag = GetTreeTag(BreedClassID, DealerTradeBreedClassID, true, falg);
                            node.Nodes.Add(treeNode);
                        }
                        node.ExpandAll();
                    }
                    //为同一个交易所类型时，直接添加品种
                    else
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["BreedClassName"].ToString()))
                        {
                            treeNode = new TreeNode();
                            treeNode.Text = ds.Tables[0].Rows[i]["BreedClassName"].ToString();
                            bool sign = false;
                            int DealerTradeBreedClassID = int.MaxValue;
                            int BreedClassID = int.Parse(ds.Tables[0].Rows[i]["BreedClassID"].ToString());
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UserID"].ToString()))
                            {
                                treeNode.Checked = true;
                                DealerTradeBreedClassID =
                                    int.Parse(ds.Tables[0].Rows[i]["DealerTradeBreedClassID"].ToString());
                                sign = true;
                            }
                            if (node != null) node.Nodes.Add(treeNode);
                            treeNode.Tag = GetTreeTag(BreedClassID, DealerTradeBreedClassID, true, sign);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0305";
                string errMsg = "初始化品种权限树失败！";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                throw exception;
            }
        }

        #endregion

        #region 根据类型获取修改的品种节点权限

        /// <summary>
        /// 根据类型获取修改的品种节点权限
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable GetModifyNodes(int Type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DealerTradeBreedClassID");
            dt.Columns.Add("BreedClassID");
            //添加
            if (Type == 1)
            {
                foreach (TreeNode node in RightTree.Nodes)
                {
                    if (node.Nodes.Count >= 1)
                    {
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            TreeNodeAttribute treeNodeAttribute = (TreeNodeAttribute)node1.Tag;
                            if (treeNodeAttribute.IsChecked == false && node1.Checked == true &&
                                treeNodeAttribute.IsBreedClass == true)
                            {
                                DataRow dr = dt.NewRow();
                                dr["DealerTradeBreedClassID"] = int.MaxValue;
                                dr["BreedClassID"] = treeNodeAttribute.BreedClassID;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            //删除
            else
            {
                foreach (TreeNode node in RightTree.Nodes)
                {
                    if (node.Nodes.Count >= 1)
                    {
                        foreach (TreeNode node1 in node.Nodes)
                        {
                            TreeNodeAttribute treeNodeAttribute = (TreeNodeAttribute)node1.Tag;
                            if (treeNodeAttribute.IsChecked == true && node1.Checked == false &&
                                treeNodeAttribute.IsBreedClass == true)
                            {
                                DataRow dr = dt.NewRow();
                                dr["DealerTradeBreedClassID"] = treeNodeAttribute.DealerTradeBreedClassID;
                                dr["BreedClassID"] = treeNodeAttribute.BreedClassID;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            return dt;
        }

        #endregion

        #region 获取树的Tag

        /// <summary>
        /// 获取树的Tag
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="DealerTradeBreedClassID">用户交易该权限生成的ID</param>
        /// <param name="IsBreedClass">是否为品种</param>
        /// <param name="IsChecked">初试化时是否有权限</param>
        /// <returns></returns>
        public Object GetTreeTag(int BreedClassID, int DealerTradeBreedClassID, bool IsBreedClass, bool IsChecked)
        {
            TreeNodeAttribute TreeNodeAttribute = new TreeNodeAttribute();
            TreeNodeAttribute.BreedClassID = BreedClassID;
            TreeNodeAttribute.IsBreedClass = IsBreedClass;
            TreeNodeAttribute.IsChecked = IsChecked;
            TreeNodeAttribute.DealerTradeBreedClassID = DealerTradeBreedClassID;
            return (object)TreeNodeAttribute;
        }

        #endregion

        #region 显示初始资金

        /// <summary>
        /// 显示初始资金
        /// </summary>
        public void InitOriginationFund()
        {
            try
            {
                UM_OriginationFundBLL OriginationFundBLL = new UM_OriginationFundBLL();
                if (CurrentUser.UserID == 0 || CurrentUser.UserID == int.MinValue) return;
                List<UM_OriginationFund> T_UM_OriginationFund =
                    OriginationFundBLL.GetListArrayByUserID(CurrentUser.UserID);
                if (T_UM_OriginationFund == null) return;
                foreach (UM_OriginationFund OriginationFund in T_UM_OriginationFund)
                {
                    if (OriginationFund.TransactionCurrencyTypeID == (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.RMB)
                    {
                        this.txt_RMB.Text = OriginationFund.FundMoney.ToString();
                    }
                    else if (OriginationFund.TransactionCurrencyTypeID == (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.HK)
                    {
                        this.txt_HK.Text = OriginationFund.FundMoney.ToString();
                    }
                    else if (OriginationFund.TransactionCurrencyTypeID == (int)GTA.VTS.Common.CommonObject.Types.CurrencyType.US)
                    {
                        this.txt_US.Text = OriginationFund.FundMoney.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-0306";
                string errMsg = "获取初始资金显示失败！";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
                //throw exception;
            }
        }

        #endregion

        #region 改变按纽事件

        private void che_UpdatePass_CheckedChanged(object sender, EventArgs e)
        {
            if (EditType == 1) return;
            if (this.che_UpdatePass.Checked == true)
            {
                this.txt_Password.Enabled = true;
                this.txt_PassAgain.Enabled = true;
                this.txt_Password.Text = string.Empty;
                this.txt_PassAgain.Text = string.Empty;
            }
            else
            {
                this.txt_Password.Enabled = false;
                this.txt_PassAgain.Enabled = false;
                this.txt_Password.Text = "********";
                this.txt_PassAgain.Text = "********";
            }
        }

        #endregion

        #region 初始化控件

        private void SetControlsIsEnabled()
        {
            if (EditType == 1)
            {
                this.che_UpdatePass.Visible = false;
            }
            else
            {
                this.che_UpdatePass.Visible = true;
                this.panelFund.Enabled = false;
                this.txt_Password.Enabled = false;
                this.txt_PassAgain.Enabled = false;
                this.dll_CouterID.Enabled = false;
            }
        }

        #endregion

        #region 取消按纽事件

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 选择权限树后的Check事件
        /// <summary>
        /// 选择权限树后的Check事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            this.RightTree.AfterCheck -= new TreeViewEventHandler(RightTree_AfterCheck);
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Checked == true)
                {
                    foreach (TreeNode Node in e.Node.Nodes)
                    {
                        Node.Checked = true;
                    }
                }
                else
                {
                    foreach (TreeNode Node in e.Node.Nodes)
                    {
                        Node.Checked = false;
                    }
                }
            }
            else
            {
                if (e.Node.Parent != null)
                {
                    if (e.Node.Checked == true)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else
                    {
                        bool symble = true;
                        foreach (TreeNode Node in e.Node.Parent.Nodes)
                        {
                            if (Node.Checked == true)
                            {
                                symble = false;
                                break;
                            }
                        }
                        if (symble == true)
                            e.Node.Parent.Checked = false;
                    }
                }
            }
            this.RightTree.AfterCheck += new TreeViewEventHandler(RightTree_AfterCheck);
        }
        #endregion
    }


    #region 树的Tag属性类

    /// <summary>
    /// 树的Tag属性
    /// </summary>
    public class TreeNodeAttribute
    {
        /// <summary>
        /// 品种ID
        /// </summary>
        public int BreedClassID = int.MaxValue;
        /// <summary>
        /// 品种ID标识
        /// </summary>
        public bool IsBreedClass = false;
        /// <summary>
        /// 是否选中 
        /// </summary>
        public bool IsChecked = false;
        /// <summary>
        /// 可交易品种ID
        /// </summary>
        public int DealerTradeBreedClassID = int.MaxValue;
    }

    #endregion
}