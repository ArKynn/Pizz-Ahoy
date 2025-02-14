using Main_Assets.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Cannon : MonoBehaviour
{
    private ParticleSystem _explosionParticle;
    private GameManager gameManager;
    private XRSocketInteractor socket;

    private void Start()
    {
        _explosionParticle = GetComponent<ParticleSystem>();
        gameManager = FindAnyObjectByType<GameManager>();
        socket = GetComponentInChildren<XRSocketInteractor>();

        socket.selectEntered.AddListener(ShootPizza);
    }

    private void ShootPizza(SelectEnterEventArgs args)
    {
        if(socket.hasSelection)
        {
            PizzaBox pizzaBox = socket.interactablesSelected[0].transform.GetComponent<PizzaBox>();
            
            if(pizzaBox != null)
            {
                gameManager.DeliverPizza(pizzaBox.pizza, pizzaBox.order.order);
                socket.enabled = false;
                Destroy(pizzaBox.order);
                Destroy(pizzaBox.pizza);
                Destroy(pizzaBox.gameObject);
                _explosionParticle.Play();
            }
        }
        
        socket.enabled = true;
    }
}
