#region Using Namespace

using System;
using System.Management;

#endregion

namespace GTA.VTS.Common.CommonUtility
{
    /// <summary>
    /// 公共功能类
    /// 作者：宋涛
    /// </summary>
    public static class CommUtils
    {
        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            ManagementClass mc;
            ManagementObjectCollection moc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            moc = mc.GetInstances();
            string str = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool) mo["IPEnabled"])
                    str = mo["MacAddress"].ToString();
            }
            return str;
        }

        //取CPU编号
        public static String GetCpuId()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }

        }
    }
}