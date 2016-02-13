using System.Threading;

namespace Commons.Threading
{
    /// <summary>
    /// スレッドセーフなBool値
    /// </summary>
    public class ConcurrentBool{

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="b">初期値</param>
        public ConcurrentBool(bool b):this()
        {
            _flag = b;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConcurrentBool() { }

        private ReaderWriterLockSlim _flagLock;
        /// <summary>
        /// フラグ用のReaderWriterLock
        /// </summary>
        private ReaderWriterLockSlim FlagLock
        {
            get
            {
                return _flagLock ?? (_flagLock = new ReaderWriterLockSlim());
            }
        }

        private bool _flag;
        /// <summary>
        /// フラグ値(スレッドセーフ)
        /// </summary>
        public bool Flag
        {
            get {
                FlagLock.EnterReadLock();
                try
                {
                   return _flag;
                }
                finally
                {
                    FlagLock.ExitReadLock();
                }
            }
            set
            {
                FlagLock.EnterUpgradeableReadLock();
                try
                {
                    if (_flag != value)
                    {
                        FlagLock.EnterWriteLock();
                        try
                        {
                            _flag = value;
                        }
                        finally
                        {
                            FlagLock.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    FlagLock.ExitUpgradeableReadLock();
                }
            }
        }

        /// <summary>
        /// if(this Class)をするためのオペレータ
        /// </summary>
        /// <param name="b">this</param>
        /// <returns>this.Flag</returns>
        public static bool operator true(ConcurrentBool b) { return b.Flag; }
        /// <summary>
        /// if(this Class)をするためのオペレータ
        /// </summary>
        /// <param name="b">this</param>
        /// <returns>this.Flag</returns>
        public static bool operator false(ConcurrentBool b) { return b.Flag; }
        /// <summary>
        /// bool型への暗黙的な型キャスト
        /// </summary>
        /// <param name="b">this Class</param>
        /// <returns>bool</returns>
        public static implicit operator bool(ConcurrentBool b) { return b.Flag; }
    }
}
