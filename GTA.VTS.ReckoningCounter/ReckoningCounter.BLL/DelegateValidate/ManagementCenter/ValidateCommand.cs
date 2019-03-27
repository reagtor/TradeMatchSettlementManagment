namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 代表一个校验动作
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    /// <typeparam name="T">需要校验的委托类型</typeparam>
    public abstract class ValidateCommand<T>
    {
        protected ValidateCommand(int breedClassID)
        {
            this.BreedClassID = breedClassID;
        }

        public int BreedClassID { get; private set; }

        /// <summary>
        /// 校验方法
        /// </summary>
        /// <param name="request">委托</param>
        /// <param name="strMessage">错误信息</param>
        /// <returns>是否通过校验</returns>
        public abstract bool Validate(T request, ref string strMessage);
    }
}