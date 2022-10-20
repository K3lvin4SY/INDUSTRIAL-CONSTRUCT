using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt// : ScriptableObject
{
    public List<Bricks> subCordinates;
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
    public Belt(List<Bricks> path) {
        subCordinates = new List<Bricks>();
        foreach (var brick in path)
        {
            subCordinates.Add(brick);
            storage.Add(null);
        }
    }

    public void AddToBelt(Bricks brick)
    {
        if (subCordinates[0] == subCordinates.Last())
        {
            if (subCordinates[0].inputDirections != null || subCordinates[0].outputDirections != null)
            {
                foreach (var dir in subCordinates[0].directions)
                {
                    if (GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
                    {
                        if (subCordinates[0].inputDirections.Contains(dir))
                        {
                            // in begining of belt
                            subCordinates.Insert(0, brick);
                            storage.Insert(0, null);
                            brick.belt = this;
                            CheckForDirUpdate(GlobalMethods.GetDirV3(GlobalMethods.NextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                            return;
                        } else if (subCordinates[0].outputDirections.Contains(dir))
                        {
                            // in end of belt
                            subCordinates.Add(brick);
                            storage.Add(null);
                            brick.belt = this;
                            CheckForDirUpdate(GlobalMethods.GetDirV3(GlobalMethods.NextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
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
            Debug.Log(GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                subCordinates.Insert(0, brick);
                storage.Insert(0, null);
                brick.belt = this;
                CheckForDirUpdate(GlobalMethods.GetDirV3(GlobalMethods.NextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                return;
            }
        }

        foreach (var dir in subCordinates.Last().directions)
        {
            /*
            Debug.Log(dir);
            Debug.Log(GlobalMethods.GetDirV3(dir, subCordinates.Last().cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.GetDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                subCordinates.Add(brick);
                storage.Add(null);
                brick.belt = this;
                CheckForDirUpdate(GlobalMethods.GetDirV3(GlobalMethods.NextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                return;
            }
        }
        Debug.Log("!!!ERROR!!!");
    }

    public void CheckForDirUpdate(Vector3Int cord) { // if placed conveyor that has been aded to the belt is connected to something with an input/output dir. the belt will get updated
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

    public bool AddToBeltCheck(Bricks brick)
    {
        foreach (var dir in subCordinates[0].directions)
        {
            /*Debug.Log(dir);
            Debug.Log(GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                return true;
            }
        }

        foreach (var dir in subCordinates.Last().directions)
        {
            /*Debug.Log(dir);
            Debug.Log(GlobalMethods.GetDirV3(dir, subCordinates.Last().cordinates));
            Debug.Log(brick.cordinates);//*/
            if (GlobalMethods.GetDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                return true;
            }
        }
        return false;
    }

    public bool BegningOfBelt(Bricks brick)
    {
        foreach (var dir in subCordinates[0].directions)
        {
            if (GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                return true;
            }
        }

        foreach (var dir in subCordinates.Last().directions)
        {
            if (GlobalMethods.GetDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                return false;
            }
        }
        return false;
    }

    public void updateDirection() {
        if (!noDirection())
        {
            clearDirection();
        }
    }

    private void Flip() {
        if (noDirection())
        {
            subCordinates.Reverse();
            storage.Reverse();
        } else {
            Debug.Log("!!!Could Not Flip!!!");
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

    public void fixFaltyDirection() { // not completed
        int loopNum = -1;
        Bricks brickToBeFixed = null;
        Bricks brickToUseFixing = null;
        foreach (var brick in subCordinates)
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
        
        if (GlobalMethods.GetDirV3(brickToUseFixing.outputDirections[0], brickToUseFixing.cordinates) == brickToBeFixed.cordinates)
        {
            
        }
        foreach (var fDir in brickToBeFixed.directions)
        {
            if (brickToUseFixing.inputDirections.Contains(GlobalMethods.oppositeDir(fDir)))
            {
                brickToBeFixed.changeOutputDir(new List<string>() { fDir });
                brickToBeFixed.changeInputDir(new List<string>() { GlobalMethods.NextBrickDir(brickToBeFixed, fDir) });
            } else if (brickToUseFixing.outputDirections.Contains(GlobalMethods.oppositeDir(fDir))) {
                brickToBeFixed.changeInputDir(new List<string>() { fDir });
                brickToBeFixed.changeOutputDir(new List<string>() { GlobalMethods.NextBrickDir(brickToBeFixed, fDir) });
            }
        }
    }

    public void assignDirection(Bricks brick) {
        if (noDirection())
        {
            if (AddToBeltCheck(brick))
            {
                if (BegningOfBelt(brick))
                {
                    // beginning of belt
                    if (brick.outputDirections.Contains(GlobalMethods.oppositeDir(getEdgeDir(false))))
                    {
                        // begining is input (don't flip)
                    } else { // if something goes wrong, try using else if (brick.inputDirections.Contains(GlobalMethods.oppositeDir(getEdgeDir(false)))) - same with the one bellow, also try adding else after to see if it ever comes down there which it shouldn't
                        // begining is output (flip)
                        Flip();
                    }
                } else {
                    // end of belt
                    if (brick.outputDirections.Contains(GlobalMethods.oppositeDir(getEdgeDir(true))))
                    {
                        // end is input (flip)
                        Flip();
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
            Bricks brick = subCordinates.Last();
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.GetDirV3(dir, brick.cordinates) == subCordinates[subCordinates.Count-2].cordinates)) // if the brick is connected to the next brick in belt
                {
                    return dir;
                }
            }
        } else {
            if (subCordinates.Count == 1)
            {
                return subCordinates[0].directions[0];
            }
            Bricks brick = subCordinates[0];
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.GetDirV3(dir, brick.cordinates) == subCordinates[1].cordinates)) // if the brick is connected to the next brick in belt
                {
                    return dir;
                }
            }
        }
        Debug.Log("!!!ERROR!!!");
        return null;
    }

    public string isBrick(Bricks brick) {
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

    public bool isBrickLast(Bricks brick) {
        if (isBrick(brick) == "last")
        {
            return true;
        }
        return false;
    }

    public Bricks getConnectingEdgeBrick(bool end, bool next = false, bool manual = false) {
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
                if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates)))
                {
                    return General.bricks[GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates)];
                }
            } else if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(subCordinates[0].directions[0], subCordinates[0].cordinates)))
            {
                return General.bricks[GlobalMethods.GetDirV3(subCordinates[0].directions[0], subCordinates[0].cordinates)];
            }
            //Debug.Log("No Brick by edge");
            return null;
        }
        if (end)
        {
            
            Bricks brick = subCordinates.Last();
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.GetDirV3(dir, brick.cordinates) == subCordinates[subCordinates.Count-2].cordinates)) // if the brick is connected to the next brick in belt
                {
                    if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, brick.cordinates)))
                    {
                        return General.bricks[GlobalMethods.GetDirV3(dir, brick.cordinates)];
                    }
                }
            }
        } else {
            
            Bricks brick = subCordinates[0];
            foreach (var dir in brick.directions)
            {
                if (!(GlobalMethods.GetDirV3(dir, brick.cordinates) == subCordinates[1].cordinates)) // if the brick is connected to the next brick in belt
                {
                    if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, brick.cordinates)))
                    {
                        return General.bricks[GlobalMethods.GetDirV3(dir, brick.cordinates)];
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
                brick.changeOutputDir(new List<string>() { GlobalMethods.NextBrickDir(brick, brick.inputDirections[0]) });
                continue;
            }
            if (subCordinates.Count == 1)
            {
                foreach (var bDir in brick.directions)
                {
                    Vector3Int cord = GlobalMethods.GetDirV3(bDir, brick.cordinates);
                    if (General.bricks.ContainsKey(cord))
                    {
                        if (General.bricks[cord].outputDirections.Contains(GlobalMethods.oppositeDir(bDir)))
                        {
                            brick.changeInputDir(new List<string>() { bDir });
                            brick.changeOutputDir(new List<string>() { GlobalMethods.NextBrickDir(brick, bDir) });
                            continue;
                        } else if (General.bricks[cord].inputDirections.Contains(GlobalMethods.oppositeDir(bDir)))
                        {
                            brick.changeOutputDir(new List<string>() { bDir });
                            brick.changeInputDir(new List<string>() { GlobalMethods.NextBrickDir(brick, bDir) });
                            continue;
                        }
                    }
                }
                Debug.Log("!!!ERROR!!!");
                continue;
            }
            string dir = GlobalMethods.BrickToBrickConnectionDirection(brick, subCordinates[index + 1]);
            if (index == 0) {
                brick.changeInputDir(new List<string>() { GlobalMethods.NextBrickDir(brick, dir) });
            }
            subCordinates[index + 1].changeInputDir(new List<string>() { GlobalMethods.oppositeDir(dir) });
            brick.changeOutputDir(new List<string>() { dir });
            index += 1;
        }
        Debug.Log(subCordinates.Count);
        updateConnectionBelt(inBrick);
    }

    private void updateConnectionBelt(Bricks brick) {
        Vector3Int connectionBrickCordinates = GlobalMethods.GetDirV3(subCordinates.Last().outputDirections[0], subCordinates.Last().cordinates);
        Bricks beltBrick = subCordinates.Last();
        if (connectionBrickCordinates == brick.cordinates)
        {
            connectionBrickCordinates = GlobalMethods.GetDirV3(subCordinates[0].inputDirections[0], subCordinates[0].cordinates);
            beltBrick = subCordinates[0];
        }
        if (General.bricks.ContainsKey(connectionBrickCordinates))
        {
            Debug.Log(General.bricks[connectionBrickCordinates].belt.subCordinates.Count);
            //Debug.Log(General.bricks[connectionBrickCordinates].belt.subCordinates.Last().tile.name);
            General.bricks[connectionBrickCordinates].belt.assignDirection(beltBrick);
        }
    }

    public void Select() {
        foreach (var brick in subCordinates)
        {
            brick.changeTileTag("selected", temp: true);
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
            if (moveToNextCheck()) // if path continues
            {
                if (!getNextItemHandler().ifStorageFull(storage[storage.Count-1])) // next storage is not full
                {
                    moveToNext(storage[storage.Count-1]);
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
            Debug.Log(storage.Count);
            Debug.Log(subCordinates.Count);
            for (int i = 0; i < subCordinates.Count/*-1*/; i++)
            {
                /*
                foreach (var item in storage)
                {
                    Debug.Log(item);
                }//*/
                if (storage[i] != null)
                {
                    subCordinates[i].changeTileTag("selected", true);
                } else {
                    subCordinates[i].resetTileTag();
                }
            }
        //}
    }

    public bool ifStorageFull(string item) {
        if (storage.Where(c => c != null).ToList().Count >= subCordinates.Count) // if storage full
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
        Bricks brick = getConnectingEdgeBrick(true, true, true); // if some error may look inte the second true - note
        if (brick != null)
        {
            //removeEmptyStorageSpace();
            
            Debug.Log("Name: "+brick.tile.name);
            if (brick.belt != null)
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
            if (brick.belt != null)
            {
                return brick.belt;
            }
        }
        return brick;
    }//*/

    private Bricks getNextItemHandler() { // send the item to the connected brick
        return getConnectingEdgeBrick(true, true, true); // if some error may look inte the second true - note
    }

    private bool moveToNextCheck() { // check if there is a brick to move item to
        Bricks brick = getConnectingEdgeBrick(true, true, true); // if some error may look inte the second true - note
        if (brick != null)
        {
            return true;
        }
        return false;
    }

}
