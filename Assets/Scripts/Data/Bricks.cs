//using System.Threading;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

//[CreateAssetMenu(fileName = "GameSence", menuName = "Game/Bricks")]
public class Bricks
{
    public Tile tile;
    public Vector3Int cordinates;
    public List<string> directions;
    public List<string> inputDirections;
    public List<string> outputDirections;

    public Dictionary<string, List<string>> crafting = new Dictionary<string, List<string>>()
    {
        {
            "input",
            new List<string>() {null}
        },
        {
            "output",
            new List<string>() {null}
        }
    };

    

    private string currentTag;

    public Bricks(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir)
    {
        tile = cTile;
        cordinates = coords;
        changeInputDir(inputDir);
        changeOutputDir(outputDir);
        directions = dir;
        //General.bricks[cordinates] = this;

        if (tile != null)
        {
            

            /*
            if (tile.name.ToLower().Contains("slant")) {
                //linkedBrick.belt.addToBelt(linkedBrick);
                //Debug.Log("Belt Length: "+belt.subCordinates.Count.ToString());

                //Debug.Log(linkedBrick.outputDirections.Count.ToString()); // should not be 0 FIX!!!
                //Debug.Log(linkedBrick.inputDirections.Count.ToString()); // should not be 0 FIX!!!
            }//*/
            

            //*
            // if statement is for adjusting direction on nearby belt if not conveyor
            if (GlobalMethods.isBrickNotExcludedType(this.tile.name, "conveyor"))
            {
                if (GlobalMethods.getBelt(this.tile.name, cordinates, true) != null)
                {
                    GlobalMethods.getBelt(this.tile.name, cordinates, true).assignDirection(this);
                }
            }//*/
        }


        
    }

    public virtual void changeTileTag(string tag, bool temp = false) {
        if (tile != null)
        {
            if (tag == null)
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.getTileByName(GlobalMethods.removeTagFromBlockName(tile.name))); // Set tile to original type
                if (!temp)
                {
                    currentTag = tag;
                }
                return;
            }
            string newBrickName = GlobalMethods.addTagToBlockName(tile.name, tag);
            if (tag.Contains("animated"))
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.getAnimatedTileByName(newBrickName));
            } else {
                General.Instance.map.SetTile(cordinates, GlobalMethods.getTileByName(newBrickName));
            }
            if (!temp)
            {
                currentTag = tag;
            }
        }
        
    }

    public string getTileTag() {
        string blockName = getTileName();
        if (doesHaveTag())
        {
            List<string> name = blockName.Split('_').ToList();
            //Debug.Log(name[name.Count - 2]);
            return name[name.Count - 2];
        }
        return null;
    }

    private bool doesHaveTag() {
        string blockName = getTileName();
        //Debug.Log("TTT: "+blockName);
        if (blockName.Contains("selected") || blockName.Contains("selectorredbox") || blockName.Contains("selectorbox") || blockName.Contains("animated"))
        {
            return true;
        }
        return false;
    }

    public void resetTileTag() {
        if (tile != null)
        {
            if (currentTag == null)
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.getTileByName(GlobalMethods.removeTagFromBlockName(tile.name))); // Set tile to original type
                return;
            }
            string newBrickName = GlobalMethods.addTagToBlockName(tile.name, currentTag);
            if (currentTag.Contains("animated"))
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.getAnimatedTileByName(newBrickName));
            } else {
                General.Instance.map.SetTile(cordinates, GlobalMethods.getTileByName(newBrickName));
            }
        }
    }

    public void changeInputDir(List<string> dirs) {
        inputDirections = dirs;
    }

    public virtual void changeOutputDir(List<string> dirs) {
        outputDirections = dirs;
    }

    // can i use async here??? last time i tried it the entire unity program crashed.
    

    


    public virtual void receiveItem(string item) {
        moveToNext(item);
    }

    
    

    

    public string getName() {
        return tile.name.ToLower();
    }

    public string getTileName() {
        return General.Instance.map.GetTile(cordinates).name.ToLower();
    }
    
    

    

    

    private protected virtual void moveToNext(string item) {
        if (outputDirections != null)
        {
            foreach (string outputDir in outputDirections)
            {
                var itemHandler = GlobalMethods.getBrickByDirCord(outputDir, cordinates);
                if (!(itemHandler == null || itemHandler.ifStorageFull(item))) // if path is full or if there is no path at all
                {
                    outputDirections.Remove(outputDir); // remove from output dirs
                    outputDirections.Add(outputDir); // add the removed to the end of list - why? because: that way it will rotate and not send everything though only one way untill full
                    itemHandler.receiveItem(item);
                    break;
                }
            }
        } else {
            Debug.Log("!!!ERROR!!! - FIX ME");
        }
    }

    public virtual bool ifStorageFull(string item) {
        if (outputDirections != null)
        {
            foreach (string outputDir in outputDirections)
            {
                var itemHandler = GlobalMethods.getBrickByDirCord(outputDir, cordinates);
                if (itemHandler == null || itemHandler.ifStorageFull(item)) // if path is full or if there is no path at all
                {
                    if (outputDirections.Last() == outputDir)
                    {
                        return true;
                    }
                } else {
                    return false;
                }
            } 
        }
        return true;
    }
}
