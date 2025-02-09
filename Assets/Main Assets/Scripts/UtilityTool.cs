using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class UtilityTool : MonoBehaviour
{
    protected XRGrabInteractable grabInteractable;

    protected virtual void Start()
    {
        grabInteractable.GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(ActivatedAction);
        grabInteractable.deactivated.AddListener(DeactivatedAction);
    }

    protected virtual void ActivatedAction(ActivateEventArgs args)
    {
        Debug.Log("Activated");
    }

    protected virtual void DeactivatedAction(DeactivateEventArgs args)
    {
        Debug.Log("Deactivated");
    }
}
