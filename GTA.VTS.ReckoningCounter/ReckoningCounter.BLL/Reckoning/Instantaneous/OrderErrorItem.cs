using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

namespace ReckoningCounter.BLL
{

    /// <summary>
    /// 有价格问题的委托信息处理元素
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class OrderErrorItem
    {
        /// <summary>
        /// 撮合中心返回有价格问题的委托信息
        /// </summary>
        public ResultDataEntity ErrorItem { get; private set; }

        /// <summary>
        /// 品种类型
        /// </summary>
        public Types.BreedClassTypeEnum BreedClassType { get; private set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="errorItem"></param>
        /// <param name="breedClassType"></param>
        public OrderErrorItem(ResultDataEntity errorItem, Types.BreedClassTypeEnum breedClassType)
        {
            ErrorItem = errorItem;
            BreedClassType = breedClassType;
        }
    }
}
