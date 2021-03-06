using System;
using System.Collections.Generic;



namespace Commons.Extensions
{
    /// <summary>
    /// IEnumerableの拡張メソッド
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// IEnumerable用のForEach
        /// </summary>
        /// <typeparam name="TSource">ソース</typeparam>
        /// <param name="Source">list</param>
        /// <param name="Action">action</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> Source, Action<TSource> Action)
        {
            if (Source == null) throw new ArgumentException("list is null.");
            if (Action == null) throw new ArgumentException("func is null.");
            foreach (var t in Source)
            {
                Action(t);
            }
        }

        /// <summary>
        /// IEnumerable用のForEach
        /// </summary>
        /// <typeparam name="TSource">ソース</typeparam>
        /// <param name="Source">list</param>
        /// <param name="Action">action</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> Source, Action<TSource, int> Action)
        {
            if (Source == null) throw new ArgumentException("list is null.");
            if (Action == null) throw new ArgumentException("func is null.");
            int i = 0;
            foreach (var t in Source)
            {
                Action(t, i);
                i++;
            }
        }

        /// <summary>
        /// IEnumerable用のForEach(Breakつき)
        /// funcがtrueの間繰り返しを行います。
        /// </summary>
        /// <typeparam name="TSource">ソース</typeparam>
        /// <param name="Source">list</param>
        /// <param name="Func">func</param>
        public static void ForEach<TSource>(this IEnumerable<TSource> Source, Func<TSource, bool> Func)
        {
            if (Source == null) throw new ArgumentException("list is null.");
            if (Func == null) throw new ArgumentException("func is null.");
            foreach (var t in Source)
            {
                if (!Func(t))
                {
                    break;
                }
            }
        }
    }
}