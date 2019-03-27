using System;
using System.Collections.Generic;
using System.Data;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using ManagementCenter.Model.CommonClass;

namespace ManagementCenter.BLL
{
    /// <summary>
    ///描述：商品期货_保证金比例 业务逻辑类QH_CFBailScaleValueBLL 的摘要说明。错误编码范围:6600-6619
    ///作者：刘书伟
    ///日期:2008-12-13
    /// </summary>
    public class QH_CFBailScaleValueBLL
    {
        private readonly ManagementCenter.DAL.QH_CFBailScaleValueDAL qH_CFBailScaleValueDAL =
            new ManagementCenter.DAL.QH_CFBailScaleValueDAL();

        public QH_CFBailScaleValueBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return qH_CFBailScaleValueDAL.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CFBailScaleValueID)
        {
            return qH_CFBailScaleValueDAL.Exists(CFBailScaleValueID);
        }

        #region 添加商品期货_保证金比例

        /// <summary>
        /// 添加商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public int AddQHCFBailScaleValue(ManagementCenter.Model.QH_CFBailScaleValue model)
        {
            try
            {
                return qH_CFBailScaleValueDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6600";
                string errMsg = "添加商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        /// <summary>
        /// 添加商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <param name="model2">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public int AddQHCFBailScaleValue(QH_CFBailScaleValue model, QH_CFBailScaleValue model2)
        {
            try
            {
                //新增关联记录
                int rlt = qH_CFBailScaleValueDAL.Add(model2);
                if (rlt != AppGlobalVariable.INIT_INT)
                {
                    model.RelationScaleID = rlt;
                }
                return qH_CFBailScaleValueDAL.Add(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6600";
                string errMsg = "添加商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return AppGlobalVariable.INIT_INT;
            }
        }

        /// <summary>
        /// 添加商品期货_最低保证金比例
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddQHCFMinScaleValue(QH_SIFBail model)
        {
            try
            {
                QH_SIFBailDAL bailDal = new QH_SIFBailDAL();
                if (bailDal.GetModel(model.BreedClassID) == null)
                {
                    return bailDal.Add(model);
                }
                else
                {
                    return bailDal.Update(model);
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-6600";
                string errMsg = "设置商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 更新商品期货_保证金比例

        /// <summary>
        /// 更新商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public bool UpdateQHCFBailScaleValue(ManagementCenter.Model.QH_CFBailScaleValue model)
        {
            try
            {
                return qH_CFBailScaleValueDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6601";
                string errMsg = "更新商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 更新商品期货_保证金比例
        /// </summary>
        /// <param name="model">商品期货_保证金比例实体</param>
        /// <param name="model2">商品期货_保证金比例实体</param>
        /// <returns></returns>
        public bool UpdateQHCFBailScaleValue(QH_CFBailScaleValue model, QH_CFBailScaleValue model2)
        {
            try
            {
                if (model.RelationScaleID != null && qH_CFBailScaleValueDAL.GetModel(model.RelationScaleID.Value) != null)
                {
                    //关联记录存在，更新
                    qH_CFBailScaleValueDAL.Update(model2);
                }
                else
                {
                    //关联记录不存在，新增记录
                    int rtn = qH_CFBailScaleValueDAL.Add(model2);
                    if (rtn != AppGlobalVariable.INIT_INT)
                    {
                        model.RelationScaleID = rtn;
                    }
                }
                return qH_CFBailScaleValueDAL.Update(model);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6601";
                string errMsg = "更新商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        #region 删除商品期货_保证金比例

        /// <summary>
        /// 删除商品期货_保证金比例
        /// </summary>
        /// <param name="CFBailScaleValueID">商品期货_保证金比例ID</param>
        /// <returns></returns>
        public bool DeleteQHCFBailScaleValue(int CFBailScaleValueID)
        {
            try
            {
                return qH_CFBailScaleValueDAL.Delete(CFBailScaleValueID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6602";
                string errMsg = "删除商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return false;
            }
        }

        #endregion

        

        #region 根据商品期货_保证金比例ID获取商品期货_保证金比例对象实体

        /// <summary>
        /// 根据商品期货_保证金比例ID获取商品期货_保证金比例对象实体
        /// </summary>
        /// <param name="CFBailScaleValueID">商品期货_保证金比例ID</param>
        /// <returns></returns>
        public ManagementCenter.Model.QH_CFBailScaleValue GetQHCFBailScaleValueModel(int CFBailScaleValueID)
        {
            try
            {
                return qH_CFBailScaleValueDAL.GetModel(CFBailScaleValueID);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6603";
                string errMsg = "根据商品期货_保证金比例ID获取商品期货_保证金比例对象实体失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.QH_CFBailScaleValue GetModelByCache(int CFBailScaleValueID)
        {
            string CacheKey = "QH_CFBailScaleValueModel-" + CFBailScaleValueID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = qH_CFBailScaleValueDAL.GetModel(CFBailScaleValueID);
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
            return (ManagementCenter.Model.QH_CFBailScaleValue) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return qH_CFBailScaleValueDAL.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 根据查询条件获取所有的商品期货_保证金比例（查询条件可为空）
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public List<ManagementCenter.Model.QH_CFBailScaleValue> GetListArray(string strWhere)
        {
            try
            {
                return qH_CFBailScaleValueDAL.GetListArray(strWhere);
            }
            catch (Exception ex)
            {
                string errCode = "GL-6604";
                string errMsg = "根据查询条件获取所有的商品期货_保证金比例（查询条件可为空失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #region 获取所有商品期货_保证金比例

        /// <summary>
        ///获取所有商品期货_保证金比例
        /// </summary>
        /// <param name="BreedClassName">品种名称</param>
        /// <param name="DeliveryMonthTypeID">交割月份类型标识</param>
        /// <param name="PositionBailTypeID">持仓和保证金控制类型标识</param>
        /// <param name="pageNo">当前页</param>
        /// <param name="pageSize">显示记录数</param>
        /// <param name="rowCount">总行数</param>
        /// <returns></returns>
        public DataSet GetAllQHCFBailScaleValue(string BreedClassName, int DeliveryMonthTypeID, int PositionBailTypeID,
                                                int pageNo, int pageSize,
                                                out int rowCount)
        {
            try
            {
                QH_CFBailScaleValueDAL qHCFBailScaleValueDAL = new QH_CFBailScaleValueDAL();
                return qHCFBailScaleValueDAL.GetAllQHCFBailScaleValue(BreedClassName, DeliveryMonthTypeID,
                                                                      PositionBailTypeID, pageNo, pageSize, out rowCount);
            }
            catch (Exception ex)
            {
                rowCount = AppGlobalVariable.INIT_INT;
                string errCode = "GL-6605";
                string errMsg = "获取所有商品期货_保证金比例失败!";
                VTException exception = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(exception.ToString(), exception.InnerException);
                return null;
            }
        }

        #endregion

        #endregion  成员方法


        
    }
}