using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Converter : Bricks
{

    public List<string> inStorage = new List<string>(); // only for converters and other machines
    public List<string> outStorage = new List<string>(); // only for miners, converters and other machines
    public Converter(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null) : base(cTile, coords, dir, inputDir, outputDir) {
        General.bricks[coords] = this;
        General.tickers[coords] = this;
    }

    public override void receiveItem(string item)
    {
        processConvertion(item);
    }

    private protected void processConvertion(string item) {
        if (item != null)
        {
            collectItem(item);
        }
        convertItem();
        if (outStorage.Count >= 1)
        {
            string newItem = outStorage[outStorage.Count-1];
            if (moveToNextCheck(item))
            {
                outStorage.RemoveAt(outStorage.Count-1); // cant remove here
                moveToNext(newItem);
            }
        } else {
            moveToNext(null);
        }
    }

    private void collectItem(string item) {
        inStorage.Add(item);
        //Debug.Log("InItem: " + item);
    }

    private bool moveToNextCheck(string item) {
        if (outputDirections != null)
        {
            foreach (var oDir in outputDirections)
            {
                var itemHandler = GlobalMethods.GetBrickByDirCord(oDir, cordinates);
                if (itemHandler == null || itemHandler.ifStorageFull(item)) // if path is full or if there is no path at all
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private void convertItem() {
        string tag;
        if (inStorage.Count == 0 || outStorage.Count >= 100)
        {
            //Debug.Log("Nothing to convert or cant");
            if (getName().Contains("smelter"))
            {
                tag = getTileTag();
                if (tag == null)
                {
                    tag = null;
                } else if (tag == "animated+fireup")
                {
                    tag = "animated+shutdown";
                } else if (tag == "animated+shutdown")
                {
                    tag = null;
                } else if (tag == "animated+on")
                {
                    tag = "animated+shutdown";
                } else {
                    tag = null;
                }
            } else {
                tag = null;
            }
            //Debug.Log("off: "+tag);
            changeTileTag(tag, true);
            return;
        }
        inStorage.RemoveAt(inStorage.Count-1);
        // Generate Produced item
        if (getName().Contains("smelter"))
        {
            tag = getTileTag();
            if (tag == null)
            {
                tag = "animated+fireup";
            } else if (tag == "animated+fireup")
            {
                tag = "animated+on";
            } else if (tag == "animated+shutdown")
            {
                tag = "animated+fireup";
            } else if (tag == "animated+on")
            {
                tag = "animated+on";
            }
        } else {
            tag = "animated";
        }
        //Debug.Log("on: "+tag);
        changeTileTag(tag, true);
        foreach (var item in crafting["output"])
        {
            outStorage.Add(item);
            //Debug.Log("OutItem: " + item);
        }
    }

    public override bool ifStorageFull(string item)
    {
        if (item == null)
        {
            return false;
        }
        if (crafting["input"][0] == item)
        {
            if (inStorage.Count >= 100)
            {
                return true;
            } else {
                return false;
            }
        } else {
            return true;
        }
    }
}
