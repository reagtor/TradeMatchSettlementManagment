using System;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.DAL.Data.HK;
using ReckoningCounter.Entity.Model.HK;
using ReckoningCounter.Model;

namespace ReckoningCounter.BLL.Reckoning.Instantaneous
{
    /// <summary>
    /// 清算数据逻辑
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    class ReckonDataLogic
    {

        /// <summary>
        /// 依据期货委托单号获取委托单实体
        /// </summary>
        /// <param name="strEntrustId"></param>
        /// <returns></returns>
        public static QH_TodayEntrustTableInfo GetQHEntrustEntity(string strEntrustId)
        {
            QH_TodayEntrustTableInfo result = null;

            //var tetp = new SqlQhTodayEntrustTableProvider(TransactionFactory.RC_ConnectionString, true, string.Empty);

            QH_TodayEntrustTableDal qhtodayEntrustTabledDal = new QH_TodayEntrustTableDal();

            result = qhtodayEntrustTabledDal.GetModel(strEntrustId);

            return result;
        }

        /// <summary>
        /// 依据期货委托单号获取委托单实体
        /// </summary>
        /// <param name="strEntrustId"></param>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static QH_TodayEntrustTableInfo GetQHEntrustEntity(string strEntrustId, ReckoningTransaction tm)
        {
            QH_TodayEntrustTableInfo result = null;

            QH_TodayEntrustTableDal qhtodayEntrustTabledDal = new QH_TodayEntrustTableDal();

            result = qhtodayEntrustTabledDal.GetModel(strEntrustId);

            return result;
        }

        /// <summary>
        /// 创建期货委托标识
        /// </summary>
        /// <returns></returns>
        public static string BuildQHOrderNo()
        {
            //string strResult = DateTime.Now.ToString("yyMMddhhmmssfff");
            //strResult += QHCounterCache.Instance.NewDealOrderNo.ToString();
            //return strResult;

            DateTime now = DateTime.Now;

            //将小时由12小时制改为24小时
            //string strResult = DateTime.Now.ToString("yyMMddhhmmssfff");
            string strResult = now.ToString("yyMMdd");//"hhmmssfff");
            string hour = now.ToString("HH");
            //if(hour.Length == 1)
            //    hour = "0" + hour;

            strResult = strResult + hour + now.ToString("mmssfff");

            strResult += QHCounterCache.Instance.NewOrderNo.ToString();

            LogHelper.WriteDebug("----------***********创建期货委托单号BuildQHOrderNo:" + strResult);
            return strResult;
        }

        /// <summary>
        /// 创建现货委托标识
        /// </summary>
        /// <returns></returns>
        public static string BuildXHOrderNo()
        {
            DateTime now = DateTime.Now;

            //将小时由12小时制改为24小时
            //string strResult = DateTime.Now.ToString("yyMMddhhmmssfff");
            string strResult = now.ToString("yyMMdd");//"hhmmssfff");
            string hour = now.ToString("HH");
            //if(hour.Length == 1)
            //    hour = "0" + hour;

            strResult = strResult + hour + now.ToString("mmssfff");

            strResult += XHCounterCache.Instance.NewOrderNo.ToString();

            LogHelper.WriteDebug("----------***********创建现货委托单号BuildXHOrderNo:" + strResult);
            return strResult;
        }

        /// <summary>
        /// 创建港股委托标识
        /// </summary>
        /// <returns></returns>
        public static string BuildHKOrderNo()
        {
            DateTime now = DateTime.Now;

            //将小时由12小时制改为24小时
            //string strResult = DateTime.Now.ToString("yyMMddhhmmssfff");
            string strResult = now.ToString("yyMMdd");//"hhmmssfff");
            string hour = now.ToString("HH");
            //if(hour.Length == 1)
            //    hour = "0" + hour;

            strResult = strResult + hour + now.ToString("mmssfff");

            strResult += HKCounterCache.Instance.NewOrderNo.ToString();

            LogHelper.WriteDebug("----------***********创建港股委托单号BuildHKOrderNo:" + strResult);
            return strResult;
        }

        /// <summary>
        /// 依据现货委托单号获取委托单对象
        /// </summary>
        /// <param name="strEntrustId"></param>
        /// <returns></returns>
        public static XH_TodayEntrustTableInfo GetXHEntrustEntity(string strEntrustId)
        {
            XH_TodayEntrustTableInfo result = null;
            XH_TodayEntrustTableDal xh_TodayEntrustTableDal = new XH_TodayEntrustTableDal();


            result = xh_TodayEntrustTableDal.GetModel(strEntrustId);

            return result;
        }

        /// <summary>
        /// 依据港股委托单号获取委托单对象
        /// </summary>
        /// <param name="strEntrustId"></param>
        /// <returns></returns>
        public static HK_TodayEntrustInfo GetHKEntrustEntity(string strEntrustId)
        {
            HK_TodayEntrustInfo result = null;
            HK_TodayEntrustDal hkTodayEntrustDal = new HK_TodayEntrustDal();


            result = hkTodayEntrustDal.GetModel(strEntrustId);

            return result;
        }

        /// <summary>
        /// 依据现货委托单号获取委托单对象
        /// </summary>
        /// <param name="strEntrustId"></param>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static XH_TodayEntrustTableInfo GetXHEntrustEntity(string strEntrustId, ReckoningTransaction tm)
        {
            XH_TodayEntrustTableInfo result = null;

            XH_TodayEntrustTableDal xh_TodayEntrustTableDal = new XH_TodayEntrustTableDal();


            result = xh_TodayEntrustTableDal.GetModel(strEntrustId,tm);

            return result;
        }

        /// <summary>
        /// 依据港股委托单号获取委托单对象
        /// </summary>
        /// <param name="strEntrustId"></param>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static HK_TodayEntrustInfo GetHKEntrustEntity(string strEntrustId, ReckoningTransaction tm)
        {
            HK_TodayEntrustInfo result = null;

            HK_TodayEntrustDal hkTodayEntrustDal = new HK_TodayEntrustDal();


            result = hkTodayEntrustDal.GetModel(strEntrustId);//TODO:trans

            return result;
        }
       
    }
}
