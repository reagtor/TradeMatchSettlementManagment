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
    ///描述：合约交割月份  业务逻辑类QH_AgreementDeliveryMonthBLL 的摘要说明。错误编码范围:6060-6069
    ///作者：刘书伟
    ///日期：2008-11-22
    ///修改：叶振东
    ///日期：2010-01-20
    ///描述：添加通过交割月份标识和品种标识查询该品种的某个交割月份是否存在
    /// </summary>
    public class QH_AgreementDeliveryMonthBLL
    {
        private readonly ManagementCenter.DAL.QH_AgreementDeliveryMonthDAL qH_AgreementDeliveryMonthDAL =
            new ManagementCenter.DAL.QH_AgreementDeliveryMonthDAL();

        public QH_AgreementDeliveryMonthBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_AgreementDeliveryMonthDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AgreementDeliveryMonthID)
        {
            return qH_AgreementDeliveryMonthDAL.Exists(AgreementDeliveryMonthID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.QH_AgreementDeliveryMonth model)
        {
            return qH_AgreementDeliveryMonthDAL.Add(model);
        }

        #region 更新交割月份(包括添加,删除)

        /// <summary>
        /// 更新交割月份(包括添加,删除)
        /// </summary>
        /// <param name="addMonthID">需要添加的月份ID</param>
        /// <param name="deleteMonthID">需要删除的月份ID</param>
        /// <param name="BreedClassID">品种ID</param>
        /// <returns></returns>
        public bool UpdateQHAgreementDeliveryMonth(List<int> addMonthID, List<int> deleteMonthID, int BreedClassID)
        {
            QH_AgreementDeliveryMonthDAL qHAgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
            QH_AgreementDeliveryMonth QH_AgreementDeliveryMonth = new QH_AgreementDeliveryMonth();
            QH_AgreementDeliveryMonth.BreedClassID = BreedClassID;
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
                foreach (int addM in addMonthID)
                {
                    QH_AgreementDeliveryMonth.MonthID = addM;
                 int a= qHAgreementDeliveryMonthDAL.Add(QH_AgreementDeliveryMonth, Tran, db);
                }

                foreach (int  deleM in deleteMonthID)
                {
                    QH_AgreementDeliveryMonth.MonthID = deleM;
                    qHAgreementDeliveryMonthDAL.Delete(deleM, BreedClassID, Tran, db);
                }
                Tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Tran.Rollback();
                string errCode = "GL-6060";
                string errMsg = "更新交割月份(包括添加,删除)失败!";
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
        public void Update(ManagementCenter.Model.QH_AgreementDeliveryMonth model)
        {
            qH_AgreementDeliveryMonthDAL.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int AgreementDeliveryMonthID)
        {
            qH_AgreementDeliveryMonthDAL.Delete(AgreementDeliveryMonthID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.QH_AgreementDeliveryMonth GetModel(int AgreementDeliveryMonthID)
        {
            return qH_AgreementDeliveryMonthDAL.GetModel(AgreementDeliveryMonthID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_AgreementDeliveryMonth GetModelByCache(int AgreementDeliveryMonthID)
        {
            string CacheKey = "QH_AgreementDeliveryMonthModel-" + AgreementDeliveryMonthID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_AgreementDeliveryMonthDAL.GetModel(AgreementDeliveryMonthID);
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
            return (ManagementCenter.Model.QH_AgreementDeliveryMonth) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_AgreementDeliveryMonthDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的合约交割月份（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_AgreementDeliveryMonth> GetListArray(string strWhere)
        {
            try
            {
                return qH_AgreementDeliveryMonthDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6061";
                string errMsg = "根据查询条件获取所有的合约交割月份（查询条件可为空）失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        /// <summary>
        /// 根据品种标识返回合约交割月份
        /// </summary>
        /// <param name="breedClassID">品种标识</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_AgreementDeliveryMonth> GetQHAgreementDeliveryMonth(int breedClassID)
        {
            try
            {
                QH_AgreementDeliveryMonthDAL qHAgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
                return qHAgreementDeliveryMonthDAL.GetListArray(string.Format("BreedClassID={0}", breedClassID));
            }
            catch (Exception ex)
            {
                string errCode = "GL-6062";
                string errMsg = "根据品种标识返回合约交割月份失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        /// <summary>
        /// 通过交割月份标识和品种标识查询某个品种的某个交割月份是否存在
        /// </summary>
        /// <param name="BreedClassID">品种标识</param>
        /// <param name="monthid">交割月份标识</param>
        /// <returns>某个品种的某个交割月份是否存在</returns>
        public QH_AgreementDeliveryMonth GetQHAgreementDeliveryBreedClassID(int BreedClassID, int monthid)
        {
            try
            {
                QH_AgreementDeliveryMonthDAL qHAgreementDeliveryMonthDAL = new QH_AgreementDeliveryMonthDAL();
                return qHAgreementDeliveryMonthDAL.GetQHAgreementDeliveryBreedClassID (BreedClassID, monthid);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6062";
                string errMsg = "根据品种标识返回合约交割月份失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }
        #endregion  成员方法

        
    }
}