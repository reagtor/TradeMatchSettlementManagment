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
using ManagementCenter.Model.XH;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：现货_品种_涨跌幅_控制类型 业务逻辑类XH_SpotHighLowControlTypeBLL 的摘要说明。
    /// 错误编码范围:5220-5239
    ///作者：刘书伟
    ///日期：2008-11-27
    /// </summary>
    public class XH_SpotHighLowControlTypeBLL
    {
        private readonly ManagementCenter.DAL.XH_SpotHighLowControlTypeDAL xH_SpotHighLowControlTypeDAL =
            new ManagementCenter.DAL.XH_SpotHighLowControlTypeDAL();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public XH_SpotHighLowControlTypeBLL()
        {
        }
        #endregion

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return xH_SpotHighLowControlTypeDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int BreedClassHighLowID)
        {
            return xH_SpotHighLowControlTypeDAL.Exists(BreedClassHighLowID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(ManagementCenter.Model.XH_SpotHighLowControlType model)
        {
            xH_SpotHighLowControlTypeDAL.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.XH_SpotHighLowControlType model)
        {
            xH_SpotHighLowControlTypeDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int BreedClassHighLowID)
        {
            xH_SpotHighLowControlTypeDAL.Delete(BreedClassHighLowID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.XH_SpotHighLowControlType GetModel(int BreedClassHighLowID)
        {
            try
            {
                return xH_SpotHighLowControlTypeDAL.GetModel(BreedClassHighLowID);
            }
            catch (Exception ex)
            {
                GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.XH_SpotHighLowControlType GetModelByCache(int BreedClassHighLowID)
        {
            string CacheKey = "XH_SpotHighLowControlTypeModel-" + BreedClassHighLowID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = xH_SpotHighLowControlTypeDAL.GetModel(BreedClassHighLowID);
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
            return (ManagementCenter.Model.XH_SpotHighLowControlType)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return xH_SpotHighLowControlTypeDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的现货_品种_涨跌幅_控制类型（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.XH_SpotHighLowControlType> GetListArray(string strWhere)
        {
            try
            {
                return xH_SpotHighLowControlTypeDAL.GetListArray(strWhere);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5224";
                string errMsg = "根据查询条件获取所有的现货_品种_涨跌幅_控制类型失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion  成员方法

        #region 添加现货涨跌幅和有效申报
        /// <summary>
        /// 添加现货涨跌幅和有效申报
        /// </summary>
        /// <param name="xHSpotHighLowConType"></param>
        /// <param name="xHSpotHighLowValue"></param>
        /// <param name="xHValidDecType">有效申报类型实体类</param>
        /// <param name="xHValidDeclareValue">有效申报取值实体</param>
        /// <returns></returns>
        public XH_AboutSpotHighLowEntity AddXHSpotHighLowAndValidDecl(XH_SpotHighLowControlType xHSpotHighLowConType,
                                         XH_SpotHighLowValue xHSpotHighLowValue, XH_ValidDeclareType xHValidDecType,
                                          XH_ValidDeclareValue xHValidDeclareValue)
        {
            XH_SpotHighLowValueDAL xHSpotHighLowValueDAL = new XH_SpotHighLowValueDAL();
            XH_SpotHighLowControlTypeDAL xHSpotHighLowControlTypeDAL = new XH_SpotHighLowControlTypeDAL();
            XH_ValidDeclareTypeDAL xHValidDeclareTypeDAL = new XH_ValidDeclareTypeDAL();
            XH_ValidDeclareValueDAL xHValidDeclareValueDAL = new XH_ValidDeclareValueDAL();

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
                int breedClassHighLowID = AppGlobalVariable.INIT_INT;
                int breedClassValidID = AppGlobalVariable.INIT_INT;
                //List<XH_AboutSpotHighLowEntity> xhAboutSpotHighLowEList=new List<XH_AboutSpotHighLowEntity>();
                XH_AboutSpotHighLowEntity xhAboutSpotHighLowEntity = new XH_AboutSpotHighLowEntity();
                breedClassHighLowID = xHSpotHighLowControlTypeDAL.Add(xHSpotHighLowConType, Tran, db);
                if (breedClassHighLowID != AppGlobalVariable.INIT_INT)
                {
                    xHSpotHighLowValue.BreedClassHighLowID = breedClassHighLowID;

                    if (xHSpotHighLowValueDAL.AddXHSpotHighLowValue(xHSpotHighLowValue, Tran, db) == AppGlobalVariable.INIT_INT)
                    {
                        Tran.Rollback();
                    }
                }
                breedClassValidID = xHValidDeclareTypeDAL.AddValidDeclareType(xHValidDecType, Tran, db);

                if (breedClassValidID != AppGlobalVariable.INIT_INT)
                {
                    xHValidDeclareValue.BreedClassValidID = breedClassValidID;

                    if (!xHValidDeclareValueDAL.Add(xHValidDeclareValue, Tran, db))
                    {
                        Tran.Rollback();
                    }

                }
                Tran.Commit();
                xhAboutSpotHighLowEntity.BreedClassHighLowID = breedClassHighLowID;
                xhAboutSpotHighLowEntity.BreedClassValidID = breedClassValidID;
                return xhAboutSpotHighLowEntity;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5224";
                string errMsg = "添加现货和有效申报失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
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

        #region 根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型

        /// <summary>
        /// 根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <param name="tran"></param>
        /// <param name="db"></param>
        /// <param name="falg">标记是否使用了外部事务</param>
        /// <returns></returns>
        public bool DeleteSpotHighLowValue(int BreedClassHighLowID, DbTransaction tran, Database db, bool falg)
        {
            XH_SpotHighLowValueDAL xHSpotHighLowValueDAL = new XH_SpotHighLowValueDAL();
            XH_SpotHighLowControlTypeDAL xHSpotHighLowControlTypeDAL = new XH_SpotHighLowControlTypeDAL();
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

                xHSpotHighLowValueDAL.DeleteSpotHighLowValue(BreedClassHighLowID, tran, db);
                xHSpotHighLowControlTypeDAL.Delete(BreedClassHighLowID, tran, db);
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
                    string errCode = "GL-5221";
                    string errMsg = "根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型失败!";
                    VTException exception = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(exception.ToString(), exception.InnerException);

                }
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

        #region 根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型(重载)

        /// <summary>
        /// 根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型
        /// </summary>
        /// <param name="BreedClassHighLowID">品种涨跌幅标识</param>
        /// <returns></returns>
        public bool DeleteSpotHighLowValue(int BreedClassHighLowID)
        {
            try
            {
                return DeleteSpotHighLowValue(BreedClassHighLowID, null, null, false);

            }
            catch (Exception ex)
            {
                string errCode = "GL-5222";
                string errMsg = "根据品种涨跌幅标识删除涨跌幅取值和涨跌幅类型(重载)失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 更新现货涨跌幅取值和涨跌幅类型及有效申报和有效申报类型
        /// <summary>
        /// 更新现货涨跌幅取值和涨跌幅类型及有效申报和有效申报类型
        /// </summary>
        /// <param name="xHSpotHighLowConType"></param>
        /// <param name="xHSpotHighLowValue"></param>
        /// <param name="xHValidDecType"></param>
        /// <param name="xHValidDeclareValue"></param>
        /// <returns></returns>
        public bool UpdateXHSpotHighLowAndValidDecl(XH_SpotHighLowControlType xHSpotHighLowConType,
                                         XH_SpotHighLowValue xHSpotHighLowValue, XH_ValidDeclareType xHValidDecType,
                                          XH_ValidDeclareValue xHValidDeclareValue)
        {
            XH_SpotHighLowValueDAL xHSpotHighLowValueDAL = new XH_SpotHighLowValueDAL();
            XH_SpotHighLowControlTypeDAL xHSpotHighLowControlTypeDAL = new XH_SpotHighLowControlTypeDAL();
            XH_ValidDeclareTypeDAL xHValidDeclareTypeDAL = new XH_ValidDeclareTypeDAL();
            XH_ValidDeclareValueDAL xHValidDeclareValueDAL = new XH_ValidDeclareValueDAL();

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
                if (!xHSpotHighLowControlTypeDAL.Update(xHSpotHighLowConType))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!xHSpotHighLowValueDAL.Update(xHSpotHighLowValue))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!xHValidDeclareTypeDAL.Update(xHValidDecType))
                {
                    Tran.Rollback();
                    return false;
                }
                if (!xHValidDeclareValueDAL.Update(xHValidDeclareValue))
                {
                    Tran.Rollback();
                    return false;
                }
                Tran.Commit();
                return true;

            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-5223";
                string errMsg = "更新涨跌幅取值和涨跌幅类型失败!";
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
    }
}