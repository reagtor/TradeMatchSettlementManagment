#region Using Namespace

using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.BLL.Reckoning.Logic.GZQH;
using ReckoningCounter.BLL.Reckoning.Logic.HK;
using ReckoningCounter.BLL.Reckoning.Logic.XH;
using ReckoningCounter.DAL.CustomDataAccess;
using ReckoningCounter.Model;
using ReckoningCounter.BLL.Reckoning.Logic.SPQH;

#endregion

namespace ReckoningCounter.BLL.Common
{
    /// <summary>
    /// 清算失败数据恢复处理类
    /// </summary>
    public class RescueManager
    {
        /// <summary>
        /// 操作最大编号
        /// </summary>
        private const int maxValue = 1000000000;

        public static RescueManager Instance = new RescueManager();

        /// <summary>
        /// 操作计数器
        /// </summary>
        private int counter;

        /// <summary>
        /// 操作计数锁对象
        /// </summary>
        private object counterLocker = new object();

        private object fileLocker = new object();
        private object fileLocker2 = new object();
        private bool isChecking;
        private string resuceFileName = "rescue.log";
        private string successFileName = "rescue2.log";
        private Timer timer;


        private RescueManager()
        {
            timer = new Timer();
            this.timer.Interval = 3 * 60 * 1000;
            this.timer.Elapsed += CheckRescueOperation;
            this.timer.Enabled = true;
        }

        /// <summary>
        /// 操作计数器
        /// </summary>
        private int NewNo
        {
            get
            {
                lock (counterLocker)
                {
                    if (counter >= maxValue)
                        counter = 0;
                    counter += 1;

                    return counter;
                }
            }
        }

        #region 故障恢复方法

        private List<BD_RescueTableInfo> LoadResuceObject()
        {
            List<BD_RescueTableInfo> roList = null;
            IList<string> rostrs = null;
            bool canGet = GetFile(resuceFileName, out rostrs);
            if (!canGet)
                return null;
            roList = GetResuceObjects(rostrs);
            if (Utils.IsNullOrEmpty(roList))
                return null;

            IList<string> successList = null;
            canGet = GetFile(successFileName, out successList);
            if (canGet)
            {
                IDictionary<int, BD_RescueTableInfo> roDic = new Dictionary<int, BD_RescueTableInfo>();
                foreach (var ro in roList)
                {
                    roDic[ro.Id] = ro;
                }

                foreach (var sID in successList)
                {
                    if (string.IsNullOrEmpty(sID))
                        continue;

                    int id;
                    bool canParse = int.TryParse(sID.Trim(), out id);
                    if (canParse)
                        roDic.Remove(id);
                }

                roList = new List<BD_RescueTableInfo>();
                foreach (var ro in roDic.Values)
                {
                    roList.Add(ro);
                }
            }

            return roList;
        }

        private void CheckRescueOperation(object sender, ElapsedEventArgs e)
        {
            if (isChecking)
                return;

            bool isConnect = DaoUtil.TestConnection();
            if (!isConnect)
                return;

            isChecking = true;

            List<BD_RescueTableInfo> ooList = LoadResuceObject();
            if (!Utils.IsNullOrEmpty(ooList))
            {
                foreach (var ro in ooList)
                {
                    DoResuce(ro);
                }
            }
            else
            {
                DeleteAllFile();
            }

            isChecking = false;
        }

        private void DoResuce(BD_RescueTableInfo ro)
        {
            if (ro == null)
                return;

            bool isSuccess = false;
            switch (ro.Type)
            {
                case (int)RescueType.DeleteXHEntrust:
                    isSuccess = Rescue_XH_DeleteTodayEntrust(ro);
                    break;
                case (int)RescueType.DeleteHKEntrust:
                    isSuccess = Rescue_HK_DeleteTodayEntrust(ro);
                    break;
                case (int)RescueType.DeleteQHEntrust:
                    isSuccess = Rescue_QH_DeleteTodayEntrust(ro);
                    break;
                case (int)RescueType.XHBuyHoldingProcess:
                    isSuccess = Rescue_XHBuy_InstantReckon_HoldingProcess(ro);
                    break;
                case (int)RescueType.HKBuyHoldingProcess:
                    isSuccess = Rescue_HKBuy_InstantReckon_HoldingProcess(ro);
                    break;
                case (int)RescueType.GZQHOpenHoldingProcess:
                    isSuccess = Rescue_GZQHOpen_InstantReckon_HoldingProcess(ro);
                    break;
                case (int)RescueType.SPQHOpenHoldingProcess:
                    isSuccess = Rescue_SPQHOpen_InstantReckon_HoldingProcess(ro);
                    break;
                case (int)RescueType.XHLastCheckFreezeMoney:
                    isSuccess = Rescue_XH_LastCheckFreezeMoney(ro);
                    break;
                case (int)RescueType.HKLastCheckFreezeMoney:
                    isSuccess = Rescue_HK_LastCheckFreezeMoney(ro);
                    break;
                case (int)RescueType.GZQHLastCheckFreezeMoney:
                    isSuccess = Rescue_GZQH_LastCheckFreezeMoney(ro);
                    break;
                case (int)RescueType.SPQHLastCheckFreezeMoney:
                    isSuccess = Rescue_SPQH_LastCheckFreezeMoney(ro);
                    break;
            }

            if (isSuccess)
            {
                WriteSuccessText(ro.Id.ToString());
            }
        }


