using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{

    public Toggle fullscreenToggle, vsyncToggle;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;

    public TMP_Text masterVolProLabel;
    public Slider masterVolPro;
    public TMP_Text musicVolProLabel;
    public Slider musicVolPro;
    public TMP_Text sfxVolProLabel;
    public Slider sfxVolPro;
    public GameObject audioEmitter;
    public static OptionsScreen Instance;

    public float masterVal = 100;
    public float musicVal = 100;
    public float sfxVal = 100;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            masterVal = OptionsScreen.Instance.masterVal;
            musicVal = OptionsScreen.Instance.musicVal;
            sfxVal = OptionsScreen.Instance.sfxVal;
        }
        catch (System.Exception)
        {
        }

        OptionsScreen.Instance = this;

        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0) // no vsync
        {
            vsyncToggle.isOn = false;
        } else {
            vsyncToggle.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;

                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;

            UpdateResLabel();
        }

        masterVolPro.value = masterVal;
        masterVolProLabel.text = masterVolPro.value.ToString()+ " %";

        musicVolPro.value = musicVal;
        musicVolProLabel.text = musicVolPro.value.ToString()+ " %";

        sfxVolPro.value = sfxVal;
        sfxVolProLabel.text = sfxVolPro.value.ToString()+ " %";
        UpdateVolume();

    }

    public void ResLeft() {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight() {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count -1;
        }

        UpdateResLabel();
    }

    public void UpdateResLabel() {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics() {
        //Screen.fullScreen = fullscreenToggle.isOn;
        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1; // vsync on
        } else {
            QualitySettings.vSyncCount = 0; // vsync off
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    }

    private void UpdateVolume() {
        audioEmitter.GetComponents<AudioSource>()[0].volume = masterVolPro.value*0.01f*musicVolPro.value*0.01f;

        //Btn sound
        audioEmitter.GetComponents<AudioSource>()[1].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
        audioEmitter.GetComponents<AudioSource>()[2].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
        audioEmitter.GetComponents<AudioSource>()[3].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
        audioEmitter.GetComponents<AudioSource>()[4].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
        audioEmitter.GetComponents<AudioSource>()[5].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
        audioEmitter.GetComponents<AudioSource>()[6].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
        audioEmitter.GetComponents<AudioSource>()[7].volume = masterVolPro.value*0.01f*sfxVolPro.value*0.01f;
    }

    public void MasterValueChange() {
        masterVolProLabel.text = masterVolPro.value.ToString()+ " %";
        masterVal = masterVolPro.value;
        UpdateVolume();
    }

    public void MusicValueChange() {
        musicVolProLabel.text = musicVolPro.value.ToString()+ " %";
        musicVal = musicVolPro.value;
        UpdateVolume();
    }

    public void SfxValueChange() {
        sfxVolProLabel.text = sfxVolPro.value.ToString()+ " %";
        sfxVal = sfxVolPro.value;
        UpdateVolume();
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
