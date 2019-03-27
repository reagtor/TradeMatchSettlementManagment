#region Using Namespace

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using GTA.VTS.Common.CommonUtility;
using ReckoningCounter.PushBackTest.DoOrderService;

#endregion

namespace ReckoningCounter.PushBackTest
{
    public class Utils
    {
        /// <summary>
        /// 获取实时行情组件使用模式
        /// </summary>
        /// <returns>1.socket 2.fast</returns>
        public static int GetRealTimeMode()
        {
            int realTimeMode = 1;
            try
            {
                string setting = ConfigurationManager.AppSettings["realTimeMode"];
                if (!string.IsNullOrEmpty(setting))
                {
                    int count;
                    bool isSuccess = int.TryParse(setting.Trim(), out count);
                    if (isSuccess)
                        realTimeMode = count;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return realTimeMode;
        }

        /// <summary>
        /// 获取实时行情组件用户名
        /// </summary>
        /// <returns>实时行情组件用户名</returns>
        public static string GetRealTimeUserName()
        {
            string userName = "rtuser";
            try
            {
                string name = ConfigurationManager.AppSettings["ServerUserName"];
                if (!string.IsNullOrEmpty(name))
                {
                    userName = name;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return userName;
        }

        /// <summary>
        /// 获取实时行情组件密码
        /// </summary>
        /// <returns>实时行情组件密码</returns>
        public static string GetRealTimePassword()
        {
            string password = "11";
            try
            {
                string ps = ConfigurationManager.AppSettings["ServerPassword"];
                if (!string.IsNullOrEmpty(ps))
                {
                    password = ps;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }

            return password;
        }

        public static string GetOrderStateMsg(int type)
        {
            //OrderStateType state = OrderStateType.None;
            string msg = "无状态01";

            switch (type)
            {
                case 1:
                    //state = OrderStateType.None;
                    msg = "无状态01";
                    break;
                case 2:
                    //state = OrderStateType.DOSUnRequired;
                    msg = "未报02";
                    break;
                case 3:
                    //state = OrderStateType.DOSRequiredSoon;
                    msg = "待报03";
                    break;
                case 4:
                    //state = OrderStateType.DOSRequiredRemoveSoon;
                    msg = "已报待撤04";
                    break;
                case 5:
                    //state = OrderStateType.DOSIsRequired;
                    msg = "已报05";
                    break;
                case 6:
                    //state = OrderStateType.DOSCanceled;
                    msg = "废单06";
                    break;
                case 7:
                    //state = OrderStateType.DOSRemoved;
                    msg = "已撤07";
                    break;
                case 8:
                    //state = OrderStateType.DOSPartRemoved;
                    msg = "部撤08";
                    break;
                case 9:
                    //state = OrderStateType.DOSPartDealed;
                    msg = "部成09";
                    break;
                case 10:
                    //state = OrderStateType.DOSDealed;
                    msg = "已成10";
                    break;
                case 11:
                    //state = OrderStateType.DOSPartDealRemoveSoon;
                    msg = "部成待撤11";
                    break;
            }

            return msg;
        }

        public static TypesFutureOpenCloseType GetFutureOpenCloseType(string comboType)
        {
            TypesFutureOpenCloseType result = TypesFutureOpenCloseType.OpenPosition;
            try
            {
                string type = comboType.Substring(comboType.Length - 1, 1);
                int i = int.Parse(type);

                result = (TypesFutureOpenCloseType) i;
                //switch (type)
                //{
                //    case 1://FutureOpenCloseType.OpenPosition:
                //        result = "开仓";
                //        break;
                //    case 2://FutureOpenCloseType.ClosePosition:
                //        result = "平仓(历史)";
                //        break;
                //    case 3://FutureOpenCloseType.CloseTodayPosition:
                //        result = "平仓";
                //        break;
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        public static string GetFutureOpenCloseType(TypesFutureOpenCloseType type)
        {
            string result = "";
            try
            {
                switch (type)
                {
                    case TypesFutureOpenCloseType.OpenPosition:
                        result = "开仓1";
                        break;
                    case TypesFutureOpenCloseType.ClosePosition:
                        result = "平仓(历史)2";
                        break;
                    case TypesFutureOpenCloseType.CloseTodayPosition:
                        result = "平仓3";
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }
            return result;
        }

        public static GTA.VTS.Common.CommonObject.Types.UnitType GetUnit(string comboType)
        {
            GTA.VTS.Common.CommonObject.Types.UnitType result = GTA.VTS.Common.CommonObject.Types.UnitType.Thigh;

            try
            {
                string type = comboType.Substring(comboType.Length - 1, 1);
                int i = int.Parse(type);

                result = (GTA.VTS.Common.CommonObject.Types.UnitType)i;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.Message, ex);
            }


            return result;
        }

        public static bool IsFinalState(string comboType)
        {
            string type = comboType.Substring(comboType.Length - 2, 2);
            int state = int.Parse(type);
            //如果状态时废单，部撤，已撤，已成，那么代表是最终状态
            if (state == (int) TypesOrderStateType.DOSCanceled || state == (int) TypesOrderStateType.DOSPartRemoved
                || state == (int) TypesOrderStateType.DOSRemoved || state == (int) TypesOrderStateType.DOSDealed)
            {
                return true;
            }

            return false;
        }
    }

    public class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private PropertyDescriptor propertyDescriptor;
        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        public SortableBindingList()
        {
        }

        public SortableBindingList(IList<T> t) : base(t)
        {
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return propertyDescriptor; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        private PropertyDescriptor GetPropertyDescriptor(string propertyName)
        {
            bool findName = false;
            PropertyDescriptorCollection propertyCollection = TypeDescriptor.GetProperties(typeof (T));
            foreach (PropertyDescriptor item in propertyCollection)
            {
                if (item.Name == propertyName)
                {
                    findName = true;
                    return item;
                }
            }
            if (!findName)
            {
                throw (new Exception("排序字段名不存在!"));
            }
            else
                return null;
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            propertyDescriptor = prop;
            sortDirection = direction;
            InnerSort();
            isSorted = true;
        }

        private void InnerSort()
        {
            List<T> list = (this.Items as List<T>);
            list.Sort(Compare);
            ResetBindings();
        }

        private int Compare(T x, T y)
        {
            if (sortDirection == ListSortDirection.Descending)
                return CompareValue(propertyDescriptor.GetValue(y), propertyDescriptor.GetValue(x));
            else
                return CompareValue(propertyDescriptor.GetValue(x), propertyDescriptor.GetValue(y));
        }

        public void Sort(string sortPropertyName, bool isAscending)
        {
            if (isAscending)
                sortDirection = ListSortDirection.Ascending;
            else
                sortDirection = ListSortDirection.Descending;
            propertyDescriptor = GetPropertyDescriptor(sortPropertyName);
            ApplySortCore(propertyDescriptor, sortDirection);
        }

        private static int CompareValue(object o1, object o2)
        {
            if (o1 == null) return o2 == null ? 0 : -1;
            else if (o2 == null) return 1;
            else if (o1 is IComparable) return ((IComparable) o1).CompareTo(o2);
            else if (o2 is IComparable) return ((IComparable) o2).CompareTo(o1);
            else return o1.ToString().CompareTo(o2.ToString());
        }
    }
}