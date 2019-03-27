using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GTA.VTS.Common.CommonObject
{
    /// <summary>
    /// 同步的列表类
    /// 作者：宋涛
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SyncList<T>
    {
        protected ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();
        protected List<T> innerList = new List<T>();

        public List<T> GetAllAndClear()
        {
            listLock.EnterWriteLock();
            try
            {
                List<T> returnList = new List<T>();

                foreach (var t in innerList)
                {
                    returnList.Add(t);
                }

                innerList.Clear();

                return returnList;
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        public List<T> GetAll()
        {
            listLock.EnterReadLock();
            try
            {
                return innerList;
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }

        public bool Contains(T t)
        {
            listLock.EnterReadLock();
            try
            {
                return innerList.Contains(t);
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }

        public void Add(T t)
        {
            listLock.EnterWriteLock();
            try
            {
                innerList.Add(t);
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        public bool AddWithTimeout(T t, int timeout)
        {
            if (listLock.TryEnterWriteLock(timeout))
            {
                try
                {
                    innerList.Add(t);
                }
                finally
                {
                    listLock.ExitWriteLock();
                }
                return true;
            }

            return false;
        }

        public void AddRange(List<T> list)
        {
            listLock.EnterWriteLock();
            try
            {
                innerList.AddRange(list);
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        public void Delete(T t)
        {
            listLock.EnterWriteLock();
            try
            {
                innerList.Remove(t);
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        public void Reset()
        {
            listLock.EnterWriteLock();
            try
            {
                innerList.Clear();
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }
    }
}
