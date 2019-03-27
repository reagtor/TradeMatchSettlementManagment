using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using DevExpress.XtraEditors;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.CommonParameterSet
{
    /// <summary>
    /// 描述：交易时间管理窗体   错误编码范围:4440-4459
    /// 作者：刘书伟
    /// 日期：2008-12-05
    /// </summary>
    public partial class TradeTimeManagerUI : XtraForm
    {
        #region 构造函数

        public TradeTimeManagerUI()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量

        #region  当前页

        /// <summary>
        /// 当前页
        /// </summary>
        public int m_pageNo = int.MaxValue;

        #endregion

        #region 一页展示记录数

        /// <summary>
        /// 一页展示记录数
        /// </summary>
        public int m_pageSize = ParameterSetting.PageSize;

        #endregion

        #region  总记录数

        /// <summary>
        /// 总记录数
        /// </summary>
        public int m_rowCount = int.MaxValue;

        #endregion

        /// <summary>
        /// 存放交易时间
        /// </summary>
        private DataTable _dtTradeTime = new DataTable();

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

        /// <summary>
        /// 交易所类型_交易时间ID
        /// </summary>
        private int m_TradeTimeID = AppGlobalVariable.INIT_INT;

        #region 操作类型　 1:添加,2:修改

        private int m_EditType = (int)UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return m_EditType; }
            set { m_EditType = value; }
        }

        #endregion

        #region 交易所类型实体

        /// <summary>
        /// 交易所类型实体
        /// </summary>
        private CM_BourseType m_CM_BourseType = null;

        /// <summary>
        /// 交易所类型属性
        /// </summary>
        public CM_BourseType CMBourseType
        {
            get { return m_CM_BourseType; }
            set
            {
                m_CM_BourseType = new CM_BourseType();
                m_CM_BourseType = value;
            }
        }

        #endregion

        #region 交易时间实体

        /// <summary>
        /// 交易时间实体
        /// </summary>
        private CM_TradeTime m_CM_TradeTime = null;

        ///// <summary>
        ///// 交易时间属性
        ///// </summary>
        public CM_TradeTime CMTradeTime
        {
            get { return m_CM_TradeTime; }
            set
            {
                m_CM_TradeTime = new CM_TradeTime();
                m_CM_TradeTime = value;
            }
        }

        #endregion

        /// <summary>
        /// 交易所类型ID
        /// </summary>
        private int m_BourseTypeID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 记录已经比较过的行（区间比较时用）
        /// </summary>
        private int m_IsCompareRow = 0;

        #endregion

        //================================  私有  方法 ================================

        #region 根据查询条件，获取交易所类型_交易时间

        /// <summary>
        /// 根据查询条件，获取交易所类型_交易时间
        /// </summary>
        /// <returns></returns>
        private bool QueryCMTradeTime()
        {
            try
            {
                if (m_BourseTypeID != AppGlobalVariable.INIT_INT)
                {
                    DataSet _dsCMTradeTime =
                        CommonParameterSetCommon.GetTradeTimeAndBourseTypeList(m_BourseTypeID);//GetTradeTimeByBourseTypeID(m_BourseTypeID);
                    DataTable _dtCMTradeTime;
                    if (_dsCMTradeTime == null || _dsCMTradeTime.Tables[0].Rows.Count == 0)
                    {
                        _dtCMTradeTime = new DataTable();
                    }
                    else
                    {
                        _dtCMTradeTime = _dsCMTradeTime.Tables[0];
                    }
                    gdTradeTimeResult.DataSource = _dtCMTradeTime;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4445";
                string errMsg = " 根据查询条件，获取交易所类型_交易时间失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            return true;
        }

        #endregion

        #region 当前UI是修改交易时间UI时,初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改交易时间UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (m_CM_BourseType != null)
                {
                    m_BourseTypeID = Convert.ToInt32(m_CM_BourseType.BourseTypeID);
                    txtBourseTypeName.Text = m_CM_BourseType.BourseTypeName;
                    tmCounFromSubmitStartTime.EditValue = m_CM_BourseType.CounterFromSubmitStartTime;
                    tmCounFromSubmitEndTime.EditValue = m_CM_BourseType.CounterFromSubmitEndTime;
                    //根据交易所类型ID获取相关交易时间
                    List<CM_TradeTime> cmTradeTimes =
                        CommonParameterSetCommon.GetCMTradeTimeListArray(string.Format("BourseTypeID={0}", m_BourseTypeID));
                    if (cmTradeTimes.Count > 0)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4448";
                string errMsg = "当前UI是修改交易时间UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                throw exception;
            }
        }

        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //组合框的值全清空?
            //cmbBourseTypeID.Text = string.Empty;
            tmTradeStartTime.EditValue = "00:00";
            tmTradeEndTime.EditValue = "00:00";
        }

        #endregion

        #region 修改交易所类型_交易时间

        /// <summary>
        /// 修改交易所类型_交易时间
        /// </summary>
        private void ModifyCMTradeTime()
        {
            try
            {
                if (gdvTradeTimeSelect != null && gdvTradeTimeSelect.DataSource != null &&
                    gdvTradeTimeSelect.RowCount > 0 && gdvTradeTimeSelect.FocusedRowHandle >= 0)
                {
                    DataRow _dr = gdvTradeTimeSelect.GetDataRow(gdvTradeTimeSelect.FocusedRowHandle);
                    m_TradeTimeID = Convert.ToInt32(_dr["TradeTimeID"]);
                    tmTradeStartTime.EditValue = Convert.ToDateTime(_dr["StartTime"]);
                    tmTradeEndTime.EditValue = Convert.ToDateTime(_dr["EndTime"]);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4444";
                string errMsg = "获取需要修改的交易所类型_交易时间失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region  交易时间管理窗体 TradeTimeManagerUI_Load

        /// <summary>
        /// 交易时间管理窗体 TradeTimeManagerUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeTimeManagerUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_CM_BourseType = new CM_BourseType();
                    m_CM_TradeTime = new CM_TradeTime();

                    _dtTradeTime.Columns.Add(gdvTradeTimeSelect.Columns[0].FieldName, Type.GetType("System.String"));

                    //添加时在自定义的交易时间DataTabel中新增2列
                    _dtTradeTime.Columns.Add(gdvTradeTimeSelect.Columns[1].FieldName, Type.GetType("System.String"));
                    _dtTradeTime.Columns.Add(gdvTradeTimeSelect.Columns[2].FieldName, Type.GetType("System.String"));
                    this.btnUpdateTradeTime.Enabled = false;
                    this.btnOk.Text = "确定";
                }
                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    UpdateInitData();
                    QueryCMTradeTime();
                    this.btnOk.Text = "修改交易所类型";
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4440";
                string errMsg = "交易时间管理窗体加载失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 交易所类型_交易时间 GridView的双击事件

        /// <summary>
        ///交易所类型_交易时间 GridView的双击事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdTradeTimeResult_DoubleClick(object sender, EventArgs e)
        {
            ModifyCMTradeTime(); 
            this.btnOk.Text = "修改交易所类型";
        }

        #endregion

        #region 取消按钮事件 btnCancel_Click

        /// <summary>
        /// 取消按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        #endregion

        #region  添加非交易日期  btnAddNotTradeDate_Click

        /// <summary>
        ///  添加非交易日期  btnAddNotTradeDate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNotTradeDate_Click(object sender, EventArgs e)
        {
            try
            {
                //显示添加非交易日期窗体
                var notTradeDateManagerUI = new NotTradeDateManagerUI();
                notTradeDateManagerUI.ShowDialog();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4446";
                string errMsg = "显示添加非交易日期窗体失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

            }
        }

        #endregion

        #region  添加交易时间 btnAddTradeTime_Click
        /// <summary>
        /// 添加交易时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTradeTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    DataRow _dr = _dtTradeTime.NewRow();
                    if (!string.IsNullOrEmpty(this.txtBourseTypeName.Text))
                    {
                        _dr[gdvTradeTimeSelect.Columns[0].FieldName] = this.txtBourseTypeName.Text; //BourseTypeName;
                    }
                    if ((DateTime)tmTradeStartTime.EditValue < (DateTime)tmTradeEndTime.EditValue)
                    {
                        _dr[gdvTradeTimeSelect.Columns[1].FieldName] = tmTradeStartTime.Text;
                        _dr[gdvTradeTimeSelect.Columns[2].FieldName] = tmTradeEndTime.Text;
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("开始时间需小于结束时间!");
                        return;
                    }

                    if (_dtTradeTime.Rows.Count == 0)
                    {
                        _dtTradeTime.Rows.Add(_dr);
                    }
                    else
                    {
                        DateTime tradeStartTime = (DateTime)tmTradeStartTime.EditValue;
                        string startTime = tradeStartTime.ToString("HH:mm");
                        DateTime dtTStartTime = Convert.ToDateTime(startTime);//转换成当前日期的时间

                        DateTime tradeEndTime = (DateTime)tmTradeEndTime.EditValue;
                        string endTime = tradeEndTime.ToString("HH:mm");
                        DateTime dtTEndTime = Convert.ToDateTime(endTime);//转换成当前日期的时间

                        for (int i = 0; i < _dtTradeTime.Rows.Count; i++)
                        {
                            i = m_IsCompareRow;
                            DateTime _isstartTime = Convert.ToDateTime(_dtTradeTime.Rows[i][this.gdvTradeTimeSelect.Columns[1].FieldName]);
                            string isStartTime = _isstartTime.ToString("HH:mm");
                            DateTime dtInDrStartTime = Convert.ToDateTime(isStartTime);//转换成当前日期的时间

                            DateTime _isendTime = Convert.ToDateTime(_dtTradeTime.Rows[i][this.gdvTradeTimeSelect.Columns[2].FieldName]);
                            string isEndTime = _isendTime.ToString("HH:mm");
                            DateTime dtInDrEndTime = Convert.ToDateTime(isEndTime);//转换成当前日期的时间
                            if (dtTEndTime < dtInDrStartTime)
                            {
                                _dtTradeTime.Rows.Add(_dr);
                            }
                            else
                            {
                                if ((dtTEndTime < dtInDrEndTime) || (dtTEndTime == dtInDrEndTime))
                                {
                                    ShowMessageBox.ShowInformation("开始时间和结束时间的时间区间不能重复!");
                                    return;
                                }
                                else
                                {
                                    if (dtTStartTime > dtInDrEndTime)
                                    {
                                        _dtTradeTime.Rows.Add(_dr);
                                    }
                                    else
                                    {
                                        ShowMessageBox.ShowInformation("开始时间和结束时间的时间区间不能重复!");
                                        return;
                                    }
                                }
                            }
                            m_IsCompareRow++;
                            if (m_IsCompareRow == _dtTradeTime.Rows.Count - 1)//记录比较的行数=当前的总行数-1时退出循环
                            {
                                break;
                            }
                        }

                    }
                    gdTradeTimeResult.DataSource = _dtTradeTime;
                }
                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)//直接添加的数据库中
                {
                    if ((DateTime)tmTradeStartTime.EditValue < (DateTime)tmTradeEndTime.EditValue)
                    {
                        var cM_TradeTime = new CM_TradeTime();
                        if (m_BourseTypeID != AppGlobalVariable.INIT_INT)
                        {
                            cM_TradeTime.BourseTypeID = m_BourseTypeID;
                        }
                        else
                        {
                            cM_TradeTime.BourseTypeID = AppGlobalVariable.INIT_INT;
                        }
                        cM_TradeTime.StartTime = Convert.ToDateTime(tmTradeStartTime.Text);
                        cM_TradeTime.EndTime = Convert.ToDateTime(tmTradeEndTime.Text);

                        //根据交易所类型ID获取相关交易时间
                        List<CM_TradeTime> cmTradeTimeList =
                            CommonParameterSetCommon.GetCMTradeTimeListArray(string.Format("BourseTypeID={0}",
                                                                                           m_BourseTypeID));
                        if (cmTradeTimeList.Count > 0)
                        {
                            DateTime tradeStartTime = (DateTime)tmTradeStartTime.EditValue;
                            string startTime = tradeStartTime.ToString("HH:mm");
                            DateTime dtTStartTime = Convert.ToDateTime(startTime); //转换成当前日期的时间

                            DateTime tradeEndTime = (DateTime)tmTradeEndTime.EditValue;
                            string endTime = tradeEndTime.ToString("HH:mm");
                            DateTime dtTEndTime = Convert.ToDateTime(endTime); //转换成当前日期的时间

                            for (int i = 0; i < cmTradeTimeList.Count; i++)
                            {
                                i = m_IsCompareRow;
                                CM_TradeTime _cmTradeTime = cmTradeTimeList[i];
                                DateTime _isstartTime = Convert.ToDateTime(_cmTradeTime.StartTime);
                                string isStartTime = _isstartTime.ToString("HH:mm");
                                DateTime dtInDrStartTime = Convert.ToDateTime(isStartTime); //转换成当前日期的时间
                                DateTime _isendTime = Convert.ToDateTime(_cmTradeTime.EndTime);
                                string isEndTime = _isendTime.ToString("HH:mm");
                                DateTime dtInDrEndTime = Convert.ToDateTime(isEndTime); //转换成当前日期的时间
                                if (!(dtTEndTime < dtInDrStartTime))
                                {
                                    if ((dtTEndTime < dtInDrEndTime) || (dtTEndTime == dtInDrEndTime))
                                    {
                                        ShowMessageBox.ShowInformation("开始时间和结束时间的时间区间不能重复!");
                                        return;
                                    }
                                    else
                                    {
                                        if (!(dtTStartTime > dtInDrEndTime))
                                        {
                                            ShowMessageBox.ShowInformation("开始时间和结束时间的时间区间不能重复!");
                                            return;
                                        }
                                    }
                                }
                                m_IsCompareRow++;
                                if (m_IsCompareRow == cmTradeTimeList.Count) //记录比较的行数=当前的总行数时退出循环
                                {
                                    int result = CommonParameterSetCommon.AddCMTradeTime(cM_TradeTime);//, ref msg);
                                    if (result != AppGlobalVariable.INIT_INT)
                                    {
                                        ShowMessageBox.ShowInformation("交易时间添加成功!");
                                        ClearAll();
                                        m_IsCompareRow = 0;
                                        QueryCMTradeTime();
                                    }
                                    else
                                    {
                                        ShowMessageBox.ShowInformation("交易时间添加失败!");
                                        m_IsCompareRow = 0;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("开始时间需小于结束时间!");
                        return;
                    }



                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4441";
                string errMsg = "添加交易所类型_交易时间失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;

            }
        }
        #endregion

        #region  删除交易时间 btnDeleteTradeTime_Click
        /// <summary>
        /// 删除交易时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteTradeTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdvTradeTimeSelect.DataSource != null && gdvTradeTimeSelect.RowCount > 0)
                {
                    if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                    {
                        if (_dtTradeTime != null || _dtTradeTime.Rows.Count > 0)
                        {
                            _dtTradeTime.Rows.Remove(gdvTradeTimeSelect.GetDataRow(gdvTradeTimeSelect.FocusedRowHandle));
                            gdTradeTimeResult.DataSource = _dtTradeTime;
                        }
                    }
                    else if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                    {
                        if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                        DataRow _dr = gdvTradeTimeSelect.GetDataRow(gdvTradeTimeSelect.FocusedRowHandle);
                        if (_dr == null)
                        {
                            ShowMessageBox.ShowInformation("请选择数据!");
                            return;
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(_dr["TradeTimeID"])))
                        {
                            m_TradeTimeID = Convert.ToInt32(_dr["TradeTimeID"]);
                        }
                        else
                        {
                            m_TradeTimeID = AppGlobalVariable.INIT_INT;
                        }
                        if (m_TradeTimeID != AppGlobalVariable.INIT_INT)
                        {
                            m_Result = CommonParameterSetCommon.DeleteCMTradeTime(m_TradeTimeID);
                        }
                        if (m_Result)
                        {
                            ShowMessageBox.ShowInformation("交易时间删除成功!");
                            m_TradeTimeID = AppGlobalVariable.INIT_INT;
                        }
                        else
                        {
                            ShowMessageBox.ShowInformation("交易时间删除失败!");
                        }
                        QueryCMTradeTime();
                    }
                    if (m_IsCompareRow != 0)
                    {
                        m_IsCompareRow--;//执行一次删除，已经比较的记录就减一次
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4442";
                string errMsg = "删除交易所类型_交易时间失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 保存交易的类型和交易时间到数据库 btnOk_Click
        /// <summary>
        /// 保存交易的类型和交易时间到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    if (!string.IsNullOrEmpty(this.txtBourseTypeName.Text))
                    {
                        if (!CommonParameterSetCommon.IsExistBourseTypeName(this.txtBourseTypeName.Text))
                        {
                            ShowMessageBox.ShowInformation("交易所名称已经存在!");
                            return;
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("交易所名称不能为空!");
                        return;
                    }
                }
                //交易所类型
                CM_BourseType cM_BourseType = new CM_BourseType();
                if (CMBourseType != null)
                {
                    ManagementCenter.Model.CommonClass.UtilityClass.CopyEntityToEntity(CMBourseType, cM_BourseType);
                    cM_BourseType.BourseTypeName = this.txtBourseTypeName.Text;
                    if ((DateTime)tmCounFromSubmitStartTime.EditValue < (DateTime)tmCounFromSubmitEndTime.EditValue)
                    {
                        cM_BourseType.CounterFromSubmitStartTime =
                            Convert.ToDateTime(this.tmCounFromSubmitStartTime.Text);
                        cM_BourseType.CounterFromSubmitEndTime = Convert.ToDateTime(this.tmCounFromSubmitEndTime.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("开始时间需小于结束时间!");
                        return;

                    }
                    cM_BourseType.ISSysDefaultBourseType = (int)GTA.VTS.Common.CommonObject.Types.IsYesOrNo.No;//用户添加的交易所不是系统默认交易所
                }
                if (EditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    List<CM_TradeTime> cmTradeTimeList = new List<CM_TradeTime>();
                    if (_dtTradeTime != null && _dtTradeTime.Rows.Count > 0)
                    {
                        foreach (DataRow _dr in _dtTradeTime.Rows)
                        { //交易时间
                            CM_TradeTime cM_TradeTime = new CM_TradeTime();

                            //cM_TradeTime.BourseTypeID =;
                            cM_TradeTime.StartTime = Convert.ToDateTime(_dr[this.gdvTradeTimeSelect.Columns[1].FieldName]);
                            cM_TradeTime.EndTime = Convert.ToDateTime(_dr[this.gdvTradeTimeSelect.Columns[2].FieldName]);
                            cmTradeTimeList.Add(cM_TradeTime);

                        }

                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请填写交易时间!");
                        return;
                    }

                    int _BourseTypeMaxID = CommonParameterSetCommon.GetCMBourseTypeMaxId();
                    if (_BourseTypeMaxID != AppGlobalVariable.INIT_INT)
                    {
                        if (_BourseTypeMaxID > 100) //交易所类型表中的最大ID大于系统默认的ID，100时
                        {
                            cM_BourseType.BourseTypeID = _BourseTypeMaxID; //因为在DAL层返回的最大ID已经加1
                        }
                        else
                        {
                            cM_BourseType.BourseTypeID = 100 + 1;
                        }
                    }
                    int result = CommonParameterSetCommon.AddCMBourseTypeAndTradeTime(cM_BourseType, cmTradeTimeList);
                    if (result != AppGlobalVariable.INIT_INT)
                    {
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
                else if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)//此处只修改交易所类型
                {
                    bool _Result = CommonParameterSetCommon.UpdateCMBourseType(cM_BourseType);//, ref msg);
                    if (_Result)
                    {
                        ShowMessageBox.ShowInformation("交易所类型修改成功!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("交易所类型修改失败!");
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4447";
                string errMsg = "保存交易的类型和交易时间到数据库失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 修改交易时间(修改数据库中已存在的记录) btnUpdateTradeTime_Click
        /// <summary>
        /// 修改交易时间(修改数据库中已存在的记录)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateTradeTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdvTradeTimeSelect.DataSource != null && gdvTradeTimeSelect.RowCount > 0)
                {
                    var cM_TradeTime = new CM_TradeTime();
                    if (m_TradeTimeID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    DataRow _dr = gdvTradeTimeSelect.GetDataRow(gdvTradeTimeSelect.FocusedRowHandle);
                    int tradeTimeID = Convert.ToInt32(_dr["TradeTimeID"]);//交易时间ID
                    cM_TradeTime.TradeTimeID = m_TradeTimeID;
                    cM_TradeTime.BourseTypeID = m_BourseTypeID;
                    if ((DateTime)tmTradeStartTime.EditValue < (DateTime)tmTradeEndTime.EditValue)
                    {
                        cM_TradeTime.StartTime = Convert.ToDateTime(tmTradeStartTime.Text);
                        cM_TradeTime.EndTime = Convert.ToDateTime(tmTradeEndTime.Text);

                        //根据交易所类型ID获取相关交易时间
                        List<CM_TradeTime> cmTradeTimeList =
                            CommonParameterSetCommon.GetCMTradeTimeListArray(string.Format("BourseTypeID={0}",
                                                                                           m_BourseTypeID));
                        if (cmTradeTimeList.Count > 0)
                        {
                            DateTime tradeStartTime = (DateTime)tmTradeStartTime.EditValue;
                            string startTime = tradeStartTime.ToString("HH:mm");
                            DateTime dtTStartTime = Convert.ToDateTime(startTime); //转换成当前日期的时间

                            DateTime tradeEndTime = (DateTime)tmTradeEndTime.EditValue;
                            string endTime = tradeEndTime.ToString("HH:mm");
                            DateTime dtTEndTime = Convert.ToDateTime(endTime); //转换成当前日期的时间

                            for (int i = 0; i < cmTradeTimeList.Count; i++)
                            {
                                i = m_IsCompareRow;
                                if (cmTradeTimeList[i].TradeTimeID == tradeTimeID)//不与自己比较
                                {
                                    m_IsCompareRow++;
                                    if (m_IsCompareRow == cmTradeTimeList.Count) //记录比较的行数=当前的总行数时退出循环
                                    {
                                        string msg = string.Empty;
                                        m_Result = CommonParameterSetCommon.UpdateCMTradeTime(cM_TradeTime);//, ref msg);
                                        if (m_Result)
                                        {
                                            ShowMessageBox.ShowInformation("交易时间修改成功!");
                                            m_IsCompareRow = 0;
                                            ClearAll();
                                        }
                                        else
                                        {
                                            ShowMessageBox.ShowInformation("交易时间修改失败!");
                                        }
                                        QueryCMTradeTime();
                                        break;
                                    }
                                    continue;
                                }
                                CM_TradeTime _cmTradeTime = cmTradeTimeList[i];
                                DateTime _isstartTime = Convert.ToDateTime(_cmTradeTime.StartTime);
                                string isStartTime = _isstartTime.ToString("HH:mm");
                                DateTime dtInDrStartTime = Convert.ToDateTime(isStartTime); //转换成当前日期的时间
                                DateTime _isendTime = Convert.ToDateTime(_cmTradeTime.EndTime);
                                string isEndTime = _isendTime.ToString("HH:mm");
                                DateTime dtInDrEndTime = Convert.ToDateTime(isEndTime); //转换成当前日期的时间
                                if (!(dtTEndTime < dtInDrStartTime))
                                {
                                    if ((dtTEndTime < dtInDrEndTime) || (dtTEndTime == dtInDrEndTime))
                                    {
                                        ShowMessageBox.ShowInformation("开始时间和结束时间的时间区间不能重复!");
                                        return;
                                    }
                                    else
                                    {
                                        if (!(dtTStartTime > dtInDrEndTime))
                                        {
                                            ShowMessageBox.ShowInformation("开始时间和结束时间的时间区间不能重复!");
                                            return;
                                        }
                                    }
                                }
                                m_IsCompareRow++;
                                if (m_IsCompareRow == cmTradeTimeList.Count) //记录比较的行数=当前的总行数时退出循环
                                {
                                    string msg = string.Empty;
                                    m_Result = CommonParameterSetCommon.UpdateCMTradeTime(cM_TradeTime);//, ref msg);
                                    if (m_Result)
                                    {
                                        ShowMessageBox.ShowInformation("修改成功!");
                                        m_IsCompareRow = 0;
                                        ClearAll();
                                        QueryCMTradeTime();
                                    }
                                    else
                                    {
                                        //MessageBox.Show("修改失败!", "系统提示");
                                        ShowMessageBox.ShowInformation("修改失败!");
                                        m_IsCompareRow = 0;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("开始时间需小于结束时间!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-4443";
                string errMsg = "修改交易所类型_交易时间失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 交易时间取消事件 btnCancelTradeTime_Click
        /// <summary>
        /// 交易时间取消事件 btnCancelTradeTime_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelTradeTime_Click(object sender, EventArgs e)
        {
            this.ClearAll();

        }
        #endregion
    }
}