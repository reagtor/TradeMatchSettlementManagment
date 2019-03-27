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
    /// 描述：股指期货持仓限制 业务逻辑类QH_SIFPositionBLL 的摘要说明。错误编码范围:6660-6679
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_SIFPositionBLL
    {
        private readonly ManagementCenter.DAL.QH_SIFPositionDAL qH_SIFPositionDAL =
            new ManagementCenter.DAL.QH_SIFPositionDAL();

        public QH_SIFPositionBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_SIFPositionDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID)
        {
            return qH_SIFPositionDAL.Exists(BreedClassID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public void Add(ManagementCenter.Model.QH_SIFPosition model)
        //{
        //    qH_SIFPositionDAL.Add(model);
        //}

        #region 添加股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 添加股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="qHSIFPosition">股指期货持仓限制实体</param>
        /// <param name="qHSIFBail">品种_股指期货_保证金实体</param>
        /// <returns></returns>
        public bool AddQHSIFPositionAndQHSIFBail(ManagementCenter.Model.QH_SIFPosition qHSIFPosition,
                                                 ManagementCenter.Model.QH_SIFBail qHSIFBail)
        {
            QH_SIFBailDAL qHSIFBailDAL = new QH_SIFBailDAL();
            QH_SIFPositionDAL qHSIFPositionDAL = new QH_SIFPositionDAL();
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
                qHSIFPositionDAL.Add(qHSIFPosition, Tran, db);
                qHSIFBailDAL.Add(qHSIFBail, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6660";
                string errMsg = "添加股指期货持仓限制和品种_股指期货_保证金数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public void Update(ManagementCenter.Model.QH_SIFPosition model)
        //{
        //    qH_SIFPositionDAL.Update(model);
        //}

        #region  修改股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 修改股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="qHSIFPosition">股指期货持仓限制实体</param>
        /// <param name="qHSIFBail">品种_股指期货_保证金实体</param>
        /// <returns></returns>
        public bool UpdateQHSIFPositionAndQHSIFBail(ManagementCenter.Model.QH_SIFPosition qHSIFPosition,
                                                    ManagementCenter.Model.QH_SIFBail qHSIFBail)
        {
            QH_SIFBailDAL qHSIFBailDAL = new QH_SIFBailDAL();
            QH_SIFPositionDAL qHSIFPositionDAL = new QH_SIFPositionDAL();
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
                qHSIFPositionDAL.Update(qHSIFPosition, Tran, db);
                qHSIFBailDAL.Update(qHSIFBail, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6661";
                string errMsg = "修改股指期货持仓限制和品种_股指期货_保证金数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        /// <summary>
        /// 删除一条数据
        /// </summary>
        //public void Delete(int BreedClassID)
        //{
        //    qH_SIFPositionDAL.Delete(BreedClassID);
        //}

        #region 删除股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        /// 删除股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteQHSIFPositionAndQHSIFBail(int BreedClassID)
        {
            QH_SIFBailDAL qHSIFBailDAL = new QH_SIFBailDAL();
            QH_SIFPositionDAL qHSIFPositionDAL = new QH_SIFPositionDAL();
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
                qHSIFPositionDAL.Delete(BreedClassID, Tran, db);
                qHSIFBailDAL.Delete(BreedClassID, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6662";
                string errMsg = "删除股指期货持仓限制和品种_股指期货_保证金数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        #endregion

        #region  获取所有股指期货持仓限制和品种_股指期货_保证金数据

        /// <summary>
        ///  获取所有股指期货持仓限制和品种_股指期货_保证金数据
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllQHSIFPositionAndQHSIFBail(string BreedClassName, int pageNo, int pageSize,
                                                       out int rowCount)
        {
            try
            {
                QH_SIFPositionDAL qHSIFPositionDAL = new QH_SIFPositionDAL();
                return qHSIFPositionDAL.GetAllQHSIFPositionAndQHSIFBail(BreedClassName, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-6663";
                string errMsg = "获取所有股指期货持仓限制和品种_股指期货_保证金数据失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_SIFPosition GetModel(int BreedClassID)
        {
            return qH_SIFPositionDAL.GetModel(BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_SIFPosition GetModelByCache(int BreedClassID)
        {
            string CacheKey = "QH_SIFPositionModel-" + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_SIFPositionDAL.GetModel(BreedClassID);
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
            return (ManagementCenter.Model.QH_SIFPosition) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_SIFPositionDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }


        /// <summary>
        /// 根据查询条件获取所有的股指期货持仓限制（查询条件可为空）
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_SIFPosition> GetListArray(string strWhere)
        {
            try
            {
                return qH_SIFPositionDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6664";
                string errMsg = "根据查询条件获取所有的股指期货持仓限制（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法
    }
}