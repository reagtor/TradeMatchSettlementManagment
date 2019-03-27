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
//using ManagementCenterConsole.UI.SpotAndFutureCommonUI;
using Types=GTA.VTS.Common.CommonObject.Types;

namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    /// <summary>
    /// 描述：添加现货交易规则窗体 错误编码范围:5000-5019
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// </summary>
    public partial class AddSpotTradeRulesUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AddSpotTradeRulesUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性

        //结果变量
        private bool m_Result = false;

        /// <summary>
        /// 品种有效申报标识
        /// </summary>
        private int m_BreedClassValidID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种涨跌幅标识
        /// </summary>
        private int m_BreedClassHighLowID = AppGlobalVariable.INIT_INT;

        #region 操作类型　 1:添加,2:修改

        //private int m_EditType = 1;
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

        #region 现货交易规则实体
        /// <summary>
        /// 现货交易规则实体
        /// </summary>
        private XH_SpotTradeRules m_SpotTradeRules = null;

        /// <summary>
        /// 现货交易规则属性
        /// </summary>
        public XH_SpotTradeRules XHSpotTradeRules
        {
            get { return this.m_SpotTradeRules; }
            set
            {
                this.m_SpotTradeRules = new XH_SpotTradeRules();
                this.m_SpotTradeRules = value;
            }
        }

        #endregion

        /// <summary>
        /// 存放更新涨跌幅时的品种涨跌幅ID
        /// </summary>
        private int m_UBreedClassHighLowID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 存放更新有效申报时的品种有效申报ID
        /// </summary>
        private int m_UBreedClassValidID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种ID(范围值时：现货交易规则_最小变动价位_范围_值和品种_现货_交易费用_成交额_交易手续费需要用)
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        ///取值类型( 最小变动价位和交易费用中的手续费取值类型1:单值2:范围值)
        /// </summary>
        private int m_ValueType = AppGlobalVariable.INIT_INT;

        ///// <summary>
        ///// 涨跌幅类型ID
        ///// </summary>
        //private int m_HighLowTypeID = AppGlobalVariable.INIT_INT;

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

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //this.cmbBreedClassID.Text = string.Empty;
            //this.cmbIsSlew.Text = string.Empty;
            //this.cmbMaxLeaveQuantityUnit.Text = string.Empty;
            //this.cmbPriceUnit.Text = string.Empty;
            //this.cmbMarketUnitID.Text = string.Empty;
            this.txtMinChangePrice.Text = string.Empty;
            this.txtMaxLeaveQuantity.Text = string.Empty;
            //this.txtMinVolumeMultiples.Text = string.Empty;
        }

        #endregion

        #region 当前UI是修改现货交易规则UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改现货交易规则UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (m_SpotTradeRules != null)
                {
                    if (m_SpotTradeRules.BreedClassID != 0)
                    {
                        foreach (object item in this.cmbBreedClassID.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex == m_SpotTradeRules.BreedClassID)
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
                    
                    //if (m_SpotTradeRules.ValueTypeMinChangePrice != 0)
                    //{
                    //    foreach (object item in this.cmbMinChangePriceVType.Properties.Items)
                    //    {
                    //        if (((UComboItem) item).ValueIndex ==
                    //            m_SpotTradeRules.ValueTypeMinChangePrice)
                    //        {
                    //            this.cmbMinChangePriceVType.SelectedItem = item;
                    //            break;
                    //            ;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    this.cmbMinChangePriceVType.SelectedIndex = 0;
                    //}
                    
                    if (m_SpotTradeRules.PriceUnit != 0)
                    {
                        foreach (object item in this.cmbPriceUnit.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex == m_SpotTradeRules.PriceUnit)
                            {
                                this.cmbPriceUnit.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbPriceUnit.SelectedIndex = 0;
                    }

                    if (m_SpotTradeRules.MarketUnitID != 0)
                    {
                        foreach (object item in this.cmbMarketUnitID.Properties.Items)
                        {
                            if (((UComboItem) item).ValueIndex == m_SpotTradeRules.MarketUnitID)
                            {
                                this.cmbMarketUnitID.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbMarketUnitID.SelectedIndex = 0;
                    }
                    this.speFundDeliveryIns.EditValue = m_SpotTradeRules.FundDeliveryInstitution;
                    this.speStockDeliveryIns.EditValue = m_SpotTradeRules.StockDeliveryInstitution;
                    this.txtMinChangePrice.Text = m_SpotTradeRules.MinChangePrice.ToString();
                    this.txtMaxLeaveQuantity.Text = m_SpotTradeRules.MaxLeaveQuantity.ToString();
                    //this.txtMinVolumeMultiples.Text = m_SpotTradeRules.MinVolumeMultiples.ToString();
                    if (!string.IsNullOrEmpty(m_SpotTradeRules.BreedClassHighLowID.ToString()))
                    {
                        m_UBreedClassHighLowID = Convert.ToInt32(m_SpotTradeRules.BreedClassHighLowID);
                    }
                    else
                    {
                        m_UBreedClassHighLowID = AppGlobalVariable.INIT_INT;
                    }
                    if (!string.IsNullOrEmpty(m_SpotTradeRules.BreedClassValidID.ToString()))
                    {
                        m_UBreedClassValidID = Convert.ToInt32(m_SpotTradeRules.BreedClassValidID);
                    }
                    else
                    {
                        m_UBreedClassValidID = AppGlobalVariable.INIT_INT;
                    }
                    m_ValueType = Convert.ToInt32(m_SpotTradeRules.ValueTypeMinChangePrice);
                    //if (m_ValueType == (int)Types.GetValueTypeEnum.Scope)
                    //{
                    //    btnMinChangePriceV.Visible = true;
                    //}
                    m_BreedClassID = Convert.ToInt32(m_SpotTradeRules.BreedClassID);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5002";
                string errMsg = "当前UI是修改现货交易规则UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        #region 绑定现货交易规则初始化数据 InitBindData()

        /// <summary>
        /// 绑定现货交易规则初始化数据
        /// </summary>
        private void InitBindData()
        {
            try
            {
                //绑定交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName(); //获取交易规则表中的品种ID对应的品种名称
                this.cmbBreedClassID.SelectedIndex = 0;
                //绑定最小变动价位取值类型
                //this.cmbMinChangePriceVType.Properties.Items.Clear();
                //this.cmbMinChangePriceVType.Properties.Items.AddRange(BindData.GetBindListValueType());
                //this.cmbMinChangePriceVType.SelectedIndex = 0;

                //绑定计价单位
                this.cmbPriceUnit.Properties.Items.Clear();
                this.cmbPriceUnit.Properties.Items.AddRange(BindData.GetBindListXHAboutUnit());
                this.cmbPriceUnit.SelectedIndex = 0;


                //绑定行情成交量单位
                this.cmbMarketUnitID.Properties.Items.Clear();
                this.cmbMarketUnitID.Properties.Items.AddRange(BindData.GetBindListXHAboutUnit());
                this.cmbMarketUnitID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5003";
                string errMsg = "绑定现货交易规则初始化数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 添加或修改现货交易规则窗体的 AddSpotTradeRulesUI_Load事件

        /// <summary>
        /// 添加或修改现货交易规则窗体的 AddSpotTradeRulesUI_Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSpotTradeRulesUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                {
                    m_SpotTradeRules = new XH_SpotTradeRules();
                    this.cmbBreedClassID.Enabled = true;
                    //this.btnMinChangePriceV.Visible = false;//添加交易规则时最小变动价位范围不显示
                }
                this.InitBindData();

                if (EditType == (int) UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    this.btnAddXHSpotHighLowValue.Text = "涨跌幅";
                    //this.btnAddXHValidDeclareValue.Text = "有效申报";
                    this.Text = "现货交易规则";
                    this.cmbBreedClassID.Enabled = false;
                }
               
                string BreedClassName = SpotManageCommon.GetBreedClassNameByID(m_BreedClassID);

                //if (((int)CommonObject.Types.GetValueTypeEnum.Scope ==
                //     ((UComboItem)this.cmbMinChangePriceVType.SelectedItem).ValueIndex)
                //    && BreedClassName == "香港H股")
                //{
                //    this.txtMinChangePrice.Enabled = false;
                //}
                //else
                //{
                //    this.txtMinChangePrice.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                string errCode = "GL-5000";
                string errMsg = "添加或修改现货交易规则窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 添加或修改现货交易规则  btnOK_Click

        /// <summary>
        /// 添加或修改现货交易规则  btnOK_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                {
                    if (
                        SpotManageCommon.ExistsSpotTradeRules(
                            ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的交易规则已存在!");
                        return;
                    }
                }

                XH_SpotTradeRules xH_SpotTradeRules = new XH_SpotTradeRules();
                if (XHSpotTradeRules != null)
                    ManagementCenter.Model.CommonClass.UtilityClass.CopyEntityToEntity(XHSpotTradeRules,
                                                                                       xH_SpotTradeRules);

                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    xH_SpotTradeRules.BreedClassID = ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    xH_SpotTradeRules.BreedClassID = AppGlobalVariable.INIT_INT;
                }
                xH_SpotTradeRules.FundDeliveryInstitution = Convert.ToInt32(this.speFundDeliveryIns.EditValue);
                xH_SpotTradeRules.StockDeliveryInstitution = Convert.ToInt32(this.speStockDeliveryIns.EditValue);
               
                //if (!string.IsNullOrEmpty(this.cmbMinChangePriceVType.Text))
                //{
                xH_SpotTradeRules.ValueTypeMinChangePrice = (int) GTA.VTS.Common.CommonObject.Types.GetValueTypeEnum.Single;
                //        ((UComboItem) this.cmbMinChangePriceVType.SelectedItem).ValueIndex;
                //}
                //else
                //{
                //    xH_SpotTradeRules.ValueTypeMinChangePrice = AppGlobalVariable.INIT_INT;
                //}
                if (!string.IsNullOrEmpty(this.txtMinChangePrice.Text))
                {
                    if (InputTest.DecimalTest(this.txtMinChangePrice.Text))
                    {
                        xH_SpotTradeRules.MinChangePrice = Convert.ToDecimal(this.txtMinChangePrice.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    //if ((int) CommonObject.Types.GetValueTypeEnum.Single ==
                    //    ((UComboItem) this.cmbMinChangePriceVType.SelectedItem).ValueIndex)
                    //{
                    //    this.txtMinChangePrice.Enabled = true;
                        ShowMessageBox.ShowInformation("最小变动价位不能为空!");
                        return;
                    //}
                    //this.txtMinChangePrice.Enabled = false;
                    //xH_SpotTradeRules.MinChangePrice = 0; //最小变动价位=0时说明是范围值// AppGlobalVariable.INIT_INT; //money类型
                }
                if (!string.IsNullOrEmpty(this.txtMaxLeaveQuantity.Text))
                {
                    if (InputTest.intTest(this.txtMaxLeaveQuantity.Text))
                    {
                        xH_SpotTradeRules.MaxLeaveQuantity = Convert.ToInt32(this.txtMaxLeaveQuantity.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入数字且第一位数不能为0!");
                        return;
                    }
                }
                else
                {
                    //xH_SpotTradeRules.MaxLeaveQuantity = AppGlobalVariable.INIT_INT;
                    ShowMessageBox.ShowInformation("每笔最大委托量不能为空!");
                    return;
                }
                //if (!string.IsNullOrEmpty(this.cmbMaxLeaveQuantityUnit.Text))
                //根据20009.05.15界面修改确认结果 计价单位赋给每笔最大委托量单，每笔最大委托量单位在界面上不显示
                if (!string.IsNullOrEmpty(this.cmbPriceUnit.Text))
                {
                    xH_SpotTradeRules.MaxLeaveQuantityUnit =
                        ((UComboItem)this.cmbPriceUnit.SelectedItem).ValueIndex;
                }
                else
                {
                    xH_SpotTradeRules.MaxLeaveQuantityUnit = AppGlobalVariable.INIT_INT;
                }
                if (!string.IsNullOrEmpty(this.cmbPriceUnit.Text))
                {
                    xH_SpotTradeRules.PriceUnit = ((UComboItem) this.cmbPriceUnit.SelectedItem).ValueIndex;
                }
                else
                {
                    xH_SpotTradeRules.PriceUnit = AppGlobalVariable.INIT_INT;
                }
                if (!string.IsNullOrEmpty(this.cmbMarketUnitID.Text))
                {
                    xH_SpotTradeRules.MarketUnitID = ((UComboItem) this.cmbMarketUnitID.SelectedItem).ValueIndex;
                }
                else
                {
                    xH_SpotTradeRules.MarketUnitID = AppGlobalVariable.INIT_INT;
                }

                //根据20009.05.15界面修改确认结果 最小交易单位倍数在界面上不显示。此值从最小交易单位管理界面中已存在
                xH_SpotTradeRules.MinVolumeMultiples = 0;
                //判断品种涨跌幅ID或品种有效申报ID为空时提示添加品种涨跌幅和品种有效申报

                if (m_BreedClassHighLowID != AppGlobalVariable.INIT_INT && m_BreedClassValidID != AppGlobalVariable.INIT_INT)
                {
                    xH_SpotTradeRules.BreedClassHighLowID = m_BreedClassHighLowID;
                    xH_SpotTradeRules.BreedClassValidID = m_BreedClassValidID;
                }
                else
                {
                    if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                    {
                        ShowMessageBox.ShowInformation("请添加涨跌幅!");
                        return;
                    }
                }
               
                if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                {
                    m_Result = SpotManageCommon.AddXHSpotTradeRules(xH_SpotTradeRules);
                    if (m_Result)
                    {
                        m_ValueType = Convert.ToInt32(xH_SpotTradeRules.ValueTypeMinChangePrice);
                        m_BreedClassID = Convert.ToInt32(xH_SpotTradeRules.BreedClassID);
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.ClearAll();
                    }
                    else
                    {
                        if (m_BreedClassValidID != AppGlobalVariable.INIT_INT)
                        {
                            SpotManageCommon.DeleteValidDeclareValue(m_BreedClassValidID);
                        }
                        if (m_BreedClassHighLowID != AppGlobalVariable.INIT_INT)
                        {
                            SpotManageCommon.DeleteSpotHighLowValue(m_BreedClassHighLowID);
                        }
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                else if (EditType == (int) UITypes.EditTypeEnum.UpdateUI)
                {
                    m_Result = SpotManageCommon.UpdateSpotTradeRules(xH_SpotTradeRules);
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
                string errCode = "GL-5001";
                string errMsg = "添加或修改现货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加现货交易规则的取消事件 btnCancel_Click

        /// <summary>
        /// 添加现货交易规则的取消事件 btnCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region  显示现货涨跌幅界面 btnAddXHSpotHighLowValue_Click

        /// <summary>
        /// 显示现货涨跌幅界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddXHSpotHighLowValue_Click(object sender, EventArgs e)
        {
            try
            {
                AddSpotHighLowValueUI addSpotHighLowValueUI = new AddSpotHighLowValueUI();
                if (EditType == (int) UITypes.EditTypeEnum.UpdateUI)
                {
                    addSpotHighLowValueUI.HighLowUIEditType = (int) UITypes.EditTypeEnum.UpdateUI;
                    if (m_UBreedClassHighLowID != AppGlobalVariable.INIT_INT || m_UBreedClassValidID != AppGlobalVariable.INIT_INT)
                    {
                        addSpotHighLowValueUI.BreedClassHighLowID = m_UBreedClassHighLowID;
                        addSpotHighLowValueUI.BreedClassValidID = m_UBreedClassValidID;
                    }
                }
                addSpotHighLowValueUI.ShowDialog();
                //注意添加时的处理
                if (EditType == (int) UITypes.EditTypeEnum.AddUI)
                {
                    m_BreedClassHighLowID = addSpotHighLowValueUI.BreedClassHighLowID;
                    m_BreedClassValidID = addSpotHighLowValueUI.BreedClassValidID;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5004";
                string errMsg = "显示现货涨跌幅界面失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 品种下拉框改变事件
        /// <summary>
        /// 品种下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBreedClassID_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_BreedClassID = ((UComboItem) this.cmbBreedClassID.SelectedItem).ValueIndex;
        }
        #endregion
    }
}