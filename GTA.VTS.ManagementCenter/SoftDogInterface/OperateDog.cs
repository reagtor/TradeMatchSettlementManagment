using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftDogInterface
{
    /// <summary>
    /// Desc：操作狗扩展类
    /// Create By：李健华
    /// Create Date:2009-06-10
    /// </summary>
    public class OperateDog : GrandDog
    {
        #region 公共变量
        /// <summary>
        /// 操作返回相关代码
        /// </summary>
        public uint RetCode;
        /// <summary>
        ///  操作狗句柄
        /// </summary>
        private uint ulDogHandle;
        #region 未使用 update by 董鹏 2010-02-25
        ///// <summary>
        /////  打开狗的标志
        ///// </summary>
        //private uint OpenDogFlag;
        ///// <summary>
        /////  密码的类型
        ///// </summary>
        //private byte bPasswordType;
        ///// <summary>
        /////  密钥类型
        ///// </summary>
        //private byte bKeyType;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        private byte[] bUpgradeData = new Byte[400];
        /// <summary>
        /// 
        /// </summary>
        private byte[] bData1 = new Byte[16];
        /// <summary>
        /// 
        /// </summary>
        private byte[] bData2 = new Byte[16];
        /// <summary>
        /// 定底加密狗操作基类
        /// </summary>
        // private GrandDog Dog;
        #endregion

        public OperateDog()
        {
            //if (Dog == null)
            //    Dog = new GrandDog();
        }

        #region   打开硬件狗  public unsafe bool OpenDog(string prcName)
        /// <summary>
        ///  打开硬件狗
        /// </summary>
        /// <param name="prcName">要打开的硬件狗对应的产品名,产品名是以null结尾的字符串，最多是15个非零字符</param>
        /// <returns>返回是否打开成功</returns>
        public unsafe bool OpenDog(string prcName)
        {
            uint RetCode;
            fixed (uint* pDogHandle = &ulDogHandle)
            {
                char[] productName = new Char[16];
                productName = prcName.ToCharArray(0, prcName.Length);
                fixed (byte* pProductName = new byte[16])
                {
                    for (int i = 0; i < prcName.Length; i++)
                    {
                        *(pProductName + i) = (byte)(productName[i]);
                    }
                    *(pProductName + prcName.Length) = 0;
                    RetCode = OpenDog(1, pProductName, pDogHandle);
                }
            }
            if (RetCode != 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 获取硬件狗产品流水号 public unsafe string GetProductCurrentNo()
        /// <summary>
        /// 获取硬件狗产品流水号
        /// </summary>
        /// <returns></returns>
        public unsafe string GetProductCurrentNo()
        {
            uint ulProductCurrentNo;
            RetCode = GetProductCurrentNo(ulDogHandle, &ulProductCurrentNo);
            return ulProductCurrentNo.ToString();
        }
        #endregion

        #region 获取的狗的信息   public unsafe DogConfig.DogInfo GetDogInfo(ref uint retCode)
        /// <summary>
        /// 获取的狗的信息
        /// </summary>
        /// <param name="retCode">返回操作后的参数</param>
        /// <returns>返回狗的相关信息结构体</returns>
        public unsafe DogConfig.DogInfo GetDogInfo(ref uint retCode)
        {
            SoftDogInterface.DogConfig.DogInfo difo = new DogConfig.DogInfo();
            byte[] Data = new Byte[13];
            uint ulLen = 14; //这里先设置14，原来的demo默认不设置只有13，会报缓存不够
            fixed (byte* pHardwareInfo = &Data[0])
            {
                RetCode = GetDogInfo(ulDogHandle, pHardwareInfo, &ulLen);
            }
            if (RetCode == 0)
            {
                //succeeded
                //取得系列号
                uint SerialNumber = (uint)(Data[0] + Data[1] * 256 + Data[2] * 256 * 256 + Data[3] * 256 * 256 * 256);
                difo.SeriNO = SerialNumber.ToString();
                //取得流水号
                uint CurrentNumber = (uint)(Data[4] + Data[5] * 256 + Data[6] * 256 * 256 + Data[7] * 256 * 256 * 256);
                difo.CurrentNo = CurrentNumber.ToString();
                //取得狗类型
                if (Data[8] == (byte)DogConfig.DogType.RC_DOGTYPE_LOCAL)
                {
                    difo.Type = DogConfig.DogType.RC_DOGTYPE_LOCAL;
                }
                else
                {
                    difo.Type = DogConfig.DogType.RC_DOGTYPE_NET;
                }
                //取得狗类型
                string model = ((char)Data[9]).ToString() + ((char)Data[10]).ToString() + ((char)Data[11]).ToString() + ((char)Data[12]).ToString();
                difo.Model = model;
            }
            retCode = RetCode;
            return difo;
        }
        #endregion

        #region 验证密码

        #region 验证密码私有方法   private unsafe bool ValidatePassword(string pwd, DogConfig.PasswordType pwdType)
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="pwd">要验证的密码</param>
        /// <param name="pwdType">密码的类型</param>
        /// <returns></returns>
        private unsafe bool ValidatePassword(string pwd, DogConfig.PasswordType pwdType)
        {
            byte bDegree;
            RetCode = VerifyPassword(ulDogHandle, (byte)pwdType, pwd, &bDegree);
            if (RetCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region  验证用户密码   public unsafe bool VerifyUserPassword(string pwd)
        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="pwd">要验证密码</param>
        /// <returns>返回验证是否成功,true为成功</returns>
        public unsafe bool VerifyUserPassword(string pwd)
        {
            return ValidatePassword(pwd, DogConfig.PasswordType.RC_PASSWORDTYPE_USER);
        }
        #endregion

        #region  验证开发商密码  public unsafe bool VerifyDevPassword(string pwd)
        /// <summary>
        /// 验证开发商密码
        /// </summary>
        /// <param name="pwd">要验证的密码</param>
        /// <returns>返回验证是否成功,true为成功</returns>
        public unsafe bool VerifyDevPassword(string pwd)
        {
            return ValidatePassword(pwd, DogConfig.PasswordType.RC_PASSWORDTYPE_DEVELOPER);
        }
        #endregion

        #endregion
    }
}
