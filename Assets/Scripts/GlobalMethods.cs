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
    public static string AddTagToBlockName(string blockName, string tag, bool noTag = true)
    {
        if (!noTag)
        {
            Debug.Log("Feature not implemented yet");
            return null;
        }
        if (getBrickType(blockName) == "block") // chooses which type of block selector to use
        {
            /* Replacing the word "block" with "SelectorBox_block" in the tile name. */
            return blockName.Replace("block", "") + tag + "_block";
        } else {
            return blockName.Replace("slab", "") + tag + "_slab";
        }
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



}




