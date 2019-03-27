using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using ReckoningCounter.Entity.Model.QueryFilter;

namespace ReckoningCounter.DAL.Data
{

    /// <summary>
    /// Title:公共DAL操作
    /// Desc.:  包括所有分页查询方法执行
    /// Demo: XH_HistoryEntrustTableDal xh = new XH_HistoryEntrustTableDal();
    ///       CommonDALOperate&lt;XH_HistoryEntrustTableInfo&gt;.DataReaderBind bind = xh.ReaderBind;
    ///       CommonDALOperate&lt;XH_HistoryEntrustTableInfo&gt; com = new CommonDALOperate&lt;XH_HistoryEntrustTableInfo&gt;();
    ///
    ///        return com.PagingQueryProcedures(ppInfo, out total, bind);
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonDALOperate<T> where T : class
    {
        /// <summary>
        /// 绑定数据委托事件
        /// </summary>
        /// <param name="dr">结果集流对象</param>
        /// <returns></returns>
        public delegate T DataReaderBind(IDataReader dr);

        #region 根据条件分页查询
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <param name="pageProcInfo">分页存储过程过滤条件</param>
        /// <param name="total">总页数</param>
        /// <param name="delegateDataBind">绑定数据委托事件</param>
        /// <returns></returns>
        public List<T> PagingQueryProcedures(PagingProceduresInfo pageProcInfo, out int total, DataReaderBind delegateDataBind)
        {
            List<T> list = new List<T>();
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_CommonPagingStoredProcedure");
            db.AddInParameter(dbCommand, "Tables", DbType.String, pageProcInfo.Tables);
            db.AddInParameter(dbCommand, "PK", DbType.String, pageProcInfo.PK);
            db.AddInParameter(dbCommand, "Sort", DbType.String, pageProcInfo.Sort);
            db.AddInParameter(dbCommand, "PageNumber", DbType.Int32, pageProcInfo.PageNumber);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageProcInfo.PageSize);
            db.AddInParameter(dbCommand, "Fields", DbType.String, pageProcInfo.Fields);
            db.AddInParameter(dbCommand, "Filter", DbType.String, pageProcInfo.Filter);
            db.AddInParameter(dbCommand, "isCount", DbType.Boolean, pageProcInfo.IsCount);
            db.AddOutParameter(dbCommand, "Total", DbType.Int32, 4);
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {

                    list.Add(delegateDataBind(dr));
                }
                dr.Close();
            }
            if (db.GetParameterValue(dbCommand, "@Total") != null && db.GetParameterValue(dbCommand, "@Total") != DBNull.Value)
            {
                total = (int)db.GetParameterValue(dbCommand, "@Total");
            }
            else
            {
                total = 0;
            }

            return list;
        }
        #endregion

        #region 执行行SQL语句委托绑定数据近回实体List
        /// <summary>
        /// 执行SQL语句委托绑定数据对象列表
        /// </summary>
        /// <param name="sqlStr">要执行的SQLScript</param>
        /// <param name="delegateDataBind">委托绑定数据方法</param>
        /// <returns>返回执行绑定数据后的数据列表</returns>
        public List<T> ExecuterReaderDataBind(string sqlStr, DataReaderBind delegateDataBind)
        {
            List<T> list = new List<T>();
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dr = db.ExecuteReader(CommandType.Text, sqlStr))
            {
                while (dr.Read())
                {
                    list.Add(delegateDataBind(dr));
                }
            }
            return list;
        }
        #endregion

    }
    /// <summary>
    /// 公开构建DAL操作SQLScript
    /// </summary>
    public class CommonDALBulidSQLScript
    {
        #region 构建查询时间段字符串
        /// <summary>
        /// Title:构建查询时间段条件语句
        /// Desc.:如果两者中有一个时间没有附值(或者开始时间大于结束时间),即以今天为结束时间向前查询N天
        /// 返回 AND ({0}>= '2009-05-07' AND  {0} &lt; '2009-05-15')
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="day">向前查询多少天</param>
        /// <returns>返回 AND ({0}>= '2009-05-07' AND  {0} &lt; '2009-05-15')</returns>
        public static string BuildWhereQueryBetwennTime(DateTime? startTime, DateTime? endTime, int day)
        {
            string start = DateTime.Today.AddDays(-day).ToShortDateString(); ;
            string end = DateTime.Today.AddDays(1).ToShortDateString();
            if (startTime != null && endTime != null && startTime != DateTime.MaxValue && endTime != DateTime.MaxValue && startTime != DateTime.MinValue && endTime != DateTime.MinValue)//起始和结束时间均已赋值
            {
                //起始时间小于等于结束时间（比如2008-05-06小于2008-05-28）
                if (DateTime.Compare(startTime.Value, endTime.Value) <= 0)
                {
                    start = startTime.Value.ToString("yyyy-MM-dd");
                    end = endTime.Value.AddDays(1).ToString("yyyy-MM-dd");
                }
                //超始时间大于结束时间则按默认当前前一个月时间查询
                //else if (DateTime.Compare(startTime.Value, endTime.Value) == 0)
                //{
                //    //起始时间等于结束时间
                //    start = startTime.Value.ToString("yyyy-MM-dd");
                //    end = endTime.Value.AddDays(1).ToString("yyyy-MM-dd");
                //}
            }

            return " AND ( {0}>= '" + start + "' AND  {0}<'" + end + "')";
        }
        #endregion
    }
}
