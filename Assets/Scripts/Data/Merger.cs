using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Merger : Bricks
{
    private int timesRun = 0; // only for item system
    private Dictionary<string, string> itemsToChoose = null; // only for item system
    public Merger(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null) : base(cTile, coords, dir, inputDir, outputDir) {
        General.bricks[coords] = this;
    }

    public bool mergerAvailable(string item) {
        if (itemsToChoose == null)
        {
            itemsToChoose = connectedPathsItems();
        }
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
    }//*/

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

}

