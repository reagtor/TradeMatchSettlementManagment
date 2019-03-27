using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonUtility;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ReckoningCounter.DAL.Data;
using ReckoningCounter.Model;

namespace ReckoningCounter.DAL.CustomDataAccess
{
    /// <summary>
    /// 数据层功能类
    /// </summary>
    public static class DaoUtil
    {
        private const string ISNORMALEXIT = "IsNormalExit";
        private const string ISNORMALEXITVALUE = "Normal";

        private static bool isNormalExitLastTime = false;

        /// <summary>
        /// 是否正常退出
        /// </summary>
        public static bool IsNormalExitLastTime
        {
            get
            {
                return isNormalExitLastTime;
            }
        }

        /// <summary>
        /// 上次程序是否正常退出
        /// </summary>
        private static bool GetNormalExitLastTime()
        {
            bool isNormalExit = false;

            BD_StatusTableDal statusTableDal = new BD_StatusTableDal();
            try
            {
                var model = statusTableDal.GetModel(ISNORMALEXIT);
                if (model != null)
                    isNormalExit = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return isNormalExit;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            isNormalExitLastTime = GetNormalExitLastTime();
        }

        /// <summary>
        /// 删除正常退出标志
        /// </summary>
        public static void DeleteNormalFlag()
        {
            BD_StatusTableDal statusTableDal = new BD_StatusTableDal();
            statusTableDal.Delete(ISNORMALEXIT);
        }

        /// <summary>
        /// 添加正常退出标志
        /// </summary>
        public static void AddNormalFlag()
        {
            BD_StatusTableDal statusTableDal = new BD_StatusTableDal();
            //if (statusTableDal.Exists(ISNORMALEXITVALUE))
            statusTableDal.Delete(ISNORMALEXITVALUE);

            BD_StatusTableInfo model = new BD_StatusTableInfo();
            model.name = ISNORMALEXIT;
            model.value = ISNORMALEXITVALUE;

            bool isSuccess = false;
            try
            {
                AddOrUpdate(statusTableDal, model);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            if (!isSuccess)
            {
                statusTableDal.Delete(ISNORMALEXITVALUE);

                try
                {
                    AddOrUpdate(statusTableDal, model);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex.Message, ex);
                }
            }
        }

        private static void AddOrUpdate(BD_StatusTableDal statusTableDal, BD_StatusTableInfo model)
        {
            if (statusTableDal.Exists(ISNORMALEXITVALUE))
            {
                statusTableDal.Update(model);
            }
            else
            {
                statusTableDal.Add(model);
            }
        }

        /// <summary>
        /// 测试数据库连接是否正常
        /// </summary>
        /// <returns></returns>
        public static bool TestConnection()
        {
            Database db = DataManager.GetDatabase();

            bool isSuccess = false;
            try
            {
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    isSuccess = true;
                }
            }
            catch
            {

            }

            return isSuccess;
        }
    }

    /// <summary>
    /// 截取字符串操作类
    /// </summary>
    public class CutStringByBytes
    {
        private static Encoding myEncoding = Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 截取字符长度，以位长截取，默认为100长度.不足100位,则不截取.原样返回.
        /// </summary>
        /// <param name="SourceString">被截取源字符串</param>
        /// <returns>返回截取后字符串</returns>
        public static string CutStringByByBytes(string SourceString)
        {
            byte[] CutStr_Bytes = new byte[100];
            byte[] SourceStr_Bytes = myEncoding.GetBytes(SourceString);
            int Bytes_Count = SourceStr_Bytes.Length;
            if (Bytes_Count > 100)
            {
                Array.Copy(SourceStr_Bytes, 0, CutStr_Bytes, 0, 100);
                string CutedStr = myEncoding.GetString(CutStr_Bytes);
                CutedStr = CutedStr.Substring(0, CutedStr.Length - 1) + "...";
                return CutedStr;
            }
            return SourceString;
        }
        /// <summary>
        /// 截取字符长度，以位长截取，并指定截取位长.不足指字位长,则不截取.原样返回.
        /// </summary>
        /// <param name="SourceString">被截取源字符串</param>
        /// <param name="CutLeng">指定要截取位长长度</param>
        /// <returns>返回截取后字符串</returns>
        public static string CutStringByByBytes(string SourceString, int CutLeng)
        {
            byte[] CutStr_Bytes1 = new byte[CutLeng];
            byte[] SourceStr_Bytes = myEncoding.GetBytes(SourceString);
            int Bytes_Count = SourceStr_Bytes.Length;
            if (Bytes_Count > CutLeng)
            {
                Array.Copy(SourceStr_Bytes, 0, CutStr_Bytes1, 0, CutLeng);
                string CutedStr = myEncoding.GetString(CutStr_Bytes1);
                CutedStr = CutedStr.Substring(0, CutedStr.Length - 1) + "...";
                return CutedStr;
            }
            return SourceString;
        }
    }
}
