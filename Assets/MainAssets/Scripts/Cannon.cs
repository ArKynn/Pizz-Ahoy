using Main_Assets.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Cannon : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioSource _source;
    private ParticleSystem _explosionParticle;
    private GameManager gameManager;
    private XRSocketInteractor socket;
    private AudioManager audioManager;

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
                Destroy(pizzaBox.order.gameObject);
                Destroy(pizzaBox.pizza.gameObject);
                Destroy(pizzaBox.gameObject);
                _explosionParticle.Play();
                AudioManager.PlayLocalSound(_source, _clip);
            }
        }
        
        socket.enabled = true;
    }
}
