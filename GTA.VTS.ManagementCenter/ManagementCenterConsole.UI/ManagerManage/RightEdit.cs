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
using DevExpress.XtraEditors.Controls;
using ManagementCenter.Model;
using  ManagementCenter.BLL;
using ManagementCenterConsole.UI.CommonClass;

namespace ManagementCenterConsole.UI.ManagerManage
{
    /// <summary>
    /// 权限组功能管理 错误编码范围1221-1240
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-29
    /// </summary>
    public partial class RightEdit :DevExpress.XtraEditors.XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RightEdit()
        {
            InitializeComponent();
        }
        #endregion

        #region 操作类型　 1:添加,2:修改
        private int m_EditType = 1;
        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get
            {
                return this.m_EditType;
            }
            set
            {
                this.m_EditType = value;
            }
        }
        #endregion

        #region 权限组实体
        /// <summary>
        /// 权限组实体
        /// </summary>
        private UM_ManagerGroup managergroup = null;
        /// <summary>
        /// 权限组属性
        /// </summary>
        public UM_ManagerGroup ManagerGroup
        {
            get
            {
                return this.managergroup;
            }
            set
            {
                this.managergroup = new UM_ManagerGroup();
                this.managergroup = value;
            }
        }
        #endregion

        /// <summary>
        /// 管理员组可用功能BLL
        /// </summary>
        private UM_ManagerGroupFunctionsBLL ManagerGroupFunctionsBLL;
        /// <summary>
        /// 权限功能表BLL
        /// </summary>
        private UM_FunctionsBLL FunctionsBLL;
        /// <summary>
        /// 管理员组BLL
        /// </summary>
        private UM_ManagerGroupBLL ManagerGroupBLL;

        #region 添加权限按纽事件
        private void btn_Add_Click(object sender, EventArgs e)
        {
            UComboItem item;
            foreach (int i in listBoxAllRight.SelectedIndices)
            {
                item = (UComboItem)listBoxAllRight.GetItem(i);
                bool falg = false;
                foreach (object  obj in listBoxHasRight.Items)
                {
                    if(item.ValueIndex==((UComboItem)obj).ValueIndex)
                    {
                        falg = true;
                        break;
                    }
                }
                if (!falg) this.listBoxHasRight.Items.Add(item);
            }
        }

        #endregion

        #region 添加所有权限按纽事件
        private void btn_AddAll_Click(object sender, EventArgs e)
        {
            this.listBoxHasRight.Items.Clear();
            foreach (object obj in listBoxAllRight.Items)
            {
                this.listBoxHasRight.Items.Add(obj);
            }
        }
        #endregion

        #region 删除权限按纽事件
        private void btn_Del_Click(object sender, EventArgs e)
        {
            //UComboItem item;
            //List<UComboItem> l_UComboItem=new List<UComboItem>();
            //foreach (int i in listBoxHasRight.SelectedIndices)
            //{
            //    item = (UComboItem)listBoxAllRight.Items[i];
            //   l_UComboItem.Add(item);

            //}
            //foreach (UComboItem ucomboitem in l_UComboItem)
            //{
            //    foreach (object obj in listBoxHasRight.Items)
            //    {
            //        if (ucomboitem.ValueIndex == ((UComboItem)obj).ValueIndex)
            //        {
            //            listBoxHasRight.Items.Remove(obj);
                        
            //            break;
            //        }
            //    }
            //}
            //this.listBoxHasRight.Refresh();
            if (listBoxHasRight.SelectedItem == null) return;
            this.listBoxHasRight.Items.Remove(listBoxHasRight.SelectedItem);
            this.listBoxHasRight.Refresh();
        }
        #endregion

        #region 删除所有权限
        /// <summary>
        /// 删除所有权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delAll_Click(object sender, EventArgs e)
        {
            this.listBoxHasRight.Items.Clear();
        }
        #endregion

        #region 保存权限组按纽事件
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txt_GroupName.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入权限组名称!");
                    return;
                }
                if(EditType==1) managergroup = new UM_ManagerGroup();
               
                managergroup.ManagerGroupName = this.txt_GroupName.Text;

                if (this.listBoxHasRight.Items.Count<1)
                {
                    ShowMessageBox.ShowInformation("请选择权限!");
                    return;
                }

                List<UM_ManagerGroupFunctions> l_ManagerGroupFunctions = new List<UM_ManagerGroupFunctions>();
                UM_ManagerGroupFunctions ManagerGroupFunction;
                foreach (object obj in listBoxHasRight.Items)
                {
                    UComboItem item = (UComboItem) obj;
                    ManagerGroupFunction = new UM_ManagerGroupFunctions();
                    ManagerGroupFunction.FunctionID = item.ValueIndex;
                    l_ManagerGroupFunctions.Add(ManagerGroupFunction);
                }
                if(EditType==1)
                {
                    if (ManagerGroupBLL.Add(managergroup, l_ManagerGroupFunctions))
                    {
                        ShowMessageBox.ShowInformation("添加权限组成功!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("添加权限组失败!");
                    }
                }
                else
                {
                    if (ManagerGroupBLL.Update(managergroup, l_ManagerGroupFunctions))
                    {
                        ShowMessageBox.ShowInformation("修改权限组成功!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        ShowMessageBox.ShowInformation("修改权限组失败!");
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-1221";
                string errMsg = "保存权限组事件失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }
        #endregion

        #region 取消按纽事件
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 页面加载事件
        private void RightEdit_Load(object sender, EventArgs e)
        {
            try
            {
                ManagerGroupFunctionsBLL = new UM_ManagerGroupFunctionsBLL();
                FunctionsBLL = new UM_FunctionsBLL();
                ManagerGroupBLL = new UM_ManagerGroupBLL();
                LoadAllRightList();
                if (EditType == 2 && managergroup != null)
                {
                    this.txt_GroupName.Text = managergroup.ManagerGroupName;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-1222";
                string errMsg = "页面加载失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }
        #endregion

        #region 加载权限列表
        /// <summary>
        /// 加载权限列表
        /// </summary>
        private void LoadAllRightList()
        {
            try
            {
                List<UM_Functions> l_Functions = FunctionsBLL.GetListArray(string.Empty);
                UComboItem item;
                this.listBoxAllRight.Items.Clear();
                foreach (UM_Functions Functions in l_Functions)
                {
                    item = new UComboItem(Functions.FunctionName, Functions.FunctionID);
                    this.listBoxAllRight.Items.Add(item);

                }
                if (EditType == 2)
                {
                    List<UM_ManagerGroupFunctions> l_ManagerGroupFunctions =
                      ManagerGroupFunctionsBLL.GetListArray(string.Format(" ManagerGroupID={0}", managergroup.ManagerGroupID));

                    foreach (UM_ManagerGroupFunctions ManagerGroupFunctions in l_ManagerGroupFunctions)
                    {
                        item = new UComboItem(ManagerGroupFunctions.FunctionName, (int)ManagerGroupFunctions.FunctionID);
                        this.listBoxHasRight.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-1223";
                string errMsg = "加载权限列表失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }
        #endregion
    }
}
