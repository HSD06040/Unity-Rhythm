using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public static class MapParser
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
}
