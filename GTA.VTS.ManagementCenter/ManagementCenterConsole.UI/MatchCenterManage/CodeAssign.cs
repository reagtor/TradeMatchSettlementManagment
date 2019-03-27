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
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 代码到撮合机的分配管理 异常编码 2031-2040
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class CodeAssign : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CodeAssign()
        {
            InitializeComponent();
        }
        #endregion

        #region 撮合机实体

        /// <summary>
        /// 撮合机实体
        /// </summary>
        private RC_MatchMachine matchMachine;

        public RC_MatchMachine MatchMachine
        {
            get { return matchMachine; }
            set { matchMachine = value; }
        }

        #endregion

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CodeAssign_Load(object sender, EventArgs e)
        {
            try
            {
                this.txt_MachineID.Text = matchMachine.MatchMachineID.ToString();
                this.txt_MachineName.Text = matchMachine.MatchMachineName.ToString();
                this.txt_MachineID.Enabled = this.txt_MachineName.Enabled = false;
                ManagementCenter.BLL.RC_TradeCommodityAssignBLL TradeCommodityAssignBLL =
                    new RC_TradeCommodityAssignBLL();
                DataSet ds = TradeCommodityAssignBLL.GetCodeListByMatchMachineID(matchMachine.MatchMachineID);
                InitListBox(ds, 2);
                DataSet dss = TradeCommodityAssignBLL.GetNotAssignCodeByBourseTypeID((int) matchMachine.BourseTypeID);
                InitListBox(dss, 1);
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("代码分配窗体加载失败!");
                string errCode = "GL-2031";
                string errMsg = "代码分配窗体加载失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                throw;
            }
        }

        #endregion

        #region 初始化商品代码列表
        /// <summary>
        /// 初始化商品代码列表
        /// </summary>
        /// <param name="ds">商品代码数据集</param>
        /// <param name="falg">1:没有分配的代码 2:撮合机可以撮合的代码</param>
        private void InitListBox(DataSet ds, int falg)
        {
            CommonClass.UComboItemCode item;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string code = ds.Tables[0].Rows[i]["CommodityCode"].ToString();
                    string name = ds.Tables[0].Rows[i]["CommodityName"].ToString();

                    item = new UComboItemCode(code + " --- " + name, code);
                    if (falg == 1)
                    {
                        item.Start_HasRight = false;
                        item.End_Hasright = false;
                        //代码来源那个表:1是CM_Commodity表;2：是HK_Commodity表
                        int CodeFormSource = Convert.ToInt32(ds.Tables[0].Rows[i]["CodeFormSource"].ToString());
                        item.CodeFormSource = CodeFormSource;

                        this.listCanUse.Items.Add(item);
                    }
                    else
                    {
                        item.Start_HasRight = true;
                        item.End_Hasright = true;
                        this.listReconce.Items.Add(item);
                    }
                }
            }
        }
        #endregion

        #region 添加
        private void btn_Add_Click(object sender, EventArgs e)
        {
            UComboItemCode item;
            List<UComboItemCode> list = new List<UComboItemCode>();
            foreach (int i in listCanUse.SelectedIndices)
            {
                item = (UComboItemCode) listCanUse.GetItem(i);
                list.Add(item);
                item.End_Hasright = true;
                this.listReconce.Items.Add(item);
            }
            foreach (UComboItemCode uc in list)
            {
                listCanUse.Items.Remove(uc);
            }
        }
        #endregion

        #region 添加全部
        private void btn_AddAll_Click(object sender, EventArgs e)
        {
            List<UComboItemCode> list = new List<UComboItemCode>();
            foreach (object obj in listCanUse.Items)
            {
                list.Add((UComboItemCode) obj);
                ((UComboItemCode) obj).End_Hasright = true;
                this.listReconce.Items.Add((UComboItem) obj);
            }
            foreach (UComboItemCode uc in list)
            {
                listCanUse.Items.Remove(uc);
            }
        }
        #endregion

        #region 删除
        private void btn_Del_Click(object sender, EventArgs e)
        {
            UComboItemCode item;
            List<UComboItemCode> list = new List<UComboItemCode>();
            foreach (int i in listReconce.SelectedIndices)
            {
                item = (UComboItemCode) listReconce.GetItem(i);
                list.Add(item);
                item.End_Hasright = false;
                this.listCanUse.Items.Add(item);
            }
            foreach (UComboItemCode uc in list)
            {
                listReconce.Items.Remove(uc);
            }
        }
        #endregion

        #region 删除全部
        private void btn_delAll_Click(object sender, EventArgs e)
        {
            List<UComboItemCode> list = new List<UComboItemCode>();
            foreach (object obj in listReconce.Items)
            {
                list.Add((UComboItemCode) obj);
                ((UComboItemCode) obj).End_Hasright = false;
                this.listCanUse.Items.Add((UComboItem) obj);
            }
            foreach (UComboItemCode uc in list)
            {
                listReconce.Items.Remove(uc);
            }
        }
        #endregion

        #region 保存
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                List<RC_TradeCommodityAssign> ladd = new List<RC_TradeCommodityAssign>();
                List<RC_TradeCommodityAssign> ldel = new List<RC_TradeCommodityAssign>();

                RC_TradeCommodityAssign TradeCommodityAssign;

                foreach (object obj in listReconce.Items)
                {
                    UComboItemCode item = (UComboItemCode) obj;
                    if (item.Start_HasRight == false && item.End_Hasright == true)
                    {
                        TradeCommodityAssign = new RC_TradeCommodityAssign();
                        TradeCommodityAssign.CommodityCode = item.ValueStr;
                        TradeCommodityAssign.CodeFormSource = item.CodeFormSource;
                        ladd.Add(TradeCommodityAssign);
                    }
                }
                foreach (object obj in listCanUse.Items)
                {
                    UComboItemCode item = (UComboItemCode) obj;
                    if (item.Start_HasRight == true && item.End_Hasright == false)
                    {
                        TradeCommodityAssign = new RC_TradeCommodityAssign();
                        TradeCommodityAssign.CommodityCode = item.ValueStr;
                        ldel.Add(TradeCommodityAssign);
                    }
                }

                ManagementCenter.BLL.RC_TradeCommodityAssignBLL TradeCommodityAssignBLL =
                    new RC_TradeCommodityAssignBLL();
                if (TradeCommodityAssignBLL.Update(matchMachine.MatchMachineID, ladd, ldel))
                {
                    ShowMessageBox.ShowInformation("保存成功!");
                    this.Close();
                }
                else
                {
                    ShowMessageBox.ShowInformation("保存失败!");
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("保存失败!");
                string errCode = "GL-2032";
                string errMsg = "保存失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }
        #endregion

        #region 取消
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}