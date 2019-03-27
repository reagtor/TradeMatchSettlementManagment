using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using MatchCenter.Entity;
using MatchCenter.BLL.RealTime;
using MatchCenter.BLL.MatchData;
using Amib.Threading;
using CommonRealtimeMarket;
using RealTime.Server.SModelData.HqData;
using GTA.VTS.Common.CommonObject;

namespace MatchCenter.BLL.RealTime
{
    /// <summary>
    /// Desc: 模拟行情处理
    /// Create By: 董鹏
    /// Create Date: 2010-01-27
    /// </summary>
    public class RealtimeMarketSimulate
    {
        private static SmartThreadPool smartPool = new SmartThreadPool();

        static RealtimeMarketSimulate()
        {
            smartPool.MaxThreads = 80;
            smartPool.MinThreads = 20;
            smartPool.Start();

        }

        #region 发送模拟行情
        /// <summary>
        /// 发送现货模拟行情
        /// </summary>
        /// <param name="hqData">自定义行情</param>
        private static void SendStockQuotes(MarketDataInfo hqData)
        {
            if (hqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.xh_ActivityOrderDic.ContainsKey(hqData.Code))
            {
                return;
            }

            StockHqDataChangeEventArg arg = new StockHqDataChangeEventArg(MarketDataInfoConvertHqExData(hqData));

            smartPool.QueueWorkItem(RealtimeMarketService.FindStockMatchCode, arg);
        }

        /// <summary>
        /// 发送港股模拟行情
        /// </summary>
        /// <param name="hqData">自定义行情</param>
        private static void SendHKQuotes(MarketDataInfo hqData)
        {
            if (hqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.hk_ActivityOrderDic.ContainsKey(hqData.Code))
            {
                return;
            }

            HKStockDataChangeEventArg arg = new HKStockDataChangeEventArg(MarketDataInfoConvertHKStockData(hqData));

            smartPool.QueueWorkItem(RealtimeMarketService.FindHKStockMatchCode, arg);
        }

        /// <summary>
        /// 发送股指期货模拟行情
        /// </summary>
        /// <param name="hqData">自定义行情</param>
        private static void SendFutureQuotes(MarketDataInfo hqData)
        {
            if (hqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.qh_ActivityOrderDic.ContainsKey(hqData.Code))
            {
                return;
            }

            FutDataChangeEventArg arg = new FutDataChangeEventArg(MarketDataInfoConvertFutDataData(hqData));

            smartPool.QueueWorkItem(RealtimeMarketService.FindFuntureMatchCode, arg);
        }

        /// <summary>
        /// 发送商品期货模拟行情
        /// </summary>
        /// <param name="hqData">自定义行情</param>
        private static void SendCommoditiesQuotes(MarketDataInfo hqData)
        {
            if (hqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.spqh_ActivityOrderDic.ContainsKey(hqData.Code))
            {
                return;
            }

            MercantileFutDataChangeEventArg arg = new MercantileFutDataChangeEventArg(MarketDataInfoConvertMerFutData(hqData));

            smartPool.QueueWorkItem(RealtimeMarketService.FindCommditiesMatchCode, arg);
        }

