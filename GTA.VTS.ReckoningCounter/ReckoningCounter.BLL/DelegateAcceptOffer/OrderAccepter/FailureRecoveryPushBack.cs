using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GTA.VTS.Common.CommonUtility;
using System.Runtime.CompilerServices;
using ReckoningCounter.DAL.Data.QH;
using ReckoningCounter.DAL.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Threading;
using ReckoningCounter.DAL.Data.HK;

namespace ReckoningCounter.BLL
{
    /// <summary>
    /// 提供回推数据故障恢复数据记录到文件中去
    /// 本功能类的调用使用工厂类new 静态实例调用FailureRecoveryFactory
    /// 作者：李健华
    /// 日期：2009-09-10
    /// </summary>
    public class FailureRecoveryPushBack
    {
        #region  变量定义
        /// <summary>
        /// 获取当前程序工作目录
        /// </summary>
        private string fileName = Directory.GetCurrentDirectory();
        //"E:\\MyProject\\NowProject\\虚拟交易系统\\1.工作区\\编码\\瑞尔格特虚拟交易系统v1.1\\TestResults"; //
        /// <summary>
        /// 记录现货回推数据文件名
        /// </summary>
        private string XHPushFileName = "XHPushBack.txt";
        /// <summary>
        /// 记录期货回推数据文件名
        /// </summary>
        private string QHPushFileName = "QHPushBack.txt";
        /// <summary>
        /// 记录港股回推数据文件名
        /// </summary>
        private string HKPushFileName = "HKPushBack.txt";
        /// <summary>
        /// 记录港股改单回推数据文件名
        /// </summary>
        private string HKModifyPushFileName = "HKModifyPushFileName.txt";
        #endregion

