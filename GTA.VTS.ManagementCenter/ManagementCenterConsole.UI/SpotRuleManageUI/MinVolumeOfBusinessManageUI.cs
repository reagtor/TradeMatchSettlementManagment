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

namespace ManagementCenterConsole.UI.SpotRuleManageUI
{
    /// <summary>
    /// 描述：现货最小交易单位管理窗体  错误编码范围:5080-5099
    /// 作者：刘书伟
    /// 日期：2008-12-05  2009-7-31
    /// 修改：叶振东
    /// 日期：2010-04-02
    /// 描述：修改现货最小交易单位管理窗体操作的使用方法
    /// </summary>
    public partial class MinVolumeOfBusinessManageUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MinVolumeOfBusinessManageUI()
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

        #region 保存操作状态
        /// <summary>
        /// 操作状态 1：添加  2：修改
        /// </summary>
        private int Status = 0;
        #endregion 保存操作状态

        ///// <summary>
        ///// 品种ID
        ///// </summary>
        //private int m_BreedClassID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 最小交易单位ID
        /// </summary>
        private int m_MinVolumeOfBusinessID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 结果变量
        /// </summary>
        private bool m_Result = false;

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

        #endregion

        //================================  私有  方法 ================================

        #region 根据现货规则表和港股规则表中的品种标识获取品种名称

        /// <summary>
        /// 根据现货规则表和港股规则表中的品种标识获取品种名称
        /// </summary>
        private void GetXHAndHKBindBreedClassName()
        {
            DataSet ds = SpotManageCommon.GetXHAndHKBreedClassNameByBreedClassID();//GetBreedClassNameByBreedClassID();
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
            //this.cmbTradeWayID.Text = string.Empty;
            //this.cmbUnitID.Text = string.Empty;
            this.txtVolumeOfBusiness.Text = string.Empty;
        }
        /// <summary>
        /// 启用文本框编辑
        /// </summary>
        private void EnabledTrue()
        {
            this.cmbBreedClassID.Enabled = true;
            this.txtVolumeOfBusiness.Enabled = true;
            this.cmbTradeWayID.Enabled = true;
            this.cmbUnitID.Enabled = true;
        }
        /// <summary>
        /// 禁用文本框编辑
        /// </summary>
        private void EnabledFalse()
        {
            this.cmbBreedClassID.Enabled = false;
            this.txtVolumeOfBusiness.Enabled = false;
            this.cmbTradeWayID.Enabled = false;
            this.cmbUnitID.Enabled = false;
        }
        #endregion

        #region 显示数据页数

