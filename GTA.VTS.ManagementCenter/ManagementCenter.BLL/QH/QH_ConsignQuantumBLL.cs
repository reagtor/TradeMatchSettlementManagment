using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：交易规则委托量 业务逻辑类QH_ConsignQuantumBLL 的摘要说明。错误编码范围:6020-6039
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class QH_ConsignQuantumBLL
    {
        private readonly ManagementCenter.DAL.QH_ConsignQuantumDAL qH_ConsignQuantumDAL =
            new ManagementCenter.DAL.QH_ConsignQuantumDAL();

        public QH_ConsignQuantumBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_ConsignQuantumDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ConsignQuantumID)
        {
            return qH_ConsignQuantumDAL.Exists(ConsignQuantumID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public int Add(ManagementCenter.Model.QH_ConsignQuantum model)
        //{
        //    return qH_ConsignQuantumDAL.Add(model);
        //}
        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        //public void Update(ManagementCenter.Model.QH_ConsignQuantum model)
        //{
        //    qH_ConsignQuantumDAL.Update(model);
        //}
        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public void Delete(int ConsignQuantumID)
        //{
        //    qH_ConsignQuantumDAL.Delete(ConsignQuantumID);
        //}
        /// <summary>
        /// 添加交易规则和单笔最大委托量
        /// </summary>
        /// <param name="qHConsignQuantum">交易规则实体</param>
        /// <param name="qHSingleRequestQuantityl">单笔最大委托量实体</param>
        /// /// <param name="qHSingleRequestQuantity2">单笔最大委托量实体</param>
        /// <returns></returns>
        public int AddQHConsignQuantumAndSingle(QH_ConsignQuantum qHConsignQuantum,
                                                QH_SingleRequestQuantity qHSingleRequestQuantityl,
                                                QH_SingleRequestQuantity qHSingleRequestQuantity2)
        {
            QH_ConsignQuantumDAL qHConsignQuantumDAL = new QH_ConsignQuantumDAL();
            QH_SingleRequestQuantityDAL qHSingleRequestQuantityDAL = new QH_SingleRequestQuantityDAL();
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction Tran = Conn.BeginTransaction();

            int consignQuantumID = AppGlobalVariable.INIT_INT;
            try
            {
                consignQuantumID = qHConsignQuantumDAL.Add(qHConsignQuantum, Tran, db);

                if (consignQuantumID != AppGlobalVariable.INIT_INT)
                {
                    if (qHSingleRequestQuantityl != null)
                    {
                        qHSingleRequestQuantityl.ConsignQuantumID = consignQuantumID;
                        qHSingleRequestQuantityDAL.Add(qHSingleRequestQuantityl, Tran, db);
                    }

                    if (qHSingleRequestQuantity2 != null)
                    {
                        qHSingleRequestQuantity2.ConsignQuantumID = consignQuantumID;
                        qHSingleRequestQuantityDAL.Add(qHSingleRequestQuantity2, Tran, db);
                    }

                    Tran.Commit();
                }

                return consignQuantumID;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6020";
                string errMsg = "添加交易规则和单笔最大委托量失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return consignQuantumID;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #region  更新交易规则和单笔最大委托量

        /// <summary>
        /// 更新交易规则和单笔最大委托量
        /// </summary>
        /// <param name="qHConsignQuantum">交易规则实体</param>
        /// <param name="qHSingleRequestQuantity1">单笔最大委托量实体</param>
        /// <param name="qHSingleRequestQuantity2">单笔最大委托量实体</param>
        /// <returns></returns>
        public bool UpdateQHConsignQuantumAndSingle(QH_ConsignQuantum qHConsignQuantum,
                                                    QH_SingleRequestQuantity qHSingleRequestQuantity1,
                                                    QH_SingleRequestQuantity qHSingleRequestQuantity2)
        {
            QH_ConsignQuantumDAL qHConsignQuantumDAL = new QH_ConsignQuantumDAL();
            QH_SingleRequestQuantityDAL qHSingleRequestQuantityDAL = new QH_SingleRequestQuantityDAL();
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction Tran = Conn.BeginTransaction();
            try
            {
                qHConsignQuantumDAL.Update(qHConsignQuantum);
                if (qHSingleRequestQuantity1 != null)
                {
                    qHSingleRequestQuantityDAL.Update(qHSingleRequestQuantity1);
                }
                if (qHSingleRequestQuantity2 != null)
                {
                    qHSingleRequestQuantityDAL.Update(qHSingleRequestQuantity2);
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6021";
                string errMsg = "更新交易规则和单笔最大委托量失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn != null && Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        #region 根据交易规则委托量标识删除交易规则和单笔最大委托量

        /// <summary>
        /// 根据交易规则委托量标识删除交易规则和单笔最大委托量
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <param name="falg">标记是否使用了外部事务</param>
        /// <returns></returns>
        public bool DeleteQHConsignQuantumAndSingle(int ConsignQuantumID, DbTransaction tran, Database db, bool falg)
        {
            QH_ConsignQuantumDAL qHConsignQuantumDAL = new QH_ConsignQuantumDAL();
            QH_SingleRequestQuantityDAL qHSingleRequestQuantityDAL = new QH_SingleRequestQuantityDAL();
            DbConnection Conn = null;
            try
            {
                //创建本地事务
                if (db == null && tran == null)
                {
                    db = DatabaseFactory.CreateDatabase();
                    Conn = db.CreateConnection();
                    if (Conn.State != ConnectionState.Open)
                    {
                        Conn.Open();
                    }
                    tran = Conn.BeginTransaction();
                }
                qHSingleRequestQuantityDAL.DeleteSingleRQByConsignQuantumID(ConsignQuantumID, tran, db);
                qHConsignQuantumDAL.Delete(ConsignQuantumID, tran, db);
                if (!falg)
                {
                    tran.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (!falg)
                {
                    tran.Rollback();
                }
                string errCode = "GL-6022";
                string errMsg = "根据交易规则委托量标识删除交易规则和单笔最大委托量失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn != null && Conn.State == ConnectionState.Open)
                {
                    if (!falg) Conn.Close();
                }
            }
        }

        #endregion

        #region 根据交易规则委托量标识删除交易规则和单笔最大委托量

        /// <summary>
        /// 根据交易规则委托量标识删除交易规则和单笔最大委托量
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <returns></returns>
        public bool DeleteQHConsignQuantumAndSingle(int ConsignQuantumID)
        {
            try
            {
                return DeleteQHConsignQuantumAndSingle(ConsignQuantumID, null, null, false);

            }
            catch (Exception ex)
            {
                string errCode = "GL-6025";
                string errMsg = "根据交易规则委托量标识删除交易规则和单笔最大委托量失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据交易规则委托量ID，获取委托量实体

        /// <summary>
        /// 根据交易规则委托量ID，获取委托量实体
        /// </summary>
        /// <param name="ConsignQuantumID">交易规则委托量ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.QH_ConsignQuantum GetQHConsignQuantumModel(int ConsignQuantumID)
        {
            try
            {
                QH_ConsignQuantumDAL qHConsignQuantumDAL = new QH_ConsignQuantumDAL();
                return qHConsignQuantumDAL.GetModel(ConsignQuantumID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6023";
                string errMsg = "根据交易规则委托量ID，获取委托量实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_ConsignQuantum GetModelByCache(int ConsignQuantumID)
        {
            string CacheKey = "QH_ConsignQuantumModel-" + ConsignQuantumID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_ConsignQuantumDAL.GetModel(ConsignQuantumID);
                    if (objModel != null)
                    {
                        int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                                                      TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return (ManagementCenter.Model.QH_ConsignQuantum)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_ConsignQuantumDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的交易规则委托量（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_ConsignQuantum> GetListArray(string strWhere)
        {
            try
            {
                return qH_ConsignQuantumDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6024";
                string errMsg = "根据查询条件获取所有的交易规则委托量（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}