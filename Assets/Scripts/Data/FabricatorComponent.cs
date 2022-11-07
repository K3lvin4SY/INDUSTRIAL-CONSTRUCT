using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FabricatorComponent : Bricks
{
    public Fabricator fabricator;
    private string realDir;
    public FabricatorComponent(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null, bool hideCoord = false, Fabricator fab = null, string realDir = null) : base(cTile, coords, dir, inputDir, outputDir) {
        this.fabricator = fab;
        this.realDir = realDir;
        if (!hideCoord)
        {
            General.bricks[coords] = this;
        }
    }

    public void reciveItem(string item) {
        fabricator.processConvertion(item, realDir);
    }

    public override bool ifStorageFull(string item) {
        return fabricator.ifStorageFull(item, realDir);
    }
}
