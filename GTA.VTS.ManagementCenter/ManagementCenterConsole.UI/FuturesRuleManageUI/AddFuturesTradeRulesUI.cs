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
using Types = GTA.VTS.Common.CommonObject.Types;

namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    /// <summary>
    /// 描述：添加期货交易规则 错误编码范围:5800-5819
    /// 作者：刘书伟
    /// 日期：2008-12-08  修改：2009-7-25
    /// Desc: 增加了交割月涨跌幅字段相关处理
    /// Update By: 董鹏
    /// Update Date: 2010-01-21
    /// </summary>
    public partial class AddFuturesTradeRulesUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AddFuturesTradeRulesUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性

        #region 操作类型　 1:添加,2:修改

        private int m_EditType = (int)UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return this.m_EditType; }
            set { this.m_EditType = value; }
        }

        #endregion

        #region 期货交易规则实体

        /// <summary>
        /// 期货交易规则实体
        /// </summary>
        private QH_FuturesTradeRules m_QH_FuturesTradeRules = null;

        /// <summary>
        /// 期货交易规则属性
        /// </summary>
        public QH_FuturesTradeRules QHFuturesTradeRules
        {
            get { return this.m_QH_FuturesTradeRules; }
            set
            {
                this.m_QH_FuturesTradeRules = new QH_FuturesTradeRules();
                this.m_QH_FuturesTradeRules = value;
            }
        }

        #endregion

        /// <summary>
        /// 品种标识
        /// </summary>
        private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 最后交易日ID
        /// </summary>
        private int m_LastTradingDayID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 交易规则委托量ID
        /// </summary>
        private int m_ConsignQuantumID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

        /// <summary>
        /// 存放更新交易规则委托量时的交易规则委托量ID
        /// </summary>
        private int m_UConsignQuantumID = AppGlobalVariable.INIT_INT;

        ///// <summary>
        ///// 月份ID（用来获取交割月份窗体中添加的某个月份的ID）
        ///// </summary>
        //private int m_getMonthID = AppGlobalVariable.INIT_INT;

        ///// <summary>
        ///// 月份ID
        ///// </summary>
        //public int MonthID
        //{
        //    set
        //    {
        //        m_MonthID = value;
        //    }
        //    get
        //    {
        //        return m_MonthID;
        //    }
        //}
        /// <summary>
        /// 区别期货类型（商品期货m_DiffFuturesType=false，股指期货:m_DiffFuturesType=true）
        /// </summary>
        private bool m_DiffFuturesType;

        #endregion

        //================================  私有  方法 ================================

        #region 获取现货品种名称 GetBindBreedClassName

        /// <summary>
        /// 获取现货品种名称
        /// </summary>
        private void GetBindBreedClassName()
        {
            DataSet ds = CommonParameterSetCommon.GetQHFutureCostsBreedClassName(); //从交易商品品种表中获取
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
            //?组合框的值清空
            //this.cmbBreedClassID.Text = string.Empty;    //品种标识
            //this.cmbIsSlew.Text = string.Empty;           //是否充许回转
            //this.cmbHighLowStopScopeID.Text = string.Empty;    //涨跌停板幅度类型标识
            //this.cmbIfContainCNewYear.Text = string.Empty;     //合约交割月是否包含春节月份
            //this.cmbMarketUnitID.Text = string.Empty;    //行情成交量单位
            //this.cmbPriceUnit.Text = string.Empty;   //计价单位
            //this.cmbUnitsID.Text = string.Empty;   //交易单位
            this.txtFutruesCode.Text = string.Empty; //期货交易代码
            this.txtHighLowStopScopeValue.Text = string.Empty; //涨跌停板幅度
            this.txtLeastChangePrice.Text = string.Empty; //计价单位最小变动价位
            this.txtNewBreedFPHighLowStopV.Text = string.Empty; //新品种期货合约上市当日涨跌停板幅度
            this.txtNewMonthFPactHighLowStopV.Text = string.Empty; //新月份期货合约上市当日涨跌停板幅度
            this.txtUnitMultiple.Text = string.Empty; //交易单位计价单位倍数
            //this.speAgreementDeliveryIns.Text = string.Empty; //合约交割制度
            //this.speFundDeliveryIns.Text = string.Empty; //资金交割制度
            this.txtDeliveryMonthHighLowStopValue.Text = string.Empty;//交割月涨跌幅
        }

        #endregion

        #region 当前UI是修改期货交易规则UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改期货交易规则UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (m_QH_FuturesTradeRules != null)
                {
                    if (m_QH_FuturesTradeRules.BreedClassID != 0)
                    {
                        foreach (object item in this.cmbBreedClassID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_FuturesTradeRules.BreedClassID)
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
                    if (m_QH_FuturesTradeRules.HighLowStopScopeID != 0)
                    {
                        foreach (object item in this.cmbHighLowStopScopeID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex ==
                                m_QH_FuturesTradeRules.HighLowStopScopeID)
                            {
                                this.cmbHighLowStopScopeID.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbHighLowStopScopeID.SelectedIndex = 0;
                    }

                    if (m_QH_FuturesTradeRules.MarketUnitID != 0)
                    {
                        foreach (object item in this.cmbMarketUnitID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_FuturesTradeRules.MarketUnitID)
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

                    if (m_QH_FuturesTradeRules.PriceUnit != 0)
                    {
                        foreach (object item in this.cmbPriceUnit.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_FuturesTradeRules.PriceUnit)
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

                    if (m_QH_FuturesTradeRules.UnitsID != 0)
                    {
                        foreach (object item in this.cmbUnitsID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_FuturesTradeRules.UnitsID)
                            {
                                this.cmbUnitsID.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbUnitsID.SelectedIndex = 0;
                    }

                    //this.speFundDeliveryIns.EditValue = m_QH_FuturesTradeRules.FundDeliveryInstitution;
                    //this.speAgreementDeliveryIns.EditValue = m_QH_FuturesTradeRules.AgreementDeliveryInstitution;
                    this.txtFutruesCode.Text = m_QH_FuturesTradeRules.FutruesCode.ToString();
                    this.txtHighLowStopScopeValue.Text = m_QH_FuturesTradeRules.HighLowStopScopeValue.ToString();
                    this.txtLeastChangePrice.Text = m_QH_FuturesTradeRules.LeastChangePrice.ToString();
                    if (m_QH_FuturesTradeRules.NewBreedFuturesPactHighLowStopValue.ToString() != "0")
                    {
                        this.txtNewBreedFPHighLowStopV.Text =
                            m_QH_FuturesTradeRules.NewBreedFuturesPactHighLowStopValue.ToString();
                    }
                    if (m_QH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue.ToString() != "0")
                    {
                        this.txtNewMonthFPactHighLowStopV.Text =
                            m_QH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue.ToString();
                    }
                    this.txtUnitMultiple.Text = m_QH_FuturesTradeRules.UnitMultiple.ToString();

                    if (!string.IsNullOrEmpty(m_QH_FuturesTradeRules.ConsignQuantumID.ToString()))
                    {
                        m_ConsignQuantumID = Convert.ToInt32(m_QH_FuturesTradeRules.ConsignQuantumID);
                    }
                    else
                    {
                        m_ConsignQuantumID = AppGlobalVariable.INIT_INT;
                    }
                    if (!string.IsNullOrEmpty(m_QH_FuturesTradeRules.LastTradingDayID.ToString()))
                    {
                        m_LastTradingDayID = Convert.ToInt32(m_QH_FuturesTradeRules.LastTradingDayID);
                    }
                    else
                    {
                        m_LastTradingDayID = AppGlobalVariable.INIT_INT;
                    }
                    //  m_ValueType = Convert.ToInt32(m_QH_FuturesTradeRules.ValueTypeMinChangePrice);
                    m_BreedClassID = Convert.ToInt32(m_QH_FuturesTradeRules.BreedClassID);
                    m_UConsignQuantumID = Convert.ToInt32(m_QH_FuturesTradeRules.ConsignQuantumID);
                    this.txtDeliveryMonthHighLowStopValue.Text = m_QH_FuturesTradeRules.DeliveryMonthHighLowStopValue.ToString();
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5807";
                string errMsg = "当前UI是修改期货交易规则UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 绑定期货交易规则初始化数据 InitBindData()

        /// <summary>
        /// 绑定期货交易规则初始化数据
        /// </summary>
        private void InitBindData()
        {
            try
            {
                //绑定期货品种的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindBreedClassName();
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定是否允许回转的是和否
                //this.cmbIsSlew.Properties.Items.Clear();
                //this.cmbIsSlew.Properties.Items.AddRange(BindData.GetBindListYesOrNo());
                //this.cmbIsSlew.SelectedIndex = 0;

                //绑定合约交割月是否包含春节月份
                //this.cmbIfContainCNewYear.Properties.Items.Clear();
                //this.cmbIfContainCNewYear.Properties.Items.AddRange(BindData.GetBindListYesOrNo());
                //this.cmbIfContainCNewYear.SelectedIndex = 0;

                //绑定交易单位
                this.cmbUnitsID.Properties.Items.Clear();
                this.cmbUnitsID.Properties.Items.AddRange(BindData.GetBindListQHAboutUnit());
                this.cmbUnitsID.SelectedIndex = 0;

                //绑定涨跌停板幅度类型标识
                this.cmbHighLowStopScopeID.Properties.Items.Clear();
                this.cmbHighLowStopScopeID.Properties.Items.AddRange(BindData.GetBindListQHHighLowStopType());
                this.cmbHighLowStopScopeID.SelectedIndex = 0;

                //绑定计价单位
                this.cmbPriceUnit.Properties.Items.Clear();
                this.cmbPriceUnit.Properties.Items.AddRange(BindData.GetBindListQHPriceUnit());
                this.cmbPriceUnit.SelectedIndex = 0;

                //绑定行情成交量单位
                this.cmbMarketUnitID.Properties.Items.Clear();
                this.cmbMarketUnitID.Properties.Items.AddRange(BindData.GetBindListQHAboutUnit());
                this.cmbMarketUnitID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5801";
                string errMsg = "绑定期货交易规则初始化数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 添加或修改期货交易规则UI的AddFuturesTradeRulesUI_Load

        /// <summary>
        /// 添加或修改期货交易规则UI的AddFuturesTradeRulesUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFuturesTradeRulesUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_QH_FuturesTradeRules = new QH_FuturesTradeRules();
                    this.btnAgreementDeliveryMonth.Enabled = false;
                }
                this.InitBindData();

                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    this.btnAgreementDeliveryMonth.Text = "合约交割月份";
                    this.btnConsignQuantum.Text = "交易规则委托量";
                    this.btnLastTradingDay.Text = "最后交易日";
                    this.Text = "期货交易规则";
                    this.cmbBreedClassID.Enabled = false;
                }
                BindFuturesHighLowStopVText();

            }
            catch (Exception ex)
            {
                string errCode = "GL-5800";
                string errMsg = "添加或修改期货交易规则窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region  添加或修改期货交易规则

        /// <summary>
        /// 添加或修改期货交易规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    if (
                        FuturesManageCommon.ExistsFuturesTradeRules(
                            ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                    {
                        ShowMessageBox.ShowInformation("此品种的交易规则已存在!");
                        return;
                    }
                }

                QH_FuturesTradeRules qH_FuturesTradeRules = new QH_FuturesTradeRules();
                if (QHFuturesTradeRules != null)
                    ManagementCenter.Model.CommonClass.UtilityClass.CopyEntityToEntity(QHFuturesTradeRules,
                                                                                       qH_FuturesTradeRules);

                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    qH_FuturesTradeRules.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    qH_FuturesTradeRules.BreedClassID = AppGlobalVariable.INIT_INT;
                }
                qH_FuturesTradeRules.FundDeliveryInstitution = 0; //根据2010。04。26需求界面上不显示；默认T+0//Convert.ToInt32(this.speFundDeliveryIns.EditValue);
                qH_FuturesTradeRules.AgreementDeliveryInstitution = 0;//根据2010。04。26需求界面上不显示；默认T+0
                   // Convert.ToInt32(this.speAgreementDeliveryIns.EditValue);

                if (!string.IsNullOrEmpty(this.cmbHighLowStopScopeID.Text))
                {
                    qH_FuturesTradeRules.HighLowStopScopeID =
                        ((UComboItem)this.cmbHighLowStopScopeID.SelectedItem).ValueIndex;
                }
                else
                {
                    qH_FuturesTradeRules.HighLowStopScopeID = AppGlobalVariable.INIT_INT;
                }
                if (!string.IsNullOrEmpty(this.cmbMarketUnitID.Text))
                {
                    qH_FuturesTradeRules.MarketUnitID =
                        ((UComboItem)this.cmbMarketUnitID.SelectedItem).ValueIndex;
                }
                else
                {
                    qH_FuturesTradeRules.MarketUnitID = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(this.cmbPriceUnit.Text))
                {
                    qH_FuturesTradeRules.PriceUnit =
                        ((UComboItem)this.cmbPriceUnit.SelectedItem).ValueIndex;
                }
                else
                {
                    qH_FuturesTradeRules.PriceUnit = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(this.cmbUnitsID.Text))
                {
                    qH_FuturesTradeRules.UnitsID =
                        ((UComboItem)this.cmbUnitsID.SelectedItem).ValueIndex;
                }
                else
                {
                    qH_FuturesTradeRules.UnitsID = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(this.txtFutruesCode.Text))
                {
                    if ((InputTest.FetureTradeCodeTest(this.txtFutruesCode.Text)) && this.txtFutruesCode.Text.Length <= 2)
                    {
                        qH_FuturesTradeRules.FutruesCode = this.txtFutruesCode.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("代码简称需要大于或小于2位的大写字母!");
                        return;
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请填写代码简称!");
                    return;
                }

                if (!string.IsNullOrEmpty(this.txtUnitMultiple.Text))
                {
                    if (InputTest.DecimalTest(this.txtUnitMultiple.Text))
                    {
                        qH_FuturesTradeRules.UnitMultiple = Convert.ToDecimal(this.txtUnitMultiple.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    //qH_FuturesTradeRules.UnitMultiple = AppGlobalVariable.INIT_DECIMAL;
                    ShowMessageBox.ShowInformation("转换比例不能为空!");
                    return;
                }
                if (!string.IsNullOrEmpty(this.txtLeastChangePrice.Text))
                {
                    if (InputTest.DecimalTest(this.txtLeastChangePrice.Text))
                    {
                        qH_FuturesTradeRules.LeastChangePrice = Convert.ToDecimal(this.txtLeastChangePrice.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("最小变动价位不能为空!");
                    return;
                }
                if (!string.IsNullOrEmpty(this.txtHighLowStopScopeValue.Text))
                {
                    if (InputTest.DecimalTest(this.txtHighLowStopScopeValue.Text))
                    {
                        qH_FuturesTradeRules.HighLowStopScopeValue = Convert.ToDecimal(this.txtHighLowStopScopeValue.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    //qH_FuturesTradeRules.HighLowStopScopeValue = AppGlobalVariable.INIT_DECIMAL;
                    ShowMessageBox.ShowInformation("涨跌幅不能为空!");
                    return;
                }
                if (!string.IsNullOrEmpty(this.txtNewBreedFPHighLowStopV.Text))
                {
                    //qH_FuturesTradeRules.NewBreedFuturesPactHighLowStopValue =
                    //    Convert.ToDecimal(this.txtNewBreedFPHighLowStopV.Text);
                    if (InputTest.DecimalTest(this.txtNewBreedFPHighLowStopV.Text))
                    {
                        qH_FuturesTradeRules.NewBreedFuturesPactHighLowStopValue = Convert.ToDecimal(this.txtNewBreedFPHighLowStopV.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    //qH_FuturesTradeRules.NewBreedFuturesPactHighLowStopValue = AppGlobalVariable.INIT_DECIMAL;
                    if (m_DiffFuturesType)
                    {
                        ShowMessageBox.ShowInformation("季月合约上市首日涨跌幅不能为空!");
                        return;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("新品种合约上市当日涨跌幅不能为空!");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(this.txtNewMonthFPactHighLowStopV.Text))
                {
                    //qH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue =
                    //    Convert.ToDecimal(this.txtNewMonthFPactHighLowStopV.Text);
                    if (InputTest.DecimalTest(this.txtNewMonthFPactHighLowStopV.Text))
                    {
                        qH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue = Convert.ToDecimal(this.txtNewMonthFPactHighLowStopV.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    //qH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue = AppGlobalVariable.INIT_DECIMAL;
                    if (m_DiffFuturesType)
                    {
                        ShowMessageBox.ShowInformation("合约最后交易日涨跌幅不能为空!");
                        return;

                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("新月份合约上市当日涨跌幅不能为空!");
                        return;

                    }
                }
                if (!string.IsNullOrEmpty(this.txtNewMonthFPactHighLowStopV.Text))
                {
                    //qH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue =
                    //    Convert.ToDecimal(this.txtNewMonthFPactHighLowStopV.Text);
                    if (InputTest.DecimalTest(this.txtNewMonthFPactHighLowStopV.Text))
                    {
                        qH_FuturesTradeRules.NewMonthFuturesPactHighLowStopValue = Convert.ToDecimal(this.txtNewMonthFPactHighLowStopV.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                else
                {
                    if (!m_DiffFuturesType)
                    {

                        ShowMessageBox.ShowInformation("合约最后交易日涨跌幅不能为空!");
                        return;
                    }
                }

                //判断交易规则委托量ID或最后交易日ID为空时提示添加交易规则委托量和最后交易日

                if (m_ConsignQuantumID != AppGlobalVariable.INIT_INT)
                {
                    qH_FuturesTradeRules.ConsignQuantumID = m_ConsignQuantumID;
                }
                else
                {
                    if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                    {
                        ShowMessageBox.ShowInformation("请添加交易规则委托量!");
                    }
                    return;
                }
                if (m_LastTradingDayID != AppGlobalVariable.INIT_INT)
                {
                    qH_FuturesTradeRules.LastTradingDayID = m_LastTradingDayID;
                }
                else
                {
                    if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                    {
                        ShowMessageBox.ShowInformation("请添加最后交易日!");
                    }
                    return;
                }
                #region 交割月涨跌幅 add by 董鹏 2010-01-21
                if (!string.IsNullOrEmpty(this.txtDeliveryMonthHighLowStopValue.Text))
                {
                    if (InputTest.DecimalTest(this.txtDeliveryMonthHighLowStopValue.Text))
                    {
                        qH_FuturesTradeRules.DeliveryMonthHighLowStopValue = Convert.ToDecimal(this.txtDeliveryMonthHighLowStopValue.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        return;
                    }
                }
                #endregion
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_Result = FuturesManageCommon.AddFuturesTradeRules(qH_FuturesTradeRules);
                    if (m_Result)
                    {
                        m_BreedClassID = Convert.ToInt32(qH_FuturesTradeRules.BreedClassID);
                        ShowMessageBox.ShowInformation("添加成功!");
                        this.btnAgreementDeliveryMonth.Enabled = true;
                        ShowMessageBox.ShowInformation("请继续添加合约交割月份!");
                        this.ClearAll();
                        AddOrUpdateUIAgreementDeliveryMonth();
                        //if(m_MonthID!=AppGlobalVariable.INIT_INT)
                        //{
                        //    this.DialogResult = DialogResult.OK;
                        //    this.Close();
                        //}

                    }
                    else
                    {
                        // bool testResult = false;
                        if (m_ConsignQuantumID != AppGlobalVariable.INIT_INT)
                        {
                            FuturesManageCommon.DeleteQHConsignQuantumAndSingle(m_ConsignQuantumID);
                        }
                        if (m_LastTradingDayID != AppGlobalVariable.INIT_INT)
                        {
                            FuturesManageCommon.DeleteQHLastTradingDay(m_LastTradingDayID);
                        }
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                else if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    m_Result = FuturesManageCommon.UpdateFuturesTradeRules(qH_FuturesTradeRules);
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
                string errCode = "GL-5802";
                string errMsg = "添加或修改期货交易规则失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 关闭添加或修改期货交易规则UI

        /// <summary>
        /// 关闭添加或修改期货交易规则UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return;
            }
        }

        #endregion

        #region 显示添加或修改期货最小和最大委托量UI

        /// <summary>
        /// 显示添加或修改修改期货最小和最大委托量UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsignQuantum_Click(object sender, EventArgs e)
        {
            try
            {
                AddMinAndMaxConsignQuantumUI addMinAndMaxConsignQuantumUI = new AddMinAndMaxConsignQuantumUI();
                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    addMinAndMaxConsignQuantumUI.MinAndMaxConsignQuantumUIEditType = (int)UITypes.EditTypeEnum.UpdateUI;
                    if (m_UConsignQuantumID != AppGlobalVariable.INIT_INT)
                    {
                        addMinAndMaxConsignQuantumUI.UpdateConsignQuantumID = m_UConsignQuantumID;
                    }
                }
                addMinAndMaxConsignQuantumUI.ShowDialog();
                //注意添加时的处理
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_ConsignQuantumID = addMinAndMaxConsignQuantumUI.ConsignQuantumID;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5803";
                string errMsg = "显示添加或修改修改期货最小和最大委托量UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 显示添加或修改期货最后交易日UI

        /// <summary>
        /// 显示添加或修改期货最后交易日UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLastTradingDay_Click(object sender, EventArgs e)
        {
            try
            {
                AddLastTradingDayUI addLastTradingDayUI = new AddLastTradingDayUI();
                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    addLastTradingDayUI.LastTradingDayUIEditType = (int)UITypes.EditTypeEnum.UpdateUI;
                    //if (m_UConsignQuantumID != AppGlobalVariable.INIT_INT)
                    if (m_LastTradingDayID != AppGlobalVariable.INIT_INT)
                    {
                        addLastTradingDayUI.UpdateLastTradingDayID = m_LastTradingDayID;
                    }
                }
                addLastTradingDayUI.ShowDialog();
                //注意添加时的处理
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_LastTradingDayID = addLastTradingDayUI.LastTradingDayID;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5804";
                string errMsg = "显示添加或修改期货最后交易日UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 显示添加或修改合约交割月份UI

        /// <summary>
        /// 显示添加或修改合约交割月份UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgreementDeliveryMonth_Click(object sender, EventArgs e)
        {
            try
            {
                AddOrUpdateUIAgreementDeliveryMonth();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5805";
                string errMsg = "显示添加或修改合约交割月份UI失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #region 添加或修改合约交割月份UI
        /// <summary>
        /// 添加或修改合约交割月份UI
        /// </summary>
        private void AddOrUpdateUIAgreementDeliveryMonth()
        {
            AgreementDeliMonthManageUI agreementDeliMonthManageUI = new AgreementDeliMonthManageUI();
            if (m_BreedClassID != AppGlobalVariable.INIT_INT)
            {
                if (
                    FuturesManageCommon.ExistsFuturesTradeRules(
                        ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex))
                {
                    agreementDeliMonthManageUI.m_BreedClassID = m_BreedClassID;
                    agreementDeliMonthManageUI.ShowDialog();
                }
                else
                {
                    ShowMessageBox.ShowInformation("请点击确定按钮!");
                }
            }
        }
        #endregion

        #endregion

        #region 期货涨跌幅类型组合框改变事件

        /// <summary>
        /// 期货涨跌幅类型组合框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHighLowStopScopeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cmbHighLowStopScopeID.SelectedIndex == 0)
                {
                    this.cmbHighLowStopScopeID.ToolTip = "不超过上一\r\n" + "交易日结算价";
                    this.labHighLowTypeScale.Text = "%";
                }
                else if (this.cmbHighLowStopScopeID.SelectedIndex == 1)
                {
                    this.cmbHighLowStopScopeID.ToolTip = "每吨不高于\r\n" + "或低于上一\r\n" + "交易日结算价格";
                    this.labHighLowTypeScale.Text = "元";
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5806";
                string errMsg = "期货涨跌幅类型组合框改变事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 品种名称组合框索引值改变事件
        /// <summary>
        /// 品种名称组合框索引值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbBreedClassID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindFuturesHighLowStopVText();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5807";
                string errMsg = "品种名称组合框索引值改变事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #region  根据期货类型（股指期货，商品期货）绑定期货涨跌幅文字
        /// <summary>
        /// 根据期货类型（股指期货，商品期货）绑定期货涨跌幅文字
        /// </summary>
        private void BindFuturesHighLowStopVText()
        {
            try
            {
                int breedClassID = AppGlobalVariable.INIT_INT;//品种ID
                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    breedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;

                    CM_BreedClass cM_BreedClass = new CM_BreedClass();
                    cM_BreedClass = CommonParameterSetCommon.GetBreedClassByBClassID(breedClassID);
                    if (cM_BreedClass != null)
                    {
                        int breedClassTypeID = Convert.ToInt32(cM_BreedClass.BreedClassTypeID);//品种类型ID
                        if (breedClassTypeID == (int)Types.BreedClassTypeEnum.CommodityFuture)
                        {
                            m_DiffFuturesType = false;
                            this.labNewBreedFPHighLowStopV.Text = "新品种合约上市" + "\n" + "当日涨跌幅:";
                            this.labNewMonthFPactHighLowStopV.Text = "新月份合约上市" + "\n" + "当日涨跌幅:";
                            this.labNewBreedFPHighLowStopV.Visible = true;
                            this.labNewMonthFPactHighLowStopV.Visible = true;
                            this.txtNewBreedFPHighLowStopV.Visible = true;
                            this.txtNewMonthFPactHighLowStopV.Visible = true;
                            this.labNewBreedB.Visible = true;
                            this.labNewMonthB.Visible = true;
                            labNewBreedFPHighL.Visible = true;
                            labNewMonthFPactH.Visible = true;
                        }
                        else if (breedClassTypeID == (int)Types.BreedClassTypeEnum.StockIndexFuture)
                        {
                            m_DiffFuturesType = true;
                            this.labNewBreedFPHighLowStopV.Text = "季月合约上市首" + "\n" + "日涨跌幅:";
                            this.labNewMonthFPactHighLowStopV.Text = "合约最后交易日" + "\n" + "涨跌幅:";
                            this.labNewBreedFPHighLowStopV.Visible = true;
                            this.labNewMonthFPactHighLowStopV.Visible = true;
                            this.txtNewBreedFPHighLowStopV.Visible = true;
                            this.txtNewMonthFPactHighLowStopV.Visible = true;
                            this.labNewBreedB.Visible = true;
                            this.labNewMonthB.Visible = true;
                            labNewBreedFPHighL.Visible = true;
                            labNewMonthFPactH.Visible = true;
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        #endregion

        #endregion
    }
}