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
    /// 描述：添加最低保证金UI
    /// 作者：修改：刘书伟
    /// 日期：2010-03-08 
    public partial class AddCFMinScaleValueUI : DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        ///构造函数 
        /// </summary>
        public AddCFMinScaleValueUI()
        {
            InitializeComponent();
        }
        #endregion

        //================================  私有  方法 ================================
        #region 绑定商品期货_保证金比例初始化数据
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
            }
            catch (Exception ex)
            {
                string errCode = "GL-6401";
                string errMsg = "绑定商品期货_最低保证金比例初始化数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 获取(商品)期货品种类型的品种名称
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

        //================================  事件 ================================

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

        #region 添加最低保证金UI加载事件
        /// <summary>
        /// 添加最低保证金UI加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCFMinScaleValueUI_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitBindData();

            }
            catch (Exception ex)
            {
                string errCode = "GL-6405";
                string errMsg = "添加最低保证金UI加载事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion

        #region 取消UI事件
        /// <summary>
        /// 取消UI事件
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

        #region 保存数据
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                QH_SIFBail bail = new QH_SIFBail();
                if (!string.IsNullOrEmpty(this.cmbBreedClassID.Text))
                {
                    bail.BreedClassID = ((UComboItem)this.cmbBreedClassID.SelectedItem).ValueIndex;
                }
                else
                {
                    ShowMessageBox.ShowInformation("请选择品种!");
                    this.cmbBreedClassID.Focus();
                    return;
                }

                if (InputTest.DecimalTest(this.txtMinScale.Text))
                {
                    bail.BailScale = Convert.ToDecimal(this.txtMinScale.Text);
                }
                else
                {
                    ShowMessageBox.ShowInformation("请输入数字!");
                    this.txtMinScale.Focus();
                    return;
                }

                bool result = FuturesManageCommon.AddQHCFMinScaleValue(bail);
                if (result)
                {
                    ShowMessageBox.ShowInformation("添加成功!");
                    FireSaved(this, new EventArgs());
                }
                else
                {
                    ShowMessageBox.ShowInformation("添加失败!");
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6402";
                string errMsg = "设置商品期货_最低保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return;
            }
        }
        #endregion
    }
}
