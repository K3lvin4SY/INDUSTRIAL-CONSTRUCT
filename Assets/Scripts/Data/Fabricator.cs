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

    string masterDir;
    public Tile tile;
    public List<string> directions;
    public List<string> inputDirections;
    public List<string> outputDirections;
    public Vector3Int cordinates;
    public Dictionary<string, List<string>> storage = new Dictionary<string, List<string>>();
    public Fabricator(Tile cTile, Vector3Int coords, List<string> dirs, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null)
    {
        Debug.Log(" - NEW Fabricator - ");
        this.masterDir = cTile.name[0].ToString();
        this.tile = cTile;
        this.directions = dirs;
        this.inputDirections = inputDir;
        this.outputDirections = outputDir;
        this.cordinates = coords;
        General.tickers[cordinates] = this;
        foreach (var dir in dirs)
        {
            storage[dir] = new List<string>();
        }
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
                    string realDir = null;
                    foreach (var iDir in inputDir)
                    {
                        Vector3Int coordDiffrence = GlobalMethods.GetDirV3(iDir, Vector3Int.zero) - innerCoord;
                        string dirDiffrence = coordToDir(coordDiffrence);
                        if (dirDiffrence.Length == 1)
                        {
                            componentInputDir = new List<string>() { dirDiffrence };
                            realDir = iDir;
                        }
                    }
                    foreach (var oDir in outputDir)
                    {
                        Vector3Int coordDiffrence = GlobalMethods.GetDirV3(oDir, Vector3Int.zero) - innerCoord;
                        string dirDiffrence = coordToDir(coordDiffrence);
                        if (dirDiffrence.Length == 1)
                        {
                            componentOutputDir = new List<string>() { dirDiffrence };
                            realDir = oDir;
                        }
                    }
                    if (componentInputDir != null)
                    {
                        componentDirs = componentInputDir;
                    } else {
                        componentDirs = componentOutputDir;
                    }
                    General.Instance.map.SetTile(GlobalMethods.CombineCoords(innerCoord, coords), tile);
                    FabricatorComponent component = new FabricatorComponent(tile, GlobalMethods.CombineCoords(innerCoord, coords), /*dirs*/componentDirs, /*input*/componentInputDir, /*output*/componentOutputDir, hideCoord: hideCoord, fab: this, realDir: realDir);
                    components[innerCoord] = component;
                }
            }
        }
    }

    public bool ifStorageFull(string item, string fromDir) {
        if (item == null)
        {
            return false;
        }
        if (crafting["input"].Contains(item))
        {
            if (storage[fromDir].Count >= 100)
            {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    public void processConvertion(string item, string fromDir) {
        if (item != null)
        {
            collectItem(item, fromDir);
        }
    }

    private void collectItem(string item, string FromDir) {
        storage[FromDir].Add(item);
    }

    private void convertItem() {
        // check
        List<string> itemsAvailable = new List<string>();
        foreach (var iDir in inputDirections)
        {
            if (storage[iDir].Count >= 1)
            {
                itemsAvailable.Add(storage[iDir][0]);
            }
        }
        foreach (var item in itemsAvailable)
        {
            if (!crafting["input"].Contains(item))
            {
                return; // does not convert
            }
        }
        itemsAvailable = crafting["input"]; // updating the list to be exactlyy to what is needed.
        // Begin convert

        // Remove ingredients
        foreach (var iDir in inputDirections)
        {
            if (storage[iDir].Count >= 1)
            {
                string itemToBeRemoved = storage[iDir][storage[iDir].Count-1];
                foreach (var item in itemsAvailable)
                {
                    if (item == itemToBeRemoved)
                    {
                        storage[iDir].RemoveAt(storage[iDir].Count-1);
                        itemsAvailable.Remove(item);
                        break;
                    }
                }
            }
        }

        // Generate Produced item
        foreach (var item in crafting["output"])
        {
            storage[outputDirections[0]].Add(item);
        }
    }


    private void moveToNext() {
        // get item to move
        string item;
        if (storage[outputDirections[0]].Count >= 1)
        {
            item = storage[outputDirections[0]][storage[outputDirections[0]].Count-1];
        } else {
            item = null;
        }

        // begin move
        var itemHandler = GlobalMethods.GetBrickByDirCord(outputDirections[0], cordinates);
        if (itemHandler != null)
        {
            if (!itemHandler.ifStorageFull(item))
            {
                storage[outputDirections[0]].RemoveAt(storage[outputDirections[0]].Count-1);
                itemHandler.receiveItem(item);
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
