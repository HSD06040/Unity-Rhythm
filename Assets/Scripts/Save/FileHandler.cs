using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FileHandler
{
    private string fullPath;
    private bool isEncrype = true;
    private string codeWord = "HSD";

    public FileHandler(string dataPath, string dataFileName)
    {
        fullPath = Path.Combine(dataPath, dataFileName);
    }

    public void SaveData(GameData data)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToSave = JsonUtility.ToJson(data, true);

            if(isEncrype)
                dataToSave = EncryptDecrypt(dataToSave);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }

        catch(Exception e)
        {
            Debug.LogError("저장 중 오류가 발생 하였습니다" + fullPath + e);
        }
    }

    public GameData LoadData()
    {
        GameData data = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(isEncrype)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("로드하는 도중에 오류가 발생하였습니다." + fullPath + e);
            }
        }

        return data;
    }

    public void DeleteFile()
    {
        File.Delete(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifedData;
    }
}
