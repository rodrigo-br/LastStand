using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public void Explode()
    {
        StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
        _particleSystem.Play();
        yield return new WaitForSeconds(_particleSystem.main.duration);
        _particleSystem.Stop();
    }
}
