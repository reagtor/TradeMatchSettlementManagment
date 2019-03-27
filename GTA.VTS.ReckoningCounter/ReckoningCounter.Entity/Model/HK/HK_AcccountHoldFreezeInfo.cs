using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReckoningCounter.Entity.Model.HK
{
    /// <summary>
    /// 港股持仓冻结实体类HK_AcccountHoldFreezeInfo 。(属性说明自动提取数据库字段的描述信息)
    /// Create BY：李健华
    /// Create Date：2009-10-15
    /// </summary>
    //[Serializable]
    public class HK_AcccountHoldFreezeInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HK_AcccountHoldFreezeInfo()
        { }
        #region Model
        private int _holdFreezeLogoId;
        private string _entrustnumber;
        private int _prepareFreezeAmount;
        private int _freezetypeid;
        private int _accountHoldLogo;
        private DateTime? _thawtime;
        private DateTime _freezetime;
        /// <summary>
        /// 港股持仓账号冻结记录主键
        /// </summary>
        public int HoldFreezeLogoId
        {
            set { _holdFreezeLogoId = value; }
            get { return _holdFreezeLogoId; }
        }
        /// <summary>
        /// 委托单号
        /// </summary>
        public string EntrustNumber
        {
            set { _entrustnumber = value; }
            get { return _entrustnumber; }
        }
        /// <summary>
        /// 准备冻结总量
        /// </summary>
        public int PrepareFreezeAmount
        {
            set { _prepareFreezeAmount = value; }
            get { return _prepareFreezeAmount; }
        }
        /// <summary>
        /// 冻结类型ID(外键BD_FreezeType)
        /// </summary>
        public int FreezeTypeID
        {
            set { _freezetypeid = value; }
            get { return _freezetypeid; }
        }
        /// <summary>
        /// 所拥有持仓ID(HK_AccountHold)，这是对所持仓的冻结
        /// </summary>
        public int AccountHoldLogo
        {
            set { _accountHoldLogo = value; }
            get { return _accountHoldLogo; }
        }
        /// <summary>
        /// 解冻时间
        /// </summary>
        public DateTime? ThawTime
        {
            set { _thawtime = value; }
            get { return _thawtime; }
        }
        /// <summary>
        /// 冻结时间
        /// </summary>
        public DateTime FreezeTime
        {
            set { _freezetime = value; }
            get { return _freezetime; }
        }
        #endregion Model

    }
}
