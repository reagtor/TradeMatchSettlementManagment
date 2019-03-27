#region Using Namespace

using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.MemoryData.HK.Capital;
using ReckoningCounter.MemoryData.HK.Hold;
using ReckoningCounter.MemoryData.QH.Capital;
using ReckoningCounter.MemoryData.QH.Hold;
using ReckoningCounter.MemoryData.XH.Capital;
using ReckoningCounter.MemoryData.XH.Hold;

#endregion

namespace ReckoningCounter.MemoryData
{
    /// <summary>
    /// 所有的内存列表管理器
    /// 作者：宋涛
    /// </summary>
    public static class MemoryDataManager
    {
        //现货内存表管理器

        /// <summary>
        /// 港股资金内存列表
        /// </summary>
        public static HKCapitalMemoryTableList HKCapitalMemoryList;
        /// <summary>
        /// 港股持仓内存列表
        /// </summary>
        public static HKHoldMemoryTableList HKHoldMemoryList;

        /// <summary>
        /// 期货资金内存列表
        /// </summary>
        public static QHCapitalMemoryTableList QHCapitalMemoryList;
        /// <summary>
        /// 期货持仓内存列表
        /// </summary>
        public static QHHoldMemoryTableList QHHoldMemoryList;

        /// <summary>
        /// 内存列表管理器状态
        /// </summary>
        private static MemoryDataManagerStatus status = MemoryDataManagerStatus.Close;

        /// <summary>
        /// 现货资金内存列表
        /// </summary>
        public static XHCapitalMemoryTableList XHCapitalMemoryList;
        /// <summary>
        /// 现货持仓内存列表
        /// </summary>
        public static XHHoldMemoryTableList XHHoldMemoryList;

        /// <summary>
        /// 内存列表管理器状态
        /// </summary>
        public static MemoryDataManagerStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// 启动内存表数据管理，只能在主程序启动时调用一次
        /// </summary>
        public static void Start()
        {
            //如果已经打开，那么直接退出
            if (status == MemoryDataManagerStatus.Open)
            {
                return;
            }

            LogHelper.WriteDebug("启动内存表数据管理……");
            bool isNormalExit; // = DaoUtil.IsNormalExitLastTime;
            //LogHelper.WriteDebug("上次是否正常退出：" + isNormalExit);

            //暂时不用，每次都同步数据库
            isNormalExit = false;

            XHCapitalMemoryDBPersister xhCapitalMemoryDBPersister = new XHCapitalMemoryDBPersister();
            XHCapitalMemoryList = new XHCapitalMemoryTableList(xhCapitalMemoryDBPersister);
            XHCapitalMemoryList.Initialize(isNormalExit);

            XHHoldMemoryDBPersister xhHoldMemoryDBPersister = new XHHoldMemoryDBPersister();
            XHHoldMemoryList = new XHHoldMemoryTableList(xhHoldMemoryDBPersister);
            XHHoldMemoryList.Initialize(isNormalExit);

            QHCapitalMemoryDBPersister qhCapitalMemoryDBPersister = new QHCapitalMemoryDBPersister();
            QHCapitalMemoryList = new QHCapitalMemoryTableList(qhCapitalMemoryDBPersister);
            QHCapitalMemoryList.Initialize(isNormalExit);

            QHHoldMemoryDBPersister qhHoldMemoryDBPersister = new QHHoldMemoryDBPersister();
            QHHoldMemoryList = new QHHoldMemoryTableList(qhHoldMemoryDBPersister);
            QHHoldMemoryList.Initialize(isNormalExit);

            HKCapitalMemoryDBPersister hkCapitalMemoryDBPersister = new HKCapitalMemoryDBPersister();
            HKCapitalMemoryList = new HKCapitalMemoryTableList(hkCapitalMemoryDBPersister);
            HKCapitalMemoryList.Initialize(isNormalExit);

            HKHoldMemoryDBPersister hkHoldMemoryDBPersister = new HKHoldMemoryDBPersister();
            HKHoldMemoryList = new HKHoldMemoryTableList(hkHoldMemoryDBPersister);
            HKHoldMemoryList.Initialize(isNormalExit);

            LogHelper.WriteDebug("内存表数据管理初始化完成！");
            status = MemoryDataManagerStatus.Open;
        }

        /// <summary>
        /// 结束内存表数据管理，只能在主程序结束时调用一次
        /// </summary>
        public static void End()
        {
            //如果已经关闭，那么直接退出
            if (status == MemoryDataManagerStatus.Close)
            {
                return;
            }

            XHCapitalMemoryList.Commit();
            XHHoldMemoryList.Commit();

            QHCapitalMemoryList.Commit();
            QHHoldMemoryList.Commit();

            HKCapitalMemoryList.Commit();
            HKHoldMemoryList.Commit();

            LogHelper.WriteDebug("结束内存表数据管理……");
            status = MemoryDataManagerStatus.Close;
        }
    }

    /// <summary>
    /// 内存表管理器状态
    /// </summary>
    public enum MemoryDataManagerStatus
    {
        /// <summary>
        /// 开启
        /// </summary>
        Open,
        /// <summary>
        /// 关闭
        /// </summary>
        Close
    }
}