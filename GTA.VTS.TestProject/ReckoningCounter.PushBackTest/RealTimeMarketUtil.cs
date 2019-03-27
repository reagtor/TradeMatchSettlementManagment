#region Using Namespace

using System;
using System.Windows.Forms;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using RealTime.Server.SModelData.HqData;
//using RealtimeMarket.factory;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public class RealTimeMarketUtil
    {
        private static RealTimeMarketUtil instance = new RealTimeMarketUtil();

        private IRealtimeMarketService service;

        private RealTimeMarketUtil()
        {
            service = GetRealMarketService();
        }

        public static RealTimeMarketUtil GetInstance()
        {
            if (instance == null)
                instance = new RealTimeMarketUtil();

            return instance;
        }

        public static IRealtimeMarketService GetRealMarketService()
        {
            int mode = Utils.GetRealTimeMode();

            IRealtimeMarketService service = null;
            //if (mode == 1)
                service = RealtimeMarketServiceFactory.GetService();
            //else if (mode == 2)
            //{
            //    service = RealtimeMarketServiceFactory2.GetService();
            //}

            return service;
        }

        /// <summary>
        /// 获取现货的最近成交价
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public decimal GetStockLastTrade(string code)
        {
            decimal result = -1;

            try
            {
                HqExData data = this.service.GetStockHqData(code);
                if (data == null)
                {
                    MessageBox.Show("GetStockHqData failure!");
                    return result;
                }

                if (data != null)
                {
                    HqExData exData = data;

                    result = (decimal) exData.HqData.Lasttrade;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 获取港股的最近成交价
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public decimal GetHKLastTrade(string code)
        {
            decimal result = -1;

            try
            {
                HKStock data = this.service.GetHKStockData(code);
                if (data == null)
                {
                    MessageBox.Show("GetHKLastTrade failure!");
                    return result;
                }

                if (data != null)
                {
                    HKStock exData = data;

                    result = (decimal) exData.Lasttrade;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// 获取期货货的最近成交价
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public decimal GetFutureLastTrade(string code)
        {
            decimal result = -1;

            try
            {
                var data = this.service.GetFutData(code);
                if (data == null)
                {
                    MessageBox.Show("GetFutData failure!");
                    return result;
                }

                if (data != null)
                {
                    var exData = data;

                    result = (decimal) exData.Lasttrade;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.WriteError(ex.Message, ex);
            }

            return result;
        }
    }
}