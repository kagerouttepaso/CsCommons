using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    /// <summary>
    /// 汎用の拡張メソッド
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// IEnumerable用のForEach
        /// </summary>
        /// <typeparam name="TSource">ソース</typeparam>
        /// <param name="source">list</param>
        /// <param name="action">action</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentException("list is null.");
            if (action == null) throw new ArgumentException("func is null.");
            foreach (var t in source)
            {
                action(t);
            }
        }        
        /// <summary>
        /// IEnumerable用のForEach(Breakつき)
        /// funcがtrueの間繰り返しを行います。
        /// </summary>
        /// <typeparam name="TSource">ソース</typeparam>
        /// <param name="source">list</param>
        /// <param name="func">func</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> func)
        {
            if (source == null) throw new ArgumentException("list is null.");
            if (a == null) throw new ArgumentException("func is null.");
            foreach (var t in source)
            {
                if (!a(t))
                {
                    break;
                }
            }

        }
    }
}
