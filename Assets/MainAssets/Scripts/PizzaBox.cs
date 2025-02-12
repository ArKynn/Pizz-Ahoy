using System;
using Main_Assets.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PizzaBox : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor bottomBox;
    [SerializeField] private XRSocketInteractor topBox;
    private Animator _animator;
    private bool _hasPizza;
    private bool _hasOrder;
    
    public Pizza pizza {get; private set;}
    public Order order {get; private set;}
    
    
    private void Start()
    {
        bottomBox.selectEntered.AddListener(UpdateRequirements);
        bottomBox.selectExited.AddListener(UpdateRequirements);
        topBox.selectEntered.AddListener(UpdateRequirements);
        topBox.selectExited.AddListener(UpdateRequirements);
    }

    private void UpdateRequirements(SelectEnterEventArgs args = null)
    {
        CheckHasPizza();
        CheckHasOrder();
        CheckCanCloseBox();
    }

    private void UpdateRequirements(SelectExitEventArgs args = null)
    {
        CheckHasPizza();
        CheckHasOrder();
        CheckCanCloseBox();
    }

    private void CheckHasPizza()
    {
        _hasPizza = bottomBox.hasSelection &&
                    (pizza = bottomBox.interactablesSelected[0].transform.gameObject.GetComponent<Pizza>()) != null;
    }

    private void CheckHasOrder()
    {
        _hasOrder = topBox.hasSelection && 
                    (order = topBox.interactablesSelected[0].transform.gameObject.GetComponent<Order>()) != null;
    }

    private void CheckCanCloseBox()
    {
        if(_hasPizza && _hasOrder)
        {
            _animator.SetTrigger("Close");
            bottomBox.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("SocketBox");
        }
    }
}
