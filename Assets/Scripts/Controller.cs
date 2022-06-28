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
    public GameObject gameUI;
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

            //In-Game Stuff
            GameTools();
            GameControl();
        }
        
    }

    void GameTools() {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {   
            Debug.Log("Pressed 0");
            GameStateMisc.Build();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Pressed 1");
            GameStateMisc.Move();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Pressed 2");
            GameStateMisc.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Pressed 3");
            GameStateMisc.Break();
        }
    }

    void GameControl() {
        if (Input.GetKey(KeyCode.W))
        {   
            Debug.Log("Pressing W");
            
        }
        if (Input.GetKey(KeyCode.D))
        {   
            Debug.Log("Pressing D");
            
        }
        if (Input.GetKey(KeyCode.S))
        {   
            Debug.Log("Pressing S");
            
        }
        if (Input.GetKey(KeyCode.A))
        {   
            Debug.Log("Pressing A");
            
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
        if (winPick == main)
        {
            gameUI.SetActive(true);
        } else {
            gameUI.SetActive(false);
        }
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

