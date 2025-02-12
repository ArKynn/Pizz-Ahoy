using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PaddleDetachPizza : MonoBehaviour
{
    
    private XRSocketInteractor socketInteractor;
    
    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Oven") == false) return;

        var ovenSocket = other.GetComponent<XRSocketInteractor>();
        if(ovenSocket == null) return;

        switch (socketInteractor.hasSelection)
        {
            case true when !ovenSocket.hasSelection:
                socketInteractor.enabled = false;
                break;
            case false when ovenSocket.hasSelection:
                ovenSocket.enabled = false;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Oven":
            {
                socketInteractor.enabled = true;
                other.GetComponent<XRSocketInteractor>().enabled = true;
                break;
            }
        }
    }
}
