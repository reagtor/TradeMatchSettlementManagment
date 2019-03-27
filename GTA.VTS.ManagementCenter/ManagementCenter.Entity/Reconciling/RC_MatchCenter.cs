using System;
using System.Runtime.Serialization;

namespace ManagementCenter.Model
{
    /// <summary>
    /// 描述:撮合中心表 实体类RC_MatchCenter 。(属性说明自动提取数据库字段的描述信息)
    /// 作者：熊晓凌
    /// 日期：2008-11-18
    /// </summary>
    [DataContract]
    public class RC_MatchCenter
    {
        public RC_MatchCenter()
        { }
        #region Model
        private int _matchcenterid;
        private string _matchcentername;
        private string _ip;
        private int? _port;
        private int? _state;
        private string _xiadanservice;
        private string _cuoheservice;
        /// <summary>
        /// 撮合中心ID
        /// </summary>
        [DataMember]
        public int MatchCenterID
        {
            set { _matchcenterid = value; }
            get { return _matchcenterid; }
        }
        /// <summary>
        /// 撮合中心名称
        /// </summary>
        [DataMember]
        public string MatchCenterName
        {
            set { _matchcentername = value; }
            get { return _matchcentername; }
        }
        /// <summary>
        /// IP
        /// </summary>
        [DataMember]
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 端口
        /// </summary>
        [DataMember]
        public int? Port
        {
            set { _port = value; }
            get { return _port; }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        [DataMember]
        public int? State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 撮合服务
        /// </summary>
        [DataMember]
        public string CuoHeService
        {
            set { _cuoheservice = value; }
            get { return _cuoheservice; }
        }
        /// <summary>
        /// 下单服务
        /// </summary>
        [DataMember]
        public string XiaDanService
        {
            set { _xiadanservice = value; }
            get { return _xiadanservice; }
        }


        #endregion Model

    }
}

