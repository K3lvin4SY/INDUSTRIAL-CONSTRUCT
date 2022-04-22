using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChooseBlock : MonoBehaviour
{
    // Start is called before the first frame update
    private Tile tile;
    void Start()
    {
        /*tile = FindObjectOfType<Tile>();
        MousePosition2D sn = gameObject.GetComponent<MousePosition2D>();
        Debug.Log(tile);
        sn.tile = tile;*/
    }

    public void ChooseBrick(string name) {
        Debug.Log(name + " was choosen!");
        Debug.Log(name + " was choosen!2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
