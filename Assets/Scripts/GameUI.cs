using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public Image SelectedBrick;

    // Update is called once per frame
    void Update()
    {
        if (General.Instance.gameState == "build")
        {
            SelectedBrick.sprite = General.tile.sprite;
            
            if (SelectedBrick.enabled == false) {
                SelectedBrick.enabled = true;
            }
            
        } else if (General.Instance.gameState == "select")
        {
            //show selected brick

            if (SelectedBrick.enabled == false) {
                SelectedBrick.enabled = true;
            }
        } else {
            SelectedBrick.sprite = null;
            if (SelectedBrick.enabled == true) {
                SelectedBrick.enabled = false;
            }
        }
    }
}
