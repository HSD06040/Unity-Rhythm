using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private Transform[] spawnPoints;

    private List<int> idxs = new();
    private bool isPlaying;
    private float timer;

    private readonly YieldInstruction loopDelay = new WaitForSeconds(1.5f);
    private readonly YieldInstruction particleDelay = new WaitForSeconds(.8f);

    public void PlayParticle()
    {
        if (isPlaying) return;

        StartCoroutine(ParticleLoop());
    }

    private IEnumerator ParticleLoop()
    {
        while (true)
        {
            idxs.Clear();

            for (int i = 0; i < 4; i++)
            {
                foreach (var particle in particles)
                {
                    if (!particle.gameObject.activeSelf)
                        particle.gameObject.SetActive(true);
                }

                CreateParticle();
                yield return particleDelay;
            }

            yield return loopDelay;
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
