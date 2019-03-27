#region Using Namespace

using System.Collections.Generic;
using GTA.VTS.Common.CommonUtility;

#endregion

namespace ReckoningCounter.BLL.DelegateValidate.ManagementCenter
{
    /// <summary>
    /// 校验规则容器的基类
    /// 作者：宋涛
    /// 日期：2008-11-20
    /// </summary>
    /// <typeparam name="T">委托类型</typeparam>
    public abstract class RuleContainer<T>
    {
        private IList<ValidateCommand<T>> m_CommandList;

        protected RuleContainer(int breedClassid)
        {
            this.BreedClassID = breedClassid;
            m_CommandList = new List<ValidateCommand<T>>();
            FillCommandList();
        }

        public int BreedClassID { get; private set; }

        /// <summary>
        /// 填充校验命令列表
        /// </summary>
        protected abstract void FillCommandList();

        /// <summary>
        /// 添加一个校验命令
        /// </summary>
        /// <param name="command"></param>
        protected void AddCommand(ValidateCommand<T> command)
        {
            this.m_CommandList.Add(command);
        }

        /// <summary>
        /// 校验总入口
        /// </summary>
        /// <param name="request">委托</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>是否校验成功</returns>
        public virtual bool ValidateAllRules(T request, ref string errMsg)
        {
            foreach (ValidateCommand<T> command in m_CommandList)
            {
                bool result = false;

                try
                {
                    result = command.Validate(request, ref errMsg);
                }
                catch (System.Exception ex)
                {
                    LogHelper.WriteError(ex.Message,ex);
                }

                if (!result)
                    return false;
            }

            return true;
        }
    }
}