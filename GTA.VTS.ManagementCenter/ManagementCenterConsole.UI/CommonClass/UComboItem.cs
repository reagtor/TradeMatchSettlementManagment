using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// ComBoxItem类
    /// 作者：程序员：熊晓凌 修改：刘书伟
    /// 日期：2008-11-19  2009-10-30
    /// </summary>
    public class UComboItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UComboItem()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_TextTitleValue">文本</param>
        /// <param name="_ValueIndex">数值</param>
        public UComboItem(string _TextTitleValue, int _ValueIndex)
        {
            this.m_TextTitleValue = _TextTitleValue;
            this.m_ValueIndex = _ValueIndex;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_TextTitleValue">文本</param>
        /// <param name="_ValueStr">字符串值</param>
        public UComboItem(string _TextTitleValue, string _ValueStr)
        {
            this.m_TextTitleValue = _TextTitleValue;
            this.m_ValueStr = _ValueStr;
        }

        /// <summary>
        /// 值
        /// </summary>
        private string m_ValueStr = string.Empty;

        /// <summary>
        /// 值
        /// </summary>
        public string ValueStr
        {
            get { return m_ValueStr; }
            set { m_ValueStr = value; }
        }
        private string m_TextTitleValue = string.Empty;
        /// <summary>
        /// 显示名
        /// </summary>
        public string TextTitleValue
        {
            get { return m_TextTitleValue; }
            set { m_TextTitleValue = value; }
        }

        /// <summary>
        /// Index值
        /// </summary>
        private int m_ValueIndex = 0;
        /// <summary>
        /// Index值
        /// </summary>
        public int ValueIndex
        {
            get { return m_ValueIndex; }
            set { m_ValueIndex = value; }
        }
        /// <summary>
        /// 重载显示
        /// </summary>
        /// <returns>返回显示内容</returns>
        public override string ToString()
        {

            return ex ? ValueStr : TextTitleValue;
        }
        /// <summary>
        /// 返回标识
        /// </summary>
        public bool ex = false;


    }

    public class UComboItemCode : UComboItem
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public UComboItemCode()
            : base()
        {

        }
        /// <summary>
        /// 代码组合类
        /// </summary>
        /// <param name="_TextTitleValue">值的名称</param>
        /// <param name="_ValueIndex">值的索引值</param>
        public UComboItemCode(string _TextTitleValue, int _ValueIndex)
            : base(_TextTitleValue, _ValueIndex)
        {

        }
        /// <summary>
        ///代码组合类 
        /// </summary>
        /// <param name="_TextTitleValue">值的名称</param>
        /// <param name="_ValueStr">值对应的字符串</param>
        public UComboItemCode(string _TextTitleValue, string _ValueStr)
            : base(_TextTitleValue, _ValueStr)
        {

        }

        /// <summary>
        /// 开始标识
        /// </summary>
        private bool m_start_hasright;
        /// <summary>
        /// 结束标识
        /// </summary>
        private bool m_end_hasright;
        /// <summary>
        /// 代码来源哪个表
        /// </summary>
        private int m_CodeFormSource;

        /// <summary>
        /// 开始标识
        /// </summary>
        public bool Start_HasRight
        {
            get { return m_start_hasright; }
            set { m_start_hasright = value; }
        }

        /// <summary>
        /// 结束标识
        /// </summary>
        public bool End_Hasright
        {
            get { return m_end_hasright; }
            set { m_end_hasright = value; }
        }

        /// <summary>
        /// 代码来源那个表
        /// </summary>
        public int CodeFormSource
        {
            get { return m_CodeFormSource; }
            set { m_CodeFormSource = value; }
        }
    }

}
