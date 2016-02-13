using System;
using System.Collections.Generic;



namespace Commons.Extensions
{

    /// <summary>
    /// int拡張
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// RubyのTakesの実装
        /// </summary>
        /// <typeparam name="Tresult">戻り値の型</typeparam>
        /// <param name="count">繰り返す回数</param>
        /// <param name="func">実行する関数</param>
        /// <returns></returns>
        public static IEnumerable<Tresult> Takes<Tresult>(this int count, Func<int, Tresult> func)
        {
            for (int i = 0; i < count; i++)
            {
                yield return func(i);
            }
        }

        /// <summary>
        /// RubyのTakesの実装
        /// </summary>
        /// <param name="count">繰り返す回数</param>
        /// <param name="action">実行する関数</param>
        public static void Takes(this int count, Action<int> action)
        {
            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }
    }
}
