using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public enum TurnMode {Smooth, Snap}

    [SerializeField] private AudioMixer globalAudioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Dropdown turnModeDropdown;
    [SerializeField] private Toggle joystickMovementToggle;

    private TurnMode turnMode;
    public TurnMode CameraTurnMode {get => turnMode;}
    private bool joystickMovementEnabled;
    public bool JoystickMovementEnabled {get => joystickMovementEnabled;}

    private void Start()
    {
        if(PlayerPrefs.HasKey("TurnMode"))
        {
            ToggleTurnMode(PlayerPrefs.GetInt("TurnMode", 0));
        }

        if(PlayerPrefs.HasKey("JoystickMovement"))
        {
            ToggleJoystickMovement(PlayerPrefs.GetInt("JoystickMovement", 0) == 0);
        }

        if(PlayerPrefs.HasKey("MasterVolume"))
        {
            globalAudioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0));
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0);
        }

        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            globalAudioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 0));
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0);
        }

        if(PlayerPrefs.HasKey("AmbienceVolume"))
        {
            globalAudioMixer.SetFloat("AmbienceVolume", PlayerPrefs.GetFloat("AmbienceVolume", 0));
            ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume", 0);
        }

        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            globalAudioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 0));
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
        }

        PlayerPrefs.Save();
    }

    public void ToggleTurnMode(int newTurnMode)
    {
        PlayerPrefs.SetInt("TurnMode", newTurnMode);
        turnModeDropdown.value = newTurnMode;

        switch(newTurnMode)
        {
            case 0:
                turnMode = TurnMode.Snap;
                break;

            case 1:
                turnMode = TurnMode.Smooth;
                break;
        }
    }

    public void ToggleJoystickMovement(bool toggle)
    {
        joystickMovementEnabled = toggle;
        joystickMovementToggle.isOn = toggle;
        
        if(joystickMovementEnabled)
            PlayerPrefs.SetInt("JoystickMovement", 0);
        
        else
            PlayerPrefs.SetInt("JoystickMovement", 1);
    }

    /// <summary>
    /// Changes the Master volume from all Mixers in the game.
    /// </summary>
    public void ChangeMasterVolume()
    {
        globalAudioMixer.SetFloat("MasterVolume", Mathf.Log10(masterSlider.value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the Music volume from the specified music Mixers.
    /// </summary>
    public void ChangeMusicVolume()
    {
        globalAudioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the Master volume from all Mixers in the game.
    /// </summary>
    public void ChangeAmbienceVolume()
    {
        globalAudioMixer.SetFloat("AmbienceVolume", Mathf.Log10(ambienceSlider.value) * 20);
        PlayerPrefs.SetFloat("AmbienceVolume", ambienceSlider.value);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the Music volume from the specified music Mixers.
    /// </summary>
    public void ChangeSFXVolume()
    {
        globalAudioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.Save();
    }
}
