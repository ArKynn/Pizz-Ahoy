using UnityEngine;

public class Dough : Ingredient
{
    private GameObject pizza;

    protected override void Start()
    {
        base.Start();

        pizza = GetComponentInChildren<Pizza>().gameObject;
        pizza.SetActive(false);
    }

    public override void Prepare()
    {
        if(!isPrepared)
        {
            isPrepared = true;
            CheckModel();

            pizza.SetActive(true);
        }
    }
}
