#region Using Namespace

using System;
using System.Collections.Generic;
using System.Threading;
using CommonRealtimeMarket;
//using CommonRealtimeMarket.entity;
using CommonRealtimeMarket.factory;
using GTA.VTS.Common.CommonUtility;
using RealTime.Server.SModelData.HqData;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.Entity.AccountManagementAndFindEntity;

#endregion

namespace ReckoningCounter.BLL.Common
{
    public class RealTimeMarketUtil
    {
        private static RealTimeMarketUtil instance = new RealTimeMarketUtil();

        private IDictionary<string, decimal> futureYCloseDic = new Dictionary<string, decimal>();
        private ReaderWriterLockSlim futureYCloseDicLock = new ReaderWriterLockSlim();
        private IRealtimeMarketService service;
        private IDictionary<string, decimal> stockYCloseDic = new Dictionary<string, decimal>();
        private ReaderWriterLockSlim stockYCloseDicLock = new ReaderWriterLockSlim();

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
            try
            {
                //if (mode == 1)
                //{
                service = RealtimeMarketServiceFactory.GetService();
                //}
                //else if (mode == 2)
                //{
                //    service = RealtimeMarket.factory.RealtimeMarketServiceFactory2.GetService();
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return service;
        }

        /// <summary>
        /// 获取现货的昨日收盘价
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public decimal GetStockYClose(string code)
        {
            decimal result = -1;

            stockYCloseDicLock.EnterUpgradeableReadLock();
            try
            {
                if (!stockYCloseDic.TryGetValue(code, out result))
                {
                    stockYCloseDicLock.EnterWriteLock();
                    try
                    {
                        HqExData data = this.service.GetStockHqData(code);
                        HqExData exData = data;

                        float yClose = exData.YClose;
                        result = (decimal)yClose;

                        stockYCloseDic[code] = result;
                    }
                    catch (Exception ex)
                    {
                        result = -1;
                        LogHelper.WriteError(ex.Message, ex);
                    }
                    finally
                    {
                        stockYCloseDicLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                stockYCloseDicLock.ExitUpgradeableReadLock();
            }

            return result;
        }

        /// <summary>
        /// Title:根据代码和代码所属类型获取最后成交价(最新成交价)
        /// Create by:李健华
        /// Create Date:2009-11-08
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MarketDataLevel GetLastPriceByCode(string code, int breedClassType, out string errMsg)
        {

            MarketDataLevel result = null;
            errMsg = "";
            try
            {
                switch ((Types.BreedClassTypeEnum)breedClassType)
                {
                    case Types.BreedClassTypeEnum.Stock:
                        HqExData data = GetRealMarketService().GetStockHqData(code);
                        if (data == null || data.HqData == null)
                        {
                            errMsg = "GT-0023：无法获取当前现货代码相关行情";
                            return result;
                        }
                        // HqExData exData = data;
                        //float lastPrice = exData.HqData.Lasttrade;
                        //result = (decimal)lastPrice;
                        result = HqExDataConvertMarketDataLevel(data);
                        break;
                    case Types.BreedClassTypeEnum.CommodityFuture:
                        //add by 董鹏 2010-01-26
                        MerFutData cfdata = GetRealMarketService().GetMercantileFutData(code);
                        if (cfdata == null)
                        {
                            errMsg = "GT-0023：无法获取当前商品期货代码相关行情";
                            return result;
                        }
                        //MerFutData cforgData = cfdata;
                        //double cflastPrice = cforgData.Lasttrade;
                        //result = (decimal)cflastPrice;
                        result = MerFutDataConvertMarketDataLevel(cfdata);
                        break;
                    case Types.BreedClassTypeEnum.StockIndexFuture:
                        FutData qhdata = GetRealMarketService().GetFutData(code);
                        if (qhdata == null)
                        {
                            errMsg = "GT-0023：无法获取当前股指期货代码相关行情";
                            return result;
                        }
                        //FutData orgData = qhdata;
                        //double qhlastPrice = orgData.Lasttrade;
                        //result = (decimal)qhlastPrice;
                        result = FutDataDataConvertMarketDataLevel(qhdata);
                        break;
                    case Types.BreedClassTypeEnum.HKStock:
                        HKStock hkdata = GetRealMarketService().GetHKStockData(code);
                        if (hkdata == null)
                        {
                            errMsg = "GT-0023：无法获取当前港股代码相关行情";
                            return result;
                        }
                        //double hkLastPrice = hkdata.Lasttrade;
                        //result = (decimal)hkLastPrice;
                        result = HKStockDataConvertMarketDataLevel(hkdata);
                        break;
                    default:
                        errMsg = "GT-0023：无此代码所属类型的行情";
                        break;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                LogHelper.WriteError("GT-0023：根据代码和代码所属类型获取最后成交价(最新成交价)异常", ex);
            }

            return result;
        }


        #region 行情实体转换成自定义实体
        /// <summary>
        /// 现货行情转换成自定义行情实体
        /// </summary>
        /// <param name="data">自定义实体</param>
        /// <returns></returns>
        public MarketDataLevel HqExDataConvertMarketDataLevel(HqExData data)
        {
            MarketDataLevel level = new MarketDataLevel();
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
            level.MarketRefreshTime = data.HqData.Time;
            return level;
        }
        /// <summary>
        /// 商品期货行情转换成自定义行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public MarketDataLevel MerFutDataConvertMarketDataLevel(MerFutData hqData)
        {
            MarketDataLevel level = new MarketDataLevel();
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
            level.MarketRefreshTime = DateTime.Parse(hqData.Time);
            return level;
        }

        /// <summary>
        /// 股指期货行情转换成自定义行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public MarketDataLevel FutDataDataConvertMarketDataLevel(FutData hqData)
        {
            MarketDataLevel level = new MarketDataLevel();
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
            level.MarketRefreshTime = Convert.ToDateTime( hqData.Time);
            return level;
        }

        /// <summary>
        /// 港股行情转换成自定义行情实体
        /// </summary>
        /// <param name="hqData">自定义实体</param>
        /// <returns></returns>
        public MarketDataLevel HKStockDataConvertMarketDataLevel(HKStock hqData)
        {
            MarketDataLevel level = new MarketDataLevel();
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
            level.MarketRefreshTime = DateTime.Parse(hqData.Time);
            return level;
        }
        #endregion
        /// <summary>
        /// 清空昨日结算价(重置） 
        /// </summary>
        public void Reset()
        {
            stockYCloseDicLock.EnterWriteLock();
            try
            {
                stockYCloseDic.Clear();
            }
            finally
            {
                stockYCloseDicLock.ExitWriteLock();
            }
        }
    }
}