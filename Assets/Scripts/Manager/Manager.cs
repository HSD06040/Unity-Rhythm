using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance 
    {  
        get 
        { 
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
            }
            if(instance == null)
            {
                SetupInstance();
            }
            return instance;
        } 
    }

    protected virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        instance = (T)FindObjectOfType(typeof(T));

        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).Name;
            instance = obj.AddComponent<T>();
            DontDestroyOnLoad(obj);
        }
    }
}
