using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SauceBottle : UtilityTool
{
    [SerializeField] private GameObject saucePrefab;

    private ParticleSystem sauceParticles;
    public List<ParticleCollisionEvent> particleCollisionEvents;

    protected override void Start()
    {
        base.Start();

        sauceParticles = GetComponentInChildren<ParticleSystem>();
        sauceParticles.Stop();
    }

    protected override void ActivatedAction(ActivateEventArgs args)
    {
        sauceParticles.Play();
    }

    protected override void DeactivatedAction(DeactivateEventArgs args)
    {
        sauceParticles.Stop();
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
}
