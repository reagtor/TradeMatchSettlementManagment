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
    /// 描述：添加期货最小和最大委托量 错误编码范围:5860-5869
    /// 作者：刘书伟
    /// 日期：2008-12-08
    /// </summary>
    public partial class AddMinAndMaxConsignQuantumUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        public AddMinAndMaxConsignQuantumUI()
        {
            InitializeComponent();
        }
        #endregion

        #region 变量及属性

        //结果变量
        private int m_Result = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 交易规则委托量标识
        /// </summary>
        private int _ConsignQuantumID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 交易规则委托量标识属性
        /// </summary>
        public int ConsignQuantumID
        {
            set { _ConsignQuantumID = value; }
            get { return _ConsignQuantumID; }
        }

        #region 操作类型　 1:添加,2:修改
        private int m_MinAndMaxConsignQuantumUIEditType = (int)UITypes.EditTypeEnum.AddUI;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int MinAndMaxConsignQuantumUIEditType
        {
            get { return this.m_MinAndMaxConsignQuantumUIEditType; }
            set { this.m_MinAndMaxConsignQuantumUIEditType = value; }
        }

        #endregion

        #region 当是修改期货最小和最大委托量UI时，获取交易规则委托量标识
        /// <summary>
        /// 当是修改期货最小和最大委托量UI时，获取交易规则委托量标识
        /// </summary>
        private int m_UpdateConsignQuantumID = AppGlobalVariable.INIT_INT;

        /// <summary>
        ///当是修改期货最小和最大委托量UI时，获取交易规则委托量标识
        /// </summary>
        public int UpdateConsignQuantumID
        {
            set { m_UpdateConsignQuantumID = value; }
            get { return m_UpdateConsignQuantumID; }
        }
        #endregion

        /// <summary>
        /// 交易规则委托量ID
        /// </summary>
        private int m_ConsignQuantumID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 市价委托类型对就的单笔最大委托量ID
        /// </summary>
        private int m_MarkSingleRequestQuantityID = AppGlobalVariable.INIT_INT;

        /// <summary>
        /// 限价委托类型对就的单笔最大委托量ID
        /// </summary>
        private int m_LimitSingleRequestQuantityID = AppGlobalVariable.INIT_INT;

        #endregion
        //================================  私有  方法 ================================
        #region 绑定初始化数据 InitBindData()

        /// <summary>
        /// 绑定初始化数据
        /// </summary>
        private void InitBindData()
        {
            //确认委托指令类型只有2种，不选则按默认2种类型都包含
            //绑定委托指令类型
            //this.cmbConsignInstType.Properties.Items.Clear();
            //this.cmbConsignInstType.Properties.Items.AddRange(BindData.GetBindListMarketPriceType());
            //this.cmbConsignInstType.SelectedIndex = 0;
        }
        #endregion

        #region 清空所有值

        /// <summary>
        /// 清空所有值
        /// </summary>
        private void ClearAll()
        {
            //this.cmbConsignInstType.Text = string.Empty;
            this.txtLimitQuantum.Text = string.Empty;
            this.txtMarketQuantum.Text = string.Empty;
            // this.txtMinConsignQuantum.Text = string.Empty;
        }

        #endregion

        #region 当前UI是修改期货最小和最大委托量UI时，初始化控件的值 UpdateInitData

        /// <summary>
        /// 当前UI是修改期货最小和最大委托量UI时,初始化控件的值 UpdateInitData
        /// </summary>
        private void UpdateInitData()
        {
            try
            {
                if (UpdateConsignQuantumID != AppGlobalVariable.INIT_INT)
                {
                    QH_ConsignQuantum qH_ConsignQuantum =
                        FuturesManageCommon.GetQHConsignQuantumModel(UpdateConsignQuantumID);
                    List<QH_SingleRequestQuantity> qH_SingleRequestQuantity = FuturesManageCommon.GetQHSingleRQuantityListByConsignQuantumID(UpdateConsignQuantumID);

                    if (qH_ConsignQuantum != null && qH_SingleRequestQuantity != null)
                    {
                        this.txtMinConsignQuantum.Text = qH_ConsignQuantum.MinConsignQuantum.ToString();
                        for (int i = 0; i < qH_SingleRequestQuantity.Count; i++)
                        {
                            if ((int)Types.MarketPriceType.MarketPrice ==
                                qH_SingleRequestQuantity[i].ConsignInstructionTypeID)
                            {
                                txtMarketQuantum.Text = qH_SingleRequestQuantity[i].MaxConsignQuanturm.ToString();
                                m_MarkSingleRequestQuantityID =
                                    qH_SingleRequestQuantity[i].SingleRequestQuantityID;
                            }
                            if ((int)Types.MarketPriceType.otherPrice ==
                                qH_SingleRequestQuantity[i].ConsignInstructionTypeID)
                            {
                                this.txtLimitQuantum.Text = qH_SingleRequestQuantity[i].MaxConsignQuanturm.ToString();
                                m_LimitSingleRequestQuantityID = qH_SingleRequestQuantity[i].SingleRequestQuantityID;
                            }
                        }
                        m_ConsignQuantumID = qH_ConsignQuantum.ConsignQuantumID;

                    }
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5862";
                string errMsg = "当前UI是修改期货最小和最大委托量UI时,初始化控件的值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }

        #endregion
        //================================  事件 ================================
        #region 添加期货最小和最大委托量UI AddMinAndMaxConsignQuantumUI_Load
        /// <summary>
        /// 添加期货最小和最大委托量UI AddMinAndMaxConsignQuantumUI_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMinAndMaxConsignQuantumUI_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitBindData();

                if (MinAndMaxConsignQuantumUIEditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    this.UpdateInitData();
                    //this.Text = "修改期货最小和最大委托量";
                }

            }
            catch (Exception ex)
            {
                string errCode = "GL-5860";
                string errMsg = "添加期货最小和最大委托量UI加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 添加或修改期货最小和最大委托量
        /// <summary>
        /// 添加或修改期货最小和最大委托量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                QH_ConsignQuantum qH_ConsignQuantum = new QH_ConsignQuantum();
                QH_SingleRequestQuantity qH_SingleRequestQuantity1;
                QH_SingleRequestQuantity qH_SingleRequestQuantity2;

                if (this.txtLimitQuantum.Text != string.Empty)
                {
                    if (InputTest.intTest(this.txtLimitQuantum.Text))
                    {
                        qH_SingleRequestQuantity1 = new QH_SingleRequestQuantity();
                        qH_SingleRequestQuantity1.ConsignInstructionTypeID = (int)GTA.VTS.Common.CommonObject.Types.MarketPriceType.otherPrice;
                        qH_SingleRequestQuantity1.MaxConsignQuanturm = int.Parse(this.txtLimitQuantum.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入正整数!");
                        return;
                    }
                }
                else
                {
                    qH_SingleRequestQuantity1 = null;
                }
                if (this.txtMarketQuantum.Text != string.Empty)
                {
                    if (InputTest.intTest(this.txtMarketQuantum.Text))
                    {
                        qH_SingleRequestQuantity2 = new QH_SingleRequestQuantity();
                        qH_SingleRequestQuantity2.ConsignInstructionTypeID =
                            (int)GTA.VTS.Common.CommonObject.Types.MarketPriceType.MarketPrice;
                        qH_SingleRequestQuantity2.MaxConsignQuanturm = int.Parse(this.txtMarketQuantum.Text);
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("请输入正整数!");
                        return;
                    }
                }
                else
                {
                    qH_SingleRequestQuantity2 = null;
                }

                if (!string.IsNullOrEmpty(this.txtMinConsignQuantum.Text))
                {
                    qH_ConsignQuantum.MinConsignQuantum = Convert.ToInt32(txtMinConsignQuantum.Text);
                }
                else
                {
                    qH_ConsignQuantum.MinConsignQuantum = AppGlobalVariable.INIT_INT;
                }

                if (m_MinAndMaxConsignQuantumUIEditType == (int)UITypes.EditTypeEnum.AddUI)
                {
                    m_Result = FuturesManageCommon.AddQHConsignQuantumAndSingle(qH_ConsignQuantum, qH_SingleRequestQuantity1, qH_SingleRequestQuantity2);
                    if (m_Result != AppGlobalVariable.INIT_INT)
                    {
                        ConsignQuantumID = m_Result;
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
                else if (m_MinAndMaxConsignQuantumUIEditType == (int)UITypes.EditTypeEnum.UpdateUI)
                {
                    if (m_ConsignQuantumID != AppGlobalVariable.INIT_INT)
                    {
                        qH_ConsignQuantum.ConsignQuantumID = m_ConsignQuantumID;
                        qH_SingleRequestQuantity1.ConsignQuantumID = m_ConsignQuantumID;
                        qH_SingleRequestQuantity1.SingleRequestQuantityID = m_LimitSingleRequestQuantityID;
                        if (m_MarkSingleRequestQuantityID == AppGlobalVariable.INIT_INT)
                        {
                            qH_SingleRequestQuantity2 = null;
                        }
                        else
                        {
                            qH_SingleRequestQuantity2.ConsignQuantumID = m_ConsignQuantumID;
                            qH_SingleRequestQuantity2.SingleRequestQuantityID = m_MarkSingleRequestQuantityID;
                        }
                    }
                    bool _UpResult = FuturesManageCommon.UpdateQHConsignQuantumAndSingle(qH_ConsignQuantum, qH_SingleRequestQuantity1, qH_SingleRequestQuantity2);
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
                string errCode = "GL-5861";
                string errMsg = "添加或修改期货最小和最大委托量失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }

        }
        #endregion

        #region 关闭 添加或修改期货最小和最大委托量UI
        /// <summary>
        /// 关闭 添加或修改期货最小和最大委托量UI
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


    }
}
