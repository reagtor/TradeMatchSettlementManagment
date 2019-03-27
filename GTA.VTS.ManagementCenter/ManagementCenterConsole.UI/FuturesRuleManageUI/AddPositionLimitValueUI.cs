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
    /// 描述：添加期货持仓限制窗体 错误编码范围:6420-6439
    /// 作者：刘书伟
    /// 日期：2008-12-15
    /// Desc: 持仓限制设置类型
    /// Update By: 董鹏
    /// Update Desc: 2010-01-21
    /// 描述：添加范围值判断
    /// 修改作者：刘书伟
    /// 修改日期：2010-05-12
    /// </summary>
    public partial class AddPositionLimitValueUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AddPositionLimitValueUI()
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

        #region (商品)期货_持仓限制实体

        /// <summary>
        /// (商品)期货_持仓限制实体
        /// </summary>
        private QH_PositionLimitValue m_QHPositionLimitValue = null;

        /// <summary>
        /// (商品)期货_持仓限制属性
        /// </summary>
        public QH_PositionLimitValue QHPositionLimitValue
        {
            get { return this.m_QHPositionLimitValue; }
            set
            {
                this.m_QHPositionLimitValue = new QH_PositionLimitValue();
                this.m_QHPositionLimitValue = value;
            }
        }

        #endregion

        /// <summary>
        /// 期货-持仓限制标识ID
        /// </summary>
        private int m_PositionLimitValueID = AppGlobalVariable.INIT_INT;

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
                string errCode = "GL-6424";
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
            //this.cmbPositionValueTypeID.Text = string.Empty;
            this.txtStart.Text = string.Empty;
            this.txtEnds.Text = string.Empty;
            this.txtPositionValue.Text = string.Empty;
        }

        #endregion

        #region 当前UI是修改商品期货持仓限制UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改商品期货持仓限制UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (m_QHPositionLimitValue != null)
                {
                    //#region 绑定持仓限制类型 add by 董鹏 2010-01-21
                    //if (m_QHPositionLimitValue.PositionLimitType != 0)
                    //{
                    //    foreach (object item in this.cmbPositionLimitType.Properties.Items)
                    //    {
                    //        if (((UComboItem)item).ValueIndex == m_QHPositionLimitValue.PositionLimitType)
                    //        {
                    //            this.cmbPositionLimitType.SelectedItem = item;
                    //            break;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    this.cmbPositionLimitType.SelectedIndex = 0;
                    //}
                    //#endregion
                    if (m_QHPositionLimitValue.BreedClassID != 0)
                    {
                        foreach (object item in this.cmbBreedClassID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QHPositionLimitValue.BreedClassID)
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
                    if (m_QHPositionLimitValue.DeliveryMonthType != 0)
                    {
                        foreach (object item in this.cmbDeliveryMonthTypeID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QHPositionLimitValue.DeliveryMonthType)
                            {
                                this.cmbDeliveryMonthTypeID.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbDeliveryMonthTypeID.SelectedIndex = 0;
                    }
                    if (m_QHPositionLimitValue.LowerLimitIfEquation.Value == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                    if (m_QHPositionLimitValue.UpperLimitIfEquation.Value == (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes)
                    {
                        checkBox3.Checked = true;
                    }
                    else
                    {
                        checkBox3.Checked = false;
                    }
                    if (m_QHPositionLimitValue.PositionBailTypeID != 0)
                    {
                        foreach (object item in this.cmbPositionBailTypeID.Properties.Items)
                        {
                            if (((UComboItem)item).ValueIndex == m_QHPositionLimitValue.PositionBailTypeID)
                            {
                                this.cmbPositionBailTypeID.SelectedItem = item;
                                break;
                                ;
                            }
                        }
                    }
                    else
                    {
                        this.cmbPositionBailTypeID.SelectedIndex = 0;
                    }

                    if (m_QHPositionLimitValue.PositionValueTypeID.Value == (int)GTA.VTS.Common.CommonObject.Types.QHPositionValueType.Positions)
                    {
                        radioButton1.Checked = true;
                    }
                    else
                    {
                        radioButton2.Checked = true;
                    }
                    //if (m_QHPositionLimitValue.PositionValueTypeID != 0)
                    //{
                    //    foreach (object item in this.cmbPositionValueTypeID.Properties.Items)
                    //    {
                    //        if (((UComboItem)item).ValueIndex == m_QHPositionLimitValue.PositionValueTypeID)
                    //        {
                    //            this.cmbPositionValueTypeID.SelectedItem = item;
                    //            break;
                    //            ;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    this.cmbPositionValueTypeID.SelectedIndex = 0;
                    //}
                    this.txtStart.Text = m_QHPositionLimitValue.Start.ToString();
                    this.txtEnds.Text = m_QHPositionLimitValue.Ends.ToString();
                    this.txtPositionValue.Text = m_QHPositionLimitValue.PositionValue.ToString();
                    m_PositionLimitValueID = Convert.ToInt32(m_QHPositionLimitValue.PositionLimitValueID);

                    #region 绑定最小交割单位整数倍验证 add by 董鹏 2010-01-28
                    if (m_QHPositionLimitValue.MinUnitLimit != null)
                    {
                        txtMinUnit.Enabled = true;
                        txtMinUnit.Text = m_QHPositionLimitValue.MinUnitLimit.Value.ToString();
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        txtMinUnit.Enabled = false;
                        txtMinUnit.Text = "";
                        checkBox1.Checked = false;
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-6423";
                string errMsg = "当前UI是修改商品期货持仓限制UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 绑定(商品)期货持仓限制初始化数据 InitBindData()

        /// <summary>
        /// 绑定(商品)期货持仓限制初始化数据
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

                ////绑定持仓取值类型
                //this.cmbPositionValueTypeID.Properties.Items.Clear();
                //this.cmbPositionValueTypeID.Properties.Items.AddRange(BindData.GetBindListQHPositionValueType());
                //this.cmbPositionValueTypeID.SelectedIndex = 0;

                //#region 绑定持仓限制设置类型 add by 董鹏 2010-01-21
                //cmbPositionLimitType.Properties.Items.Clear();
                //this.cmbPositionLimitType.Properties.Items.AddRange(BindData.GetBindListQHPositionLimitType());
                //this.cmbPositionLimitType.SelectedIndex = 0;
                //#endregion
            }
            catch (Exception ex)
            {
                string errCode = "GL-6421";
                string errMsg = "绑定(商品)期货持仓限制初始化数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 添加期货持仓限制窗体 AddPositionLimitValueUI_Load

        /// <summary>
        ///添加期货持仓限制窗体 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPositionLimitValueUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_QHPositionLimitValue = new QH_PositionLimitValue();
                }
                this.InitBindData();

                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    //this.Text = "修改持仓限制";
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6420";
                string errMsg = "添加期货持仓限制窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加或修改(商品)期货持仓限制 btnOK_Click

        /// <summary>
        /// 添加或修改(商品)期货持仓限制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                QH_PositionLimitValue qH_PositionLimitValue = new QH_PositionLimitValue();

                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    qH_PositionLimitValue.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择品种!");
                    this.cmbBreedClassID.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(this.txtStart.Text) && !string.IsNullOrEmpty(this.txtEnds.Text))
                {
                    if (InputTest.zeroStartIntTest(this.txtStart.Text) && InputTest.intTest(this.txtEnds.Text))
                    {
                        if (Convert.ToInt32(this.txtStart.Text) < Convert.ToInt32(this.txtEnds.Text))
                        {
                            qH_PositionLimitValue.Start = Convert.ToInt32(this.txtStart.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("起始值不能大于或等于结束值!");
                            this.txtStart.Focus();
                            return;

                        }
                        if (Convert.ToInt32(this.txtEnds.Text) > Convert.ToInt32(this.txtStart.Text))
                        {
                            qH_PositionLimitValue.Ends = Convert.ToInt32(this.txtEnds.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("结束值不能小于或等于起始值!");
                            this.txtEnds.Focus();
                            return;
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
                        if (InputTest.zeroStartIntTest(this.txtStart.Text))
                        {
                            qH_PositionLimitValue.Start = Convert.ToInt32(this.txtStart.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字!");
                            this.txtStart.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(this.txtEnds.Text))
                    {
                        if (InputTest.intTest(this.txtEnds.Text))
                        {
                            qH_PositionLimitValue.Ends = Convert.ToInt32(this.txtEnds.Text);
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("请输入数字!");
                            this.txtEnds.Focus();
                            return;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this.txtPositionValue.Text))
                {
                    if (InputTest.DecimalTest(this.txtPositionValue.Text))
                    {
                        qH_PositionLimitValue.PositionValue = Convert.ToDecimal(this.txtPositionValue.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入数字!");
                        this.txtPositionValue.Focus();
                        return;
                    }
                }
                else
                {
                    ShowMessageBox.ShowInformation("请输入持仓!");
                    this.txtPositionValue.Focus();
                    return;
                }

                if (!string.IsNullOrEmpty(this.cmbDeliveryMonthTypeID.Text))
                {
                    qH_PositionLimitValue.DeliveryMonthType = ((UComboItem)this.cmbDeliveryMonthTypeID.SelectedItem).ValueIndex;
                }
                else
                {
                    qH_PositionLimitValue.DeliveryMonthType = AppGlobalVariable.INIT_INT;
                }
                if (checkBox2.Checked)
                {
                    qH_PositionLimitValue.LowerLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes;
                }
                else
                {
                    qH_PositionLimitValue.LowerLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                }
                if (checkBox3.Checked)
                {
                    qH_PositionLimitValue.UpperLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.Yes;
                }
                else
                {
                    qH_PositionLimitValue.UpperLimitIfEquation = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;
                }

                if (!string.IsNullOrEmpty(this.cmbPositionBailTypeID.Text))
                {
                    qH_PositionLimitValue.PositionBailTypeID = ((UComboItem)this.cmbPositionBailTypeID.SelectedItem).ValueIndex;
                }
                if (radioButton1.Checked)
                {
                    qH_PositionLimitValue.PositionValueTypeID = (int)GTA.VTS.Common.CommonObject.Types.QHPositionValueType.Positions;
                }
                if (radioButton2.Checked)
                {
                    qH_PositionLimitValue.PositionValueTypeID = (int)GTA.VTS.Common.CommonObject.Types.QHPositionValueType.Scales;
                }
                //if (!string.IsNullOrEmpty(this.cmbPositionValueTypeID.Text))
                //{
                //    qH_PositionLimitValue.PositionValueTypeID = ((UComboItem)this.cmbPositionValueTypeID.SelectedItem).ValueIndex;
                //}
                //else
                //{
                //    qH_PositionLimitValue.PositionValueTypeID = AppGlobalVariable.INIT_INT;
                //}
                //if (!string.IsNullOrEmpty(this.cmbPositionLimitType.Text))
                //{
                //    qH_PositionLimitValue.PositionLimitType = ((UComboItem)this.cmbPositionLimitType.SelectedItem).ValueIndex;
                //}
                if (checkBox1.Checked)
                {
                    int minUnit;
                    int.TryParse(txtMinUnit.Text, out minUnit);
                    if (string.IsNullOrEmpty(txtMinUnit.Text) || minUnit == 0)
                    {
                        ShowMessageBox.ShowInformation("请输入最小交割单位!");
                        this.txtMinUnit.Focus();
                        return;
                    }
                    qH_PositionLimitValue.MinUnitLimit = Convert.ToInt32(txtMinUnit.Text);
                }
                else
                {
                    qH_PositionLimitValue.MinUnitLimit = null;
                }

                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    int result = FuturesManageCommon.AddQHPositionLimitValue(qH_PositionLimitValue);
                    if (result != AppGlobalVariable.INIT_INT)
                    {
                        FireSaved(this, new EventArgs());
                        ShowMessageBox.ShowInformation("添加成功!");
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
                    if (m_PositionLimitValueID != AppGlobalVariable.INIT_INT)
                    {
                        qH_PositionLimitValue.PositionLimitValueID = m_PositionLimitValueID;
                    }
                    m_Result = FuturesManageCommon.UpdateQHPositionLimitValue(qH_PositionLimitValue);
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
                string errCode = "GL-6422";
                string errMsg = "添加或修改(商品)期货持仓限制失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region  取消添加期货持仓限制窗体
        /// <summary>
        /// 取消添加期货持仓限制窗体
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

        #region 验证整数倍复选框改变事件
        /// <summary>
        /// 验证整数倍复选框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtMinUnit.Enabled = true;
            }
            else
            {
                txtMinUnit.Enabled = false;
            }
        }
        #endregion

        #region 持仓类型组合框改变事件
        /// <summary>
        /// 持仓类型组合框改变事件
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
                    break;
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.TwoPosition:
                    labelControl14.Text = "起始范围:";
                    labelControl2.Text = "结束范围:";
                    break;
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByDays:
                    labelControl14.Text = "起始天:";
                    labelControl2.Text = "结束天:";
                    break;
                case (int)GTA.VTS.Common.CommonObject.Types.QHPositionBailType.ByTradeDays:
                    labelControl14.Text = "起始天:";
                    labelControl2.Text = "结束天:";
                    break;
            }
        }
        #endregion
    }
}