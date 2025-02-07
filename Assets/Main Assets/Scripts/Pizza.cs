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


    private void Start()
    {
        cookTimer = 0;
        attachedIngredients = new List<Ingredient>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Ingredient ingredientToAttach = other.gameObject.GetComponent<Ingredient>();
        if(ingredientToAttach != null)
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

        foreach(Ingredient i in attachedIngredients)
        {
            i.Cook();
        }
    }

    private void BecomeBurnt()
    {
        cookState = State.Burnt;

        foreach(Ingredient i in attachedIngredients)
        {
            i.Burn();
        }
    }

    private void AddIngredient(Ingredient ingredient)
    {
        attachedIngredients.Add(ingredient);
        ingredient.CurrentModel.transform.parent = ingredientsParent;
        Destroy(ingredient.GetComponent<XRGrabInteractable>());
        Destroy(ingredient.GetComponent<Rigidbody>());
    }

    public List<Ingredient> GetIngredients()
    {
        return attachedIngredients;
    }
}
