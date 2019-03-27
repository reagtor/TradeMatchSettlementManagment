using System;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title:用户角色实体类
    /// Desc.:用户角色实体类BD_UserRoleType 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
    /// </summary>
    [Serializable]
    public class BD_UserRoleTypeInfo
    {
        /// <summary>
        /// 用户角色实体类构造函数 
        /// </summary>
        public BD_UserRoleTypeInfo()
        { }
        #region Model
        private int _rolenumber;
        private string _rolename;
        private string _remarks;
        /// <summary>
        /// 角色ID（主键）
        /// </summary>
        public int RoleNumber
        {
            set { _rolenumber = value; }
            get { return _rolenumber; }
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            set { _rolename = value; }
            get { return _rolename; }
        }
        /// <summary>
        /// 角色名称备注
        /// </summary>
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        #endregion Model

    }
}

