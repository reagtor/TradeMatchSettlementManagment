using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA.VTS.CustomersOrders.Controls
{
    public partial class PageControl : UserControl
    {
        bool IsButtonClick = false;

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage
        {
            get;
            set;
        }

        /// <summary>
        /// 记录总数
        /// </summary>
        public int RecordsCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总页数
        /// </summary>
        private int PageCount
        {
            get
            {
                if (RecordsCount % PageSize == 0)
                {
                    return RecordsCount / PageSize;
                }
                else
                {
                    return (RecordsCount / PageSize) + 1;
                }
            }
        }

        /// <summary>
        /// 翻页事件
        /// </summary>
        public event EventHandler OnPageChanged;

        public PageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        public void BindData()
        {
            IsButtonClick = true;
            comboBox1.Items.Clear();
            for (int i = 1; i <= PageCount; i++)
            {
                comboBox1.Items.Add(i);
            }
            if (comboBox1.Items.Count != 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            lbPageCount.Text = PageCount.ToString();
            ResetControlStatus();
            IsButtonClick = false;
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="recordsCount">记录总数</param>
        public void BindData(int pageSize, int recordsCount)
        {
            this.PageSize = pageSize;
            this.RecordsCount = recordsCount;
            BindData();
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        private void ResetControlStatus()
        {
            btnFirst.Enabled = true;
            btnPreview.Enabled = true;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
            if (CurrentPage == 1 || RecordsCount == 0)
            {
                btnFirst.Enabled = false;
                btnPreview.Enabled = false;
            }
            if (CurrentPage == PageCount || RecordsCount == 0)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
        }

        /// <summary>
        /// 触发翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageChanged(object sender, EventArgs e)
        {
            if (this.OnPageChanged != null)
            {
                ResetControlStatus();
                OnPageChanged(this, e);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            IsButtonClick = true;
            CurrentPage = 1;
            comboBox1.SelectedIndex = 0;

            PageChanged(this, e);
            IsButtonClick = false;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            IsButtonClick = true;
            if (CurrentPage > 1)
            {
                CurrentPage--;
                comboBox1.SelectedIndex--;
            }
            else
            {
                return;
            }
            PageChanged(this, e);
            IsButtonClick = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            IsButtonClick = true;
            if (CurrentPage < PageCount)
            {
                CurrentPage++;
                comboBox1.SelectedIndex++;
            }
            else
            {
                return;
            }
            PageChanged(this, e);
            IsButtonClick = false;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            IsButtonClick = true;
            CurrentPage = PageCount;
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;

            PageChanged(this, e);
            IsButtonClick = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsButtonClick)
            {
                CurrentPage = (int)comboBox1.SelectedItem;
                
                PageChanged(this, e);
            }
        }
    }
}
