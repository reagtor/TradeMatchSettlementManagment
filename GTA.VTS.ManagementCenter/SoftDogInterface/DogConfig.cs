using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftDogInterface
{

    /// <summary>
    /// Desc：密狗的相关配置类
    /// Create By：李健华
    /// Create Date:2009-06-10
    /// </summary>
    public class DogConfig
    {
        #region 密狗类型 public enum DogType : byte
        /// <summary>
        /// 密狗类型
        /// </summary>
        public enum DogType : byte
        {
            /// <summary>
            ///  本地狗
            /// </summary>
            RC_DOGTYPE_LOCAL = 1,
            /// <summary>
            /// 网络狗
            /// </summary>
            RC_DOGTYPE_NET = 2
        }
        #endregion

        #region 口令类型   public enum PasswordType : byte
        /// <summary>
        /// 口令类型
        /// </summary>
        public enum PasswordType : byte
        {
            /// <summary>
            /// 用户口令
            /// </summary>
            RC_PASSWORDTYPE_USER = 1,
            /// <summary>
            /// 开发商口令
            /// </summary>
            RC_PASSWORDTYPE_DEVELOPER = 2
        }
        #endregion

        #region  打开狗类型（即打开狗的标志） public enum OpenDogType : ulong
        /// <summary>
        /// 打开狗类型（即打开狗的标志）
        /// </summary>
        public enum OpenDogType : uint
        {
            /// <summary>
            /// 打开本地第一只硬件狗
            /// </summary>
            RC_OPEN_FIRST_IN_LOCAL = 1,
            /// <summary>
            /// 打开下一只硬件狗。在使用此标志前，应该先使用--打开本地第一只硬件狗
            /// </summary>
            RC_OPEN_NEXT_IN_LOCAL = 2,
            /// <summary>
            ///  只打开局域网中的硬件狗
            /// </summary>
            RC_OPEN_IN_LAN = 3,
            /// <summary>
            /// 优先打开本地硬件狗
            /// </summary>
            RC_OPEN_LOCAL_FIRST = 4,
            /// <summary>
            /// 优先打开局域网中的硬件狗
            /// </summary>
            RC_OPEN_LAN_FIRST = 5
        }
        #endregion

        #region  文件类型   public enum FileType : byte
        /// <summary>
        /// 文件类型
        /// </summary>
        public enum FileType : byte
        {
            /// <summary>
            /// 数据文件
            /// </summary>
            RC_TYPEFILE_DATA = 1,
            /// <summary>
            /// 许可证文件
            /// </summary>
            RC_TYPEFILE_LICENSE = 2,
            /// <summary>
            ///  算法文件
            /// </summary>
            RC_TYPEFILE_ALGORITHMS = 3
        }
        #endregion

        #region  密钥类型    public enum KeyType : byte
        /// <summary>
        /// 密钥类型
        /// </summary>
        public enum KeyType : byte
        {
            /// <summary>
            ///  加密或者解密密钥
            /// </summary>
            RC_KEY_AES = 1,
            /// <summary>
            ///  签名或检验密钥
            /// </summary>
            RC_KEY_SIGN = 2
        }
        #endregion

        #region  获取狗的信息 public struct DogInfo
        /// <summary>
        /// 获取狗的信息
        /// </summary>
        public struct DogInfo
        {
            /// <summary>
            /// 系列号
            /// </summary>
            public string SeriNO;
            /// <summary>
            /// 流水号
            /// </summary>
            public string CurrentNo;
            /// <summary>
            /// 狗类型
            /// </summary>
            public DogType Type;
            /// <summary>
            /// 
            /// </summary>
            public string Model;

        }
        #endregion

    }
}
