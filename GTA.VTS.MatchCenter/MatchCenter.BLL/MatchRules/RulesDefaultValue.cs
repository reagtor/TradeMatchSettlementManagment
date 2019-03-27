using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCenter.BLL
{
    /// <summary>
    /// 规则相关默认值
    /// Create BY：李健华
    /// Create Date：2009-08-18
    /// </summary>
    public class RulesDefaultValue
    {
        /// <summary>
        /// 默认小数点精确长度 2位
        /// </summary>
        public const int DefaultLength = 2;
        /// <summary>
        /// 默认值 1m
        /// </summary>
        public const decimal DefaultValue = 1m;
        /// <summary>
        /// 默认限制值0.05m
        /// </summary>
        public const decimal DefaultLimit = 0.05m;
        /// <summary>
        /// 默认熔断触发比例10.00m
        /// </summary>
        public const decimal DefaultFuseScale = 10.00m;
        /// <summary>
        /// 默认Timer执行间隔时间（100000毫秒==100秒）
        /// </summary>
        public const int DefaultInternal= 100000;
        /// <summary>
        /// 默认撤单不成功继续撤单次数(20次）
        /// </summary>
        public const int DefaultCancelFailCount = 20;
        /// <summary>
        /// 撮合中心中文窗体标题
        /// </summary>
        public const string title_ZH = "瑞尔格特撮合中心";
        /// <summary>
        /// 显示信息的默认列表数据
        /// </summary>
        public const int DefalultShowCountLists = 200;
    }
}
