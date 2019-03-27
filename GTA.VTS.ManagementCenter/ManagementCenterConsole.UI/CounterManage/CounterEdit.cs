using System;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using DevExpress.XtraEditors;
using ManagementCenter.BLL;
using ManagementCenter.Model;
using ManagementCenterConsole.UI.CommonClass;
using Types=ManagementCenter.Model.CommonClass.Types;

namespace ManagementCenterConsole.UI.CounterManage
{
    /// <summary>
    /// 柜台信息编辑页面  编码范围 GL-3031-3060
    /// 作者：熊晓凌
    /// 日期：2008-11-20
    /// </summary>
    public partial class CounterEdit : XtraForm
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CounterEdit()
        {
            InitializeComponent();
        }
        #endregion

        #region 柜台实体

        private CT_Counter m_counter;

        public CT_Counter Counter
        {
            set { m_counter = value; }
            get { return m_counter; }
        }

        #endregion

        #region 操作类型　 1:添加,2:修改

        private int m_EditType = 1;

        /// <summary>
        /// 操作类型 1:添加,2:修改
        /// </summary>
        public int EditType
        {
            get { return m_EditType; }
            set { m_EditType = value; }
        }

        #endregion

        #region 页面加载

        private void CounterEdit_Load(object sender, EventArgs e)
        {
            try
            {
              if (m_EditType ==1)
              {
                  //根据需求，指定默认值
                  txt_IP.Text="127.0.0.1";
                  txt_XiaDanServiceName.Text = "DoOrderService";//下单服务名称
                  txt_Port.Text = "9181"; //"8085";//下单服务端口
                  txtSendServiceName.Text="DoDealRptService";//回送服务名称
                  txtSendPort.Text = "9182"; //"8086";//回送服务端口
                  txt_QueryServiceName.Text = "DoQueryService";//查询服务名称
                  txtQueryServicePort.Text = "9183"; //"8087";//查询服务端口
                  txt_AccountServiceName.Text = "DoAccountAndCapitalManagementService";//帐号服务名称 ?此名称是否改短
                  txtAccountServicePort.Text = "9183"; //"8087";//帐号服务端口
              }
              else if (m_EditType == 2)
              {
                    SetCounterInfo();
              }
                
            }
            catch (Exception ex)
            {
                string errCode = "GL-3031";
                string errMsg = "柜台信息页面加载失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 设置初始值

        private void SetCounterInfo()
        {
            if (EditType == 2 && m_counter != null)
            {
                txt_CounterName.Text = m_counter.name;
                txt_IP.Text = m_counter.IP;
                //txt_MaxValues.Text = m_counter.MaxValues.ToString();
                txt_Port.Text = m_counter.XiaDanServicePort.ToString();//为方便分辨把Port改名为XiaDanServicePort
                txt_XiaDanServiceName.Text = m_counter.XiaDanServiceName;
                txt_QueryServiceName.Text = m_counter.QueryServiceName;
                txt_AccountServiceName.Text = m_counter.AccountServiceName;
                //新加代码 09.04.08
                txtSendServiceName.Text = m_counter.SendServiceName;
                txtAccountServicePort.Text = m_counter.AccountServicePort.ToString();
                txtQueryServicePort.Text = m_counter.QueryServicePort.ToString();
                txtSendPort.Text = m_counter.SendPort.ToString();
                //显示提示
                txt_CounterName.ToolTip = m_counter.name;
                txt_XiaDanServiceName.ToolTip = m_counter.XiaDanServiceName;
                txt_QueryServiceName.ToolTip = m_counter.QueryServiceName;
                txt_AccountServiceName.ToolTip = m_counter.AccountServiceName;
                txtSendServiceName.ToolTip = m_counter.SendServiceName;

                if (m_counter.State == (int) Types.StateEnum.ConnSuccess)
                {
                    labState.Text = "连接正常";
                }
                else if (m_counter.State == (int) Types.StateEnum.ConnDefeat)
                {
                    labState.Text = "连接失败";
                }
            }
        }

        #endregion

        #region 确定按纽事件

        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckInput())
                {
                    GetEntity();
                    var CounterBLL = new CT_CounterBLL();
                    if (EditType == 1)
                    {
                        CounterBLL.Add(m_counter);
                        ShowMessageBox.ShowInformation("添加成功!");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        CounterBLL.Update(m_counter);
                        ShowMessageBox.ShowInformation("修改成功!");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox.ShowInformation("编辑失败!");
                string errCode = "GL-3032";
                string errMsg = "编辑保存失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

        #region 取消事件

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region 输入检测

        private bool CheckInput()
        {
            try
            {
                if (txt_CounterName.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入柜台名称!");
                    return false;
                }
                if (txt_CounterName.Text.Length>= 50)
                {
                    ShowMessageBox.ShowInformation("名称长度不能超过50个字符!");
                    return false;
                }
                if (txt_IP.Text == string.Empty)
                {
                    ShowMessageBox.ShowInformation("请输入IP地址!");
                    return false;
                }
                string[] ip = txt_IP.Text.Split('.');
                if (ip.Length != 4)
                {
                    ShowMessageBox.ShowInformation("IP格式错误，请重新输入!");
                    return false;
                }
                for (int i = 0; i <= 3; i++)
                {
                    try
                    {
                        int k = int.Parse(ip[i]);
                        if (k < 0 || k > 255)
                        {
                            ShowMessageBox.ShowInformation("IP格式错误，请重新输入!");
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        ShowMessageBox.ShowInformation("IP格式错误，请重新输入!");
                        return false;
                    }
                }
                try
                {
                    int p = int.Parse(txt_Port.Text);
                }
                catch (Exception)
                {
                    ShowMessageBox.ShowInformation("端口输入有误，请重新录入!");
                    return false;
                }
                #region 根据V1。1需求，不需要的代码
                //try
                //{
                //    int m = int.Parse(txt_MaxValues.Text);
                //}
                //catch (Exception)
                //{
                //    ShowMessageBox.ShowInformation("最大容量人数输入有误！");
                //    return false;
                //}
                //if (txt_XiaDanServiceName.Text == string.Empty)
                //{
                //    ShowMessageBox.ShowInformation("请输入服务名称！");
                //    return false;
                //}
                //if (txt_XiaDanServiceName.Text.Length >= 100)
                //{
                //    ShowMessageBox.ShowInformation("下单服务名称长度不能超过100个字符！");//("下单服务名称长度不能超过100个字符！");
                //    return false;
                //}
                //if (txt_AccountServiceName.Text.Length >=100)
                //{
                //    ShowMessageBox.ShowInformation("帐号服务名称长度不能超过100个字符！");
                //    return false;
                //}
                //if (txt_QueryServiceName.Text.Length >= 100)
                //{
                //    ShowMessageBox.ShowInformation("查询服务名称长度不能超过100个字符！");
                //    return false;
                //}
                //新加代码 2009.04.08
                //if (txtSendServiceName.Text.Length >= 100)
                //{
                //    ShowMessageBox.ShowInformation("回送服务名称长度不能超过100个字符！");
                //    return false;
                //}
                #endregion

                try
                {
                    int p = int.Parse(txtSendPort.Text);
                }
                catch (Exception)
                {
                    ShowMessageBox.ShowInformation("端口输入有误，请重新录入!");
                    return false;
                }
                try
                {
                    int p = int.Parse(txtQueryServicePort.Text);
                }
                catch (Exception)
                {
                    ShowMessageBox.ShowInformation("端口输入有误，请重新录入!");
                    return false;
                }
                try
                {
                    int p = int.Parse(txtAccountServicePort.Text);
                }
                catch (Exception)
                {
                    ShowMessageBox.ShowInformation("端口输入有误，请重新录入!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-3033";
                string errMsg = "输入检测失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 获取实体

        private void GetEntity()
        {
            try
            {
                if (m_counter == null) m_counter = new CT_Counter();
                m_counter.IP = txt_IP.Text;
                //m_counter.MaxValues = int.Parse(txt_MaxValues.Text);
                m_counter.name = txt_CounterName.Text;
                m_counter.XiaDanServicePort = int.Parse(txt_Port.Text);
                m_counter.XiaDanServiceName = txt_XiaDanServiceName.Text;
                m_counter.AccountServiceName = txt_AccountServiceName.Text;
                m_counter.QueryServiceName = txt_QueryServiceName.Text;
                m_counter.State = (int) Types.StateEnum.ConnDefeat;
                //新加代码 09.04.08
                m_counter.SendServiceName = txtSendServiceName.Text;
                m_counter.AccountServicePort = int.Parse(txtAccountServicePort.Text);
                m_counter.QueryServicePort = int.Parse(txtQueryServicePort.Text);
                m_counter.SendPort = int.Parse(txtSendPort.Text);
            }
            catch (Exception ex)
            {
                string errCode = "GL-3034";
                string errMsg = "获取实体失败!";
                var exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
            }
        }

        #endregion

  

    }
}