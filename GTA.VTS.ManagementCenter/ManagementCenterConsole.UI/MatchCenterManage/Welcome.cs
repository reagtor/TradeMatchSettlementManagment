using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManagementCenterConsole.UI.MatchCenterManage
{
    /// <summary>
    /// 描述：撮合机相关简单配置向导欢迎界面 
    /// 作者：熊晓凌
    /// 日期：2008-12-15
    /// </summary>
    public partial class Welcome :DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Welcome()
        {
            InitializeComponent();
        }
        #endregion

        #region  Welcome窗体静态实例变量
        /// <summary>
        /// Welcome窗体静态实例变量
        /// </summary>
        private static Welcome instance = null;
        /// <summary>
        /// Welcome窗体属性
        /// </summary>
        public static Welcome Instance
        {
            get
            {
                if(instance==null)instance=new Welcome();
                return instance;
            }
            set { instance = value;}
        }
        #endregion

    } 
}
