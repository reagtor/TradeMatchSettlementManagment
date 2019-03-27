using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 价格涨跌幅实体类
    /// Create BY：李健华
    /// Create Date：2009-08-21
    /// </summary>
  public  class PriceEntity
    {
      private decimal lowerPrice = 0.0m;
      private decimal ceilingPrice = 0.0m;

      /// <summary>
      /// 下限价格
      /// </summary>
      public decimal LowerPrice
      {
          get
          {
              return lowerPrice;
          }
          set
          {
              lowerPrice = value;
          }
      }

      /// <summary>
      /// 上限价格
      /// </summary>
      public decimal CeilingPrice
      {
          get
          {
              return ceilingPrice;
          }
          set
          {
              ceilingPrice = value;
          }
      }
    }
}
