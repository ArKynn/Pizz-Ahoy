using UnityEngine;

public class CookingTool : MonoBehaviour
{
    public enum Type {Knife, RollingPin, Grater}

    [SerializeField] private Type type;
    public Type ToolType {get => type;}
}
