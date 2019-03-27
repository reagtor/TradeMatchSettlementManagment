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
    ///描述：交易规则_最小变动价位_范围_值 业务逻辑类XH_MinChangePriceValueBLL 的摘要说明。
    /// 错误编码范围:5320-5339
    ///作者：刘书伟
    ///日期：2008-12-22
    /// </summary>
    public class XH_MinChangePriceValueBLL
    {
        private readonly ManagementCenter.DAL.XH_MinChangePriceValueDAL xH_MinChangePriceValueDAL =
            new ManagementCenter.DAL.XH_MinChangePriceValueDAL();

        public XH_MinChangePriceValueBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_MinChangePriceValueDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassID, int FieldRangeID)
        {
            return xH_MinChangePriceValueDAL.Exists(BreedClassID, FieldRangeID);
        }
        #region 暂不用的公共方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public void Add(ManagementCenter.Model.XH_MinChangePriceValue model)
        //{
        //    xH_MinChangePriceValueDAL.Add(model);
        //}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public bool Update(ManagementCenter.Model.XH_MinChangePriceValue model)
        //{
        //    return xH_MinChangePriceValueDAL.Update(model);
        //}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_MinChangePriceValue GetModel(int BreedClassID, int FieldRangeID)
        {
            return xH_MinChangePriceValueDAL.GetModel(BreedClassID, FieldRangeID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_MinChangePriceValue GetModelByCache(int BreedClassID, int FieldRangeID)
        {
            string CacheKey = "XH_MinChangePriceValueModel-" + BreedClassID + FieldRangeID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_MinChangePriceValueDAL.GetModel(BreedClassID, FieldRangeID);
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
            return (ManagementCenter.Model.XH_MinChangePriceValue) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_MinChangePriceValueDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
#endregion

        #region 根据查询条件获取所有的交易规则_最小变动价位_范围_值（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的交易规则_最小变动价位_范围_值（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_MinChangePriceValue> GetListArray(string strWhere)
        {
            try
            {
                return xH_MinChangePriceValueDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5324";
                string errMsg = "根据查询条件获取所有的交易规则_最小变动价位_范围_值（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 添加现货最小变动价位范围值

        /// <summary>
        /// 添加现货最小变动价位范围值
        /// </summary>
        /// <param name="xH_MinChangePriceValue">最小变动价位实体</param>
        /// <param name="cM_FieldRange">字段范围实体</param>
        /// <returns></returns>
        public bool AddXHMinChangePriceValue(XH_MinChangePriceValue xH_MinChangePriceValue, CM_FieldRange cM_FieldRange)
        {
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_MinChangePriceValueDAL xHMinChangePriceValueDAL = new XH_MinChangePriceValueDAL();
            DbConnection Conn = null;
            Database db = DatabaseFactory.CreateDatabase();
            Conn = db.CreateConnection();
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            DbTransaction Tran = Conn.BeginTransaction();

            int fieldRangeID = AppGlobalVariable.INIT_INT;
            try
            {
                fieldRangeID = cMFieldRangeDAL.Add(cM_FieldRange, Tran, db);

                if (fieldRangeID != AppGlobalVariable.INIT_INT)
                {
                    xH_MinChangePriceValue.FieldRangeID = fieldRangeID;

                    xHMinChangePriceValueDAL.Add(xH_MinChangePriceValue, Tran, db);

                    Tran.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5320";
                string errMsg = "添加现货最小变动价位范围值失败!";
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

        #region 更新交易规则_最小变动价位_范围_值

        /// <summary>
        /// 更新交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="xHMinChangePriceValue">交易规则_最小变动价位_范围_值实体</param>
        /// <param name="cMFieldRange">字段范围值实体</param>
        /// <returns></returns>
        public bool UpdateMinChangePriceValue(CM_FieldRange cMFieldRange, XH_MinChangePriceValue xHMinChangePriceValue)
        {
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_MinChangePriceValueDAL xHMinChangePriceValueDAL = new XH_MinChangePriceValueDAL();
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
                cMFieldRangeDAL.Update(cMFieldRange, Tran, db);
                xHMinChangePriceValueDAL.Update(xHMinChangePriceValue, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5322";
                string errMsg = "更新交易规则_最小变动价位_范围_值失败!";
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

        #region 根据品种ID，字段范围ID，删除最小变动价位_范围_值

        /// <summary>
        /// 根据品种ID，字段范围ID，删除最小变动价位_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <returns></returns>
        public bool DeleteMinChangePriceValue(int BreedClassID, int FieldRangeID)
        {
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_MinChangePriceValueDAL xHMinChangePriceValueDAL = new XH_MinChangePriceValueDAL();
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
                xHMinChangePriceValueDAL.Delete(BreedClassID, FieldRangeID, Tran, db);
                cMFieldRangeDAL.Delete(FieldRangeID, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5321";
                string errMsg = "根据品种ID，字段范围ID，删除最小变动价位_范围_值失败!";
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

        #region 根据品种ID获取交易规则_最小变动价位_范围_值

        /// <summary>
        /// 根据品种ID获取交易规则_最小变动价位_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public DataSet GetMinChangePriceFieldRangeByBreedClassID(int BreedClassID)
        {
            try
            {
                XH_MinChangePriceValueDAL xHMinChangePriceValueDAL = new XH_MinChangePriceValueDAL();
                return xHMinChangePriceValueDAL.GetMinChangePriceFieldRangeByBreedClassID(BreedClassID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5323";
                string errMsg = "根据品种ID获取交易规则_最小变动价位_范围_值失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}