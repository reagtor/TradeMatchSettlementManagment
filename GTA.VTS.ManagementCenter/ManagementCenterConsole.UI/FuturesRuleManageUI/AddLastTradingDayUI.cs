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
    /// 描述：添加期货最后交易日 错误编码范围:5840-5859
    /// 作者：刘书伟
    /// 日期：2008-12-08
    /// </summary>
    public partial class AddLastTradingDayUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        public AddLastTradingDayUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量及属性

        /// <summary>
        /// 最后交易日ID
        /// </summary>
        private int _LastTradingDayID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 最后交易日标识属性
        /// </summary>
        public int LastTradingDayID
        {
            set { _LastTradingDayID = value; }
            get { return _LastTradingDayID; }
        }

        /// <summary>
        /// 最后交易日ID
        /// </summary>
        public int m_LastTradingDayID = AppGlobalVariable.INIT_INT;

        #region 操作类型　 1:添加,2:修改

        private int m_LastTradingDayUIEditType = (int) UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int LastTradingDayUIEditType
        {
            get { return this.m_LastTradingDayUIEditType; }
            set { this.m_LastTradingDayUIEditType = value; }
        }

        #endregion

        #region 当是修改有最后交易日UI时，获取最后交易日ID

        /// <summary>
        ///  当是修改有最后交易日UI时，获取最后交易日ID
        /// </summary>
        private int m_UpdateLastTradingDayID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 当是修改有最后交易日UI时，获取最后交易日ID
        /// </summary>
        public int UpdateLastTradingDayID
        {
            set { m_UpdateLastTradingDayID = value; }
            get { return m_UpdateLastTradingDayID; }
        }

        #endregion

        #endregion

        //================================  私有  方法 ================================

        #region 绑定初始化数据 InitBindData()

        /// <summary>
        ///绑定初始化数据
        /// </summary>
        private void InitBindData()
        {
            try
            {
                //绑定最后交易日类型
                this.cmbLastTradingDayType.Properties.Items.Clear();
                this.cmbLastTradingDayType.Properties.Items.AddRange(BindData.GetBindListQHLastTradingDayType());
                this.cmbLastTradingDayType.SelectedIndex = 0;

                //绑定星期几
                this.cmbWeek.Properties.Items.Clear();
                this.cmbWeek.Properties.Items.AddRange(BindData.GetBindListQHWeek());
                this.cmbWeek.SelectedIndex = 0;
                this.cmbWeek.EditValue = string.Empty;

                //绑定期货最后交易日是顺数或倒数
                this.cmbSequence.Properties.Items.Clear();
                this.cmbSequence.Properties.Items.AddRange(BindData.GetBindListQHLastTradDayIsSequence());
                this.cmbSequence.SelectedIndex = 0;
                this.cmbSequence.EditValue = string.Empty;

                //this.speWhatWeek.Value=Convert.ToDecimal(DBNull.Value);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5843";
                string errMsg = "绑定初始化数据失败!";
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
            //?下拉框清除
            this.cmbLastTradingDayType.Text = string.Empty;
            this.cmbSequence.Text = string.Empty;
            this.cmbWeek.Text = string.Empty;
            this.speWhatDay.Text = string.Empty;
            this.speWhatWeek.Text = string.Empty;
        }

        #endregion

        #region 当前UI是修改最后交易日UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改最后交易日UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (UpdateLastTradingDayID != AppGlobalVariable.INIT_INT)
                {
                    QH_LastTradingDay qH_LastTradingDay =
                        FuturesManageCommon.GetQHLastTradingDayModel(UpdateLastTradingDayID);

                    if (qH_LastTradingDay != null)
                    {
                        if (qH_LastTradingDay.LastTradingDayTypeID != 0)
                        {
                            foreach (object item in this.cmbLastTradingDayType.Properties.Items)
                            {
                                if (((UComboItem)item).ValueIndex == qH_LastTradingDay.LastTradingDayTypeID)
                                {
                                    this.cmbLastTradingDayType.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this.cmbLastTradingDayType.SelectedIndex = 0;
                        }

                        if (qH_LastTradingDay.Week != 0)
                        {
                            foreach (object item in this.cmbWeek.Properties.Items)
                            {
                                if (((UComboItem)item).ValueIndex == qH_LastTradingDay.Week)
                                {
                                    this.cmbWeek.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this.cmbWeek.SelectedIndex = 0;
                        }

                        if (qH_LastTradingDay.Sequence != 0)
                        {
                            foreach (object item in this.cmbSequence.Properties.Items)
                            {
                                if (((UComboItem)item).ValueIndex == qH_LastTradingDay.Sequence)
                                {
                                    this.cmbSequence.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this.cmbSequence.SelectedIndex = 0;
                        }
                        this.speWhatDay.Text = qH_LastTradingDay.WhatDay.ToString();
                        this.speWhatWeek.Text = qH_LastTradingDay.WhatWeek.ToString();
                        m_LastTradingDayID = qH_LastTradingDay.LastTradingDayID;
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5842";
                string errMsg = "当前UI是修改最后交易日UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }

        }

        #endregion

        //================================  事件 ================================

        #region 添加或修改最后交易日UI AddLastTradingDayUI_Load

        /// <summary>
        /// 添加或修改最后交易日UI AddLastTradingDayUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLastTradingDayUI_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitBindData();

                if (LastTradingDayUIEditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    //this.Text = "修改最后交易日";
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5840";
                string errMsg = "添加或修改最后交易日UI加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 添加或修改最后交易日
        /// <summary>
        /// 添加或修改最后交易日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                QH_LastTradingDay qHLastTradingDay = new QH_LastTradingDay();

                if (!string.IsNullOrEmpty(this.cmbLastTradingDayType.Text))
                {
                    qHLastTradingDay.LastTradingDayTypeID =
                        ((UComboItem)this.cmbLastTradingDayType.SelectedItem).ValueIndex;
                }
                else
                {
                   // qHLastTradingDay.LastTradingDayTypeID = AppGlobalVariable.INIT_INT;
                    ShowMessageBox.ShowInformation("最后交易日类型不能为空!");
                    return;
                }

                if (!string.IsNullOrEmpty(this.cmbSequence.Text))
                {
                    qHLastTradingDay.Sequence =
                        ((UComboItem)this.cmbSequence.SelectedItem).ValueIndex;
                }
                else
                {
                    qHLastTradingDay.Sequence = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(this.cmbWeek.Text))
                {
                    qHLastTradingDay.Week =
                        ((UComboItem)this.cmbWeek.SelectedItem).ValueIndex;
                }
                else
                {
                    qHLastTradingDay.Week = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(this.speWhatDay.Text))
                {
                    qHLastTradingDay.WhatDay = Convert.ToInt32(this.speWhatDay.EditValue);
                    ;
                }
                else
                {
                    qHLastTradingDay.WhatDay = AppGlobalVariable.INIT_INT;
                }

                if (!string.IsNullOrEmpty(this.speWhatWeek.Text))
                {
                    qHLastTradingDay.WhatWeek = Convert.ToInt32(this.speWhatWeek.EditValue);
                }
                else
                {
                    qHLastTradingDay.WhatWeek = AppGlobalVariable.INIT_INT;
                }


                if (LastTradingDayUIEditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    int lastTradingDayID = FuturesManageCommon.AddQHLastTradingDay(qHLastTradingDay);
                    if (lastTradingDayID != AppGlobalVariable.INIT_INT)
                    {
                        LastTradingDayID = lastTradingDayID;
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
                else if (LastTradingDayUIEditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    switch (this.cmbLastTradingDayType.SelectedIndex)
                    {
                        case (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndDay - 1:
                            qHLastTradingDay.WhatWeek = 0;
                            qHLastTradingDay.Week = 0;
                            qHLastTradingDay.Sequence = 1;//默认顺数
                            break;
                        case (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek - 1:
                            //qHLastTradingDay.WhatDay = 0;
                            qHLastTradingDay.Week = 0;
                            qHLastTradingDay.WhatWeek = 0;
                            break;
                        case (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndWeek - 1:
                            qHLastTradingDay.WhatDay = 0;
                            qHLastTradingDay.Sequence = 1;//默认顺数
                            break;
                        case (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay - 1:
                            qHLastTradingDay.WhatWeek = 0;
                            qHLastTradingDay.Week = 0;
                            //qHLastTradingDay.Sequence = 1;//默认顺数
                            break;

                    }
                    if (m_UpdateLastTradingDayID != AppGlobalVariable.INIT_INT)
                    {
                        qHLastTradingDay.LastTradingDayID = m_LastTradingDayID;
                    }
                    bool _UpResult = FuturesManageCommon.UpdateQHLastTradingDay(qHLastTradingDay);
                    if (_UpResult)
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
                string errCode = "GL-5841";
                string errMsg = "添加或修改最后交易日失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return; ;
            }

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

        #region 最后交易日类型改变事件 cmbLastTradingDayType_SelectedIndexChanged
        /// <summary>
        /// 最后交易日类型改变事件 cmbLastTradingDayType_SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLastTradingDayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cmbLastTradingDayType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndDay - 1)
                {
                    this.cmbLastTradingDayType.ToolTip = "交割月份\r\n" + "+第几个自然日";
                    this.speWhatDay.Enabled = true;
                    this.speWhatWeek.Enabled = false;
                    this.cmbWeek.Enabled = false;
                    this.cmbSequence.Enabled = false;
                    //属于此类型的选择项，默认是第一个值
                    this.speWhatDay.Value = 0;
                    //不属于此类型的选择项，界面上显示为空
                    //this.speWhatWeek.Value =Convert.ToDecimal(DBNull.Value);
                    this.cmbWeek.EditValue = string.Empty;
                    this.cmbSequence.EditValue = string.Empty;
                }
                else if (this.cmbLastTradingDayType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndDownOrShunAndWeek - 1)
                {
                    this.cmbLastTradingDayType.ToolTip = "交割月份\r\n" + "+倒数/顺数\r\n" + "+第几个交易日";
                    this.speWhatDay.Enabled = true;
                    this.speWhatWeek.Enabled = false;
                    this.cmbWeek.Enabled = false;
                    this.cmbSequence.Enabled = true;
                    //属于此类型的选择项，默认是第一个值
                    //this.cmbWeek.SelectedIndex = 0;
                    this.cmbSequence.SelectedIndex = 0;
                    //不属于此类型的选择项，界面上显示为空
                    //this.speWhatDay.Value = Convert.ToDecimal(DBNull.Value);
                    //this.speWhatWeek.Value =Convert.ToDecimal(DBNull.Value);
                }
                else if (this.cmbLastTradingDayType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAndWeek - 1)
                {
                    this.cmbLastTradingDayType.ToolTip = "交割月份\r\n" + "+第几周\r\n" + "+星期几";
                    this.speWhatDay.Enabled = false;
                    this.speWhatWeek.Enabled = true;
                    this.cmbWeek.Enabled = true;
                    this.cmbSequence.Enabled = false;
                    //属于此类型的选择项，默认是第一个值
                    this.speWhatWeek.Value = 0;
                    this.cmbWeek.SelectedIndex = 0;
                    //不属于此类型的选择项，界面上显示为空
                    //this.speWhatDay.Value =Convert.ToDecimal(DBNull.Value);
                    this.cmbSequence.EditValue = string.Empty;
                }
                else if (this.cmbLastTradingDayType.SelectedIndex == (int)GTA.VTS.Common.CommonObject.Types.QHLastTradingDayType.DeliMonthAgoMonthLastTradeDay - 1)
                {
                    this.cmbLastTradingDayType.ToolTip = "交割月份前一个月份\r\n" + "+倒数/顺数\r\n" + "+第几个交易日";
                    this.speWhatDay.Enabled = true;
                    this.speWhatWeek.Enabled = false;
                    this.cmbWeek.Enabled = false;
                    this.cmbSequence.Enabled = true;
                    this.cmbSequence.SelectedIndex = 1;
                    //属于此类型的选择项，默认是第一个值
                    this.speWhatDay.Value = 0;
                    //不属于此类型的选择项，界面上显示为空
                    //this.speWhatWeek.Value =Convert.ToDecimal(DBNull.Value);
                    this.cmbWeek.EditValue = string.Empty;
                    //this.cmbSequence.EditValue = string.Empty;
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5844";
                string errMsg = "最后交易日类型改变事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

    }
}