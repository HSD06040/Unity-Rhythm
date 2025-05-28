using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine : MonoBehaviour
{
    public List<NoteData> noteList;
    public float startTime;
    public int idx;

    private void Awake()
    {

    }

    public void Init(int _idx)
    {
        idx = _idx;
    }
}
