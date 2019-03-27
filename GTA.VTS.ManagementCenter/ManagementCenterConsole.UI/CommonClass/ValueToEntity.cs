using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagementCenter.Model.CommonClass;
namespace ManagementCenterConsole.UI.CommonClass
{
    /// <summary>
    /// 描述：值转换实体
    /// 作者：熊晓凌
    /// 日期：2008-11-19
    /// </summary>
    public static class ValueToEntity
    {
        /// <summary>
        /// 获取值并转换实体
        /// </summary>
        /// <param name="entityAttribute">实体属性</param>
        /// <param name="controlValue">值</param>
        public static void GetValueToEntity(object entityAttribute, object controlValue)
        {
            if (controlValue == null || string.IsNullOrEmpty(controlValue.ToString()))
            {
                switch (entityAttribute.GetType().ToString())
                {
                    case "Int32":
                        entityAttribute = AppGlobalVariable.INIT_INT;
                        break;
                    case "decimal":
                        entityAttribute = AppGlobalVariable.INIT_DECIMAL;
                        break;
                }
            }
            else
            {

                entityAttribute = controlValue;
            }
        }

    }
}
