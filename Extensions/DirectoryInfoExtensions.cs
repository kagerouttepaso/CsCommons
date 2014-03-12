using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;



namespace Commons.Extensions
{
    /// <summary>
    /// DirectoryInfoの拡張メソッド
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// フォルダのパスからファイル一覧を取得
        /// </summary>
        /// <param name="RootDirInfo">ルート</param>
        /// <returns></returns>
        public static async Task<List<FileInfo>> GetTreeFileInfosAsync(this DirectoryInfo RootDirInfo)
        {
            //変数初期化
            if (!RootDirInfo.Exists) { throw new FileNotFoundException("存在しないフォルダ名です"); }

            var retList = new List<FileInfo>();                 //戻り値
            var serch = new Action<DirectoryInfo>(x => { });    //検索メソッド
            //メソッド実装
            serch = dInfo =>
            {
                try
                {
                    retList.AddRange(dInfo.GetFiles());
                    dInfo.GetDirectories().ForEach(d =>
                    {
                        serch(d);
                    });
                }
                catch (Exception e)
                {
                    throw e;
                }
            };
            //非同期実行
            await Task.Run(() => serch(RootDirInfo));
            //結果を返す
            return retList;
        }
    }
}
