using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt// : ScriptableObject
{
    public List<Conveyor> subCordinates;
    //public Dictionary<int, Bricks> subCordinates;
    public List<string> storage = new List<string>();
    
    public bool selected = false;
    static public int calc = 0;

    

    /// <summary>
    /// It takes a dictionary of Vector3Ints and Tiles, and returns a dictionary of Vector3Ints and
    /// Tiles
    /// It Creates the belt from a given dictionary of Vector3Ints and Tiles
    /// </summary>
    /// <param name="path">The path that the player is currently on.</param>
    public Belt(List<Conveyor> path) {
        subCordinates = new List<Conveyor>();
        foreach (var brick in path)
        {
            subCordinates.Add(brick);
            storage.Add(null);
        }
    }

    public void addToBelt(Conveyor brick)
    {
        if (subCordinates[0] == subCordinates.Last())
        {
            if (subCordinates[0].inputDirections != null || subCordinates[0].outputDirections != null)
            {
                foreach (var dir in subCordinates[0].directions)
                {
                    if (GlobalMethods.getDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
                    {
                        if (subCordinates[0].inputDirections.Contains(dir))
                        {
                            // in begining of belt
                            subCordinates.Insert(0, brick);
                            storage.Insert(0, null);
                            brick.belt = this;
                            checkForDirUpdate(GlobalMethods.getDirV3(GlobalMethods.nextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                            return;
                        } else if (subCordinates[0].outputDirections.Contains(dir))
                        {
                            // in end of belt
                            subCordinates.Add(brick);
                            storage.Add(null);
                            brick.belt = this;
                            checkForDirUpdate(GlobalMethods.getDirV3(GlobalMethods.nextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                            return;
                        } else {
                            Debug.Log("!!!ERROR!!!");
                        }
                    }
                }
            }
        }
        foreach (var dir in subCordinates[0].directions)
        {
            /*
            Debug.Log(dir);
            Debug.Log(GlobalMethods.getDirV3(dir, subCordinates[0].cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.getDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                subCordinates.Insert(0, brick);
                storage.Insert(0, null);
                brick.belt = this;
                checkForDirUpdate(GlobalMethods.getDirV3(GlobalMethods.nextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                return;
            }
        }

        foreach (var dir in subCordinates.Last().directions)
        {
            /*
            Debug.Log(dir);
            Debug.Log(GlobalMethods.getDirV3(dir, subCordinates.Last().cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.getDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                subCordinates.Add(brick);
                storage.Add(null);
                brick.belt = this;
                checkForDirUpdate(GlobalMethods.getDirV3(GlobalMethods.nextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                return;
            }
        }
        Debug.Log("!!!ERROR!!!");
    }

    /// <summary>
    /// If a conveyor is placed next to a brick that has an input or output direction, the conveyor will
    /// get the same direction
    /// </summary>
    /// <param name="cord">The position of the conveyor belt</param>
    public void checkForDirUpdate(Vector3Int cord) { // if placed conveyor that has been aded to the belt is connected to something with an input/output dir. the belt will get updated
        if (General.bricks.ContainsKey(cord))
        {
            Bricks brick = General.bricks[cord];
            if (brick.inputDirections != null || brick.outputDirections != null)
            {
                if (noDirection())
                {
                    assignDirection(brick);
                } else {
                    Debug.Log("!!!Conveyor Placed Wrongly!!!"); // collision occurs
                }
            }
        }
    }

    public bool addToBeltCheck(Bricks brick)
    {
        foreach (var dir in subCordinates[0].directions)
        {
            /*Debug.Log(dir);
            Debug.Log(GlobalMethods.getDirV3(dir, subCordinates[0].cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.getDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                return true;
            }
        }

        foreach (var dir in subCordinates.Last().directions)
        {
            /*Debug.Log(dir);
            Debug.Log(GlobalMethods.getDirV3(dir, subCordinates.Last().cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.getDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                return true;
            }
        }
        return false;
    }

    public bool begningOfBelt(Bricks brick)
    {
        foreach (var dir in subCordinates[0].directions)
        {
            if (GlobalMethods.getDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                return true;
            }
        }

        /*foreach (var dir in subCordinates.Last().directions) // Unececery code
        {
            if (GlobalMethods.getDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                return false;
            }
        }*/
        return false;
    }

    public void updateDirection() {
        if (!noDirection())
        {
            clearDirection();
        }
    }

    private void flip() {
        if (noDirection())
        {
            subCordinates.Reverse();
            storage.Reverse();
        } else {
            Debug.Log("!!!Could Not flip!!!");
        }
        
    }

    private void clearDirection() {
        foreach (var brick in subCordinates)
        {
            brick.changeInputDir(null);
            brick.changeOutputDir(null);
        }
    }

    /*
    private bool noDirection() {
        if (subCordinates[0].inputDirections == null && subCordinates[0].outputDirections == null) {
            return true;
        }
        return false;
    }//*/

    public bool noDirection() {
        foreach (var brick in subCordinates)
        {
            if (brick.inputDirections == null || brick.outputDirections == null) {
                return true;
            }
        }
        
        return false;
    }
    public bool faltyDirection() {
        foreach (var brick in subCordinates)
        {
            if (brick.inputDirections == null || brick.outputDirections == null) {
                foreach (var brick2 in subCordinates)
                {
                    if (brick2.inputDirections != null || brick2.outputDirections != null) {
                        return true;
                    }
                }
            }
            
        }
        return false;
    }

    /// <summary>
    /// It's a function that fixes a conveyor belt that has a faulty direction
    /// </summary>
    public void fixFaltyDirection() { // not completed
        int loopNum = -1;
        Conveyor brickToBeFixed = null;
        Conveyor brickToUseFixing = null;
        foreach (Conveyor brick in subCordinates)
        {
            loopNum += 1;
            if (brick.inputDirections == null || brick.outputDirections == null) {
                if (subCordinates.Count == 1)
                {
                    Debug.Log("!!!ERROR!!! FIX ME");
                }
                brickToBeFixed = brick;
                if (loopNum == 0)
                {
                    brickToUseFixing = subCordinates[1];
                } else if (loopNum == subCordinates.Count-1)
                {
                    brickToUseFixing = subCordinates[subCordinates.Count-2];
                } else {
                    brickToUseFixing = subCordinates[loopNum+1];
                }
                break;
            }
            
        }
        
        if (GlobalMethods.getDirV3(brickToUseFixing.outputDirections[0], brickToUseFixing.cordinates) == brickToBeFixed.cordinates)
        {
            
        }
        foreach (var fDir in brickToBeFixed.directions)
        {
            if (brickToUseFixing.inputDirections.Contains(GlobalMethods.oppositeDir(fDir)))
            {
                brickToBeFixed.changeOutputDir(new List<string>() { fDir });
                brickToBeFixed.changeInputDir(new List<string>() { GlobalMethods.nextBrickDir(brickToBeFixed, fDir) });
            } else if (brickToUseFixing.outputDirections.Contains(GlobalMethods.oppositeDir(fDir))) {
                brickToBeFixed.changeInputDir(new List<string>() { fDir });
                brickToBeFixed.changeOutputDir(new List<string>() { GlobalMethods.nextBrickDir(brickToBeFixed, fDir) });
            }
        }
    }

    public void assignDirection(Bricks brick) {
        if (noDirection())
        {
            if (addToBeltCheck(brick))
            {
                if (begningOfBelt(brick))
                {
                    // beginning of belt
                    if (brick.outputDirections.Contains(GlobalMethods.oppositeDir(getEdgeDir(false))))
                    {
                        // begining is input (don't flip)
                    } else { // if something goes wrong, try using else if (brick.inputDirections.Contains(GlobalMethods.oppositeDir(getEdgeDir(false)))) - same with the one bellow, also try adding else after to see if it ever comes down there which it shouldn't
                        // begining is output (flip)
                        flip();
                    }
                } else {
                    // end of belt
                    if (brick.outputDirections.Contains(GlobalMethods.oppositeDir(getEdgeDir(true))))
                    {
                        // end is input (flip)
                        flip();
                    } else {
                        // end is output (don't flip)
                    }
                }
                assignProgressDirection(brick);
            }
        }
        
    }

    private string getEdgeDir(bool end) {
        if (end)
        {
            if (subCordinates.Count == 1)
            {
                return subCordinates[0].directions[0];
            }
            Conveyor brick = subCordinates.Last();
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.getDirV3(dir, brick.cordinates) == subCordinates[subCordinates.Count-2].cordinates)) // if the brick is connected to the next brick in belt
                {
                    return dir;
                }
            }
        } else {
            if (subCordinates.Count == 1)
            {
                return subCordinates[0].directions[0];
            }
            Conveyor brick = subCordinates[0];
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.getDirV3(dir, brick.cordinates) == subCordinates[1].cordinates)) // if the brick is connected to the next brick in belt
                {
                    return dir;
                }
            }
        }
        Debug.Log("!!!ERROR!!!");
        return null;
    }

    public string isBrick(Conveyor brick) {
        if (subCordinates.Last() == brick && subCordinates[0] == brick)
        {
            return "first&last";
        } else if (subCordinates.Last() == brick)
        {
            return "last";
        } else if (subCordinates[0] == brick) {
            return "first";
        }
        return null;
    }

    public bool isBrickLast(Conveyor brick) {
        if (isBrick(brick) == "last")
        {
            return true;
        }
        return false;
    }

    public dynamic getConnectingEdgeBrick(bool end, bool next = false, bool manual = false) {
        Belt.calc +=1;
        //Debug.Log("Times Run: "+Belt.calc);
        if (subCordinates.Count == 1)
        {
            if (manual && subCordinates[0].inputDirections != null)
            {   
                string dir;
                if (!next)
                {
                    dir = subCordinates[0].inputDirections[0];
                } else {
                    dir = subCordinates[0].outputDirections[0];
                }
                if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, subCordinates[0].cordinates)))
                {
                    return General.bricks[GlobalMethods.getDirV3(dir, subCordinates[0].cordinates)];
                }
            } else if (General.bricks.ContainsKey(GlobalMethods.getDirV3(subCordinates[0].directions[0], subCordinates[0].cordinates)))
            {
                return General.bricks[GlobalMethods.getDirV3(subCordinates[0].directions[0], subCordinates[0].cordinates)];
            }
            //Debug.Log("No Brick by edge");
            return null;
        }
        if (end)
        {
            
            Bricks brick = subCordinates.Last();
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.getDirV3(dir, brick.cordinates) == subCordinates[subCordinates.Count-2].cordinates)) // if the brick is connected to the next brick in belt
                {
                    if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, brick.cordinates)))
                    {
                        return General.bricks[GlobalMethods.getDirV3(dir, brick.cordinates)];
                    }
                }
            }
        } else {
            
            Bricks brick = subCordinates[0];
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.getDirV3(dir, brick.cordinates) == subCordinates[1].cordinates)) // if the brick is connected to the next brick in belt
                {
                    if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, brick.cordinates)))
                    {
                        return General.bricks[GlobalMethods.getDirV3(dir, brick.cordinates)];
                    }
                }
            }
        }
        //Debug.Log("NO BRICK FOUND!");
        return null;
    }

    private void assignProgressDirection(Bricks inBrick) {
        int index = 0;
        foreach (var brick in subCordinates)
        {
            if (index+1 == subCordinates.Count && subCordinates.Count != 1) {
                // if last brick in belt
                brick.changeOutputDir(new List<string>() { GlobalMethods.nextBrickDir(brick, brick.inputDirections[0]) });
                continue;
            }
            if (subCordinates.Count == 1)
            {
                foreach (var bDir in brick.directions)
                {
                    Vector3Int cord = GlobalMethods.getDirV3(bDir, brick.cordinates);
                    if (General.bricks.ContainsKey(cord))
                    {
                        if (General.bricks[cord].outputDirections.Contains(GlobalMethods.oppositeDir(bDir)))
                        {
                            brick.changeInputDir(new List<string>() { bDir });
                            brick.changeOutputDir(new List<string>() { GlobalMethods.nextBrickDir(brick, bDir) });
                            continue;
                        } else if (General.bricks[cord].inputDirections.Contains(GlobalMethods.oppositeDir(bDir)))
                        {
                            brick.changeOutputDir(new List<string>() { bDir });
                            brick.changeInputDir(new List<string>() { GlobalMethods.nextBrickDir(brick, bDir) });
                            continue;
                        }
                    }
                }
                Debug.Log("!!!ERROR!!!");
                continue;
            }
            string dir = GlobalMethods.brickToBrickConnectionDirection(brick, subCordinates[index + 1]);
            if (index == 0) {
                brick.changeInputDir(new List<string>() { GlobalMethods.nextBrickDir(brick, dir) });
            }
            subCordinates[index + 1].changeInputDir(new List<string>() { GlobalMethods.oppositeDir(dir) });
            brick.changeOutputDir(new List<string>() { dir });
            index += 1;
        }
        Debug.Log(subCordinates.Count);
        updateConnectionBelt(inBrick);
    }

    // for updating next belt in following path - i think?
    private void updateConnectionBelt(Bricks brick) {
        Vector3Int connectionBrickCordinates = GlobalMethods.getDirV3(subCordinates.Last().outputDirections[0], subCordinates.Last().cordinates);
        Bricks beltBrick = subCordinates.Last();
        if (connectionBrickCordinates == brick.cordinates)
        {
            connectionBrickCordinates = GlobalMethods.getDirV3(subCordinates[0].inputDirections[0], subCordinates[0].cordinates);
            beltBrick = subCordinates[0];
        }
        if (General.bricks.ContainsKey(connectionBrickCordinates))
        {
            Debug.Log(General.bricks[connectionBrickCordinates].belt.subCordinates.Count);
            //Debug.Log(General.bricks[connectionBrickCordinates].belt.subCordinates.Last().tile.name);
            General.bricks[connectionBrickCordinates].belt.assignDirection(beltBrick);
        }
    }

    public void Select(string tag) {
        foreach (var brick in subCordinates)
        {
            brick.changeTileTag(tag, temp: true);
        }
        selected = true;
    }

    public void Deselect() {
        if (selected)
        {
            foreach (var brick in subCordinates)
            {
                brick.resetTileTag();
            }
            selected = false;
        }
        
    }

    public void receiveItem(string item) {
        //Debug.Log("Item received: "+item);
        if (storage.Where(c => c != null).ToList().Count >= subCordinates.Count)
        {
            Debug.Log("ERROR - TO MANY ITEMS"); // remove this one?
        }
        storage.Insert(0, item);
        tickMoveStorage();
    }

    private void tickMoveStorage() {
        //if (storage.Where(c => c != null).ToList().Count > 0) { // if statement for seeing if belt is empty
            bool moveing = false;
            if (moveToNextCheck()) // if path continues
            {
                if (!getNextItemHandler().ifStorageFull(storage[storage.Count-1])) // next storage is not full
                {
                    moveToNext(storage[storage.Count-1]);
                    moveing = true;
                    //storage.RemoveAt(storage.Count-1);
                } else {
                    // next is full and connot pass
                    removeEmptyStorageSpace();
                }
            } else { // if end of path
                //if (ifStorageFull()) { // maybe it should be !ifStorageFull()???
                    removeEmptyStorageSpace();
                //}
            }
            // temp code for seeing items passing through
            //Debug.Log(storage.Count);
            //Debug.Log(subCordinates.Count);
            for (int i = 0; i < subCordinates.Count/*-1*/; i++)
            {
                /*
                foreach (var item in storage)
                {
                    Debug.Log(item);
                }//*/
                if (storage[i] != null)
                {
                    if (subCordinates[i].outputDirections != null)
                    {
                        string dir = subCordinates[i].outputDirections[0][0].ToString();
                        if (dir == "D" || dir == "U")
                        {
                            dir = GlobalMethods.oppositeDir(subCordinates[i].inputDirections[0]);
                        }
                        if (/*false && */subCordinates[i].GetItem(true) == "Iron_Ore") // temp
                        {
                            subCordinates[i].changeTileTag("animated+"+dir+"+"+subCordinates[i].GetItem(true).Replace("_", ""), true);
                        } else {
                            subCordinates[i].changeTileTag("selected", true);
                        }
                        
                    }
                } else {
                    subCordinates[i].resetTileTag();
                }
            }
            
            if (!moveing/* && false*/)
            {
                for (int i = subCordinates.Count-1; i >= 0; i--)
                {
                    if (storage[i] != null)
                    {
                        if (subCordinates[i].outputDirections != null)
                        {
                            string dir = subCordinates[i].outputDirections[0][0].ToString();
                            if (dir == "D" || dir == "U")
                            {
                                dir = GlobalMethods.oppositeDir(subCordinates[i].inputDirections[0]);
                            }
                            
                            if (/*false && */subCordinates[i].GetItem(true) == "Iron_Ore") // temp
                            {
                                subCordinates[i].changeTileTag("still+"+dir+"+"+subCordinates[i].GetItem(true).Replace("_", ""), true);
                            } else {
                                subCordinates[i].changeTileTag("selected", true);
                            }
                        }
                    } else {
                        break;
                    }
                }
            }
        //}
    }

    /// <summary>
    /// If the storage is full, check if there is a connection brick, if there is, check if the next
    /// brick's storage is full, if it is, return true, if it isn't, return false
    /// </summary>
    /// <param name="item">the item to be stored</param>
    /// <returns>
    /// The return value is a boolean.
    /// </returns>
    public bool ifStorageFull(string item) {
        // if (!storage.Contains(null)) // will this be more effective and faster to calculate?
        if (storage.Where(c => c != null).ToList().Count >= subCordinates.Count) // if storage full // 
        {
            if (moveToNextCheck()) { // if there is a connection brick (the path leads forward and doesnt end)
                return getNextItemHandler().ifStorageFull(item);
            } else {
                return true;
            }
        }
        return false;
    }

    public void removeEmptyStorageSpace() {
        if (!(storage.Count > subCordinates.Count))
        {
            return;
        }
        int removable = storage.Count - subCordinates.Count;
        for (int i = storage.Count-1; i >= 0; i--)
        {
            if (storage[i] == null)
            {
                storage.RemoveAt(i);
                removable-=1;
                return;
            }
        }
    }
    private void moveToNext(string item) { // send the item to the connected brick
        dynamic brick = getConnectingEdgeBrick(true, true, true); // if some error may look inte the second true - note
        if (brick != null)
        {
            //removeEmptyStorageSpace();
            
            Debug.Log("Name: "+brick.tile.name);
            if (brick is Conveyor)
            {
                storage.RemoveAt(storage.Count-1);
                brick.belt.receiveItem(item);
                removeEmptyStorageSpace();
                return;
            }
            if (brick.inputDirections != null && brick.inputDirections.Count > 1)
            {
                /*
                storage.RemoveAt(storage.Count-1);
                brick.receiveItem(item);//*/

                Debug.Log("try pass");
                //*
                if (brick.mergerAvailable(item))
                {
                    Debug.Log("passed");
                    //storage.RemoveAt(storage.Count-1);
                    //brick.receiveItem(item);
                } else {
                    Debug.Log("Empty");
                }//*/
            } else {
                storage.RemoveAt(storage.Count-1);
                brick.receiveItem(item);
            }
        }
        removeEmptyStorageSpace();
        Debug.Log(storage.Count - subCordinates.Count);
    }

    /*
    private dynamic getNextItemHandler() { // send the item to the connected brick
        Bricks brick = getConnectingEdgeBrick(true, true); // if some error may look inte the second true - note
        if (brick != null)
        {
            if (brick is Conveyor)
            {
                return brick.belt;
            }
        }
        return brick;
    }//*/

    private dynamic getNextItemHandler() { // send the item to the connected brick
        return getConnectingEdgeBrick(true, true, true); // if some error may look inte the second true - note
    }

    private bool moveToNextCheck() { // check if there is a brick to move item to
        dynamic brick = getConnectingEdgeBrick(true, true, true); // if some error may look inte the second true - note
        if (brick != null)
        {
            return true;
        }
        return false;
    }

}
