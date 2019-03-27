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

//using ManagementCenterConsole.UI.SpotRuleManageUI;

namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    /// <summary>
    /// 描述：添加现货交易费用窗体 错误编码范围:5400-5419
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// </summary>
    public partial class AddSpotCostsUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AddSpotCostsUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性

        #region 操作类型　 1:添加,2:修改

        private int m_EditType = (int) UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return this.m_EditType; }
            set { this.m_EditType = value; }
        }

        #endregion

        #region 现货交易费用实体
        /// <summary>
        /// 现货交易费用实体
        /// </summary>
        private XH_SpotCosts m_XH_SpotCosts = null;

        /// <summary>
        /// 现货交易费用属性
        /// </summary>
        public XH_SpotCosts XHSpotCosts
        {
            get { return this.m_XH_SpotCosts; }
            set
            {
                this.m_XH_SpotCosts = new XH_SpotCosts();
                this.m_XH_SpotCosts = value;
            }
        }

        #endregion

        ///// <summary>
        /////取值类型( 最小变动价位和交易费用中的手续费取值类型1:单值2:范围值)
        ///// </summary>
        //private int m_ValueType = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种ID(范围值时：品种_现货_交易费用_成交额_交易手续费需要用)
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

        #endregion

        //================================  私有  方法 ================================

        #region 获取现货品种名称 GetBindBreedClassName

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        private void GetBindBreedClassName()
        {
            DataSet ds = SpotManageCommon.GetBreedClassName(); //从交易商品品种表中获取
            UComboItem _item;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                _item = new UComboItem(ds.Tables[0].Rows[i]["BreedClassName"].ToString(),
                                       Convert.ToInt32(ds.Tables[0].Rows[i]["BreedClassID"]));
                this.cmbBreedClassID.Properties.Items.Add(_item);
            }
        }

        #endregion

        #region 绑定现货交易费用初始化数据 InitBindData()

        /// <summary>
        /// 绑定现货交易费用初始化数据
        /// </summary>
        private void InitBindData()
        {
            try
            {
                //绑定现货交易费用表中的品种ID对应的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName(); //获取现货交易费用表中的品种ID对应的品种名称
                this.cmbBreedClassID.SelectedIndex = 0;
                //绑定币种类型
                this.cmbCurrencyType.Properties.Items.Clear();
                this.cmbCurrencyType.Properties.Items.AddRange(BindData.GetBindListCurrencyType());
                this.cmbCurrencyType.SelectedIndex = 0;

                //绑定印花税收取方式
                this.cmbStampDutyType.Properties.Items.Clear();
                this.cmbStampDutyType.Properties.Items.AddRange(BindData.GetBindListStampDutyType());
                this.cmbStampDutyType.SelectedIndex = 0;

                //绑定过户费取值类型
                this.cmbTransferTollType.Properties.Items.Clear();
                this.cmbTransferTollType.Properties.Items.AddRange(BindData.GetBindListXHTransferTollType());
                this.cmbTransferTollType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5401";
                string errMsg = "绑定现货交易费用初始化数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //this.cmbBreedClassID.Text = string.Empty; //清空品种ID
            //this.cmbCurrencyType.Text = string.Empty; //清空货币类型
            //this.cmbStampDutyType.Text = string.Empty; //清空印花税收取方式
            //this.cmbTransferTollType.Text = string.Empty; //清空过户费取值类型
            this.txtClearingFees.Text = string.Empty; //清空结算费
            this.txtCommision.Text = string.Empty; //清空佣金
            this.txtCommisionStartpoint.Text = string.Empty; //清空佣金起点
            this.txtStampDuty.Text = string.Empty; //清空印花税
            this.txtStampDutyStartpoint.Text = string.Empty; //清空印花税起点
            this.txtTransferToll.Text = string.Empty; //清空过户费
            this.txtTransferTollStartpoint.Text = string.Empty; //清空过户费起点
        }

        #endregion

        #region 当前UI是修改现货交易费用UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改现货交易费用UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (m_XH_SpotCosts != null)
                {
                    if (m_XH_SpotCosts.BreedClassID != 0)
                    {
                        foreach (object item in this.cmbBreedClassID.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex == m_XH_SpotCosts.BreedClassID)
                            {
                                this.cmbBreedClassID.SelectedItem = item;
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.cmbBreedClassID.SelectedIndex = 0;
                    }
                    if (m_XH_SpotCosts.CurrencyTypeID != 0)
                    {
                        foreach (object item in this.cmbCurrencyType.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex ==
                                m_XH_SpotCosts.CurrencyTypeID)
                            {
                                this.cmbCurrencyType.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbCurrencyType.SelectedIndex = 0;
                    }
                    if (m_XH_SpotCosts.StampDutyTypeID != 0)
                    {
                        foreach (object item in this.cmbStampDutyType.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex ==
                                m_XH_SpotCosts.StampDutyTypeID)
                            {
                                this.cmbStampDutyType.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbStampDutyType.SelectedIndex = 0;
                    }

                    if (m_XH_SpotCosts.TransferTollTypeID != 0)
                    {
                        foreach (object item in this.cmbTransferTollType.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex == m_XH_SpotCosts.TransferTollTypeID)
                            {
                                this.cmbTransferTollType.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbTransferTollType.SelectedIndex = 0;
                    }
                    this.txtStampDuty.Text = m_XH_SpotCosts.StampDuty.ToString();
                    this.txtStampDutyStartpoint.Text = m_XH_SpotCosts.StampDutyStartingpoint.ToString();
                    this.txtTransferToll.Text = m_XH_SpotCosts.TransferToll.ToString();
                    this.txtTransferTollStartpoint.Text = m_XH_SpotCosts.TransferTollStartingpoint.ToString();
                    this.txtCommision.Text = m_XH_SpotCosts.Commision.ToString();
                    this.txtCommisionStartpoint.Text = m_XH_SpotCosts.CommisionStartingpoint.ToString();
                    this.txtClearingFees.Text = m_XH_SpotCosts.ClearingFees.ToString();

                    //m_ValueType = Convert.ToInt32(m_XH_SpotCosts.GetValueTypeID);
                    //if (m_ValueType == (int)Types.GetValueTypeEnum.Scope)
                    //{
                    //    btnMinChangePriceV.Visible = true;
                    //}
                    m_BreedClassID = Convert.ToInt32(m_XH_SpotCosts.BreedClassID);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5403";
                string errMsg = "当前UI是修改现货交易费用UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 现货交易费用窗体 AddSpotCostsUI_Load

        /// <summary>
        /// 现货交易费用窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSpotCostsUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                {
                    m_XH_SpotCosts = new XH_SpotCosts();
                    this.cmbBreedClassID.Enabled = true;
                }
                this.InitBindData();
                if (EditType == (int) UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    //this.btnAddPoundage.Text = "修改手续费";
                    //this.Text = "修改现货交易费用";
                    this.cmbBreedClassID.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5400";
                string errMsg = "现货交易费用窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region  添加或修改现货交易费用

        /// <summary>
        /// 添加或修改现货交易费用
        /// 说明：根据需求，当以下值,分别是(印花税，印花税起点，佣金起点，过户费，过户费起点，结算费)用户没有填写时，默认值是0；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    if (
                        SpotManageCommon.ExistsSpotCosts(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的交易费用已存在!");
                        return;
                    }
                }
                XH_SpotCosts xH_SpotCosts = new XH_SpotCosts();
                if (XHSpotCosts != null)
                    ManagementCenter.Model.CommonClass.UtilityClass.CopyEntityToEntity(XHSpotCosts, xH_SpotCosts);

                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    xH_SpotCosts.BreedClassID = ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    ShowMessageBox.ShowInformation("品种名称不能为空!");
                    return;
                }
                xH_SpotCosts.CurrencyTypeID = ((UComboItem) this.cmbCurrencyType.SelectedItem).ValueIndex;
                xH_SpotCosts.GetValueTypeID = (int) GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Single;
                //根据需求指定默认值 //((UComboItem)this.cmbGetValueType.SelectedItem).ValueIndex;
                xH_SpotCosts.StampDutyTypeID = ((UComboItem) this.cmbStampDutyType.SelectedItem).ValueIndex;
                xH_SpotCosts.TransferTollTypeID = ((UComboItem) this.cmbTransferTollType.SelectedItem).ValueIndex;
                if (!string.IsNullOrEmpty(this.txtClearingFees.Text))
                {
                    if (InputTest.DecimalTest(this.txtClearingFees.Text))
                    {
                        xH_SpotCosts.ClearingFees = Convert.ToDecimal(this.txtClearingFees.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.ClearingFees = 0; // AppGlobalVariable.INIT_DECIMAL;
                }
                if (!string.IsNullOrEmpty(this.txtCommision.Text))
                {
                    if (InputTest.DecimalTest(this.txtCommision.Text))
                    {
                        xH_SpotCosts.Commision = Convert.ToDecimal(this.txtCommision.Text); //null;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.Commision = 0; //？现货所有品种都有拥金 AppGlobalVariable.INIT_DECIMAL;
                }
                if (!string.IsNullOrEmpty(this.txtCommisionStartpoint.Text))
                {
                    if (InputTest.DecimalTest(this.txtCommisionStartpoint.Text))
                    {
                        xH_SpotCosts.CommisionStartingpoint = Convert.ToDecimal(this.txtCommisionStartpoint.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.CommisionStartingpoint = 0; // AppGlobalVariable.INIT_DECIMAL;
                }
                xH_SpotCosts.MonitoringFee = 0; //根据需求指定默认值 //Convert.ToDecimal(this.txtMonitoringFee.Text);
                xH_SpotCosts.PoundageSingleValue = 0; //根据需求改成默认值 //Convert.ToDecimal(this.txtPoundageSingleValue.Text);
                if (!string.IsNullOrEmpty(this.txtStampDuty.Text))
                {
                    if (InputTest.DecimalTest(this.txtStampDuty.Text))
                    {
                        xH_SpotCosts.StampDuty = Convert.ToDecimal(this.txtStampDuty.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.StampDuty = 0; // AppGlobalVariable.INIT_DECIMAL;
                }
                if (!string.IsNullOrEmpty(this.txtStampDutyStartpoint.Text))
                {
                    if (InputTest.DecimalTest(this.txtStampDutyStartpoint.Text))
                    {
                        xH_SpotCosts.StampDutyStartingpoint = Convert.ToDecimal(this.txtStampDutyStartpoint.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.StampDutyStartingpoint = 0; // AppGlobalVariable.INIT_DECIMAL;
                }

                xH_SpotCosts.SystemToll = 0; //根据需求指定默认值// Convert.ToDecimal(this.txtSystemToll.Text);
                if (!string.IsNullOrEmpty(this.txtTransferToll.Text))
                {
                    if (InputTest.DecimalTest(this.txtTransferToll.Text))
                    {
                        xH_SpotCosts.TransferToll = Convert.ToDecimal(this.txtTransferToll.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.TransferToll = 0; // AppGlobalVariable.INIT_DECIMAL;
                }
                if (!string.IsNullOrEmpty(this.txtTransferTollStartpoint.Text))
                {
                    if (InputTest.DecimalTest(this.txtTransferTollStartpoint.Text))
                    {
                        xH_SpotCosts.TransferTollStartingpoint = Convert.ToDecimal(this.txtTransferTollStartpoint.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    xH_SpotCosts.TransferTollStartingpoint = 0; // AppGlobalVariable.INIT_DECIMAL;
                }

                if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                {
                    m_Result = SpotManageCommon.AddSpotCosts(xH_SpotCosts);
                    if (m_Result)
                    {
                        //m_ValueType = Convert.ToInt32(xH_SpotCosts.ValueTypeMinChangePrice);
                        //if (m_ValueType == (int)Types.GetValueTypeEnum.Scope)
                        //{
                        //    btnMinChangePriceV.Visible = true;
                        //}
                        m_BreedClassID = Convert.ToInt32(xH_SpotCosts.BreedClassID);
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                else if (EditType == (int) UITypes.EditTypeEnum.UpdateUI)
                {
                    
                    m_Result = SpotManageCommon.UpdateSpotCosts(xH_SpotCosts);
                    if (m_Result)
                    {
                        ShowMessageBox.ShowInformation("修改成功!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改失败!");
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5402";
                string errMsg = "添加或修改现货交易费用失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 需求变更，暂不需要的代码

        #region 添加现货交易费用手续费范围值

        /// <summary>
        /// 添加现货交易费用手续费范围值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPoundage_Click(object sender, EventArgs e)
        {
            //FieldRangeManageUI fieldRangeManageUI = new FieldRangeManageUI();
            //fieldRangeManageUI.UIType = (int) UITypes.UITypeEnum.XHSpotRangeCostPoundageFieldRangeUI;
            //if (m_BreedClassID != AppGlobalVariable.INIT_INT)
            //{
            //    fieldRangeManageUI.m_BreedClassID = m_BreedClassID;
            //    fieldRangeManageUI.ShowDialog();
            //}
        }

        #endregion

        #endregion

        /// <summary>
        /// 取消按纽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}