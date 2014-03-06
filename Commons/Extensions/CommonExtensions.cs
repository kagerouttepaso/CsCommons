using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

namespace Commons.Extensions
{
    /// <summary>
    /// 汎用の拡張メソッド
    /// </summary>
    public static class IEnumerableExtensions
    {
        //IEnumrable拡張
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

    /// <summary>
    /// FileStream拡張
    /// </summary>
    public static class FileStreamExtensions{

        //FileStream拡張

        /// <summary>
        /// ファイルを最後まで一行ずつ読み込む
        /// </summary>
        /// <param name="fs">ファイルストリーム</param>
        /// <returns>1行分の情報</returns>
        public static IEnumerable<string> ReadToEndYield(this StreamReader fs)
        {
            if (fs == null) throw new ArgumentException("fs is null.");
            while (!fs.EndOfStream)
            {
                yield return fs.ReadLine();
            }
        }

    }

    /// <summary>
    /// FileInfo拡張
    /// </summary>
    public static class FileInfoExtensions{
        /// <summary>
        /// ファイル内の特定の文字列が存在しないか検索する
        /// </summary>
        /// <param name="FileInfo">ファイルインフォ</param>
        /// <param name="SerchText">検索文字列</param>
        /// <returns>ヒットした行番号(ヒットしない場合は-1)</returns>
        public static async Task<int> IsInsideTextAsync(this FileInfo FileInfo, string SerchText, StringComparison Comparison = StringComparison.Ordinal)
        {
            if (FileInfo == null) { throw new ArgumentException("FileInfo is null."); }
            if (!FileInfo.Exists) { throw new FileNotFoundException("file is not exist."); }
            if (string.IsNullOrWhiteSpace(SerchText)) { throw new ArgumentException("無効な検索文字列です"); }
            int result = -1;
            using (var file = FileInfo.OpenText())
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(file.ReadToEndYield(), (line, state, count) =>
                    {
                        if (-1 != line.IndexOf(SerchText, StringComparison.OrdinalIgnoreCase))
                        {
                            Interlocked.Exchange(ref result, (int)count);
                            state.Break();
                        }
                    });
                });
            }
            return result;
        }

    }

    /// <summary>
    /// string拡張
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 相対パス変換
        /// </summary>
        /// <param name="OursPath">自身のファイル名</param>
        /// <param name="RootPath">相対パスのルートパス</param>
        /// <returns>相対パス</returns>
        public static string GetRelativePath(this string OursPath, string RootPath)
        {
            if (string.IsNullOrWhiteSpace(OursPath)) throw new ArgumentException("OursPath is Invalid argument.");
            if (string.IsNullOrWhiteSpace(RootPath)) throw new ArgumentException("RootPath is Invalied argument.");
            return new Uri(RootPath).MakeRelativeUri(new Uri(OursPath)).ToString().Replace('/', '\\');
        }
    }
}
