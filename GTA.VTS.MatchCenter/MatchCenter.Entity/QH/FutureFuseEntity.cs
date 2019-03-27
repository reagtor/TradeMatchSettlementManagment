

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心期货实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
  public  class FutureFuseEntity
    {
      private int fuseMark = 0;

      private FutureDataOrderEntity model;

      /// <summary>
      /// 熔断标志
      /// </summary>
      public int FuseMark
      {
          get
          {
              return fuseMark;
          }
          set
          {
              fuseMark = value;
          }
      }

      /// <summary>
      /// 期货委托
      /// </summary>
      public FutureDataOrderEntity Model
      {
          get
          {
              return model;
          }
          set
          {
              model = value;
          }
      }
    }
}
