#region Using Namespace

using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Timers;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Common;
using ReckoningCounter.BLL.DelegateAcceptOffer.OrderOffer;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;
using ReckoningCounter.DAL.MatchCenterService;
using McWcf = ReckoningCounter.DAL.MatchCenterService;
using Timer=System.Timers.Timer;

#endregion

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 报盘及回送通道处理,错误编码2160-2169
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class SeviceChannelManager
    {
        //向撮合发生的心跳间隔（秒）
        private const int heartInternal = 30;

        #region == 字段/属性 ==

        /// <summary>
        /// 成交回送通道定时检查器
        /// </summary>
        private Timer _callBackChannelChecker;

        /// <summary>
        /// 成交回送通道检查状态
        /// </summary>
        private bool _callbackChannelIsChecking;

        private string _strClientId = string.Empty;

        /// <summary>
        /// WCF客户端参数配置节
        /// </summary>
        private string _strDoOrderConfigurationSectionName = string.Empty;

        private string _strOrderDealRptConfigurationSectionName = string.Empty;


        /// <summary>
        /// 成交回送通道对象容器
        /// </summary>
        private Dictionary<string, DealRptChannelState> _wcfDealRptChannelList;

        private ReaderWriterLockSlim _wcfDealRptChannelListLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 下单通道对象容器
        /// </summary>
        private Dictionary<string, DoOrderClient> _wcfDoOrderChannelList;

        //private ReaderWriterLockSlim _wcfDoOrderChannelListLock = new ReaderWriterLockSlim();

        #endregion

        #region == 构造器 ==

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="strConfigurationSectionName">客户端下单通道参数配置节</param>
        /// <param name="strOrderDealRptConfigurationSectionName">客户端成交回报通道参数配置节</param>
        /// <param name="strClientId"></param>
        public SeviceChannelManager(string strConfigurationSectionName, string strOrderDealRptConfigurationSectionName
                                    , string strClientId)
        {
            _callbackChannelIsChecking = false;
            _strDoOrderConfigurationSectionName = strConfigurationSectionName;
            _strOrderDealRptConfigurationSectionName = strOrderDealRptConfigurationSectionName;
            _wcfDoOrderChannelList = new Dictionary<string, DoOrderClient>();
            _wcfDealRptChannelList = new Dictionary<string, DealRptChannelState>();
            _strClientId = strClientId;

            _callBackChannelChecker = new Timer(heartInternal*1000);
            _callBackChannelChecker.Elapsed += _callBackChannelChecker_Elapsed;
            _callBackChannelChecker.Enabled = true;
        }

        #endregion

        #region == 方法 ==

        private void _callBackChannelChecker_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_callbackChannelIsChecking)
                return;

            try
            {
                _callbackChannelIsChecking = true;
                CheckCallbackChannel();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            finally
            {
                this._callbackChannelIsChecking = false;
            }
        }

        /// <summary>
        /// 回送通道检查
        /// </summary>
        private void CheckCallbackChannel()
        {
            string strMessage = string.Empty;

            lock (((ICollection) _wcfDealRptChannelList).SyncRoot)
            {
                foreach (var pair in _wcfDealRptChannelList.Keys)
                {
                    DealRptChannelState drcs = null;
                    try
                    {
                        drcs = _wcfDealRptChannelList[pair];
                        if (drcs == null)
                        {
                            continue;
                        }

                        if (drcs.State == ChannelState.CSError)
                        {
                            CheckFailureAction(drcs, ref strMessage);
                            continue;
                        }

                        //LogHelper.WriteDebug("ServiceChannelManger.CheckCallbackChannel");
                        drcs.State = ChannelState.CSChecking;

                        OrderDealRptClient odrc = drcs.DealRptChannel;
                        if (odrc == null)
                        {
                            CheckFailureAction(drcs, ref strMessage);
                            continue;
                        }

                        odrc.CheckChannel();
                    }
                    catch (Exception ex)
                    {
                        if (drcs != null) drcs.State = ChannelState.CSError;
                        LogHelper.WriteError("GT-2160:[报盘处理]定时检查成交回送通道异常", ex);
                    }
                }
            }
        }

        private void CheckFailureAction(DealRptChannelState drcs, ref string strMessage)
        {
            LogHelper.WriteInfo(
                "===================ServiceChannelManger.CheckCallbackChannel:回送通道检查失败！关闭失败的通道，重新创建新通道。");
            drcs.DealRptChannel.DoClose();
            drcs.DealRptChannel = this.CreateDealRptChannel(drcs.ServiceAddress, ref strMessage);
            drcs.State = ChannelState.CSNormal;
        }

        /// <summary>
        /// 创建报盘通道，每一个服务地址维护一个实例
        /// 即一个撮合中心一个下单连接
        /// </summary>
        /// <param name="strIp">服务器</param>
        /// <param name="strPort">服务器端口</param>
        /// <param name="strDoOrderServiceName">下单服务名</param>
        /// <param name="strDealRptServiceName">成交回送服务名</param>
        ///  <param name="state">成交回送通道状态</param>
        /// <param name="strMessage"></param>
        /// <returns>报盘通道实例</returns>
        public DoOrderClient GetDoOrderChannel(string strIp, string strPort,
                                               string strDoOrderServiceName, string strDealRptServiceName,
                                               ref ChannelState state, ref string strMessage)
        {
            LogHelper.WriteDebug("ServiceChannelManger.GetDoOrderChannel");
            DoOrderClient resultObj = null;
            if (_wcfDoOrderChannelList != null)
            {
                var strDoOrderAddress = string.Format("net.tcp://{0}:{1}/{2}", strIp, strPort, strDoOrderServiceName);

                string strPort2 = "";
                try
                {
                    int dealPort = int.Parse(strPort);
                    dealPort++;
                    strPort2 = dealPort.ToString();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                    strPort2 = strPort;
                }

                var strDealRptAddress = string.Format("net.tcp://{0}:{1}/{2}", strIp, strPort2,
                                                      strDealRptServiceName);

                _wcfDealRptChannelListLock.EnterUpgradeableReadLock();
                try
                {
                    //如果缓存中没有，那么新建
                    if (!_wcfDoOrderChannelList.TryGetValue(strDoOrderAddress, out resultObj))
                    {
                        _wcfDealRptChannelListLock.EnterWriteLock();
                        try
                        {
                            var doOrderChannel = CreateDoOrderChannel(strDoOrderAddress, ref strMessage);
                            if (doOrderChannel != null)
                            {
                                resultObj = doOrderChannel;
                                _wcfDoOrderChannelList[strDoOrderAddress] = doOrderChannel;

                                var dealBackChannel = this.CreateDealRptChannel(strDealRptAddress, ref strMessage);
                                if (dealBackChannel != null)
                                {
                                    _wcfDealRptChannelList[strDealRptAddress] = new DealRptChannelState(dealBackChannel,
                                                                                                        ChannelState.
                                                                                                            CSNormal,
                                                                                                        strDealRptAddress);
                                }
                            }
                        }
                        finally
                        {
                            _wcfDealRptChannelListLock.ExitWriteLock();
                        }
                    }
                    else
                    {
                        if (resultObj != null &&
                            ((_wcfDealRptChannelList[strDealRptAddress].State == ChannelState.CSError) ||
                             (resultObj.State == CommunicationState.Faulted ||
                              resultObj.State == CommunicationState.Closed)))
                        {
                            _wcfDealRptChannelListLock.EnterWriteLock();
                            try
                            {
                                LogHelper.WriteInfo("ServiceChannelManager.GetDoOrderChannel下单通道出错，状态为" +
                                                     resultObj.State);
                                RemoveDoOrderAndRptChannel(strDoOrderAddress, strDealRptAddress);

                                resultObj = null;
                            }
                            finally
                            {
                                _wcfDealRptChannelListLock.ExitWriteLock();
                            }
                        }
                    }
                }
                finally
                {
                    _wcfDealRptChannelListLock.ExitUpgradeableReadLock();
                }

                #region old code

                //lock (((ICollection) _wcfDealRptChannelList).SyncRoot)
                //{
                //    var strDoOrderAddress = string.Format("net.tcp://{0}:{1}/{2}", strIp, strPort, strDoOrderServiceName);

                //    string strPort2 = "";
                //    try
                //    {
                //        int dealPort = int.Parse(strPort);
                //        dealPort++;
                //        strPort2 = dealPort.ToString();
                //    }
                //    catch (Exception ex)
                //    {
                //        LogHelper.WriteError(ex.Message, ex);
                //        strPort2 = strPort;
                //    }

                //    var strDealRptAddress = string.Format("net.tcp://{0}:{1}/{2}", strIp, strPort2,
                //                                          strDealRptServiceName);

                //    //如果缓存中没有，那么新建
                //    if (!_wcfDoOrderChannelList.ContainsKey(strDoOrderAddress))
                //    {
                //        var doOrderChannel = CreateDoOrderChannel(strDoOrderAddress, ref strMessage);
                //        if (doOrderChannel != null)
                //        {
                //            resultObj = doOrderChannel;
                //            _wcfDoOrderChannelList[strDoOrderAddress] = doOrderChannel;

                //            var dealBackChannel = this.CreateDealRptChannel(strDealRptAddress, ref strMessage);
                //            if (dealBackChannel != null)
                //            {
                //                _wcfDealRptChannelList[strDealRptAddress] = new DealRptChannelState(dealBackChannel,
                //                                                                                    ChannelState.
                //                                                                                        CSNormal,
                //                                                                                    strDealRptAddress);
                //            }
                //        }
                //    }
                //        //if (_wcfDoOrderChannelList.ContainsKey(strDoOrderAddress))
                //    else
                //    {
                //        resultObj = _wcfDoOrderChannelList[strDoOrderAddress];

                //        if (resultObj != null &&
                //            ((_wcfDealRptChannelList[strDealRptAddress].State == ChannelState.CSError) ||
                //             (resultObj.State == CommunicationState.Faulted ||
                //              resultObj.State == CommunicationState.Closed)))
                //        {
                //            //_wcfDoOrderChannelList.Remove(strDoOrderAddress);
                //            //_wcfDealRptChannelList.Remove(strDealRptAddress);
                //            LogHelper.WriteDebug("ServiceChannelManager.GetDoOrderChannel下单通道出错，状态为" + resultObj.State);
                //            RemoveDoOrderAndRptChannel(strDoOrderAddress, strDealRptAddress);

                //            resultObj = null;
                //        }
                //    }
                //}

                #endregion
            }
            return resultObj;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strIp"></param>
        /// <param name="strPort"></param>
        /// <param name="strDoOrderServiceName"></param>
        /// <param name="strDealRptServiceName"></param>
        /// <returns></returns>
        public bool RemoveDoOrderAndRptChannel(string strIp, string strPort,
                                               string strDoOrderServiceName, string strDealRptServiceName)
        {
            var strDoOrderAddress = BuildRemotingServiceAddress(strIp, strPort, strDoOrderServiceName);
            var strDealRptAddress = BuildRemotingServiceAddress(strIp, strPort, strDealRptServiceName);

            bool result = RemoveDoOrderAndRptChannel(strDoOrderAddress, strDealRptAddress);
            return result;
        }

        private bool RemoveDoOrderAndRptChannel(string strDoOrderAddress, string strDealRptAddress)
        {
            if (_wcfDoOrderChannelList != null && _wcfDealRptChannelList != null)
            {
                CloseChannel(strDoOrderAddress, strDealRptAddress);

                LogHelper.WriteDebug("ServiceChannelManger.RemoveDoOrderAndRptChannel:移除缓存中的下单通道和回送通道");


                return _wcfDoOrderChannelList.Remove(strDoOrderAddress) &&
                       _wcfDealRptChannelList.Remove(strDealRptAddress);
            }

            return false;
        }


        /// <summary>
        /// 关闭下单和回报通道
        /// </summary>
        /// <param name="strIp"></param>
        /// <param name="strPort"></param>
        /// <param name="strDoOrderServiceName"></param>
        /// <param name="strDealRptServiceName"></param>
        private void CloseChannel(string strIp, string strPort,
                                  string strDoOrderServiceName, string strDealRptServiceName)
        {
            var strDoOrderAddress = BuildRemotingServiceAddress(strIp, strPort, strDoOrderServiceName);
            var strDealRptAddress = BuildRemotingServiceAddress(strIp, strPort, strDealRptServiceName);

            CloseChannel(strDoOrderAddress, strDealRptAddress);
        }

        private void CloseChannel(string strDoOrderAddress, string strDealRptAddress)
        {
            if (_wcfDoOrderChannelList.ContainsKey(strDoOrderAddress))
            {
                LogHelper.WriteInfo("ServiceChannelManger.CloseChannel:关闭下单通道");
                var doOrder = _wcfDoOrderChannelList[strDoOrderAddress];
                doOrder.DoClose();
            }

            if (_wcfDealRptChannelList.ContainsKey(strDealRptAddress))
            {
                LogHelper.WriteInfo("ServiceChannelManger.CloseChannel:关闭回送通道");
                var dealRpt = _wcfDealRptChannelList[strDealRptAddress];
                dealRpt.DealRptChannel.DoClose();
            }
        }

        /// <summary>
        /// 构服务地址
        /// </summary>
        /// <param name="strIp"></param>
        /// <param name="strPort"></param>
        /// <param name="strServiceName"></param>
        /// <returns></returns>
        private string BuildRemotingServiceAddress(string strIp, string strPort,
                                                   string strServiceName)
        {
            string strResult;
            strResult = string.Format("net.tcp://{0}:{1}/{2}", strIp, strPort, strServiceName);
            return strResult;
        }

        /// <summary>
        /// 创建下单通道实例
        /// </summary>
        /// <param name="strRemotingServiceAddress"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private DoOrderClient CreateDoOrderChannel(string strRemotingServiceAddress, ref string strMessage)
        {
            DoOrderClient resultObj = null;
            try
            {
                LogHelper.WriteDebug(string.Format("ServiceChannelManger.CreateDoOrderChannel[{0}]",
                                                   strRemotingServiceAddress));
                resultObj = new DoOrderClient(this._strDoOrderConfigurationSectionName, strRemotingServiceAddress);
                ICommunicationObject co = resultObj;
                co.Faulted +=
                    DoOrderChannelFaultEvent;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2161:[报盘处理]创建下单通道实例异常";
                resultObj = null;
                LogHelper.WriteError(strMessage, ex);
            }
            return resultObj;
        }

        private static void DoOrderChannelFaultEvent(object sender, EventArgs e)
        {
            LogHelper.WriteInfo(
                "*******************ServiceChannelManger.DoOrderChannel回送通道发生异常*******************");

            IContextChannel channel = sender as IContextChannel;
            if (channel == null)
                return;

            channel.DoClose();
        }

        private OrderDealRptClient CreateDealRptChannel(string strRemotingServiceAddress, ref string strMessage)
        {
            var result = InternalCreateDealRptChannel(strRemotingServiceAddress, ref strMessage);

            //再创建一次
            if (result == null)
            {
                LogHelper.WriteDebug("SeviceChannelManager.CreateDealRptChannel再创建一次");
                result = InternalCreateDealRptChannel(strRemotingServiceAddress, ref strMessage);
            }

            return result;
        }

        /// <summary>
        /// 创建回送通道实例
        /// </summary>
        /// <param name="strRemotingServiceAddress"></param>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        private OrderDealRptClient InternalCreateDealRptChannel(string strRemotingServiceAddress, ref string strMessage)
        {
            OrderDealRptClient resultObj = null;
            try
            {
                LogHelper.WriteDebug(string.Format("ServiceChannelManger.CreateDealRptChannel[{0}]",
                                                   strRemotingServiceAddress));

                var _wcfCallbackObj = new DoCallbackProcess();
                var ic = new InstanceContext(_wcfCallbackObj);
                resultObj = new OrderDealRptClient(ic, this._strOrderDealRptConfigurationSectionName,
                                                   strRemotingServiceAddress);
                resultObj.RegisterChannel(this._strClientId);

                ICommunicationObject co = resultObj;
                co.Faulted += DealRptChannelFaultEvent;
            }
            catch (Exception ex)
            {
                strMessage = "GT-2162:[报盘处理]创建成交回送通道实例异常";
                LogHelper.WriteError(strMessage, ex);
                resultObj = null;
            }
            return resultObj;
        }

        private static void DealRptChannelFaultEvent(object sender, EventArgs e)
        {
            LogHelper.WriteInfo(
                "*******************ServiceChannelManger.DealRptChannel回送通道发生异常*******************");

            IContextChannel channel = sender as IContextChannel;
            if (channel == null)
                return;

            channel.DoClose();
        }

        #endregion

        #region 退出时清理

        public void DoClose()
        {
            //关闭所有的回送通道
            lock (((ICollection)_wcfDealRptChannelList).SyncRoot)
            {
                foreach (var state in _wcfDealRptChannelList.Values)
                {
                    try
                    {
                        if (state.DealRptChannel != null)
                        {
                            state.DealRptChannel.UnRegisterChannel(this._strClientId);
                            state.DealRptChannel.DoClose();
                        }
                    }
                    catch (Exception ex)
                    {
                       LogHelper.WriteError(ex.Message, ex);
                    }
                }
            }

            //关闭所有的下单通道
            lock (((ICollection)_wcfDoOrderChannelList).SyncRoot)
            {
                foreach (var val in _wcfDoOrderChannelList.Values)
                {
                    try
                    {
                        val.DoClose();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex.Message,ex);
                    }
                }
            }
        }

        #endregion
    }
}