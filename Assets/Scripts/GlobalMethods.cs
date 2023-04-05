using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEditor;

public class GlobalMethods : MonoBehaviour
{

    private static Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    public static List<Tile> getTiles() {
        return tiles.Values.ToList<Tile>();
    }
    public static List<string> getTileNames() {
        return tiles.Keys.ToList<string>();
    }
    private static Dictionary<string, Tile> flatTiles = new Dictionary<string, Tile>();
    private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private static Dictionary<string, AnimatedTile> aTiles = new Dictionary<string, AnimatedTile>();
    public static int calc = 0;
    
    static string[] directions = new string[4]{"N", "E", "S", "W"};

    static Dictionary<string, Vector3Int> dirConvertDic = new Dictionary<string, Vector3Int>() {
        {"N", new Vector3Int(0, 1, 0)},
        {"E", new Vector3Int(1, 0, 0)},
        {"S", new Vector3Int(0, -1, 0)},
        {"W", new Vector3Int(-1, 0, 0)},
        {"U", new Vector3Int(0, 0, 1)},
        {"D", new Vector3Int(0, 0, -1)}
    };

    public static void loadAllAssets()
    {
        Tile[] assetTiles = Resources.LoadAll("Tiles/Assets", typeof(Tile)).Cast<Tile>().ToArray();
        foreach (Tile item in assetTiles)
        {
            string assetTileName = item.name.ToLower(); // gets the name of the tile
            tiles[assetTileName] = item; // inserts the data into a dictionary
            
        }
        AnimatedTile[] assetATiles = Resources.LoadAll("Tiles/Animated Assets", typeof(AnimatedTile)).Cast<AnimatedTile>().ToArray();
        foreach (AnimatedTile item in assetATiles)
        {
            string assetTileName = item.name.ToLower(); // gets the name of the tile
            Debug.Log(assetTileName);
            aTiles[assetTileName] = item; // inserts the data into a dictionary
            
        }
        Tile[] assetTiles2 = Resources.LoadAll("Tiles/flat Assets", typeof(Tile)).Cast<Tile>().ToArray();
        foreach (Tile item in assetTiles2)
        {
            string assetTileName = item.name.ToLower(); // gets the name of the tile
            flatTiles[assetTileName] = item; // inserts the data into a dictionary
        }
        Sprite[] assetSprite = Resources.LoadAll("Tiles/flat Assets", typeof(Sprite)).Cast<Sprite>().ToArray();
        foreach (Sprite item in assetSprite)
        {
            string assetTileName = item.name.ToLower(); // gets the name of the tile
            sprites[assetTileName] = item; // inserts the data into a dictionary
        }
    }
    public static string addTagToBlockName(string blockName, string tag)
    {
        if (doesBlockNameHaveTag(blockName))
        {
            blockName = removeTagFromBlockName(blockName);
        }
        /*if (blockName.ToLower().Contains("smelter"))
        {
            Debug.Log("---");
            Debug.Log(blockName);
            Debug.Log(tag);
            Debug.Log("---");
        }*/
        if (getBrickType(blockName) == "block") // chooses which type of block selector to use
        {
            /* Replacing the word "block" with "SelectorBox_block" in the tile name. */
            return blockName.Replace("block", "") + tag + "_block";
        } else {
            return blockName.Replace("slab", "") + tag + "_slab";
        }
    }

