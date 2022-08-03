using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public static Controller Instance;
    public GameObject brickPicker;
    public GameObject main;
    public GameObject gameUI;
    public GameObject grid;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject selectInspector;
    List<GameObject> gameWindows;
    List<GameObject> inGameWindows;

    public static bool shiftPressed = false;
    float moveSpeed;
    void Start()
    {
        gameWindows = new List<GameObject> {brickPicker, main, grid, pauseMenu, optionsMenu, selectInspector};
        inGameWindows = new List<GameObject> {brickPicker, selectInspector};
        brickPicker.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        selectInspector.SetActive(false);
        main.SetActive(true);
        Controller.Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenuHandler();
        if (!(pauseMenu.activeSelf || optionsMenu.activeSelf))
        {
            BrickSelector();
        }
        if (main.activeSelf) // only activate if game isn't paused
        {
            

            //In-Game Stuff
            GameTools();
            GameControl();
        }
        
    }

    void GameTools() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {   
            Debug.Log("Pressed 1");
            GameStateMisc.Build();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Pressed 2");
            GameStateMisc.Move();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Pressed 3");
            GameStateMisc.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Pressed 4");
            GameStateMisc.Break();
        }
    }

    void GameControl() {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Controller.shiftPressed = true;
            moveSpeed = 1.0f;
        } else {
            moveSpeed = 0.5f;
            Controller.shiftPressed = false;
        }
        if (Input.GetKey(KeyCode.W))
        {   
            grid.transform.Translate(0f, -moveSpeed, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {   
            grid.transform.Translate(-moveSpeed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {   
            grid.transform.Translate(0f, moveSpeed, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {   
            grid.transform.Translate(moveSpeed, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                General.Instance.UpdateControlZ();
            }

        if (Input.GetKeyDown(KeyCode.R))
        {
            /*GameSenceHandler gmh = new GameSenceHandler();
            BoundsInt cellBox = new BoundsInt(gmh.GetDirV3("SW", selectorLocation), gmh.makeV3Int(3, 3, 4));
            tileBox = map.GetTilesBlock(cellBox);
            tileBoxCheck = tileBox.Select(s => s == null).ToArray();//*/
            General.Instance.rotateBrick();
        }

        //*
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // scroll up z pos
        {
            General.Instance.ScrollWheelZPos(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // scroll down z pos
        {   
            General.Instance.ScrollWheelZPos(-1);
        }


        //Debug.Log(Input.mousePosition);
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
            General.Instance.MouseClick();
        }//*/
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
        if (winPick == pauseMenu) {
            foreach (var win in inGameWindows)
            {
                if (win.activeSelf) {
                    UseWindow(main);
                    return;
                }
            }
        }//*/
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