        private void DeleteAllFile()
        {
            if (File.Exists(resuceFileName))
            {
                File.Delete(resuceFileName);
            }

            if (File.Exists(successFileName))
                File.Delete(successFileName);
        }

        #endregion

        #region 功能方法

        private bool GetFile(string file, out IList<string> txtList)
        {
            StreamReader inStream;
            txtList = null;

            if (!File.Exists(file))
                return false;

            try
            {
                inStream = File.OpenText(file);
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
                return false;
            }

            txtList = new List<string>();

            try
            {
                string txt;
                do
                {
                    txt = inStream.ReadLine();
                    txtList.Add(txt);
                } while (txt != null);
            }
            finally
            {
                inStream.Close();
            }
            return true;
        }

        private List<BD_RescueTableInfo> GetResuceObjects(IList<string> list)
        {
            if (Utils.IsNullOrEmpty(list))
                return null;

            List<BD_RescueTableInfo> ooList = new List<BD_RescueTableInfo>();
            foreach (var txt in list)
            {
                BD_RescueTableInfo oo = GetResuceObjectFromTxt(txt);
                if (oo != null)
                    ooList.Add(oo);
            }

            return ooList;
        }

        private BD_RescueTableInfo GetResuceObjectFromTxt(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return null;

            BD_RescueTableInfo ro = new BD_RescueTableInfo();

            string[] strs = txt.Split('|');
            ro.Id = int.Parse(strs[0]);
            ro.Type = int.Parse(strs[1]);
            ro.Value1 = strs[2];
            ro.Value2 = strs[3];
            ro.Value3 = strs[4];
            ro.Value4 = strs[5];
            ro.Value5 = strs[6];

            CheckRescueObject(ro);

            return ro;
        }

        private string SetResuceObjectToTxt(BD_RescueTableInfo ro)
        {
            if (ro == null)
                return "";

            string part = "|";
            string nulval = "*";

            if (string.IsNullOrEmpty(ro.Value1))
                ro.Value1 = nulval;

            if (string.IsNullOrEmpty(ro.Value2))
                ro.Value2 = nulval;

            if (string.IsNullOrEmpty(ro.Value3))
                ro.Value3 = nulval;

            if (string.IsNullOrEmpty(ro.Value4))
                ro.Value4 = nulval;

            if (string.IsNullOrEmpty(ro.Value5))
                ro.Value5 = nulval;

            return ro.Id + part + ro.Type + part + ro.Value1 + part + ro.Value2 + part + ro.Value3 + part + ro.Value4 +
                   part + ro.Value5;
        }

        private void CheckRescueObject(BD_RescueTableInfo ro)
        {
            if (ro == null)
                return;

            string nulval = "*";
            if (ro.Value1 == nulval)
                ro.Value1 = "";

            if (ro.Value2 == nulval)
                ro.Value2 = "";

            if (ro.Value3 == nulval)
                ro.Value3 = "";

            if (ro.Value4 == nulval)
                ro.Value4 = "";

            if (ro.Value5 == nulval)
                ro.Value5 = "";
        }

        private void WriteSuccessText(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            lock (fileLocker2)
            {
                FileInfo fi = new FileInfo(successFileName);

                if (!fi.Exists)
                {
                    //Create a file to write to.
                    using (StreamWriter sw = fi.CreateText())
                    {
                        sw.WriteLine(id);
                    }

                    return;
                }

                // This text will always be added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = fi.AppendText())
                {
                    sw.WriteLine(id);
                }
            }
        }

