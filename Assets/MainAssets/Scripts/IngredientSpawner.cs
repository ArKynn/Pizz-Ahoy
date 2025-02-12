using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objPrefab;
    private XRGrabInteractable _xrGrab;
    private Vector3 position;
    private Quaternion rotation;

    private void Start()
    {
        SpawnObj();
    }

    private void SpawnObj(SelectEnterEventArgs args = null)
    {
        var obj = Instantiate(objPrefab, transform.position, quaternion.identity);
        _xrGrab = obj.GetComponent<XRGrabInteractable>();
        _xrGrab.firstSelectEntered.AddListener(SpawnObj);
    }
}
