using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;

    private void Start() {
        options.SetActive(false);
    }
    public void playBtn() {
        SceneManager.LoadScene(1);
    }

    public void quitBtn() {
        Application.Quit();
    }

    public void toggleOptionsBtn() {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
            options.SetActive(true);
        } else {
            options.SetActive(false);
            menu.SetActive(true);
        }
    }
}
