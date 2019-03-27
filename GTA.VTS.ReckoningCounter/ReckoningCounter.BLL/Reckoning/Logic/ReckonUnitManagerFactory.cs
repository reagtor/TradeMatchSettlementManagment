namespace ReckoningCounter.BLL.Reckoning.Logic
{
    /// <summary>
    /// 清算单元管理器工厂
    /// 作者：宋涛
    /// </summary>
    public class ReckonUnitManagerFactory
    {

        /// <summary>
        /// 获取现货业务处理器
        /// </summary>
        /// <returns></returns>
        public static XHReckonUnitManager GetXHReckonUnitManager()
        {
            return XHReckonUnitManager.GetInstance();
        }

        /// <summary>
        /// 获取港股业务处理器
        /// </summary>
        /// <returns></returns>
        public static HKReckonUnitManager GetHKReckonUnitManager()
        {
            return HKReckonUnitManager.GetInstance();
        }

        /// <summary>
        /// 获取股指期货业务处理器
        /// </summary>
        /// <returns></returns>
        public static GZQHReckonUnitManager GetGZQHReckonUnitManager()
        {
            return GZQHReckonUnitManager.GetInstance();
        }

        /// <summary>
        /// 获取商品期货业务处理器
        /// </summary>
        /// <returns></returns>
        public static SPQHReckonUnitManager GetSPQHReckonUnitManager()
        {
            return SPQHReckonUnitManager.GetInstance();
        }
    }
}