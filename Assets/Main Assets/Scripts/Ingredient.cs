
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Ingredient : MonoBehaviour
{
    [SerializeField] protected Transform modelParent;
    [SerializeField] protected GameObject rawRegularModel;
    [SerializeField] protected GameObject rawPreparedModel;
    [SerializeField] protected GameObject rawOnPizzaModel;
    [SerializeField] protected GameObject cookedModel;
    [SerializeField] protected GameObject burntModel;
    [SerializeField] protected CookingTool.Type preparationTool;
    [SerializeField] protected AudioClip preparedSound;
    [SerializeField] protected AudioMixerGroup audioMixerGroup;
    [SerializeField] protected bool snapToPizza;
    [SerializeField] protected bool spawnPrepared;
    [SerializeField] protected bool splitWhenPrepared;
    [SerializeField] [Min(1)] protected int splitAmount;
    [SerializeField] protected int moneyValue = 10;
    [SerializeField] protected Sprite sprite;

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
    public bool SnapToPizza {get => snapToPizza;}
    public int MoneyValue {get => moneyValue;}
    public Sprite Sprite {get => sprite;}
    public GameObject CurrentModel {get => modelParent.GetChild(0).gameObject;}
    protected XRGrabInteractable grabInteractable;
    protected bool hasSplit;
    protected AudioSource audioSource;


    protected virtual void Start()
    {
        cookState = State.Raw;
        isPrepared = spawnPrepared;
        isOnPizza = false;
        hasSplit = false;
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = AudioManager.CreateLocalAudioSource(gameObject, audioMixerGroup);

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
                correctModel = CurrentModel;
                break;

        }

        // Updates the model if necessary
        if(CurrentModel != correctModel)
            UpdateModel(correctModel.GetComponent<MeshFilter>(), correctModel.GetComponent<MeshRenderer>());
    }

    private void UpdateModel(MeshFilter newMesh, MeshRenderer newMaterial)
    {   
        CurrentModel.GetComponent<MeshFilter>().mesh = newMesh.mesh;
        CurrentModel.GetComponent<MeshRenderer>().materials = newMaterial.materials;
        CurrentModel.GetComponent<MeshCollider>().sharedMesh = newMesh.mesh;
        CurrentModel.transform.localScale = newMesh.transform.localScale;
        CurrentModel.transform.rotation = Quaternion.identity;
        if(newMesh.transform.localScale.y >= 0.01f)
            CurrentModel.transform.localScale =  new Vector3(newMesh.transform.localScale.x, newMesh.transform.localScale.y + 0.005f, newMesh.transform.localScale.z);
    }

    public virtual void Prepare()
    {
        if(!isPrepared)
        {
            if(splitWhenPrepared && !hasSplit)
            {
                hasSplit = true;

                for(int i = 0; i < splitAmount; i++)
                {
                    spawnPrepared = true;
                    splitWhenPrepared = false;
                    Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
                }

                gameObject.SetActive(false);
            }

            else
            {
                isPrepared = true;
                CheckModel();
            }
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
        if(cookState == State.Cooked)
        {
            cookState = State.Burnt;
            CheckModel();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        CookingTool tool = collision.gameObject.GetComponent<CookingTool>();

        if(tool != null && tool.ToolType == preparationTool)
        {
            AudioManager.PlayLocalSound(audioSource, preparedSound);
            Prepare();
        }
    }
}
