using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RankParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private Transform[] spawnPoints;

    private List<int> idxs = new();
    private readonly YieldInstruction delay = new WaitForSeconds(.5f);

    public void PlayParticle()
    {
        foreach (var particle in particles)
        {
            particle.gameObject.SetActive(true);
        }

        StartCoroutine(ParticleRoutine());
    }

    private IEnumerator ParticleRoutine()
    {
        idxs.Clear();

        for (int i = 0; i < 5; i++)
        {
            CreateParticle();
            yield return delay;
        }        
    }

    private void CreateParticle()
    {
        gameObject.transform.position = GetRandomPos();

        foreach (var particle in particles)
        {
            particle.Play();
        }
    }

    private Vector3 GetRandomPos()
    {
        int idx;

        do
        {
            idx = Random.Range(0, spawnPoints.Length);
        }
        while (idxs.Contains(idx));

        idxs.Add(idx);

        return spawnPoints[idx].transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f);
    }
}
