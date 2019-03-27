#region Using Namespace

using System;

#endregion

namespace GTA.VTS.Common.CommonObject
{
    /// <summary>
    /// 内部异常包装类
    /// </summary>
    public class VTException : Exception
    {
        public VTException(string code, string msg) : base(msg)
        {
            this.Code = code;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="CodeAndMessage">格式 --异常代码 : 异常消息</param>
        public VTException(string CodeAndMessage) :base(CodeAndMessage)
        {
            
        }

        public VTException(string code, string msg, Exception innerException) : base(msg, innerException)
        {
            this.Code = code;
        }

        public string Code { get; private set; }

        public override string ToString()
        {
            return this.Code + ":" + this.Message;
        }
    }
}