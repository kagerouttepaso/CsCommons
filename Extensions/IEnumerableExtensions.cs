using System;
using System.Collections.Generic;



namespace Commons.Extensions
{
    /// <summary>
    /// IEnumerable�̊g�����\�b�h
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// IEnumerable�p��ForEach
        /// </summary>
        /// <typeparam name="TSource">�\�[�X</typeparam>
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
        /// IEnumerable�p��ForEach
        /// </summary>
        /// <typeparam name="TSource">�\�[�X</typeparam>
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
        /// IEnumerable�p��ForEach(Break��)
        /// func��true�̊ԌJ��Ԃ����s���܂��B
        /// </summary>
        /// <typeparam name="TSource">�\�[�X</typeparam>
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