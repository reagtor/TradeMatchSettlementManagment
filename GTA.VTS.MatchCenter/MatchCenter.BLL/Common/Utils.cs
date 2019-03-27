using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using GTA.VTS.Common.CommonUtility;
using System.Windows.Forms;

namespace MatchCenter.BLL.Common
{
    /// <summary>
    /// 把信息显示到Form的Title上的事件
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// </summary>
    /// <param name="eventMessage"></param>
    /// <param name="lm"></param>
    public delegate void OnFormEventDelegate(string eventMessage, Form lm);

    /// <summary>
    /// 公共功能类
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// </summary>
    public class Utils
    {
        #region 判断一个List是否为空或者Count=0
        /// <summary>
        /// 判断一个List是否为空或者Count=0
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }
            if (list.Count == 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 根据数据去除小数点最后无意义为0的位数，返回小数点的有效位数
        /// <summary>
        /// Title:根据数据去除小数点最后无意义为0的位数，返回小数点的有效位数
        /// Desc.:去除小数点最后为0无意义的位数.如果数据没有小数位（如：100-->2）返回默认有效小数位长度
        ///       (但是如果isDefault=false（如：100-->0）
        ///       如果所有的小数位都为0则返回默认有效小数为长度（如：100.000-->2）(但是如果isDefault=false（如：100-->0）
        ///       如果有有效位数则返回有效位数（如：3.1010-->3，5.00002-->5,3.200-->1）
        /// </summary>
        /// <param name="k">要返回位数的数据</param>
        /// <param name="isDefault">是返回默认长度(True),还是返回真实长度(false)</param>
        /// <returns></returns>
        public static int GetDecimalDigit(decimal k, bool isDefault)
        {
            string[] strs = k.ToString().Split('.');
            if (strs.Length <= 1)
            {
                if (isDefault)
                {
                    return RulesDefaultValue.DefaultLength;
                }
                else
                {
                    return 0;
                }
            }
            string round = strs[1];
            string lenghtStr = "";
            for (int i = round.Length - 1; i >= 0; i--)
            {
                int s = 0;
                bool typ = int.TryParse(round[i].ToString(), out s);
                if (!typ || s == 0)
                {
                    continue;
                }
                else
                {
                    lenghtStr = round.Substring(0, i + 1);
                    break;
                }
            }
            if (lenghtStr.Length <= 0)
            {
                if (isDefault)
                {
                    return RulesDefaultValue.DefaultLength;
                }
            }
            return lenghtStr.Length;
        }
        #endregion

        #region 比较两个值的大小
        /// <summary>
        /// 把theValue与otherValue比较，判断theValue是否小于(等于)otherValue
        /// </summary>
        /// <param name="theValue">要比较的值</param>
        /// <param name="otehrValue">要与比较的值</param>
        /// <param name="isEqual">是否可以包括相等比较</param>
        /// <returns></returns>
        public static bool TheValueCompareOtherValue(decimal theValue, decimal otehrValue, bool isEqual)
        {
            if (isEqual)
            {
                if (theValue <= otehrValue)
                {
                    return true;
                }
            }
            else
            {
                if (theValue < otehrValue)
                {
                    return true;
                }
            }
            return false;

        }
        #endregion

        #region 数字 x 的 y 次幂的倒数
        /// <summary>
        ///  返回指定数字的指定次幂的倒数   
        /// </summary>
        /// <param name="x">要乘幂的双精度浮点数</param>
        /// <param name="y">指定幂的双精度浮点数</param>
        /// <returns>如果次幂结果倒数后转化为decimal溢出异常，则返回默认1.00</returns>
        public static decimal XPowerYCountdown(double x, double y)
        {
            decimal min = 1.00m;
            try
            {
                min = decimal.Divide(1, (decimal)Math.Pow(x, y));
            }
            catch  //这里异常多为decimal溢出,几率很小,如长度为50时
            {
                min = 1.00M;
            }
            return min;
        }

        #endregion

        #region 把传入的日期时间转为当前日期时间
        /// <summary>
        /// 把传入的日期时间转为当前日期时间 如果转换有异常则返回当前时间DateTime.Now
        /// </summary>
        /// <param name="time">要转换的时间</param>
        /// <returns></returns>
        public static DateTime ConvertToNowDateTime(DateTime time)
        {
            string strDate = DateTime.Now.ToString("yyyy-MM-dd");
            string strTime = time.ToString("HH:mm:ss");
            try
            {
                DateTime tradeTime = DateTime.Parse(strDate + " " + strTime);
                return tradeTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// 把传入的日期时间转为当前日期时间 如果转换有异常则返回当前时间DateTime.Now
        /// </summary>
        /// <param name="timeString">要转换的时间字符串</param>
        /// <returns></returns>
        public static DateTime ConvertToNowDateTime(string timeString)
        {
            string strDate = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                DateTime time = DateTime.Parse(timeString);
                string strTime = time.ToString("HH:mm:ss");

                DateTime tradeTime = DateTime.Parse(strDate + " " + strTime);
                return tradeTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        #region new Create by:董鹏 2009-12-16
        /// <summary>
        /// 把传入的日期和时间字符串转换为日期时间类型
        /// </summary>
        /// <param name="date">日期字符串</param>
        /// <param name="time">时间字符串</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string date, string time)
        {
            try
            {
                DateTime tradeTime = DateTime.Parse(date + " " + time);
                return tradeTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }
        #endregion
        #endregion

        #region  把时间与当前系统时间比较相差(时间间隔)分钟
        /// <summary>
        /// 把时间与当前系统时间比较相差(时间间隔)分钟(span.Minutes)
        /// </summary>
        /// <param name="timeSpan">要比较的时间</param>
        /// <returns></returns>
        public static int TimeSpanMinutesToNowDateTime(DateTime timeSpan)
        {
            TimeSpan span = DateTime.Now - timeSpan;
            return span.Minutes;
        }
        #endregion

        #region 获取时间与当前时间相差的总秒数
        /// <summary>
        /// 把时间与当前时间相差(时间间隔)的【总】秒数(span.TotalSeconds)
        /// </summary>
        /// <param name="startTime">时间</param>
        /// <returns></returns>
        public static double TimeSpanSecondsToNowDateTime(DateTime startTime)
        {
            TimeSpan span = DateTime.Now - startTime;
            return span.TotalSeconds;
            //return span.Hours * 3600 + span.Minutes * 60 + span.Seconds;

        }
        #endregion

        #region 在指定的ListBox中前面插入的信息字符串
        /// <summary>
        /// Title:把信息插入到ListBox中显示
        /// Desc.:在指定的ListBox中前面插入的信息字符串
        /// </summary>
        /// <param name="eventMessage">要显示的字符串</param>
        /// <param name="lstMessages">指定的ListBox</param>
        public static void ShowMessageToUIEvent(string eventMessage, ListBox lstMessages)
        {
            if (lstMessages == null)
            {
                return;
            }
            try
            {
                if (lstMessages.InvokeRequired)
                {
                    lstMessages.Invoke(new OnUIEventDelegate(ShowMessageToUIEvent), eventMessage, lstMessages);
                }
                else
                {
                    if (lstMessages.Items.Count > RulesDefaultValue.DefalultShowCountLists)
                    {
                        lstMessages.Items.Clear();
                    }
                    lstMessages.Items.Insert(0, eventMessage);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(string.Format(GenerateInfo.CH_E005, lstMessages.Name, eventMessage), ex);
            }
        }
        #endregion

        #region 在指定的Form中把指定的字符串显示到Title上
        /// <summary>
        /// Title:把信息显示到对应的窗体Title上
        /// Desc.:在指定的Form中的Title显示的信息字符串
        /// </summary>
        /// <param name="eventMessage">要显示的字符串</param>
        /// <param name="formMessages">指定的Form</param>
        public static void ShowMessageToFormTitleEvent(string eventMessage, Form formMessages)
        {
            if (formMessages == null)
            {
                return;
            }
            try
            {
                if (formMessages.InvokeRequired)
                {
                    formMessages.Invoke(new OnFormEventDelegate(ShowMessageToFormTitleEvent), eventMessage, formMessages);
                }
                else
                {
                    formMessages.Text = RulesDefaultValue.title_ZH + eventMessage;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(string.Format(GenerateInfo.CH_E005, formMessages.Name, eventMessage), ex);
            }

        }
        #endregion

       
    }
}
