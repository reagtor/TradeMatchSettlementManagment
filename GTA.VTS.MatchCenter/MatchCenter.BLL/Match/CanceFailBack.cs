using MatchCenter.Entity;

namespace MatchCenter.BLL.match
{
    /// <summary>
    /// 撮合中心撤单失败返回抽象类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// Desc.：添加商品期货相关的内容
    /// Update By: 董鹏
    /// Update Date:2010-01-22
    /// </summary>
    public abstract class CanceFailBack
    {
        /// <summary>
        /// 撮合机撤单实体
        /// </summary>
        private static CanceFailBack stockinstanse;
        /// <summary>
        /// 撮合机撤单实体
        /// </summary>
        private static CanceFailFutureBack futureInstanse;
        /// <summary>
        /// 撮合机
        /// </summary>
        private static CanceFailHKBack hkInstance;

        #region 商品期货撮合机撤单实体 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撮合机撤单实体
        /// </summary>
        private static CanceFailCommoditiesBack commoditiesInstanse;
        #endregion

        /// <summary>
        /// 现货撤单实体
        /// </summary>
        public static CanceFailBack Stockinstanse
        {
            get
            {
                //撮合中心实体不能为空
                if (stockinstanse == null)
                {
                    stockinstanse = new CanceFailStockBack();
                }
                return stockinstanse;
            }
        }

        /// <summary>
        /// 期货撤单实体
        /// </summary>
        public static CanceFailBack Futureinstanse
        {
            get
            {
                //撮合中心实体不能为空
                if (futureInstanse == null)
                {
                    futureInstanse = new CanceFailFutureBack();
                }
                return futureInstanse;
            }
        }

        #region 商品期货撤单实体 add by 董鹏 2010-01-22
        /// <summary>
        /// 商品期货撤单实体
        /// </summary>
        public static CanceFailBack CommoditiesInstanse
        {
            get
            {
                //撮合中心实体不能为空
                if (commoditiesInstanse == null)
                {
                    commoditiesInstanse = new CanceFailCommoditiesBack();
                }
                return commoditiesInstanse;
            }
        }
        #endregion

        /// <summary>
        /// 获取单一实例
        /// </summary>
        public static CanceFailHKBack HKInstance
        {
            get
            {
                if (hkInstance == null)
                {
                    hkInstance = new CanceFailHKBack();
                }
                return hkInstance;
            }

        }

        /// <summary>
        /// 添加撤单回报
        /// </summary>
        /// <param name="cancelEntity">撤单实体</param>
        public abstract void Add(CancelEntity cancelEntity);
    }
}
