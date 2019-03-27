using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoftDogInterface
{
    /// <summary>
    /// Desc：加密狗操作相关基类
    /// Create By：李健华
    /// Create Date:2009-06-10
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe class GrandDog
    {
        #region 定义加载 指示该属性化方法由非托管动态链接库 (DLL) 作为静态入口点公开 的相关方法
        //define the import function  ,CallingConvention=CallingConvention.Cdecl 
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_OpenDog(uint ulFlag, byte* pszProductName, uint* pDogHandle);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_GetDogInfo(uint DogHandle, byte* pHardwareInfo, uint* pulLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_GetProductCurrentNo(uint DogHandle, uint* pulProductCurrentNo);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_VerifyPassword(uint DogHandle, byte bPasswordType, string szPassword, byte* pbDegree);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_ChangePassword(uint DogHandle, byte bPasswordType, string szPassword);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_SetKey(uint DogHandle, byte bKeyType, byte* pucIn, uint ulLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_EncryptData(uint DogHandle, byte* pucIn, uint ulInLen, byte* pucOut, uint* pulOutLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_DecryptData(uint DogHandle, byte* pucIn, uint ulInLen, byte* pucOut, uint* pulOutLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_SignData(uint DogHandle, byte* pucIn, uint ulInLen, byte* pucOut, uint* pulOutLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_ConvertData(uint DogHandle, byte* pucIn, uint ulInLen, uint* pulResult);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_CheckDog(uint DogHandle);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_GetRandom(uint DogHandle, byte* pucOut, uint ulInLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_CreateDir(uint DogHandle, ushort usDirID, uint ulDirSize);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_CreateFile(uint DogHandle, ushort usDirID, ushort usFileID, byte bFiletype, uint ulFileSize);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_DeleteDir(uint DogHandle, ushort usDirID);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_DeleteFile(uint DogHandle, ushort usDirID, ushort usFileID);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_DefragFileSystem(uint DogHandle, ushort usDirID);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_ReadFile(uint DogHandle, ushort usDirID, ushort usFileID, uint ulPos, uint ulLen, byte* pucOut);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_WriteFile(uint DogHandle, ushort usDirID, ushort usFileID, uint ulPos, uint ulLen, byte* pucIn);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_VisitLicenseFile(uint DogHandle, ushort usDirID, ushort usFileID, uint ulReserved);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_ExecuteFile(uint DogHandle, ushort usDirID, ushort usFileID, byte* pucIn, uint ulInlen, byte* pucOut, uint* pulOutlen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_GetUpgradeRequestString(uint DogHandle, byte* pucBuf, uint* pulLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_Upgrade(uint DogHandle, byte* pucUpgrade, uint ulLen);
        [DllImport("RCGrandDogW32.dll", CharSet = CharSet.Ansi)]
        public static unsafe extern uint rc_CloseDog(uint DogHandle);
        #endregion

        #region 扩展后的相关操作方法
        /// <summary>
        /// 打开硬件狗
        /// </summary>
        /// <param name="ulFlag">打开狗的标志1，本地第一个</param>
        /// <param name="pszProductName">硬件狗对应的产品名，名称只能为16个字符（字节）内</param>
        /// <param name="pDogHandle">硬件狗句柄</param>
        /// <returns></returns>
        protected unsafe uint OpenDog(uint ulFlag, byte* pszProductName, uint* pDogHandle)
        {
            return rc_OpenDog(ulFlag, pszProductName, pDogHandle);
        }
        /// <summary>
        /// 关闭加密狗操作
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <returns></returns>
        protected unsafe uint CloseDog(uint DogHandle)
        {
            return rc_CloseDog(DogHandle);
        }
        /// <summary>
        /// 获取密狗相关信息
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pHardwareInfo"></param>
        /// <param name="pulLen"></param>
        /// <returns></returns>
        protected unsafe uint GetDogInfo(uint DogHandle, byte* pHardwareInfo, uint* pulLen)
        {
            return rc_GetDogInfo(DogHandle, pHardwareInfo, pulLen);
        }
        /// <summary>
        /// 获取密狗产品流水号
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pulProductCurrentNo"></param>
        /// <returns></returns>
        protected unsafe uint GetProductCurrentNo(uint DogHandle, uint* pulProductCurrentNo)
        {
            return rc_GetProductCurrentNo(DogHandle, pulProductCurrentNo);
        }
        /// <summary>
        ///  检查密狗密码
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="bPasswordType"></param>
        /// <param name="szPassword"></param>
        /// <param name="pbDegree"></param>
        /// <returns></returns>
        protected unsafe uint VerifyPassword(uint DogHandle, byte bPasswordType, string szPassword, byte* pbDegree)
        {
            return rc_VerifyPassword(DogHandle, bPasswordType, szPassword, pbDegree);
        }
        /// <summary>
        /// 修改密狗密码
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="bPasswordType"></param>
        /// <param name="szPassword"></param>
        /// <returns></returns>
        protected unsafe uint ChangePassword(uint DogHandle, byte bPasswordType, string szPassword)
        {
            return rc_ChangePassword(DogHandle, bPasswordType, szPassword);
        }
        /// <summary>
        /// 设置签名及加解密密钥
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="bKeyType"></param>
        /// <param name="pucIn"></param>
        /// <param name="ulLen"></param>
        /// <returns></returns>
        protected unsafe uint SetKey(uint DogHandle, byte bKeyType, byte* pucIn, uint ulLen)
        {
            return rc_SetKey(DogHandle, bKeyType, pucIn, ulLen);
        }
        /// <summary>
        /// 数据加密
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucIn"></param>
        /// <param name="ulInLen"></param>
        /// <param name="pucOut"></param>
        /// <param name="pulOutLen"></param>
        /// <returns></returns>
        protected unsafe uint EncryptData(uint DogHandle, byte* pucIn, uint ulInLen, byte* pucOut, uint* pulOutLen)
        {
            return rc_EncryptData(DogHandle, pucIn, ulInLen, pucOut, pulOutLen);
        }
        /// <summary>
        /// 数据解密
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucIn"></param>
        /// <param name="ulInLen"></param>
        /// <param name="pucOut"></param>
        /// <param name="pulOutLen"></param>
        /// <returns></returns>
        protected unsafe uint DecryptData(uint DogHandle, byte* pucIn, uint ulInLen, byte* pucOut, uint* pulOutLen)
        {
            return rc_DecryptData(DogHandle, pucIn, ulInLen, pucOut, pulOutLen);
        }
        /// <summary>
        /// 数据签名
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucIn"></param>
        /// <param name="ulInLen"></param>
        /// <param name="pucOut"></param>
        /// <param name="pulOutLen"></param>
        /// <returns></returns>
        protected unsafe uint SignData(uint DogHandle, byte* pucIn, uint ulInLen, byte* pucOut, uint* pulOutLen)
        {
            return rc_SignData(DogHandle, pucIn, ulInLen, pucOut, pulOutLen);
        }
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucIn"></param>
        /// <param name="ulInLen"></param>
        /// <param name="pulResult"></param>
        /// <returns></returns>
        protected unsafe uint ConvertData(uint DogHandle, byte* pucIn, uint ulInLen, uint* pulResult)
        {
            return rc_ConvertData(DogHandle, pucIn, ulInLen, pulResult);
        }
        /// <summary>
        /// 检测硬件狗
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <returns></returns>
        protected unsafe uint CheckDog(uint DogHandle)
        {
            return rc_CheckDog(DogHandle);
        }
        /// <summary>
        /// 获取硬件随机数
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucOut"></param>
        /// <param name="ulInLen"></param>
        /// <returns></returns>
        protected unsafe uint GetRandom(uint DogHandle, byte* pucOut, uint ulInLen)
        {
            return rc_GetRandom(DogHandle, pucOut, ulInLen);
        }
        /// <summary>
        ///  创建文件夹
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="ulDirSize"></param>
        /// <returns></returns>
        protected unsafe uint CreateDir(uint DogHandle, ushort usDirID, uint ulDirSize)
        {
            return rc_CreateDir(DogHandle, usDirID, ulDirSize);
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="usFileID"></param>
        /// <param name="bFiletype"></param>
        /// <param name="ulFileSize"></param>
        /// <returns></returns>
        protected unsafe uint CreateFile(uint DogHandle, ushort usDirID, ushort usFileID, byte bFiletype, uint ulFileSize)
        {
            return rc_CreateFile(DogHandle, usDirID, usFileID, bFiletype, ulFileSize);
        }
        /// <summary>
        ///  删除文件夹
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <returns></returns>
        protected unsafe uint DeleteDir(uint DogHandle, ushort usDirID)
        {
            return rc_DeleteDir(DogHandle, usDirID);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="usFileID"></param>
        /// <returns></returns>
        protected unsafe uint DeleteFile(uint DogHandle, ushort usDirID, ushort usFileID)
        {
            return rc_DeleteFile(DogHandle, usDirID, usFileID);
        }
        /// <summary>
        /// 整理目录
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <returns></returns>
        protected unsafe uint DefragFileSystem(uint DogHandle, ushort usDirID)
        {
            return rc_DefragFileSystem(DogHandle, usDirID);
        }
        /// <summary>
        ///  读取文件
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="usFileID"></param>
        /// <param name="ulPos"></param>
        /// <param name="ulLen"></param>
        /// <param name="pucOut"></param>
        /// <returns></returns>
        protected unsafe uint ReadFile(uint DogHandle, ushort usDirID, ushort usFileID, uint ulPos, uint ulLen, byte* pucOut)
        {
            return rc_ReadFile(DogHandle, usDirID, usFileID, ulPos, ulLen, pucOut);
        }
        /// <summary>
        ///  写文件
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="usFileID"></param>
        /// <param name="ulPos"></param>
        /// <param name="ulLen"></param>
        /// <param name="pucIn"></param>
        /// <returns></returns>
        protected unsafe uint WriteFile(uint DogHandle, ushort usDirID, ushort usFileID, uint ulPos, uint ulLen, byte* pucIn)
        {
            return rc_WriteFile(DogHandle, usDirID, usFileID, ulPos, ulLen, pucIn);
        }
        /// <summary>
        /// 访问许可证文件
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="usFileID"></param>
        /// <param name="ulReserved"></param>
        /// <returns></returns>
        protected unsafe uint VisitLicenseFile(uint DogHandle, ushort usDirID, ushort usFileID, uint ulReserved)
        {
            return rc_VisitLicenseFile(DogHandle, usDirID, usFileID, ulReserved);
        }
        /// <summary>
        ///   执行算法文件
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="usDirID"></param>
        /// <param name="usFileID"></param>
        /// <param name="pucIn"></param>
        /// <param name="ulInlen"></param>
        /// <param name="pucOut"></param>
        /// <param name="pulOutlen"></param>
        /// <returns></returns>
        protected unsafe uint ExecuteFile(uint DogHandle, ushort usDirID, ushort usFileID, byte* pucIn, uint ulInlen, byte* pucOut, uint* pulOutlen)
        {
            return rc_ExecuteFile(DogHandle, usDirID, usFileID, pucIn, ulInlen, pucOut, pulOutlen);
        }
        /// <summary>
        ///  获取升级请求串
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucBuf"></param>
        /// <param name="pulLen"></param>
        /// <returns></returns>
        protected unsafe uint GetUpgradeRequestString(uint DogHandle, byte* pucBuf, uint* pulLen)
        {
            return rc_GetUpgradeRequestString(DogHandle, pucBuf, pulLen);
        }
        /// <summary>
        ///   使用升级串升级
        /// </summary>
        /// <param name="DogHandle"></param>
        /// <param name="pucUpgrade"></param>
        /// <param name="pulLen"></param>
        /// <returns></returns>
        protected unsafe uint Upgrade(uint DogHandle, byte* pucUpgrade, uint pulLen)
        {
            return rc_Upgrade(DogHandle, pucUpgrade, pulLen);
        }
        #endregion

    }
}
