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

    // Start is called before the first frame update
    void Start()
    {
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
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
