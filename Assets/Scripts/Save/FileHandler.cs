using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FileHandler
{
    private string fullPath;
    private bool isEncrype;

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
            Debug.LogError("���� �� ������ �߻� �Ͽ����ϴ�" + fullPath + e);
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

                data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("�ε��ϴ� ���߿� ������ �߻��Ͽ����ϴ�." + fullPath + e);
            }
        }

        return data;
    }

    public void DeleteFile()
    {
        File.Delete(fullPath);
    }
}
