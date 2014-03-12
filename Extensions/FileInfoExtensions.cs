using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace Commons.Extensions
{
    /// <summary>
    /// FileInfo拡張
    /// </summary>
    public static class FileInfoExtensions
    {
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

        /// <summary>
        /// 相対パス変換
        /// </summary>
        /// <param name="OursPath">自身のファイル名</param>
        /// <param name="RootPath">相対パスのルートパス</param>
        /// <returns>相対パス</returns>
        public static string GetRelativePath(this FileInfo OursPath, string RootPath)
        {
            if (OursPath == null) throw new ArgumentException("OurPath is null.");
            if (string.IsNullOrWhiteSpace(OursPath.FullName)) throw new ArgumentException("OursPath is Invalid argument.");
            if (string.IsNullOrWhiteSpace(RootPath)) throw new ArgumentException("RootPath is Invalied argument.");
            return new Uri(RootPath).MakeRelativeUri(new Uri(OursPath.FullName)).ToString().Replace('/', '\\');
        }

    }
}