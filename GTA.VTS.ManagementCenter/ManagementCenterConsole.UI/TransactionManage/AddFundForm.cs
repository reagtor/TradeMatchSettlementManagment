using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using GTA.VTS.Common.CommonObject;
using ManagementCenterConsole.UI.CommonClass;


namespace ManagementCenterConsole.UI.TransactionManage
{
    /// <summary>
    /// 追加资金 错误编码范围0331-0340
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// 描述:添加资金相关输入检验
    /// 修改作者：刘书伟
    /// 日期：2010-05-06
    /// </summary>
    public partial class AddFundForm : DevExpress.XtraEditors.XtraForm
    {
        #region  构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AddFundForm()
        {
            InitializeComponent();
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
            string mess;
            try
            {
                ManagementCenter.BLL.UserManage.TransactionManage tm = new ManagementCenter.BLL.UserManage.TransactionManage();

                decimal rbm = decimal.MaxValue, hk = decimal.MaxValue, us = decimal.MaxValue;
                int userid = int.MaxValue;
                try
                {
                    userid = int.Parse(this.txt_ID.Text);
                }
                catch (Exception)
                {
                    ShowMessageBox.ShowInformation("请输入正确的交易员编号!");
                    return;
                }
                try
                {
                    UM_FundAddInfo uM_FundAddInfo =
                        tm.AdminFindTraderBankCapitalAccountInfoByID(
                            CommonClass.ParameterSetting.Mananger.UserID.ToString(),
                            CommonClass.ParameterSetting.Mananger.Password, userid.ToString(), out mess);
                    
                    string _Money = string.Empty;//输入的金额
                    decimal _BankAvailableCapital = 0;//银行账户可用资金
                    if (this.txt_rmb.Text != string.Empty)
                    {
                        _Money = this.txt_rmb.Text;
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
                        if (this.txt_rmb.Text.Length > 16)
                        {
                            ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                            return;
                        }
                        _BankAvailableCapital = 0;
                        _BankAvailableCapital = uM_FundAddInfo.RMBNumber.Value + decimal.Parse(this.txt_rmb.Text);
                        _lengthRMB = _BankAvailableCapital.ToString().Split('.');
                        if (_lengthRMB[0].Length > 12)
                        {
                            ShowMessageBox.ShowInformation("银行账户的人民币总资金整数部分不能大于12位!");
                            return;
                        }

                        rbm = decimal.Parse(this.txt_rmb.Text);
                    }

                    if (this.txt_hk.Text != string.Empty)
                    {
                        _Money = this.txt_hk.Text;
                        string[] _lengthHk = _Money.Split('.');
                        if (_lengthHk[0].Length > 12)
                        {
                            ShowMessageBox.ShowInformation("超出存储的范围(整数部分不能大于12位)!");
                            return;
                        }
                        if (_lengthHk.Length > 1)
                        {
                            if (_lengthHk[1].Length > 3)
                            {
                                ShowMessageBox.ShowInformation("小数部分不能大于3位!");
                                return;
                            }
                        }
                        if (this.txt_hk.Text.Length > 16)
                        {
                            ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                            return;
                        }

                        _BankAvailableCapital = 0;
                        _BankAvailableCapital = uM_FundAddInfo.HKNumber.Value + decimal.Parse(this.txt_hk.Text);
                        _lengthHk = _BankAvailableCapital.ToString().Split('.');
                        if (_lengthHk[0].Length > 12)
                        {
                            ShowMessageBox.ShowInformation("银行账户的港币总资金整数部分不能大于12位!");
                            return;
                        }

                        hk = decimal.Parse(this.txt_hk.Text);
                    }

                    if (this.txt_us.Text != string.Empty)
                    {
                        _Money = this.txt_us.Text;
                        string[] _lengthUS = _Money.Split('.');
                        if (_lengthUS[0].Length > 12)
                        {
                            ShowMessageBox.ShowInformation("超出存储的范围(整数部分不能大于12位)!");
                            return;
                        }
                        if (_lengthUS.Length > 1)
                        {
                            if (_lengthUS[1].Length > 3)
                            {
                                ShowMessageBox.ShowInformation("小数部分不能大于3位!");
                                return;
                            }
                        }
                        if (this.txt_us.Text.Length > 16)
                        {
                            ShowMessageBox.ShowInformation("超出存储的范围(不能大于16位)!");
                            return;
                        }
                        _BankAvailableCapital = 0;
                        _BankAvailableCapital = uM_FundAddInfo.USNumber.Value + decimal.Parse(this.txt_us.Text);
                        _lengthUS = _BankAvailableCapital.ToString().Split('.');
                        if (_lengthUS[0].Length > 12)
                        {
                            ShowMessageBox.ShowInformation("银行账户的美元总资金整数部分不能大于12位!");
                            return;
                        }

                        us = decimal.Parse(this.txt_us.Text);
                    }

                    if (string.IsNullOrEmpty(txt_rmb.Text) && string.IsNullOrEmpty(txt_hk.Text) && string.IsNullOrEmpty(txt_us.Text))
                    {
                        ShowMessageBox.ShowInformation("金额不能为空!");
                        return;
                    }
                }
                catch
                {
                    ShowMessageBox.ShowInformation("请输入正确的金额!");
                    return;
                }
                //ManagementCenter.BLL.UserManage.TransactionManage tm=new ManagementCenter.BLL.UserManage.TransactionManage();
                UM_FundAddInfo UM_FundAddInfo = new UM_FundAddInfo();
                UM_FundAddInfo.AddTime = System.DateTime.Now;
                UM_FundAddInfo.UserID = userid;
                UM_FundAddInfo.ManagerID = ParameterSetting.Mananger.UserID;
                //调用柜台开户方法

                UM_FundAddInfo.RMBNumber = (decimal)rbm;
                UM_FundAddInfo.HKNumber = (decimal)hk;
                UM_FundAddInfo.USNumber = (decimal)us;

                if (tm.AddFund(UM_FundAddInfo, out mess))
                {
                    ShowMessageBox.ShowInformation("追加资金成功!");
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    ShowMessageBox.ShowInformation(mess);
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("追加资金失败!");
                string errCode = "GL-0331";
                string errMsg = "追加资金失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), ex);
            }
        }
        #endregion

        #region 取消按纽事件
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID = int.MaxValue;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFundForm_Load(object sender, EventArgs e)
        {
            if (UserID != int.MaxValue)
            {
                this.txt_ID.Text = UserID.ToString();
            }
        }
    }
}