using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;

namespace GTA.VTS.CustomersOrders.BLL
{


    /// <summary>
    /// 自动撤单委托事件
    /// </summary>
    /// <param name="entrustNumber"></param>
    /// <param name="errMsg"></param>
    /// <returns></returns>
    public delegate bool AutoCancelOrder(string entrustNumber, ref string errMsg);

    /// <summary>
    /// 自动撤单实体
    /// </summary>
    public class AutoCanceEntry
    {
        /// <summary>
        /// 委托单号
        /// </summary>
        public string EntrustNumber { get; set; }
        /// <summary>
        /// 委托时间
        /// </summary>
        public DateTime OrderTime { get; set; }
    }

    /// <summary>
    /// 自动撤单操作类
    /// </summary>
    public class AutoCancleOperater
    {
        #region 自动撤单
        /// <summary>
        /// 自动下单自动撤单缓存列表
        /// </summary>
        protected SyncCache<string, AutoCanceEntry> autoCanceOrder = new SyncCache<string, AutoCanceEntry>();

        /// <summary>
        /// 自动撤单事件
        /// </summary>
        public event AutoCancelOrder AutoEvent;
        #endregion
        #region 自动撤单方法
        /// <summary>
        /// 缓存要自动撤单的列表
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="value">委托单时间</param>
        public void AddAutoCanceOrder(string entrustNumber, DateTime value)
        {
            if (string.IsNullOrEmpty(entrustNumber))
            {
                return;
            }

            if (autoCanceOrder.Contains(entrustNumber))
            {
                return;
            }
            AutoCanceEntry item = new AutoCanceEntry();
            item.EntrustNumber = entrustNumber;
            item.OrderTime = value;
            autoCanceOrder.Add(entrustNumber, item);
        }
        /// <summary>
        /// 缓存要自动撤单的列表
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="value">委托单时间</param>
        public void DelateAutoCanceOrder(string entrustNumber)
        {
            if (string.IsNullOrEmpty(entrustNumber))
            {
                return;
            }

            if (autoCanceOrder.Contains(entrustNumber))
            {
                autoCanceOrder.Delete(entrustNumber);
            }
        }
        /// <summary>
        /// 获取所有要处理的自动撤单的委托单列表
        /// </summary>
        /// <returns></returns>
        public List<AutoCanceEntry> GetAllAutocanceOrder()
        {
            ICollection<AutoCanceEntry> list;
            list = autoCanceOrder.GetAll();
            if (list == null)
            {
                return null;
            }
            List<AutoCanceEntry> lists = new List<AutoCanceEntry>();
            foreach (var item in list)
            {
                AutoCanceEntry model = new AutoCanceEntry();
                model.EntrustNumber = item.EntrustNumber;
                model.OrderTime = item.OrderTime;
                lists.Add(model);
            }
            return lists;
        }


        /// <summary>
        /// 处理自动撤单操作
        /// </summary>
        /// <param name="minute">处理已经委托多少分钟后处理自动撤单</param>
        public void DisposeAutoCance(int minute)
        {
            List<AutoCanceEntry> list = new List<AutoCanceEntry>();
            list = GetAllAutocanceOrder();
            if (list == null)
            {
                return;
            }
            string mesg = "";
            foreach (var item in list)
            {
                if (DateTime.Now > item.OrderTime.AddMinutes(minute))
                {
                    //撤单
                    if (AutoEvent != null)
                    {
                        if (AutoEvent(item.EntrustNumber, ref mesg))
                        {
                            autoCanceOrder.Delete(item.EntrustNumber);
                        }
                    }
                }
            }
        }
        #endregion
    }




    /// <summary>
    /// Create by:李健华
    /// Creae Date:2010-05-17
    /// desc.:自动下单实体
    /// </summary>
    public class AutoDoOrder
    {
        /// <summary>
        /// 卖买方向
        /// </summary>
        public int BuySell { get; set; }
        /// <summary>
        /// 委托价格类型
        /// </summary>
        public int OrderPriceType { get; set; }
        /// <summary>
        /// 期货开平方向
        /// </summary>
        public string FutureOpenCloseType { get; set; }
        /// <summary>
        /// 委托单位类型
        /// </summary>
        public string OrderUnitType { get; set; }
        /// <summary>
        /// 通道号
        /// </summary>
        public string PortfoliosID { get; set; }
        /// <summary>
        /// 开始索引
        /// </summary>
        public int IndexStart { get; set; }
        /// <summary>
        /// 结束索引
        /// </summary>
        public int IndexEnd { get; set; }
        /// <summary>
        /// 委托量
        /// </summary>
        public int OrderAmount { get; set; }
        /// <summary>
        /// 代码表数量
        /// </summary>
        public int CodeCount { get; set; }

        /// <summary>
        /// 是否对持仓操作
        /// </summary>
        public bool IsHoldAccount
        { get; set; }

        /// <summary>
        /// 每次下笔间隔 k
        /// </summary>
        public int DoOrderTimeSapn
        {
            get;
            set;
        }

    }


}
