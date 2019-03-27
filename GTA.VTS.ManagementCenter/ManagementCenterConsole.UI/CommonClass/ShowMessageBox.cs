using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 封装MessageBox类
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-26
    /// </summary>
    public class ShowMessageBox
    {
        const string captionInfo = "系统提示";
        const string captionQues = "系统询问";

        /// <summary>
        /// 信息提示框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult ShowInformation(string text)
        {
            return MessageBox.Show(null, text, captionInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 咨询提示框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult ShowQuestion(string text)
        {
            return MessageBox.Show(null, text, captionQues, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
