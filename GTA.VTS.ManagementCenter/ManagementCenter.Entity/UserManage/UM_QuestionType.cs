using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：问题类型表 实体类UM_QuestionType 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
	/// </summary>
    [DataContract]
	public class UM_QuestionType
	{
		public UM_QuestionType()
		{}
		#region Model
		private int _questionid;
		private string _content;
		/// <summary>
		/// 问题标识
		/// </summary>
        [DataMember]
		public int QuestionID
		{
			set{ _questionid=value;}
			get{return _questionid;}
		}
		/// <summary>
		/// 问题内容
		/// </summary>
        [DataMember]
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		#endregion Model

	}
}

