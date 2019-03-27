using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.Model;

namespace ReckoningCounter.DAL.Data
{
    /// <summary>
    /// 分红相关数据访问类
    /// </summary>
    public class BD_RescueTableDal
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BD_RescueTableDal()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			Database db = DatabaseFactory.CreateDatabase();
			StringBuilder strSql = new StringBuilder();
			strSql.Append("select count(1) from BD_RescueTable where id=@id ");
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "id", DbType.Int32,id);
			int cmdresult;
			object obj = db.ExecuteScalar(dbCommand);
			if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
			{
				cmdresult = 0;
			}
			else
			{
				cmdresult = int.Parse(obj.ToString());
			}
			if (cmdresult == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BD_RescueTableInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BD_RescueTable(");
			strSql.Append("type,Value1,Value2,Value3,Value4,Value5)");

			strSql.Append(" values (");
			strSql.Append("@type,@Value1,@Value2,@Value3,@Value4,@Value5)");
			strSql.Append(";select @@IDENTITY");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "type", DbType.Int32, model.Type);
			db.AddInParameter(dbCommand, "Value1", DbType.String, model.Value1);
			db.AddInParameter(dbCommand, "Value2", DbType.String, model.Value2);
			db.AddInParameter(dbCommand, "Value3", DbType.String, model.Value3);
			db.AddInParameter(dbCommand, "Value4", DbType.String, model.Value4);
			db.AddInParameter(dbCommand, "Value5", DbType.String, model.Value5);
			int result;
			object obj = db.ExecuteScalar(dbCommand);
			if(!int.TryParse(obj.ToString(),out result))
			{
				return 0;
			}
			return result;
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
        public void Update(BD_RescueTableInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BD_RescueTable set ");
			strSql.Append("type=@type,");
			strSql.Append("Value1=@Value1,");
			strSql.Append("Value2=@Value2,");
			strSql.Append("Value3=@Value3,");
			strSql.Append("Value4=@Value4,");
			strSql.Append("Value5=@Value5");
			strSql.Append(" where id=@id ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "id", DbType.Int32, model.Id);
			db.AddInParameter(dbCommand, "type", DbType.Int32, model.Type);
			db.AddInParameter(dbCommand, "Value1", DbType.String, model.Value1);
			db.AddInParameter(dbCommand, "Value2", DbType.String, model.Value2);
			db.AddInParameter(dbCommand, "Value3", DbType.String, model.Value3);
			db.AddInParameter(dbCommand, "Value4", DbType.String, model.Value4);
			db.AddInParameter(dbCommand, "Value5", DbType.String, model.Value5);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BD_RescueTable ");
			strSql.Append(" where id=@id ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "id", DbType.Int32,id);
			db.ExecuteNonQuery(dbCommand);

		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BD_RescueTableInfo GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,type,Value1,Value2,Value3,Value4,Value5 from BD_RescueTable ");
			strSql.Append(" where id=@id ");
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand dbCommand = db.GetSqlStringCommand(strSql.ToString());
			db.AddInParameter(dbCommand, "id", DbType.Int32,id);
            BD_RescueTableInfo model = null;
			using (IDataReader dataReader = db.ExecuteReader(dbCommand))
			{
				if(dataReader.Read())
				{
					model=ReaderBind(dataReader);
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
        public List<BD_RescueTableInfo> GetListArray(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,type,Value1,Value2,Value3,Value4,Value5 ");
			strSql.Append(" FROM BD_RescueTable ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            List<BD_RescueTableInfo> list = new List<BD_RescueTableInfo>();
			Database db = DatabaseFactory.CreateDatabase();
			using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))
			{
				while (dataReader.Read())
				{
					list.Add(ReaderBind(dataReader));
				}
			}
			return list;
		}


		/// <summary>
		/// 对象实体绑定数据
		/// </summary>
        public BD_RescueTableInfo ReaderBind(IDataReader dataReader)
		{
            BD_RescueTableInfo model = new BD_RescueTableInfo();
			object ojb; 
			ojb = dataReader["id"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.Id=(int)ojb;
			}
			ojb = dataReader["type"];
			if(ojb != null && ojb != DBNull.Value)
			{
				model.Type=(int)ojb;
			}
			model.Value1=dataReader["Value1"].ToString();
			model.Value2=dataReader["Value2"].ToString();
			model.Value3=dataReader["Value3"].ToString();
			model.Value4=dataReader["Value4"].ToString();
			model.Value5=dataReader["Value5"].ToString();
			return model;
		}

		#endregion  成员方法
    }
}
