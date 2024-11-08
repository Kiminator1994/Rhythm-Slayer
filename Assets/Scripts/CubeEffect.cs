using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEffect : MonoBehaviour
{
    public GameObject particleEffectPrefab;
    public bool isRed;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 11) // Saber
        {
            SpawnParticleEffect();
        }
    }



    private void FixedUpdate()
    {
        if (GameManager.Instance.GameIsOver)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnParticleEffect()
    {
        if (particleEffectPrefab != null)
        {
            GameObject effect = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);

            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                Destroy(effect, 1.5f);
            }
        }
    }
}
