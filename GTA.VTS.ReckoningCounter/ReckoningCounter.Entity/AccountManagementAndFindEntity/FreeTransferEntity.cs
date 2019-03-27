using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReckoningCounter.Entity.AccountManagementAndFindEntity
{
    /// <summary>
    /// Title:自由转账实体
    /// Desc.:自由转账实体
    /// Create by:李健华
    /// Create date:2009-11-2
    /// </summary>
    [DataContract]
    public class FreeTransferEntity
    {
        /// <summary>
        /// 交易员ID
        /// </summary>
        private string _TraderID = string.Empty;
        /// <summary>
        /// 交易员ID
        /// </summary>
        [DataMember]
        public string TraderID
        {
            get { return _TraderID; }
            set { _TraderID = value; }
        }


        /// <summary>
        /// 转出资金账户(from)
        /// </summary>
        private string _FromCapitalAccount = string.Empty;
        /// <summary>
        /// 转出资金账户
        /// </summary>
        [DataMember]
        public string FromCapitalAccount
        {
            get { return _FromCapitalAccount; }
            set { _FromCapitalAccount = value; }
        }

        /// <summary>
        /// 转出资金账户类型（注：1-银行帐号,2-证券资金帐户,3-证券持仓帐户,4-商品期货资金帐户,5-商品期货持仓帐户,6-股指期货资金帐号,7-股指期货持仓帐户,8-港股资金帐户,9-港股持仓帐户）
        /// </summary>
        private int _FromCapitalAccountType =-100;
        /// <summary>
        /// 转出资金账户类型（注：1-银行帐号,2-证券资金帐户,3-证券持仓帐户,4-商品期货资金帐户,5-商品期货持仓帐户,6-股指期货资金帐号,7-股指期货持仓帐户,8-港股资金帐户,9-港股持仓帐户）
        /// </summary>
        [DataMember]
        public int FromCapitalAccountType
        {
            get { return _FromCapitalAccountType; }
            set { _FromCapitalAccountType = value; }
        }

        /// <summary>
        /// 转入资金账户(to)
        /// </summary>
        private string _ToCapitalAccount = string.Empty;
        /// <summary>
        /// 转入资金账户
        /// </summary>
        [DataMember]
        public string ToCapitalAccount
        {
            get { return _ToCapitalAccount; }
            set { _ToCapitalAccount = value; }
        }

        /// <summary>
        /// 转入资金账户类型（注：1-银行帐号,2-证券资金帐户,3-证券持仓帐户,4-商品期货资金帐户,5-商品期货持仓帐户,6-股指期货资金帐号,7-股指期货持仓帐户,8-港股资金帐户,9-港股持仓帐户）
        /// </summary>
        private int _ToCapitalAccountType=-100;
        /// <summary>
        ///  转入资金账户类型（注：1-银行帐号,2-证券资金帐户,3-证券持仓帐户,4-商品期货资金帐户,5-商品期货持仓帐户,6-股指期货资金帐号,7-股指期货持仓帐户,8-港股资金帐户,9-港股持仓帐户）
        /// </summary>
        [DataMember]
        public int ToCapitalAccountType
        {
            get { return _ToCapitalAccountType; }
            set { _ToCapitalAccountType = value; }
        }
       

        /// <summary>
        /// 转账数量
        /// </summary>
        private decimal  _TransferAmount = -100;
        /// <summary>
        /// 转账数量
        /// </summary>
        [DataMember]
        public decimal TransferAmount
        {
            get { return _TransferAmount; }
            set { _TransferAmount = value; }
        }
    }
}