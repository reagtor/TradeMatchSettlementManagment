using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL;
using ManagementCenter.Model;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 撮合中心配置向导完成页 
    /// 作者：熊晓凌
    /// 日期：2008-12-12
    /// </summary>
    public partial class Complete : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Complete()
        {
            InitializeComponent();
        }
        #endregion

        private static Complete instance = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static Complete Instance
        {
            get
            {
                if (instance == null) instance = new Complete();
                return instance;
            }
            set { instance = value; }
        }

        /// <summary>
        /// 保存数据事件
        /// </summary>
        /// <returns></returns>
        public bool SaveDate()
        {
            try
            {
                ManagementCenter.BLL.RC_MatchCenterBLL RC_MatchCenter = new RC_MatchCenterBLL();
                if (RC_MatchCenter.GuideSave(CenterInfo.Instance.MatchCenter, Bourse_Machine.Instance.ds.Tables[0]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-2050";
                string errMsg = "保存数据事件失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }
    }
}