        private void WriteResuceText(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return;

            lock (fileLocker)
            {
                FileInfo fi = new FileInfo(resuceFileName);

                if (!fi.Exists)
                {
                    //Create a file to write to.
                    using (StreamWriter sw = fi.CreateText())
                    {
                        sw.WriteLine(txt);
                    }

                    return;
                }

                // This text will always be added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = fi.AppendText())
                {
                    sw.WriteLine(txt);
                }
            }
        }

        private void WriteResuceObject(BD_RescueTableInfo ro)
        {
            if (ro == null)
                return;

            ro.Id = NewNo;
            string txt = SetResuceObjectToTxt(ro);
            WriteResuceText(txt);
        }

        #endregion

        #region DeleteEntrust

        /// <summary>
        /// 记录必须执行的删除现货当日委托
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        public void Record_XH_DeleteTodayEntrust(string entrustNumber)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            RescueType type = RescueType.DeleteXHEntrust;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;

            WriteResuceObject(ro);
        }

        private bool Rescue_XH_DeleteTodayEntrust(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;

            return Rescue_XH_DeleteTodayEntrust(entrustNumber);
        }

        /// <summary>
        /// 执行删除现货当日委托动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <returns>是否成功</returns>
        private bool Rescue_XH_DeleteTodayEntrust(string entrustNumber)
        {
            return XHDataAccess.DeleteTodayEntrust(entrustNumber);
        }

        /// <summary>
        /// 记录必须执行的删除港股当日委托
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        public void Record_HK_DeleteTodayEntrust(string entrustNumber)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            RescueType type = RescueType.DeleteHKEntrust;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;

