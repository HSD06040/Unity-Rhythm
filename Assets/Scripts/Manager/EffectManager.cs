using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectManager : Manager<EffectManager>
{
    private Dictionary<string, GameObject> effectDic = new Dictionary<string, GameObject>();
    private Dictionary<string, ObjectPool<GameObject>> effectPoolDic = new Dictionary<string, ObjectPool<GameObject>>();

    private YieldInstruction delay = new WaitForSeconds(1);

    public void CreateEffect(string name, Vector3 posision, Transform parent)
    {
        GameObject go = null;

        if (effectPoolDic.TryGetValue(name, out var pool))
            go = pool.Get();
        else
            go = CreatePool(GetEffect(name), name, parent).Get();

        go.transform.position = posision;

        StartCoroutine(ReleaseRoutine(name,go));
    }

    private IEnumerator ReleaseRoutine(string name, GameObject go)
    {
        yield return delay;

        if (effectPoolDic.TryGetValue(name, out var pool))
            pool.Release(go);
        else
            Destroy(go);
    }

    public GameObject GetEffect(string name)
    {
        if (effectDic.TryGetValue(name, out GameObject effect))
            return effect;

        effect = Resources.Load<GameObject>($"Effects/{name}");
        effectDic.Add(name, effect);

        return effect;
    }

    private ObjectPool<GameObject> CreatePool(GameObject prefab, string name, Transform parent)
    {
        Transform root = new GameObject($"{prefab.name} Pool").transform;
        root.SetParent(parent, false);

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>
            (
                () =>
                {
                    GameObject go = GameObject.Instantiate(prefab);

                    if (go.name != name)
                        go.name = name;

                    go.transform.SetParent(root, false);
                    return go;
                },
                obj =>
                {
                    obj.SetActive(true);                    
                    obj.transform.SetParent(null);
                },
                obj =>
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(root, false);
                },
                obj =>
                {
                    Destroy(obj.gameObject);
                },
                false,
                10
            );

        effectPoolDic.Add(name, pool);

        return pool;
    }

    public void Release(GameObject perfab)
    {
        if (effectPoolDic.TryGetValue(perfab.name, out var pool))
            pool.Release(perfab);
    }
}
