using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PopulateInventory : MonoBehaviour
{
    public GameObject prefab;
    public static Tile tilePick;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void Populate() { //https://www.youtube.com/watch?v=kdkrjCF0KCo
        GameObject newObj;

        foreach (var (itemName, amount) in inventory)
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = GlobalMethods.getSpriteByName(itemName);
            //newObj.AddComponent<ChooseBlock>();
            // Now use the Button to run a function in ChooseBrick.
            newObj.AddComponent<Button>();
            //newObj.GetComponent<Button>().onClick.AddListener(delegate { ChooseBrick(spritei.name);});
            
            newObj.GetComponent<Button>().onClick.AddListener(delegate { chooseItem(itemName, amount);});
        }
    }

    public void chooseItem(string item, int amount) {
        Debug.Log(item + " was choosen!");
    }
}
