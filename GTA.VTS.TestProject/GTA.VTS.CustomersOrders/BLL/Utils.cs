#region Using Namespace

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using GTA.VTS.Common.CommonUtility;
using GTA.VTS.CustomersOrders.DoOrderService;

#endregion

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// Title: 公共数据工具类
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 获取实时行情组件使用模式
        /// </summary>
        /// <returns>1.socket 2.fast</returns>
        public static int GetRealTimeMode()
        {
            int realTimeMode = 1;
            try
            {
                string setting = ConfigurationManager.AppSettings["realTimeMode"];
                if (!string.IsNullOrEmpty(setting))
                {
                    int count;
                    bool isSuccess = int.TryParse(setting.Trim(), out count);
                    if (isSuccess)
                        realTimeMode = count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return realTimeMode;
        }

        /// <summary>
        /// 获取实时行情组件用户名
        /// </summary>
        /// <returns>实时行情组件用户名</returns>
        public static string GetRealTimeUserName()
        {
            string userName = "rtuser";
            try
            {
                string name = ConfigurationManager.AppSettings["ServerUserName"];
                if (!string.IsNullOrEmpty(name))
                {
                    userName = name;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return userName;
        }

        /// <summary>
        /// 获取实时行情组件密码
        /// </summary>
        /// <returns>实时行情组件密码</returns>
        public static string GetRealTimePassword()
        {
            string password = "11";
            try
            {
                string ps = ConfigurationManager.AppSettings["ServerPassword"];
                if (!string.IsNullOrEmpty(ps))
                {
                    password = ps;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return password;
        }

        /// <summary>
        /// 委托状态信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetOrderStateMsg(int type)
        {
            //OrderStateType state = OrderStateType.None;
            string msg = "无状态01";

            switch (type)
            {
                case 1:
                    //state = OrderStateType.None;
                    msg = "无状态01";
                    break;
                case 2:
                    //state = OrderStateType.DOSUnRequired;
                    msg = "未报02";
                    break;
                case 3:
                    //state = OrderStateType.DOSRequiredSoon;
                    msg = "待报03";
                    break;
                case 4:
                    //state = OrderStateType.DOSRequiredRemoveSoon;
                    msg = "已报待撤04";
                    break;
                case 5:
                    //state = OrderStateType.DOSIsRequired;
                    msg = "已报05";
                    break;
                case 6:
                    //state = OrderStateType.DOSCanceled;
                    msg = "废单06";
                    break;
                case 7:
                    //state = OrderStateType.DOSRemoved;
                    msg = "已撤07";
                    break;
                case 8:
                    //state = OrderStateType.DOSPartRemoved;
                    msg = "部撤08";
                    break;
                case 9:
                    //state = OrderStateType.DOSPartDealed;
                    msg = "部成09";
                    break;
                case 10:
                    //state = OrderStateType.DOSDealed;
                    msg = "已成10";
                    break;
                case 11:
                    //state = OrderStateType.DOSPartDealRemoveSoon;
                    msg = "部成待撤11";
                    break;
            }

            return msg;
        }

        /// <summary>
        /// 根据期货开平方向文本获取期货开平方向类型
        /// </summary>
        /// <param name="comboType">期货开平方向文本</param>
        /// <returns>期货开平方向类型</returns>
        public static TypesFutureOpenCloseType GetFutureOpenCloseType(string comboType)
        {
            TypesFutureOpenCloseType result = TypesFutureOpenCloseType.OpenPosition;
            try
            {
                string type = comboType.Substring(comboType.Length - 1, 1);
                int i = int.Parse(type);

                result = (TypesFutureOpenCloseType) i;
                //switch (type)
                //{
                //    case 1://FutureOpenCloseType.OpenPosition:
                //        result = "开仓";
                //        break;
                //    case 2://FutureOpenCloseType.ClosePosition:
                //        result = "平仓(历史)";
                //        break;
                //    case 3://FutureOpenCloseType.CloseTodayPosition:
                //        result = "平仓";
                //        break;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 根据期货开平方向类型获取期货开平方向文本
        /// </summary>
        /// <param name="type">期货开平方向类型</param>
        /// <returns>期货开平方向文本</returns>
        public static string GetFutureOpenCloseType(TypesFutureOpenCloseType type)
        {
            string result = "";
            try
            {
                switch (type)
                {
                    case TypesFutureOpenCloseType.OpenPosition:
                        result = "开仓1";
                        break;
                    case TypesFutureOpenCloseType.ClosePosition:
                        result = "平仓(历史)2";
                        break;
                    case TypesFutureOpenCloseType.CloseTodayPosition:
                        result = "平仓3";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取对应的交易单位类型
        /// </summary>
        /// <param name="comboType"></param>
        /// <returns></returns>
        public static GTA.VTS.Common.CommonObject.Types.UnitType GetUnit(string comboType)
        {
            GTA.VTS.Common.CommonObject.Types.UnitType result = GTA.VTS.Common.CommonObject.Types.UnitType.Thigh;

            try
            {
                string type = comboType.Substring(comboType.Length - 1, 1);
                int i = int.Parse(type);

                result = (GTA.VTS.Common.CommonObject.Types.UnitType)i;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }


            return result;
        }

        /// <summary>
        /// 委托是否最终状态
        /// </summary>
        /// <param name="comboType">状态类型代码</param>
        /// <returns></returns>
        public static bool IsFinalState(string comboType)
        {
            string type = comboType.Substring(comboType.Length - 2, 2);
            int state = int.Parse(type);
            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            if (state == (int) TypesOrderStateType.DOSCanceled || state == (int) TypesOrderStateType.DOSPartRemoved
                || state == (int) TypesOrderStateType.DOSRemoved || state == (int) TypesOrderStateType.DOSDealed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 四舍五入小数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal Round(decimal val)
        {
            int len = 3;
            return Round(val, len);
        }

        /// <summary>
        /// 四舍五入小数
        /// </summary>
        /// <param name="val"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static decimal Round(decimal val, int len)
        {
            return Math.Round(val, len, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 验证是否是合法的数字格式
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool DecimalTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9]+$");
        }
    }
    
}