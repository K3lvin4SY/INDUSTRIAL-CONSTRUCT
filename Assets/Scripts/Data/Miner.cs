using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Miner : Bricks
{
    private List<string> outStorage = new List<string>(); // only for miners, converters and other machines

    public bool powerOn = false;
    public Miner(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null) : base(cTile, coords, dir, inputDir, outputDir) {
        General.bricks[coords] = this;
        General.tickers[coords] = this;
    }

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

    private protected override void moveToNext(string item) {
        var itemHandler = GlobalMethods.GetBrickByDirCord(outputDirections[0], cordinates);
        if (!(itemHandler == null || itemHandler.ifStorageFull(item))) // if path is full or if there is no path at all
        {
            itemHandler.receiveItem(item);
        }
    }
    
}
