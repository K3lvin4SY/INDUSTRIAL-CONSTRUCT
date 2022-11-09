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
                //linkedBrick.belt.AddToBelt(linkedBrick);
                //Debug.Log("Belt Length: "+belt.subCordinates.Count.ToString());

                //Debug.Log(linkedBrick.outputDirections.Count.ToString()); // should not be 0 FIX!!!
                //Debug.Log(linkedBrick.inputDirections.Count.ToString()); // should not be 0 FIX!!!
            }//*/
            

            //*
            if (GlobalMethods.isBrickNotExcludedType(this.tile.name, "conveyor"))
            {
                if (GlobalMethods.GetBelt(this.tile.name, cordinates, true) != null)
                {
                    GlobalMethods.GetBelt(this.tile.name, cordinates, true).assignDirection(this);
                }
            }//*/
        }


        
    }

    public virtual void changeTileTag(string tag, bool temp = false) {
        if (tile != null)
        {
            if (tag == null)
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.GetTileByName(GlobalMethods.RemoveTagFromBlockName(tile.name))); // Set tile to original type
                if (!temp)
                {
                    currentTag = tag;
                }
                return;
            }
            string newBrickName = GlobalMethods.AddTagToBlockName(tile.name, tag);
            if (tag.Contains("animated"))
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.GetAnimatedTileByName(newBrickName));
            } else {
                General.Instance.map.SetTile(cordinates, GlobalMethods.GetTileByName(newBrickName));
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
                General.Instance.map.SetTile(cordinates, GlobalMethods.GetTileByName(GlobalMethods.RemoveTagFromBlockName(tile.name))); // Set tile to original type
                return;
            }
            string newBrickName = GlobalMethods.AddTagToBlockName(tile.name, currentTag);
            if (currentTag.Contains("animated"))
            {
                General.Instance.map.SetTile(cordinates, GlobalMethods.GetAnimatedTileByName(newBrickName));
            } else {
                General.Instance.map.SetTile(cordinates, GlobalMethods.GetTileByName(newBrickName));
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
    

    


    // below is only methods for splitter & merger

    public virtual void receiveItem(string item) {
        moveToNext(item);
    }

    
    

    

    public string getName() {
        return tile.name.ToLower();
    }

    public string getTileName() {
        return General.Instance.map.GetTile(cordinates).name.ToLower();
    }
    
    

    private protected int connectedPaths() {
        int amount = 0;
        foreach (var dir in inputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, cordinates)) && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount += 1;
            }
        }
        return amount;
    }

    private protected List<Bricks> connectedOutputPaths() {
        List<Bricks> amount = new List<Bricks>();
        foreach (var dir in outputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, cordinates)) && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount.Add(General.bricks[GlobalMethods.GetDirV3(dir, cordinates)]);
            }
        }
        return amount;
    }

    private protected Dictionary<string, string> connectedPathsItems() {
        Dictionary<string, string> amount = new Dictionary<string, string>();
        foreach (var dir in inputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, cordinates)) && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount[dir] = General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].GetItem(true, 1);
                //Debug.Log("BeltLast: "+General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].belt.storage[General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].belt.storage.Count-1]);
                //Debug.Log("BeltLast: "+General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].belt.subCordinates.Last().GetItem(true, 1));
                //Debug.Log("Brickitem: "+amount[dir]);
            } else {
                amount[dir] = null;
            }
        }
        return amount;
    }

    

    private protected virtual void moveToNext(string item) {
        if (outputDirections != null)
        {
            foreach (string outputDir in outputDirections)
            {
                var itemHandler = GlobalMethods.GetBrickByDirCord(outputDir, cordinates);
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
                var itemHandler = GlobalMethods.GetBrickByDirCord(outputDir, cordinates);
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
