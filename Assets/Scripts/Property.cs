using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Property<T>
{
    private T _value;
    public T Value { get { return _value; } set { if (EqualityComparer<T>.Default.Equals(_value, value)) return; _value = value; onValueChanged?.Invoke(_value); } }

    private event Action<T> onValueChanged;

    public Property()
    {
        _value = default;
    }

    public void AddEvent(Action<T> action)
    {
        onValueChanged += action;
    }

    public void RemoveEvent(Action<T> action)
    {
        onValueChanged -= action;
    }

    public void AllRemoveEvent()
    {
        foreach (Action<T> action in onValueChanged.GetInvocationList())
        {
            onValueChanged -= action;
        }
    }
}
