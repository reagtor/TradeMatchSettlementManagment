using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ManagementCenter.Model.CommonClass
{
    /// <summary>
    /// 描述:实体拷贝
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    public class UtilityClass
    {
        /// <summary>
        /// 实体拷贝.
        /// 拷贝值类型和非数组类型．
        /// </summary>
        /// <param name="entityFrom">源实体类</param>
        /// <param name="entityTo">目标实体类</param>
        public static void CopyEntityToEntity(object entityFrom, object entityTo)
        {
            if (entityFrom == null || entityTo == null) return;
            Type typeFrom = entityFrom.GetType();
            Type typeTo = entityTo.GetType();

            PropertyInfo[] propertys = typeFrom.GetProperties();
            foreach (PropertyInfo pInfo in propertys)
            {
                PropertyInfo pInfoTo = typeTo.GetProperty(pInfo.Name);
                //不为空且不为数组类型时赋值.
                if (pInfoTo != null && !pInfoTo.PropertyType.IsArray)
                {
                    object obj = pInfo.GetValue(entityFrom, null);
                    if (obj == null) continue;
                    //为值类型或者类型相同时赋值．
                    if (obj.GetType().IsValueType || obj.GetType().Equals(pInfoTo.PropertyType))
                    {
                        pInfoTo.SetValue(entityTo, obj, null);
                    }
                }
            }
        }

        public static string Key = "GTA-2008";

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="sDecrKey"></param>
        /// <returns></returns>
        public static string DesDecrypt(string strText, string sDecrKey)
        {
            sDecrKey = Key;
            byte[] rgbKey = null;
            byte[] rgbIV = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xef, 0xcd };
            byte[] buffer = new byte[strText.Length];
            try
            {
                rgbKey = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                buffer = Convert.FromBase64String(strText);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                Encoding encoding = new UTF8Encoding();
                return encoding.GetString(stream.ToArray());
            }
            catch (Exception exception)
            {
                return ("error:" + exception.Message + "\r");
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="strEncrKey"></param>
        /// <returns></returns>
        public static string DesEncrypt(string strText, string strEncrKey)
        {
            strEncrKey = Key;
            byte[] rgbKey = null;
            byte[] rgbIV = new byte[] { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xef, 0xcd };
            try
            {
                rgbKey = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, strEncrKey.Length));
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                byte[] bytes = Encoding.UTF8.GetBytes(strText);
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception exception)
            {
                return ("error:" + exception.Message + "\r");
            }
        }

    }
}
