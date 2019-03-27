using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementCenter.Model.CommonClass
{
    /// <summary>
    /// 生成规则帐号
    /// 作者：程序员：熊晓凌
    /// 日期：2008-11-19
    /// </summary>
    public class ProductionAccount
    {
        /// <summary>
        /// 根据柜台ID，用户ID，帐号类型生成规则帐号
        /// </summary>
        /// <param name="CounterID"></param>
        /// <param name="UserID"></param>
        /// <param name="AccountTypeID"></param>
        /// <returns></returns>
        public static string FormationAccount(int CounterID, int UserID, int AccountTypeID)
        {
            if (CounterID >= 100) return string.Empty;
            if (UserID > 99999999) return string.Empty;
            if (AccountTypeID >= 100) return string.Empty;

            string CounterIDstring = FormationLenth(CounterID, 2);
            string UserIDstring = FormationLenth(UserID, 8);
            string AccountTypeIDstring = FormationLenth(AccountTypeID, 2);

            string AccountID = CounterIDstring + UserIDstring + AccountTypeIDstring;

            return AccountID;
        }

        /// <summary>
        /// 把整形变成指定长度的字符串不足长度的前补O
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lenth"></param>
        /// <returns></returns>
        public static  string FormationLenth(int value, int lenth)
        {
            string str = string.Empty;
            if (value.ToString().Length >= lenth)
            {
                str = value.ToString();
            }
            else 
            {
                for (int i = 1; i <= (lenth - value.ToString().Length); i++)
                {
                    str = "0" + str;
                }
                str = str + value.ToString();
            }
            return str;
        }
   
    }
}
