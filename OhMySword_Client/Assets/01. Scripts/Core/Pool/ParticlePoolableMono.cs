using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolableMono : PoolableMono
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public override void Init()
    {
        particle.Play();
        StartCoroutine(Push());
    }

    private IEnumerator Push()
    {
        yield return new WaitUntil(() => particle.isPlaying == false);

        PoolManager.Instance.Push(this);
    }
}
