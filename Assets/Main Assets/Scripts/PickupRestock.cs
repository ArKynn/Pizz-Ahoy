using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PickupRestock : MonoBehaviour
{
    [SerializeField] private GameObject objPrefab;
    private XRGrabInteractable _xrGrab;
    private Vector3 position;
    private Quaternion rotation;

    private void Start()
    {
        _xrGrab = GetComponent<XRGrabInteractable>();
        _xrGrab.firstFocusEntered.AddListener(OnPickup);
        position = transform.position;
        rotation = transform.rotation;
    }

    private void OnPickup(FocusEnterEventArgs args)
    {
        Instantiate(objPrefab, position, rotation);
        
        _xrGrab.firstFocusEntered.RemoveListener(OnPickup);
    }
}