            WriteResuceObject(ro);
        }

        private bool Rescue_HK_DeleteTodayEntrust(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;

            return Rescue_HK_DeleteTodayEntrust(entrustNumber);
        }

        /// <summary>
        /// 执行删除港股当日委托动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <returns>是否成功</returns>
        private bool Rescue_HK_DeleteTodayEntrust(string entrustNumber)
        {
            return HKDataAccess.DeleteTodayEntrust(entrustNumber);
        }


        /// <summary>
        /// 记录必须执行的删除期货当日委托
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        public void Record_QH_DeleteTodayEntrust(string entrustNumber)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            RescueType type = RescueType.DeleteQHEntrust;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;

            WriteResuceObject(ro);
        }

        private bool Rescue_QH_DeleteTodayEntrust(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;

            return Rescue_QH_DeleteTodayEntrust(entrustNumber);
        }

        /// <summary>
        /// 执行删除期货当日委托动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <returns>委托单号</returns>
        private bool Rescue_QH_DeleteTodayEntrust(string entrustNumber)
        {
            return QHDataAccess.DeleteTodayEntrust(entrustNumber);
        }

        #endregion

        #region HoldingProcess

        /// <summary>
        /// 记录必须执行的现货买清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        public void Record_XHBuy_InstantReckon_HoldingProcess(int holdingAccountId, XHDealSum dealSum,
                                                              int holdingTradingRule)
        {
            RescueType type = RescueType.XHBuyHoldingProcess;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = holdingAccountId.ToString();
            ro.Value2 = dealSum.AmountSum.ToString();
            ro.Value3 = dealSum.CapitalSum.ToString();
            ro.Value4 = dealSum.CostSum.ToString();
            ro.Value5 = holdingTradingRule.ToString();

            WriteResuceObject(ro);
        }

        private bool Rescue_XHBuy_InstantReckon_HoldingProcess(BD_RescueTableInfo ro)
        {
            int holdingAccountId = int.Parse(ro.Value1);
            XHDealSum dealSum = new XHDealSum();
            dealSum.AmountSum = decimal.Parse(ro.Value2);
            dealSum.CapitalSum = decimal.Parse(ro.Value3);
            dealSum.CostSum = decimal.Parse(ro.Value4);
            int holdingTradingRule = int.Parse(ro.Value5);

            return Rescue_XHBuy_InstantReckon_HoldingProcess(holdingAccountId, dealSum, holdingTradingRule);
        }

        /// <summary>
        /// 执行现货买清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        private bool Rescue_XHBuy_InstantReckon_HoldingProcess(int holdingAccountId, XHDealSum dealSum,
                                                               int holdingTradingRule)
        {
            return XHReckonUnit.DoXHBuy_HoldingRescue(holdingAccountId, dealSum, holdingTradingRule);
        }

        /// <summary>
        /// 记录必须执行的港股买清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        public void Record_HKBuy_InstantReckon_HoldingProcess(int holdingAccountId, HKDealSum dealSum,
                                                              int holdingTradingRule)
        {
            RescueType type = RescueType.HKBuyHoldingProcess;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = holdingAccountId.ToString();
            ro.Value2 = dealSum.AmountSum.ToString();
            ro.Value3 = dealSum.CapitalSum.ToString();
            ro.Value4 = dealSum.CostSum.ToString();
            ro.Value5 = holdingTradingRule.ToString();

            WriteResuceObject(ro);
        }

        private bool Rescue_HKBuy_InstantReckon_HoldingProcess(BD_RescueTableInfo ro)
        {
            int holdingAccountId = int.Parse(ro.Value1);
            HKDealSum dealSum = new HKDealSum();
            dealSum.AmountSum = decimal.Parse(ro.Value2);
            dealSum.CapitalSum = decimal.Parse(ro.Value3);
            dealSum.CostSum = decimal.Parse(ro.Value4);
            int holdingTradingRule = int.Parse(ro.Value5);

            return Rescue_HKBuy_InstantReckon_HoldingProcess(holdingAccountId, dealSum, holdingTradingRule);
        }

        /// <summary>
        /// 执行港股买清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        private bool Rescue_HKBuy_InstantReckon_HoldingProcess(int holdingAccountId, HKDealSum dealSum,
                                                               int holdingTradingRule)
        {
            return HKReckonUnit.DoHKBuy_HoldingRescue(holdingAccountId, dealSum, holdingTradingRule);
        }
        #region 股指期货
        /// <summary>
        /// 记录必须执行的股指期货开仓清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        public void Record_GZQHOpen_InstantReckon_HoldingProcess(int holdingAccountId, GZQHDealSum dealSum,
                                                                 int holdingTradingRule)
        {
            RescueType type = RescueType.GZQHOpenHoldingProcess;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = holdingAccountId.ToString();
            ro.Value2 = dealSum.AmountSum.ToString();
            ro.Value3 = dealSum.CapitalSum.ToString();
            ro.Value4 = dealSum.CapitalSumNoScale.ToString();
            ro.Value5 = dealSum.CostSum.ToString();
            ro.Value6 = holdingTradingRule.ToString();

            WriteResuceObject(ro);
        }

        private bool Rescue_GZQHOpen_InstantReckon_HoldingProcess(BD_RescueTableInfo ro)
        {
            int holdingAccountId = int.Parse(ro.Value1);
            GZQHDealSum dealSum = new GZQHDealSum();
            dealSum.AmountSum = decimal.Parse(ro.Value2);
            dealSum.CapitalSum = decimal.Parse(ro.Value3);
            dealSum.CapitalSumNoScale = decimal.Parse(ro.Value4);
            dealSum.CostSum = decimal.Parse(ro.Value5);
            int holdingTradingRule = int.Parse(ro.Value6);

            return Rescue_GZQHOpen_InstantReckon_HoldingProcess(holdingAccountId, dealSum, holdingTradingRule);
        }

        /// <summary>
        /// 执行股指期货开仓清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        private bool Rescue_GZQHOpen_InstantReckon_HoldingProcess(int holdingAccountId, GZQHDealSum dealSum,
                                                                  int holdingTradingRule)
        {
            return GZQHReckonUnit.DoGZQHOpen_HoldingRescue(holdingAccountId, dealSum, holdingTradingRule);
        }
        #endregion

        #region 商品期货
        /// <summary>
        /// 记录必须执行的商品期货开仓清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        public void Record_SPQHOpen_InstantReckon_HoldingProcess(int holdingAccountId, SPQHDealSum dealSum, int holdingTradingRule)
        {
            RescueType type = RescueType.SPQHOpenHoldingProcess;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = holdingAccountId.ToString();
            ro.Value2 = dealSum.AmountSum.ToString();
            ro.Value3 = dealSum.CapitalSum.ToString();
            ro.Value4 = dealSum.CapitalSumNoScale.ToString();
            ro.Value5 = dealSum.CostSum.ToString();
            ro.Value6 = holdingTradingRule.ToString();

            WriteResuceObject(ro);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ro"></param>
        /// <returns></returns>
        private bool Rescue_SPQHOpen_InstantReckon_HoldingProcess(BD_RescueTableInfo ro)
        {
            int holdingAccountId = int.Parse(ro.Value1);
            SPQHDealSum dealSum = new SPQHDealSum();
            dealSum.AmountSum = decimal.Parse(ro.Value2);
            dealSum.CapitalSum = decimal.Parse(ro.Value3);
            dealSum.CapitalSumNoScale = decimal.Parse(ro.Value4);
            dealSum.CostSum = decimal.Parse(ro.Value5);
            int holdingTradingRule = int.Parse(ro.Value6);

            return Rescue_SPQHOpen_InstantReckon_HoldingProcess(holdingAccountId, dealSum, holdingTradingRule);
        }

        /// <summary>
        /// 执行商品期货开仓清算-持仓操作
        /// </summary>
        /// <param name="holdingAccountId">持仓id</param>
        /// <param name="dealSum">成交汇总</param>
        /// <param name="holdingTradingRule">持仓交割制度</param>
        private bool Rescue_SPQHOpen_InstantReckon_HoldingProcess(int holdingAccountId, SPQHDealSum dealSum, int holdingTradingRule)
        {
            return SPQHReckonUnit.DoSPQHOpen_HoldingRescue(holdingAccountId, dealSum, holdingTradingRule);
        }
        #endregion

        #endregion

        #region LastCheckFreezeMoney

        /// <summary>
        /// 记录必须执行的现货LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        public void Record_XH_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (capitalAccountId == -1)
                return;

            RescueType type = RescueType.XHLastCheckFreezeMoney;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;
            ro.Value2 = capitalAccountId.ToString();

            WriteResuceObject(ro);
        }

        private bool Rescue_XH_LastCheckFreezeMoney(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;
            int capitalAccountId = int.Parse(ro.Value2);

            return Rescue_XH_LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }

        /// <summary>
        /// 执行现货LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        private bool Rescue_XH_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            return XHReckonUnit.LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }

        /// <summary>
        /// 记录必须执行的港股LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        public void Record_HK_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (capitalAccountId == -1)
                return;

            RescueType type = RescueType.HKLastCheckFreezeMoney;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;
            ro.Value2 = capitalAccountId.ToString();

            WriteResuceObject(ro);
        }

        private bool Rescue_HK_LastCheckFreezeMoney(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;
            int capitalAccountId = int.Parse(ro.Value2);

            return Rescue_HK_LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }

        /// <summary>
        /// 执行港股LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        private bool Rescue_HK_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            return HKReckonUnit.LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }

        #region 股指期货
        /// <summary>
        /// 记录必须执行的股指期货LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        public void Record_GZQH_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (capitalAccountId == -1)
                return;

            RescueType type = RescueType.GZQHLastCheckFreezeMoney;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;
            ro.Value2 = capitalAccountId.ToString();

            WriteResuceObject(ro);
        }

        private bool Rescue_GZQH_LastCheckFreezeMoney(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;
            int capitalAccountId = int.Parse(ro.Value2);

            return Rescue_GZQH_LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }

        /// <summary>
        /// 执行股指期货LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        private bool Rescue_GZQH_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            return GZQHReckonUnit.LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }
        #endregion

        #region 商品期货
        /// <summary>
        /// 记录必须执行的商品期货LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        public void Record_SPQH_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            if (string.IsNullOrEmpty(entrustNumber))
                return;

            if (capitalAccountId == -1)
                return;

            RescueType type = RescueType.SPQHLastCheckFreezeMoney;
            BD_RescueTableInfo ro = new BD_RescueTableInfo();
            ro.Type = (int)type;
            ro.Value1 = entrustNumber;
            ro.Value2 = capitalAccountId.ToString();

            WriteResuceObject(ro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ro"></param>
        /// <returns></returns>
        private bool Rescue_SPQH_LastCheckFreezeMoney(BD_RescueTableInfo ro)
        {
            string entrustNumber = ro.Value1;
            int capitalAccountId = int.Parse(ro.Value2);

            return Rescue_SPQH_LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }

        /// <summary>
        /// 执行商品期货LastCheckFreezeMoney动作
        /// </summary>
        /// <param name="entrustNumber">委托单号</param>
        /// <param name="capitalAccountId">资金id</param>
        private bool Rescue_SPQH_LastCheckFreezeMoney(string entrustNumber, int capitalAccountId)
        {
            return SPQHReckonUnit.LastCheckFreezeMoney(entrustNumber, capitalAccountId);
        }
        #endregion

        #endregion
    }

    public enum RescueType
    {
        DeleteXHEntrust = 1,
        DeleteQHEntrust = 2,
        DeleteHKEntrust = 3,

        XHBuyHoldingProcess = 4,
        GZQHOpenHoldingProcess = 5,
        SPQHOpenHoldingProcess = 6,
        HKBuyHoldingProcess = 7,

        XHLastCheckFreezeMoney = 8,
        GZQHLastCheckFreezeMoney = 9,
        SPQHLastCheckFreezeMoney = 10,
        HKLastCheckFreezeMoney = 11
    }
}