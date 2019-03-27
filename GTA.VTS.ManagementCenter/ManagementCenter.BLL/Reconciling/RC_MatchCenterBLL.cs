using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using GTA.VTS.Common.CommonObject;
using GTA.VTS.Common.CommonUtility;
using LTP.Common;
using ManagementCenter.DAL;
using ManagementCenter.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ManagementCenter.BLL
{
    /// <summary>
    /// 描述：业务逻辑类RC_MatchCenterBLL 的摘要说明。
    /// 作者：熊晓凌    修改：刘书伟
    /// 日期：2008-12-15  2009-10-30 异常编码范围 2200-2220
    /// </summary>
    public class RC_MatchCenterBLL
    {
        private readonly ManagementCenter.DAL.RC_MatchCenterDAL dal = new ManagementCenter.DAL.RC_MatchCenterDAL();

        public RC_MatchCenterBLL()
        {
        }

        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int MatchCenterID)
        {
            return dal.Exists(MatchCenterID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(ManagementCenter.Model.RC_MatchCenter model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(ManagementCenter.Model.RC_MatchCenter model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int MatchCenterID)
        {
            dal.Delete(MatchCenterID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ManagementCenter.Model.RC_MatchCenter GetModel(int MatchCenterID)
        {
            return dal.GetModel(MatchCenterID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public ManagementCenter.Model.RC_MatchCenter GetModelByCache(int MatchCenterID)
        {
            string CacheKey = "RC_MatchCenterModel-" + MatchCenterID;
            object objModel = LTP.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(MatchCenterID);
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
            return (ManagementCenter.Model.RC_MatchCenter) objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}
        /// <summary>
        /// 获取所有的撮合中心列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<ManagementCenter.Model.RC_MatchCenter> GetListArray(string strWhere)
        {
            return dal.GetListArray(strWhere);
        }

        /// <summary>
        /// 向导数据保存
        /// </summary>
        /// <param name="MatchCenter">撮合中心实体</param>
        /// <param name="dt">交易所撮合机个数分配表</param>
        /// <returns></returns>
        public bool GuideSave(RC_MatchCenter MatchCenter, DataTable dt)
        {
            try
            {
                RC_MatchCenterDAL MatchCenterDAL = new RC_MatchCenterDAL();
                RC_MatchMachineDAL MatchMachineDAL = new RC_MatchMachineDAL();
                RC_TradeCommodityAssignDAL TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();

                Database db = DatabaseFactory.CreateDatabase();
                DbConnection Conn = db.CreateConnection();
                if (Conn.State != ConnectionState.Open) Conn.Open();
                DbTransaction Tran = Conn.BeginTransaction();
                try
                {
                    TradeCommodityAssignDAL.DeleteAll(Tran, db);
                    MatchMachineDAL.DeleteAll(Tran, db);
                    MatchCenterDAL.DeleteAll(Tran, db);

                    int MatchCenterID = MatchCenterDAL.Add(MatchCenter, Tran, db);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int BourseTypeID = int.Parse(dt.Rows[i]["BourseTypeID"].ToString());
                        int MachineNO = int.Parse(dt.Rows[i]["MachineNo"].ToString());
                        if (!AssignCode(MachineNO, BourseTypeID, MatchCenterID, Tran, db))
                            return false;
                    }
                    Tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Tran.Rollback();
                    string errCode = "GL-2200";
                    string errMsg = "向导数据保存失败";
                    VTException vte = new VTException(errCode, errMsg, ex);
                    LogHelper.WriteError(vte.ToString(), vte.InnerException);
                    return false;
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open) Conn.Close();
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-2201";
                string errMsg = "数据库连接失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }


        /// <summary>
        /// 代码分配
        /// </summary>
        /// <param name="MachineNO">撮合机个数</param>
        /// <param name="BourseTypeID">交易所标识ID</param>
        /// <param name="MatchCenterID">撮合中心ID</param>
        /// <param name="Tran">事务</param>
        /// <param name="db">数据库</param>
        /// <returns></returns>
        private bool AssignCode(int MachineNO, int BourseTypeID, int MatchCenterID, DbTransaction Tran, Database db)
        {
            RC_MatchMachineDAL MatchMachineDAL = new RC_MatchMachineDAL();
            RC_TradeCommodityAssignDAL TradeCommodityAssignDAL = new RC_TradeCommodityAssignDAL();
            try
            {
                RC_MatchMachine MatchMachine;
                RC_TradeCommodityAssign TradeCommodityAssign;

                DataSet ds = TradeCommodityAssignDAL.GetCodeListByBourseTypeID(BourseTypeID);
                int CodeNum = ds.Tables[0].Rows.Count;
                int M_C_Num = (int) CodeNum/MachineNO;

                for (int i = 1; i <= MachineNO; i++)
                {
                    MatchMachine = new RC_MatchMachine();
                    MatchMachine.BourseTypeID = BourseTypeID;
                    MatchMachine.MatchCenterID = MatchCenterID;
                    int MachineID = MatchMachineDAL.Add(MatchMachine, Tran, db);

                    int start, end;
                    if (i < MachineNO)
                    {
                        start = (i - 1)*M_C_Num;
                        end = i*M_C_Num;
                    }
                    else
                    {
                        start = (i - 1)*M_C_Num;
                        end = CodeNum;
                    }
                    for (int j = start; j < end; j++)
                    {
                        TradeCommodityAssign = new RC_TradeCommodityAssign();
                        TradeCommodityAssign.CommodityCode = ds.Tables[0].Rows[j]["CommodityCode"].ToString();
                        TradeCommodityAssign.MatchMachineID = MachineID;
                        //代码来源那个表:1是CM_Commodity表;2：是HK_Commodity表
                        TradeCommodityAssign.CodeFormSource = Convert.ToInt32(ds.Tables[0].Rows[j]["CodeFormSource"].ToString());
                        TradeCommodityAssignDAL.Add(TradeCommodityAssign, Tran, db);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string errCode = "GL-2202";
                string errMsg = "代码分配失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
                return false;
            }
        }

        /// <summary>
        /// 检测撮合中心连接
        /// </summary>
        public void CenterTestConnection()
        {
            try
            {
                List<ManagementCenter.Model.RC_MatchCenter> l_MatchCenter = dal.GetListArray(string.Empty);
                foreach (RC_MatchCenter center in l_MatchCenter)
                {
                    bool flag = false;
                    try
                    {
                        flag = ServiceIn.DoOrderServiceProxy.GetInstance().TestConnection(center.IP, (int) center.Port,
                                                                                          center.XiaDanService);
                    }
                    catch (VTException ex)
                    {
                        //写异常日志
                        flag = false;
                        GTA.VTS.Common.CommonUtility.LogHelper.WriteError(ex.Message, ex);
                    }
                    if (center.State == null)
                    {
                        if (flag)
                        {
                            center.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess;
                        }
                        else
                        {
                            center.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat;
                        }
                        dal.Update(center);
                    }
                    else if (center.State == (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat)
                    {
                        if (flag)
                        {
                            center.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnSuccess;
                            dal.Update(center);
                        }
                    }
                    else
                    {
                        if (!flag)
                        {
                            center.State = (int) ManagementCenter.Model.CommonClass.Types.StateEnum.ConnDefeat;
                            dal.Update(center);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errCode = "GL-2203";
                string errMsg = "检测撮合中心连接失败";
                VTException vte = new VTException(errCode, errMsg, ex);
                LogHelper.WriteError(vte.ToString(), vte.InnerException);
            }
        }

        #endregion  成员方法
    }
}