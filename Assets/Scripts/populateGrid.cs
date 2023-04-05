using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class populateGrid : MonoBehaviour
{
    public GameObject prefab;
    public Image SelectedBrick;

    public static Tile tilePick;
    public static populateGrid Instance;

    public Dictionary<Sprite, Tile> sprites = new Dictionary<Sprite, Tile>();

    private void Start() {
        //Populate();
        //SelectedBrick.sprite = MousePosition2D.tile.sprite;
        /*MousePosition2D sn = gameObject.GetComponent<MousePosition2D>();
        Debug.Log(sn.getTileByName("simple_grass_block").name);//*/
        populateGrid.Instance = this;
    }

    private void Update() {
        
        //sn.tile = tilePick;
        SelectedBrick.sprite = General.tile.sprite;
    }

    private void loadSprites() {
        sprites.Clear();

        //Debug.Log(GlobalMethods.getTiles().Count());
        foreach (var item in GlobalMethods.getTiles())
        {
            //Debug.Log(item);
            if (item.sprite.name.Contains("brpck"))
            {
                //Debug.Log(item);
                sprites[item.sprite] = item; // inserts the data into a dictionary
            }
                
        }
    }

    public void Populate() { //https://www.youtube.com/watch?v=kdkrjCF0KCo
        loadSprites();
        GameObject newObj;

        foreach (var (spritei, tilei) in sprites)
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = spritei;
            //newObj.AddComponent<ChooseBlock>();
            // Now use the Button to run a function in ChooseBrick.
            newObj.AddComponent<Button>();
            //newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseBrick(spritei.name);});
            
            newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseBrick(spritei.name, tilei);});
        }
    }

    public void ChooseBrick(string brickName, Tile tilei) {
        Debug.Log(tilei.name + " was choosen!");
        tilePick = tilei;
        General.tile = tilei;
    }

}

