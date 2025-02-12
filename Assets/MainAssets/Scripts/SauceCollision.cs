using System.Collections.Generic;
using UnityEngine;

public class SauceCollision : MonoBehaviour
{
    [SerializeField] private GameObject saucePrefab;

    private ParticleSystem sauceParticles;

    private void Start()
    {
        sauceParticles = GetComponent<ParticleSystem>();
    }

    protected void OnParticleCollision(GameObject other)
    {
        Dough dough = other.GetComponentInParent<Dough>();
        
        if (dough != null)
        {
            Pizza pizza = dough.GetComponentInChildren<Pizza>();

            if(!pizza.HasSauce)
            {
                Instantiate(saucePrefab, pizza.transform.position, Quaternion.identity);
                pizza.HasSauce = true;
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
