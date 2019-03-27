using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MatchCenter.DAL.DevolveVerifyCommonService;
using MatchCenter.BLL.ManagementCenter;
using MatchCenter.BLL.Common;
//using CommonRealtimeMarket.entity;
using MatchCenter.BLL.RealTime;
using MatchCenter.Entity;
using System.Collections;
using RealTime.Server.SModelData.HqData;

namespace MatchCenter.BLL.MatchRules
{
    /// <summary>
    /// 熔断管理中心
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// </summary>
    public class FuseManager : Singleton<FuseManager>
    {
        #region 单一进入模式
        /// <summary>
        /// 费用价格计算功能类功能单一进入模式
        /// </summary>
        public static FuseManager Instanse
        {
            get
            {
                return singletonInstance;
            }
        }
        #endregion

        #region 根据商品代码获取熔断触发比例
        /// <summary>
        /// 根据商品代码获取熔断触发比例 如果没有找到要熔断对象则返回默认值
        /// </summary>
        public decimal GetFuseTriggeringScaleByCommodityCode(string commodityCode)
        {
            CM_CommodityFuse model = CommonDataCacheProxy.Instanse.GetCacheCommodityFuseByCode(commodityCode);
            if (model == null)
            {
                return RulesDefaultValue.DefaultFuseScale;
            }
            else
            {
                return (decimal)model.TriggeringScale;
            }
        }
        #endregion

