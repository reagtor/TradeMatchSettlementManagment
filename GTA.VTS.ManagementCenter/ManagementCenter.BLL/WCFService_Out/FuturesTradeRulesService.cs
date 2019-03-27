using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using ManagementCenter.BLL.WCFService_Out.interfases;
using Entity = ManagementCenter.Model;
using ManagementCenter.BLL;

namespace ManagementCenter.BLL.WCFService_Out
{
    /// <summary>
    /// 描述：期货交易规则对外服务
    /// 作者：刘书伟
    /// 日期：2008-11-21
    /// 描述:添加Debug日志
    /// 修改作者：刘书伟
    /// 日期：2010-05-10
    /// </summary>
    /// 错误编码 8500-8599
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FuturesTradeRulesService : IFuturesTradeRules
    {
        #region IFuturesTradeRules 成员

        /// <summary>
        /// 获取所有的合约交割月份
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_AgreementDeliveryMonth> GetALLAgreementDeliveryMonth()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8500获取所有的合约交割月份方法名称:GetALLAgreementDeliveryMonth()" + DateTime.Now);
                QH_AgreementDeliveryMonthBLL qH_AgreementDeliveryMonthBLL = new QH_AgreementDeliveryMonthBLL();
                return qH_AgreementDeliveryMonthBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8500";
                string errMsg = "获取所有的合约交割月份失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回合约交割月份
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.QH_AgreementDeliveryMonth> GetAgreementDeliveryMonthByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8501根据品种标识返回合约交割月份方法名称:GetAgreementDeliveryMonthByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                QH_AgreementDeliveryMonthBLL qH_AgreementDeliveryMonthBLL = new QH_AgreementDeliveryMonthBLL();
                return qH_AgreementDeliveryMonthBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8501";
                string errMsg = "根据品种标识返回合约交割月份失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的期货_品种_交割月份
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_CFPositionMonth> GetAllCFPositionMonth()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8502获取所有的期货_品种_交割月份方法名称:GetAllCFPositionMonth()" + DateTime.Now);
                QH_CFPositionMonthBLL qH_CFPositionMonthBLL = new QH_CFPositionMonthBLL();
                return qH_CFPositionMonthBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8502";
                string errMsg = "获取所有的期货_品种_交割月份失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的委托指令类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_ConsignInstructionType> GetAllConsignInstructionType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8503获取所有的委托指令类型方法名称:GetAllConsignInstructionType()" + DateTime.Now);
                QH_ConsignInstructionTypeBLL qH_ConsignInstructionTypeBLL = new QH_ConsignInstructionTypeBLL();
                return qH_ConsignInstructionTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8503";
                string errMsg = "获取所有的委托指令类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的交易规则委托量
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_ConsignQuantum> GetAllConsignQuantum()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8504获取所有的交易规则委托量方法名称:GetAllConsignQuantum()" + DateTime.Now);
                QH_ConsignQuantumBLL qH_ConsignQuantumBLL = new QH_ConsignQuantumBLL();
                return qH_ConsignQuantumBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8504";
                string errMsg = "获取所有的交易规则委托量失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 根据交易规则委托量标识返回交易规则委托量
        /// </summary>
        /// <param name="consignQuantumID">交易规则委托量标识</param>
        /// <returns></returns>
        public Entity.QH_ConsignQuantum GetConsignQuantumByConsignQuantumID(int consignQuantumID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8505根据交易规则委托量标识返回交易规则委托量方法名称：GetConsignQuantumByConsignQuantumID(int consignQuantumID)参数是:" + consignQuantumID + "时间是:" + DateTime.Now);
                QH_ConsignQuantumBLL qH_ConsignQuantumBLL = new QH_ConsignQuantumBLL();
                List<Entity.QH_ConsignQuantum> qH_ConsignQuantumList =
                   qH_ConsignQuantumBLL.GetListArray(string.Format("ConsignQuantumID={0}", consignQuantumID));
                if (qH_ConsignQuantumList.Count > 0)
                {
                    Entity.QH_ConsignQuantum qH_ConsignQuantum = qH_ConsignQuantumList[0];
                    if (qH_ConsignQuantum != null)
                    {
                        return qH_ConsignQuantum;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8505";
                string errMsg = "根据交易规则委托量标识返回交易规则委托量失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的品种_期货_交易费用
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_FutureCosts> GetAllFutureCosts()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8506获取所有的品种_期货_交易费用方法名称:GetAllFutureCosts()" + DateTime.Now);
                QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
                return qH_FutureCostsBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8506";
                string errMsg = "获取所有的品种_期货_交易费用失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回品种_期货_交易费用
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.QH_FutureCosts GetFutureCostsByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8507根据品种标识返回品种_期货_交易费用方法名称:GetFutureCostsByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                QH_FutureCostsBLL qH_FutureCostsBLL = new QH_FutureCostsBLL();
                List<Entity.QH_FutureCosts> qH_FutureCostsList =
                     qH_FutureCostsBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (qH_FutureCostsList.Count > 0)
                {
                    Entity.QH_FutureCosts qH_FutureCosts = qH_FutureCostsList[0];
                    if (qH_FutureCosts != null)
                    {
                        return qH_FutureCosts;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8507";
                string errMsg = "根据品种标识返回品种_期货_交易费用失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的期货_品种_交易规则
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_FuturesTradeRules> GetAllFuturesTradeRules()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8508获取所有的期货_品种_交易规则方法名称:GetAllFuturesTradeRules()" + DateTime.Now);
                QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
                return qH_FuturesTradeRulesBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8508";
                string errMsg = "获取所有的期货_品种_交易规则失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回期货_品种_交易规则
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.QH_FuturesTradeRules GetFuturesTradeRulesByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8509根据品种标识返回期货_品种_交易规则方法名称：GetFuturesTradeRulesByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                QH_FuturesTradeRulesBLL qH_FuturesTradeRulesBLL = new QH_FuturesTradeRulesBLL();
                List<Entity.QH_FuturesTradeRules> qH_FuturesTradeRulesList =
                    qH_FuturesTradeRulesBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (qH_FuturesTradeRulesList.Count > 0)
                {
                    Entity.QH_FuturesTradeRules qH_FuturesTradeRules = qH_FuturesTradeRulesList[0];
                    if (qH_FuturesTradeRules != null)
                    {
                        return qH_FuturesTradeRules;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8509";
                string errMsg = "根据品种标识返回期货_品种_交易规则失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的涨跌停板幅度类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_HighLowStopScopeType> GetAllHighLowStopScopeType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8510获取所有的涨跌停板幅度类型方法名称:GetAllHighLowStopScopeType()" + DateTime.Now);
                QH_HighLowStopScopeTypeBLL qH_HighLowStopScopeTypeBLL = new QH_HighLowStopScopeTypeBLL();
                return qH_HighLowStopScopeTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8510";
                string errMsg = "获取所有的涨跌停板幅度类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的最后交易日
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_LastTradingDay> GetAllLastTradingDay()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8511获取所有的最后交易日方法名称:GetAllLastTradingDay()" + DateTime.Now);
                QH_LastTradingDayBLL qH_LastTradingDayBLL = new QH_LastTradingDayBLL();
                return qH_LastTradingDayBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8511";
                string errMsg = " 获取所有的最后交易日失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据最后交易日标识返回最后交易日
        /// </summary>
        /// <param name="lastTradingDayID">最后交易日标识</param>
        /// <returns></returns>
        public Entity.QH_LastTradingDay GetLastTradingDayByLastTradingDayID(int lastTradingDayID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8512根据最后交易日标识返回最后交易日方法名称：GetLastTradingDayByLastTradingDayID(int lastTradingDayID)参数是:" + lastTradingDayID + "时间是:" + DateTime.Now);
                QH_LastTradingDayBLL qH_LastTradingDayBLL = new QH_LastTradingDayBLL();
                List<Entity.QH_LastTradingDay> qH_LastTradingDayList =
                    qH_LastTradingDayBLL.GetListArray(string.Format("LastTradingDayID={0}", lastTradingDayID));
                if (qH_LastTradingDayList.Count > 0)
                {
                    Entity.QH_LastTradingDay qH_LastTradingDay = qH_LastTradingDayList[0];
                    if (qH_LastTradingDay != null)
                    {
                        return qH_LastTradingDay;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8512";
                string errMsg = "根据最后交易日标识返回最后交易日失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的最后交易日类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_LastTradingDayType> GetAllLastTradingDayType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8513获取所有的最后交易日类型方法名称:GetAllLastTradingDayType()" + DateTime.Now);
                QH_LastTradingDayTypeBLL qH_LastTradingDayTypeBLL = new QH_LastTradingDayTypeBLL();
                return qH_LastTradingDayTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8513";
                string errMsg = "获取所有的最后交易日类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的月份
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_Month> GetAllMonth()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8514获取所有的月份方法名称:GetAllMonth()" + DateTime.Now);
                QH_MonthBLL qH_MonthBLL = new QH_MonthBLL();
                return qH_MonthBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8514";
                string errMsg = "获取所有的月份失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的品种_股指期货_保证金
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_SIFBail> GetAllSIFBail()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8515获取所有的品种_股指期货_保证金方法名称:GetAllSIFBail()" + DateTime.Now);
                QH_SIFBailBLL qH_SIFBailBLL = new QH_SIFBailBLL();
                return qH_SIFBailBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8515";
                string errMsg = "获取所有的品种_股指期货_保证金失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回品种_股指期货_保证金
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.QH_SIFBail GetSIFBailByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8516根据品种标识返回品种_股指期货_保证金方法名称：GetSIFBailByBreedClassID(int breedClassID)参数是:" + breedClassID+"时间是:" + DateTime.Now);
                QH_SIFBailBLL qH_SIFBailBLL = new QH_SIFBailBLL();
                List<Entity.QH_SIFBail> qH_SIFBailList =
                    qH_SIFBailBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (qH_SIFBailList.Count > 0)
                {
                    Entity.QH_SIFBail qH_SIFBail = qH_SIFBailList[0];
                    if (qH_SIFBail != null)
                    {
                        return qH_SIFBail;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8516";
                string errMsg = "根据品种标识返回品种_股指期货_保证金失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的股指期货持仓限制
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_SIFPosition> GetAllSIFPosition()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8517获取所有的股指期货持仓限制方法名称:GetAllSIFPosition()" + DateTime.Now);
                QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
                return qH_SIFPositionBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8517";
                string errMsg = "获取所有的股指期货持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回股指期货持仓限制
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public Entity.QH_SIFPosition GetSIFPositionByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8518根据品种标识返回股指期货持仓限制方法名称：GetSIFPositionByBreedClassID(int breedClassID)参数是:" + breedClassID+"时间是:" + DateTime.Now);
                QH_SIFPositionBLL qH_SIFPositionBLL = new QH_SIFPositionBLL();
                List<Entity.QH_SIFPosition> qH_SIFPositionList =
                    qH_SIFPositionBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
                if (qH_SIFPositionList.Count > 0)
                {
                    Entity.QH_SIFPosition qH_SIFPosition = qH_SIFPositionList[0];
                    if (qH_SIFPosition != null)
                    {
                        return qH_SIFPosition;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8518";
                string errMsg = "根据品种标识返回股指期货持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的单笔委托量
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_SingleRequestQuantity> GetAllSingleRequestQuantity()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8519获取所有的单笔委托量方法名称:GetAllSingleRequestQuantity()" + DateTime.Now);
                QH_SingleRequestQuantityBLL qH_SingleRequestQuantityBLL = new QH_SingleRequestQuantityBLL();
                return qH_SingleRequestQuantityBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8519";
                string errMsg = "获取所有的单笔委托量失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据交易规则委托量标识返回单笔委托量
        /// </summary>
        /// <param name="consignQuantumID">交易规则委托量标识</param>
        /// <returns></returns>
        public List<Entity.QH_SingleRequestQuantity> GetSingleRequestQuantityByConsignQuantumID(int consignQuantumID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8520根据交易规则委托量标识返回单笔委托量方法名称：GetSingleRequestQuantityByConsignQuantumID(int consignQuantumID)参数是:" + consignQuantumID + "时间是:" + DateTime.Now);
                QH_SingleRequestQuantityBLL qH_SingleRequestQuantityBLL = new QH_SingleRequestQuantityBLL();
                return qH_SingleRequestQuantityBLL.GetListArray(string.Format("ConsignQuantumID={0}", consignQuantumID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8520";
                string errMsg = "根据交易规则委托量标识返回单笔委托量失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的商品期货_保证金比例
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_CFBailScaleValue> GetAllCFBailScaleValue()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8521获取所有的商品期货_保证金比例方法名称:GetAllCFBailScaleValue()" + DateTime.Now);
                QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
                return qH_CFBailScaleValueBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8521";
                string errMsg = "获取所有的商品期货_保证金比例失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回商品期货_保证金比例
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.QH_CFBailScaleValue> GetCFBailScaleValueByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8522根据品种标识返回商品期货_保证金比例方法名称:GetCFBailScaleValueByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
                return qH_CFBailScaleValueBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8522";
                string errMsg = "根据品种标识返回商品期货_保证金比例失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 根据商品期货-保证金比例标识返回商品期货_保证金比例
        /// </summary>
        /// <param name="cFBailScaleValueID">商品期货-保证金比例标识</param>
        /// <returns></returns>
        public Entity.QH_CFBailScaleValue GetCFBailScaleValueByCFBailScaleValueID(int cFBailScaleValueID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8523根据商品期货-保证金比例标识返回商品期货_保证金比例：GetCFBailScaleValueByCFBailScaleValueID(int cFBailScaleValueID)参数是:" + cFBailScaleValueID + "时间是:" + DateTime.Now);
                QH_CFBailScaleValueBLL qH_CFBailScaleValueBLL = new QH_CFBailScaleValueBLL();
                List<Entity.QH_CFBailScaleValue> qH_CFBailScaleValueList =
                     qH_CFBailScaleValueBLL.GetListArray(string.Format("CFBailScaleValueID={0}", cFBailScaleValueID));
                if (qH_CFBailScaleValueList.Count > 0)
                {
                    Entity.QH_CFBailScaleValue qH_CFBailScaleValue = qH_CFBailScaleValueList[0];
                    if (qH_CFBailScaleValue != null)
                    {
                        return qH_CFBailScaleValue;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8523";
                string errMsg = "根据商品期货-保证金比例标识返回商品期货_保证金比例失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的持仓和保证金控制类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_PositionBailType> GetAllPositionBailType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8524获取所有的持仓和保证金控制类型方法名称:GetAllPositionBailType()" + DateTime.Now);
                QH_PositionBailTypeBLL qH_PositionBailTypeBLL = new QH_PositionBailTypeBLL();
                return qH_PositionBailTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8524";
                string errMsg = " 获取所有的持仓和保证金控制类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 获取所有的期货_持仓限制
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_PositionLimitValue> GetAllQHPositionLimitValue()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8525获取所有的期货_持仓限制方法名称:GetAllQHPositionLimitValue()" + DateTime.Now);
                QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
                return qH_PositionLimitValueBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8525";
                string errMsg = "获取所有的期货_持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回期货_持仓限制
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<Entity.QH_PositionLimitValue> GetPositionLimitValueByBreedClassID(int breedClassID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8526根据品种标识返回期货_持仓限制方法名称：GetPositionLimitValueByBreedClassID(int breedClassID)参数是:" + breedClassID + "时间是:" + DateTime.Now);
                QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
                return qH_PositionLimitValueBLL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-8526";
                string errMsg = "根据品种标识返回期货_持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }


        /// <summary>
        /// 根据期货-持仓限制标识返回期货_持仓限制
        /// </summary>
        /// <param name="positionLimitValueID">期货-持仓限制标识</param>
        /// <returns></returns>
        public Entity.QH_PositionLimitValue GetPositionLimitValueByPositionLimitValueID(int positionLimitValueID)
        {
            try
            {
                LogHelper.WriteDebug("Debug-8527根据期货-持仓限制标识返回期货_持仓限制方法名称：GetPositionLimitValueByPositionLimitValueID(int positionLimitValueID)参数是:" + positionLimitValueID + "时间是:" + DateTime.Now);
                QH_PositionLimitValueBLL qH_PositionLimitValueBLL = new QH_PositionLimitValueBLL();
                Entity.QH_PositionLimitValue qH_PositionLimitValue =
                    qH_PositionLimitValueBLL.GetListArray(string.Format("PositionLimitValueID={0}", positionLimitValueID))
                        [0];
                if (qH_PositionLimitValue != null)
                {
                    return qH_PositionLimitValue;
                }
                return null;
            }
            catch (Exception ex)
            {
                string errCode = "GL-8527";
                string errMsg = "根据期货-持仓限制标识返回期货_持仓限制失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的商品期货_持仓取值类型
        /// </summary>
        /// <returns></returns>
        public List<Entity.QH_PositionValueType> GetAllPositionValueType()
        {
            try
            {
                LogHelper.WriteDebug("Debug-8528获取所有的商品期货_持仓取值类型方法名称:GetAllPositionValueType()" + DateTime.Now);
                QH_PositionValueTypeBLL qH_PositionValueTypeBLL = new QH_PositionValueTypeBLL();
                return qH_PositionValueTypeBLL.GetListArray(string.Empty);
            }
            catch (Exception ex)
            {
                string errCode = "GL-8528";
                string errMsg = "获取所有的商品期货_持仓取值类型失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return null;
            }
        }

        #endregion
    }
}