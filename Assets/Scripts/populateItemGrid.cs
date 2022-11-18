using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class populateItemGrid : MonoBehaviour
{
    public GameObject prefab;
    
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
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "output",
                            new List<string>()
                            {
                                "Copper_Ore"
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
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Copper_Ore"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Copper_Bar"
                            }
                        }
                    }
                }
            },
            {
                "constructer",
                new List<Dictionary<string, List<string>>>()
                {
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Iron_Bar"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Iron_Plate"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Gold_Bar"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Gold_Plate"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Iron_Bar"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Iron_Rod"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Gold_Bar"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Gold_Rod"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Iron_Bar"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Spikes"
                            }
                        }
                    },
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Copper_Bar"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Copper_Wire"
                            }
                        }
                    }
                }
            },
            {
                "fabricator",
                new List<Dictionary<string, List<string>>>()
                {
                    new Dictionary<string, List<string>>()
                    {
                        {
                            "input",
                            new List<string>()
                            {
                                "Iron_Plate",
                                "Copper_Wire",
                                "Spikes"
                            }
                        },
                        {
                            "output",
                            new List<string>()
                            {
                                "Processor"
                            }
                        }
                    }
                }
            }
        };

        populateItemGrid.Instance = this;
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
        } else if (SelectInspecter.brickSelected.tile.name.ToLower().Contains("constructer"))
        {
            type = "constructer";
        } else if (SelectInspecter.brickSelected.tile.name.ToLower().Contains("fabricator"))
        {
            type = "fabricator";
        } else {
            type = "miner";
        }

        foreach (var recepie in craftingRecepie[type])
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = GlobalMethods.GetSpriteByName("crafting-window");

            if (recepie.ContainsKey("input"))
            {
                int inputLoopNum = 0;
                foreach (var item in recepie["input"])
                {
                    
                    newObjItem = (GameObject)Instantiate(prefab, newObj.transform);
                    newObjItem.GetComponent<Image>().sprite = GlobalMethods.GetSpriteByName(item);
                    newObjItem.transform.Translate(new Vector3(-21.2f+12.2f*inputLoopNum, 3f, 0f));
                    inputLoopNum++;
                }
            }
            if (recepie.ContainsKey("output"))
            {
                var item = recepie["output"][0];
                newObjItem = (GameObject)Instantiate(prefab, newObj.transform);
                newObjItem.GetComponent<Image>().sprite = GlobalMethods.GetSpriteByName(item);
                newObjItem.transform.Translate(new Vector3(22.3f, 3f, 0f));
            }
            

            newObj.AddComponent<Button>();
            //newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseBrick(spritei.name);});
            
            newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseMethod(recepie); });

            
        }
    }

    public void ChooseMethod(Dictionary<string, List<string>> recepie)
    {
        dynamic machine;
        if (SelectInspecter.brickSelected.tile.name.ToLower().Contains("fabricator"))
        {
            machine = SelectInspecter.fabricatorSelected;
        } else {
            machine = SelectInspecter.brickSelected;
        }
        if (recepie.ContainsKey("output"))
        {
            machine.crafting["output"] = recepie["output"];
        } else {
            machine.crafting["output"] = new List<string>();
        }
        if (recepie.ContainsKey("input"))
        {
            machine.crafting["input"] = recepie["input"];
        } else {
            machine.crafting["input"] = new List<string>();
        }

        Controller.Instance.UseWindow(Controller.Instance.selectInspector);
    }

}

