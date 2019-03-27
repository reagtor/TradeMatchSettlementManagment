#region Using Namespace

using System;
using Amib.Threading;

#endregion

namespace ReckoningCounter.BLL.Reckoning.Logic
{
    /// <summary>
    /// 清算单元接口
    /// </summary>
    public interface IReckonUnit
    {
        /// <summary>
        /// 定时清算提交通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void DoReckonCommitCheck(Object sender, EventArgs args);

        SmartThreadPool SmartPool { get; set; }
    }
}
