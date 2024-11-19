using System;
using System.Collections.Generic;
using System.IO;

public class FileManager
{
    /// <summary>
    /// 폴더 내 모든 파일의 이름을 숫자로 변환해 리스트로 반환 (숫자가 아닌 파일은 제외)
    /// </summary>
    /// <param name="argFolderPath">파일 경로</param>
    /// <returns>정렬된 파일 이름 리스트</returns>
    public List<int> GetFileNum(string argFolderPath)
    {
        if (Directory.Exists(argFolderPath))
        {
            List<int> numFileNames = new List<int>();
            string[] files = Directory.GetFiles(argFolderPath);

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (int.TryParse(fileName, out int fileNumber))
                {
                    numFileNames.Add(fileNumber);
                }
            }
            numFileNames.Sort();
            return numFileNames;
        }
        return new List<int>();
    }
}

