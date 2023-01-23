using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Collector : Bricks
{
    
    public Collector(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir) : base(cTile, coords, dir, inputDir, outputDir) {
        General.bricks[coords] = this;
    }

    public override void receiveItem(string item)
    {
        collectItem(item);
    }

    private protected void collectItem(string item) {

    }
}
