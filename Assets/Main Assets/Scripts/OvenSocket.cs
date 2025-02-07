using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class OvenSocket : MonoBehaviour
{
    private XRSocketInteractor socket;
    private Pizza selectedPizza;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(UpdateSocketContent);
        socket.selectExited.AddListener(UpdateSocketContent);
    }

    private void Update()
    {
        if(selectedPizza != null) 
            selectedPizza.AddCookTime();
    }

    private void UpdateSocketContent(SelectEnterEventArgs args)
    {
        selectedPizza = socket.interactablesSelected[0].transform.GetComponentInChildren<Pizza>();

    }

    private void UpdateSocketContent(SelectExitEventArgs args)
    {
        selectedPizza = null;
    }
}