        #region 根据商品代码判断当前系统时间是否启动熔断
        /// <summary> 
        /// 根据商品代码判断当前系统时间是否启动熔断
        /// 查询熔断实体的熔断时间与当前系统时间比较如果商品没有熔断设置返回不启动熔断
        /// 如果当前系统时间在熔断实体中的开始与结束时间内即启动熔断，否则不启动
        /// </summary>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        public bool CompareNowTimeIsFuseTime(string code)
        {
            List<CM_FuseTimesection> list = CommonDataManagerOperate.GetFuseTimeEntitysByCommondityCode(code);
            if (Utils.IsNullOrEmpty(list))
            {
                return false;
            }
            DateTime fuseTime = DateTime.Now;
            foreach (var timesection in list)
            {
                if (fuseTime >= Utils.ConvertToNowDateTime((DateTime)timesection.StartTime) && fuseTime <= Utils.ConvertToNowDateTime((DateTime)timesection.EndTime))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 根据商品代码获取行情熔断比例与当前熔断比例比较是否可以启动熔断
        /// <summary>
        /// 根据商品代码获取行情熔断比例与当前熔断比例比较是否可以启动熔断
        /// 如果当前比较小于或者等于行情比例则不启动熔断
        /// </summary>
        /// <param name="triggeringScale">触发比例</param>
        /// <param name="code">商品代码</param>
        /// <returns></returns>
        public bool CompareHQScaleIsFuse(decimal triggeringScale, string code)
        {
            triggeringScale = triggeringScale / 100;
            //VTFutData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetFutData(code);
            FutData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetFutData(code);
            if (triggeringScale <= (decimal)data.ChgPct)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 根据商品获取行情最后交易价与昨日收盘价和触发比例判断是否启动熔断
        /// <summary>
        /// 【是否产生熔断】根据商品获取行情最后交易价与昨日收盘价和触发比例判断是否启动熔断
        /// 最后交易价大于等于昨日收盘价*（1+熔断比例）或者小于等于昨日收盘价*（1-熔断比例）则启动熔断
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="triggerScale">比例</param>
        /// <returns></returns>
        public  bool IsStartFuse(string code, decimal triggerScale)
        {
            //VTFutData data = RealtimeMarketService.GetRealtimeMark().GetFutData(code);
            FutData data = RealtimeMarketService.GetStaticRealtimeServiceNotEvent.GetFutData(code);
            var price = (decimal)data.Lasttrade;
            var yclose = (decimal)data.Yclose;
            if (price >= yclose * (1.00m + triggerScale))
            {
                return true;
            }
            if (price <= yclose * (1.00m - triggerScale))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 根据商品判断当前时间是否是否发生熔断如果发生返回缓存中熔断的标识
        /// <summary>
        /// 根据商品判断当前时间是否是否发生熔断如果发生返回缓存中熔断的标识是否发生熔断
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public bool IsFuse(string code)
        {
            //判断当前时间是否启动熔断时间
            if (!FuseManager.Instanse.CompareNowTimeIsFuseTime(code))
            {
                return false;
            }
            //熔断实体
            FuseHanderEntity entity = null;
            lock (((ICollection)MatchCenterManager.Instance.FuseHanderEntityList).SyncRoot)
            {
                if (MatchCenterManager.Instance.FuseHanderEntityList.ContainsKey(code))
                {
                    entity = MatchCenterManager.Instance.FuseHanderEntityList[code];
                }
                //实体不能为空
                if (entity == null)
                {
                    return false;
                }
                //返回熔断标志
                return entity.IsFuse;
            }
        }
        #endregion

        #region 修改涨跌幅状态
        /// <summary>
        /// 修改涨跌幅状态
        /// </summary>
        /// <param name="code">代码</param>
        public void ModifyState(string code)
        {
            //熔断实体是否存在
            //判断当前时间是否启动熔断时间
            if (!FuseManager.Instanse.CompareNowTimeIsFuseTime(code))
            {
                //初始化熔断
                InitFuseEntity(code);
                return;
            }
            //管理中心熔断初始化数据
            CM_CommodityFuse fuse = CommonDataCacheProxy.Instanse.GetCacheCommodityFuseByCode(code);
            if (fuse == null)
            {
                return;
            }

            //修改熔断实体
            lock (((ICollection)MatchCenterManager.Instance.FuseHanderEntityList).SyncRoot)
            {
                FuseHanderEntity entity = MatchCenterManager.Instance.FuseHanderEntityList[code];
                if (entity == null)
                {
                    return;
                }
                //是否产生熔断
                if (entity.IsFuse)
                {
                    //熔断持续时间
                    // int FuseContinuTime = CalculationTime(entity.FuseTime);
                    int FuseContinuTime = (int)Utils.TimeSpanSecondsToNowDateTime(entity.FuseTime);
                    if (FuseContinuTime >= fuse.FuseDurationLimit * 60)
                    {
                        //是否产生熔断
                        entity.IsFuse = false;
                        entity.PriorFuse = false;
                    }
                }
                else
                {
                    //涨跌幅判断标志
                    if (entity.PriorFuse)
                    {
                        //if (JudgePrice((decimal) fuse.TriggeringScale, code))
                        if (FuseManager.Instanse.CompareHQScaleIsFuse((decimal)fuse.TriggeringScale, code))
                        {
                            //int contrinueTime = CalculationTime(entity.FuseTime);
                            int contrinueTime = (int)Utils.TimeSpanSecondsToNowDateTime(entity.FuseTime);
                            if (contrinueTime >= fuse.TriggeringDuration * 60 && entity.FuseCount < fuse.FuseTimeOfDay)
                            {
                                //熔断数量
                                entity.FuseCount = entity.FuseCount + 1;
                                //熔断标志
                                entity.IsFuse = true;
                                //熔断启动时间
                                entity.StartFuseTime = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        //熔断涨跌幅度判断
                        if (FuseManager.Instanse.CompareHQScaleIsFuse((decimal)fuse.TriggeringScale, code))
                        {
                            entity.PriorFuse = true;
                            entity.FuseTime = DateTime.Now;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化熔断实体
        /// </summary>
        /// <param name="code">代码</param>
        private void InitFuseEntity(string code)
        {
            FuseHanderEntity entity;
            lock (((ICollection)MatchCenterManager.Instance.FuseHanderEntityList).SyncRoot)
            {
                if (MatchCenterManager.Instance.FuseHanderEntityList.ContainsKey(code))
                {
                    entity = MatchCenterManager.Instance.FuseHanderEntityList[code];
                    //涨跌幅超过触发比例标志
                    entity.PriorFuse = false;
                    //是否产生熔断标志
                    entity.IsFuse = false;
                }
            }
        }

        #endregion


    }
}
