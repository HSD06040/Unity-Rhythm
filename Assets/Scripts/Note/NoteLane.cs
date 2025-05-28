using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoteLane
{
    private Queue<Note> notes = new Queue<Note>();
    [SerializeField]
    private List<Note> debugNotes = new(); // 인스펙터에서 보기 위한 리스트

    public void AddNote(Note note)
    {
        notes.Enqueue(note);
        debugNotes.Add(note);
    }

    public Note GetNote()
    {
        foreach (var note in notes)
        {
            if (!note.data.isJudgeDone)
            {
                return note;
            }
        }
        return null;
    }

    public Note DequeueNote()
    {
        if (notes.Count == 0)
        {
            return null;
        }

        notes.TryDequeue(out Note note);
        debugNotes.Remove(note);
        return note;
    }

    public Note GetNextLongBody()
    {
        foreach (var note in notes)
        {
            if (note.data.isLongNoteBody && !note.data.isJudgeDone)
            {
                return note;
            }
        }
        return null;
    }
}
