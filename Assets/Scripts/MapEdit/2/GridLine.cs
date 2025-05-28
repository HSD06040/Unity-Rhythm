using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine : MonoBehaviour
{
    public List<NoteData> noteList;
    public float startTime;
    public int idx;

    public void Init(int _idx)
    {
        idx = _idx;
        startTime = 100 * idx;
    }

    public void AddNoteData(NoteData data)
    {
        bool isAdd = false;

        foreach (var note in noteList)
        {
            if(note.keyPos == data.keyPos)
            {
                isAdd = true;
                Debug.Log("이미 그 자리에는 노트가 있습니다.");
            }
        }

        if (!isAdd)
        {
            noteList.Add(data);
        }
    }
}
