using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Pizza : MonoBehaviour
{
    public enum State {Raw, Cooked, Burnt}

    [SerializeField] private float cookTime;
    [SerializeField] private float burnTime;
    [SerializeField] private Transform ingredientsParent;

    private float cookTimer;
    public float CookTimer {get => cookTimer;}
    private State cookState;
    public State CookState {get => cookState;}
    private List<Ingredient> attachedIngredients;
    public List<Ingredient> AttachedIngredients {get => attachedIngredients;}
    private float snapHeight;
    private float snapHeightIncrements;


    private void Start()
    {
        cookTimer = 0;
        attachedIngredients = new List<Ingredient>();
        snapHeight = 0.007f;
        snapHeightIncrements = 0.005f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredientToAttach = other.gameObject.GetComponentInParent<Ingredient>();
        if(ingredientToAttach != null && !ingredientToAttach.GetComponent<Dough>())
        {
            AddIngredient(ingredientToAttach);
        }
    }

    public void AddCookTime()
    {
        cookTimer += Time.deltaTime;

        if(cookState == State.Raw && cookTimer >= cookTime)
            BecomeCooked();
        
        if(cookState == State.Cooked && cookTimer >= cookTime + burnTime)
            BecomeBurnt();
    }

    private void BecomeCooked()
    {
        cookState = State.Cooked;

        GetComponentInParent<Dough>().Cook();

        foreach(Ingredient i in attachedIngredients)
        {
            i.Cook();
        }
    }

    private void BecomeBurnt()
    {
        cookState = State.Burnt;

        GetComponentInParent<Dough>().Burn();

        foreach(Ingredient i in attachedIngredients)
        {
            i.Burn();
        }
    }

    public void AddIngredient(Ingredient ingredient)
    {
        attachedIngredients.Add(ingredient);
        ingredient.transform.parent = ingredientsParent;
        ingredient.CurrentModel.GetComponent<Collider>().isTrigger = true;
        ingredient.GetComponent<Rigidbody>().isKinematic = true;
        ingredient.PutOnPizza();
        Destroy(ingredient.GetComponent<XRGrabInteractable>());

        if(ingredient.SnapToPizza && ingredient.IsPrepared)
        {
            ingredient.transform.localPosition = new Vector3(0f, snapHeight, 0f);
            ingredient.transform.localRotation = Quaternion.identity;

            snapHeight += snapHeightIncrements;
        }
    }
}
