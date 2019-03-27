using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
	/// <summary>
    /// 描述：管理员组可用功能表 实体类UM_ManagerGroupFunctions 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
	public class UM_ManagerGroupFunctions
	{
		public UM_ManagerGroupFunctions()
		{}
		#region Model
		private int _managegroupfuntctiosid;
		private int? _functionid;
		private int? _managergroupid;

	    private string _functionname;
		/// <summary>
		/// 管理组可用功能标识
		/// </summary>
        [DataMember]
		public int ManageGroupFuntctiosID
		{
			set{ _managegroupfuntctiosid=value;}
			get{return _managegroupfuntctiosid;}
		}
		/// <summary>
		/// 用户可用的功能表ID号
		/// </summary>
        [DataMember]
		public int? FunctionID
		{
			set{ _functionid=value;}
			get{return _functionid;}
		}
		/// <summary>
		/// 管理员组ID号
		/// </summary>
        [DataMember]
		public int? ManagerGroupID
		{
			set{ _managergroupid=value;}
			get{return _managergroupid;}
		}

        /// <summary>
        /// 功能描述
        /// </summary>
        [DataMember]
        public string FunctionName
        {
            set { _functionname = value; }
            get { return _functionname; }
        }
		#endregion Model

	}
}

