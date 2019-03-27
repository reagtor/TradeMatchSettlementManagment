using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.BLL;
using GTA.VTS.CustomersOrders.TransactionManageService;
using GTA.VTS.CustomersOrders.Resources;

namespace GTA.VTS.CustomersOrders.AppForm
{
    /// <summary>
    /// 描述：登陆界面
    /// 作者：李建华
    /// 时间：2010-01-14
    /// 修改
    /// 描述：添加登陆界面的用户验证功能和多语言
    /// 作者：叶振东
    /// 时间：2010-01-22
    /// </summary>
    public partial class LoginForm : Form
    {
        #region 变量
        internal WCFServices wcfLogic;
        #endregion 变量

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
            LocalhostResourcesFormText();
            //Initializew();
            //  string x = Encrypt.DesDecrypt("rB7FoTU315s=", string.Empty);
            wcfLogic = WCFServices.Instance;
        }
        #endregion 构造函数

        #region 初始化
        /// <summary>
        /// 窗体下载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginForm_Load(object sender, EventArgs e)
        {
            string traderID = ServerConfig.TraderID;
            string password = ServerConfig.PassWord;

            if (!string.IsNullOrEmpty(traderID))
            {
                txtTradeID.Text = traderID;
            }
            if (!string.IsNullOrEmpty(password))
            {
                string pws = Encrypt.DesDecrypt(password, string.Empty);
                this.txtPassword.Text = pws;
            }
            this.cmbLanguageType.SelectedIndex = 0;
        }

        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.gpbLoginInfo.Text = ResourceOperate.Instanse.GetResourceByKey("LoginInfo");
            this.labLanguage.Text = ResourceOperate.Instanse.GetResourceByKey("LableLanguage");
            this.btnLogin.Text = ResourceOperate.Instanse.GetResourceByKey("Login");
            this.btnCancel.Text = ResourceOperate.Instanse.GetResourceByKey("Cancel");
            this.lblTradeID.Text = ResourceOperate.Instanse.GetResourceByKey("TradeID");
            this.lblTradePassword.Text = ResourceOperate.Instanse.GetResourceByKey("PassWord");
            this.Text = ResourceOperate.Instanse.GetResourceByKey("Test");
        }

        /// <summary>
        /// 初始化WCF连接
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Initializew()
        {

            //try
            //{
            //    wcfLogic = new WCFServices();
            //    string ChannelID = "";
            //    bool isSuccess = wcfLogic.Initialize(ChannelID);
            //    if (!isSuccess)
            //    {
            //        return false;
            //    }
            //    string channleid = ServerConfig.ChannelID;
            //    string title = " [ChannelID: " + channleid + "]";
            //    this.Text += title;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.WriteError(ex.Message, ex);
            //    return false;
            //}

            return true;
        }
        #endregion 初始化

        #region 窗体事件
        /// <summary>
        /// 退出按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 验证是否是合法的数字格式
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool DecimalTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9]+$");

        }

        /// <summary>
        /// 语言类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLanguageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLanguageType.SelectedIndex == 0)
            {
                ResourceOperate.Instanse.InitResourceLocal("zh-CN");
            }
            else if (cmbLanguageType.SelectedIndex == 1)
            {
                ResourceOperate.Instanse.InitResourceLocal("EN");
            }
            LocalhostResourcesFormText();
        }

        /// <summary>
        /// 登录按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                #region 根据用户输入的服务其地址进行连接相对应的服务
                string errMsg = "";
                mdiMainForm mainForm = new mdiMainForm();
                bool isSuccess = mainForm.Initialize(txtTradeID.Text.Trim(), ref errMsg);
                if (!isSuccess)
                {
                    //errMsg = ResourceOperate.Instanse.GetResourceByKey("WCF");
                    error.SetError(txtPassword, errMsg);
                    return;
                }
                #endregion 根据用户输入的服务其地址进行连接相对应的服务
                #region 对用户输入的信息进行判断，并在判断通过时保存到配置文件中

                if (!string.IsNullOrEmpty(this.txtTradeID.Text))
                {
                    #region 对资金帐户的长度和输入格式进行判断

                    #region 判断文本框中数据是否与APP中数据是否相等，如果相等则不修改APP中数据，否则修改APP中数据

                    string Password = this.txtPassword.Text;
                    int userId;
                    if (!string.IsNullOrEmpty(Password))
                    {
                        #region 对TraderID进行判断

                        string message;
                        //将用户输入的密码进行加密，否则与数据库中加密过的密码进行比较会出现密码验证错误
                        string password = Encrypt.DesEncrypt(Password, string.Empty);

                        //对用户输入的TradeId进行判断，如果查询出柜台说明该用户是交易员，否则进行管理员判断
                        int.TryParse(this.txtTradeID.Text.Trim(), out userId);
                        GTA.VTS.CustomersOrders.TransactionManageService.CT_Counter counter =
                            wcfLogic.TransactionConfirm(userId, password, out message);
                        if (counter == null)
                        {
                            string LoginName = this.txtTradeID.Text.Trim();
                            string Loginerror = ResourceOperate.Instanse.GetResourceByKey("Loginerror");

                            #region 管理员验证

                            string messages;
                            bool result = wcfLogic.AdminConfirmation(LoginName,
                                                                     password,
                                                                     out messages);
                            if (!result)
                            {
                                error.Clear();
                                error.SetError(txtPassword, Loginerror);
                                return;
                            }
                            ServerConfig.TraderID = LoginName;
                            ServerConfig.PassWord = password;
                            ServerConfig.Refresh();
                            this.DialogResult = DialogResult.OK;
                            this.Close();

                            #endregion 管理员验证
                        }
                        else
                        {
                            int counterID = counter.CouterID;
                            int trade;
                            if (int.TryParse(txtTradeID.Text, out trade))
                            {
                                Capitalaccount(counterID, trade);
                                ServerConfig.PassWord = password;
                                ServerConfig.TraderID = txtTradeID.Text.Trim();
                                ServerConfig.Refresh();
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                error.Clear();
                                string errors = ResourceOperate.Instanse.GetResourceByKey("TradeError");
                                error.SetError(txtPassword, errors);
                                return;
                            }
                        }

                        #endregion 对TraderID进行判断
                    }
                    else
                    {
                        error.Clear();
                        string errors = ResourceOperate.Instanse.GetResourceByKey("errors");
                        error.SetError(txtPassword, errors);
                        return;
                    }

                    #endregion 判断文本框中数据是否与APP中数据是否相等，如果相等则不修改APP中数据，否则修改APP中数据

                    #endregion 对资金帐户的长度和输入格式进行判断
                }
                else
                {
                    error.Clear();
                    string TradeIDerror = ResourceOperate.Instanse.GetResourceByKey("TradeIDerror");
                    error.SetError(txtTradeID, TradeIDerror);
                    return;
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据柜台和交易员ID帐号类型获取交易员资金帐号
        /// </summary>
        public void Capitalaccount(int counterID, int trade)
        {
            string XHCapitalAccount = ProductionAccount.FormationAccount(counterID, trade, 2);
            string HKCapitalAccount = ProductionAccount.FormationAccount(counterID, trade, 8);
            string GZQHCapitalAccount = ProductionAccount.FormationAccount(counterID, trade, 6);
            string SPQHCapitalAccount = ProductionAccount.FormationAccount(counterID, trade, 4);
            ServerConfig.XHCapitalAccount = XHCapitalAccount;
            ServerConfig.HKCapitalAccount = HKCapitalAccount;
            ServerConfig.GZQHCapitalAccount = GZQHCapitalAccount;
            ServerConfig.SPQHCapitalAccount = SPQHCapitalAccount;
        }

        /// <summary>
        /// 对用户输入的IP地址进行验证是否合法的IP地址
        /// </summary>
        /// <param name="ip">用户输入的IP地址</param>
        /// <returns>IP地址是否合法</returns>
        public bool Checkip(string ip)
        {
            int i, j, k;
            if (ip.Length > 15 || ip.Length < 7)
            {
                return false;
            }
            for (i = 0; i < 3; i++)
            {
                j = ip.IndexOf(".", 0);
                if (j == -1)
                {
                    return false;
                }
                k = Convert.ToInt32(ip.Substring(0, j));
                if (k < 0 || k > 255)
                {
                    return false;
                }
                if (k == 0 && i == 0)
                {
                    return false;
                }
                ip = ip.Substring(j + 1);
            }
            k = Convert.ToInt32(ip);
            if (k < 0 || k > 255)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region 登陆界面文本框回车事件
        /// <summary>
        /// 交易员回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTradeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.txtPassword.Focus();
            }
        }

        /// <summary>
        /// 密码文本框回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.cmbLanguageType.Focus();
            }
        }
        /// <summary>
        /// 语言下拉列表回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLanguageType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.btnLogin.Focus();
            }
        }

        /// <summary>
        /// 登陆按钮回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            #region 根据用户输入的服务其地址进行连接相对应的数据库
            #endregion 根据用户输入的服务其地址进行连接相对应的数据库
            #region 对用户输入的信息进行判断，并在判断通过时保存到配置文件中
            if (!string.IsNullOrEmpty(this.txtTradeID.Text))
            {
                #region 对资金帐户的长度和输入格式进行判断
                #region 判断文本框中数据是否与APP中数据是否相等，如果相等则不修改APP中数据，否则修改APP中数据
                string Password = this.txtPassword.Text;
                int userId;
                if (!string.IsNullOrEmpty(Password))
                {
                    #region 对TraderID进行判断
                    string message;
                    //将用户输入的密码进行加密，否则与数据库中加密过的密码进行比较会出现密码验证错误
                    string password = Encrypt.DesEncrypt(Password, string.Empty);

                    //对用户输入的TradeId进行判断，如果查询出柜台说明该用户是交易员，否则进行管理员判断
                    int.TryParse(this.txtTradeID.Text.Trim(), out userId);
                    GTA.VTS.CustomersOrders.TransactionManageService.CT_Counter counter = wcfLogic.TransactionConfirm(userId, password, out message);
                    if (counter == null)
                    {
                        string LoginName = this.txtTradeID.Text.Trim();
                        string Loginerror = ResourceOperate.Instanse.GetResourceByKey("Loginerror");
                        #region 管理员验证
                        string messages;
                        bool result = wcfLogic.AdminConfirmation(LoginName,
                                                                 password,
                                                                 out messages);
                        if (!result)
                        {
                            error.Clear();
                            error.SetError(txtPassword, Loginerror);
                            return;
                        }
                        ServerConfig.TraderID = LoginName;
                        ServerConfig.PassWord = password;
                        ServerConfig.Refresh();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        #endregion 管理员验证
                    }
                    else
                    {
                        int counterID = counter.CouterID;
                        int trade;
                        if (int.TryParse(txtTradeID.Text, out trade))
                        {
                            Capitalaccount(counterID, trade);
                            ServerConfig.PassWord = password;
                            ServerConfig.TraderID = txtTradeID.Text.Trim();
                            ServerConfig.Refresh();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            error.Clear();
                            string errors = ResourceOperate.Instanse.GetResourceByKey("TradeError");
                            error.SetError(txtPassword, errors);
                            return;
                        }
                    }
                    #endregion 对TraderID进行判断
                }
                else
                {
                    error.Clear();
                    string errors = ResourceOperate.Instanse.GetResourceByKey("errors");
                    error.SetError(txtPassword, errors);
                    return;
                }
                #endregion 判断文本框中数据是否与APP中数据是否相等，如果相等则不修改APP中数据，否则修改APP中数据
                #endregion 对资金帐户的长度和输入格式进行判断
            }
            else
            {
                error.Clear();
                string TradeIDerror = ResourceOperate.Instanse.GetResourceByKey("TradeIDerror");
                error.SetError(txtTradeID, TradeIDerror);
                return;
            }
            #endregion
        }
        #endregion 登陆界面文本框回车事件

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //LoginForm login = new LoginForm();
            this.Hide();

            frmConfiguration configuration = new frmConfiguration();
            configuration.ShowDialog();
            this.Show();
        }
    }
        #endregion
}
