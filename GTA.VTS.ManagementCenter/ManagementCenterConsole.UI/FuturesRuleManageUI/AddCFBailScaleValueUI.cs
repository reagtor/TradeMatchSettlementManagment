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

namespace ManagementCenterConsole.UI.FuturesRuleManageUI
{
    /// <summary>
    /// 描述：添加商品期货_保证金窗体 错误编码范围:6400-6419
    /// 作者：刘书伟
    /// 日期：2008-12-06
    /// Desc.:添加了保证金比例设置类型相关控件和代码
    /// Update by：董鹏
    /// Update date:2010-01-20
    /// 描述：添加范围值判断
    /// 修改作者：刘书伟
    /// 修改日期：2010-05-12
    /// </summary>
    public partial class AddCFBailScaleValueUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AddCFBailScaleValueUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性

        //结果变量
        private bool m_Result = false;

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

        #region 商品期货_保证金比例实体

        /// <summary>
        /// 商品期货_保证金比例实体
        /// </summary>
        private QH_CFBailScaleValue m_QH_CFBailScaleValue = null;

        /// <summary>
        /// 商品期货_保证金比例属性
        /// </summary>
        public QH_CFBailScaleValue QHCFBailScaleValue
        {
            get { return this.m_QH_CFBailScaleValue; }
            set
            {
                this.m_QH_CFBailScaleValue = new QH_CFBailScaleValue();
                this.m_QH_CFBailScaleValue = value;
            }
        }

        #endregion

        /// <summary>
        /// 商品期货-保证金比例标识ID
        /// </summary>
        private int m_CFBailScaleValueID = AppGlobalVariable.INIT_INT;
        /// <summary>
        /// 商品期货-保证金比例标识ID2
        /// </summary>
        private int m_CFBailScaleValueID2 = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 保证金控制类型
        /// </summary>
        private int m_PositionBailTypeID = 0;
        #endregion

        #region 事件
        /// <summary>
        /// 保存成功
        /// </summary>
        public event EventHandler OnSaved;

