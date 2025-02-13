using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CookingTool : MonoBehaviour
{
    public enum Type {Knife, RollingPin, Grater}

    [SerializeField] private Type type;
    [SerializeField] private int minHandGrabs = 1;
    public Type ToolType {get => type;}
    public bool IsBeingHeld {get => interactors.Count >= minHandGrabs;}
    private XRGrabInteractable grabInteractable;
    private HashSet<XRBaseInteractor> interactors = new HashSet<XRBaseInteractor>();

    private void Start()
    {
        grabInteractable = GetComponentInParent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(RegisterGrab);
        grabInteractable.selectExited.AddListener(RegisterRelease);
    }

    private void RegisterGrab(SelectEnterEventArgs args)
    {
        interactors.Add((XRBaseInteractor)args.interactorObject);
    }

    private void RegisterRelease(SelectExitEventArgs args)
    {
        interactors.Remove((XRBaseInteractor)args.interactorObject);
    }
}
