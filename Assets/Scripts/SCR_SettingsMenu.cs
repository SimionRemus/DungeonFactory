using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SCR_SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    private Resolution[] resolutions;

    public Dropdown resolutionDD;

    public void Start()
    {
        SetMasterVolume(0.8f);
        SetMusicVolume(0.8f);
        SetEffectsVolume(0.8f);
        resolutions = Screen.resolutions;
        resolutionDD.ClearOptions();
        List<string> resolutionList = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionList.Add(resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "Hz");
            if (resolutions[i].width==Screen.currentResolution.width && resolutions[i].height==Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDD.AddOptions(resolutionList);
        resolutionDD.value = currentResolutionIndex;
        resolutionDD.RefreshShownValue();
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetEffectsVolume(float volume)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }

    public void SetGFXQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height,Screen.fullScreen);
    }
}