        #region 把数据记录到文件中
        /// <summary>
        /// 记录回推数据
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        /// <param name="isStock">0，现货，1，期货，2港股,3港股改单</param>
        private void WriterFailurePushData(string tradeNumber, string channelID, int isStock)
        {
            FileStream fs = null;
            string pushFileName = fileName;
            if (isStock == 0)
            {
                pushFileName = pushFileName + "\\" + XHPushFileName;
            }
            else if (isStock == 1)
            {
                pushFileName = pushFileName + "\\" + QHPushFileName;
            }
            else if (isStock == 2)
            {
                pushFileName = pushFileName + "\\" + HKPushFileName;
            }
            else if (isStock == 3)
            {
                pushFileName = pushFileName + "\\" + HKModifyPushFileName;
            }
            try
            {
                if (!File.Exists(pushFileName))
                {
                    using (File.Create(pushFileName)) { }
                }
                fs = File.Open(pushFileName, FileMode.Append, FileAccess.Write);
                byte[] arr = System.Text.Encoding.Default.GetBytes(tradeNumber.Trim() + "," + channelID.Trim() + "\r\n");
                fs.Write(arr, 0, arr.Length);
            }
            catch (Exception ex)
            {

                LogHelper.WriteError("文件保存回推" + isStock + " 数据失败", ex);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }


        /// <summary>
        /// 删除清空文件
        /// </summary>
        /// <param name="isType">是否是清空现货数据0，现货，1，期货，2港股，3港股改单</param>
        private void ClearData(int isType)
        {
            try
            {
                string clearFileName = fileName;
                if (isType == 0)
                {
                    clearFileName += "\\" + XHPushFileName;
                }
                else if (isType == 1)
                {
                    clearFileName += "\\" + QHPushFileName;
                }
                else if (isType == 2)
                {
                    clearFileName += "\\" + HKPushFileName;
                }
                else if (isType == 3)
                {
                    clearFileName += "\\" + HKModifyPushFileName;
                }
                if (File.Exists(clearFileName))
                {
                    File.Delete(clearFileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("删除回推数据" + isType + "文件失败", ex);
            }
        }
        #endregion

        #region 把数据写到数据库中，导入数据库
        /// <summary>
        /// 把文件中的现货回推数据导入到数据库中
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReaderXHPushDataToDB()
        {
            XH_PushBackOrderTableDal dal = new XH_PushBackOrderTableDal();
            if (!dal.JudgeConnectionState())
            {
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string filePath = fileName + "\\" + XHPushFileName;
                    if (!File.Exists(filePath))
                    {
                        break;
                    }
                    using (StreamReader sr = new StreamReader(filePath))
                    {

                        Database db = DatabaseFactory.CreateDatabase();
                        string s;
                        s = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            try
                            {
                                string[] strs = s.Split(',');
                                if (strs.Length > 1)
                                {
                                    string tradeNumber = strs[0].Trim();
                                    string channelID = strs[1].Trim();
                                    if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                    {
                                        dal.Add(tradeNumber, channelID, db);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteError("读取现货回推文件数据插入数据库失败!", ex);
                                s = sr.ReadLine();
                                continue;
                            }
                            s = sr.ReadLine();
                        }
                        #region 为了防止最后一条数据作为已经为最后文件流内容不再操作所以再检查一次最后读到的内容
                        try
                        {
                            string[] strs = s.Split(',');
                            if (strs.Length > 1)
                            {
                                string tradeNumber = strs[0].Trim();
                                string channelID = strs[1].Trim();
                                if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                {
                                    dal.Add(tradeNumber, channelID, db);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError("最后一条数据插入失败!", ex);
                        }
                        #endregion
                    }
                    ClearData(0);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("打开现货回推数据文件异常，程序进入等待100毫秒再重试，第 " + i + " 次等待!", ex);
                    Thread.CurrentThread.Join(100);
                    continue;
                    //重试五次
                }
                break;
            }
        }
        /// <summary>
        /// 把文件中的期货回推数据导入到数据库中
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReaderQHPushDataToDB()
        {
            QH_PushBackOrderTableDal dal = new QH_PushBackOrderTableDal();
            if (!dal.JudgeConnectionState())
            {
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string filePath = fileName + "\\" + QHPushFileName;
                    if (!File.Exists(filePath))
                    {
                        break;
                    }
                    using (StreamReader sr = new StreamReader(filePath))
                    {

                        Database db = DatabaseFactory.CreateDatabase();
                        string s;
                        s = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            try
                            {
                                string[] strs = s.Split(',');
                                if (strs.Length > 1)
                                {
                                    string tradeNumber = strs[0].Trim();
                                    string channelID = strs[1].Trim();
                                    if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                    {
                                        dal.Add(tradeNumber, channelID, db);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteError("读取期货回推文件数据插入数据库失败!", ex);
                                s = sr.ReadLine();
                                continue;
                            }
                            s = sr.ReadLine();
                        }
                        #region 为了防止最后一条数据作为已经为最后文件流内容不再操作所以再检查一次最后读到的内容
                        try
                        {
                            string[] strs = s.Split(',');
                            if (strs.Length > 1)
                            {
                                string tradeNumber = strs[0].Trim();
                                string channelID = strs[1].Trim();
                                if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                {
                                    dal.Add(tradeNumber, channelID, db);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError("最后一条数据插入失败!", ex);
                        }
                        #endregion
                    }
                    ClearData(1);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("打开期货回推数据文件异常，程序进入等待100毫秒再重试，第 " + i + " 次等待!", ex);
                    Thread.CurrentThread.Join(100);
                    continue;
                    //重试五次
                }
                break;
            }
        }
        /// <summary>
        /// 把文件中的港股回推数据导入到数据库中
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReaderHKPushDataToDB()
        {
            HK_PushBackOrderDal dal = new HK_PushBackOrderDal();
            if (!dal.JudgeConnectionState())
            {
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string filePath = fileName + "\\" + HKPushFileName;
                    if (!File.Exists(filePath))
                    {
                        break;
                    }
                    using (StreamReader sr = new StreamReader(filePath))
                    {

                        Database db = DatabaseFactory.CreateDatabase();
                        string s;
                        s = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            try
                            {
                                string[] strs = s.Split(',');
                                if (strs.Length > 1)
                                {
                                    string tradeNumber = strs[0].Trim();
                                    string channelID = strs[1].Trim();
                                    if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                    {
                                        dal.Add(tradeNumber, channelID, db);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteError("读取港股回推文件数据插入数据库失败!", ex);
                                s = sr.ReadLine();
                                continue;
                            }
                            s = sr.ReadLine();
                        }
                        #region 为了防止最后一条数据作为已经为最后文件流内容不再操作所以再检查一次最后读到的内容
                        try
                        {
                            string[] strs = s.Split(',');
                            if (strs.Length > 1)
                            {
                                string tradeNumber = strs[0].Trim();
                                string channelID = strs[1].Trim();
                                if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                {
                                    dal.Add(tradeNumber, channelID, db);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError("最后一条数据插入失败!", ex);
                        }
                        #endregion
                    }
                    ClearData(2);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("打开港股回推数据文件异常，程序进入等待100毫秒再重试，第 " + i + " 次等待!", ex);
                    Thread.CurrentThread.Join(100);
                    continue;
                    //重试五次
                }
                break;
            }
        }
        /// <summary>
        /// 把文件中的港股改单回推数据导入到数据库中
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReaderHKModifyPushDataToDB()
        {
            HK_ModifyPushBackOrderDal dal = new HK_ModifyPushBackOrderDal();
            if (!dal.JudgeConnectionState())
            {
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string filePath = fileName + "\\" + HKModifyPushFileName;
                    if (!File.Exists(filePath))
                    {
                        break;
                    }
                    using (StreamReader sr = new StreamReader(filePath))
                    {

                        Database db = DatabaseFactory.CreateDatabase();
                        string s;
                        s = sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            try
                            {
                                string[] strs = s.Split(',');
                                if (strs.Length > 1)
                                {
                                    string tradeNumber = strs[0].Trim();
                                    string channelID = strs[1].Trim();
                                    if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                    {
                                        dal.Add(tradeNumber, channelID);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteError("读取港股改单回推文件数据插入数据库失败!", ex);
                                s = sr.ReadLine();
                                continue;
                            }
                            s = sr.ReadLine();
                        }
                        #region 为了防止最后一条数据作为已经为最后文件流内容不再操作所以再检查一次最后读到的内容
                        try
                        {
                            string[] strs = s.Split(',');
                            if (strs.Length > 1)
                            {
                                string tradeNumber = strs[0].Trim();
                                string channelID = strs[1].Trim();
                                if (!string.IsNullOrEmpty(tradeNumber) && !string.IsNullOrEmpty(channelID))
                                {
                                    dal.Add(tradeNumber, channelID);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError("最后一条数据插入失败!", ex);
                        }
                        #endregion
                    }
                    ClearData(3);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("打开港股改单回推数据文件异常，程序进入等待100毫秒再重试，第 " + i + " 次等待!", ex);
                    Thread.CurrentThread.Join(100);
                    continue;
                    //重试五次
                }
                break;
            }
        }
        #endregion

        /// <summary>
        /// 现货数据读取或者写入
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void XHWriter(string tradeNumber, string channelID)
        {
            if (string.IsNullOrEmpty(tradeNumber) || string.IsNullOrEmpty(channelID))
            {
                return;
            }
            else
            {
                WriterFailurePushData(tradeNumber, channelID, 0);
            }

        }
        /// <summary>
        /// 期货数据读取或者写入
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void QHWriter(string tradeNumber, string channelID)
        {

            if (string.IsNullOrEmpty(tradeNumber) || string.IsNullOrEmpty(channelID))
            {
                return;
            }
            else
            {
                WriterFailurePushData(tradeNumber, channelID, 1);
            }
        }
        /// <summary>
        /// 港股数据读取或者写入
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void HKWriter(string tradeNumber, string channelID)
        {
            if (string.IsNullOrEmpty(tradeNumber) || string.IsNullOrEmpty(channelID))
            {
                return;
            }
            else
            {
                WriterFailurePushData(tradeNumber, channelID, 2);
            }

        }
        /// <summary>
        /// 港股改单数据读取或者写入
        /// </summary>
        /// <param name="newRequestNumber"></param>
        /// <param name="channelID"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void HKModifyWriter(string newRequestNumber, string channelID)
        {
            if (string.IsNullOrEmpty(newRequestNumber) || string.IsNullOrEmpty(channelID))
            {
                return;
            }
            else
            {
                WriterFailurePushData(newRequestNumber, channelID, 3);
            }

        }
    }
    /// <summary>
    /// 提供对故障恢复类的辅助锁定控制多线程下同时互赤抢占资源对文件的破坏，对方法提供对象上锁定
    /// 所有对FailureRecoveryPushBack类的操作从这里调用
    /// 作者：李健华
    /// 日期：2009-09-10
    /// </summary>
    public class FailureRecoveryFactory
    {
        private static FailureRecoveryPushBack frp = new FailureRecoveryPushBack();
        private static bool IsXHImp = false;
        private static bool IsQHImp = false;
        private static bool IsHKImp = false;
        private static bool IsHKModifyImp = false;

        /// <summary>
        /// 现货回推数据记录到文件中
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        public static void XHWriter(string tradeNumber, string channelID)
        {
            Monitor.Enter(frp);
            frp.XHWriter(tradeNumber, channelID);
            IsXHImp = false;
            Monitor.Exit(frp);
        }
        /// <summary>
        /// 现货文件数据读取写入到数据库中
        /// </summary>
        public static void XHReaderToDB()
        {
            Monitor.Enter(frp);
            if (!IsXHImp)
            {
                frp.ReaderXHPushDataToDB();
                IsXHImp = true;
            }
            Monitor.Pulse(frp);
            Monitor.Exit(frp);
        }
        /// <summary>
        /// 期货回推数据记录到文件中
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        public static void QHWriter(string tradeNumber, string channelID)
        {
            Monitor.Enter(frp);
            frp.QHWriter(tradeNumber, channelID);
            IsQHImp = false;
            Monitor.Exit(frp);
        }
        /// <summary>
        /// 期货文件数据读取写入到数据库中
        /// </summary>
        public static void QHReaderToDB()
        {
            Monitor.Enter(frp);
            if (!IsQHImp)
            {
                frp.ReaderQHPushDataToDB();
                IsQHImp = true;
            }
            Monitor.Pulse(frp);
            Monitor.Exit(frp);
        }

        /// <summary>
        /// 港股回推数据记录到文件中
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="channelID"></param>
        public static void HKWriter(string tradeNumber, string channelID)
        {
            Monitor.Enter(frp);
            frp.HKWriter(tradeNumber, channelID);
            IsHKImp = false;
            Monitor.Exit(frp);
        }
        /// <summary>
        /// 港股文件数据读取写入到数据库中
        /// </summary>
        public static void HKReaderToDB()
        {
            Monitor.Enter(frp);
            if (!IsHKImp)
            {
                frp.ReaderHKPushDataToDB();
                IsHKImp = true;
            }
            Monitor.Pulse(frp);
            Monitor.Exit(frp);
        }

        /// <summary>
        /// 港股回推改单数据记录到文件中
        /// </summary>
        /// <param name="newRquestNumber"></param>
        /// <param name="channelID"></param>
        public static void HKModifyWriter(string newRquestNumber, string channelID)
        {
            Monitor.Enter(frp);
            frp.HKModifyWriter(newRquestNumber, channelID);
            IsHKModifyImp = false;
            Monitor.Exit(frp);
        }
        /// <summary>
        /// 港股改单文件数据读取写入到数据库中
        /// </summary>
        public static void HKModifyReaderToDB()
        {
            Monitor.Enter(frp);
            if (!IsHKModifyImp)
            {
                frp.ReaderHKModifyPushDataToDB();
                IsHKModifyImp = true;
            }
            Monitor.Pulse(frp);
            Monitor.Exit(frp);
        }
    }
}
