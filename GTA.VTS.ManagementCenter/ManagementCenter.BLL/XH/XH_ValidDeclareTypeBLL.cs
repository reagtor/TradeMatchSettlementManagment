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
    ///描述：有效申报类型表 业务逻辑类XH_ValidDeclareTypeBLL 的摘要说明。错误编码范围:5260-5279
    ///作者：刘书伟
    ///日期:2008-12-2
    /// </summary>
    public class XH_ValidDeclareTypeBLL
    {
        /// <summary>
        /// 有效申报类型表 DAL
        /// </summary>
        private readonly ManagementCenter.DAL.XH_ValidDeclareTypeDAL xH_ValidDeclareTypeDAL =
            new ManagementCenter.DAL.XH_ValidDeclareTypeDAL();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_ValidDeclareTypeBLL()
        {
        }
        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_ValidDeclareTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassValidID)
        {
            return xH_ValidDeclareTypeDAL.Exists(BreedClassValidID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        //public void Add(ManagementCenter.Model.XH_ValidDeclareType model)
        //{
        //    xH_ValidDeclareTypeDAL.Add(model);
        //}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        //public void Update(ManagementCenter.Model.XH_ValidDeclareType model)
        //{
        //    xH_ValidDeclareTypeDAL.Update(model);
        //}

        #region 根据品种有效申报标识获取有效申报类型实体
        /// <summary>
        /// 根据品种有效申报标识获取有效申报类型实体
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public ManagementCenter.Model.XH_ValidDeclareType GetModelValidDeclareType(int BreedClassValidID)
        {
            try
            {
                return xH_ValidDeclareTypeDAL.GetModel(BreedClassValidID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5266";
                string errMsg = "根据查询条件获取所有的有效申报类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_ValidDeclareType GetModelByCache(int BreedClassValidID)
        {
            string CacheKey = "XH_ValidDeclareTypeModel-" + BreedClassValidID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_ValidDeclareTypeDAL.GetModel(BreedClassValidID);
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
            return (ManagementCenter.Model.XH_ValidDeclareType)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_ValidDeclareTypeDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #region 根据查询条件获取所有的有效申报类型（查询条件可为空）
        /// <summary>
        /// 根据查询条件获取所有的有效申报类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_ValidDeclareType> GetListArray(string strWhere)
        {
            try
            {
                return xH_ValidDeclareTypeDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5265";
                string errMsg = "根据查询条件获取所有的有效申报类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion

        #endregion  成员方法

        #region 根据品种有效申报标识删除有效申报取值和有效申报类型

        /// <summary>
        ///  根据品种有效申报标识删除有效申报取值和有效申报类型
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <param name="tran">事务</param>
        /// <param name="db">db</param>
        /// <param name="flag">状态标识</param>
        /// <returns></returns>
        public bool DeleteValidDeclareValue(int BreedClassValidID, DbTransaction tran, Database db, bool flag)
        {
            XH_ValidDeclareTypeDAL xHValidDeclareTypeDAL = new XH_ValidDeclareTypeDAL();
            XH_ValidDeclareValueDAL xHValidDeclareValueDAL = new XH_ValidDeclareValueDAL();
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

                xHValidDeclareValueDAL.DeleteVDeclareValue(BreedClassValidID, tran, db);
                xHValidDeclareTypeDAL.DeleteValidDeclareType(BreedClassValidID, tran, db);
                if (!flag)
                {
                    tran.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                if (!flag) tran.Rollback();
                string errCode = "GL-5261";
                string errMsg = "根据品种有效申报标识删除有效申报取值和有效申报类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);

                return false;
            }
            finally
            {
                if (Conn != null && Conn.State == ConnectionState.Open)
                {
                    if (!flag) Conn.Close();
                }
            }
        }

        #endregion

        #region 根据品种有效申报标识删除有效申报取值和有效申报类型(重载)

        /// <summary>
        ///  根据品种有效申报标识删除有效申报取值和有效申报类型
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public bool DeleteValidDeclareValue(int BreedClassValidID)
        {
            try
            {
                return DeleteValidDeclareValue(BreedClassValidID, null, null, false);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5262";
                string errMsg = "根据品种有效申报标识删除有效申报取值和有效申报类型(重载)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 根据品种有效申报标识获取有效申报取值实体

        /// <summary>
        /// 根据品种有效申报标识获取有效申报取值实体
        /// </summary>
        /// <param name="BreedClassValidID">品种有效申报标识</param>
        /// <returns></returns>
        public ManagementCenter.Model.XH_ValidDeclareValue GetModelValidDeclareValue(int BreedClassValidID)
        {
            try
            {
                XH_ValidDeclareValueDAL xHValidDeclareValueDAL = new XH_ValidDeclareValueDAL();
                return xHValidDeclareValueDAL.GetModelValidDeclareValue(BreedClassValidID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-5264";
                string errMsg = "根据品种有效申报标识获取有效申报取值实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion
    }
}