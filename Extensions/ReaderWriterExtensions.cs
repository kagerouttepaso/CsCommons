using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    /// <summary>
    /// ReaderWriterSlim用の拡張メソッド
    /// </summary>
    public static class ReaderWriterSlimExtensions
    {
        /// <summary>
        /// スレッドセーフな値書込
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rwl"></param>
        /// <param name="dstData"></param>
        /// <param name="srcData"></param>
        public static void WriteValue<T>(this ReaderWriterLockSlim rwl, ref T dstData, T srcData)
            where T : struct
        {
            rwl.EnterUpgradeableReadLock();
            try
            {
                if (dstData.Equals(srcData)) return;
                rwl.EnterWriteLock();
                try
                {
                    dstData = srcData;
                }
                finally
                {
                    rwl.ExitWriteLock();
                }
            }
            finally
            {
                rwl.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// スレッドセーフな値読み込み
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rwl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ReadValue<T>(this ReaderWriterLockSlim rwl, ref T data)
            where T : struct
        {
            T ret;
            rwl.EnterReadLock();
            try
            {
                ret = data;
            }
            finally
            {
                rwl.ExitReadLock();
            }
            return ret;
        }

        /// <summary>
        /// ReadLockを掛けてメソッド実行
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static void ReadMethod(this ReaderWriterLockSlim rwl, Action method)
        {
            rwl.EnterReadLock();
            try
            {
                method();
            }
            finally
            {
                rwl.ExitReadLock();
            }
        }

        /// <summary>
        /// ReadLockを掛けてメソッド実行(非同期)
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static async Task ReadMethodAsync(this ReaderWriterLockSlim rwl, Action method)
        {
            await Task.Factory.StartNew(() => ReadMethod(rwl, method));
        }

        /// <summary>
        /// ReadLockを掛けてメソッド実行
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static T ReadMethod<T>(this ReaderWriterLockSlim rwl, Func<T> method)
        {
            rwl.EnterReadLock();
            try
            {
                return method();
            }
            finally
            {
                rwl.ExitReadLock();
            }
        }

        /// <summary>
        /// ReadLockを掛けてメソッド実行(非同期)
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static async Task<T> ReadMethodAsync<T>(this ReaderWriterLockSlim rwl, Func<T> method)
        {
            return await Task.Factory.StartNew(() => ReadMethod(rwl, method));
        }

        /// <summary>
        /// WriteLockを掛けてメソッド実行
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static void WriteMethod(this ReaderWriterLockSlim rwl, Action method)
        {
            rwl.EnterWriteLock();
            try
            {
                method();
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        /// <summary>
        /// WriteLockを掛けてメソッド実行(非同期)
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static async Task WriteMethodAsync(this ReaderWriterLockSlim rwl, Action method)
        {
            await Task.Factory.StartNew(() => WriteMethod(rwl, method));
        }

        /// <summary>
        /// WriteLockを掛けてメソッド実行
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static T WriteMethod<T>(this ReaderWriterLockSlim rwl, Func<T> method)
        {
            rwl.EnterWriteLock();
            try
            {
                return method();
            }
            finally
            {
                rwl.ExitWriteLock();
            }
        }

        /// <summary>
        /// WriteLockを掛けてメソッド実行(非同期)
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="method"></param>
        public static async Task<T> WriteMethodAsync<T>(this ReaderWriterLockSlim rwl, Func<T> method)
        {
            return await Task.Factory.StartNew(() => WriteMethod(rwl, method));
        }

        /// <summary>
        /// ロックを掛けて比較の後メソッドを実行
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="Compare"></param>
        /// <param name="method"></param>
        public static bool UpgradeableReadMethod(this ReaderWriterLockSlim rwl, Func<bool> Compare, Action method)
        {
            rwl.EnterUpgradeableReadLock();
            try
            {
                if (Compare())
                {
                    rwl.EnterWriteLock();
                    try
                    {
                        method();
                    }
                    finally
                    {
                        rwl.ExitWriteLock();
                    }
                    return true;
                }
                else {
                    return false;
                }
            }
            finally
            {
                rwl.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// ロックを掛けて比較の後メソッドを実行(非同期)
        /// </summary>
        /// <param name="rwl"></param>
        /// <param name="compare"></param>
        /// <param name="method"></param>
        public static async Task<bool> UpgradeableReadMethodAsync(this ReaderWriterLockSlim rwl, Func<bool> compare, Action method)
        {
            return await Task.Factory.StartNew(() => UpgradeableReadMethod(rwl, compare, method));
        }
    }
}
