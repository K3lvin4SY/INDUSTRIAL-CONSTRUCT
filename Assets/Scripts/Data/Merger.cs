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

    private protected int connectedPaths() {
        int amount = 0;
        foreach (var dir in inputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, cordinates)) && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount += 1;
            }
        }
        return amount;
    }

    private protected Dictionary<string, string> connectedPathsItems() {
        Dictionary<string, string> amount = new Dictionary<string, string>();
        foreach (var dir in inputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, cordinates)) && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount[dir] = General.bricks[GlobalMethods.getDirV3(dir, cordinates)].GetItem(true, 1);
                //Debug.Log("BeltLast: "+General.bricks[GlobalMethods.getDirV3(dir, cordinates)].belt.storage[General.bricks[GlobalMethods.getDirV3(dir, cordinates)].belt.storage.Count-1]);
                //Debug.Log("BeltLast: "+General.bricks[GlobalMethods.getDirV3(dir, cordinates)].belt.subCordinates.Last().GetItem(true, 1));
                //Debug.Log("Brickitem: "+amount[dir]);
            } else {
                amount[dir] = null;
            }
        }
        return amount;
    }

    public bool mergerAvailable(string item) {
        if (itemsToChoose == null)
        {
            // This only runs first in the first wave
            itemsToChoose = connectedPathsItems();
        }
        timesRun += 1; // adds wave to count

        // Checks if the amount of recieved waves equals the amount of total waves it will receive
        if (timesRun == connectedPaths()) // In other words, This will run in the last wave
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
            } else { // None of the paths has an item
                receiveItem(null);
            }
            itemsToChoose = null; // reset
            timesRun = 0; // reset
            return true;
        } else {
            return false;
        }
    }//*/

    private void removeItemFromBelt(string dir) {
        if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, cordinates)) && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
        {
            Belt belt = General.bricks[GlobalMethods.getDirV3(dir, cordinates)].belt;
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

