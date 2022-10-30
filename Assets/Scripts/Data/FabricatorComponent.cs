using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FabricatorComponent : Bricks
{
    public FabricatorComponent(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null, bool hideCoord = false, Fabricator fab = null) : base(cTile, coords, dir, inputDir, outputDir, cBelt, linkBrick, fabricator:true) {
        Fabricator fabricator = fab;
        if (!hideCoord)
        {
            General.bricks[coords] = this;
        }
    }
}
