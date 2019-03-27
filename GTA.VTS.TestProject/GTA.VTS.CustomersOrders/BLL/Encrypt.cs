using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// 描述:对用户输入的密码或从数据库中获取的密码进行加密和解密
    /// 作者：叶振东
    /// 日期：2009-12-22
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// 设置一个加密和解密Key值
        /// </summary>
        public static string Key = "GTA-2008";

        /// <summary>
        /// 对用户在文本框输入的值进行加密
        /// </summary>
        /// <param name="strText">获取的文本框值</param>
        /// <param name="strEncrKey"></param>
        /// <returns>加密后的值</returns>
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

        /// <summary>
        /// 对用户获取的加密过的数据进行解密
        /// </summary>
        /// <param name="strText">获取的加密过的数据</param>
        /// <param name="sDecrKey"></param>
        /// <returns>解密后的数据</returns>
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
    }
}
