using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public ParticleSystem prefab;
    public int poolSize = 10;

    private Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var ps = Instantiate(prefab, transform);
            ps.gameObject.SetActive(false);
            pool.Enqueue(ps);
        }
    }

    public void Spawn(Vector3 position)
    {
        if (pool.Count == 0) return;

        var ps = pool.Dequeue();
        ps.transform.position = position;
        ps.gameObject.SetActive(true);
        ps.Play();

        StartCoroutine(ReturnAfter(ps, ps.main.duration));
    }

    private IEnumerator ReturnAfter(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        ps.Stop();
        ps.gameObject.SetActive(false);
        pool.Enqueue(ps);
    }
}
