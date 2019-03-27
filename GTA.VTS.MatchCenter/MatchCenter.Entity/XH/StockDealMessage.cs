namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
   public class StockDealMessage
    {
       private StockDealEntity stockEntity =null;

       private FutureDealBackEntity futureEtity=null;

       private string stockCode;

       /// <summary>
       /// 成交实体
       /// </summary>
       public StockDealEntity StockEntity
       {
           get
           {
               return stockEntity;
           }
           set
           {
               stockEntity = value;
           }
       }

       /// <summary>
       /// 期货成交回报
       /// </summary>
       public FutureDealBackEntity FutureEtity
       {
           get
           {
               return futureEtity;
           }
           set
           {
               futureEtity = value;
           }
       }

       /// <summary>
       /// 证卷代码
       /// </summary>
       public string StockCode
       {
           get
           {
               return stockCode;
           }
           set
           {
               stockCode = value;
           }
       }
    }
}
