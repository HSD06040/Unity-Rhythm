using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<string, Component> componentDic;

    protected virtual void Awake()
    {
        RectTransform[] rects = GetComponentsInChildren<RectTransform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(rects.Length * 4);
        foreach (var rect in rects)
        {
            gameObjectDic.TryAdd(rect.name, rect.gameObject);
        }

        Component[] components = GetComponentsInChildren<Component>(true);
        componentDic = new Dictionary<string, Component>(components.Length * 4);
        foreach (var child in components)
        {
            componentDic.TryAdd($"{child.gameObject.name}_{child.GetType().Name}", child);
            Debug.Log($"[BaseUI] Registered: {child.gameObject.name}_{child.GetType().Name}");
        }
    }

    public GameObject GetUI(in string name)
    {
        if(string.IsNullOrEmpty(name)) return null;

        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
    }

    public T GetUI<T> (in string name) where T : Component
    {
        if(string.IsNullOrEmpty(name)) return null;

        Debug.Log($"[BaseUI] Registered: {name}_{typeof(T).Name}");
        componentDic.TryGetValue($"{name}_{typeof(T).Name}", out var component);
        return component as T;
    }
}
