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
        if (!doesBlockNameHaveTag(blockName)) {
            return blockName;
        }
        List<string> name = blockName.Split('_').ToList();
        name.RemoveAt(name.Count - 2);
        Debug.Log(string.Join("_", name));
        return string.Join("_", name);

    }

    private static bool doesBlockNameHaveTag(string blockName) {
        if (blockName.ToLower().Contains("selected") || blockName.ToLower().Contains("selectorredbox") || blockName.ToLower().Contains("selectorbox") || blockName.ToLower().Contains("animated"))
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

    public static bool isBrickNotExcludedType(string brickName, string excludeType)
    {
        string[] brickTypes = new string[] {"conveyor", "splitter", "merger", "machine"};
        excludeType = excludeType.ToLower();
        if (brickTypes.Contains(excludeType))
        {
            brickTypes = brickTypes.Where(x => x != excludeType).ToArray();
        }
        foreach (string brickType in brickTypes) {
            if (brickName.ToLower().Contains(brickType))
            {
                if (!(brickName.ToLower().Contains(excludeType)))
                {
                    return true;
                }
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
        if (dirIndex == 5)
        {
            dirIndex = 1;
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
        } else if (tileName.ToLower().Contains("slant"))
        {
            return new List<string>() { tileName[0].ToString(), oppositeDir(tileName[0].ToString())+"U" };
        } else if (tileName.ToLower().Contains("bend"))
        {
            return new List<string>() { tileName[0].ToString(), nextDir(tileName[0].ToString()) };
        } else if (tileName.ToLower().Contains("merger") || tileName.ToLower().Contains("splitter"))
        {
            return new List<string>() { "W", "E", "N", "S" };
        }
        return null;
    }

    public static string NextTileDir(string tileName, string dir) {
        List<string> directions = GetDirections(tileName);
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 1;
        if (dirIndex == directions.Count)
        {
            dirIndex = 0;
        }
        return directions[dirIndex];
    }

    public static string NextBrickDir(Bricks brick, string dir) {
        List<string> directions = brick.directions;
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 1;
        if (dirIndex == directions.Count)
        {
            dirIndex = 0;
        }
        return directions[dirIndex];
    }

    public static bool IsConnectionPossible(string tileName, Vector3Int loc) {
        List<string> dirs = GetDirections(tileName);
        foreach (var dir in dirs)
        {
            
            if (General.bricks.ContainsKey(GetDirV3(dir, loc)))
            {
                Bricks brick = General.bricks[GetDirV3(dir, loc)];
                if (brick.directions.Contains(oppositeDir(dir)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool arePathsColliding(string tileName, Vector3Int loc) {
        if (isBrickNotExcludedType(tileName, "conveyor"))
        {
            List<string> iDirs = GetInputDirections(tileName, loc);
            List<string> oDirs = GetOutputDirections(tileName, loc);
            foreach (var dir in iDirs)
            {
                if (General.bricks.ContainsKey(GetDirV3(dir, loc)))
                {
                    Bricks brick = General.bricks[GetDirV3(dir, loc)];
                    if (brick.inputDirections != null) {
                        if (brick.inputDirections.Contains(oppositeDir(dir)))
                        {
                            return true;
                        }
                    }
                
                }
            }
            foreach (var dir in oDirs)
            {
                if (General.bricks.ContainsKey(GetDirV3(dir, loc)))
                {
                    Bricks brick = General.bricks[GetDirV3(dir, loc)];
                    if (brick.outputDirections != null) {
                        if (brick.outputDirections.Contains(oppositeDir(dir)))
                        {
                            return true;
                        }
                    }
                
                }
            }
        }
        return false;
    }

    public static bool willCollitionOccur(string tileName, Vector3Int loc) {
        if (tileName.ToLower().Contains("conveyor"))
        {
            List<string> dirs = GetDirections(tileName);
            Debug.Log(dirs[0] + dirs[1]);
            if (dirs.Count == 2)
            {
                Vector3Int loc1 = GetDirV3(dirs[0], loc);
                Vector3Int loc2 = GetDirV3(dirs[1], loc);
                Debug.Log(loc1);
                Debug.Log(loc2);
                if (General.bricks.ContainsKey(loc1) && General.bricks.ContainsKey(loc2))
                {
                    Bricks brick1 = General.bricks[loc1];
                    Bricks brick2 = General.bricks[loc2];
                    Debug.Log(brick1.tile.name);
                    Debug.Log(brick2.tile.name);
                    if (brick1.inputDirections != null && brick2.outputDirections != null)
                    {
                        Debug.Log("pas1");
                        if ((brick1.inputDirections.Contains(oppositeDir(dirs[0][0].ToString())) && brick2.inputDirections.Contains(oppositeDir(dirs[1][0].ToString()))) || (brick1.outputDirections.Contains(oppositeDir(dirs[0][0].ToString())) && brick2.outputDirections.Contains(oppositeDir(dirs[1][0].ToString()))))
                        {
                            return true;
                        }
                    }
                    
                }
            } else {
                Debug.Log("Error: " + tileName + " has more than 2 directions");
            }
        }
        
        return false;
    }

    public static bool IsBrickToBrickConnectionPossible(Bricks brick1, Bricks brick2) {
        List<string> dirs = brick1.directions;
        foreach (var dir in dirs)
        {
            
            if (brick2.cordinates == GetDirV3(dir, brick1.cordinates))
            {
                if (brick2.directions.Contains(oppositeDir(dir)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static string BrickToBrickConnectionDirection(Bricks brick1, Bricks brick2) {
        List<string> dirs = brick1.directions;
        foreach (var dir in dirs)
        {
            
            if (brick2.cordinates == GetDirV3(dir, brick1.cordinates))
            {
                if (brick2.directions.Contains(oppositeDir(dir)))
                {
                    return dir;
                }
            }
        }
        return null;
    }

    // Make a method for getting input and out put dirs. Copy the code from the above method and change it to get input and output dirs.
    public static List<string> GetInputDirections(string tileName, Vector3Int loc) {
        List<string> dirs = GetDirections(tileName);
        if (tileName.ToLower().Contains("merger")) {
            return GetDirections(tileName).Where(c => c != tileName[0].ToString()).ToList();
        } else if (tileName.ToLower().Contains("splitter")) {
            return new List<string>() { tileName[0].ToString() };
        }
        foreach (var dir in dirs) // for each direction
        {
            if (General.bricks.ContainsKey(GetDirV3(dir, loc)))
            {
                Bricks brick = General.bricks[GetDirV3(dir, loc)];
                
                if (brick.directions.Contains(oppositeDir(dir)))
                {
                    Debug.Log(brick.directions[0]);
                    if (brick.outputDirections == null || brick.inputDirections == null)
                    {
                        return null;
                    }
                    
                    Debug.Log(brick.outputDirections[0]);
                    List<string> returnValue = null;
                    if (brick.outputDirections.Contains(oppositeDir(dir)))
                    {
                        Debug.Log(dir);
                        if (brick.tile == null) {
                            return new List<string>() { dir };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { dir };
                        } else {
                            return new List<string>() { dir };
                        }
                        
                    } else {
                        Debug.Log(NextTileDir(tileName, dir));
                        Debug.Log(brick.tile);
                        if (brick.tile == null) {
                            return new List<string>() { NextTileDir(tileName, dir) };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { NextTileDir(tileName, dir) };
                        } else {
                            return new List<string>() { NextTileDir(tileName, dir) };
                        }
                        
                    }

                    if (returnValue != null) {
                        return returnValue;
                    }

                }
            }
        }
        return null;
    }

    public static List<string> GetOutputDirections(string tileName, Vector3Int loc) {
        List<string> dirs = GetDirections(tileName);
        if (tileName.ToLower().Contains("splitter")) {
            return GetDirections(tileName).Where(c => c != tileName[0].ToString()).ToList();
        } else if (tileName.ToLower().Contains("merger")) {
            return new List<string>() { tileName[0].ToString() };
        }
        foreach (var dir in dirs)
        {
            if (General.bricks.ContainsKey(GetDirV3(dir, loc)))
            {
                Bricks brick = General.bricks[GetDirV3(dir, loc)];
                
                if (brick.directions.Contains(oppositeDir(dir)))
                {
                    if (brick.outputDirections == null || brick.inputDirections == null)
                    {
                        return null;
                    }
                    List<string> returnValue = null;
                    if (brick.inputDirections.Contains(oppositeDir(dir)))
                    {
                        if (brick.tile == null) {
                            return new List<string>() { dir };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { dir };
                        } else {
                            return new List<string>() { dir };
                        }
                    } else {
                        Debug.Log(NextTileDir(tileName, dir));
                        if (brick.tile == null) {
                            return new List<string>() { NextTileDir(tileName, dir) };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { NextTileDir(tileName, dir) };
                        } else {
                            return new List<string>() { NextTileDir(tileName, dir) };
                        }
                    }

                    if (returnValue != null) {
                        return returnValue;
                    }

                }
            }
        }
        return null;
    }


    public static Belt GetBelt(string tileName, Vector3Int loc, bool overRule = false, List<string> dirs = null) {
        //Debug.Log(loc);
        if (!overRule && (tileName.ToLower().Contains("splitter") || tileName.ToLower().Contains("merger"))) {
            return null;
        }
        if (dirs == null) {
            if (!overRule) {
                dirs = GetDirections(tileName);
            } else {
                dirs = new List<string>() { "W", "E", "N", "S" };
            }
        }
        
        foreach (var dir in dirs)
        {
            //Debug.Log(dir);
            if (General.bricks.ContainsKey(GetDirV3(dir, loc)))
            {
                //Debug.Log("Found a belt");
                Bricks brick = General.bricks[GetDirV3(dir, loc)];
                if (brick.directions.Contains(oppositeDir(dir[0].ToString())) && (brick.tile == null || brick.tile.name.ToLower().Contains("conveyor")))
                {
                    //Debug.Log("Found a connection possible belt");
                    return brick.belt;
                }
            }
        }
        //Debug.Log("No belt found");
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




