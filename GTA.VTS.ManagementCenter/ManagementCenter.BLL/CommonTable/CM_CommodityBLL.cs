using System;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using System.Collections.Generic;
using ManagementCenter.Model.CommonClass;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述:交易商品 业务逻辑类CM_CommodityBLL 的摘要说明。  错误编码范围:4300-4319
    ///作者：刘书伟
    ///日期:2008-11-21 修改:2009-08-13
    /// </summary>
    public class CM_CommodityBLL
    {
        private readonly ManagementCenter.DAL.CM_CommodityDAL cM_CommodityDAL =
            new ManagementCenter.DAL.CM_CommodityDAL();

        public CM_CommodityBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string CommodityCode)
        {
            return cM_CommodityDAL.Exists(CommodityCode);
        }

        #region 添加交易商品

        /// <summary>
        /// 添加交易商品
        /// </summary>
        /// <param name="model">交易商品实体</param>
        /// <returns></returns>
        public bool AddCMCommodity(ManagementCenter.Model.CM_Commodity model)
        {
            try
            {
                return cM_CommodityDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4300";
                string errMsg = "添加交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 更新交易商品

        /// <summary>
        /// 更新交易商品
        /// </summary>
        /// <param name="model">交易商品实体</param>
        /// <returns></returns>
        public bool UpdateCMCommodity(ManagementCenter.Model.CM_Commodity model)
        {
            try
            {
                return cM_CommodityDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4302";
                string errMsg = "更新交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据商品代码删除交易商品（相关表的记录同时删除）

        /// <summary>
        /// 根据商品代码删除交易商品（相关表的记录同时删除）
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool DeleteCMCommodity(string CommodityCode, int BreedClassID)
        {
            CM_CommodityDAL cMCommodityDAL = new CM_CommodityDAL();
            RC_TradeCommodityAssignDAL TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();
            CM_CommodityFuseDAL cMCommodityFuseDAL = new CM_CommodityFuseDAL();
            CM_FuseTimesectionDAL cMFuseTimesectionDAL = new CM_FuseTimesectionDAL();
            CM_BreedClassBLL cM_BreedClassBLL = new CM_BreedClassBLL();
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
                int breedClassTypeID = AppGlobalVariable.INIT_INT;//品种类型ID
                List<CM_BreedClass> cM_BreedClassList =
                  cM_BreedClassBLL.GetListArray(string.Format("BreedClassID={0}", BreedClassID));
                if (cM_BreedClassList.Count > 0)
                {
                    CM_BreedClass cM_BreedClass = cM_BreedClassList[0];
                    if (cM_BreedClass != null)
                    {
                        breedClassTypeID = Convert.ToInt32(cM_BreedClass.BreedClassTypeID);
                    }
                }
                if (breedClassTypeID == (int)GTA.VTS.Common.CommonObject.Types.BreedClassTypeEnum.StockIndexFuture)
                {
                    if (!string.IsNullOrEmpty(CommodityCode))
                    {
                        if (!cMFuseTimesectionDAL.DeleteByCommodityCode(CommodityCode, Tran, db))
                        {
                            Tran.Rollback();
                            return false;
                        }
                        else
                        {
                            if (!cMCommodityFuseDAL.Delete(CommodityCode, Tran, db))
                            {
                                Tran.Rollback();
                                return false;
                            }
                        }
                    }
                    TradeCommodityAssignDAL.DeleteByCommodityCode(CommodityCode, Tran, db);
                    if (!cMCommodityDAL.Delete(CommodityCode, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                else
                {
                    TradeCommodityAssignDAL.DeleteByCommodityCode(CommodityCode, Tran, db);
                    if (!cMCommodityDAL.Delete(CommodityCode, Tran, db))
                    {
                        Tran.Rollback();
                        return false;
                    }
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-4301";
                string errMsg = "删除交易商品失败!";
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
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.CM_Commodity GetModel(string CommodityCode)
        {
            return cM_CommodityDAL.GetModel(CommodityCode);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.CM_Commodity GetModelByCache(string CommodityCode)
        {
            string CacheKey = "CM_CommodityModel-" + CommodityCode;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = cM_CommodityDAL.GetModel(CommodityCode);
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
            return (ManagementCenter.Model.CM_Commodity)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return cM_CommodityDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 获取所有交易商品

        /// <summary>
        /// 获取所有交易商品
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <param name="CommodityName">商品名称</param>
        ///  <param name="BreedClassID">品种ID</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllCMCommodity(string CommodityCode, string CommodityName, int BreedClassID, int pageNo,
                                         int pageSize,
                                         out int rowCount)
        {
            try
            {
                CM_CommodityDAL cMCommodityDAL = new CM_CommodityDAL();
                return cMCommodityDAL.GetAllCMCommodity(CommodityCode, CommodityName, BreedClassID, pageNo, pageSize,
                                                        out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-4303";
                string errMsg = "获取所有交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取品种类型股指期货的商品代码

        /// <summary>
        /// 获取品种类型股指期货的商品代码
        /// </summary>
        /// <returns></returns>
        public DataSet GetQHSIFCommodityCode()
        {
            try
            {
                CM_CommodityDAL cMCommodityDAL = new CM_CommodityDAL();
                return cMCommodityDAL.GetQHSIFCommodityCode();
            }
            catch (Exception ex)
            {
                string errCode = "GL-4304";
                string errMsg = "获取品种类型股指期货的商品代码失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 根据查询条件获取所有的交易商品（查询条件可为空）

        /// <summary>
        /// 根据查询条件获取所有的交易商品（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_Commodity> GetListArray(string strWhere)
        {
            try
            {
                return cM_CommodityDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4305";
                string errMsg = "根据查询条件获取所有的交易商品失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #region 获取所有代码及昨日收盘价
        /// <summary>
        /// 获取所有代码及昨日收盘价
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CM_Commodity> GetListCMCommodityAndClosePrice(string strWhere)
        {
            try
            {
                return cM_CommodityDAL.GetListCMCommodityAndClosePrice(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4309";
                string errMsg = "获取所有代码及昨日收盘价失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #region 提供前台获取商品代码的方法

        /// <summary>
        /// 提供前台获取商品代码的方法
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.CommonTable.OnstageCommodity> GetCommodityListArray(string strWhere)
        {
            try
            {
                return cM_CommodityDAL.GetCommodityListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-4306";
                string errMsg = "提供前台获取商品代码的方法失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return null;
            }
        }

        #endregion

        #region 判断交易商品代码是否已经存在

        /// <summary>
        /// 判断交易商品代码是否已经存在
        /// </summary>
        /// <param name="CommodityCode">商品代码</param>
        /// <returns></returns>
        public bool IsExistCommodityCode(string CommodityCode)
        {
            try
            {
                CM_CommodityDAL cMCommodityDAL = new CM_CommodityDAL();
                string strWhere = string.Format("CommodityCode='{0}'", CommodityCode);
                DataSet _ds = cMCommodityDAL.GetList(strWhere);
                if (_ds != null)
                {
                    if (_ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4307";
                string errMsg = "判断交易商品代码是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 判断交易商品名称是否已经存在

        /// <summary>
        /// 判断交易商品名称是否已经存在
        /// </summary>
        /// <param name="CommodityName">商品名称</param>
        /// <returns></returns>
        public bool IsExistCommodityName(string CommodityName)
        {
            try
            {
                CM_CommodityDAL cMCommodityDAL = new CM_CommodityDAL();
                string strWhere = string.Format("CommodityName='{0}'", CommodityName);
                DataSet _ds = cMCommodityDAL.GetList(strWhere);
                if (_ds != null)
                {
                    if (_ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                string errCode = "GL-4308";
                string errMsg = "判断交易商品名称是否已经存在失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #endregion  成员方法
    }
}