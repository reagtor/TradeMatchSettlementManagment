using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ManagementCenter.BLL.QH;
using ManagementCenter.Model.QH;

namespace ManagementCenterConsole.UI.CommonParameterSet
{
    /// <summary>
    /// Desc: 结算价管理
    /// Create By: 董鹏
    /// Create Date:2010-03-11
    /// </summary>
    public partial class TodaySettlementPriceManagerUI : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 结算价管理业务类
        /// </summary>
        QH_TodaySettlementPriceBLL qh_TodaySettlementPriceBLL;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TodaySettlementPriceManagerUI()
        {
            InitializeComponent();
            qh_TodaySettlementPriceBLL = new QH_TodaySettlementPriceBLL();
        }
        #endregion

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TodaySettlementPriceManagergerUI_Load(object sender, EventArgs e)
        {

            //ShowSettlementPriceList();
        }

        /// <summary>
        /// 点击手动清算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReckoning_Click(object sender, EventArgs e)
        {
            string errMsg;
            List<QH_TodaySettlementPriceInfoEx> list = new List<QH_TodaySettlementPriceInfoEx>();
            if (gridControl1.DataSource != null)
            {

                list = gridControl1.DataSource as List<QH_TodaySettlementPriceInfoEx>;
            }

            //验证必须输入所有结算价
            foreach (QH_TodaySettlementPriceInfoEx item in list)
            {
                if (item.SettlementPrice == 0)
                {
                    MessageBox.Show("请输入结算价！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            btnReckoning.Enabled = false;
            lblMessage.Text = "清算处理已经提交，请稍候再进行查询。";
            lblMessage.Refresh();
            bool rtn = qh_TodaySettlementPriceBLL.DoManualReckoning(list, out errMsg);
            if (rtn)
            {
                //System.Threading.Thread.Sleep(3000);
                //ShowSettlementPriceList();
                //提交后清空列表
                gridControl1.DataSource = null;
            }
            else
            {
                MessageBox.Show("手动清算失败！\r\n" + errMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //清算失败再查询一次，按钮在内部会有所处理
                ShowSettlementPriceList();
                //btnReckoning.Enabled = true;
            }
        }

        /// <summary>
        /// 显示待录入结算价的列表
        /// </summary>
        private void ShowSettlementPriceList()
        {
            string errMsg;
            List<QH_TodaySettlementPriceInfoEx> list;
            list = qh_TodaySettlementPriceBLL.GetReckoningHoldCodeList(out errMsg);

            gridControl1.DataSource = list;

            //当记录数为零时也必须清算，否则无法更新清算日期 modify by 董鹏 2010-04-27
            //if (list != null && list.Count != 0)
            if (list != null)
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    lblMessage.Text = "尊敬的用户，以下期货清算失败，系统未读取到部分代码期货结算价，请管理员手动输入正确值！";
                }
                else
                {
                    lblMessage.Text = errMsg;
                }
                if (qh_TodaySettlementPriceBLL.IsReckoningDone(DateTime.Now.AddDays(-1)))
                {
                    btnReckoning.Enabled = false;
                }
                else
                {
                    btnReckoning.Enabled = true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    lblMessage.Text = "上一交易日盘后清算已全部成功！";
                }
                else
                {
                    lblMessage.Text = errMsg;
                }
                btnReckoning.Enabled = false;
            }


        }

        /// <summary>
        /// 查询结算价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            simpleButton1.Enabled = false;
            lblMessage.Text = "正在查询数据，请稍候……";
            lblMessage.Refresh();
            try
            {
                ShowSettlementPriceList();
            }
            catch
            {
            }
            finally
            {
                simpleButton1.Enabled = true;
            }

        }

    }
}