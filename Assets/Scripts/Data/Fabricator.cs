using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fabricator
{
    Dictionary<Vector3Int, FabricatorComponent> components = new Dictionary<Vector3Int, FabricatorComponent>();

    public Dictionary<string, List<string>> crafting = new Dictionary<string, List<string>>()
    {
        {
            "input",
            new List<string>() {null}
        },
        {
            "output",
            new List<string>() {null}
        }
    };
    public Fabricator(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null)
    {
        Debug.Log(" - NEW Fabricator - ");
        string masterDir = cTile.name[0].ToString();
        for (int z = 0; z <= 1; z++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    bool hideCoord = false;
                    if (x == 0 && y == 0 && z == 0)
                    {
                        hideCoord = true;
                    }
                    Tile tile = GlobalMethods.GetTileByNameAndDir("-fabricator_"+x+"x"+y+"x"+z+"_", masterDir);
                    Debug.Log(tile);
                    Vector3Int innerCoord = new Vector3Int(x, y, z*2);

                    List<string> componentInputDir = null;
                    List<string> componentOutputDir = null;
                    List<string> componentDirs = null;
                    foreach (var iDir in inputDir)
                    {
                        Vector3Int coordDiffrence = GlobalMethods.GetDirV3(iDir, Vector3Int.zero) - innerCoord;
                        string dirDiffrence = coordToDir(coordDiffrence);
                        if (dirDiffrence.Length == 1)
                        {
                            componentInputDir = new List<string>() { dirDiffrence };
                        }
                    }
                    foreach (var oDir in outputDir)
                    {
                        Vector3Int coordDiffrence = GlobalMethods.GetDirV3(oDir, Vector3Int.zero) - innerCoord;
                        string dirDiffrence = coordToDir(coordDiffrence);
                        if (dirDiffrence.Length == 1)
                        {
                            componentOutputDir = new List<string>() { dirDiffrence };
                        }
                    }
                    if (componentInputDir != null)
                    {
                        componentDirs = componentInputDir;
                    } else {
                        componentDirs = componentOutputDir;
                    }
                    General.Instance.map.SetTile(GlobalMethods.CombineCoords(innerCoord, coords), tile);
                    FabricatorComponent component = new FabricatorComponent(tile, GlobalMethods.CombineCoords(innerCoord, coords), /*dirs*/componentDirs, /*input*/componentInputDir, /*output*/componentOutputDir, hideCoord: hideCoord, fab: this);
                    components[innerCoord] = component;
                }
            }
        }
    }

    public void select() {
        foreach ((Vector3Int coord, Bricks fc) in components)
        {
            fc.changeTileTag("selected", temp: true);
        }
    }

    public void deSelect() {
        foreach ((Vector3Int coord, Bricks fc) in components)
        {
            fc.resetTileTag();
        }
    }

    private string coordToDir(Vector3Int coord) {
        string dir = "";
        if (coord.x < 0)
        {
            for (int i = 0; i < coord.x*-1; i++)
            {
                dir += "W";
            }
        } else {
            for (int i = 0; i < coord.x; i++)
            {
                dir += "E";
            }
        }

        
        if (coord.y < 0)
        {
            for (int i = 0; i < coord.y*-1; i++)
            {
                dir += "S";
            }
        } else {
            for (int i = 0; i < coord.y; i++)
            {
                dir += "N";
            }
        }
        
        if (coord.z < 0)
        {
            for (int i = 0; i < coord.z*-1; i++)
            {
                dir += "D";
            }
        } else {
            for (int i = 0; i < coord.z; i++)
            {
                dir += "U";
            }
        }

        return dir;
    }
}
