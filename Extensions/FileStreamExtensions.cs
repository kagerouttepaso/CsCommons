using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace Commons.Extensions
{
    /// <summary>
    /// FileStream拡張
    /// </summary>
    public static class FileStreamExtensions
    {
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
}