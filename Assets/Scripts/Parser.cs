using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public static class Parser
{
    public static List<NoteData> LoadMap(BGM bgm)
    {
        string json = File.ReadAllText($"Assets/Maps/{bgm.ToString()}.json");
        MapData map = JsonUtility.FromJson<MapData>(json);

        return map.notes;
    }

    public static void SaveMap(MapData mapData, BGM bgm)
    {
        string json = JsonUtility.ToJson(mapData, true);
        File.WriteAllText($"Assets/Maps/{bgm.ToString()}.json", json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        Debug.Log($"{bgm.ToString()} 맵 저장 완료!");
    }

    public static PlayData LoadPlayData(BGM bgm)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, bgm.ToString());

        if (!File.Exists(fullPath))
            return null;

        string json = File.ReadAllText(fullPath);
        
        return JsonUtility.FromJson<PlayData>(json);
    }

    public static void SavePlayData(PlayData playData)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, playData.bgm.ToString());

        string json = JsonUtility.ToJson(playData, true);
        File.WriteAllText(fullPath, json);

        Debug.Log($"{playData.bgm.ToString()} 플레이 기록 저장 완료");
    }
}
