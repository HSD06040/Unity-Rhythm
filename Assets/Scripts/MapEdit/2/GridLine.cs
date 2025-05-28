using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridLine : MonoBehaviour
{
    public List<NoteData> noteList;
    public GameObject[] noteObjs = new GameObject[4];
    public GameObject[] longBodys = new GameObject[4];
    public GameObject[] longEnds = new GameObject[4];
    public float startTime;
    public int idx;
    public int longIdx;

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

    public void RemoveNote(int idx)
    {
        NoteData newData = null;

        foreach (var note in noteList)
        {
            if(note.keyPos == idx)
            {
                newData = note;
                continue;
            }
        }

        if(newData != null)
        {
            Destroy(noteObjs[newData.keyPos]);
            noteObjs[newData.keyPos] = null;
            noteList.Remove(newData);
        }
    }

    public NoteData GetNoteData(int idx)
    {
        foreach (var note in noteList)
        {
            if (note.keyPos == idx)
            {
                return note;
            }
        }

        return null;
    }

    public void AddNoteObj(int idx, GameObject obj)
    {
        noteObjs[idx] = obj;
    }

    public void SetLongNote(int idx, GameObject body, GameObject end)
    {
        longBodys[idx] = body;
        longEnds[idx] = end;
    }

    public void RemoveLongNote(int idx)
    {
        Destroy(longBodys[idx]);
        Destroy(longEnds[idx]);
        longBodys[idx] = null;
        noteObjs[idx] = null;
    }
}
