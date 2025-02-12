using System.Collections.Generic;
using UnityEngine;

public class SauceCollision : MonoBehaviour
{
    [SerializeField] private GameObject saucePrefab;

    private ParticleSystem sauceParticles;
    private float spawnBufferTimer;

    private void Start()
    {
        sauceParticles = GetComponent<ParticleSystem>();
        spawnBufferTimer = 0f;
    }

    private void Update()
    {
        if(spawnBufferTimer > 0) spawnBufferTimer -= Time.deltaTime;
    }

    protected void OnParticleCollision(GameObject other)
    {
        Dough dough = other.GetComponentInParent<Dough>();
        
        if (dough != null && spawnBufferTimer <= 0)
        {
            Pizza pizza = dough.GetComponentInChildren<Pizza>();

            if(pizza != null && !pizza.HasSauce)
            {
                Instantiate(saucePrefab, pizza.transform.position, Quaternion.identity);
                spawnBufferTimer = 0.3f;
            }
        }

        else
        {
            //Vector3 pos = particleCollisionEvents[i].intersection;

            //spawn sauce stain decal
            //enqueue the decal for despawn once max decals are spawned
        }
    }

    public void PlayParticles()
    {
        sauceParticles.Play();
    }

    public void StopParticles()
    {
        sauceParticles.Stop();
    }
}
