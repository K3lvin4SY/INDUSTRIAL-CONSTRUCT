using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt// : ScriptableObject
{
    public List<Bricks> subCordinates;
    //public Dictionary<int, Bricks> subCordinates;
    public List<GameItem> storage;
    

    

    /// <summary>
    /// It takes a dictionary of Vector3Ints and Tiles, and returns a dictionary of Vector3Ints and
    /// Tiles
    /// It Creates the belt from a given dictionary of Vector3Ints and Tiles
    /// </summary>
    /// <param name="path">The path that the player is currently on.</param>
    public Belt(Dictionary<Vector3Int, Tile> path) {
        int indexNumber = -1;
        int pathIndex = -1;
        string lastPut = "X";
        subCordinates = new List<Bricks>();
        foreach (var (coords, tile) in path)
        {
            indexNumber += 1;

            if (tile == null)
            {
                continue;
            }

            pathIndex += 1;
            
            Tile bTile = tile;
            Vector3Int bCordinates = coords;
            List<string> bOutputDirections = new List<string>();
            List<string> bInputDirections = new List<string>();
            string tileDir = tile.name[0].ToString();
            if (lastPut == "X") // if this is first in list (since if X isn't vhanged it is the first in the list)
            {
                if (!path.Keys.ToList().Contains(GlobalMethods.GetDirV3(tileDir, coords)) || !path.Keys.ToList().Contains(GlobalMethods.GetDirV3(tileDir, GlobalMethods.makeV3Int(coords.x, coords.y, coords.z-1)))) // if path connected to dir of path begining
                {
                    bInputDirections.Add(tileDir);
                    if (tile.name.ToLower().Contains("bend"))
                    {
                        bOutputDirections.Add(GlobalMethods.nextDir(tileDir));
                        lastPut = GlobalMethods.nextDir(tileDir);
                    } else if (tile.name.ToLower().Contains("straight"))
                    {
                        bOutputDirections.Add(GlobalMethods.oppositeDir(tileDir));
                    }
                    
                } else
                {
                    if (tile.name.ToLower().Contains("bend"))
                    {
                        bInputDirections.Add(GlobalMethods.nextDir(tileDir));
                    
                    } else if (tile.name.ToLower().Contains("straight"))
                    {
                        bInputDirections.Add(GlobalMethods.oppositeDir(tileDir));
                    }
                    bOutputDirections.Add(tileDir);
                    lastPut = tileDir;
                    
                }
                break;
            } // -----------------------------------------
            // if brick in miidle of path
            bInputDirections.Add(GlobalMethods.oppositeDir(lastPut));

            if (tile.name.ToLower().Contains("straight")) // if conveyor is straight
            {
                bInputDirections.Add(lastPut);
                //lastPut = lastPut;
            } else if (tile.name.ToLower().Contains("eli")) // if conveyor is an elivator (going up or down)
            {
                if (tile.name.ToLower().Contains("elip")) // if conbveruor is a pipe
                {
                    bOutputDirections.Add(lastPut);
                    //lastPut = lastPut;
                } else { // all elivator entrencases
                    if (path.Values.ToList()[indexNumber-1].name.Contains("elip")) // if converor before was a pipe
                    {
                        bOutputDirections.Add(tileDir);
                        lastPut = tileDir;
                    } else {
                        bOutputDirections.Add("U");
                        lastPut = "U";
                    }
                }

                
            } else if (tile.name.ToLower().Contains("bend")) // if conveoyor is a bend
            {
                if (GlobalMethods.oppositeDir(lastPut) == tileDir)
                {
                    bOutputDirections.Add(GlobalMethods.nextDir(tileDir));
                    lastPut = GlobalMethods.nextDir(tileDir);
                } else {
                    bOutputDirections.Add(GlobalMethods.prevDir(tileDir));
                    lastPut = GlobalMethods.prevDir(tileDir);
                }
            }
            Bricks newBrick = new Bricks(bTile, bCordinates, GlobalMethods.GetDirections(bTile.name), bInputDirections, bOutputDirections, this);
            subCordinates.Add(newBrick);
        }
    }

    public void AddToBelt(Bricks brick, bool last)
    {
        if (last) {
            subCordinates.Add(brick);
        } else {
            subCordinates.Insert(0, brick);
        }
    }

    public void updateDirection(string tileName, Vector3Int loc) {
        
    }

}
