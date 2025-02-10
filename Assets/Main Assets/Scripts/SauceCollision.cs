using System.Collections.Generic;
using UnityEngine;

public class SauceCollision : MonoBehaviour
{
    [SerializeField] private GameObject saucePrefab;

    private List<ParticleCollisionEvent> particleCollisionEvents;
    private ParticleSystem sauceParticles;

    private void Start()
    {
        sauceParticles = GetComponent<ParticleSystem>();
    }

    protected void OnParticleCollision(GameObject other)
    {
        int numOfCollisionEvents = sauceParticles.GetCollisionEvents(other, particleCollisionEvents);
        Dough dough = other.GetComponentInParent<Dough>();

        for(int i = 0; i < numOfCollisionEvents; i++)
        {
            if (dough != null)
            {
                Pizza pizza = dough.GetComponentInChildren<Pizza>();

                if(!pizza.AttachedIngredients.Contains(saucePrefab.GetComponent<Ingredient>()))
                {
                    GameObject newSauce = Instantiate(
                        saucePrefab, pizza.transform.position, Quaternion.identity);

                    pizza.AddIngredient(newSauce.GetComponent<Ingredient>());
                }
            }

            else
            {
                //Vector3 pos = particleCollisionEvents[i].intersection;

                //spawn sauce stain decal
                //enqueue the decal for despawn once max decals are spawned
            }
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
