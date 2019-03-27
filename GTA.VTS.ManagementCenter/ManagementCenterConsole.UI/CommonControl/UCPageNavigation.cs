using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ManagementCenterConsole.UI.CommonControl
{
    /// <summary>
    /// 翻页控件
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public partial class UCPageNavigation : DevExpress.XtraEditors.XtraUserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCPageNavigation()
        {
            InitializeComponent();
        }
        #endregion
        /// <summary>
        /// 页数改变事件
        /// </summary>
        public event PageIndexChangedCallBack PageIndexChanged = null;

        #region 总页数
        private int m_pageCount = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return m_pageCount;
            }
            set
            {
                if (m_pageCount != value && value >= 0)
                {
                    m_pageCount = value;
                    labPagCount.Text = string.Format("{0}", value.ToString());
                    InilstPage(value);
                    RefreshControls();
                }
            }
        }
        #endregion

        private static int stepStep = 50;

        #region 初始化页数
        /// <summary>
        /// 初始化页数
        /// </summary>
        /// <param name="value"></param>
        private void InilstPage(int value)
        {

            ((System.ComponentModel.ISupportInitialize)(this.lstSelectPage.Properties)).BeginInit();
            lstSelectPage.SelectedIndex = -1;
            lstSelectPage.Properties.Items.Clear();
            int step = value / stepStep;
            if (value % stepStep > 0)
                step++;

            if (step > 0)
            {
                for (int i = 1; i <= value; i += step)
                {
                    lstSelectPage.Properties.Items.Add(i);
                }
                if (value % step > 0)
                    lstSelectPage.Properties.Items.Add(value);
            }
            ((System.ComponentModel.ISupportInitialize)(this.lstSelectPage.Properties)).EndInit();
            if (value > 0)
            {
                lstSelectPage.SelectedIndex = 0;
            }
            else
                m_currentPage = 0;
        }
        #endregion

        #region 当前页数
        private int m_currentPage = 0;
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return m_currentPage;
            }
            set
            {
                if (m_currentPage != value && value <= PageCount && value > 0)
                {

                    m_currentPage = value;
                    RefreshControls();
                    if (PageIndexChanged != null)
                        PageIndexChanged(m_currentPage);
                }
                else
                {
                    lstSelectPage.Text = m_currentPage.ToString();
                }
            }
        }
        #endregion

        #region 更新控件
        private void RefreshControls()
        {
            btnFirst.Enabled = btnPre.Enabled = CurrentPage > 1;

            btnNext.Enabled = btnLast.Enabled = CurrentPage < PageCount;

            lstSelectPage.Text = m_currentPage.ToString();

        }
        #endregion

        #region 内部的控件事件

        private void btnFirst_Click(object sender, System.EventArgs e)
        {
            CurrentPage = 1;
        }

        private void btnPrevious_Click(object sender, System.EventArgs e)
        {
            CurrentPage--;
        }

        private void lstSelectPage_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    CurrentPage = Convert.ToInt32(lstSelectPage.Text);
                }
                catch
                {
                    lstSelectPage.Text = CurrentPage.ToString();
                }
            }
        }
        private void lstSelectPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lstSelectPage.SelectedIndex >= 0)
                CurrentPage = Convert.ToInt32(lstSelectPage.Text);
        }

        private void btnNext_Click(object sender, System.EventArgs e)
        {
            CurrentPage++;
        }

        private void btnLast_Click(object sender, System.EventArgs e)
        {
            CurrentPage = PageCount;
        }

        #endregion
    }
    /// <summary>
    /// 页数改变回调委托方法
    /// </summary>
    /// <param name="page"></param>
    public delegate void PageIndexChangedCallBack(int page);
}
