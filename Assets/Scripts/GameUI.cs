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
        SelectedBrick.sprite = MousePosition2D.tile.sprite;
    }
}
