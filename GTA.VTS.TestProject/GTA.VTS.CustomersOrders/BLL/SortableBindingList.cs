using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GTA.VTS.CustomersOrders.BLL
{
    /// <summary>
    /// Title: 可排序列表
    /// Create BY：董鹏
    /// Create date:2009-12-22
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortableBindingList<T> : BindingList<T>
    {
        /// <summary>
        /// 是否已排序
        /// </summary>
        private bool isSorted;
        /// <summary>
        /// 属性描述
        /// </summary>
        private PropertyDescriptor propertyDescriptor;
        /// <summary>
        /// 排序方向
        /// </summary>
        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SortableBindingList()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="t"></param>
        public SortableBindingList(IList<T> t)
            : base(t)
        {
        }

        /// <summary>
        /// 列表是否支持排序
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// 属性描述
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return propertyDescriptor; }
        }

        /// <summary>
        /// 排序方向
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        /// <summary>
        /// 获取一个值指示是否对列表排序。
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        /// <summary>
        /// 获取属性描述
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性描述</returns>
        private PropertyDescriptor GetPropertyDescriptor(string propertyName)
        {
            PropertyDescriptorCollection propertyCollection = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor item in propertyCollection)
            {
                if (item.Name == propertyName)
                {
                    return item;
                }
            }
            throw (new Exception("排序字段名不存在!"));
        }

        /// <summary>
        /// 开始进行排序
        /// </summary>
        /// <param name="prop">属性描述</param>
        /// <param name="direction">排序方向</param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            propertyDescriptor = prop;
            sortDirection = direction;
            InnerSort();
            isSorted = true;
        }

        /// <summary>
        /// 执行排序
        /// </summary>
        private void InnerSort()
        {
            List<T> list = (this.Items as List<T>);
            list.Sort(Compare);
            ResetBindings();
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int Compare(T x, T y)
        {
            if (sortDirection == ListSortDirection.Descending)
                return CompareValue(propertyDescriptor.GetValue(y), propertyDescriptor.GetValue(x));
            else
                return CompareValue(propertyDescriptor.GetValue(x), propertyDescriptor.GetValue(y));
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sortPropertyName">排序字段</param>
        /// <param name="isAscending">正序</param>
        public void Sort(string sortPropertyName, bool isAscending)
        {
            if (isAscending)
                sortDirection = ListSortDirection.Ascending;
            else
                sortDirection = ListSortDirection.Descending;
            propertyDescriptor = GetPropertyDescriptor(sortPropertyName);
            ApplySortCore(propertyDescriptor, sortDirection);
        }

        /// <summary>
        /// 比较对象
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        private static int CompareValue(object o1, object o2)
        {
            if (o1 == null) return o2 == null ? 0 : -1;
            else if (o2 == null) return 1;
            else if (o1 is IComparable) return ((IComparable)o1).CompareTo(o2);
            else if (o2 is IComparable) return ((IComparable)o2).CompareTo(o1);
            else return o1.ToString().CompareTo(o2.ToString());
        }
    }
}
