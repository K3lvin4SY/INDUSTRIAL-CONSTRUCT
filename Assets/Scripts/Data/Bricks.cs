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
    public Belt belt;
    public Tile tile;
    public Vector3Int cordinates;
    public List<string> directions;
    public List<string> inputDirections;
    private int timesRun = 0; // only for item system
    private Dictionary<string, string> itemsToChoose = null; // only for item system
    public List<string> outputDirections;
    public Bricks linkedBrick;

    public Dictionary<string, List<string>> crafting = new Dictionary<string, List<string>>()
    {
        {
            "input",
            new List<string>()
        },
        {
            "output",
            new List<string>()
        }
    };

    public List<string> inStorage; // only for converters and other machines
    public List<string> outStorage; // only for miners, converters and other machines

    private string currentTag;

    public bool powerOn = false;

    public Bricks(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null)
    {
        tile = cTile;
        cordinates = coords;
        changeInputDir(inputDir);
        changeOutputDir(outputDir);
        directions = dir;
        belt = cBelt;
        linkedBrick = linkBrick;
        General.bricks[cordinates] = this;

        if (tile != null)
        {
            
            if (tile.name.ToLower().Contains("slant")) // creates an invicibale brick if it is a slant brick
            {
                directions.Remove(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString()));
                directions.Add("U");

                linkedBrick = new Bricks(null, GlobalMethods.GetDirV3("U", cordinates), new List<string>() { "D", GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString() }, null, null, belt, this);
                General.bricks[GlobalMethods.GetDirV3("U", cordinates)] = linkedBrick;
                //Debug.Log(inputDirections);
                //Debug.Log(outputDirections);
                if (inputDirections != null || outputDirections != null)
                {
                    Debug.Log("!!!PASS!!!");
                    linkedBrick.changeOutputDir(new List<string>());
                    linkedBrick.changeInputDir(new List<string>());
                    if (inputDirections[0] == tile.name[0].ToString())
                    {
                        outputDirections.RemoveAt(0);
                        outputDirections.Add("U");
                        linkedBrick.inputDirections.Add("D");
                        linkedBrick.outputDirections.Add(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString());
                    }
                    else
                    {
                        inputDirections.RemoveAt(0);
                        inputDirections.Add("U");
                        linkedBrick.inputDirections.Add(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString());
                        linkedBrick.outputDirections.Add("D"); // Not working ?
                    }
                }
            }

            if (belt == null && tile.name.ToLower().Contains("conveyor")) // if its a new conveyor it creates a belt for itself
            {
                Debug.Log("!!!NEW BELT!!!");
                belt = new Belt(new List<Bricks>() {this});
                //*
                if (linkedBrick != null)
                {
                    belt.AddToBelt(linkedBrick); // adds linkedBrick to belt
                }//*/
            } else if (belt != null && tile.name.ToLower().Contains("conveyor")) {
                if (linkedBrick == null)
                {
                    belt.AddToBelt(this);
                }

                if (linkedBrick != null) // if conveyor plack has a linked brick it adds the linked brick to the belt
                {
                    if (tile != null)
                    {
                        //Debug.Log("!!!Belt Add SUCCESS!!!");
                        if (belt.AddToBeltCheck(this))
                        {
                            belt.AddToBelt(this);
                            belt.AddToBelt(linkedBrick);
                        } else {
                            belt.AddToBelt(linkedBrick);
                            belt.AddToBelt(this);
                        }
                    }
                }
                /*else if (linkedBrick.tile == null)
                {
                    belt.AddToBelt(this);
                    /*
                    if (linkedBrick.tile.name.ToLower().Contains("slant"))
                    {
                        belt.AddToBelt(this); 
                    } 
                }*/
                
                //Debug.Log(belt.subCordinates.Count.ToString());
                Debug.Log("Belt Length: "+belt.subCordinates.Count.ToString());
            }

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


        if (tile == null || ((inputDirections != null || outputDirections != null) && tile.name.ToLower().Contains("conveyor"))) // here only conveyor bricks with direction passes through
        {
            if (tile == null)
            {
                //return;
            }
            Debug.Log("p1");
            //*  CODE down bellow may not be finished (its for updating a belt that is already created)
            if (belt != null) {
                if (belt.isBrick(this) != null) // if brick is in end or start of the belt
                {
                    Debug.Log("1");
                    if (belt.getConnectingEdgeBrick(belt.isBrickLast(this)) != null)
                    {
                        Debug.Log("2");
                        Bricks connectionBrick = belt.getConnectingEdgeBrick(belt.isBrickLast(this));
                        if (connectionBrick.inputDirections != null || connectionBrick.outputDirections != null) { // if the brick has direction
                            connectionBrick = belt.getConnectingEdgeBrick(false == belt.isBrickLast(this)); // change the connection brick to the other brick on the oteher side of the belt
                        }
                        if (connectionBrick == null)
                        {
                            connectionBrick = belt.getConnectingEdgeBrick(belt.isBrickLast(this));
                        }
                        if (connectionBrick.tile != null)
                        {
                            Debug.Log(connectionBrick.tile.name);
                        }
                        if (connectionBrick.belt != null)
                        {
                            connectionBrick.belt.assignDirection(this);
                            Debug.Log(connectionBrick.belt.subCordinates.Count.ToString());
                        }
                    }
                }
                Debug.Log("!!!PASSINATION!!!");//*/
                //Debug.Log(GlobalMethods.GetBelt("", cordinates, true, directions).subCordinates.Count.ToString());

                //GlobalMethods.GetBelt("", cordinates, true, directions).assignDirection(this);
            } else {
                Debug.Log("!!!LOOK IN TO PROBLEM!!!");
                Debug.Log(tile);
            }
            
        }

        // This last code for fixing when you connect a belt to an already placed dir brick
        if (tile == null ||  tile.name.ToLower().Contains("conveyor"))
        {
            if (inputDirections != null || outputDirections != null)
            {
                if (belt.noDirection())
                {
                    Debug.Log("p21");
                    changeInputDir(null);
                    changeOutputDir(null);
                    belt.assignDirection(this);
                }
            }
        }
        
        if (belt != null)
        {
            if (belt.faltyDirection())
            {
                Debug.Log(inputDirections);
                //belt.fixFaltyDirection();
            }
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
        Debug.Log("inputDirections");
        Debug.Log(inputDirections);
    }

    public void changeOutputDir(List<string> dirs) {
        outputDirections = dirs;
        if (tile != null && tile.name.ToLower().Contains("conveyor") && !tile.name.ToLower().Contains("eli"))
        {
            if (outputDirections != null)
            {
                string dir = outputDirections[0][0].ToString();
                if (dir == "D" || dir == "U")
                {
                    dir = GlobalMethods.oppositeDir(inputDirections[0]);
                }
                changeTileTag("animated"+dir);
            }
        }
    }

    // can i use async here??? last time i tried it the entire unity program crashed.
    public void GenerateItem() {
        string item;
        if (powerOn)
        {
            item = crafting["output"][0];
            Debug.Log("item generated");
            Debug.Log(Time.time);
        } else {
            item = null;
            //Debug.Log("update sequence");
        }
        //float time1 = Time.time;
        moveToNext(item);
        //Debug.Log(Time.time-time1);
    }

    public string GetItem(bool raw = false) {
        if (belt != null)
        {
            int index = belt.subCordinates.IndexOf(this);
            if (belt.storage[index] != null)
            {
                if (raw)
                {
                    return belt.storage[index];
                } else {
                    return "Item: " + belt.storage[index];
                }
            }
        }
        if (raw)
        {
            return null;
        } else {
            return "Item: None";
        }
    }


    // below is only methods for splitter & merger

    public void receiveItem(string item) {
        if (belt != null)
        {
            belt.receiveItem(item);
        } else {
            moveToNext(item);
        }
    }

    
    public bool mergerAvailable(string item) { // fix this one
        Debug.Log(item);
        return false;
        if (itemsToChoose == null)
        {
            itemsToChoose = connectedPathsItems();
        }
        timesRun += 1;


        if (timesRun == connectedPaths())
        {
            if (itemsToChoose[inputDirections[0]] != null) // if priority one has an item
            {
                removeItemFromBelt(inputDirections[0]);
                receiveItem(itemsToChoose[inputDirections[0]]);
            } else if (itemsToChoose[inputDirections[1]] != null) // if priority two has an item
            {
                removeItemFromBelt(inputDirections[1]);
                receiveItem(itemsToChoose[inputDirections[1]]);
            } else if (itemsToChoose[inputDirections[2]] != null) // if priority three has an item
            {
                removeItemFromBelt(inputDirections[2]);
                receiveItem(itemsToChoose[inputDirections[2]]);
            } else {
                receiveItem(null);
            }
            itemsToChoose = null;
            timesRun = 0;
            return true;
        } else {
            return false;
        }




        /*if (tempInputDirections != inputDirections) // if another path has passed through
        {
            if (timesRun == connectedPaths())
            {
                inputDirections = tempInputDirections;
                timesRun = 0;
            }
            return false;
        }//*/
        /*
        string dir = GlobalMethods.oppositeDir(comingFromDir);

        foreach (var iDir in inputDirections)
        {
            if (itemsToChoose[iDir] != null) // this fixes everything when priority 1/2/3 (loop num) has an item
            {
                if (iDir == dir)
                {
                    string dirToMove = iDir;
                    tempInputDirections.Remove(dirToMove);
                    tempInputDirections.Add(dirToMove);
                }
                if (timesRun == connectedPaths())
                {
                    inputDirections = tempInputDirections;
                    timesRun = 0;
                    itemsToChoose = null;
                }
                if (iDir == dir)
                {
                    return true;
                } else {
                    return false;
                }
            }
        }
        // should only run if all itemsToChoose is null
        if (dir == inputDirections[0])
        {
            string dirToMove = tempInputDirections[0];
            tempInputDirections.Remove(dirToMove);
            tempInputDirections.Add(dirToMove);
        }
        if (timesRun == connectedPaths())
        {
            inputDirections = tempInputDirections;
            timesRun = 0;
            itemsToChoose = null;
        }
        if (dir == inputDirections[0])
        {
            return true;
        } else {
            return false;
        }*/

        /*if (inputDirections[0] == dir && item != null) // if dir is first in place
        {
            string dirToMove = tempInputDirections[0];
            tempInputDirections.Remove(dirToMove);
            tempInputDirections.Add(dirToMove);
            if (timesRun == connectedPaths())
            {
                inputDirections = tempInputDirections;
                timesRun = 0;
            }
            return true;
        }
        if (timesRun == connectedPaths())
        {
            inputDirections = tempInputDirections;
            timesRun = 0;
        }
        return false;
        /*
        foreach (var iDir in inputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(iDir, cordinates))&& General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.GetDirV3(iDir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(iDir))) // if brick exist & it is connected to this brick
            {
                // In here it will only loop how many belts that are connected
                Bricks brick = General.bricks[GlobalMethods.GetDirV3(iDir, cordinates)]; // connected itemhandler
                if (brick.belt != null) // if brick is conveyor and has belt
                {
                    if (brick.belt.storage.Last() != null) // if it has an item to send
                    {
                        //*
                        if (connectedPaths() == timesRun)
                        {
                            inputDirections = tempInputDirections;
                            timesRun = 0;
                        }
                        if (dir == iDir)
                        {
                            string dirToMove = tempInputDirections[0];
                            tempInputDirections.Remove(dirToMove);
                            tempInputDirections.Add(dirToMove);
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
            }
        }
        if (inputDirections[0] == dir)
        {
            string dirToMove = tempInputDirections[0];
            tempInputDirections.Remove(dirToMove);
            tempInputDirections.Add(dirToMove);
            if (connectedPaths() == timesRun)
            {
                inputDirections = tempInputDirections;
                timesRun = 0;
            }
            return true; // the others didnt have anything either
        } else {
            if (connectedPaths() == timesRun)
            {
                inputDirections = tempInputDirections;
                timesRun = 0;
            }
            return false;
        }//*/
    }//*/

    private int connectedPaths() {
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

    private List<Bricks> connectedOutputPaths() {
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

    private Dictionary<string, string> connectedPathsItems() {
        Dictionary<string, string> amount = new Dictionary<string, string>();
        foreach (var dir in inputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, cordinates)) && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount[dir] = General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].GetItem(true);
            } else {
                amount[dir] = null;
            }
        }
        return amount;
    }

    private void removeItemFromBelt(string dir) {
        if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, cordinates)) && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
        {
            Belt belt = General.bricks[GlobalMethods.GetDirV3(dir, cordinates)].belt;
            Debug.Log("Removed:");
            Debug.Log(belt.storage.Count-1);
            Debug.Log(belt.storage[belt.storage.Count-1]);
            
            Debug.Log("start:");
            foreach (var item in belt.storage)
            {
                Debug.Log(item);
            }
            Debug.Log("end:");

            belt.storage.RemoveAt(belt.storage.Count-1/*-1*/); // the last -1 is added because one null came at the end for some rreason
        }
    }

    private void moveToNext(string item) {
        if (outputDirections != null)
        {
            foreach (string outputDir in outputDirections)
            {
                //Debug.Log(outputDir);
                var itemHandler = GlobalMethods.GetBrickByDirCord(outputDir, cordinates);
                if (itemHandler == null || itemHandler.ifStorageFull()) // if path is full or if there is no path at all
                {
                    //Debug.Log("1");
                    if (outputDirections.Last() == outputDir && itemHandler != null)
                    {
                        //Debug.Log("3");
                        if (tile.name.ToLower().Contains("miner"))
                        {
                           Debug.Log("Connot send more from gen"); 
                        } else {
                            Debug.Log("!!!ERROR!!! - FIX ME - isStorageFull check failed");
                        }
                    }
                    continue;
                } else if (outputDirections[0] == outputDir) {
                    //Debug.Log("2");
                    outputDirections.Remove(outputDir); // remove from output dirs
                    outputDirections.Add(outputDir); // add the removed to the end of list - why? because: that way it will rotate and not send everything though only one way untill full
                    itemHandler.receiveItem(item);
                    foreach (var brick in connectedOutputPaths())
                    {
                        if (brick != itemHandler)
                        {
                            brick.receiveItem(null);
                        }
                    }
                    break;
                } else if (outputDirections.Contains(outputDir)) {
                    //Debug.Log("2");
                    outputDirections.Remove(outputDir); // remove from output dirs
                    outputDirections.Add(outputDir); // add the removed to the end of list - why? because: that way it will rotate and not send everything though only one way untill full
                    itemHandler.receiveItem(item);
                    foreach (var brick in connectedOutputPaths())
                    {
                        if (brick != itemHandler)
                        {
                            brick.receiveItem(null);
                        }
                    }
                    break;
                } else {
                    itemHandler.receiveItem(null);
                }
            }
        } else {
            Debug.Log("!!!ERROR!!! - FIX ME");
        }
    }

    public bool ifStorageFull() {
        if (belt != null)
        {
            return belt.ifStorageFull();
        }
        if (outputDirections != null)
        {
            foreach (string outputDir in outputDirections)
            {
                var itemHandler = GlobalMethods.GetBrickByDirCord(outputDir, cordinates);
                if (itemHandler == null || itemHandler.ifStorageFull()) // if path is full or if there is no path at all
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

