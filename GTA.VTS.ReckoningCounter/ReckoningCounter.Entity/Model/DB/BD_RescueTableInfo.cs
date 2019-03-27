using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title:分红相关数据表
    /// Desc.:分红相关数据表BD_RescueTable 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
    /// </summary>
    public class BD_RescueTableInfo
    {
        /// <summary>
        /// 分红相关数据表构造函数 
        /// </summary>
        public BD_RescueTableInfo()
		{}
		#region Model
		private int _id;
		private int _type;
		private string _value1;
		private string _value2;
		private string _value3;
		private string _value4;
		private string _value5;
        private string _value6;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Value1
		{
			set{ _value1=value;}
			get{return _value1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Value2
		{
			set{ _value2=value;}
			get{return _value2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Value3
		{
			set{ _value3=value;}
			get{return _value3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Value4
		{
			set{ _value4=value;}
			get{return _value4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Value5
		{
			set{ _value5=value;}
			get{return _value5;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string Value6
        {
            set { _value6 = value; }
            get { return _value6; }
        }
		#endregion Model

    }
}
