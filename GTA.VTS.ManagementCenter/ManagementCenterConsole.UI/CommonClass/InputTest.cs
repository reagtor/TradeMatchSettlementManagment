using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 描述：输入校验
    /// 作者：程序员;熊晓凌 修改：刘书伟
    /// 日期：2008-11-19  修改日期：2009-11-06
    /// </summary>
    public class InputTest
    {
        /// <summary>
        /// 验证字符串是否包含非正常字符
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool keyTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^(?:[\u4e00-\u9fa5]*\w*\s*)+$");
        }

        /// <summary>
        /// 验证是否合法用户名(只包括数字、字母、以及下划线和－)
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        public static bool LoginTest(string loginName)
        {
            return Regex.IsMatch(loginName, @"^[A-Za-z0-9_-]+$");
        }

        /// <summary>
        /// 验证字符串是否合法email地址
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool emailTest(string keyWords)
        {
            string s =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Match m = Regex.Match(keyWords, s);
            if (!m.Success)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证字符串是否为正整数
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool intTest(string keyWords)
        {
            string s = "^[0-9]*[1-9][0-9]*$";
            Match m = Regex.Match(keyWords, s);
            if (!m.Success)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证IP是否合法
        /// </summary>
        /// <param name="StrIp"></param>
        /// <returns></returns>
        public static bool IPTest(string StrIp)
        {
            string[] ip = StrIp.Split('.');
            if (ip.Length != 4)
            {
                return false;
            }
            for (int i = 0; i <= 3; i++)
            {
                try
                {
                    int k = int.Parse(ip[i]);
                    if (k < 0 || k > 255)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        #region 新增检验方法[刘书伟]
        /// <summary>
        /// 验证是否是合法的小数点格式(可包含数字及小数点)
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool DecimalTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9.]+$");
        }

        /// <summary>
        /// 验证是否是合法期货交易代码(只包括大写字母)
        /// </summary>
        /// <param name="tradeCode">期货交易代码</param>
        /// <returns></returns>
        public static bool FetureTradeCodeTest(string tradeCode)
        {
            return Regex.IsMatch(tradeCode, @"^[A-Z]+$");
        }

        /// <summary>
        /// 验证零开头的整数
        /// </summary>
        /// <param name="keyWords">需要验证的字符串</param>
        /// <returns></returns>
        public static bool zeroStartIntTest(string keyWords)
        {
            return Regex.IsMatch(keyWords, @"^[0-9]+$");
            //return Regex.IsMatch(keyWords, @"^[A-Z0-9]+$");
        }

        /// <summary>
        /// 验证现货（不包括港股）和期货的代码输入(验证零开头的整数)
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public static bool zeroXHAndQHStartIntTest(string keyWords)
        {
            // return Regex.IsMatch(keyWords, @"^[0-9]+$");
            return Regex.IsMatch(keyWords, @"^[A-Z0-9]+$");
        }

        /// <summary>
        /// 邮政编码 6个数字
        /// </summary>
        /// <param name="postCode">邮编</param>
        /// <returns></returns>
        public static bool IsPostCode(string postCode)
        {
            return Regex.IsMatch(postCode, @"^\d{6}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是手机号码及座机号码
        /// </summary>
        /// <param name="mobileOrTelephone">手机号码及座机号码</param>
        ///(1)电话号码由数字、和"-"构成 
        ///(2)电话号码为7到8位 
        ///(3)如果电话号码中包含有区号，那么区号为三位或四位 
        ///(4)区号用"-"和其他部分隔开 
        ///(5)移动电话号码为11或12位，如果为12位,那么第一位为0 
        ///(6)11位移动电话号码的第一位和第二位为"13" ，或"15"，或"18"
        ///(7)12位移动电话号码的第二位和第三位为"13" ，或"15"，或"18"

        public static bool IsMobileOrTelephone(string mobileOrTelephone)
        {
            return Regex.IsMatch(mobileOrTelephone, @"(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{7,8}$)|(^0{0,1}1[358][0-9]{9}$)", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证15位或18位的身份证号码
        /// </summary>
        /// <param name="statusCard">身份证号码</param>
        /// <returns></returns>
        public static bool IsStatusCard(string statusCard)
        {
            statusCard.ToLower();
            return Regex.IsMatch(statusCard, @"(^\d{15}$)|(^\d{17}([0-9]|X)$)");
        }

        #endregion
    }
}