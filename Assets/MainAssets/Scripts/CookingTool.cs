using UnityEngine;

public class CookingTool : MonoBehaviour
{
    public enum Type {Knife, RollingPin, Grater}

    [SerializeField] private Type type;
    public Type ToolType {get => type;}

    private Rigidbody toolRigidbody;
    public bool IsBeingHeld {get => toolRigidbody.isKinematic;}

    private void Start()
    {
        toolRigidbody = GetComponentInParent<Rigidbody>();
    }
}
