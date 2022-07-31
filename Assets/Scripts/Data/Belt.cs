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
    public List<GameItem> storage;
    
    public bool selected;

    

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
        }
    }

    public void AddToBelt(Bricks brick)
    {
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
                brick.belt = this;
                CheckForDirUpdate(GlobalMethods.GetDirV3(GlobalMethods.NextBrickDir(brick, GlobalMethods.oppositeDir(dir)), brick.cordinates));
                return;
            }
        }
        Debug.Log("!!!ERROR!!!");
    }

    private void CheckForDirUpdate(Vector3Int cord) { // if placed conveyor that has been aded to the belt is connected to something with an input/output dir. the belt will get updated
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
        } else {
            Debug.Log("!!!Could Not Flip!!!");
        }
        
    }

    private void clearDirection() {
        foreach (var brick in subCordinates)
        {
            brick.inputDirections = null;
            brick.outputDirections = null;
        }
    }

    private bool noDirection() {
        if (subCordinates[0].inputDirections == null && subCordinates[0].outputDirections == null) {
            return true;
        }
        return false;
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
                assignProgressDirection();
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

    private void assignProgressDirection() {
        int index = 0;
        foreach (var brick in subCordinates)
        {
            if (index+1 == subCordinates.Count && subCordinates.Count != 1) {
                // if last brick in belt
                brick.outputDirections = new List<string>() { GlobalMethods.NextBrickDir(brick, brick.inputDirections[0]) };
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
                            brick.inputDirections = new List<string>() { bDir };
                            brick.outputDirections = new List<string>() { GlobalMethods.NextBrickDir(brick, bDir) };
                            continue;
                        }
                    }
                }
                Debug.Log("!!!ERROR!!!");
                continue;
            }
            string dir = GlobalMethods.BrickToBrickConnectionDirection(brick, subCordinates[index + 1]);
            if (index == 0) {
                brick.inputDirections = new List<string>() { GlobalMethods.NextBrickDir(brick, dir) };
            }
            subCordinates[index + 1].inputDirections = new List<string>() { GlobalMethods.oppositeDir(dir) };
            brick.outputDirections = new List<string>() { dir };
            index += 1;
        }
        Debug.Log(subCordinates.Count);
        updateConnectionBelt();
    }

    private void updateConnectionBelt() {
        Vector3Int connectionBrickCordinates = GlobalMethods.GetDirV3(subCordinates.Last().outputDirections[0], subCordinates.Last().cordinates);
        if (General.bricks.ContainsKey(connectionBrickCordinates))
        {
            General.bricks[connectionBrickCordinates].belt.assignDirection(subCordinates.Last());
        }
    }

    public void Select() {
        foreach (var brick in subCordinates)
        {
            brick.changeTileTag("selected");
        }
        selected = true;
    }

    public void Deselect() {
        if (selected)
        {
            foreach (var brick in subCordinates)
            {
                brick.changeTileTag(null);
            }
            selected = false;
        }
        
    }

}
