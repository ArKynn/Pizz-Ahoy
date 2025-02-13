using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Dough : Ingredient
{
    private GameObject pizza;

    protected override void Start()
    {
        base.Start();

        pizza = GetComponentInChildren<Pizza>().gameObject;
        if(!spawnPrepared)
            pizza.SetActive(false);
    }

    public override void Prepare()
    {
        if(!isPrepared)
        {
            isPrepared = true;
            grabInteractable.interactionLayers = InteractionLayerMask.GetMask("Default", "SocketPizza");
            CheckModel();

            if(pizza == null) Start();
            if(pizza != null)
                pizza.SetActive(true);
        }
    }
}