        /// <summary>
        /// 发送股指期货模拟行情
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="Name">名称</param>
        /// <param name="BuyFirstPrice">买1价</param>
        /// <param name="BuyFirstVolume">买1量</param>
        /// <param name="BuySecondPrice">买2价</param>
        /// <param name="BuySecondVolume">买2量</param>
        /// <param name="BuyThirdPrice">买3价</param>
        /// <param name="BuyThirdVolume">买3量</param>
        /// <param name="BuyFourthPrice">买4价</param>
        /// <param name="BuyFourthVolume">买4量</param>
        /// <param name="BuyFivePrice">买5价</param>
        /// <param name="BuyFiveVolume">买5量</param>
        /// <param name="SellFirstPrice">卖1价</param>
        /// <param name="SellFirstVolume">卖1量</param>
        /// <param name="SellSecondPrice">卖2价</param>
        /// <param name="SellSecondVolume">卖2量</param>
        /// <param name="SellThirdPrice">卖3价</param>
        /// <param name="SellThirdVolume">卖3量</param>
        /// <param name="SellFourthPrice">卖4价</param>
        /// <param name="SellFourthVolume">卖4量</param>
        /// <param name="SellFivePrice">卖5价</param>
        /// <param name="SellFiveVolume">卖5量</param>
        /// <param name="LastPrice">成交价</param>
        /// <param name="LastVolume">成交量</param>
        /// <param name="LowerPrice">跌停价</param>
        /// <param name="UpPrice">涨停价</param>
        /// <param name="YesterPrice">昨日收盘价</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendCommoditiesQuotes(string Code, string Name, string
                BuyFirstPrice, string BuyFirstVolume, string BuySecondPrice, string BuySecondVolume, string BuyThirdPrice, string BuyThirdVolume, string BuyFourthPrice, string BuyFourthVolume, string BuyFivePrice, string BuyFiveVolume, string
                SellFirstPrice, string SellFirstVolume, string SellSecondPrice, string SellSecondVolume, string SellThirdPrice, string SellThirdVolume, string SellFourthPrice, string SellFourthVolume, string SellFivePrice, string SellFiveVolume, string
                LastPrice, string LastVolume, string LowerPrice, string UpPrice, string YesterPrice, string
                Batch, string Interval)
        {
            MarketDataInfo hqData = GetMarketDataInfo(Code, Name,
                BuyFirstPrice, BuyFirstVolume, BuySecondPrice, BuySecondVolume, BuyThirdPrice, BuyThirdVolume, BuyFourthPrice, BuyFourthVolume, BuyFivePrice, BuyFiveVolume,
                SellFirstPrice, SellFirstVolume, SellSecondPrice, SellSecondVolume, SellThirdPrice, SellThirdVolume, SellFourthPrice, SellFourthVolume, SellFivePrice, SellFiveVolume,
                LastPrice, LastVolume, LowerPrice, UpPrice, YesterPrice);
            int batch = 1;
            int interval = 1;
            if (!string.IsNullOrEmpty(Batch))
            {
                int.TryParse(Batch, out batch);
            }
            if (!string.IsNullOrEmpty(Interval))
            {
                int.TryParse(Interval, out interval);
            }
            for (int i = 0; i < batch; i++)
            {
                SendCommoditiesQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送现货模拟行情
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="Name">名称</param>
        /// <param name="BuyFirstPrice">买1价</param>
        /// <param name="BuyFirstVolume">买1量</param>
        /// <param name="BuySecondPrice">买2价</param>
        /// <param name="BuySecondVolume">买2量</param>
        /// <param name="BuyThirdPrice">买3价</param>
        /// <param name="BuyThirdVolume">买3量</param>
        /// <param name="BuyFourthPrice">买4价</param>
        /// <param name="BuyFourthVolume">买4量</param>
        /// <param name="BuyFivePrice">买5价</param>
        /// <param name="BuyFiveVolume">买5量</param>
        /// <param name="SellFirstPrice">卖1价</param>
        /// <param name="SellFirstVolume">卖1量</param>
        /// <param name="SellSecondPrice">卖2价</param>
        /// <param name="SellSecondVolume">卖2量</param>
        /// <param name="SellThirdPrice">卖3价</param>
        /// <param name="SellThirdVolume">卖3量</param>
        /// <param name="SellFourthPrice">卖4价</param>
        /// <param name="SellFourthVolume">卖4量</param>
        /// <param name="SellFivePrice">卖5价</param>
        /// <param name="SellFiveVolume">卖5量</param>
        /// <param name="LastPrice">成交价</param>
        /// <param name="LastVolume">成交量</param>
        /// <param name="LowerPrice">跌停价</param>
        /// <param name="UpPrice">涨停价</param>
        /// <param name="YesterPrice">昨日收盘价</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendStockQuotes(string Code, string Name, string
                BuyFirstPrice, string BuyFirstVolume, string BuySecondPrice, string BuySecondVolume, string BuyThirdPrice, string BuyThirdVolume, string BuyFourthPrice, string BuyFourthVolume, string BuyFivePrice, string BuyFiveVolume, string
                SellFirstPrice, string SellFirstVolume, string SellSecondPrice, string SellSecondVolume, string SellThirdPrice, string SellThirdVolume, string SellFourthPrice, string SellFourthVolume, string SellFivePrice, string SellFiveVolume, string
                LastPrice, string LastVolume, string LowerPrice, string UpPrice, string YesterPrice, string
                Batch, string Interval)
        {
            MarketDataInfo hqData = GetMarketDataInfo(Code, Name,
               BuyFirstPrice, BuyFirstVolume, BuySecondPrice, BuySecondVolume, BuyThirdPrice, BuyThirdVolume, BuyFourthPrice, BuyFourthVolume, BuyFivePrice, BuyFiveVolume,
               SellFirstPrice, SellFirstVolume, SellSecondPrice, SellSecondVolume, SellThirdPrice, SellThirdVolume, SellFourthPrice, SellFourthVolume, SellFivePrice, SellFiveVolume,
               LastPrice, LastVolume, LowerPrice, UpPrice, YesterPrice);
            int batch = 1;
            int interval = 1;
            if (!string.IsNullOrEmpty(Batch))
            {
                int.TryParse(Batch, out batch);
            }
            if (!string.IsNullOrEmpty(Interval))
            {
                int.TryParse(Interval, out interval);
            }
            for (int i = 0; i < batch; i++)
            {
                SendStockQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送股指期货模拟行情
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="Name">名称</param>
        /// <param name="BuyFirstPrice">买1价</param>
        /// <param name="BuyFirstVolume">买1量</param>
        /// <param name="BuySecondPrice">买2价</param>
        /// <param name="BuySecondVolume">买2量</param>
        /// <param name="BuyThirdPrice">买3价</param>
        /// <param name="BuyThirdVolume">买3量</param>
        /// <param name="BuyFourthPrice">买4价</param>
        /// <param name="BuyFourthVolume">买4量</param>
        /// <param name="BuyFivePrice">买5价</param>
        /// <param name="BuyFiveVolume">买5量</param>
        /// <param name="SellFirstPrice">卖1价</param>
        /// <param name="SellFirstVolume">卖1量</param>
        /// <param name="SellSecondPrice">卖2价</param>
        /// <param name="SellSecondVolume">卖2量</param>
        /// <param name="SellThirdPrice">卖3价</param>
        /// <param name="SellThirdVolume">卖3量</param>
        /// <param name="SellFourthPrice">卖4价</param>
        /// <param name="SellFourthVolume">卖4量</param>
        /// <param name="SellFivePrice">卖5价</param>
        /// <param name="SellFiveVolume">卖5量</param>
        /// <param name="LastPrice">成交价</param>
        /// <param name="LastVolume">成交量</param>
        /// <param name="LowerPrice">跌停价</param>
        /// <param name="UpPrice">涨停价</param>
        /// <param name="YesterPrice">昨日收盘价</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendFutureQuotes(string Code, string Name, string
                BuyFirstPrice, string BuyFirstVolume, string BuySecondPrice, string BuySecondVolume, string BuyThirdPrice, string BuyThirdVolume, string BuyFourthPrice, string BuyFourthVolume, string BuyFivePrice, string BuyFiveVolume, string
                SellFirstPrice, string SellFirstVolume, string SellSecondPrice, string SellSecondVolume, string SellThirdPrice, string SellThirdVolume, string SellFourthPrice, string SellFourthVolume, string SellFivePrice, string SellFiveVolume, string
                LastPrice, string LastVolume, string LowerPrice, string UpPrice, string YesterPrice, string
                Batch, string Interval)
        {
            MarketDataInfo hqData = GetMarketDataInfo(Code, Name,
              BuyFirstPrice, BuyFirstVolume, BuySecondPrice, BuySecondVolume, BuyThirdPrice, BuyThirdVolume, BuyFourthPrice, BuyFourthVolume, BuyFivePrice, BuyFiveVolume,
              SellFirstPrice, SellFirstVolume, SellSecondPrice, SellSecondVolume, SellThirdPrice, SellThirdVolume, SellFourthPrice, SellFourthVolume, SellFivePrice, SellFiveVolume,
              LastPrice, LastVolume, LowerPrice, UpPrice, YesterPrice);
            int batch = 1;
            int interval = 1;
            if (!string.IsNullOrEmpty(Batch))
            {
                int.TryParse(Batch, out batch);
            }
            if (!string.IsNullOrEmpty(Interval))
            {
                int.TryParse(Interval, out interval);
            }
            for (int i = 0; i < batch; i++)
            {
                SendFutureQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送港股模拟行情
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="Name">名称</param>
        /// <param name="BuyFirstPrice">买1价</param>
        /// <param name="BuyFirstVolume">买1量</param>
        /// <param name="BuySecondPrice">买2价</param>
        /// <param name="BuySecondVolume">买2量</param>
        /// <param name="BuyThirdPrice">买3价</param>
        /// <param name="BuyThirdVolume">买3量</param>
        /// <param name="BuyFourthPrice">买4价</param>
        /// <param name="BuyFourthVolume">买4量</param>
        /// <param name="BuyFivePrice">买5价</param>
        /// <param name="BuyFiveVolume">买5量</param>
        /// <param name="SellFirstPrice">卖1价</param>
        /// <param name="SellFirstVolume">卖1量</param>
        /// <param name="SellSecondPrice">卖2价</param>
        /// <param name="SellSecondVolume">卖2量</param>
        /// <param name="SellThirdPrice">卖3价</param>
        /// <param name="SellThirdVolume">卖3量</param>
        /// <param name="SellFourthPrice">卖4价</param>
        /// <param name="SellFourthVolume">卖4量</param>
        /// <param name="SellFivePrice">卖5价</param>
        /// <param name="SellFiveVolume">卖5量</param>
        /// <param name="LastPrice">成交价</param>
        /// <param name="LastVolume">成交量</param>
        /// <param name="LowerPrice">跌停价</param>
        /// <param name="UpPrice">涨停价</param>
        /// <param name="YesterPrice">昨日收盘价</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendHKQuotes(string Code, string Name, string
                BuyFirstPrice, string BuyFirstVolume, string BuySecondPrice, string BuySecondVolume, string BuyThirdPrice, string BuyThirdVolume, string BuyFourthPrice, string BuyFourthVolume, string BuyFivePrice, string BuyFiveVolume, string
                SellFirstPrice, string SellFirstVolume, string SellSecondPrice, string SellSecondVolume, string SellThirdPrice, string SellThirdVolume, string SellFourthPrice, string SellFourthVolume, string SellFivePrice, string SellFiveVolume, string
                LastPrice, string LastVolume, string LowerPrice, string UpPrice, string YesterPrice, string
                Batch, string Interval)
        {
            MarketDataInfo hqData = GetMarketDataInfo(Code, Name,
             BuyFirstPrice, BuyFirstVolume, BuySecondPrice, BuySecondVolume, BuyThirdPrice, BuyThirdVolume, BuyFourthPrice, BuyFourthVolume, BuyFivePrice, BuyFiveVolume,
             SellFirstPrice, SellFirstVolume, SellSecondPrice, SellSecondVolume, SellThirdPrice, SellThirdVolume, SellFourthPrice, SellFourthVolume, SellFivePrice, SellFiveVolume,
             LastPrice, LastVolume, LowerPrice, UpPrice, YesterPrice);
            int batch = 1;
            int interval = 1;
            if (!string.IsNullOrEmpty(Batch))
            {
                int.TryParse(Batch, out batch);
            }
            if (!string.IsNullOrEmpty(Interval))
            {
                int.TryParse(Interval, out interval);
            }
            for (int i = 0; i < batch; i++)
            {
                SendHKQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送现货模拟行情
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendStockQuotes(IRealtimeMarketView view, int Batch, int Interval)
        {
            view.BreedClassType = Types.BreedClassTypeEnum.Stock;
            MarketDataInfo hqData = GetMarketDataInfo(view.hqCode, view.hqName, view.hqBuyFirstPrice.ToString(), view.hqBuyFirstVolume.ToString(), view.hqBuySecondPrice.ToString(), view.hqBuySecondVolume.ToString(), view.hqBuyThirdPrice.ToString(), view.hqBuyThirdVolume.ToString(), view.hqBuyFourthPrice.ToString(), view.hqBuyFourthVolume.ToString(), view.hqBuyFivePrice.ToString(), view.hqBuyFiveVolume.ToString(), view.hqSellFirstPrice.ToString(), view.hqSellFirstVolume.ToString(), view.hqSellSecondPrice.ToString(), view.hqSellSecondVolume.ToString(), view.hqSellThirdPrice.ToString(), view.hqSellThirdVolume.ToString(), view.hqSellFourthPrice.ToString(), view.hqSellFourthVolume.ToString(), view.hqSellFivePrice.ToString(), view.hqSellFiveVolume.ToString(), view.hqLastPrice.ToString(), view.hqLastVolume.ToString(), view.hqLowerPrice.ToString(), view.hqUpPrice.ToString(), view.hqYesterPrice.ToString());

            int batch = 1;
            int interval = 1;
            if (Batch > 0)
            {
                batch = Batch;
            }
            if (Interval > 0)
            {
                interval = Interval;
            }
            for (int i = 0; i < batch; i++)
            {
                SendStockQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送港股模拟行情
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendHKQuotes(IRealtimeMarketView view, int Batch, int Interval)
        {
            view.BreedClassType = Types.BreedClassTypeEnum.HKStock;
            MarketDataInfo hqData = GetMarketDataInfo(view.hqCode, view.hqName, view.hqBuyFirstPrice.ToString(), view.hqBuyFirstVolume.ToString(), view.hqBuySecondPrice.ToString(), view.hqBuySecondVolume.ToString(), view.hqBuyThirdPrice.ToString(), view.hqBuyThirdVolume.ToString(), view.hqBuyFourthPrice.ToString(), view.hqBuyFourthVolume.ToString(), view.hqBuyFivePrice.ToString(), view.hqBuyFiveVolume.ToString(), view.hqSellFirstPrice.ToString(), view.hqSellFirstVolume.ToString(), view.hqSellSecondPrice.ToString(), view.hqSellSecondVolume.ToString(), view.hqSellThirdPrice.ToString(), view.hqSellThirdVolume.ToString(), view.hqSellFourthPrice.ToString(), view.hqSellFourthVolume.ToString(), view.hqSellFivePrice.ToString(), view.hqSellFiveVolume.ToString(), view.hqLastPrice.ToString(), view.hqLastVolume.ToString(), view.hqLowerPrice.ToString(), view.hqUpPrice.ToString(), view.hqYesterPrice.ToString());

            int batch = 1;
            int interval = 1;
            if (Batch > 0)
            {
                batch = Batch;
            }
            if (Interval > 0)
            {
                interval = Interval;
            }
            for (int i = 0; i < batch; i++)
            {
                SendHKQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送股指期货模拟行情
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendFutureQuotes(IRealtimeMarketView view, int Batch, int Interval)
        {
            view.BreedClassType = Types.BreedClassTypeEnum.StockIndexFuture;
            MarketDataInfo hqData = GetMarketDataInfo(view.hqCode, view.hqName, view.hqBuyFirstPrice.ToString(), view.hqBuyFirstVolume.ToString(), view.hqBuySecondPrice.ToString(), view.hqBuySecondVolume.ToString(), view.hqBuyThirdPrice.ToString(), view.hqBuyThirdVolume.ToString(), view.hqBuyFourthPrice.ToString(), view.hqBuyFourthVolume.ToString(), view.hqBuyFivePrice.ToString(), view.hqBuyFiveVolume.ToString(), view.hqSellFirstPrice.ToString(), view.hqSellFirstVolume.ToString(), view.hqSellSecondPrice.ToString(), view.hqSellSecondVolume.ToString(), view.hqSellThirdPrice.ToString(), view.hqSellThirdVolume.ToString(), view.hqSellFourthPrice.ToString(), view.hqSellFourthVolume.ToString(), view.hqSellFivePrice.ToString(), view.hqSellFiveVolume.ToString(), view.hqLastPrice.ToString(), view.hqLastVolume.ToString(), view.hqLowerPrice.ToString(), view.hqUpPrice.ToString(), view.hqYesterPrice.ToString());

            int batch = 1;
            int interval = 1;
            if (Batch > 0)
            {
                batch = Batch;
            }
            if (Interval > 0)
            {
                interval = Interval;
            }
            for (int i = 0; i < batch; i++)
            {
                SendFutureQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        /// <summary>
        /// 发送商品期货模拟行情
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="Batch">批量数</param>
        /// <param name="Interval">批量发送间隔时间</param>
        public static void SendCommoditiesQuotes(IRealtimeMarketView view, int Batch, int Interval)
        {
            view.BreedClassType = Types.BreedClassTypeEnum.CommodityFuture;
            MarketDataInfo hqData = GetMarketDataInfo(view.hqCode, view.hqName, view.hqBuyFirstPrice.ToString(), view.hqBuyFirstVolume.ToString(), view.hqBuySecondPrice.ToString(), view.hqBuySecondVolume.ToString(), view.hqBuyThirdPrice.ToString(), view.hqBuyThirdVolume.ToString(), view.hqBuyFourthPrice.ToString(), view.hqBuyFourthVolume.ToString(), view.hqBuyFivePrice.ToString(), view.hqBuyFiveVolume.ToString(), view.hqSellFirstPrice.ToString(), view.hqSellFirstVolume.ToString(), view.hqSellSecondPrice.ToString(), view.hqSellSecondVolume.ToString(), view.hqSellThirdPrice.ToString(), view.hqSellThirdVolume.ToString(), view.hqSellFourthPrice.ToString(), view.hqSellFourthVolume.ToString(), view.hqSellFivePrice.ToString(), view.hqSellFiveVolume.ToString(), view.hqLastPrice.ToString(), view.hqLastVolume.ToString(), view.hqLowerPrice.ToString(), view.hqUpPrice.ToString(), view.hqYesterPrice.ToString());

            int batch = 1;
            int interval = 1;
            if (Batch > 0)
            {
                batch = Batch;
            }
            if (Interval > 0)
            {
                interval = Interval;
            }
            for (int i = 0; i < batch; i++)
            {
                SendCommoditiesQuotes(hqData);
                if (batch > 1 && i < (batch - 1))
                {
                    Thread.Sleep(interval * 1000);
                }
            }
        }

        #endregion

        #region 自定义实体转换成行情实体
        /// <summary>
        /// 自定义实体转换成现货行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static HqExData MarketDataInfoConvertHqExData(MarketDataInfo hqData)
        {
            HqExData exdata = new HqExData();
            HqData data = new HqData();
            data.CodeKey = hqData.Code;
            //data.Stockno = hqData.Name;

            #region 买价
            data.Buyprice1 = (float)hqData.BuyFirstPrice;
            data.Buyprice2 = (float)hqData.BuySecondPrice;
            data.Buyprice3 = (float)hqData.BuyThirdPrice;
            data.Buyprice4 = (float)hqData.BuyFourthPrice;
            data.Buyprice5 = (float)hqData.BuyFivePrice;
            #endregion

            #region 买量
            data.Buyvol1 = (float)hqData.BuyFirstVolume;
            data.Buyvol2 = (float)hqData.BuySecondVolume;
            data.Buyvol3 = (float)hqData.BuyThirdVolume;
            data.Buyvol4 = (float)hqData.BuyFourthVolume;
            data.Buyvol5 = (float)hqData.BuyFiveVolume;
            #endregion

            #region 卖价
            data.Sellprice1 = (float)hqData.SellFirstPrice;
            data.Sellprice2 = (float)hqData.SellSecondPrice;
            data.Sellprice3 = (float)hqData.SellThirdPrice;
            data.Sellprice4 = (float)hqData.SellFourthPrice;
            data.Sellprice5 = (float)hqData.SellFivePrice;
            #endregion

            #region 卖量
            data.Sellvol1 = (float)hqData.SellFirstVolume;
            data.Sellvol2 = (float)hqData.SellSecondVolume;
            data.Sellvol3 = (float)hqData.SellThirdVolume;
            data.Sellvol4 = (float)hqData.SellFourthVolume;
            data.Sellvol5 = (float)hqData.SellFiveVolume;
            #endregion

            data.Lasttrade = (float)hqData.LastPrice;
            exdata.LastVolume = (float)hqData.LastVolume;
            exdata.Low = (float)hqData.LowerPrice;
            exdata.UpB = (float)hqData.UpPrice;
            exdata.YClose = (float)hqData.YesterPrice;
            exdata.HqData = data;
            return exdata;
        }

        /// <summary>
        /// 自定义实体转换成商品期货行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static MerFutData MarketDataInfoConvertMerFutData(MarketDataInfo hqData)
        {
            MerFutData data = new MerFutData();
            data.ContractID = hqData.Code;
            data.StrName = hqData.Name;

            #region 买价
            data.Buyprice1 = (double)hqData.BuyFirstPrice;
            data.Buyprice2 = (double)hqData.BuySecondPrice;
            data.Buyprice3 = (double)hqData.BuyThirdPrice;
            data.Buyprice4 = (double)hqData.BuyFourthPrice;
            data.Buyprice5 = (double)hqData.BuyFivePrice;
            #endregion

            #region 买量
            data.Buyvol1 = (double)hqData.BuyFirstVolume;
            data.Buyvol2 = (double)hqData.BuySecondVolume;
            data.Buyvol3 = (double)hqData.BuyThirdVolume;
            data.Buyvol4 = (double)hqData.BuyFourthVolume;
            data.Buyvol5 = (double)hqData.BuyFiveVolume;
            #endregion

            #region 卖价
            data.Sellprice1 = (double)hqData.SellFirstPrice;
            data.Sellprice2 = (double)hqData.SellSecondPrice;
            data.Sellprice3 = (double)hqData.SellThirdPrice;
            data.Sellprice4 = (double)hqData.SellFourthPrice;
            data.Sellprice5 = (double)hqData.SellFivePrice;
            #endregion

            #region 卖量
            data.Sellvol1 = (double)hqData.SellFirstVolume;
            data.Sellvol2 = (double)hqData.SellSecondVolume;
            data.Sellvol3 = (double)hqData.SellThirdVolume;
            data.Sellvol4 = (double)hqData.SellFourthVolume;
            data.Sellvol5 = (double)hqData.SellFiveVolume;
            #endregion

            data.Lasttrade = (double)hqData.LastPrice;
            data.PTrans = (double)hqData.LastVolume;
            data.LowerLimit = (double)hqData.LowerPrice;
            data.UpperLimit = (double)hqData.UpPrice;
            data.PreClosePrice = (double)hqData.YesterPrice;
            return data;
        }

        /// <summary>
        /// 自定义实体转换成股指期货行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static FutData MarketDataInfoConvertFutDataData(MarketDataInfo hqData)
        {
            FutData data = new FutData();
            data.CodeKey = hqData.Code;
            //data.Stockno = hqData.Name;

            #region 买价
            data.Buyprice1 = (double)hqData.BuyFirstPrice;
            data.Buyprice2 = (double)hqData.BuySecondPrice;
            data.Buyprice3 = (double)hqData.BuyThirdPrice;
            data.Buyprice4 = (double)hqData.BuyFourthPrice;
            data.Buyprice5 = (double)hqData.BuyFivePrice;
            #endregion

            #region 买量
            data.Buyvol1 = (double)hqData.BuyFirstVolume;
            data.Buyvol2 = (double)hqData.BuySecondVolume;
            data.Buyvol3 = (double)hqData.BuyThirdVolume;
            data.Buyvol4 = (double)hqData.BuyFourthVolume;
            data.Buyvol5 = (double)hqData.BuyFiveVolume;
            #endregion

            #region 卖价
            data.Sellprice1 = (double)hqData.SellFirstPrice;
            data.Sellprice2 = (double)hqData.SellSecondPrice;
            data.Sellprice3 = (double)hqData.SellThirdPrice;
            data.Sellprice4 = (double)hqData.SellFourthPrice;
            data.Sellprice5 = (double)hqData.SellFivePrice;
            #endregion

            #region 卖量
            data.Sellvol1 = (double)hqData.SellFirstVolume;
            data.Sellvol2 = (double)hqData.SellSecondVolume;
            data.Sellvol3 = (double)hqData.SellThirdVolume;
            data.Sellvol4 = (double)hqData.SellFourthVolume;
            data.Sellvol5 = (double)hqData.SellFiveVolume;
            #endregion

            data.Lasttrade = (double)hqData.LastPrice;
            data.LastVolume = (double)hqData.LastVolume;
            data.LowerLimitPrice = (double)hqData.LowerPrice;
            data.UpperLimitPrice = (double)hqData.UpPrice;
            data.Yclose = (double)hqData.YesterPrice;
            return data;
        }

        /// <summary>
        /// 自定义实体转换成港股行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static HKStock MarketDataInfoConvertHKStockData(MarketDataInfo hqData)
        {
            HKStock data = new HKStock();
            data.StockNo = hqData.Code;
            data.StrName = hqData.Name;

            #region 买价
            data.Buyprice1 = (float)hqData.BuyFirstPrice;
            data.Buyprice2 = (float)hqData.BuySecondPrice;
            data.Buyprice3 = (float)hqData.BuyThirdPrice;
            data.Buyprice4 = (float)hqData.BuyFourthPrice;
            data.Buyprice5 = (float)hqData.BuyFivePrice;
            #endregion

            #region 买量
            data.Buyvol1 = (int)hqData.BuyFirstVolume;
            data.Buyvol2 = (int)hqData.BuySecondVolume;
            data.Buyvol3 = (int)hqData.BuyThirdVolume;
            data.Buyvol4 = (int)hqData.BuyFourthVolume;
            data.Buyvol5 = (int)hqData.BuyFiveVolume;
            #endregion

            #region 卖价
            data.Sellprice1 = (float)hqData.SellFirstPrice;
            data.Sellprice2 = (float)hqData.SellSecondPrice;
            data.Sellprice3 = (float)hqData.SellThirdPrice;
            data.Sellprice4 = (float)hqData.SellFourthPrice;
            data.Sellprice5 = (float)hqData.SellFivePrice;
            #endregion

            #region 卖量
            data.Sellvol1 = (int)hqData.SellFirstVolume;
            data.Sellvol2 = (int)hqData.SellSecondVolume;
            data.Sellvol3 = (int)hqData.SellThirdVolume;
            data.Sellvol4 = (int)hqData.SellFourthVolume;
            data.Sellvol5 = (int)hqData.SellFiveVolume;
            #endregion

            data.Lasttrade = (float)hqData.LastPrice;
            data.PTrans = (long)hqData.LastVolume;
            data.Low = (float)hqData.LowerPrice;
            data.High = (float)hqData.UpPrice;
            data.ClosePrice = (float)hqData.YesterPrice;
            return data;
        }
        #endregion

        #region 行情实体转换成自定义实体
        /// <summary>
        /// 现货行情转换成自定义行情实体
        /// </summary>
        /// <param name="data">自定义实体</param>
        /// <returns></returns>
        public static MarketDataInfo HqExDataConvertMarketDataInfo(HqExData data)
        {
            MarketDataInfo level = new MarketDataInfo();
            level.Code = data.CodeKey;
            level.Name = data.Name;
            HqData hqData = data.HqData;

            #region 买价
            level.BuyFirstPrice = (decimal)hqData.Buyprice1;
            level.BuySecondPrice = (decimal)hqData.Buyprice2;
            level.BuyThirdPrice = (decimal)hqData.Buyprice3;
            level.BuyFourthPrice = (decimal)hqData.Buyprice4;
            level.BuyFivePrice = (decimal)hqData.Buyprice5;
            #endregion

            #region 买量
            level.BuyFirstVolume = (decimal)hqData.Buyvol1;
            level.BuySecondVolume = (decimal)hqData.Buyvol2;
            level.BuyThirdVolume = (decimal)hqData.Buyvol3;
            level.BuyFourthVolume = (decimal)hqData.Buyvol4;
            level.BuyFiveVolume = (decimal)hqData.Buyvol5;
            #endregion

            #region 卖价
            level.SellFirstPrice = (decimal)hqData.Sellprice1;
            level.SellSecondPrice = (decimal)hqData.Sellprice2;
            level.SellThirdPrice = (decimal)hqData.Sellprice3;
            level.SellFourthPrice = (decimal)hqData.Sellprice4;
            level.SellFivePrice = (decimal)hqData.Sellprice5;
            #endregion

            #region 卖量
            level.SellFirstVolume = (decimal)hqData.Sellvol1;
            level.SellSecondVolume = (decimal)hqData.Sellvol2;
            level.SellThirdVolume = (decimal)hqData.Sellvol3;
            level.SellFourthVolume = (decimal)hqData.Sellvol4;
            level.SellFiveVolume = (decimal)hqData.Sellvol5;
            #endregion

            level.LastPrice = decimal.Parse(hqData.Lasttrade.ToString());
            level.LastVolume = decimal.Parse(data.LastVolume.ToString());
            level.LowerPrice = decimal.Parse(data.LowB.ToString());
            level.UpPrice = decimal.Parse(data.UpB.ToString());
            level.YesterPrice = decimal.Parse(data.YClose.ToString());
            return level;
        }

        /// <summary>
        /// 商品期货行情转换成自定义行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static MarketDataInfo MerFutDataConvertMarketDataInfo(MerFutData hqData)
        {
            MarketDataInfo level = new MarketDataInfo();
            level.Code = hqData.CodeKey;
            level.Name = hqData.StrName;

            #region 买价
            level.BuyFirstPrice = (decimal)hqData.Buyprice1;
            level.BuySecondPrice = (decimal)hqData.Buyprice2;
            level.BuyThirdPrice = (decimal)hqData.Buyprice3;
            level.BuyFourthPrice = (decimal)hqData.Buyprice4;
            level.BuyFivePrice = (decimal)hqData.Buyprice5;
            #endregion

            #region 买量
            level.BuyFirstVolume = (decimal)hqData.Buyvol1;
            level.BuySecondVolume = (decimal)hqData.Buyvol2;
            level.BuyThirdVolume = (decimal)hqData.Buyvol3;
            level.BuyFourthVolume = (decimal)hqData.Buyvol4;
            level.BuyFiveVolume = (decimal)hqData.Buyvol5;
            #endregion

            #region 卖价
            level.SellFirstPrice = (decimal)hqData.Sellprice1;
            level.SellSecondPrice = (decimal)hqData.Sellprice2;
            level.SellThirdPrice = (decimal)hqData.Sellprice3;
            level.SellFourthPrice = (decimal)hqData.Sellprice4;
            level.SellFivePrice = (decimal)hqData.Sellprice5;
            #endregion

            #region 卖量
            level.SellFirstVolume = (decimal)hqData.Sellvol1;
            level.SellSecondVolume = (decimal)hqData.Sellvol2;
            level.SellThirdVolume = (decimal)hqData.Sellvol3;
            level.SellFourthVolume = (decimal)hqData.Sellvol4;
            level.SellFiveVolume = (decimal)hqData.Sellvol5;
            #endregion

            level.LastPrice = decimal.Parse(hqData.Lasttrade.ToString());
            level.LastVolume = decimal.Parse(hqData.PTrans.ToString());
            level.LowerPrice = decimal.Parse(hqData.LowerLimit.ToString());
            level.UpPrice = decimal.Parse(hqData.UpperLimit.ToString());
            level.YesterPrice = decimal.Parse(hqData.PreClosePrice.ToString());
            return level;
        }

        /// <summary>
        /// 股指期货行情转换成自定义行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static MarketDataInfo FutDataDataConvertMarketDataInfo(FutData hqData)
        {
            MarketDataInfo level = new MarketDataInfo();
            level.Code = hqData.CodeKey;
            level.Name = hqData.Stockno;

            #region 买价
            level.BuyFirstPrice = (decimal)hqData.Buyprice1;
            level.BuySecondPrice = (decimal)hqData.Buyprice2;
            level.BuyThirdPrice = (decimal)hqData.Buyprice3;
            level.BuyFourthPrice = (decimal)hqData.Buyprice4;
            level.BuyFivePrice = (decimal)hqData.Buyprice5;
            #endregion

            #region 买量
            level.BuyFirstVolume = (decimal)hqData.Buyvol1;
            level.BuySecondVolume = (decimal)hqData.Buyvol2;
            level.BuyThirdVolume = (decimal)hqData.Buyvol3;
            level.BuyFourthVolume = (decimal)hqData.Buyvol4;
            level.BuyFiveVolume = (decimal)hqData.Buyvol5;
            #endregion

            #region 卖价
            level.SellFirstPrice = (decimal)hqData.Sellprice1;
            level.SellSecondPrice = (decimal)hqData.Sellprice2;
            level.SellThirdPrice = (decimal)hqData.Sellprice3;
            level.SellFourthPrice = (decimal)hqData.Sellprice4;
            level.SellFivePrice = (decimal)hqData.Sellprice5;
            #endregion

            #region 卖量
            level.SellFirstVolume = (decimal)hqData.Sellvol1;
            level.SellSecondVolume = (decimal)hqData.Sellvol2;
            level.SellThirdVolume = (decimal)hqData.Sellvol3;
            level.SellFourthVolume = (decimal)hqData.Sellvol4;
            level.SellFiveVolume = (decimal)hqData.Sellvol5;
            #endregion

            level.LastPrice = decimal.Parse(hqData.Lasttrade.ToString());
            level.LastVolume = decimal.Parse(hqData.LastVolume.ToString());
            level.LowerPrice = decimal.Parse(hqData.LowerLimitPrice.ToString());
            level.UpPrice = decimal.Parse(hqData.UpperLimitPrice.ToString());
            level.YesterPrice = decimal.Parse(hqData.Yclose.ToString());
            return level;
        }

        /// <summary>
        /// 港股行情转换成自定义行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public static MarketDataInfo HKStockDataConvertMarketDataInfo(HKStock hqData)
        {
            MarketDataInfo level = new MarketDataInfo();
            level.Code = hqData.CodeKey;
            level.Name = hqData.StrName;

            #region 买价
            level.BuyFirstPrice = (decimal)hqData.Buyprice1;
            level.BuySecondPrice = (decimal)hqData.Buyprice2;
            level.BuyThirdPrice = (decimal)hqData.Buyprice3;
            level.BuyFourthPrice = (decimal)hqData.Buyprice4;
            level.BuyFivePrice = (decimal)hqData.Buyprice5;
            #endregion

            #region 买量
            level.BuyFirstVolume = (decimal)hqData.Buyvol1;
            level.BuySecondVolume = (decimal)hqData.Buyvol2;
            level.BuyThirdVolume = (decimal)hqData.Buyvol3;
            level.BuyFourthVolume = (decimal)hqData.Buyvol4;
            level.BuyFiveVolume = (decimal)hqData.Buyvol5;
            #endregion

            #region 卖价
            level.SellFirstPrice = (decimal)hqData.Sellprice1;
            level.SellSecondPrice = (decimal)hqData.Sellprice2;
            level.SellThirdPrice = (decimal)hqData.Sellprice3;
            level.SellFourthPrice = (decimal)hqData.Sellprice4;
            level.SellFivePrice = (decimal)hqData.Sellprice5;
            #endregion

            #region 卖量
            level.SellFirstVolume = (decimal)hqData.Sellvol1;
            level.SellSecondVolume = (decimal)hqData.Sellvol2;
            level.SellThirdVolume = (decimal)hqData.Sellvol3;
            level.SellFourthVolume = (decimal)hqData.Sellvol4;
            level.SellFiveVolume = (decimal)hqData.Sellvol5;
            #endregion

            level.LastPrice = decimal.Parse(hqData.Lasttrade.ToString());
            level.LastVolume = decimal.Parse(hqData.PTrans.ToString());
            level.LowerPrice = decimal.Parse(hqData.Low.ToString());
            level.UpPrice = decimal.Parse(hqData.High.ToString());
            level.YesterPrice = decimal.Parse(hqData.ClosePrice.ToString());
            return level;
        }
        #endregion

        #region 根据接收参数组装自定义行情实体
        /// <summary>
        /// 获取行情实体
        /// </summary>
        /// <param name="Code">代码</param>
        /// <param name="Name">名称</param>
        /// <param name="BuyFirstPrice">买1价</param>
        /// <param name="BuyFirstVolume">买1量</param>
        /// <param name="BuySecondPrice">买2价</param>
        /// <param name="BuySecondVolume">买2量</param>
        /// <param name="BuyThirdPrice">买3价</param>
        /// <param name="BuyThirdVolume">买3量</param>
        /// <param name="BuyFourthPrice">买4价</param>
        /// <param name="BuyFourthVolume">买4量</param>
        /// <param name="BuyFivePrice">买5价</param>
        /// <param name="BuyFiveVolume">买5量</param>
        /// <param name="SellFirstPrice">卖1价</param>
        /// <param name="SellFirstVolume">卖1量</param>
        /// <param name="SellSecondPrice">卖2价</param>
        /// <param name="SellSecondVolume">卖2量</param>
        /// <param name="SellThirdPrice">卖3价</param>
        /// <param name="SellThirdVolume">卖3量</param>
        /// <param name="SellFourthPrice">卖4价</param>
        /// <param name="SellFourthVolume">卖4量</param>
        /// <param name="SellFivePrice">卖5价</param>
        /// <param name="SellFiveVolume">卖5量</param>
        /// <param name="LastPrice">成交价</param>
        /// <param name="LastVolume">成交量</param>
        /// <param name="LowerPrice">跌停价</param>
        /// <param name="UpPrice">涨停价</param>
        /// <param name="YesterPrice">昨日收盘价</param>
        /// <returns>行情数据实体</returns>
        private static MarketDataInfo GetMarketDataInfo(string Code, string Name, string
                BuyFirstPrice, string BuyFirstVolume, string BuySecondPrice, string BuySecondVolume, string BuyThirdPrice, string BuyThirdVolume, string BuyFourthPrice, string BuyFourthVolume, string BuyFivePrice, string BuyFiveVolume, string
                SellFirstPrice, string SellFirstVolume, string SellSecondPrice, string SellSecondVolume, string SellThirdPrice, string SellThirdVolume, string SellFourthPrice, string SellFourthVolume, string SellFivePrice, string SellFiveVolume, string
                LastPrice, string LastVolume, string LowerPrice, string UpPrice, string YesterPrice)
        {
            MarketDataInfo hqData = new MarketDataInfo();
            hqData.Code = Code;
            hqData.Name = Name;
            hqData.BuyFirstPrice = Convert.ToDecimal(BuyFirstPrice);
            hqData.BuyFirstVolume = Convert.ToDecimal(BuyFirstVolume);
            hqData.BuySecondPrice = Convert.ToDecimal(BuySecondPrice);
            hqData.BuySecondVolume = Convert.ToDecimal(BuySecondVolume);
            hqData.BuyThirdPrice = Convert.ToDecimal(BuyThirdPrice);
            hqData.BuyThirdVolume = Convert.ToDecimal(BuyThirdVolume);
            hqData.BuyFourthPrice = Convert.ToDecimal(BuyFourthPrice);
            hqData.BuyFourthVolume = Convert.ToDecimal(BuyFourthVolume);
            hqData.BuyFivePrice = Convert.ToDecimal(BuyFivePrice);
            hqData.BuyFiveVolume = Convert.ToDecimal(BuyFiveVolume);
            hqData.SellFirstPrice = Convert.ToDecimal(SellFirstPrice);
            hqData.SellFirstVolume = Convert.ToDecimal(SellFirstVolume);
            hqData.SellSecondPrice = Convert.ToDecimal(SellSecondPrice);
            hqData.SellSecondVolume = Convert.ToDecimal(SellSecondVolume);
            hqData.SellThirdPrice = Convert.ToDecimal(SellThirdPrice);
            hqData.SellThirdVolume = Convert.ToDecimal(SellThirdVolume);
            hqData.SellFourthPrice = Convert.ToDecimal(SellFourthPrice);
            hqData.SellFourthVolume = Convert.ToDecimal(SellFourthVolume);
            hqData.SellFivePrice = Convert.ToDecimal(SellFivePrice);
            hqData.SellFiveVolume = Convert.ToDecimal(SellFiveVolume);
            hqData.LastPrice = Convert.ToDecimal(LastPrice);
            hqData.LastVolume = Convert.ToDecimal(LastVolume);
            hqData.LowerPrice = Convert.ToDecimal(LowerPrice);
            hqData.UpPrice = Convert.ToDecimal(UpPrice);
            hqData.YesterPrice = Convert.ToDecimal(YesterPrice);

            return hqData;
        }

        #endregion

        #region 绑定行情数据视图
        /// <summary>
        /// 绑定行情数据
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="hqData">行情数据实体</param>
        /// <param name="breedClassType">品种类型</param>
        private static void BindHqDataValue(IRealtimeMarketView view, MarketDataInfo hqData, Types.BreedClassTypeEnum breedClassType)
        {
            view.BreedClassType = breedClassType;
            view.hqCode = hqData.Code;
            view.hqName = hqData.Name;
            view.hqBuyFirstPrice = hqData.BuyFirstPrice;
            view.hqBuyFirstVolume = hqData.BuyFirstVolume;
            view.hqBuySecondPrice = hqData.BuySecondPrice;
            view.hqBuySecondVolume = hqData.BuySecondVolume;
            view.hqBuyThirdPrice = hqData.BuyThirdPrice;
            view.hqBuyThirdVolume = hqData.BuyThirdVolume;
            view.hqBuyFourthPrice = hqData.BuyFourthPrice;
            view.hqBuyFourthVolume = hqData.BuyFourthVolume;
            view.hqBuyFivePrice = hqData.BuyFivePrice;
            view.hqBuyFiveVolume = hqData.BuyFiveVolume;
            view.hqSellFirstPrice = hqData.SellFirstPrice;
            view.hqSellFirstVolume = hqData.SellFirstVolume;
            view.hqSellSecondPrice = hqData.SellSecondPrice;
            view.hqSellSecondVolume = hqData.SellSecondVolume;
            view.hqSellThirdPrice = hqData.SellThirdPrice;
            view.hqSellThirdVolume = hqData.SellThirdVolume;
            view.hqSellFourthPrice = hqData.SellFourthPrice;
            view.hqSellFourthVolume = hqData.SellFourthVolume;
            view.hqSellFivePrice = hqData.SellFivePrice;
            view.hqSellFiveVolume = hqData.SellFiveVolume;
            view.hqLastPrice = hqData.LastPrice;
            view.hqLastVolume = hqData.LastVolume;
            view.hqLowerPrice = hqData.LowerPrice;
            view.hqUpPrice = hqData.UpPrice;
            view.hqYesterPrice = hqData.YesterPrice;
        }
        #endregion

        #region 从行情服务器获取行情

        /// <summary>
        /// 获取现货行情数据
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="code">代码</param>
        public static void GetRealTimeStockData(IRealtimeMarketView view, string code)
        {
            HqExData hqData = RealtimeMarketService.GetRealTimeStockDataByCommdityCode(code);
            if (hqData != null)
            {
                BindHqDataValue(view, HqExDataConvertMarketDataInfo(hqData), Types.BreedClassTypeEnum.Stock);
            }
        }

        /// <summary>
        /// 获取股指期货行情数据
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="code">代码</param>
        public static void GetRealTimeFutureData(IRealtimeMarketView view, string code)
        {
            FutData hqData = RealtimeMarketService.GetRealTimeFutureDataByContractCode(code);
            if (hqData != null)
            {
                BindHqDataValue(view, FutDataDataConvertMarketDataInfo(hqData), Types.BreedClassTypeEnum.StockIndexFuture);
            }
        }

        /// <summary>
        /// 获取商品期货行情数据
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="code">代码</param>
        public static void GetRealTimeCommditiesData(IRealtimeMarketView view, string code)
        {
            MerFutData hqData = RealtimeMarketService.GetRealTimeCommditiesDataByContractCode(code);
            if (hqData != null)
            {
                BindHqDataValue(view, MerFutDataConvertMarketDataInfo(hqData), Types.BreedClassTypeEnum.CommodityFuture);
            }
        }

        /// <summary>
        /// 获取港股行情数据
        /// </summary>
        /// <param name="view">行情视图接口</param>
        /// <param name="code">代码</param>
        public static void GetRealTimeHKData(IRealtimeMarketView view, string code)
        {
            HKStock hqData = RealtimeMarketService.GetRealTimeHKStockDataByCommdityCode(code);
            if (hqData != null)
            {
                BindHqDataValue(view, HKStockDataConvertMarketDataInfo(hqData), Types.BreedClassTypeEnum.HKStock);
            }
        }

        #endregion

    }

    #region 行情视图接口
    /// <summary>
    /// Desc: 行情视图接口
    /// Create By: 董鹏
    /// Create Date: 2010-01-28
    /// </summary>
    public interface IRealtimeMarketView
    {
        /// <summary>品种类型</summary>
        Types.BreedClassTypeEnum BreedClassType { set; get; }
        /// <summary>代码</summary>
        string hqCode { set; get; }
        /// <summary>名称</summary>
        string hqName { set; get; }
        /// <summary>买1价</summary>
        decimal hqBuyFirstPrice { set; get; }
        /// <summary>买1量</summary>
        decimal hqBuyFirstVolume { set; get; }
        /// <summary>买2价</summary>
        decimal hqBuySecondPrice { set; get; }
        /// <summary>买2量</summary>
        decimal hqBuySecondVolume { set; get; }
        /// <summary>买3价</summary>
        decimal hqBuyThirdPrice { set; get; }
        /// <summary>买3量</summary>
        decimal hqBuyThirdVolume { set; get; }
        /// <summary>买4价</summary>
        decimal hqBuyFourthPrice { set; get; }
        /// <summary>买4量</summary>
        decimal hqBuyFourthVolume { set; get; }
        /// <summary>买5价</summary>
        decimal hqBuyFivePrice { set; get; }
        /// <summary>买5量</summary>
        decimal hqBuyFiveVolume { set; get; }
        /// <summary>卖1价</summary>
        decimal hqSellFirstPrice { set; get; }
        /// <summary>卖1量</summary>
        decimal hqSellFirstVolume { set; get; }
        /// <summary>卖2价</summary>
        decimal hqSellSecondPrice { set; get; }
        /// <summary>卖2量</summary>
        decimal hqSellSecondVolume { set; get; }
        /// <summary>卖3价</summary>
        decimal hqSellThirdPrice { set; get; }
        /// <summary>卖3量</summary>
        decimal hqSellThirdVolume { set; get; }
        /// <summary>卖4价</summary>
        decimal hqSellFourthPrice { set; get; }
        /// <summary>卖4量</summary>
        decimal hqSellFourthVolume { set; get; }
        /// <summary>卖5价</summary>
        decimal hqSellFivePrice { set; get; }
        /// <summary>卖5量</summary>
        decimal hqSellFiveVolume { set; get; }
        /// <summary>成交价</summary>
        decimal hqLastPrice { set; get; }
        /// <summary>成交量</summary>
        decimal hqLastVolume { set; get; }
        /// <summary>跌停价</summary>
        decimal hqLowerPrice { set; get; }
        /// <summary>涨停价</summary>
        decimal hqUpPrice { set; get; }
        /// <summary>昨日收盘价</summary>
        decimal hqYesterPrice { set; get; }
    }
    #endregion
}
