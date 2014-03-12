using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace Commons.Extensions
{
    /// <summary>
    /// FileInfo�g��
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// �t�@�C�����̓���̕����񂪑��݂��Ȃ�����������
        /// </summary>
        /// <param name="FileInfo">�t�@�C���C���t�H</param>
        /// <param name="SerchText">����������</param>
        /// <returns>�q�b�g�����s�ԍ�(�q�b�g���Ȃ��ꍇ��-1)</returns>
        public static async Task<int> IsInsideTextAsync(this FileInfo FileInfo, string SerchText, StringComparison Comparison = StringComparison.Ordinal)
        {
            if (FileInfo == null) { throw new ArgumentException("FileInfo is null."); }
            if (!FileInfo.Exists) { throw new FileNotFoundException("file is not exist."); }
            if (string.IsNullOrWhiteSpace(SerchText)) { throw new ArgumentException("�����Ȍ���������ł�"); }
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
        /// ���΃p�X�ϊ�
        /// </summary>
        /// <param name="OursPath">���g�̃t�@�C����</param>
        /// <param name="RootPath">���΃p�X�̃��[�g�p�X</param>
        /// <returns>���΃p�X</returns>
        public static string GetRelativePath(this FileInfo OursPath, string RootPath)
        {
            if (OursPath == null) throw new ArgumentException("OurPath is null.");
            if (string.IsNullOrWhiteSpace(OursPath.FullName)) throw new ArgumentException("OursPath is Invalid argument.");
            if (string.IsNullOrWhiteSpace(RootPath)) throw new ArgumentException("RootPath is Invalied argument.");
            return new Uri(RootPath).MakeRelativeUri(new Uri(OursPath.FullName)).ToString().Replace('/', '\\');
        }

    }
}