using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject brickPicker;
    public GameObject main;
    public GameObject grid;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    List<GameObject> gameWindows;
    void Start()
    {
        gameWindows = new List<GameObject> {brickPicker, main, grid, pauseMenu, optionsMenu};
        brickPicker.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        main.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenuHandler();
        if (!pauseMenu.activeSelf) // only activate if game isn't paused
        {
            BrickSelector();
        }
        
    }

    void BrickSelector() {
        if (Input.GetKeyDown(KeyCode.E))
        {   
            Debug.Log("Pressed E");
            UseWindow(brickPicker);
        }
    }

    void PauseMenuHandler() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UseWindow(pauseMenu);
        }
    }
    public void Resume() {
        UseWindow(pauseMenu);
    }

    public void Options() {
        UseWindow(optionsMenu);
    }

    public void UseWindow(GameObject winPick) {
        if (winPick.activeSelf)
        {
            if (winPick == optionsMenu)
            {
                UseWindow(pauseMenu);
                return;
            }
            UseWindow(main);
            return;
        }
        foreach (var win in gameWindows)
        {
            if (!(win == winPick))
            {
                if (win == grid)
                {
                    if (winPick == main)
                    {
                        win.GetComponent<SortingGroup>().enabled = false;
                    } else {
                        win.GetComponent<SortingGroup>().enabled = true;
                    }
                } else {
                    win.SetActive(false);
                }
            }
        }
        winPick.SetActive(true);
    }
}

