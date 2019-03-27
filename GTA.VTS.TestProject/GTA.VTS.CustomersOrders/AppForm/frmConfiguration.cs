using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.CustomersOrders.BLL;

namespace GTA.VTS.CustomersOrders.AppForm
{
    public partial class frmConfiguration : Form
    {
        #region 变量
        internal WCFServices wcfLogic;
        #endregion 变量

        #region 构造函数
        public frmConfiguration()
        {
            InitializeComponent();
            wcfLogic = WCFServices.Instance;
            LocalhostResourcesFormText();
        }
        #endregion

        #region 窗体加载事件
        /// <summary>
        /// 窗体加载时候将配置文件信息显示在界面中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            this.txtManagementIP.Text = ServerConfig.ManagementIP;
            this.txtReckoningIP.Text = ServerConfig.ReckoningIP;
            this.txtAccountAndCapitalManagementClient.Text = ServerConfig.AccountAndCapitalManagementServerName;
            this.txtAccountAndCapitalManagementClientIP.Text = ServerConfig.AccountAndCapitalManagementClientPort;
            this.txtTransactionManageClient.Text = ServerConfig.TransactionManageServerName;
            this.txtTransactionManageClientIP.Text = ServerConfig.TransactionManageClientPort;
            this.txtDoOrderClient.Text = ServerConfig.DoOrderServerName;
            this.txtDoOrderClientIP.Text = ServerConfig.DoOrderClientPort;
            this.txtHKTraderQueryClient.Text = ServerConfig.HKTraderQueryServerName;
            this.txtHKTraderQueryClientIP.Text = ServerConfig.HKTraderQueryClientPort;
            this.txtTraderQueryClient.Text = ServerConfig.TraderQueryServerName;
            this.txtTraderQueryClientIP.Text = ServerConfig.TraderQueryClientPort;
            this.txtOrderDealRptClient.Text = ServerConfig.OrderDealRptServerName;
            this.txtOrderDealRptClientIP.Text = ServerConfig.OrderDealRptClientPort;
            this.txtQueryPageSize.Text = ServerConfig.QueryPageSize.ToString();
        }
        #endregion

