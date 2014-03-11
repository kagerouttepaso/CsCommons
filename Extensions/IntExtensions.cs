using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace Commons.Extensions
{

    /// <summary>
    /// int拡張
    /// </summary>
    public static class IntExtensions
    {
        public static IEnumerable<Tresult> Takes<Tresult>(this int count, Func<int, Tresult> func)
        {
            for (int i = 0; i < count; i++)
			{
                yield return func(i);
			}
        }
        public static void Takes(this int count, Action<int> action)
        {
            for (int i = 0; i < count; i++)
            {
                action(i);
            }
        }
    }
}
