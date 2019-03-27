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
using ManagementCenter.Model.CommonClass;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    /// <summary>
    /// 描述：合约交割月份管理UI 错误编码范围:5870-5889
    /// 作者：刘书伟
    /// 日期：2008-12-08
    /// 修改：叶振东
    /// 日期：2010-01-20
    /// 描述：对添加现货交割月份的错误进行修改
    /// </summary>
    public partial class AgreementDeliMonthManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AgreementDeliMonthManageUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性

        /// <summary>
        /// 品种ID
        /// </summary>
        public int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种ID
        /// </summary>
        public int BreedClassID
        {
            set { m_BreedClassID = value; }
            get { return m_BreedClassID; }
        }

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

        /// <summary>
        /// 月份ID（用来返回添加的某个月份的ID）
        /// </summary>
        private int m_MonthID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 月份ID
        /// </summary>
        public int MonthID
        {
            set
            {
                m_MonthID = value;
            }
            get
            {
                return m_MonthID;
            }
        }
        #endregion

        //================================  私有  方法 ================================

        #region 获取期货品种名称 GetBindBreedClassName

        /// <summary>
        /// 获取期货品种名称
        /// </summary>
        private void GetBindBreedClassName()
        {
            try
            {
                DataSet ds = FuturesManageCommon.GetQHBreedClassNameByBreedClassID(); //从交易商品品种表中获取
                if(ds!=null)
                { 
                    UComboItem _item;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
                                               Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));
                        this.cmbBreedClassID.Properties.Items.Add(_item);
                    }
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5873";
                string errMsg = "获取期货品种名称失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 添加或修改合约交割月份UI AgreementDeliMonthManageUI_Load

        /// <summary>
        /// 添加或修改合约交割月份UI AgreementDeliMonthManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgreementDeliMonthManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //绑定期货交易规则表中的品种ID对应的品种名称
                //this.cmbBreedClassID.Properties.Items.Clear();
                //this.GetBindBreedClassName(); //获取期货交易规则表中的品种ID对应的品种名称
                //this.cmbBreedClassID.SelectedIndex = 0;
                labBreedClassName.Text = SpotManageCommon.GetBreedClassNameByID(m_BreedClassID);
                List<QH_AgreementDeliveryMonth> MonthID = new List<QH_AgreementDeliveryMonth>();
                MonthID = FuturesManageCommon.GetQHAgreementDeliveryMonth(m_BreedClassID);

                foreach (Control c in this.panelControl1.Controls)
                {
                    if (c is DevExpress.XtraEditors.CheckEdit)
                    {
                        DevExpress.XtraEditors.CheckEdit checkEdit = (DevExpress.XtraEditors.CheckEdit)c;

                        int id = int.Parse(c.Name.Substring(9));

                        CheckTag ct = new CheckTag();
                        ct.ID = id;
                        ct.InitIsChecked = false;
                        checkEdit.Checked = false;

                        foreach (QH_AgreementDeliveryMonth i in MonthID)
                        {
                            if (i.MonthID == id)
                            {
                                ct.InitIsChecked = true;
                                checkEdit.Checked = true;
                            }
                        }
                        c.Tag = ct;
                    }
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5870";
                string errMsg = "添加或修改合约交割月份UI加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加或修改合约交割月份

        /// <summary>
        /// 添加或修改合约交割月份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> addID = new List<int>();
                List<int> delID = new List<int>();

                foreach (Control t in this.panelControl1.Controls)
                {
                    if (t is DevExpress.XtraEditors.CheckEdit)
                    {
                        DevExpress.XtraEditors.CheckEdit checkEdit = (DevExpress.XtraEditors.CheckEdit)t;
                        object obj = checkEdit.Tag;
                        if (obj == null) continue;
                        CheckTag ct = (CheckTag)checkEdit.Tag;
                        int BreedClassID = m_BreedClassID;
                       // if (checkEdit.Checked == true && ct.InitIsChecked == false)
                        //对选中的复选框与数据库数据进行比对
                        if (checkEdit.Checked == true)
                        {
                            //执行添加
                            int monthid = ct.ID;
                            QH_AgreementDeliveryMonth QHAgreementDeliveryMonth = FuturesManageCommon.GetQHAgreementDeliveryBreedClassID(BreedClassID,monthid);
                            if (QHAgreementDeliveryMonth ==null)
                            {
                                addID.Add(monthid);
                            }
                        }
                        //if (checkEdit.Checked == false && ct.InitIsChecked == true)
                        //对未选中的复选框与数据库数据进行比对
                       if (checkEdit.Checked == false)
                        {
                            //执行删除
                            int monthid = ct.ID;
                            QH_AgreementDeliveryMonth QHAgreementDeliveryMonth = FuturesManageCommon.GetQHAgreementDeliveryBreedClassID(BreedClassID, monthid);
                            if (QHAgreementDeliveryMonth != null)
                            {
                                delID.Add(monthid);
                            }
                        }
                    }
                }
                //调用添加删除方法
                m_Result = FuturesManageCommon.UpdateQHAgreementDeliveryMonth(addID, delID, m_BreedClassID);
                if (m_Result)
                {
                    if (addID.Count > 0) //根据添加标识ID，显示不同提示信息并返回添加标识ID
                    {
                       ShowMessageBox.ShowInformation("添加成功!");
                       //AddFuturesTradeRulesUI addFuturesTradeRulesUi=new AddFuturesTradeRulesUI();
                       // addFuturesTradeRulesUi.MonthID = addID[0];
                       //m_MonthID = addID[0];
                       this.DialogResult = DialogResult.OK;
                       this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    //添加一种新的交易规则时，自动添加此品种的代码
                    FuturesManageCommon.QHCommdityCodeInit(m_BreedClassID);

                }
                else
                {
                    ShowMessageBox.ShowInformation("添加失败!");
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5871";
                string errMsg = "添加或修改合约交割月份失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #region 本月,下月,随后的两个季度月份复选框的 checkEdit13_CheckedChanged事件

        /// <summary>
        /// 本月,下月,随后的两个季度月份复选框的 checkEdit13_CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit13_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (Control t in this.panelControl1.Controls)
                {
                    if (t is DevExpress.XtraEditors.CheckEdit)
                    {
                        DevExpress.XtraEditors.CheckEdit checkEdit = (DevExpress.XtraEditors.CheckEdit)t;
                        if (checkEdit.Checked && checkEdit.Name == "checkEdit13")
                        {
                            ShowMessageBox.ShowInformation("此月份不能与其它月份同时选择!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5872";
                string errMsg = "本月,下月,随后的两个季度月份复选框的事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #endregion

        #region 标记复选框状态的类

        /// <summary>
        /// 标记复选框状态的类
        /// </summary>
        public class CheckTag
        {
            /// <summary>
            /// 复选框对应的月份ID
            /// </summary>
            public int ID;

            /// <summary>
            /// 复选框的初始状态
            /// </summary>
            public bool InitIsChecked;
        }

        #endregion
    }
}