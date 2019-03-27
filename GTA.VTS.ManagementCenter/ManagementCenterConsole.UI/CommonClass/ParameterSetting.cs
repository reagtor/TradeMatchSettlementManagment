using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 参数设置类
    /// 作者：程序员;熊晓凌
    /// 日期：2008-11-26
    /// </summary>
    public class ParameterSetting
    {
        /// <summary>
        /// 每页数据的条数
        /// </summary>
        public static int m_pagesize = -1;
        public static int PageSize
        {
            get
            {
                if (m_pagesize == -1)
                {
                    int pagesize;
                    if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString(),
                                     out pagesize))
                    {
                        pagesize = 50; //20;
                    }
                    m_pagesize = pagesize;
                }
                return m_pagesize;
            }
        }

        /// <summary>
        /// 管理员实体
        /// </summary>
        public static ManagementCenter.Model.UM_UserInfo Mananger;
    }
}
