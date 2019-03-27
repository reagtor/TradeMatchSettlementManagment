using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;

namespace ReckoningCounter.Entity
{
    /// <summary>
    /// 持仓限制实体类（因为商品期货有需求所以而不修改之前的方法调用，所以这里创建些类)
    /// Create by:李健华
    /// Create Date:2010-01-28
    /// </summary>
    public class PositionLimitValueInfo
    {
        /// <summary>
        /// 持仓限制量
        /// </summary>
        public decimal PositionValue { get; set; }

        /// <summary>
        /// 是否判断最小倍数
        /// </summary>
        public bool IsMinMultiple { get; set; }

        /// <summary>
        /// 最小倍率值
        /// </summary>
        public decimal MinMultipleValue { get; set; }

        /// <summary>
        /// 当持仓量限制为-1时，是否忽略不计算
        /// </summary>
        public bool IsNoComputer { get; set; }

        /// <summary>
        /// 持仓和保证金控制类型
        /// </summary>
        public Types.QHPositionBailType QHPositionBailType { get; set; }
    }
}
