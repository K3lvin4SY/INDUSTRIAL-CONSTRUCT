using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class GlobalMethods : MonoBehaviour
{

    private static Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    
    static string[] directions = new string[4]{"N", "E", "S", "W"};
    public static string AddTagToBlockName(string blockName, string tag)
    {
        if (doesBlockNameHaveTag(blockName))
        {
            blockName = RemoveTagFromBlockName(blockName);
        }
        if (getBrickType(blockName) == "block") // chooses which type of block selector to use
        {
            /* Replacing the word "block" with "SelectorBox_block" in the tile name. */
            return blockName.Replace("block", "") + tag + "_block";
        } else {
            return blockName.Replace("slab", "") + tag + "_slab";
        }
    }

    public static string RemoveTagFromBlockName(string blockName) {
        List<string> name = blockName.Split('_').ToList();
        name.RemoveAt(name.Count - 2);
        Debug.Log(string.Join("_", name));
        return string.Join("_", name);
    }

    private static bool doesBlockNameHaveTag(string blockName) {
        if (blockName.ToLower().Contains("selected") || blockName.ToLower().Contains("selectorbox"))
        {
            return true;
        }
        return false;
    }

    public static bool isPlayerEditable(string blockName)
    {
        string[] playerEditable = new string[] {"conveyor", "splitter", "merger", "machine"};
        foreach (string editableBlock in playerEditable) {
            if (blockName.ToLower().Contains(editableBlock))
            {
                return true;
            }
        }
        return false;
    }


    public static Tile GetTileByName(string key) {
        key = key.ToLower();
        if (!tiles.ContainsKey(key))
        {
            string[] assetFiles = Directory.GetFiles("Assets/Tiles/Assets/"); // Gets string array of the tile assets file path
            assetFiles = assetFiles.Select(s => s.ToLowerInvariant()).ToArray(); // to lowercase
            bool[] assetFilesCheck = assetFiles.Select(s => s.Contains("selector")).ToArray();
            
            if (assetFilesCheck.Contains(true))
            {
                string asset = "Assets/Tiles/Assets/"+key+".asset";

                Tile assetTile = (Tile)AssetDatabase.LoadAssetAtPath(asset, typeof(Tile)); // loads the tile asset from path
                if (assetTile == null)
                {
                    return null;
                }
                string assetTileName = assetTile.name.ToLower(); // gets the name of the tile
                tiles[assetTileName] = assetTile; // inserts the data into a dictionary
                Debug.Log(assetTile.name);
            } else {
                Debug.Log("null tile");
                return null;
            }
        }
        return tiles[key];
    }

    public static string getBrickType(string mode = "default") {
        if (mode == "default")
        {
            if (General.tile.name.ToLower().Contains("block"))
            {
                return "block";
            } else {
                return "slab";
            }
        } else {
            if (GetTileByName(mode).name.ToLower().Contains("block"))
            {
                return "block";
            } else {
                return "slab";
            }
        }
        
    }


    public static string oppositeDir(string dir)
    {
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 2;
        if (dirIndex == 4)
        {
            dirIndex = 0;
        }
        if (dir == "U")
        {
            return "D";
        } else if (dir == "D")
        {
            return "U";
        }
        return directions[dirIndex];
    }

    public static string nextDir(string dir) {
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 1;
        if (dirIndex == 4)
        {
            dirIndex = 0;
        }
        return directions[dirIndex];
    }

    public static string prevDir(string dir) {
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex -= 1;
        if (dirIndex == -1)
        {
            dirIndex = 3;
        }
        return directions[dirIndex];
    }

    public static Vector3Int makeV3Int(int x, int y, int z)
    {
        Vector3Int newV3I = new Vector3Int(x, y, z);
        return newV3I;
    }

    public static Vector3 makeV3(float x, float y, float z)
    {
        Vector3 newV3 = new Vector3(x, y, z);
        return newV3;
    }

    public static Vector3Int GetDirV3(string dir, Vector3Int coords, int distance = 1) {
        Dictionary<string, Vector3Int> dirConvertDic = new Dictionary<string, Vector3Int>();
        dirConvertDic.Add("N", makeV3Int(0, 1*distance, 0));
        dirConvertDic.Add("E", makeV3Int(1*distance, 0, 0));
        dirConvertDic.Add("S", makeV3Int(0, -1*distance, 0));
        dirConvertDic.Add("W", makeV3Int(-1*distance, 0, 0));
        dirConvertDic.Add("U", makeV3Int(0, 0, 1*distance)); // Up
        dirConvertDic.Add("D", makeV3Int(0, 0, -1*distance)); // Down
        Vector3Int combineCoords = coords + dirConvertDic[dir[0].ToString()];
        foreach (var dirChar in dir)
        {
            if (dirChar == dir[0])
            {
                continue;
            }
            combineCoords += dirConvertDic[dirChar.ToString()];
        }
        return combineCoords;
    }

    public static List<string> GetDirections(string tileName) {
        if (tileName.ToLower().Contains("elip"))
        {
            return new List<string>() { "U", "D" };
        } else if (tileName.ToLower().Contains("eli"))
        {
            return new List<string>() { tileName[0].ToString(), "U", "D" };
        } else if (tileName.ToLower().Contains("straight"))
        {
            return new List<string>() { tileName[0].ToString(), oppositeDir(tileName[0].ToString()) };
        } else if (tileName.ToLower().Contains("bend"))
        {
            return new List<string>() { tileName[0].ToString(), nextDir(tileName[0].ToString()) };
        } else if (tileName.ToLower().Contains("merger") || tileName.ToLower().Contains("splitter"))
        {
            return new List<string>() { "W", "E", "N", "S" };
        }
        return null;
    }

    public static bool IsConnectionPossible(string tileName, Vector3Int loc) {
        List<string> dirs = GetDirections(tileName);
        foreach (var (coord, brick) in General.bricks)
        {
            foreach (var dir in dirs)
            {
                if (GetDirV3(dir, loc) == coord)
                {
                    if (brick.directions.Contains(oppositeDir(dir)))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Make a method for getting input and out put dirs. Copy the code from the above method and change it to get input and output dirs.
    public static List<string> GetInputDirections(string tileName, Vector3Int loc) {
        List<string> dirs = GetDirections(tileName);
        foreach (var (coord, brick) in General.bricks)
        {
            foreach (var dir in dirs)
            {
                if (GetDirV3(dir, loc) == coord)
                {
                    if (brick.directions.Contains(oppositeDir(dir)))
                    {
                        if (brick.tile.name.ToLower().Contains("conveyor"))
                        {
                            
                        }
                        return brick.belt;
                    }
                }
            }
        }
        return null;
    }


    public static Belt GetBelt(string tileName, Vector3Int loc) {
        List<string> dirs = GetDirections(tileName);
        foreach (var (coord, brick) in General.bricks)
        {
            foreach (var dir in dirs)
            {
                if (GetDirV3(dir, loc) == coord)
                {
                    if (brick.directions.Contains(oppositeDir(dir)))
                    {
                        return brick.belt;
                    }
                }
            }
        }
        return null;
    }

    /*public static bool IsConnectionPossible(Bricks brick, Vector3Int placementCordinates, List<string> placementDirections)
    {
        foreach (var dir in brick.directions)
        {
            if (placementDirections.Contains(dir))
            {
                if (placementCordinates == GetDirV3(dir.ToString(), brick.cordinates))
                {
                    return true;
                }
            }
        }
        
        return false;
    }*/



}




