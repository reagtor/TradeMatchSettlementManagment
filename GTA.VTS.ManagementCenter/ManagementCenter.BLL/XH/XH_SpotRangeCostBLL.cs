using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;
using GTA.VTS.Common.CommonUtility;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：品种_现货_交易费用_成交额_交易手续费 业务逻辑类XH_SpotRangeCostBLL 的摘要说明。
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class XH_SpotRangeCostBLL
    {
        private readonly ManagementCenter.DAL.XH_SpotRangeCostDAL xH_SpotRangeCostDAL =
            new ManagementCenter.DAL.XH_SpotRangeCostDAL();

        public XH_SpotRangeCostBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_SpotRangeCostDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int FieldRangeID, int BreedClassID)
        {
            return xH_SpotRangeCostDAL.Exists(FieldRangeID, BreedClassID);
        }

        #region 添加现货交易费用交易手续费

        /// <summary>
        /// 添加现货交易费用交易手续费
        /// </summary>
        /// <param name="xH_SpotRangeCost">现货交易手续费实体</param>
        /// <param name="cM_FieldRange">字段范围实体</param>
        /// <returns></returns>
        public bool AddXHSpotRangeCost(XH_SpotRangeCost xH_SpotRangeCost, CM_FieldRange cM_FieldRange)
        {
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_SpotRangeCostDAL xHSpotRangeCostDAL = new XH_SpotRangeCostDAL();
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
                    //xH_SpotRangeCost.FieldRangeID = fieldRangeID;

                    xHSpotRangeCostDAL.Add(xH_SpotRangeCost, Tran, db);

                    Tran.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                LogHelper.WriteError(ex.Message, ex);
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

        #region 更新交易费用交易手续费_范围_值

        /// <summary>
        /// 更新交易费用交易手续费_范围_值
        /// </summary>
        /// <param name="xH_SpotRangeCost">交易手续费_范围_值实体类</param>
        /// <param name="cMFieldRange">字段范围值实体</param>
        /// <returns></returns>
        public bool UpdateSpotRangeCost(CM_FieldRange cMFieldRange, XH_SpotRangeCost xH_SpotRangeCost)
        {
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_SpotRangeCostDAL xHSpotRangeCostDAL = new XH_SpotRangeCostDAL();
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
                xHSpotRangeCostDAL.Update(xH_SpotRangeCost, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
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

        #region 根据品种ID，字段范围ID，删除交易手续费_范围_值

        /// <summary>
        /// 根据品种ID，字段范围ID，删除交易手续费_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <param name="FieldRangeID">字段范围ID</param>
        /// <returns></returns>
        public bool DeleteSpotRangeCost(int BreedClassID, int FieldRangeID)
        {
            CM_FieldRangeDAL cMFieldRangeDAL = new CM_FieldRangeDAL();
            XH_SpotRangeCostDAL xHSpotRangeCostDAL = new XH_SpotRangeCostDAL();
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
                xHSpotRangeCostDAL.Delete(BreedClassID, FieldRangeID, Tran, db);
                cMFieldRangeDAL.Delete(FieldRangeID, Tran, db);
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
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

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotRangeCost GetModel(int FieldRangeID, int BreedClassID)
        {
            return xH_SpotRangeCostDAL.GetModel(FieldRangeID, BreedClassID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_SpotRangeCost GetModelByCache(int FieldRangeID, int BreedClassID)
        {
            string CacheKey = "XH_SpotRangeCostModel-" + FieldRangeID + BreedClassID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_SpotRangeCostDAL.GetModel(FieldRangeID, BreedClassID);
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
            return (ManagementCenter.Model.XH_SpotRangeCost) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_SpotRangeCostDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的品种_现货_交易费用_成交额_交易手续费（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_SpotRangeCost> GetListArray(string strWhere)
        {
            return xH_SpotRangeCostDAL.GetListArray(strWhere);
        }

        #region 根据品种ID获取现货交易费用交易手续费_范围_值

        /// <summary>
        /// 根据品种ID获取现货交易费用交易手续费_范围_值
        /// </summary>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public DataSet GetXHSpotRangeCostByBreedClassID(int BreedClassID)
        {
            try
            {
                XH_SpotRangeCostDAL xHSpotRangeCostDAL = new XH_SpotRangeCostDAL();
                return xHSpotRangeCostDAL.GetXHSpotRangeCostByBreedClassID(BreedClassID);
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #endregion  成员方法
    }
}