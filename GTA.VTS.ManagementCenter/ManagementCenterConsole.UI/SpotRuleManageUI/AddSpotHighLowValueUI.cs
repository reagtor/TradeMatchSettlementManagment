using System;
using System.Drawing;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using DevExpress.XtraEditors;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using ManagementCenter.Model.XH;
using ManagementCenterConsole.UI.CommonClass;
using Types = GTA.VTS.Common.CommonObject.Types;

namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    /// <summary>
    /// 描述：添加现货涨跌幅窗体 错误编码范围:5040-5059
    /// 作者：刘书伟
    /// 日期：2008-12-17
    /// </summary>
    public partial class AddSpotHighLowValueUI : XtraForm
    {
        #region 现货涨跌幅窗体的构造函数

        /// <summary>
        /// 现货涨跌幅窗体的构造函数
        /// </summary>
        public AddSpotHighLowValueUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性
        /// <summary>
        /// 品种涨跌幅ID
        /// </summary>
        private int m_BreedClassHighLowID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种有效申报ID
        /// </summary>
        public int m_BreedClassValidID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种涨跌幅标识(存添加后的值)
        /// </summary>
        private int m_ResultBreedClassHighLowID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种有效申报标识(存添加后的值)
        /// </summary>
        private int m_ResultBreedClassValidID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 品种涨跌幅标识属性(存添加后的值)
        /// </summary>
        public int BreedClassHighLowID
        {
            set { m_ResultBreedClassHighLowID = value; }
            get { return m_ResultBreedClassHighLowID; }
        }

        /// <summary>
        /// 品种有效申报标识属性(存添加后的值)
        /// </summary>
        public int BreedClassValidID
        {
            set { m_ResultBreedClassValidID = value; }
            get { return m_ResultBreedClassValidID; }
        }

        #region 操作类型　 1:添加,2:修改

        private int m_HighLowUIEditType = (int)UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int HighLowUIEditType
        {
            get { return m_HighLowUIEditType; }
            set { m_HighLowUIEditType = value; }
        }

        #endregion

        #region 当是修改涨跌幅UI时，获取品种涨跌幅标识

        /// <summary>
        /// 当是修改涨跌幅UI时，获取品种涨跌幅标识
        /// </summary>
        private int m_UpdateBreedClassHighLowID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 当是修改涨跌幅UI时，获取品种涨跌幅标识
        /// </summary>
        public int UpdateBreedClassHighLowID
        {
            set { m_UpdateBreedClassHighLowID = value; }
            get { return m_UpdateBreedClassHighLowID; }
        }

        #endregion

        #endregion


        //================================  私有  方法 ================================
        #region 绑定初始化数据 InitBindData()

        /// <summary>
        /// 绑定初始化数据
        /// </summary>
        private void InitBindData()
        {
            try
            {
                //绑定现货涨跌幅类型
                cmbHighLowTypeID.Properties.Items.Clear();
                cmbHighLowTypeID.Properties.Items.AddRange(BindData.GetBindListXHHighLowType());
                cmbHighLowTypeID.SelectedIndex = 0;
                //绑定现货有效申报类型
                cmbValidDeclareTypeID.Properties.Items.Clear();
                cmbValidDeclareTypeID.Properties.Items.AddRange(BindData.GetBindListXHValidDeclareType());
                cmbValidDeclareTypeID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5041";
                string errMsg = " 现货涨跌幅UI初始化数据失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            cmbHighLowTypeID.Text = string.Empty;
            txtHighLowValue1.Text = string.Empty;
            //txtFundYClosePriceScale.Text = string.Empty;
            txtHighLowValue2.Text = string.Empty;
            //txtRightHighLowScale.Text = string.Empty;
        }

        #endregion

        #region 当前UI是修改涨跌幅UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改涨跌幅UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (BreedClassHighLowID != AppGlobalVariable.INIT_INT || BreedClassValidID != AppGlobalVariable.INIT_INT)
                {
                    XH_SpotHighLowControlType xH_SpotHighLowControlType =
                        SpotManageCommon.GetModelSpotHighLowControlType(BreedClassHighLowID);
                    //(UpdateBreedClassHighLowID);
                    XH_SpotHighLowValue xH_SpotHighLowValue =
                        SpotManageCommon.GetModelByBCHighLowID(BreedClassHighLowID); //(UpdateBreedClassHighLowID);
                    XH_ValidDeclareType xH_ValidDeclareType =
                      SpotManageCommon.GetModelValidDeclareType(BreedClassValidID);
                    XH_ValidDeclareValue xH_ValidDeclareValue =
                        SpotManageCommon.GetModelValidDeclareValue(BreedClassValidID);
                    //涨跌幅
                    if (xH_SpotHighLowControlType != null && xH_SpotHighLowValue != null)
                    {
                        if (xH_SpotHighLowControlType.HighLowTypeID != 0)
                        {
                            foreach (object item in cmbHighLowTypeID.Properties.Items)
                            {
                                if (((UComboItem)item).ValueIndex == xH_SpotHighLowControlType.HighLowTypeID)
                                {
                                    cmbHighLowTypeID.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            cmbHighLowTypeID.SelectedIndex = 0;
                        }
                        switch (cmbHighLowTypeID.SelectedIndex)
                        {
                            case 0: //股票
                                txtHighLowValue1.Text = xH_SpotHighLowValue.StValue.ToString();
                                txtHighLowValue2.Text = xH_SpotHighLowValue.NormalValue.ToString();
                                break;
                            case 1: //权证
                                txtHighLowValue1.Text = xH_SpotHighLowValue.RightHighLowScale.ToString();
                                break;
                            case 2: //基金
                                txtHighLowValue1.Text = xH_SpotHighLowValue.FundYestClosePriceScale.ToString();
                                break;
                            case 3:  //债券和港股(无涨跌幅限制:因债券和港股均无涨跌幅限制，所以当是修改时界面上是否显示“港股”由有效申报类型决定)
                                cmbHighLowTypeID.SelectedIndex = 3;
                                break;
                        }
                        m_BreedClassHighLowID = xH_SpotHighLowControlType.BreedClassHighLowID;
                    }
                    //有效申报
                    if (xH_ValidDeclareType != null && xH_ValidDeclareValue != null)
                    {
                        if (xH_ValidDeclareType.ValidDeclareTypeID != 0)
                        {
                            foreach (object item in this.cmbValidDeclareTypeID.Properties.Items)
                            {
                                if (((UComboItem)item).ValueIndex == xH_ValidDeclareType.ValidDeclareTypeID)
                                {
                                    this.cmbValidDeclareTypeID.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this.cmbValidDeclareTypeID.SelectedIndex = 0;
                        }
                        this.txtUpperLimit.Text = xH_ValidDeclareValue.UpperLimit.ToString();
                        this.txtLowerLimit.Text = xH_ValidDeclareValue.LowerLimit.ToString();
                        if (xH_ValidDeclareValue.NewDayUpperLimit != null)
                        {
                            this.txtNewDayUpperLimit.Text = xH_ValidDeclareValue.NewDayUpperLimit.ToString();
                        }
                        if (xH_ValidDeclareValue.NewDayLowerLimit != null)
                        {
                            this.txtNewDayLowerLimit.Text = xH_ValidDeclareValue.NewDayLowerLimit.ToString();

                        }
                        m_BreedClassValidID = xH_ValidDeclareType.BreedClassValidID;
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5043";
                string errMsg = " 当前UI是修改涨跌幅UI时,初始化控件的值失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        #region 检验有效申报取值上，下限的输入
        /// <summary>
        /// 检验有效申报取值上，下限的输入
        /// </summary>
        /// <param name="strMess">提示信息</param>
        /// <returns></returns>
        private XH_ValidDeclareValue VeriyXHValidDeclareValue(ref string strMess)
        {
            strMess = string.Empty;
            try
            {
                XH_ValidDeclareValue xHValidDeclareVEntity = new XH_ValidDeclareValue();
                if (!string.IsNullOrEmpty(txtUpperLimit.Text))
                {
                    if (InputTest.DecimalTest(this.txtUpperLimit.Text))
                    {
                        xHValidDeclareVEntity.UpperLimit = Convert.ToDecimal(txtUpperLimit.Text);
                    }
                    else
                    {
                        //ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        strMess = "格式不正确(只能包含数字和小数点)!";
                    }
                }
                else
                {
                    //ShowMessageBox.ShowInformation("上限不能为空!");
                    strMess = "上限不能为空!";
                }
                if (!string.IsNullOrEmpty(txtLowerLimit.Text))
                {
                    if (InputTest.DecimalTest(this.txtLowerLimit.Text))
                    {
                        xHValidDeclareVEntity.LowerLimit = Convert.ToDecimal(txtLowerLimit.Text);
                    }
                    else
                    {
                        // ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                        strMess += "格式不正确(只能包含数字和小数点)!";

                    }
                }
                else
                {
                    //ShowMessageBox.ShowInformation("下限不能为空!");
                    strMess += "下限不能为空!";
                }
                return xHValidDeclareVEntity;

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }
        #endregion

        //================================  事件 ================================

        #region 添加现货涨跌幅窗体 AddSpotHighLowValueUI_Load

        /// <summary>
        /// 添加现货涨跌幅窗体 AddSpotHighLowValueUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSpotHighLowValueUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (HighLowUIEditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    //添加功能时涨跌幅指定默认值
                    txtHighLowValue2.Visible = true;
                    txtHighLowValue1.Visible = true;
                    labHighLow1.Text = "ST股票:";
                    labHighLow1.Location = new Point(14, 67);
                    labHighLow2.Text = "正常股票:";
                    labHighLow2.Location = new Point(14, 98);
                    labUnit1.Visible = true;
                    labUnit2.Visible = true;

                    //添加功能时有效申报指定默认值
                    labValidDeclareTypeDetail.Text = "最近成交价的上下百分比。";
                    labUpperLUnit.Text = "%";
                    labLowerLUnit.Text = "%";
                    labNewDayUpperLimit.Visible = false;
                    labNewDayLowerLimit.Visible = false;
                }
                InitBindData();
                if (HighLowUIEditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    Text = "修改现货涨跌幅";
                    UpdateInitData();
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5040";
                string errMsg = " 添加现货涨跌幅窗体加载失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region  添加/修改现货涨跌幅和有效申报

        /// <summary>
        /// 添加/修改现货涨跌幅和有效申报
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var xHSpotHighLowValue = new XH_SpotHighLowValue();
                var xHSpotHighLowControlType = new XH_SpotHighLowControlType();
                var xHValidDeclareType = new XH_ValidDeclareType();
                var xHValidDeclareValue = new XH_ValidDeclareValue();

                //涨跌幅类型和有效申报类型
                if (!string.IsNullOrEmpty(cmbHighLowTypeID.Text) && !string.IsNullOrEmpty(cmbValidDeclareTypeID.Text))
                {
                    xHSpotHighLowControlType.HighLowTypeID = ((UComboItem)cmbHighLowTypeID.SelectedItem).ValueIndex;

                    switch (cmbHighLowTypeID.SelectedIndex)
                    {
                        case 0:
                            //股票
                            if (!string.IsNullOrEmpty(txtHighLowValue1.Text))
                            {
                                if (InputTest.DecimalTest(this.txtHighLowValue1.Text))
                                {
                                    xHSpotHighLowValue.StValue = Convert.ToDecimal(txtHighLowValue1.Text);
                                }
                                else
                                {
                                    ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                                    return;
                                }
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("ST股票不能为空!");
                                return;
                            }

                            if (!string.IsNullOrEmpty(txtHighLowValue2.Text))
                            {
                                if (InputTest.DecimalTest(this.txtHighLowValue2.Text))
                                {
                                    xHSpotHighLowValue.NormalValue = Convert.ToDecimal(txtHighLowValue2.Text);
                                }
                                else
                                {
                                    ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                                    return;
                                }
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("正常股票不能为空!");
                                return;
                            }
                            break;
                        case 1: //权证
                            if (!string.IsNullOrEmpty(txtHighLowValue1.Text))
                            {
                                if (InputTest.DecimalTest(this.txtHighLowValue1.Text))
                                {
                                    xHSpotHighLowValue.RightHighLowScale = Convert.ToDecimal(txtHighLowValue1.Text);
                                }
                                else
                                {
                                    ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                                    return;
                                }
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("权证涨跌幅不能为空!");
                                return;
                            }
                            break;
                        case 2: //基金
                            if (!string.IsNullOrEmpty(txtHighLowValue1.Text))
                            {
                                if (InputTest.DecimalTest(this.txtHighLowValue1.Text))
                                {
                                    xHSpotHighLowValue.FundYestClosePriceScale = Convert.ToDecimal(txtHighLowValue1.Text);
                                }
                                else
                                {
                                    ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                                    return;
                                }
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("基金涨跌幅不能为空!");
                                return;
                            }
                            break;
                        case 3:
                            //债券(无涨跌幅限制)
                            xHSpotHighLowValue.StValue = AppGlobalVariable.INIT_DECIMAL;
                            xHSpotHighLowValue.NormalValue = AppGlobalVariable.INIT_DECIMAL;
                            xHSpotHighLowValue.RightHighLowScale = AppGlobalVariable.INIT_DECIMAL;
                            xHSpotHighLowValue.FundYestClosePriceScale = AppGlobalVariable.INIT_DECIMAL;
                            break;
                    }
                    //有效申报
                    switch (cmbValidDeclareTypeID.SelectedIndex)
                    {
                        case 0: //最近成交价的上下百分比
                            if (!string.IsNullOrEmpty(cmbValidDeclareTypeID.Text))
                            {
                                xHValidDeclareType.ValidDeclareTypeID =
                                    ((UComboItem)cmbValidDeclareTypeID.SelectedItem).ValueIndex;
                            }
                            else
                            {
                                xHValidDeclareType.ValidDeclareTypeID = AppGlobalVariable.INIT_INT;
                            }
                            string mess = string.Empty;
                            xHValidDeclareValue = VeriyXHValidDeclareValue(ref mess);
                            if (!string.IsNullOrEmpty(mess))
                            {
                                ShowMessageBox.ShowInformation(mess);
                                return;
                            }
                            break;
                        case 1: //不高于即时揭示的最低卖出价格的百分比且不低于即时揭示
                            if (!string.IsNullOrEmpty(cmbValidDeclareTypeID.Text))
                            {
                                xHValidDeclareType.ValidDeclareTypeID =
                                    ((UComboItem)cmbValidDeclareTypeID.SelectedItem).ValueIndex;
                            }
                            else
                            {
                                xHValidDeclareType.ValidDeclareTypeID = AppGlobalVariable.INIT_INT;
                            }
                            mess = string.Empty;
                            xHValidDeclareValue = VeriyXHValidDeclareValue(ref mess);
                            if (!string.IsNullOrEmpty(mess))
                            {
                                ShowMessageBox.ShowInformation(mess);
                                return;
                            }
                            break;
                        case 2://3:
                            if (!string.IsNullOrEmpty(cmbValidDeclareTypeID.Text))
                            {
                                xHValidDeclareType.ValidDeclareTypeID =
                                    ((UComboItem)cmbValidDeclareTypeID.SelectedItem).ValueIndex;
                            }
                            else
                            {
                                xHValidDeclareType.ValidDeclareTypeID = AppGlobalVariable.INIT_INT;
                            }
                            mess = string.Empty;
                            xHValidDeclareValue = VeriyXHValidDeclareValue(ref mess);
                            if (!string.IsNullOrEmpty(mess))
                            {
                                ShowMessageBox.ShowInformation(mess);
                                return;
                            }
                            if (!string.IsNullOrEmpty(txtNewDayUpperLimit.Text))
                            {
                                if (InputTest.DecimalTest(this.txtNewDayUpperLimit.Text))
                                {
                                    xHValidDeclareValue.NewDayUpperLimit = Convert.ToDecimal(txtNewDayUpperLimit.Text);
                                }
                                else
                                {
                                    ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                                    return;
                                }
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("上市首日上限不能为空!");
                                return;
                            }
                            if (!string.IsNullOrEmpty(txtNewDayLowerLimit.Text))
                            {
                                if (InputTest.DecimalTest(this.txtNewDayLowerLimit.Text))
                                {
                                    xHValidDeclareValue.NewDayLowerLimit = Convert.ToDecimal(txtNewDayLowerLimit.Text);
                                }
                                else
                                {
                                    ShowMessageBox.ShowInformation("格式不正确(只能包含数字和小数点)!");
                                    return;
                                }
                            }
                            else
                            {
                                ShowMessageBox.ShowInformation("上市首日下限不能为空!");
                                return;
                            }
                           
                            break;
                    }
                    if (HighLowUIEditType == (int)UITypes.EditTypeEnum.AddUI)
                    {
                        XH_AboutSpotHighLowEntity xhAboutSpotHighLowEntity =
                            SpotManageCommon.AddXHSpotHighLowAndValidDecl(xHSpotHighLowControlType, xHSpotHighLowValue,
                                                                          xHValidDeclareType, xHValidDeclareValue);
                        if (xhAboutSpotHighLowEntity != null)
                        {
                            BreedClassHighLowID = (int)xhAboutSpotHighLowEntity.BreedClassHighLowID;
                            BreedClassValidID = (int)xhAboutSpotHighLowEntity.BreedClassValidID;
                            ShowMessageBox.ShowInformation("添加成功!");
                            ClearAll();
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("添加失败!");
                        }
                    }
                    else if (HighLowUIEditType == (int)UITypes.EditTypeEnum.UpdateUI)
                    {
                        if (m_BreedClassHighLowID != AppGlobalVariable.INIT_INT)
                        {
                            xHSpotHighLowControlType.BreedClassHighLowID = m_BreedClassHighLowID;
                            xHSpotHighLowValue.BreedClassHighLowID = m_BreedClassHighLowID;
                            xHValidDeclareType.BreedClassValidID = m_BreedClassValidID;
                            xHValidDeclareValue.BreedClassValidID = m_BreedClassValidID;

                        }
                        bool _UpResult = SpotManageCommon.UpdateXHSpotHighLowAndValidDecl(xHSpotHighLowControlType,
                                                                                 xHSpotHighLowValue, xHValidDeclareType, xHValidDeclareValue);
                        if (_UpResult)
                        {
                            ShowMessageBox.ShowInformation("修改成功!");
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("修改失败!");
                        }
                    }
                }
                else
                {
                    xHSpotHighLowControlType.HighLowTypeID = AppGlobalVariable.INIT_INT;
                    xHValidDeclareType.ValidDeclareTypeID = AppGlobalVariable.INIT_INT;
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5042";
                string errMsg = " 添加现货涨跌幅失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 添加现货涨跌幅的取消事件 btnCancel_Click

        /// <summary>
        /// 添加现货涨跌幅的取消事件 btnCancel_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region 现货涨跌幅类型组合框改变事件

        /// <summary>
        /// 现货涨跌幅类型组合框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHighLowTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbHighLowTypeID.SelectedIndex ==
                    (int)Types.XHSpotHighLowControlType.NewThighAddFatStopAfterOrOtherDate - 1)
                {
                    //股票
                    txtHighLowValue2.Visible = true;
                    txtHighLowValue1.Visible = true;
                    labHighLow1.Text = "ST股票:";
                    labHighLow1.Location = new Point(14, 67);
                    labHighLow2.Text = "正常股票:";
                    labHighLow2.Location = new Point(14, 98);
                    labUnit1.Visible = true;
                    labUnit2.Visible = true;
                    labHighID1.Visible = true;
                    labHighID2.Visible = true;
                    labHighLow1.Visible = true;
                    labHighLow2.Visible = true;
                }
                else if (cmbHighLowTypeID.SelectedIndex == (int)Types.XHSpotHighLowControlType.RightPermitHighLow - 2)
                {
                    //权证
                    txtHighLowValue1.Visible = true;
                    txtHighLowValue2.Visible = false;
                    labHighLow1.Text = "权证涨跌幅:";
                    labHighLow1.Visible = true;
                    labHighLow1.Location = new Point(14, 67);
                    labHighLow2.Visible = false;
                    labHighID1.Visible = true;
                    labUnit1.Visible = true;
                    labUnit2.Visible = false;
                    labHighID2.Visible = false;
                }
                else if (cmbHighLowTypeID.SelectedIndex ==
                         (int)Types.XHSpotHighLowControlType.NewFundAddFatStopAfterOrOtherDate - 2)
                {
                    //基金
                    txtHighLowValue1.Visible = true;
                    txtHighLowValue2.Visible = false;
                    labHighLow1.Text = "基金涨跌幅:";
                    labHighLow1.Visible = true;
                    labHighLow1.Location = new Point(14, 67);
                    labHighLow2.Visible = false;
                    labUnit1.Visible = true;
                    labHighID1.Visible = true;
                    labUnit2.Visible = false;
                    labHighID2.Visible = false;
                }
                else if (cmbHighLowTypeID.SelectedIndex == (int)Types.XHSpotHighLowControlType.NotHighLowControl + 1)
                {
                    //债券 "无涨跌幅限制";
                    txtHighLowValue1.Visible = false;
                    txtHighLowValue2.Visible = false;
                    labHighLow1.Visible = false;
                    labHighLow2.Visible = false;
                    labUnit1.Visible = false;
                    labUnit2.Visible = false;
                    labHighID1.Visible = false;
                    labHighID2.Visible = false;
                }
                //else if (cmbHighLowTypeID.SelectedIndex ==
                //         (int)Types.XHSpotHighLowControlType.NotHighLowControl + 2)
                //{
                //    //港股 "无涨跌幅限制";
                //    txtHighLowValue1.Visible = false;
                //    txtHighLowValue2.Visible = false;
                //    labHighLow1.Visible = false;
                //    labHighLow2.Visible = false;
                //    labUnit1.Visible = false;
                //    labUnit2.Visible = false;
                //    labHighID1.Visible = false;
                //    labHighID2.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                string errCode = "GL-5044";
                string errMsg = "现货涨跌幅类型组合框改变事件失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 现货有效申报类型的组合框改变事件

        /// <summary>
        /// 现货有效申报类型的组合框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbValidDeclareTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbValidDeclareTypeID.SelectedIndex == 0)
                {
                    labValidDeclareTypeDetail.Text = "最近成交价的上下百分比。";
                    labUpperLUnit.Text = "%";
                    labLowerLUnit.Text = "%";
                    labNewDayUpperLUnit.Visible = false;
                    labNewDayLowerLUnit.Visible = false;
                    labNewDayLowerID.Visible = false;
                    labNewDayUpperID.Visible = false;
                    txtNewDayLowerLimit.Visible = false;
                    txtNewDayUpperLimit.Visible = false;
                    labNewDayUpperLimit.Visible = false;
                    labNewDayLowerLimit.Visible = false;
                }
                else if (cmbValidDeclareTypeID.SelectedIndex == 1)
                {
                    labValidDeclareTypeDetail.Text = "不高于即时揭示的最低卖出价格的百分比且不低于即时揭示" + "\n" + "的最高买入价格的百分比。";
                    labUpperLUnit.Text = "%";
                    labLowerLUnit.Text = "%";
                    labNewDayUpperLUnit.Visible = false;
                    labNewDayLowerLUnit.Visible = false;
                    labNewDayLowerID.Visible = false;
                    labNewDayUpperID.Visible = false;
                    txtNewDayLowerLimit.Visible = false;
                    txtNewDayUpperLimit.Visible = false;
                    labNewDayUpperLimit.Visible = false;
                    labNewDayLowerLimit.Visible = false;
                }
                //else if (cmbValidDeclareTypeID.SelectedIndex == 2)
                //{
                //    labValidDeclareTypeDetail.Text = "低于买一价的个价位与卖一价之间或低于买一价与高于卖一" + "\n" + "价的个价位之间。";
                //    labUpperLUnit.Text = "个";
                //    labLowerLUnit.Text = "个";
                //    labNewDayUpperLUnit.Visible = false;
                //    labNewDayLowerLUnit.Visible = false;
                //    labNewDayLowerID.Visible = false;
                //    labNewDayUpperID.Visible = false;
                //    txtNewDayLowerLimit.Visible = false;
                //    txtNewDayUpperLimit.Visible = false;
                //    labNewDayUpperLimit.Visible = false;
                //    labNewDayLowerLimit.Visible = false;
                //}
                else if (cmbValidDeclareTypeID.SelectedIndex == 2) //3)
                {
                    labValidDeclareTypeDetail.Text = "最近成交价的上下各多少元。";
                    labUpperLUnit.Text = "元";
                    labLowerLUnit.Text = "元";
                    labNewDayUpperLUnit.Visible = true;
                    labNewDayLowerLUnit.Visible = true;
                    labNewDayLowerID.Visible = true;
                    labNewDayUpperID.Visible = true;
                    txtNewDayLowerLimit.Visible = true;
                    txtNewDayUpperLimit.Visible = true;
                    labNewDayUpperLimit.Visible = true;
                    labNewDayLowerLimit.Visible = true;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5045";
                string errMsg = "现货有效申报类型的组合框改变事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

    }
}