        /// <summary>
        /// 显示数据页数
        /// </summary>
        private void ShowDataPage()
        {
            if (m_rowCount != AppGlobalVariable.INIT_INT)
            {
                if (m_rowCount == 0)
                {
                    this.UCPageNavig.PageCount = 0;
                }
                else
                {
                    if (m_rowCount % this.m_pageSize == 0)
                    {
                        this.UCPageNavig.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize);
                    }
                    else
                    {
                        this.UCPageNavig.PageCount = Convert.ToInt32(m_rowCount / this.m_pageSize) + 1;
                    }
                }

                this.UCPageNavig.CurrentPage = this.m_pageNo;
            }
        }

        #endregion

        #region 根据查询条件，获取现货最小交易单位

        /// <summary>
        /// 根据查询条件，获取现货最小交易单位
        /// </summary>
        /// <returns></returns>
        private bool QueryMinVolumeOfBusiness()
        {
            try
            {
                string BreedClassName = this.txtBreedClassName.Text;
                DataSet _dsMinVolumeOfBus = SpotManageCommon.GetAllXHMinVolumeOfBusiness(BreedClassName, m_pageNo,
                                                                                         m_pageSize,
                                                                                         out m_rowCount);
                DataTable _dtMinVolumeOfBus;
                if (_dsMinVolumeOfBus == null || _dsMinVolumeOfBus.Tables[0].Rows.Count == 0)
                {
                    _dtMinVolumeOfBus = new DataTable();
                }
                else
                {
                    _dtMinVolumeOfBus = _dsMinVolumeOfBus.Tables[0];
                }
                this.gdMinVolumeOfBusResult.DataSource = _dtMinVolumeOfBus;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5086";
                string errMsg = "根据查询条件，获取现货最小交易单位失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }

            return true;
        }

        #endregion

        #region 获取需要修改的现货最小交易单位数据

        /// <summary>
        /// 获取需要修改的现货最小交易单位数据
        /// </summary>
        private void ModifyMinVolumeOfBusiness()
        {
            try
            {
                if (this.gdvMinVolumeOfBusSelect != null && this.gdvMinVolumeOfBusSelect.DataSource != null &&
                    this.gdvMinVolumeOfBusSelect.RowCount > 0 && this.gdvMinVolumeOfBusSelect.FocusedRowHandle >= 0)
                {
                    btnModify.Enabled = true;
                    DataRow _dr = this.gdvMinVolumeOfBusSelect.GetDataRow(this.gdvMinVolumeOfBusSelect.FocusedRowHandle);
                    m_MinVolumeOfBusinessID = Convert.ToInt32(_dr["MinVolumeOfBusinessID"]);
                    foreach (object item in cmbBreedClassID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["BreedClassID"].ToString()))
                        {
                            cmbBreedClassID.SelectedItem = item;
                            break;
                        }
                    }
                    foreach (object item in cmbTradeWayID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["TradeWayID"].ToString()))
                        {
                            cmbTradeWayID.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (object item in cmbUnitID.Properties.Items)
                    {
                        if (((UComboItem)item).ValueIndex == int.Parse(_dr["UnitID"].ToString()))
                        {
                            cmbUnitID.SelectedItem = item;
                            break;
                        }
                    }
                    txtVolumeOfBusiness.Text = Convert.ToString(_dr["VolumeOfBusiness"]);
                    this.EnabledFalse();
                    this.btnAdd.Text = "添加";
                    this.btnModify.Text = "修改";
                    this.btnOK.Enabled = false;
                    this.btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5083";
                string errMsg = "获取需要修改的现货最小交易单位数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        //================================  事件 ================================

        #region 现货最小交易单位管理窗体 MinVolumeOfBusinessManageUI_Load

        /// <summary>
        /// 现货最小交易单位管理窗体 MinVolumeOfBusinessManageUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinVolumeOfBusinessManageUI_Load(object sender, EventArgs e)
        {
            try
            {
                //初次加载时，更新按钮禁用
                //btnModify.Enabled = false;
                this.cmbBreedClassID.Properties.Items.Clear();
                this.GetXHAndHKBindBreedClassName(); //根据现货规则表和港股规则表中的品种标识获取品种名称
                this.cmbBreedClassID.SelectedIndex = 0;
                this.cmbTradeWayID.Properties.Items.Clear();
                this.cmbTradeWayID.Properties.Items.AddRange(BindData.GetBindListTransDire()); //绑定交易方向
                this.cmbTradeWayID.SelectedIndex = 0;
                this.cmbUnitID.Properties.Items.Clear();
                this.cmbUnitID.Properties.Items.AddRange(BindData.GetBindListXHAboutUnit());//GetBindListUnitType()); //绑定单位类型
                this.cmbUnitID.SelectedIndex = 0;

                //绑定交易方向
                this.ddlTradeWayID.DataSource = BindData.GetBindListTransDire();
                this.ddlTradeWayID.ValueMember = "ValueIndex";
                this.ddlTradeWayID.DisplayMember = "TextTitleValue";

                //绑定交易单位
                this.ddlUnitID.DataSource = BindData.GetBindListUnitType();
                this.ddlUnitID.ValueMember = "ValueIndex";
                this.ddlUnitID.DisplayMember = "TextTitleValue";

                //绑定品种名称
                this.ddlBreedClassID.DataSource = SpotManageCommon.GetXHAndHKBreedClassNameByBreedClassID().Tables[0];
                this.ddlBreedClassID.ValueMember =
                    SpotManageCommon.GetXHAndHKBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassID"].ToString();
                this.ddlBreedClassID.DisplayMember =
                    SpotManageCommon.GetXHAndHKBreedClassNameByBreedClassID().Tables[0].Columns["BreedClassName"].ToString();


                //绑定查询结果
                this.m_pageNo = 1;
                this.gdMinVolumeOfBusResult.DataSource = this.QueryMinVolumeOfBusiness();
                this.ShowDataPage();

                //禁用界面上输入框
                this.cmbBreedClassID.Enabled = false;
                this.txtVolumeOfBusiness.Enabled = false;
                this.cmbTradeWayID.Enabled = false;
                this.cmbUnitID.Enabled = false;
                this.btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-5080";
                string errMsg = "现货最小交易单位管理窗体加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 添加现货最小交易单位

        /// <summary>
        /// 添加现货最小交易单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string Name = this.btnAdd.Text.ToString();
            if (Name.Equals("添加"))
            {
                this.btnAdd.Text = "取消";
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnOK.Enabled = true;
                Status = 1;
                this.EnabledTrue();
                this.txtVolumeOfBusiness.Text = string.Empty;
            }
            else
            {
                this.btnAdd.Text = "添加";
                this.btnModify.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnOK.Enabled = false;
                Status = 0;
                this.EnabledFalse();
            }
            
        }
        #endregion

        #region 根据条件查询最小交易单位  btnQuery_Click

        /// <summary>
        /// 根据条件查询最小交易单位  btnQuery_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_pageNo = 1; //设当前页是第一页
                this.QueryMinVolumeOfBusiness(); //绑定数据源
                this.ShowDataPage(); //显示数据分页

                //DataTable _dtMinVolumeOfB = (DataTable) this.gdMinVolumeOfBusResult.DataSource;
                DataView _dvMinVolumeOfB = (DataView)this.gdvMinVolumeOfBusSelect.DataSource;
                DataTable _dtMinVolumeOfB = _dvMinVolumeOfB.Table;
                if (_dtMinVolumeOfB == null || _dtMinVolumeOfB.Rows.Count == 0)
                {
                    MessageBox.Show("没有你要查找的数据,请重新选择!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-5085";
                string errMsg = "根据条件查询最小交易单位失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion

        #region 分页控件事件 UCPageNavig_PageIndexChanged

        /// <summary>
        /// 分页控件事件 UCPageNavig_PageIndexChanged
        /// </summary>
        /// <param name="page"></param>
        private void UCPageNavig_PageIndexChanged(int page)
        {
            if (m_pageNo != page)
            {
                this.m_pageNo = page;
                this.QueryMinVolumeOfBusiness();
            }
        }

        #endregion

        #region 删除现货最小交易单位事件 btnDelete_Click

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMessageBox.ShowQuestion("确认删除吗？") == DialogResult.No) return;
                DataRow _dr = this.gdvMinVolumeOfBusSelect.GetDataRow(this.gdvMinVolumeOfBusSelect.FocusedRowHandle);
                if (_dr == null)
                {
                    ShowMessageBox.ShowInformation("请选择数据!");
                    return;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_dr["MinVolumeOfBusinessID"])))
                {
                    m_MinVolumeOfBusinessID = Convert.ToInt32(_dr["MinVolumeOfBusinessID"]);
                }
                else
                {
                    m_MinVolumeOfBusinessID = AppGlobalVariable.INIT_INT;
                }
                if (m_MinVolumeOfBusinessID != AppGlobalVariable.INIT_INT)
                {
                    m_Result = SpotManageCommon.DeleteXHMinVolumeOfBusByID(m_MinVolumeOfBusinessID);
                }
                if (m_Result)
                {
                    ShowMessageBox.ShowInformation("删除成功!");
                    m_MinVolumeOfBusinessID = AppGlobalVariable.INIT_INT;
                }
                else
                {
                    ShowMessageBox.ShowInformation("删除失败!");
                }
                this.QueryMinVolumeOfBusiness();
            }
            catch (Exception ex)
            {
                string errCode = "GL-5082";
                string errMsg = "删除现货最小交易单位失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 修改现货最小交易单位事件 btnModify_Click

        /// <summary>
        /// 修改现货最小交易单位事件 btnModify_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            string Name = this.btnModify.Text.ToString();
            if (Name.Equals("修改"))
            {
                this.btnModify.Text = "取消";
                this.btnAdd.Enabled = false;
                this.btnDelete.Enabled = false;
                this.btnOK.Enabled = true;
                Status = 2;
                this.EnabledTrue();
            }
            else
            {
                this.btnModify.Text = "修改";
                this.btnAdd.Enabled = true;
                this.btnDelete.Enabled = true;
                this.btnOK.Enabled = false;
                Status = 0;
                this.EnabledFalse();
            }
            
        }

        #endregion

        #region 修改现货最小交易单位数据 gdMinVolumeOfBusResult_DoubleClick

        /// <summary>
        /// 修改现货最小交易单位数据 gdMinVolumeOfBusResult_DoubleClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdMinVolumeOfBusResult_DoubleClick(object sender, EventArgs e)
        {
            this.ModifyMinVolumeOfBusiness();
        }

        #endregion

        #region 现货最小交易单位数据输入检测
        /// <summary>
        /// 现货最小交易单位数据输入检测
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        private XH_MinVolumeOfBusiness VerifyXHMinVolumeOfBusInput(ref string msg)
        {
            try
            {
                msg = string.Empty;
                XH_MinVolumeOfBusiness xH_MinVolumeOfBusiness = new XH_MinVolumeOfBusiness();

                if (!string.IsNullOrEmpty(this.txtVolumeOfBusiness.Text))
                {
                    if (InputTest.intTest(this.txtVolumeOfBusiness.Text))
                    {
                        xH_MinVolumeOfBusiness.VolumeOfBusiness =
                            Convert.ToInt32(this.txtVolumeOfBusiness.Text);
                    }
                    else
                    {
                        msg = "请输入数字且第一位数不能为0!";
                    }
                }
                else
                {
                    msg = "最小交易量不能为空!";
                }
                xH_MinVolumeOfBusiness.BreedClassID =
                    ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                xH_MinVolumeOfBusiness.UnitID =
                    ((UComboItem)this.cmbUnitID.SelectedItem).ValueIndex;
                xH_MinVolumeOfBusiness.TradeWayID =
                    ((UComboItem)this.cmbTradeWayID.SelectedItem).ValueIndex;
                if (EditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    xH_MinVolumeOfBusiness.MinVolumeOfBusinessID = m_MinVolumeOfBusinessID;
                }
                return xH_MinVolumeOfBusiness;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return null;
            }
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
            this.ClearAll();
        }
        #endregion

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //获取状态值，并对状态值进行比较 1代表添加操作，2代表修改操作
            int status = Status;
            if(status==1)
            {
                #region
                EditType = (int)UITypes.EditTypeEnum.AddUI;
                int result = AppGlobalVariable.INIT_INT;
                try
                {
                    XH_MinVolumeOfBusiness xH_MinVolumeOfBusiness = new XH_MinVolumeOfBusiness();
                    int breedClassID = AppGlobalVariable.INIT_INT; //品种ID
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        breedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                        if (breedClassID != AppGlobalVariable.INIT_INT)
                        {
                            List<XH_MinVolumeOfBusiness> xhMinVolumeOfBusList =
                                SpotManageCommon.GetXHMinVolumeOfBusByBreedClassID(breedClassID);
                            if (xhMinVolumeOfBusList.Count > 0)
                            {
                                int _curRow = 0; //当前记录行
                                foreach (XH_MinVolumeOfBusiness _xHMinVOfBus in xhMinVolumeOfBusList)
                                {
                                    _curRow++;
                                    if (_xHMinVOfBus.BreedClassID ==
                                        ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex)
                                    {
                                        if (_xHMinVOfBus.UnitID == ((UComboItem)this.cmbUnitID.SelectedItem).ValueIndex &&
                                            _xHMinVOfBus.TradeWayID ==
                                            ((UComboItem)this.cmbTradeWayID.SelectedItem).ValueIndex)
                                        {
                                            ShowMessageBox.ShowInformation("同一品种，同一交易方向，同一交易单位只允许一条记录!");
                                            break;
                                        }
                                        else
                                        {
                                            if (_curRow == xhMinVolumeOfBusList.Count)
                                            {
                                                string msg = string.Empty;
                                                xH_MinVolumeOfBusiness = VerifyXHMinVolumeOfBusInput(ref msg);
                                                if (!string.IsNullOrEmpty(msg))
                                                {
                                                    ShowMessageBox.ShowInformation(msg);
                                                }
                                                else
                                                {
                                                    result =
                                                        SpotManageCommon.AddXHMinVolumeOfBusiness(xH_MinVolumeOfBusiness);
                                                    if (result != AppGlobalVariable.INIT_INT)
                                                    {
                                                        ShowMessageBox.ShowInformation("添加成功!");
                                                        this.ClearAll();
                                                        this.QueryMinVolumeOfBusiness();
                                                    }
                                                    else
                                                    {
                                                        ShowMessageBox.ShowInformation("添加失败!");
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                string msg = string.Empty;
                                xH_MinVolumeOfBusiness = VerifyXHMinVolumeOfBusInput(ref msg);
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    ShowMessageBox.ShowInformation(msg);
                                }
                                else
                                {
                                    result = SpotManageCommon.AddXHMinVolumeOfBusiness(xH_MinVolumeOfBusiness);
                                    if (result != AppGlobalVariable.INIT_INT)
                                    {
                                        ShowMessageBox.ShowInformation("添加成功!");
                                        this.ClearAll();
                                        this.QueryMinVolumeOfBusiness();
                                    }
                                    else
                                    {
                                        ShowMessageBox.ShowInformation("添加失败!");
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    string errCode = "GL-5081";
                    string errMsg = "添加现货最小交易单位失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                    return;
                }
                #endregion
            }
            else if(status==2)
            {
                #region
                try
                {
                    EditType = (int)UITypes.EditTypeEnum.UpdateUI;
                    XH_MinVolumeOfBusiness xH_MinVolumeOfBusiness = new XH_MinVolumeOfBusiness();
                    int breedClassID = AppGlobalVariable.INIT_INT; //品种ID
                    if (m_MinVolumeOfBusinessID == AppGlobalVariable.INIT_INT)
                    {
                        ShowMessageBox.ShowInformation("请选择更新数据!");
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                    {
                        breedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                        if (breedClassID != AppGlobalVariable.INIT_INT)
                        {
                            List<XH_MinVolumeOfBusiness> xhMinVolumeOfBusList =
                                SpotManageCommon.GetXHMinVolumeOfBusByBreedClassID(breedClassID);
                            if (xhMinVolumeOfBusList.Count > 0)
                            {
                                int _curRow = 0; //当前记录行
                                foreach (XH_MinVolumeOfBusiness _xHMinVOfBus in xhMinVolumeOfBusList)
                                {
                                    if (m_MinVolumeOfBusinessID == _xHMinVOfBus.MinVolumeOfBusinessID)//不与自己比较
                                    {
                                        _curRow++;
                                        if (_curRow == xhMinVolumeOfBusList.Count)
                                        {
                                            string msg = string.Empty;
                                            xH_MinVolumeOfBusiness = VerifyXHMinVolumeOfBusInput(ref msg);
                                            if (!string.IsNullOrEmpty(msg))
                                            {
                                                ShowMessageBox.ShowInformation(msg);
                                            }
                                            else
                                            {
                                                m_Result = SpotManageCommon.UpdateXHMinVolumeOfBus(xH_MinVolumeOfBusiness);
                                                if (m_Result)
                                                {
                                                    ShowMessageBox.ShowInformation("修改成功!");
                                                    this.ClearAll();
                                                    this.QueryMinVolumeOfBusiness();
                                                    m_MinVolumeOfBusinessID = AppGlobalVariable.INIT_INT;
                                                }
                                                else
                                                {
                                                    ShowMessageBox.ShowInformation("修改失败!");
                                                }
                                            }
                                            break;
                                        }
                                        continue;
                                    }
                                    _curRow++;
                                    if (_xHMinVOfBus.BreedClassID ==
                                        ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex)
                                    {
                                        if (_xHMinVOfBus.UnitID == ((UComboItem)this.cmbUnitID.SelectedItem).ValueIndex &&
                                            _xHMinVOfBus.TradeWayID ==
                                            ((UComboItem)this.cmbTradeWayID.SelectedItem).ValueIndex)
                                        {
                                            ShowMessageBox.ShowInformation("同一品种，同一交易方向，同一交易单位只允许一条记录!");
                                            break;
                                        }
                                        else
                                        {
                                            if (_curRow == xhMinVolumeOfBusList.Count)
                                            {
                                                string msg = string.Empty;
                                                xH_MinVolumeOfBusiness = VerifyXHMinVolumeOfBusInput(ref msg);
                                                if (!string.IsNullOrEmpty(msg))
                                                {
                                                    ShowMessageBox.ShowInformation(msg);
                                                }
                                                else
                                                {
                                                    m_Result = SpotManageCommon.UpdateXHMinVolumeOfBus(xH_MinVolumeOfBusiness);
                                                    if (m_Result)
                                                    {
                                                        ShowMessageBox.ShowInformation("修改成功!");
                                                        this.ClearAll();
                                                        this.QueryMinVolumeOfBusiness();
                                                        m_MinVolumeOfBusinessID = AppGlobalVariable.INIT_INT;

                                                    }
                                                    else
                                                    {
                                                        ShowMessageBox.ShowInformation("修改失败!");
                                                    }
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errCode = "GL-5084";
                    string errMsg = "修改现货最小交易单位失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);
                }
                #endregion
            }
            this.btnAdd.Enabled = true;
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnOK.Enabled = false;
            this.btnAdd.Text = "添加";
            this.btnModify.Text = "修改";
            this.EnabledFalse();
            Status = 0;
        }


    }
}