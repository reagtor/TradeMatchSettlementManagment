using System;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Text;
using ReckoningCounter.Model;
using System.Collections.Generic;

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 数据访问类 XH_PushBackOrderTableDal
    /// </summary>
    public class XH_PushBackOrderTableDal
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string tradeNumber, string channelId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Add(tradeNumber, channelId, db);
        }
        /// <summary>
        /// 根据数据操作句柄增加一条数据
        /// Create By:李健华
        /// Create Date:2009-8-01
        /// Desc.:为了批增加数据不用每次创建数据操作句柄
        /// </summary>
        public void Add(string tradeNumber, string channelId, Database database)
        {
            Database db = database;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_PushBackOrderTable(");
            strSql.Append("TradeNumber,ChannelID");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + tradeNumber + "',");
            strSql.Append("'" + channelId + "'");
            strSql.Append(")");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(string tradeNumber, string channelId, Database database, DbTransaction transaction)
        {
            Database db = database;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XH_PushBackOrderTable(");
            strSql.Append("TradeNumber,ChannelID");
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append("'" + tradeNumber + "',");
            strSql.Append("'" + channelId + "'");
            strSql.Append(")");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);

        }
        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <returns></returns>
        public bool JudgeConnectionState()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("数据库连接异常", ex);
                return false;
            }
            return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string tradeNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Delete(tradeNumber, db);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string tradeNumber, Database database)
        {
            Database db = database;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_PushBackOrderTable ");
            strSql.Append(" where TradeNumber ='" + tradeNumber + "'");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="tradeNumber"></param>
        /// <param name="database"></param>
        /// <param name="transaction"></param>
        public void Delete(string tradeNumber, Database database, DbTransaction transaction)
        {
            Database db = database;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete XH_PushBackOrderTable ");
            strSql.Append(" where TradeNumber ='" + tradeNumber + "'");
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);
        }


        #region Create By:李健华Crate Date:2009-08-01

        #region 根据交易编号更新通道号
        /// <summary>
        /// 根据交易编号更新通道号
        /// </summary>
        /// <param name="tradeNumber">要更新的交易编号</param>
        /// <param name="channelID">要更新的通道号</param>
        /// <param name="database"></param>
        /// <param name="transaction"></param>
        public void Update(string tradeNumber, string channelID, Database database, DbTransaction transaction)
        {
            Database db = database;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE [dbo].[XH_PushBackOrderTable]   SET [ChannelID] ='{0}'", channelID);
            strSql.AppendFormat(" where TradeNumber in ( {0} )", tradeNumber);
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            db.ExecuteNonQuery(dbCommand, transaction);
        }
        #endregion

        #region 根据通道ID删除已经不是今日交易的回推数据，即不再回推
        /// <summary>
        /// 根据通道ID删除已经不是今日交易的回推数据，即不再回推,并且返回还要回推的总数
        /// Create BY:李健华
        /// Create Date:2009-008-01
        /// Desc.:为了加推数据故障恢复或者数据维护时增加对回推数据的删除
        /// </summary>
        /// <param name="channelID">通道ID</param>
        /// <returns>返回删除后是否还有该通道是否还有要回推数据</returns>
        public int DeleteNotPushToday(string channelID)
        {
            string startTime = DateTime.Now.ToShortDateString();
            string endTime = DateTime.Now.AddDays(1).ToShortDateString();
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  delete XH_PushBackOrderTable where TradeNumber not in( ");
            strSql.AppendFormat(" select TradeNumber from dbo.XH_HistoryTradeTable where tradetime>='{0}' and TradeTime<'{1}'", startTime, endTime);
            strSql.Append("  union all");
            strSql.AppendFormat("  select TradeNumber from dbo.XH_TodayTradeTable where tradetime>='{0}' and TradeTime<'{1}'", startTime, endTime);
            strSql.AppendFormat("   ) and ChannelID='{0}' ", channelID.Trim());
            strSql.AppendFormat(" select count(1) from  dbo.XH_PushBackOrderTable where  ChannelID='{0}'", channelID.Trim());
            DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
            int cmdresult = 0;
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            return cmdresult;

        }
        #endregion

        #region 私有方法根据通道ID返回唯一的委托单号SQLScript
        /// <summary>
        /// 私有方法,在加推数据中根据通道ID返回唯一的委托单号SQLScript 
        /// </summary>
        /// <returns></returns>
        private string GetDistincEntrustNumberSqlScript()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select distinct EntrustNumber  from ( ");
            strSql.Append("select EntrustNumber from XH_TodayTradeTable where tradenumber in(select tradenumber from XH_PushBackOrderTable where channelid='{0}')");
            strSql.Append(" union all ");
            strSql.Append(" select EntrustNumber from XH_HistoryTradeTable where tradenumber in(select tradenumber from XH_PushBackOrderTable where channelid='{0}')");
            strSql.Append(" )  as t ");
            return strSql.ToString();
        }
        #endregion

        #region 私有方法根据委托单号，从今日委托和历史委托中查询相关的委托单数据
        /// <summary>
        /// 根据委托单号，从今日委托和历史委托中查询相关的委托单数据
        /// 这里如果在历史委托表中的数据因为没有可撤标识，这里为了能转换回今日委托，所以直接转为不可撤0
        /// 通道号附为空字符串""
        /// </summary>
        /// <returns></returns>
        private string GetEntrustListByEnturstNoSqlScript()
        {
            StringBuilder sqlStr = new StringBuilder("");
            sqlStr.Append(" select [EntrustNumber],[PortfolioLogo],[EntrustPrice],[EntrustAmount],[SpotCode],[TradeAmount],[TradeAveragePrice]");
            sqlStr.Append(" ,[CancelAmount],[CancelLogo],[BuySellTypeId],[StockAccount],[CapitalAccount],[OrderStatusId]");
            sqlStr.Append(" ,[IsMarketValue],[OrderMessage],[CurrencyTypeId],[TradeUnitId],[CallbackChannlId]");
            sqlStr.Append(" ,[McOrderId],[HasDoneProfit],[OfferTime],[EntrustTime] from dbo.XH_TodayEntrustTable");
            sqlStr.Append(" where EntrustNumber in({0})");
            sqlStr.Append(" union all");
            sqlStr.Append(" select [EntrustNumber],[PortfolioLogo],[EntrustPrice],[Entrustmount] as [EntrustAmount],[SpotCode],[TradeAmount],[TradeAveragePrice]");
            sqlStr.Append(" ,[CancelAmount],[CancelLogo]=cast(0 as bit),[BuySellTypeId],[StockAccount],[CapitalAccount],[OrderStatusId]");
            sqlStr.Append(" ,[IsMarketValue],[OrderMessage],[CurrencyTypeId],[TradeUnitId],[CallbackChannlId]=''");
            sqlStr.Append(" ,[McOrderId],[HasDoneProfit],[OfferTime],[EntrustTime] from dbo.XH_HistoryEntrustTable");
            sqlStr.Append(" where EntrustNumber in({0})");
            return sqlStr.ToString();
        }
        #endregion

        #region 根据通道号返回唯一的委托单号,返回以逗号分隔（','）的字条串如：'234234523','2345235232'
        /// <summary>
        /// 根据通道号返回唯一的委托单号,返回以逗号分隔（','）的字条串如：'234234523','2345235232'
        /// Create BY:李健华
        /// Create Date:2009-008-01
        /// Desc.:根据通道号返回唯一的委托单号 即本通道所要回推的所有委托单号,这里因为清算把当日的数据
        ///       移动了历史表，所以还得联合查询到历史表中去获取相关的数据,这里获取到的数据不管是否为当日还是历史，只管通道
        ///       如果要获取保证是当日的数据，所以要先调用以上方法DeleteNotPushToday()删除非当日数据
        /// </summary>
        /// <param name="channelID">通道号</param>
        /// <returns></returns>
        public string GetDistinctEntrustNumberByChannelID(string channelID)
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(string.Format(GetDistincEntrustNumberSqlScript().ToString(), channelID.Trim()));
            StringBuilder sb = new StringBuilder("");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    sb.AppendFormat(",  '{0}'", dataReader["EntrustNumber"].ToString());
                }
            }
            string numberStr = sb.ToString();
            if (!string.IsNullOrEmpty(numberStr))
            {
                numberStr = numberStr.Substring(numberStr.IndexOf(',') + 1);
            }
            return numberStr;

        }
        #endregion

        #region 根据委托单号，从今日委托和历史委托中查询相关的委托单数据
        /// <summary>
        /// 根据委托单号，从今日委托和历史委托中查询相关的委托单数据
        /// 这里如果在历史委托表中的数据因为没有可撤标识，这里为了能转换回今日委托，所以直接转为不可撤0
        /// 通道号附为空字符串""
        /// </summary>
        /// <param name="entrustStr">委托单号</param>
        /// <returns></returns>
        public List<XH_TodayEntrustTableInfo> GetEntrustListByEnturstNo(string entrustStr)
        {

            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            CommonDALOperate<XH_TodayEntrustTableInfo> com = new CommonDALOperate<XH_TodayEntrustTableInfo>();
            return com.ExecuterReaderDataBind(string.Format(GetEntrustListByEnturstNoSqlScript(), entrustStr.ToString()), dal.ReaderBind);

        }
        #endregion

        #region 根据通道号，从今日委托和历史委托中查询相关的委托单数据
        /// <summary>
        /// 根据通道号返回要回道的所有委托数据，这里包含历史委托的数据
        /// </summary>
        /// <param name="channleID">通道号</param>
        /// <returns></returns>
        public List<XH_TodayEntrustTableInfo> GetEntrustListByChannleID(string channleID)
        {

            string sql = string.Format(GetDistincEntrustNumberSqlScript(), channleID);
            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            CommonDALOperate<XH_TodayEntrustTableInfo> com = new CommonDALOperate<XH_TodayEntrustTableInfo>();
            return com.ExecuterReaderDataBind(string.Format(GetEntrustListByEnturstNoSqlScript(), sql), dal.ReaderBind);

        }
        #endregion

        #region 根据通道号返回当日成交数据，这里包括清算后移动到历史表中还是当日的数据
        /// <summary>
        /// 根据通道号返回当日成交数据，这里包括清算后移动到历史表中还是当日的数据
        /// </summary>
        /// <param name="channleID">通道号</param>
        /// <returns></returns>
        public List<XH_TodayTradeTableInfo> GetTodayTradeListByChannleID(string channleID)
        {

            StringBuilder sb = new StringBuilder("");
            sb.Append(" select TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,TradeAmount,");
            sb.Append(" EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee, ");
            sb.Append(" MonitoringFee,TradingSystemUseFee,TradeCapitalAmount,ClearingFee,StockAccount, ");
            sb.Append(" CapitalAccount,SpotCode,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ");
            sb.Append("  from XH_TodayTradeTable ");
            sb.AppendFormat(" where  TradeNumber in(select TradeNumber from dbo.XH_PushBackOrderTable where channelID='{0}')", channleID);
            sb.Append(" union  all");
            sb.Append(" select TradeNumber,EntrustNumber,PortfolioLogo,TradePrice,TradeAmount,");
            sb.Append(" EntrustPrice,StampTax,Commission,TransferAccountFee,TradeProceduresFee, ");
            sb.Append(" MonitoringFee,TradingSystemUseFee,TradeCapitalAmount=0.00,ClearingFee,StockAccount, ");
            sb.Append(" CapitalAccount,SpotCode,TradeTypeId,TradeUnitId,BuySellTypeId,CurrencyTypeId,TradeTime ");
            sb.Append(" from XH_HistoryTradeTable ");
            sb.AppendFormat(" where  TradeNumber in(select TradeNumber from dbo.XH_PushBackOrderTable where channelID='{0}')", channleID);
            XH_TodayTradeTableDal dal = new XH_TodayTradeTableDal();
            CommonDALOperate<XH_TodayTradeTableInfo> com = new CommonDALOperate<XH_TodayTradeTableInfo>();
            return com.ExecuterReaderDataBind(sb.ToString(), dal.ReaderBind);
        }
        #endregion

        #region  根据委托单列表返回所有成交编号列表以'322332',233232'形式返回
        /// <summary>
        /// 根据委托单列表返回所有成交编号列表以'322332',233232'形式返回
        /// 这里会在今日和历史中查询
        /// </summary>
        /// <param name="entrustNumberList">委托单编号列表以'3333','kkk'方式传入</param>
        /// <returns></returns>
        private string GetTodayAndHistroyTradeNumberString(string entrustNumberList)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(" select TradeNumber ");
            sb.Append("  from XH_TodayTradeTable ");
            sb.AppendFormat(" where  EntrustNumber in( {0} )", entrustNumberList);
            sb.Append(" union  all");
            sb.Append(" select TradeNumber ");
            sb.Append(" from XH_HistoryTradeTable ");
            sb.AppendFormat(" where  EntrustNumber in( {0} )", entrustNumberList);
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sb.ToString());
            StringBuilder rtsb = new StringBuilder("");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    rtsb.AppendFormat(",  '{0}'", dataReader["TradeNumber"].ToString());
                }
            }
            string numberStr = rtsb.ToString();
            if (!string.IsNullOrEmpty(numberStr))
            {
                numberStr = numberStr.Substring(numberStr.IndexOf(',') + 1);
            }
            return numberStr;
        }
        #endregion

        #region 根据委托单号列表更新回推通道
        /// <summary>
        /// Title:根据要更新的委托单列表更新回推的通道
        /// Desc.:本方法逻辑，首先这里要更新现货今日委托表中的通道
        ///       更新之后再根据委托单列表在今日成交和历史成交表获取对应的交易单编号更新回推故障中的通道.
        /// </summary>
        /// <param name="numberList">要更新的委托单列表</param>
        /// <param name="newClientId">列新的通道号</param>
        public void UpdateEntrustPushBackChannelID(List<string> numberList, string newClientId)
        {
            StringBuilder sb = new StringBuilder("");
            foreach (var item in numberList)
            {
                if (item != null && !string.IsNullOrEmpty(item.Trim()))
                {
                    sb.AppendFormat(",  '{0}'", item);
                }
            }
            string updateStr = sb.ToString();
            if (!string.IsNullOrEmpty(updateStr))
            {
                updateStr = updateStr.Substring(updateStr.IndexOf(',') + 1);
            }
            if (string.IsNullOrEmpty(updateStr.Trim()))
            {
                return;
            }
            //根据委托单列表获取今日成交的成交编号并在历史表中获取，
            //因为会在清算时会把当日的移到了历史表中
            string tradeNumberList = GetTodayAndHistroyTradeNumberString(updateStr);

            XH_TodayEntrustTableDal dal = new XH_TodayEntrustTableDal();
            DataManager.ExecuteInTransaction((db, trans) =>
                                                   {
                                                       //更新完今日委托表中的通道号
                                                       dal.UpdateChannel(updateStr, newClientId, db, trans);
                                                       //更新回推数据表中的通道
                                                       Update(tradeNumberList, newClientId, db, trans);
                                                   });
        }
        #endregion

        #endregion
    }
}
