using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// Title:港股改单记录明细
    /// Desc.:本实体类只对已经改单成功后的新委托与被修改的委托单号作一一对应的记录。
    ///       不成功的记录应在委托记录请求表中查询，而委托表中记录的改单记录只是对已当前的委托
    ///       最后一次的改单委托
    /// 实体类HK_ModifyOrderDetailsInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create By:李健华
    /// Create Date:2009-11-10
    /// </summary>
    [Serializable]
    public class HK_ModifyOrderDetailsInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_ModifyOrderDetailsInfo()
        { }

        #region Model
        private int _id;
        private string _newRequestNumber;
        private string _originalRequestNumber;
        private int _modifytype;
        private DateTime _createdate;
        /// <summary>
        /// 主键
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 委托编号,新委托编号即改单生成的委托编号（这改单类型2时就有可能与ModifyOrderNumber相同)
        /// </summary>
        public string NewRequestNumber
        {
            set { _newRequestNumber = value; }
            get { return _newRequestNumber; }
        }
        /// <summary>
        /// 改单委托编号(即生成本记录时是由修改原始的那一笔委托单记录的委托编号)
        /// </summary>
        public string OriginalRequestNumber
        {
            set { _originalRequestNumber = value; }
            get { return _originalRequestNumber; }
        }
        /// <summary>
        /// 改单类型(1未报，2量减，3量价变)
        /// </summary>
        public int ModifyType
        {
            set { _modifytype = value; }
            get { return _modifytype; }
        }
        /// <summary>
        /// 创建记录时间(系统默认值)
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        #endregion Model

    }
}
