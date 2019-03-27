using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using GTA.VTS.CustomersOrders.Resources;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// 资源文件操作类
    /// Create by:李健华
    /// Create date:2009-12-30
    /// </summary>
    public class ResourceOperate
    {
        private ResourceManager rm;

        #region 初始化类单一进入模式
        /// <summary>
        /// 撮合中心对象
        /// </summary>
        private static ResourceOperate instanse;
        /// <summary>
        /// 初始化撮合中心类单一进入模式
        /// </summary>
        public static ResourceOperate Instanse
        {
            get
            {
                //撮合中心实体不能为空
                if (instanse == null)
                {
                    instanse = new ResourceOperate();
                }
                return instanse;
            }
        }
        #endregion

        /// <summary>
        /// 初始化获取资源文件的内容
        /// </summary>
        /// <param name="type">1-中文，2-英文</param>
        public void InitResourceLocal(string type)
        {
            string local = "GTA.VTS.CustomersOrders.Resources.Resource_EN";
            switch (type)
            {
                case "zh-CN":
                    local = "GTA.VTS.CustomersOrders.Resources.Resource_ZH_CN";
                    break;
                case "EN":
                    local = "GTA.VTS.CustomersOrders.Resources.Resource_EN";
                    break;
                default:
                    break;
            }
            rm = new ResourceManager(local, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 根据资源文件中的Key返回value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetResourceByKey(string key)
        {
            string value = "";
            value = rm.GetString(key);
            return value;
        }
    }
}
