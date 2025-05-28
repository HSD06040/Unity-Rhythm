using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteData data = new();
    private SpriteRenderer sr;

    [SerializeField] private float[] offset;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void InitData(NoteData _data)
    {
        _data.TransmitData(data);

        if(data.isJudgeDone)
            data.isJudgeDone = false;

        transform.position = new Vector3(transform.position.x + offset[data.keyPos], transform.position.y, 0);

        GetKeyPos();

        if (data.isLongNoteBody)
        {
            SetLongNoteScale();
        }
        else if (data.isLongNoteStart || (data.longNoteData != null && data.longNoteData.isLongNoteEnd))
        {
            sr.color = Color.clear;
        }
    }

    private void GetKeyPos()
    {
        transform.position = new Vector3(GameManager.Instance.spawnLine.position.x + offset[data.keyPos], transform.position.y);
    }

    private void SetLongNoteScale()
    {
        float height = sr.sprite.bounds.size.y;

        float distance = (GetCurrentNotePos(data).y - GetCurrentNotePos(data.longNoteData).y);
        float result = distance / height;
        transform.localScale = new Vector3(transform.localScale.x, -result, transform.localScale.z);
    }

    private Vector3 GetCurrentNotePos(NoteData data)
    {
        double currentTime = GameManager.Instance.time;
        double delta = currentTime - data.startTime;

        double progress = delta / GameManager.Instance.endTime;

        double adjustedDistance = GameManager.Instance.distnace * GameManager.Instance.scrollSpeed;

        double y = GameManager.Instance.judgeLine.position.y - adjustedDistance * progress;

        return new Vector3(transform.position.x, (float)y, 0f);
    }


    private void Update()
    {
        if (data.isLongNoteBody)
        {
            SetLongNoteScale();
        }

        transform.position = GetCurrentNotePos(data);

        if (data.isLongNoteBody)
            return;

        if (data.startTime - GameManager.Instance.time < 0)
        {
            JudgeManager.Instance.Miss(this);
        }
    }
}
