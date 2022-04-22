using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.UI;

public class populateGrid : MonoBehaviour
{
    public GameObject prefab;
    public Image SelectedBrick;

    private MousePosition2D sn;

    public Tile tilePick;
    public String tilePick2;

    public Dictionary<Sprite, Tile> sprites = new Dictionary<Sprite, Tile>();

    private void Start() {
        Populate();
        SelectedBrick.sprite = tilePick.sprite;
        /*MousePosition2D sn = gameObject.GetComponent<MousePosition2D>();
        Debug.Log(sn.GetTileByName("simple_grass_block").name);//*/
    }

    private void Update() {
        
        //sn.tile = tilePick;
    }

    private void GetSpriteByName() {
        sprites.Clear();

        string[] assetFiles = Directory.GetFiles("Assets/Tiles/Assets/"); // Gets string array of the tile assets file path
        
        foreach (var item in assetFiles)
        {
            if (item.EndsWith(".asset"))
            {
                //Debug.Log(item);
                Tile assetTile = (Tile)AssetDatabase.LoadAssetAtPath<Tile>(item); // loads the tile asset from path
                if (assetTile.sprite.name.Contains("brpck"))
                {
                    sprites[assetTile.sprite] = assetTile; // inserts the data into a dictionary
                }
                
            }
        }
    }

    void Populate() { //https://www.youtube.com/watch?v=kdkrjCF0KCo
        GetSpriteByName();
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
        MousePosition2D sn = gameObject.GetComponent<MousePosition2D>();
        SelectedBrick.sprite = tilei.sprite;
        tilePick = tilei;
        tilePick2 = tilei.name;
    }

}

