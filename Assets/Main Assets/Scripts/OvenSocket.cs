using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class OvenSocket : MonoBehaviour
{
    [SerializeField] private AudioClip ambienceSound;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    private XRSocketInteractor socket;
    private Pizza selectedPizza;

    private void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(UpdateSocketContent);
        socket.selectExited.AddListener(UpdateSocketContent);
        AudioManager.CreateLocalAudioSource(gameObject, audioMixerGroup,
            ambienceSound, maxDistance:5f, loop:true, playOnAwake:true);
        
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
