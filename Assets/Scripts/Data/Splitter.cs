using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Splitter : Bricks
{
    public Splitter(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null) : base(cTile, coords, dir, inputDir, outputDir) {
        General.bricks[coords] = this;
        //General.tickers[coords] = this;
    }

    private protected List<Bricks> connectedOutputPaths() {
        List<Bricks> amount = new List<Bricks>();
        foreach (var dir in outputDirections)
        {
            if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, cordinates)) && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions != null && General.bricks[GlobalMethods.getDirV3(dir, cordinates)].directions.Contains(GlobalMethods.oppositeDir(dir))) // if brick exist & it is connected to this brick
            {
                amount.Add(General.bricks[GlobalMethods.getDirV3(dir, cordinates)]);
            }
        }
        return amount;
    }

    private protected override void moveToNext(string item)
    {
        if (outputDirections != null)
        {
            foreach (string outputDir in outputDirections)
            {
                //Debug.Log(outputDir);
                var itemHandler = GlobalMethods.getBrickByDirCord(outputDir, cordinates);
                if (itemHandler == null || itemHandler.ifStorageFull(item)) // if path is full or if there is no path at all
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
}
