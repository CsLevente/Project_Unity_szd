using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour{

    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public Slider volumeSlider;
    public TMPro.TMP_Dropdown graphicsDropdown;
    public TMPro.TMP_Dropdown mapsizeDropdown;

    Resolution[] resolutions;
    void Start(){

        fullScreenToggle.isOn = GameObject.Find("SettingManager").GetComponent<SettingManager>().isFullscreen;
        volumeSlider.value = GameObject.Find("SettingManager").GetComponent<SettingManager>().volume;
        mapsizeDropdown.value = GameObject.Find("SettingManager").GetComponent<SettingManager>().mapsize;
        graphicsDropdown.value = GameObject.Find("SettingManager").GetComponent<SettingManager>().qualityIndex;


    }

    public void SetResolution (int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume (float volume){
        GameObject.Find("SettingManager").GetComponent<SettingManager>().VolumeChange(volume);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex); 
        GameObject.Find("SettingManager").GetComponent<SettingManager>().qualityIndex = qualityIndex;
    }

    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        GameObject.Find("SettingManager").GetComponent<SettingManager>().isFullscreen = isFullscreen;
    }
    
    public void SetMapsize (int num){
        GameObject.Find("SettingManager").GetComponent<SettingManager>().mapsize = num;

    }
}
