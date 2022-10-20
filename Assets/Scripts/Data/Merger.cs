/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Merger : Bricks
{
    public Merger(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null) : base(cTile, coords, dir, inputDir, outputDir, cBelt, linkBrick) {

    }


    public override bool mergerAvailable(string item) {
        if (itemsToChoose == null)
        {
            itemsToChoose = connectedPathsItems();
        }
        /*
        if (item == null)
        {
            item = "none";
        }//*/
        //Debug.Log("Item: "+item);
        /*
        foreach (var item2 in General.bricks[GlobalMethods.GetDirV3("N", cordinates)].belt.storage)
        {
            string item3;
            if (item2 == null)
            {
                item3 = "none";
            } else {
                item3 = item2;
            }
            Debug.Log("I: "+item3);
        }//*/
        /*
        foreach (var (dir2, item2) in itemsToChoose)
        {
            Debug.Log("Item2: "+item2);
            if (item2 == item)
            {
                Debug.Log("Dir2: "+dir2);
            }
        }//*
        //itemsToChoose = null;
        //return false;
        timesRun += 1;

        // add so that all directrions get removed (emptybelt) (including null)
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
        }//
    }/

}
*/
