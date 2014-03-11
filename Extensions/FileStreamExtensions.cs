using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;



namespace Commons.Extensions
{
    /// <summary>
    /// FileStream�g��
    /// </summary>
    public static class FileStreamExtensions
    {
        /// <summary>
        /// �t�@�C�����Ō�܂ň�s���ǂݍ���
        /// </summary>
        /// <param name="fs">�t�@�C���X�g���[��</param>
        /// <returns>1�s���̏��</returns>
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