
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private Transform modelParent;
    [SerializeField] private GameObject rawRegularModel;
    [SerializeField] private GameObject rawPreparedModel;
    [SerializeField] private GameObject rawOnPizzaModel;
    [SerializeField] private GameObject cookedModel;
    [SerializeField] private GameObject burntModel;
    [SerializeField] private bool snapToPizza;
    [SerializeField] private CookingTool.Type preparationTool;

    public enum State {Raw, Cooked, Burnt}

    protected State cookState;
    public State CookState {get => cookState; set
    {
        switch(value)
        {
            case State.Cooked:
                if(cookState == State.Raw)
                    cookState = value;
                break;

            case State.Burnt:
                if(cookState == State.Cooked)
                    cookState = value;
                break;
        }
    }}
    protected bool isPrepared;
    public bool IsPrepared {get => isPrepared;}
    protected bool isOnPizza;
    public bool IsOnPizza {get => isOnPizza;}
    private GameObject currentModel;
    public GameObject CurrentModel {get => currentModel;}


    protected virtual void Start()
    {
        cookState = State.Raw;
        isPrepared = false;
        isOnPizza = false;
        currentModel = modelParent.GetChild(0).gameObject;
        CheckModel();
    }

    public void CheckModel()
    {
        GameObject correctModel;

        // Check which model is the correct one for the current state of the ingredient
        switch(cookState)
        {
            case State.Raw:
                if(!isPrepared) correctModel = rawRegularModel;
                else if(!snapToPizza || !isOnPizza) correctModel = rawPreparedModel;
                else correctModel = rawOnPizzaModel;
                break;

            case State.Cooked:
                correctModel = cookedModel;
                break;

            case State.Burnt:
                correctModel = burntModel;
                break;

            default:
                correctModel = currentModel;
                break;

        }

        // Updates the model if necessary
        if(currentModel != correctModel)
            UpdateModel(correctModel);
    }

    private void UpdateModel(GameObject newModel)
    {   
        Quaternion currentRotation = currentModel.transform.rotation;
        Destroy(currentModel);

        currentModel = Instantiate(newModel, Vector3.zero, currentRotation, modelParent);
        currentModel.transform.localPosition = Vector3.zero;
    }

    public virtual void Prepare()
    {
        if(!isPrepared)
        {
            isPrepared = true;
            CheckModel();
        }
    }

    public virtual void PutOnPizza()
    {
        if(!isOnPizza)
        {
            isOnPizza = true;
            CheckModel();
        }
    }

    public virtual void Cook()
    {
        if(cookState == State.Raw)
        {
            cookState = State.Cooked;
            CheckModel();
        }
    }

    public virtual void Burn()
    {
        if(cookState == State.Burnt)
        {
            cookState = State.Burnt;
            CheckModel();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(!isPrepared)
        {
            CookingTool tool = collision.gameObject.GetComponent<CookingTool>();

            if(tool != null && tool.ToolType == preparationTool)
            {
                Prepare();
            }
        }
    }
}
