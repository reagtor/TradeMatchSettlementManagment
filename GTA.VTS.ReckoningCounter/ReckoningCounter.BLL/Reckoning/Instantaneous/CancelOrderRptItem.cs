using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;
using ReckoningCounter.DAL.MatchCenterOrderDealRpt;

namespace ReckoningCounter.BLL
{

    /// <summary>
    /// 撤单异步回报处理元素
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class CancelOrderRptItem
    {
        /// <summary>
        /// 撤单异步回报元素
        /// </summary>
        public CancelOrderEntity RptItem { get; private set; }
        /// <summary>
        /// 品种类型
        /// </summary>
        public Types.BreedClassTypeEnum BreedClassType { get; private set; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="rptItem"></param>
        /// <param name="breedClassType"></param>
        public CancelOrderRptItem(CancelOrderEntity rptItem, Types.BreedClassTypeEnum breedClassType)
        {
            RptItem = rptItem;
            BreedClassType = breedClassType;
        }

    }
}