    public static string removeTagFromBlockName(string blockName) {
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

    public static bool isBrickPlayerEditable(string blockName)
    {
        string[] playerEditable = new string[] {"conveyor", "splitter", "merger", "machine", "miner", "smelter", "constructer", "fabricator"};
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
        string[] brickTypes = new string[] {"conveyor", "splitter", "merger", "machine", "miner", "smelter", "constructer", "fabricator"};
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

    public static Sprite getSpriteByName(string key) {
        if (key == null)
        {
            return null;
        }
        key = key.ToLower();
        if (!tiles.ContainsKey(key))
        {

            
            string asset = "imgs/items/"+key;

            Sprite assetTile = (Sprite)Resources.Load(asset, typeof(Sprite)); // loads the tile asset from path
            if (assetTile == null)
            {
                Debug.Log("null assetTile");
                Debug.Log(Resources.Load(asset));
                return null;
            }
            string assetTileName = assetTile.name.ToLower(); // gets the name of the tile
            sprites[assetTileName] = assetTile; // inserts the data into a dictionary
            
        }
        return sprites[key];
        
    }

    public static Tile getTileByName(string key) {
        key = key.ToLower();
        if (!tiles.ContainsKey(key))
        {
            return null;
        }
        return tiles[key];
    }

    public static Tile getTileByNameAndDir(string key, string dir) {
        key = key.ToLower();
        dir = dir.ToLower();
        foreach (var (name, tile) in tiles)
        {
            if (name.Contains(key))
            {
                if (name.Split("-")[0].Contains(dir))
                {
                    return tile;
                }
            }
        }
        
        return null;
    }

    public static AnimatedTile getAnimatedTileByNameAndDir(string key, string dir) {
        key = key.ToLower();
        foreach (var (name, tile) in aTiles)
        {
            if (name.Contains(key))
            {
                if (name.Split("-")[0].Contains(dir))
                {
                    return tile;
                }
            }
        }
        return null;
    }

    public static Tile getFlatTileByName(string key) {
        key = key.ToLower();
        if (!flatTiles.ContainsKey(key))
        {
            return null;
        }
        return flatTiles[key];
    }

    public static AnimatedTile getAnimatedTileByName(string key) {
        key = key.ToLower();
        if (!aTiles.ContainsKey(key))
        {
            return null;
        }
        return aTiles[key];
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
            if (getTileByName(mode).name.ToLower().Contains("block"))
            {
                return "block";
            } else {
                return "slab";
            }
        }
        
    }


    public static string oppositeDir(string dir)
    {
        dir = dir[0].ToString();
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

    public static Vector3Int makeV3Int(int x, int y, int z) // remove
    {
        Vector3Int newV3I = new Vector3Int(x, y, z);
        return newV3I;
    }

    public static Vector3 makeV3(float x, float y, float z) // remove
    {
        Vector3 newV3 = new Vector3(x, y, z);
        return newV3;
    }

    public static Vector3Int getDirV3(string dir, Vector3Int coords, int distance = 1) {
        GlobalMethods.calc +=1;
        //Debug.Log("Times Run: "+GlobalMethods.calc);
        Vector3Int combineCoords = coords;
        foreach (var dirChar in dir)
        {
            combineCoords += GlobalMethods.dirConvertDic[dirChar.ToString()];
        }
        return combineCoords;
    }

    public static Vector3Int combineCoords(Vector3Int c1, Vector3Int c2) { // remove
        return c1 + c2;
    }

    /*
    static List<string> elipDirections = new List<string>() { "U", "D" };
    static List<string> eliDirections = new List<string>() { "no", "U", "D" };
    static List<string> straightDirections = new List<string>() { "no", "op" };
    static List<string> slantDirections = new List<string>() { "no", "op"+"U" };
    static List<string> bendDirections = new List<string>() { "no", "nn" };
    static List<string> minerDirections = new List<string>() { "no" };
    static List<string> fullBlockDirections = new List<string>() { "W", "E", "N", "S" };*/

    public static List<string> getDirections(string tileName) {
        if (tileName.ToLower().Contains("elip"))
        {
            return new List<string>() { "U", "D" };
        } else if (tileName.ToLower().Contains("eli"))
        {
            return new List<string>() { tileName[0].ToString(), "U", "D" };
        } else if (tileName.ToLower().Contains("straight") || tileName.ToLower().Contains("smelter") || tileName.ToLower().Contains("constructer"))
        {
            return new List<string>() { tileName[0].ToString(), oppositeDir(tileName[0].ToString()) };
        } else if (tileName.ToLower().Contains("slant"))
        {
            return new List<string>() { tileName[0].ToString(), oppositeDir(tileName[0].ToString())+"U" };
        } else if (tileName.ToLower().Contains("bend"))
        {
            return new List<string>() { tileName[0].ToString(), nextDir(tileName[0].ToString()) };
        } else if (tileName.ToLower().Contains("miner"))
        {
            return new List<string>() { tileName[0].ToString() };
        } else if (tileName.ToLower().Contains("merger") || tileName.ToLower().Contains("splitter"))
        {
            return new List<string>() { "W", "E", "N", "S" };
        } else if (tileName.ToLower().Contains("fabricator"))
        {
            return new List<string>() { tileName[0].ToString()+tileName[0].ToString(), oppositeDir(tileName[0].ToString())+oppositeDir(tileName[0].ToString()), oppositeDir(tileName[0].ToString())+oppositeDir(tileName[0].ToString())+nextDir(tileName[0].ToString()), oppositeDir(tileName[0].ToString())+oppositeDir(tileName[0].ToString())+prevDir(tileName[0].ToString()) };
        }
        return null;
    }

    public static string nextTileDir(string tileName, string dir) {
        List<string> directions = getDirections(tileName);
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 1;
        if (dirIndex == directions.Count)
        {
            dirIndex = 0;
        }
        return directions[dirIndex];
    }

    public static Bricks getBrickByDirCord(string dir, Vector3Int cord) {
        cord = getDirV3(dir, cord);
        if (General.bricks.ContainsKey(cord))
        {
            return General.bricks[cord];
        }
        return null;
    }

    /*
    public static dynamic GetItemHandlerByDirCord(string dir, Vector3Int cord) {
        cord = getDirV3(dir, cord);
        if (General.bricks.ContainsKey(cord))
        {
            if (General.bricks[cord] is Conveyor)
            {
                return General.bricks[cord].belt;
            }
            return General.bricks[cord];
        }
        return null;
    }//*/

    public static string nextBrickDir(Bricks brick, string dir) {
        List<string> directions = brick.directions;
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 1;
        if (dirIndex == directions.Count)
        {
            dirIndex = 0;
        }
        return directions[dirIndex];
    }

    public static bool isConnectionPossible(string tileName, Vector3Int loc) { // remove - not using
        List<string> dirs = getDirections(tileName);
        foreach (var dir in dirs)
        {
            
            if (General.bricks.ContainsKey(getDirV3(dir, loc)))
            {
                Bricks brick = General.bricks[getDirV3(dir, loc)];
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
            List<string> iDirs = getInputDirections(tileName, loc);
            List<string> oDirs = getOutputDirections(tileName, loc);
            if (iDirs != null)
            {
                foreach (var dir in iDirs)
                {
                    if (General.bricks.ContainsKey(getDirV3(dir, loc)))
                    {
                        Bricks brick = General.bricks[getDirV3(dir, loc)];
                        if (brick.inputDirections != null) {
                            if (brick.inputDirections.Contains(oppositeDir(dir)))
                            {
                                return true;
                            }
                        }
                    
                    }
                }
            }
            
            if (oDirs != null)
            {
                foreach (var dir in oDirs)
                {
                    if (General.bricks.ContainsKey(getDirV3(dir, loc)))
                    {
                        Bricks brick = General.bricks[getDirV3(dir, loc)];
                        if (brick.outputDirections != null) {
                            if (brick.outputDirections.Contains(oppositeDir(dir)))
                            {
                                return true;
                            }
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
            List<string> dirs = getDirections(tileName);
            //Debug.Log(dirs[0] + dirs[1]);
            if (dirs.Count == 2)
            {
                Vector3Int loc1 = getDirV3(dirs[0], loc);
                Vector3Int loc2 = getDirV3(dirs[1], loc);
                //Debug.Log(loc1);
                //Debug.Log(loc2);
                if (General.bricks.ContainsKey(loc1) && General.bricks.ContainsKey(loc2))
                {
                    Bricks brick1 = General.bricks[loc1];
                    Bricks brick2 = General.bricks[loc2];
                    //Debug.Log(brick1.tile.name);
                    //Debug.Log(brick2.tile.name);
                    if (brick1.inputDirections != null && brick2.outputDirections != null)
                    {
                        //Debug.Log("pas1");
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

    public static bool isBrickToBrickConnectionPossible(Bricks brick1, Bricks brick2) { // remove - not using?
        List<string> dirs = brick1.directions;
        foreach (var dir in dirs)
        {
            
            if (brick2.cordinates == getDirV3(dir, brick1.cordinates))
            {
                if (brick2.directions.Contains(oppositeDir(dir)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static string brickToBrickConnectionDirection(Bricks brick1, Bricks brick2) {
        List<string> dirs = brick1.directions;
        foreach (var dir in dirs)
        {
            
            if (brick2.cordinates == getDirV3(dir, brick1.cordinates))
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
    public static List<string> getInputDirections(string tileName, Vector3Int loc) {
        if (tileName.ToLower().Contains("miner"))
        {
            return null;
        }
        List<string> dirs = getDirections(tileName);
        if (tileName.ToLower().Contains("merger")) {
            return getDirections(tileName).Where(c => c != tileName[0].ToString()).ToList();
        } else if (tileName.ToLower().Contains("splitter")) {
            return new List<string>() { tileName[0].ToString() };
        } else if (tileName.ToLower().Contains("smelter") || tileName.ToLower().Contains("constructer")) {
            return new List<string>() { oppositeDir(tileName[0].ToString()) };
        } else if (tileName.ToLower().Contains("fabricator"))
        {
            return new List<string>() { oppositeDir(tileName[0].ToString())+oppositeDir(tileName[0].ToString()), oppositeDir(tileName[0].ToString())+oppositeDir(tileName[0].ToString())+nextDir(tileName[0].ToString()), oppositeDir(tileName[0].ToString())+oppositeDir(tileName[0].ToString())+prevDir(tileName[0].ToString()) };
        }
        foreach (var dir in dirs) // for each direction
        {
            if (General.bricks.ContainsKey(getDirV3(dir, loc)))
            {
                Bricks brick = General.bricks[getDirV3(dir, loc)];
                
                if (brick.directions.Contains(oppositeDir(dir)))
                {
                    Debug.Log(brick.directions[0]);
                    if (brick.outputDirections == null && brick.inputDirections == null)
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
                        Debug.Log(nextTileDir(tileName, dir));
                        Debug.Log(brick.tile);
                        if (brick.tile == null) {
                            return new List<string>() { nextTileDir(tileName, dir) };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { nextTileDir(tileName, dir) };
                        } else {
                            return new List<string>() { nextTileDir(tileName, dir) };
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

    public static List<string> getOutputDirections(string tileName, Vector3Int loc) {
        List<string> dirs = getDirections(tileName);
        if (tileName.ToLower().Contains("miner"))
        {
            return dirs;
        }
        if (tileName.ToLower().Contains("splitter")) {
            return getDirections(tileName).Where(c => c != tileName[0].ToString()).ToList();
        } else if (tileName.ToLower().Contains("merger") || tileName.ToLower().Contains("smelter") || tileName.ToLower().Contains("constructer")) {
            return new List<string>() { tileName[0].ToString() };
        } else if (tileName.ToLower().Contains("fabricator"))
        {
            return new List<string>() { tileName[0].ToString()+tileName[0].ToString() };
        }
        foreach (var dir in dirs)
        {
            if (General.bricks.ContainsKey(getDirV3(dir, loc)))
            {
                Bricks brick = General.bricks[getDirV3(dir, loc)];
                
                if (brick.directions.Contains(oppositeDir(dir)))
                {
                    if (brick.outputDirections == null && brick.inputDirections == null)
                    {
                        return null;
                    }
                    List<string> returnValue = null;
                    if (!brick.outputDirections.Contains(oppositeDir(dir)))
                    {
                        if (brick.tile == null) {
                            return new List<string>() { dir };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { dir };
                        } else {
                            return new List<string>() { dir };
                        }
                    } else {
                        Debug.Log(nextTileDir(tileName, dir));
                        if (brick.tile == null) {
                            return new List<string>() { nextTileDir(tileName, dir) };
                        } else if (isBrickNotExcludedType(brick.tile.name, "conveyor")) {
                            returnValue = new List<string>() { nextTileDir(tileName, dir) };
                        } else {
                            return new List<string>() { nextTileDir(tileName, dir) };
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


    public static Belt getBelt(string tileName, Vector3Int loc, bool overRule = false, List<string> dirs = null) {
        //Debug.Log(loc);
        if (!overRule && (tileName.ToLower().Contains("splitter") || tileName.ToLower().Contains("merger") || tileName.ToLower().Contains("smelter") || tileName.ToLower().Contains("constructer") || tileName.ToLower().Contains("miner") || tileName.ToLower().Contains("fabricator"))) {
            return null;
        }
        if (dirs == null) {
            if (!overRule) {
                dirs = getDirections(tileName);
            } else {
                dirs = new List<string>() { "W", "E", "N", "S" };
            }
        }
        
        foreach (var dir in dirs)
        {
            //Debug.Log(dir);
            if (General.bricks.ContainsKey(getDirV3(dir, loc)))
            {
                Debug.Log("Found a belt");
                dynamic brick = General.bricks[getDirV3(dir, loc)];
                if (brick.directions != null && brick.directions.Contains(oppositeDir(dir[0].ToString())) && (brick.tile == null || brick.tile.name.ToLower().Contains("conveyor")))
                {
                    Debug.Log("Found a connection possible belt");
                    return brick.belt;
                }
            }
        }
        //Debug.Log("No belt found");
        return null;
    }

    /*public static bool isConnectionPossible(Bricks brick, Vector3Int placementCordinates, List<string> placementDirections)
    {
        foreach (var dir in brick.directions)
        {
            if (placementDirections.Contains(dir))
            {
                if (placementCordinates == getDirV3(dir.ToString(), brick.cordinates))
                {
                    return true;
                }
            }
        }
        
        return false;
    }*/



}




