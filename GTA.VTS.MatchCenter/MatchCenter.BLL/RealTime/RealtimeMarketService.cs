using CommonRealtimeMarket;
using CommonRealtimeMarket.factory;
using MatchCenter.BLL.Common;
//using CommonRealtimeMarket.entity;
//using RealtimeMarket.factory;
using MatchCenter.BLL.MatchData;
using GTA.VTS.Common.CommonUtility;
using System;
using Amib.Threading;
using System.Windows.Forms;
using RealTime.Server.SModelData.HqData;

namespace MatchCenter.BLL.RealTime
{
    /// <summary>
    /// 行情类
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// Desc:增加商品期货相关方法
    /// Update BY：董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public class RealtimeMarketService
    {
        #region 变量定义

        #region private私有变量定义
        /// <summary>
        /// 线程池
        /// </summary>
        private static SmartThreadPool smartPool = new SmartThreadPool();
        /// <summary>
        /// 期货行情接口
        /// </summary>
        private static IRealtimeMarketService _funtureRealtimeService;
        /// <summary>
        /// 现货行情接口
        /// </summary>
        private static IRealtimeMarketService _stockRealtimeService;
        /// <summary>
        /// 港股证卷行情接口
        /// </summary>
        private static IRealtimeMarketService _hkStockRealtimeService;

        private static IRealtimeMarketService _commditiesRealtimeService;
        #endregion

        #region public 公共变量定义
        /// <summary>
        /// 显示接收到股指期货行情信息Listbox
        /// </summary>
        public static ListBox ListQHHQWork;
        /// <summary>
        /// 显示接收到现货行情信息Listbox
        /// </summary>
        public static ListBox ListXHHQWork;
        /// <summary>
        /// 显示接收到港股行情信息Listbox
        /// </summary>
        public static ListBox ListHKHQWork;

        /// <summary>
        /// Desc: 显示接收到商品期货行情信息Listbox
        /// Create By: 董鹏
        /// Create Date: 2010-01-27
        /// </summary>
        public static ListBox ListCFHQWork;
        /// <summary>
        /// 是否显示行情信息
        /// </summary>
        public static bool isShowRealTimeMsg = false;
        #endregion
        #endregion

        /// <summary>
        /// 获取行情
        /// </summary>
        /// <returns></returns>
        public static IRealtimeMarketService GetRealtimeMark()
        {
            int realMode = 0;
            realMode = AppConfig.GetConfigRealTimeMode();
            //if (realMode == 1)
            //{
            return RealtimeMarketServiceFactory.GetService();
            //}
            //return RealtimeMarketServiceFactory2.GetService();
        }
        /// <summary>
        /// 静态构造函数 初始化线程池、行情接口事件
        /// </summary>
        static RealtimeMarketService()
        {
            smartPool.MaxThreads = 80;
            smartPool.MinThreads = 20;
            smartPool.Start();

        }
        /// <summary>
        ///根据现货代码获取当前代码行情
        /// </summary>
        /// <param name="commdityCode">代码</param>
        /// <returns></returns>
        public static HqExData GetRealTimeStockDataByCommdityCode(string commdityCode)
        {

            //撮合中心代码不能为空
            if (string.IsNullOrEmpty(commdityCode))
            {
                return null;
            }
            IRealtimeMarketService realTimeService = GetRealtimeMark();
            //撮合中心实体不能为空
            if (realTimeService == null)
            {
                return null;
            }
            HqExData vtHqExData = realTimeService.GetStockHqData(commdityCode);
            return vtHqExData;

        }
        /// <summary>
        ///根据期货合约代码获取当前代码行情
        /// </summary>
        /// <param name="contractCode">合约代码</param>
        /// <returns></returns>
        public static FutData GetRealTimeFutureDataByContractCode(string contractCode)
        {

            //撮合中心代码不能为空
            if (string.IsNullOrEmpty(contractCode))
            {
                return null;
            }
            IRealtimeMarketService realTimeService = GetRealtimeMark();
            //撮合中心实体不能为空
            if (realTimeService == null)
            {
                return null;
            }
            FutData vtFutData = realTimeService.GetFutData(contractCode);
            return vtFutData;

        }
        /// <summary>
        ///根据港股代码获取当前代码行情
        /// </summary>
        /// <param name="commdityCode">代码</param>
        /// <returns></returns>
        public static HKStock GetRealTimeHKStockDataByCommdityCode(string commdityCode)
        {

            //撮合中心代码不能为空
            if (string.IsNullOrEmpty(commdityCode))
            {
                return null;
            }
            IRealtimeMarketService realTimeService = GetRealtimeMark();
            //撮合中心实体不能为空
            if (realTimeService == null)
            {
                return null;
            }
            HKStock vtHqExData = realTimeService.GetHKStockData(commdityCode);
            return vtHqExData;

        }

        /// <summary>
        /// Desc: 根据商品期货合约代码获取当前代码行情
        /// Create by: 董鹏
        /// Create Date: 2010-01-25
        /// </summary>
        /// <param name="contractCode">合约代码</param>
        /// <returns></returns>
        public static MerFutData GetRealTimeCommditiesDataByContractCode(string contractCode)
        {

            //撮合中心代码不能为空
            if (string.IsNullOrEmpty(contractCode))
            {
                return null;
            }
            IRealtimeMarketService realTimeService = GetRealtimeMark();
            //撮合中心实体不能为空
            if (realTimeService == null)
            {
                return null;
            }
            MerFutData vtFutData = realTimeService.GetMercantileFutData(contractCode);
            return vtFutData;
        }

        /// <summary>
        /// 内部静态创建无事件行情接口定义
        /// </summary>
        private static IRealtimeMarketService _realtimeService = GetRealtimeMark();
        /// <summary>
        /// 静态创建无事件行情接口
        /// </summary>
        public static IRealtimeMarketService GetStaticRealtimeServiceNotEvent
        {
            get
            {
                if (_realtimeService == null)
                {
                    _realtimeService = GetRealtimeMark();
                }
                return _realtimeService;
            }
        }
        /// <summary>
        /// 开始初始化行情接收事件
        /// </summary>
        public static void InitRealTimeStart()
        {
            //ShowMessage.Instanse.ShowFormTitleMessage("正在初始化行情事件");
            //获取配置的撮合类型
            string matchTypeStr = AppConfig.GetConfigMatchBreedClassType();
            if (matchTypeStr.Substring(3, 1) == "1")
            {
                //现货
                if (_stockRealtimeService != null)
                {
                    _stockRealtimeService.StockRealtimeMarketChangeEvent -= StockRealtimeMarketChangeEvent;
                    _stockRealtimeService = null;
                }

                _stockRealtimeService = RealtimeMarketService.GetRealtimeMark();
                _stockRealtimeService.StockRealtimeMarketChangeEvent += StockRealtimeMarketChangeEvent;
            }

            if (matchTypeStr.Substring(2, 1) == "1")
            {

                //期货
                if (_funtureRealtimeService != null)
                {
                    _funtureRealtimeService.FutRealtimeMarketChangeEvent -= FutRealtimeMarketChangeEvent;
                    _funtureRealtimeService = null;
                }
                _funtureRealtimeService = RealtimeMarketService.GetRealtimeMark();
                _funtureRealtimeService.FutRealtimeMarketChangeEvent += FutRealtimeMarketChangeEvent;
            }

            if (matchTypeStr.Substring(1, 1) == "1")
            {
                //港股
                if (_hkStockRealtimeService != null)
                {
                    _hkStockRealtimeService.HKStockRealtimeMarketChangeEvent -= HKStockRealtimeMarketChangeEvent;
                    _hkStockRealtimeService = null;
                }

                _hkStockRealtimeService = RealtimeMarketService.GetRealtimeMark();
                _hkStockRealtimeService.HKStockRealtimeMarketChangeEvent += HKStockRealtimeMarketChangeEvent;
            }

            if (matchTypeStr.Substring(0, 1) == "1")
            {
                //商品期货 add by 董鹏 2010-01-25
                if (_commditiesRealtimeService != null)
                {
                    _commditiesRealtimeService.MercantileFutRealtimeMarketChangeEvent -= CommditiesRealtimeMarketChangeEvent;
                    _commditiesRealtimeService = null;
                }

                _commditiesRealtimeService = RealtimeMarketService.GetRealtimeMark();
                _commditiesRealtimeService.MercantileFutRealtimeMarketChangeEvent += CommditiesRealtimeMarketChangeEvent;
            }
            //  ShowMessage.Instanse.ShowFormTitleMessage("初始化行情事件(完)");
        }

        #region 期货行情事件操作

        /// <summary>
        /// 期货行情调度查询撮合代码撮合事件
        /// </summary>
        /// <param name="hqData">期货行情实体</param>
        public static void FindFuntureMatchCode(FutDataChangeEventArg hqData)
        {
            string code = hqData.HqData.CodeKey;
            if (MatchCodeDictionary.funtureMatchCodeDic.ContainsKey(code))
            {
                EventHandler<FutDataChangeEventArg> eventArg = MatchCodeDictionary.funtureMatchCodeDic[code];
                if (eventArg != null)
                {
                    eventArg(null, hqData);
                }
            }
        }
        /// <summary>
        /// 期货行情调度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void FutRealtimeMarketChangeEvent(object sender, FutDataChangeEventArg e)
        {
            if (e == null || e.HqData == null)
            {
                return;
            }
            if (!MatchCodeDictionary.qh_ActivityOrderDic.ContainsKey(e.HqData.CodeKey))
            {
                return;
            }

            //不显示行情信息就不再处理
            if (isShowRealTimeMsg)
            {
                string nowTime = DateTime.Now.ToString();
                var data = e.HqData;
                string msg = data.CodeKey + "行情到达：{0}  行情时间：{1}\r\n成交价：{2}--{3}\r\n";
                msg += "卖一：{4}--{5}   卖五：{6}--{7}   ";
                msg += "买一：{8}--{9}   买五：{10}--{11}\r\n  ";
                string wrmsg = string.Format(msg, nowTime, data.Time, data.Lasttrade, data.LastVolume, data.Sellprice1, data.Sellvol1, data.Sellprice5, data.Sellvol5
                                          , data.Buyprice1, data.Buyvol1, data.Buyprice5, data.Buyvol5);
                smartPool.QueueWorkItem(delegate(object state) { MessageDisplayHelper.Event((string)state, ListQHHQWork); }, wrmsg);
            }

            //LogHelper.WriteDebug(wrmsg);
            // MessageDisplayHelper.Event(wrmsg, ListQHHQWork);
            smartPool.QueueWorkItem(FindFuntureMatchCode, e);


        }
        #endregion

        #region 现货行情事件操作
        /// <summary>
        /// 现货行情调度事件
        /// </summary>
        /// <param name="sender">调度事件</param>
        /// <param name="e">现货行情实体</param>
        public static void StockRealtimeMarketChangeEvent(object sender, StockHqDataChangeEventArg e)
        {
            if (e == null || e.HqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.xh_ActivityOrderDic.ContainsKey(e.HqData.CodeKey))
            {
                return;
            }

            //不显示行情信息就不再处理
            if (isShowRealTimeMsg)
            {
                string nowTime = DateTime.Now.ToString();
                var data = e.HqData.HqData;
                string msg = data.CodeKey + "行情到达：{0}  行情时间：{1} 成交价：{2}--{3}\r\n";
                msg += "卖一：{4}--{5}   卖五：{6}--{7}   ";
                msg += "买一：{8}--{9}   买五：{10}--{11}\r\n  ";
                string wrmsg = string.Format(msg, nowTime, data.Time, data.Lasttrade, e.HqData.LastVolume, data.Sellprice1, data.Sellvol1, data.Sellprice5, data.Sellvol5
                                          , data.Buyprice1, data.Buyvol1, data.Buyprice5, data.Buyvol5);

                smartPool.QueueWorkItem(delegate(object state) { MessageDisplayHelper.Event((string)state, ListXHHQWork); }, wrmsg);
            }
            // LogHelper.WriteDebug(wrmsg);
            // MessageDisplayHelper.Event(wrmsg, ListXHHQWork);    
            smartPool.QueueWorkItem(FindStockMatchCode, e);

        }
        /// <summary>
        /// 现货行情调度查询撮合代码撮合事件
        /// </summary>
        /// <param name="hqData">撮合行情</param>
        public static void FindStockMatchCode(StockHqDataChangeEventArg hqData)
        {
            string code = hqData.HqData.CodeKey;
            if (MatchCodeDictionary.stockMatchCodeDic.ContainsKey(code))
            {
                EventHandler<StockHqDataChangeEventArg> eventArg = MatchCodeDictionary.stockMatchCodeDic[code];
                if (eventArg != null)
                {
                    eventArg(null, hqData);
                }
            }
        }
        #endregion

        #region 港股行情事件操作
        /// <summary>
        /// 港股行情调度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void HKStockRealtimeMarketChangeEvent(object sender, HKStockDataChangeEventArg e)
        {
            if (e == null || e.HqData == null || e.HqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.hk_ActivityOrderDic.ContainsKey(e.HqData.CodeKey))
            {
                return;
            }

            //不显示行情信息就不再处理
            if (isShowRealTimeMsg)
            {
                string nowTime = DateTime.Now.ToString();
                var data = e.HqData;
                string msg = data.CodeKey + "行情到达：{0}  行情时间：{1} 成交价：{2}--{3}\r\n";
                msg += "卖一：{4}--{5}   卖五：{6}--{7}   ";
                msg += "买一：{8}--{9}   买五：{10}--{11}\r\n  ";
                string wrmsg = string.Format(msg, nowTime, data.Time, data.Lasttrade, e.HqData.PTrans, data.Sellprice1, data.Sellvol1, data.Sellprice5, data.Sellvol5
                                          , data.Buyprice1, data.Buyvol1, data.Buyprice5, data.Buyvol5);
                smartPool.QueueWorkItem(delegate(object state) { MessageDisplayHelper.Event((string)state, ListHKHQWork); }, wrmsg);
            }

            smartPool.QueueWorkItem(FindHKStockMatchCode, e);

        }

        /// <summary>
        /// 港股行情调度查询撮合代码撮合事件
        /// </summary>
        /// <param name="hqData">撮合行情</param>
        public static void FindHKStockMatchCode(HKStockDataChangeEventArg hqData)
        {
            string code = hqData.HqData.CodeKey;
            if (MatchCodeDictionary.hkStockMatchCodeDic.ContainsKey(code))
            {
                EventHandler<HKStockDataChangeEventArg> eventArg = MatchCodeDictionary.hkStockMatchCodeDic[code];
                if (eventArg != null)
                {
                    eventArg(null, hqData);
                }
            }
        }
        #endregion

        #region 商品期货行情调度事件操作 add by 董鹏 2010-01-25
        /// <summary>
        /// 商品期货行情调度事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CommditiesRealtimeMarketChangeEvent(object sender, MercantileFutDataChangeEventArg e)
        {
            if (e == null || e.HqData == null)
            {
                return;
            }

            if (!MatchCodeDictionary.spqh_ActivityOrderDic.ContainsKey(e.HqData.CodeKey))
            {
                return;
            }

            //不显示行情信息就不再处理
            if (isShowRealTimeMsg)
            {

                string nowTime = DateTime.Now.ToString();
                var data = e.HqData;
                string msg = data.CodeKey + "行情到达：{0}  行情时间：{1} 成交价：{2}--{3}\r\n";
                msg += "卖一：{4}--{5}   卖五：{6}--{7}   ";
                msg += "买一：{8}--{9}   买五：{10}--{11}\r\n  ";
                string wrmsg = string.Format(msg, nowTime, data.Time, data.Lasttrade, e.HqData.PTrans, data.Sellprice1, data.Sellvol1, data.Sellprice5, data.Sellvol5
                                          , data.Buyprice1, data.Buyvol1, data.Buyprice5, data.Buyvol5);
                smartPool.QueueWorkItem(delegate(object state) { MessageDisplayHelper.Event((string)state, ListCFHQWork); }, wrmsg);
            }

            smartPool.QueueWorkItem(FindCommditiesMatchCode, e);


        }

        /// <summary>
        /// 商品期货行情调度查询撮合代码撮合事件
        /// </summary>
        /// <param name="hqData"></param>
        public static void FindCommditiesMatchCode(MercantileFutDataChangeEventArg hqData)
        {
            string code = hqData.HqData.CodeKey;
            if (MatchCodeDictionary.spQHMatchCodeDic.ContainsKey(code))
            {
                EventHandler<MercantileFutDataChangeEventArg> eventArg = MatchCodeDictionary.spQHMatchCodeDic[code];
                if (eventArg != null)
                {
                    eventArg(null, hqData);
                }
            }
        }
        #endregion

    }
}
