using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;



namespace Commons.Extensions
{
    /// <summary>
    /// DirectoryInfo�̊g�����\�b�h
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// �t�H���_�̃p�X����t�@�C���ꗗ���擾
        /// </summary>
        /// <param name="RootDirInfo">���[�g</param>
        /// <returns></returns>
        public static async Task<List<FileInfo>> GetTreeFileInfosAsync(this DirectoryInfo RootDirInfo)
        {
            //�ϐ�������
            if (!RootDirInfo.Exists) { throw new FileNotFoundException("���݂��Ȃ��t�H���_���ł�"); }

            var retList = new List<FileInfo>();                 //�߂�l
            var serch = new Action<DirectoryInfo>(x => { });    //�������\�b�h
            //���\�b�h����
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
            //�񓯊����s
            await Task.Run(() => serch(RootDirInfo));
            //���ʂ�Ԃ�
            return retList;
        }
    }
}
