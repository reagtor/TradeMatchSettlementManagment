using System;
using System.Runtime.Serialization;

namespace MatchCenter.Entity
{
    /// <summary>
    /// 撮合中心撤单回报实体类
    /// Create BY：李健华
    /// Create Date：2009-08-19
    /// </summary>
   [DataContract]
   public class CancelResultEntity
    {
       private string orderNo = string.Empty;

       private bool isSuccess;

        private string message;

       /// <summary>
       /// 委托单号码
       /// </summary>
       [DataMember]
       public string OrderNo
       {
           get
           {
               return orderNo;
           }
           set
           {
               orderNo = value;
           }
       }

       /// <summary>
       /// 撤单是否成功
       /// </summary>
       [DataMember]
       public bool IsSuccess
       {
           get
           {
               return isSuccess;
           }
           set
           {
               isSuccess = value;
           }
       }

        /// <summary>
        /// 消息
        /// </summary>
       [DataMember]
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}
