using System;
namespace ReckoningCounter.Model
{
    /// <summary>
    /// Title:账号类别实体类
    /// Desc.:账号类别实体类DB_AccountTypeClass 。(属性说明自动提取数据库字段的描述信息)
    /// Create by:李健华
    /// Create date:2009-07-08
    /// </summary>
    [Serializable]
    public class DB_AccountTypeClassInfo
    {
        /// <summary>
        /// 账号类别实体类构造函数 
        /// </summary>
        public DB_AccountTypeClassInfo()
        { }
        #region Model
        private int _atcid;
        private string _atcname;
        /// <summary>
        /// 账户类型分类ID
        /// </summary>
        public int ATCId
        {
            set { _atcid = value; }
            get { return _atcid; }
        }
        /// <summary>
        /// 账户类型分类名称
        /// </summary>
        public string ATCName
        {
            set { _atcname = value; }
            get { return _atcname; }
        }
        #endregion Model

    }
}

