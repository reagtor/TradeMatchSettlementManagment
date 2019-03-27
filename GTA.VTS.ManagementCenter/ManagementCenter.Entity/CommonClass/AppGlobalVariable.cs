using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ManagementCenter.Model.CommonClass
{
    /// <summary>
    /// 描述：全局变量
    /// 作者：刘书伟
    /// 日期：2008-12-11
    /// </summary>
    public class AppGlobalVariable
    {
        /// <summary>
        /// 初始化INT的值
        /// </summary>
        public const int INIT_INT = Int32.MaxValue;              // Int型变量初始值

        /// <summary>
        /// 初始化LONG的值
        /// </summary>
        public const Int64 INIT_LONG = Int64.MaxValue;        // Long型变量初始值

        /// <summary>
        /// 初始化FLOAT的值
        /// </summary>
        public const float INIT_FLOAT = float.MaxValue;       // float型变量初始值

        /// <summary>
        /// 初始化DOUBLE的值
        /// </summary>
        public const Double INIT_DOUBLE = Double.MaxValue;         // DOUBLE型变量初始值

        /// <summary>
        /// DATETIME型变量初始值
        /// </summary>
        private static DateTime m_DataTime = DateTime.MaxValue;
        /// <summary>
        /// DATETIME型变量初始值
        /// </summary>
        public static DateTime INIT_DATETIME     //DATETIME型变量初始值
        {
            get
            {
                return m_DataTime;
            }
        }

        /// <summary>
        /// DataTable类型的初始化
        /// </summary>
        public const DataTable INIT_DATATABLE = null;           // DataTable类型的初始化

        /// <summary>
        /// decimal类型的初始化
        /// </summary>
        public const decimal INIT_DECIMAL = decimal.MaxValue;      //decimal类型的初始化

        /// <summary>
        /// STRING型变量初始值
        /// </summary>
        public const string INIT_STRING = null;               //STRING型变量初始值

        /// <summary>
        /// byte[] 数组类型初始化
        /// </summary>
        public const byte[] INIT_BYTEARRAY = null;           //byte[] 数组类型初始化

    }
}