        /// <summary>
        /// 触发保存成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FireSaved(object sender, EventArgs e)
        {
            try
            {
                if (this.OnSaved != null)
                    OnSaved(this, e);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
        }
        #endregion

        //================================  私有  方法 ================================

        #region 获取(商品)期货品种类型的品种名称 GetBindSpQhTypeBreedClassName

        /// <summary>
        /// 获取(商品)期货品种类型的品种名称
        /// </summary>
        private void GetBindSpQhTypeBreedClassName()
        {
            try
            {
                DataSet ds = CommonParameterSetCommon.GetSpQhTypeBreedClassName(); //从交易商品品种表中获取
                if (ds != null)
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
                string errCode = "GL-6404";
                string errMsg = "获取(商品)期货品种类型的品种名称失败!";
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
            this.cmbBreedClassID.Text = string.Empty;
            this.cmbDeliveryMonthTypeID.Text = string.Empty;
            this.cmbPositionBailTypeID.Text = string.Empty;
            //this.cmbUpperLimitIfEquation.Text = string.Empty;
            //this.cmbLowerLimitIfEquation.Text = string.Empty;
            this.txtStart.Text = string.Empty;
            this.txtEnds.Text = string.Empty;
            this.txtBailScale.Text = string.Empty;
        }

        #endregion

        #region 当前UI是修改商品期货_保证金比例UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是商品期货_保证金比例UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (m_QH_CFBailScaleValue != null)
                {
                    if (m_QH_CFBailScaleValue.BreedClassID != 0)
                    {
                        foreach (object item in this.cmbBreedClassID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_CFBailScaleValue.BreedClassID)
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
                    if (m_QH_CFBailScaleValue.DeliveryMonthType != 0)
                    {
                        foreach (object item in this.cmbDeliveryMonthTypeID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_CFBailScaleValue.DeliveryMonthType)
                            {
                                this.cmbDeliveryMonthTypeID.SelectedItem = item;
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.cmbDeliveryMonthTypeID.SelectedIndex = 0;
                    }
                    if (m_QH_CFBailScaleValue.LowerLimitIfEquation.Value == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                    if (m_QH_CFBailScaleValue.UpperLimitIfEquation.Value == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                    if (m_QH_CFBailScaleValue.PositionBailTypeID != 0)
                    {
                        foreach (object item in this.cmbPositionBailTypeID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QH_CFBailScaleValue.PositionBailTypeID)
                            {
                                this.cmbPositionBailTypeID.SelectedItem = item;
                                m_PositionBailTypeID = Convert.ToInt32(m_QH_CFBailScaleValue.PositionBailTypeID);
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.cmbPositionBailTypeID.SelectedIndex = 0;
                    }
                    this.txtStart.Text = m_QH_CFBailScaleValue.Start.ToString();
                    this.txtEnds.Text = m_QH_CFBailScaleValue.Ends.ToString();
                    this.txtBailScale.Text = m_QH_CFBailScaleValue.BailScale.ToString();
                    m_CFBailScaleValueID = Convert.ToInt32(m_QH_CFBailScaleValue.CFBailScaleValueID);

                    if (m_QH_CFBailScaleValue.RelationScaleID != null)
                    {
                        BindRelationValues(m_QH_CFBailScaleValue.RelationScaleID.Value);
                    }

                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-6403";
                string errMsg = "当前UI是商品期货_保证金比例UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #region 绑定保证金关系表数据
        /// <summary>
        /// 绑定保证金关系表数据
        /// </summary>
        /// <param name="id"></param>
        private void BindRelationValues(int id)
        {
            QH_CFBailScaleValue model = FuturesManageCommon.GetQHCFBailScaleValueModel(id);
            if (model != null)
            {
                chkLastTrDay.Checked = true;
                //if (model.LowerLimitIfEquation.Value == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)
                //{
                //    //checkBox4.Checked = true;
                //}
                //else
                //{
                //    //checkBox4.Checked = false;
                //}
                if (model.UpperLimitIfEquation.Value == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)
                {
                    checkBox5.Checked = true;
                }
                else
                {
                    checkBox5.Checked = false;
                }
                //if (model.Start != null)
                //{
                //    //txtStart2.Text = model.Start.Value.ToString();
                //}
                if (model.Ends != null)
                {
                    txtEnds2.Text = model.Ends.Value.ToString();
                }
                if (model.BailScale != null)
                {
                    txtBailScale2.Text = model.BailScale.Value.ToString();
                }
                m_CFBailScaleValueID2 = Convert.ToInt32(model.CFBailScaleValueID);
            }
        }
        #endregion

        #endregion

        #region 绑定商品期货_保证金比例初始化数据 InitBindData()

        /// <summary>
        /// 绑定商品期货_保证金比例初始化数据
        /// </summary>
        private void InitBindData()
        {
            try
            {
                //绑定(商品)期货品种类型的品种名称
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetBindSpQhTypeBreedClassName();
                this.cmbBreedClassID.SelectedIndex = 0;

                //绑定交割月份类型
                this.cmbDeliveryMonthTypeID.Properties.Items.Clear();
                this.cmbDeliveryMonthTypeID.Properties.Items.AddRange(BindData.GetBindListQHCFPositionMonthType());
                this.cmbDeliveryMonthTypeID.SelectedIndex = 0;

                //绑定持仓控制类型
                this.cmbPositionBailTypeID.Properties.Items.Clear();
                this.cmbPositionBailTypeID.Properties.Items.AddRange(BindData.GetBindListQHPositionBailType());
                this.cmbPositionBailTypeID.SelectedIndex = 0;

                ////绑定上限是否相等
                //this.cmbUpperLimitIfEquation.Properties.Items.Clear();
                //this.cmbUpperLimitIfEquation.Properties.Items.AddRange(BindData.GetBindListYesOrNo());
                //this.cmbUpperLimitIfEquation.SelectedIndex = 0;

                ////绑定下限是否相等
                //this.cmbLowerLimitIfEquation.Properties.Items.Clear();
                //this.cmbLowerLimitIfEquation.Properties.Items.AddRange(BindData.GetBindListYesOrNo());
                //this.cmbLowerLimitIfEquation.SelectedIndex = 0;

                ////绑定保证金比例设置类型
                ////add by 董鹏 2010-01-20
                //this.cmbScaleSetType.Properties.Items.Clear();
                //this.cmbScaleSetType.Properties.Items.AddRange(BindData.GetBindListQHCFScaleSetType());
                //this.cmbScaleSetType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string errCode = "GL-6401";
                string errMsg = "绑定商品期货_保证金比例初始化数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 添加商品期货_保证金窗体  AddCFBailScaleValueUI_Load

        /// <summary>
        ///添加商品期货_保证金窗体  AddCFBailScaleValueUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCFBailScaleValueUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_QH_CFBailScaleValue = new QH_CFBailScaleValue();
                }
                this.InitBindData();

                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    //this.Text = "修改商品期货保证金";
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6400";
                string errMsg = "添加商品期货_保证金窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加或修改商品期货_保证金比例

        /// <summary>
        /// 添加或修改商品期货_保证金比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                QH_CFBailScaleValue qH_CFBailScaleValue = new QH_CFBailScaleValue();
                QH_CFBailScaleValue qH_CFBailScaleValue2 = new QH_CFBailScaleValue();

                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    qH_CFBailScaleValue.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                    qH_CFBailScaleValue2.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择品种!");
                    this.cmbBreedClassID.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(this.txtStart.Text) && !string.IsNullOrEmpty(this.txtEnds.Text))
                {
                    if (InputTest.intTest(this.txtStart.Text) && InputTest.intTest(this.txtEnds.Text))
                    {
                        if (Convert.ToInt32(this.txtStart.Text) < Convert.ToInt32(this.txtEnds.Text))
                        {
                            qH_CFBailScaleValue.Start = Convert.ToInt32(this.txtStart.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("起始值不能大于或等于结束值!");
                            this.txtStart.Focus();
                            return;

                        }
                        if (Convert.ToInt32(this.txtEnds.Text) > Convert.ToInt32(this.txtStart.Text))
                        {
                            qH_CFBailScaleValue.Ends = Convert.ToInt32(this.txtEnds.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("结束值不能小于或等于起始值!");
                            this.txtEnds.Focus();
                            return;
                        }
                        if (InputTest.intTest(this.txtEnds2.Text))
                        {
                            qH_CFBailScaleValue2.Ends = Convert.ToInt32(this.txtEnds2.Text);
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入数字!");
                        this.txtStart.Focus();
                        this.txtEnds.Focus();
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(this.txtStart.Text) && string.IsNullOrEmpty(this.txtEnds.Text))
                    {
                        ShowMessageBox.ShowInformation("范围值不能为空!");
                        this.txtStart.Focus();
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.txtStart.Text))
                    {
                        if (InputTest.intTest(this.txtStart.Text))
                        {
                            qH_CFBailScaleValue.Start = Convert.ToInt32(this.txtStart.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入正整数字!");
                            this.txtStart.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(this.txtEnds.Text))
                    {
                        if (InputTest.intTest(this.txtEnds.Text))
                        {
                            qH_CFBailScaleValue.Ends = Convert.ToInt32(this.txtEnds.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入正整数字!");
                            this.txtEnds.Focus();
                            return;
                        }
                        if (InputTest.intTest(this.txtEnds2.Text))
                        {
                            qH_CFBailScaleValue2.Ends = Convert.ToInt32(this.txtEnds2.Text);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.txtBailScale.Text))
                {
                    if (InputTest.DecimalTest(this.txtBailScale.Text))
                    {
                        qH_CFBailScaleValue.BailScale = Convert.ToDecimal(this.txtBailScale.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入数字!");
                        this.txtBailScale.Focus();
                        return;
                    }
                    if (InputTest.DecimalTest(this.txtBailScale2.Text))
                    {
                        qH_CFBailScaleValue2.BailScale = Convert.ToDecimal(this.txtBailScale2.Text);
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请输入保证金比例!");
                    this.txtBailScale.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(this.cmbDeliveryMonthTypeID.Text))
                {
                    qH_CFBailScaleValue.DeliveryMonthType = ((UComboItem)this.cmbDeliveryMonthTypeID.SelectedItem).ValueIndex;
                    qH_CFBailScaleValue2.DeliveryMonthType = ((UComboItem)this.cmbDeliveryMonthTypeID.SelectedItem).ValueIndex;
                }
                if (checkBox1.Checked)
                {
                    qH_CFBailScaleValue.LowerLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes;
                }
                else
                {
                    qH_CFBailScaleValue.LowerLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                }
                if (checkBox2.Checked)
                {
                    qH_CFBailScaleValue.UpperLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes;
                }
                else
                {
                    qH_CFBailScaleValue.UpperLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                }
                if (checkBox5.Checked)
                {
                    qH_CFBailScaleValue2.UpperLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes;
                }
                else
                {
                    qH_CFBailScaleValue2.UpperLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                }

                if (!string.IsNullOrEmpty(this.cmbPositionBailTypeID.Text))
                {
                    qH_CFBailScaleValue.PositionBailTypeID = ((UComboItem)this.cmbPositionBailTypeID.SelectedItem).ValueIndex;
                    qH_CFBailScaleValue2.PositionBailTypeID = ((UComboItem)this.cmbPositionBailTypeID.SelectedItem).ValueIndex;
                }
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    int result;
                    if (!chkLastTrDay.Checked)
                    {
                        result = FuturesManageCommon.AddQHCFBailScaleValue(qH_CFBailScaleValue);
                    }
                    else
                    {
                        result = FuturesManageCommon.AddQHCFBailScaleValue(qH_CFBailScaleValue, qH_CFBailScaleValue2);
                    }
                    if (result != AppGlobalVariable.INIT_INT)
                    {
                        FireSaved(this, new EventArgs());
                        ShowMessageBox.ShowInformation("添加成功!");
                        //this.ClearAll();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加失败!");
                    }
                }
                else if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    if (m_CFBailScaleValueID != AppGlobalVariable.INIT_INT)
                    {
                        qH_CFBailScaleValue.CFBailScaleValueID = m_CFBailScaleValueID;
                    }
                    if (m_CFBailScaleValueID2 != AppGlobalVariable.INIT_INT)
                    {
                        qH_CFBailScaleValue2.CFBailScaleValueID = m_CFBailScaleValueID2;
                        qH_CFBailScaleValue.RelationScaleID = m_CFBailScaleValueID2;
                    }
                    if (!chkLastTrDay.Checked)
                    {
                        //当现在修改后的保证金控制类型不等于原来的保证金控制类型：交易日 时，则把原来的子记录删除，并把
                        //父记录中子记录的ID变为NULL
                        if (m_PositionBailTypeID == (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByTradeDays)
                        {
                            if (((UComboItem)this.cmbPositionBailTypeID.SelectedItem).ValueIndex != m_PositionBailTypeID)
                            {
                                FuturesManageCommon.DeleteQHCFBailScaleValue(m_CFBailScaleValueID2);
                                qH_CFBailScaleValue.RelationScaleID = null;//AppGlobalVariable.INIT_INT;
                            }

                        }
                        m_Result = FuturesManageCommon.UpdateQHCFBailScaleValue(qH_CFBailScaleValue);
                    }
                    else
                    {


                        m_Result = FuturesManageCommon.UpdateQHCFBailScaleValue(qH_CFBailScaleValue, qH_CFBailScaleValue2);
                    }
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
                string errCode = "GL-6402";
                string errMsg = "添加或修改商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 取消添加商品期货_保证金窗体

        /// <summary>
        /// 取消添加商品期货_保证金窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch
            {
                return;
            }
        }

        #endregion


        /// <summary>
        /// 保证金比例设置类型改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbScaleSetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControlsStatus();
        }

        /// <summary>
        /// 重新设置控件状态
        /// </summary>
        private void ResetControlsStatus()
        {
            //switch (cmbScaleSetType.SelectedIndex)
            //{
            //    case 0:
            //        cmbBreedClassID.Enabled = true;
            //        cmbDeliveryMonthTypeID.Enabled = false;
            //        cmbPositionBailTypeID.Enabled = false;
            //        cmbUpperLimitIfEquation.Enabled = false;
            //        cmbLowerLimitIfEquation.Enabled = false;
            //        txtStart.Enabled = false;
            //        txtEnds.Enabled = false;
            //        txtBailScale.Enabled = true;
            //        break;
            //    case 1:
            //        cmbBreedClassID.Enabled = true;
            //        cmbDeliveryMonthTypeID.Enabled = true;
            //        cmbPositionBailTypeID.Enabled = true;
            //        cmbUpperLimitIfEquation.Enabled = true;
            //        cmbLowerLimitIfEquation.Enabled = true;
            //        txtStart.Enabled = true;
            //        txtEnds.Enabled = true;
            //        txtBailScale.Enabled = true;
            //        break;
            //    case 2:
            //        cmbBreedClassID.Enabled = true;
            //        cmbDeliveryMonthTypeID.Enabled = true;
            //        cmbPositionBailTypeID.Enabled = true;
            //        cmbUpperLimitIfEquation.Enabled = true;
            //        cmbLowerLimitIfEquation.Enabled = true;
            //        txtStart.Enabled = true;
            //        txtEnds.Enabled = true;
            //        txtBailScale.Enabled = true;
            //        break;
            //}
        }

        #region 保证金控制类型组合框改变事件
        /// <summary>
        /// 保证金控制类型组合框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPositionBailTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((UComboItem)this.cmbPositionBailTypeID.SelectedItem).ValueIndex)
            {
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.SinglePosition:
                    labelControl14.Text = "起始范围:";
                    labelControl2.Text = "结束范围:";
                    this.palLastTrDay.Enabled = false;
                    txtBailScale2.Enabled = false;
                    chkLastTrDay.Checked = false;
                    break;
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.TwoPosition:
                    labelControl14.Text = "起始范围:";
                    labelControl2.Text = "结束范围:";
                    this.palLastTrDay.Enabled = false;
                    txtBailScale2.Enabled = false;
                    chkLastTrDay.Checked = false;
                    break;
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByDays:
                    labelControl14.Text = "起始天:";
                    labelControl2.Text = "结束天:";
                    this.palLastTrDay.Enabled = false;
                    txtBailScale2.Enabled = false;
                    chkLastTrDay.Checked = false;
                    break;
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByTradeDays:
                    labelControl14.Text = "起始天:";
                    labelControl2.Text = "结束天:";
                    this.palLastTrDay.Enabled = true;
                    txtBailScale2.Enabled = true;
                    chkLastTrDay.Checked = true;
                    break;
            }
        }
        #endregion

        #region 最后交易日复选框事件
        /// <summary>
        /// 最后交易日复选框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLastTrDay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLastTrDay.Checked)
            {
                //txtStart2.Enabled = true;
                txtEnds2.Enabled = true;
                txtBailScale2.Enabled = true;
            }
            else
            {
                //txtStart2.Enabled = false;
                txtEnds2.Enabled = false;
                txtBailScale2.Enabled = false;
            }
        }
        #endregion

    }
}