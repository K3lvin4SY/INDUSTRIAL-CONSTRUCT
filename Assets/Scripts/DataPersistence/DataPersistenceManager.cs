using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    public GameData gameData;

    private void Awake() {
        if (instance != null)
        {
            Debug.Log("More than one manager in the scene");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // TODO : load saved data from a file using the data handler
        // if no data can be loaded, then initialize a new game
        if (this.gameData == null)
        {
            Debug.Log("No data found. Creating new game.");
            NewGame();
        }

        // TODO : Push the loaded data to all other scripts that need it
    }

    public void SaveGame()
    {
        // TODO : pass the data to other scripts so thay can update it

        // TODO : save that data to a file using the data handler
    }
}
