using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEffect : MonoBehaviour
{
    public GameObject particleEffectPrefab;

    private void OnDestroy()
    {
        SpawnParticleEffect();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameIsOver)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnParticleEffect()
    {
        if (particleEffectPrefab != null)
        {
            GameObject effect = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);

            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                Destroy(effect, particleSystem.main.duration);
            }
        }
    }
}
