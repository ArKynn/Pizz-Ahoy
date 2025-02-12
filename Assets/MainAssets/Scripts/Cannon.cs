using Main_Assets.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Cannon : MonoBehaviour
{
    private GameManager gameManager;
    private XRSocketInteractor socket;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        socket = GetComponentInChildren<XRSocketInteractor>();

        socket.selectEntered.AddListener(ShootPizza);
    }

    private void ShootPizza(SelectEnterEventArgs args)
    {
        if(socket.hasSelection)
        {
            PizzaBox pizzaBox = socket.interactablesSelected[0].transform.GetComponentInChildren<PizzaBox>();

            if(pizzaBox != null)
            {
                gameManager.DeliverPizza(pizzaBox.pizza, pizzaBox.order.order);
                socket.interactablesSelected[0] = null;
                Destroy(pizzaBox);
            }
        }
    }
}
