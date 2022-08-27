using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.UI;

public class populateItemGrid : MonoBehaviour
{
    public GameObject prefab;
    public Image SelectedBrick;
    public TextAsset jsonFile;
    
    public static populateItemGrid Instance;
    public static Sprite tilePick;
    public Dictionary<string, List<Dictionary<string, List<string>>>> craftingRecepie;

    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    private void Start() {
        
        

        craftingRecepie = new Dictionary<string, List<Dictionary<string, List<string>>>>()
        {
            {
                "miner",
                new List<Dictionary<string, List<string>>>()
                {
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Iron_Ore",
                                "Copper_Wire",
                                "Gold_Rod"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Iron_Bar"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "output",
                            new List<string>()
                            {
                                "Iron_Ore"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "output",
                            new List<string>()
                            {
                                "Gold_Ore"
                            }
                        }
                    }
                }
            },
            {
                "smelter",
                new List<Dictionary<string, List<string>>>()
                {
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Iron_Ore"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Iron_Bar"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Gold_Ore"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Gold_Bar"
                            }
                        }
                    }
                }
            }
        };

        populateItemGrid.Instance = this;
        //Populate();
        //SelectedBrick.sprite = MousePosition2D.tile.sprite;
        /*MousePosition2D sn = gameObject.GetComponent<MousePosition2D>();
        Debug.Log(sn.GetTileByName("simple_grass_block").name);//*/
    }

    private void Update() {
        
        SelectedBrick.sprite = populateItemGrid.tilePick;
    }

    private Sprite GetSpriteByName(string key) {
        if (sprites.ContainsKey(key.ToLower()))
        {
            return sprites[key.ToLower()];
        }
        sprites.Clear();

        string[] assetFiles = Directory.GetFiles("Assets/imgs/items/"); // Gets string array of the tile assets file path
        
        foreach (var item in assetFiles)
        {
            if (item.EndsWith(".png"))
            {
                //Debug.Log(item);
                Sprite assetTile = (Sprite)AssetDatabase.LoadAssetAtPath<Sprite>(item); // loads the tile asset from path
                if (assetTile != null) // exclude animation tiles
                {
                    sprites[assetTile.name.ToLower()] = assetTile; // inserts the data into a dictionary
                }
                
                
            }
        }
        if (sprites.ContainsKey(key.ToLower()))
        {
            return sprites[key.ToLower()];
        }else {
            return null;
        }
        
    }

    public void Populate() { //https://www.youtube.com/watch?v=kdkrjCF0KCo
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        GameObject newObj;
        GameObject newObjItem;
        string type;
        if (SelectInspecter.brickSelected.tile.name.ToLower().Contains("miner"))
        {
            type = "miner";
        } else if (SelectInspecter.brickSelected.tile.name.ToLower().Contains("smelter"))
        {
            type = "smelter";
        } else {
            type = "miner";
        }

        foreach (var recepie in craftingRecepie[type])
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = GetSpriteByName("crafting-window");

            if (recepie.ContainsKey("input"))
            {
                int inputLoopNum = 0;
                foreach (var item in recepie["input"])
                {
                    
                    newObjItem = (GameObject)Instantiate(prefab, newObj.transform);
                    newObjItem.GetComponent<Image>().sprite = GetSpriteByName(item);
                    newObjItem.transform.Translate(new Vector3(-21.2f+12.2f*inputLoopNum, 3f, 0f));
                    inputLoopNum++;
                }
            }
            if (recepie.ContainsKey("output"))
            {
                var item = recepie["output"][0];
                newObjItem = (GameObject)Instantiate(prefab, newObj.transform);
                newObjItem.GetComponent<Image>().sprite = GetSpriteByName(item);
                newObjItem.transform.Translate(new Vector3(22.3f, 3f, 0f));
            }
            

            newObj.AddComponent<Button>();
            //newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseBrick(spritei.name);});
            
            newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseMethod(recepie); });

            
        }
    }

    public void ChooseMethod(Dictionary<string, List<string>> recepie)
    {
        Debug.Log(recepie["output"][0]);
        SelectInspecter.brickSelected.crafting["output"] = recepie["output"];
        SelectInspecter.brickSelected.crafting["input"] = recepie["input"];
    }

}