        #region 取消按钮事件
        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            //this.Close();
            //LoginForm login = new LoginForm();
            //login.Show();
        }
        #endregion

        #region 确定按钮事件
        /// <summary>
        /// 确定修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            #region 获取用户输入的信息

            string ManagementIP = this.txtManagementIP.Text.ToString();
            if (!string.IsNullOrEmpty(ManagementIP))
            {
                if (!ManagementIP.ToLower().Equals("localhost"))
                {
                    if (Checkip(ManagementIP))
                    {
                        ServerConfig.ManagementIP = ManagementIP;
                    }
                    else
                    {
                        error.Clear();
                        error.SetError(txtManagementIP, "ManagementIP 地址无效");
                        return;
                    }
                }
                else if (ManagementIP.ToLower().Equals("localhost"))
                {
                    ServerConfig.ManagementIP = ManagementIP;
                }
                else
                {
                    error.Clear();
                    error.SetError(txtManagementIP, "ManagementIP 地址无效");
                    return;
                }
            }
            else
            {
                error.Clear();
                error.SetError(txtManagementIP, "ManagementIP 不能为空");
                return;
            }
            string ReckoningIP = this.txtReckoningIP.Text.ToString();
            if (!string.IsNullOrEmpty(ReckoningIP))
            {
                if (!ReckoningIP.ToLower().Equals("localhost"))
                {
                    if (Checkip(ReckoningIP))
                    {
                        ServerConfig.ReckoningIP = ReckoningIP;
                    }
                    else
                    {
                        error.Clear();
                        error.SetError(txtReckoningIP, "ReckoningIP 地址无效");
                        return;
                    }
                }
                else if (ReckoningIP.ToLower().Equals("localhost"))
                {
                    ServerConfig.ReckoningIP = ReckoningIP;
                }
                else
                {
                    error.Clear();
                    error.SetError(txtReckoningIP, "ReckoningIP 地址无效");
                    return;
                }
            }
            else
            {
                error.Clear();
                error.SetError(txtReckoningIP, "ReckoningIP 不能为空");
                return;
            }
            string AccountAndCapttalManagementClient = this.txtAccountAndCapitalManagementClient.Text;
            if (!string.IsNullOrEmpty(AccountAndCapttalManagementClient))
            {
                ServerConfig.AccountAndCapitalManagementServerName = AccountAndCapttalManagementClient;
            }
            else
            {
                error.Clear();
                error.SetError(txtAccountAndCapitalManagementClient, "AccountAndCapttalManagementClient 不能为空");
                return;
            }
            string AccountAndCapitalManagementClientPort = this.txtAccountAndCapitalManagementClientIP.Text.ToString();
            if (!string.IsNullOrEmpty(AccountAndCapitalManagementClientPort))
            {
                ServerConfig.AccountAndCapitalManagementClientPort = AccountAndCapitalManagementClientPort;
            }
            else
            {
                error.Clear();
                error.SetError(txtAccountAndCapitalManagementClientIP, "AccountAndCapitalManagementClientPort 不能为空");
                return;
            }

            string TraderQueryClient = this.txtTraderQueryClient.Text;
            if (!string.IsNullOrEmpty(TraderQueryClient))
            {
                ServerConfig.TraderQueryServerName = TraderQueryClient;
            }
            else
            {
                error.Clear();
                error.SetError(txtAccountAndCapitalManagementClient, "TraderQueryClient 不能为空");
                return;
            }
            string TraderQueryClientPort = this.txtTraderQueryClientIP.Text.ToString();
            if (!string.IsNullOrEmpty(TraderQueryClientPort))
            {
                ServerConfig.TraderQueryClientPort = TraderQueryClientPort;
            }
            else
            {
                error.Clear();
                error.SetError(txtAccountAndCapitalManagementClientIP, "TraderQueryClientPort 不能为空");
                return;
            }

            string TransactionManageClient = this.txtTransactionManageClient.Text;
            if (!string.IsNullOrEmpty(TransactionManageClient))
            {
                ServerConfig.TransactionManageServerName = TransactionManageClient;
            }
            else
            {
                error.Clear();
                error.SetError(txtTransactionManageClient, "TransactionManageClient 不能为空");
                return;
            }
            string TransactionManageClientPort = this.txtTransactionManageClientIP.Text.ToString();
            if (!string.IsNullOrEmpty(TransactionManageClientPort))
            {
                ServerConfig.TransactionManageClientPort = TransactionManageClientPort;
            }
            else
            {
                error.Clear();
                error.SetError(txtTransactionManageClientIP, "TransactionManageClientPort 不能为空");
                return;
            }
            string OrderClient = this.txtDoOrderClient.Text;
            if (!string.IsNullOrEmpty(OrderClient))
            {
                ServerConfig.DoOrderServerName = OrderClient;
            }
            else
            {
                error.Clear();
                error.SetError(txtDoOrderClient, "OrderClient 不能为空");
                return;
            }
            string OrderClientPort = this.txtDoOrderClientIP.Text.ToString();
            if (!string.IsNullOrEmpty(OrderClientPort))
            {
                ServerConfig.DoOrderClientPort = OrderClientPort;
            }
            else
            {
                error.Clear();
                error.SetError(txtDoOrderClientIP, "OrderClientPort 不能为空");
                return;
            }
            string HKTraderQueryClient = this.txtHKTraderQueryClient.Text;
            if (!string.IsNullOrEmpty(HKTraderQueryClient))
            {
                ServerConfig.HKTraderQueryServerName = HKTraderQueryClient;
            }
            else
            {
                error.Clear();
                error.SetError(txtHKTraderQueryClient, "HKTraderQueryClient 不能为空");
                return;
            }
            string HKTraderQueryClientPort = this.txtHKTraderQueryClientIP.Text.ToString();
            if (!string.IsNullOrEmpty(HKTraderQueryClientPort))
            {
                ServerConfig.HKTraderQueryClientPort = HKTraderQueryClientPort;
            }
            else
            {
                error.Clear();
                error.SetError(txtHKTraderQueryClientIP, "HKTraderQueryClientPort 不能为空");
                return;
            }
            string OrderDealRptClient = this.txtOrderDealRptClient.Text;
            if (!string.IsNullOrEmpty(OrderDealRptClient))
            {
                ServerConfig.OrderDealRptServerName = OrderDealRptClient;
            }
            else
            {
                error.Clear();
                error.SetError(txtOrderDealRptClient, "OrderDealRptClient 不能为空");
                return;
            }
            string OrderDealRptClientPort = this.txtOrderDealRptClientIP.Text.ToString();
            if (!string.IsNullOrEmpty(OrderDealRptClientPort))
            {
                ServerConfig.OrderDealRptClientPort = OrderDealRptClientPort;
            }
            else
            {
                error.Clear();
                error.SetError(txtOrderDealRptClientIP, "OrderDealRptClientPort 不能为空");
                return;
            }
            #endregion

            error.Clear();
            int pageSize;
            if (!int.TryParse(txtQueryPageSize.Text.Trim(), out pageSize))
            {
                error.SetError(txtQueryPageSize, "查询结果每页记录数必须是整数！");
                return;
            }
            if (pageSize > 500)
            {
                error.SetError(txtQueryPageSize, "查询结果每页记录数不能超过500！");
                return;
            }
            if (pageSize < 1)
            {
                error.SetError(txtQueryPageSize, "查询结果每页记录数不能小于1！");
                return;
            }
            ServerConfig.QueryPageSize = pageSize;

            MessageBox.Show("配置修改成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.Close();
        }
        #endregion

        #region  对IP地址进行验证
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
        #endregion

        #region 界面多语言
        /// <summary>
        /// 刷新显示相关语方类型的标签
        /// </summary>
        private void LocalhostResourcesFormText()
        {
            this.lblManagementIP.Text = ResourceOperate.Instanse.GetResourceByKey("ManagementIP");
            this.lblTransactionManageClient.Text = ResourceOperate.Instanse.GetResourceByKey("TransactionManageClient");
            this.lblTransactionManageClientPort.Text = ResourceOperate.Instanse.GetResourceByKey("Port");
            this.lblReckoningIP.Text = ResourceOperate.Instanse.GetResourceByKey("ReckoningIP");
            this.lblDoOrderClient.Text = ResourceOperate.Instanse.GetResourceByKey("DoOrderClient");
            this.lblDoOrderClientPort.Text = ResourceOperate.Instanse.GetResourceByKey("Port");
            this.lblTraderQueryClient.Text = ResourceOperate.Instanse.GetResourceByKey("TraderQueryClient");
            this.lblTraderQueryClientPort.Text = ResourceOperate.Instanse.GetResourceByKey("Port");
            this.lblHKTraderQueryClient.Text = ResourceOperate.Instanse.GetResourceByKey("HKTraderQueryClient");
            this.lblHKTraderQueryClientPort.Text = ResourceOperate.Instanse.GetResourceByKey("Port");
            this.lblOrderDealRptClient.Text = ResourceOperate.Instanse.GetResourceByKey("OrderDealRptClient");
            this.lblOrderDealRptClientPort.Text = ResourceOperate.Instanse.GetResourceByKey("Port");
            this.lblAccountAndCapitalManagementClient.Text = ResourceOperate.Instanse.GetResourceByKey("AccountAndCapitalManagementClient");
            this.lblAccountAndCapitalManagementClientPort.Text = ResourceOperate.Instanse.GetResourceByKey("Port");
            this.btnOK.Text = ResourceOperate.Instanse.GetResourceByKey("OK");
            this.btnCancel.Text = ResourceOperate.Instanse.GetResourceByKey("Cancel");
            this.lblPagesize.Text = ResourceOperate.Instanse.GetResourceByKey("Pagesize");
        }
        #endregion
    }
}
