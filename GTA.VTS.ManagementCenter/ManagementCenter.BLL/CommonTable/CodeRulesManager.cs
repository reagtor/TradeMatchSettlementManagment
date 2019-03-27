using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.VTS.Common.CommonObject;

namespace ManagementCenter.BLL.CommonTable
{

    /// <summary>
    /// 代码规则管理
    /// </summary>
    public class CodeRulesManager
    {
        /// <summary>
        /// 声明一个IMessageStrategy类型
        /// </summary>
        private ICodeRulesStrategy _strategy;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strategy">具体算法</param>
        public CodeRulesManager(ICodeRulesStrategy strategy)
        {
            if (strategy != null)
            {
                this._strategy = strategy;
            }
            else
            {
                this._strategy = new CodeRulesDefault();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codeRulesType">代码规则类型</param>
        public CodeRulesManager(Types.CodeRulesType codeRulesType)
        {
            switch (codeRulesType)
            {
                case Types.CodeRulesType.SixFigures:
                    this._strategy = new CodeRulesSixFigures();
                    break;
                case Types.CodeRulesType.FiveFigures:
                    this._strategy = new CodeRulesFiveFigures();
                    break;
                case Types.CodeRulesType.PrefixWithTwoDigitYearTwoDigitMonth:
                    this._strategy = new CodeRulesTwoDigitYear();
                    break;
                case Types.CodeRulesType.PrefixWithOneDigitYearTwoDigitMonth:
                    this._strategy = new CodeRulesOneDigitYear();
                    break;
                default:
                    this._strategy = new CodeRulesDefault();
                    break;
            }
        }

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetCode(params string[] Params)
        {
            return _strategy.GetCode(Params);
        }

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void GetFutureYearMonth(string code, out int year, out int month)
        {
            _strategy.GetFutureYearMonth(code, out year, out month);
        }
    }

    /// <summary>
    /// 代码规则策略接口
    /// </summary>
    public interface ICodeRulesStrategy
    {
        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        string GetCode(params string[] Params);

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        void GetFutureYearMonth(string code, out int year, out int month);
    }

    /// <summary>
    /// 6位数字代码
    /// </summary>
    public class CodeRulesSixFigures : ICodeRulesStrategy
    {
        #region ICodeRulesStrategy 成员

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetCode(params string[] Params)
        {
            if (Params[0].Length == 6)
            {
                return Params[0];
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void GetFutureYearMonth(string code, out int year, out int month)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 5位数字代码
    /// </summary>
    public class CodeRulesFiveFigures : ICodeRulesStrategy
    {
        #region ICodeRulesStrategy 成员

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetCode(params string[] Params)
        {
            if (Params[0].Length == 5)
            {
                return Params[0];
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void GetFutureYearMonth(string code, out int year, out int month)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 品种代码+2位年份+2位月份
    /// </summary>
    public class CodeRulesTwoDigitYear : ICodeRulesStrategy
    {
        #region ICodeRulesStrategy 成员

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetCode(params string[] Params)
        {
            if (Params[1].Length != 2)
            {
                Params[1] = Params[1].Substring(Params[1].Length - 2);
            }
            if (Params[2].Length != 2)
            {
                Params[2] = Params[2].Substring(Params[2].Length - 2);
            }
            return Params[0] + Params[1] + Params[2];
        }

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void GetFutureYearMonth(string code, out int year, out int month)
        {
            string str = code.Substring(code.Length - 4);
            string yearStr = str.Substring(0, 2);
            yearStr = DateTime.Now.Year.ToString().Substring(0, 2) + yearStr;
            string monthStr = str.Substring(2, 2);

            year = int.Parse(yearStr);
            month = int.Parse(monthStr);
        }

        #endregion
    }

    /// <summary>
    /// 品种代码+1位年份+2位月份
    /// </summary>
    public class CodeRulesOneDigitYear : ICodeRulesStrategy
    {
        #region ICodeRulesStrategy 成员

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetCode(params string[] Params)
        {
            if (Params[1].Length != 1)
            {
                Params[1] = Params[1].Substring(Params[1].Length - 1);
            }
            if (Params[2].Length != 2)
            {
                Params[2] = Params[2].Substring(Params[2].Length - 2);
            }
            return Params[0] + Params[1] + Params[2];
        }

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void GetFutureYearMonth(string code, out int year, out int month)
        {
            string str = code.Substring(code.Length - 3);
            string yearStr = str.Substring(0, 1);
            yearStr = DateTime.Now.Year.ToString().Substring(0, 3) + yearStr;
            string monthStr = str.Substring(1, 2);

            year = int.Parse(yearStr);
            month = int.Parse(monthStr);
        }

        #endregion
    }

    /// <summary>
    /// 其它情况，直接返回原值
    /// </summary>
    public class CodeRulesDefault : ICodeRulesStrategy
    {
        #region ICodeRulesStrategy 成员

        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        public string GetCode(params string[] Params)
        {
            string code = "";
            for (int i = 0; i < Params.Length; i++)
            {
                if (!string.IsNullOrEmpty(Params[i]))
                {
                    code += Params[i];
                }
            }
            return code;
        }

        /// <summary>
        /// 获取期货合约年月
        /// </summary>
        /// <param name="code">合约代码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        public void GetFutureYearMonth(string code, out int year, out int month)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



}
