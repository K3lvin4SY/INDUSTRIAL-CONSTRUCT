using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt// : ScriptableObject
{
    public Dictionary<int, Bricks> subCordinates;
    public List<GameItem> storage;
    GameSenceHandler gmh = new GameSenceHandler();
    

    

    public void Main(Dictionary<Vector3Int, Tile> path) {
        int indexNumber = -1;
        int pathIndex = -1;
        string lastPut = "X";
        Belt newBelt = new Belt();
        newBelt.subCordinates = new Dictionary<int, Bricks>();
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
            Belt bBelt = newBelt;
            List<string> bOutputDirections = new List<string>();
            List<string> bInputDirections = new List<string>();
            string tileDir = tile.name[0].ToString();
            if (lastPut == "X") // if this is first in list (since if X isn't vhanged it is the first in the list)
            {
                if (!path.Keys.ToList().Contains(gmh.GetDirV3(tileDir, coords)) || !path.Keys.ToList().Contains(gmh.GetDirV3(tileDir, gmh.makeV3Int(coords.x, coords.y, coords.z-1)))) // if path connected to dir of path begining
                {
                    bInputDirections.Add(tileDir);
                    if (tile.name.ToLower().Contains("bend"))
                    {
                        bOutputDirections.Add(gmh.nextDir(tileDir));
                        lastPut = gmh.nextDir(tileDir);
                    } else if (tile.name.ToLower().Contains("straight"))
                    {
                        bOutputDirections.Add(gmh.oppositeDir(tileDir));
                    }
                    
                } else
                {
                    if (tile.name.ToLower().Contains("bend"))
                    {
                        bInputDirections.Add(gmh.nextDir(tileDir));
                    
                    } else if (tile.name.ToLower().Contains("straight"))
                    {
                        bInputDirections.Add(gmh.oppositeDir(tileDir));
                    }
                    bOutputDirections.Add(tileDir);
                    lastPut = tileDir;
                    
                }
                break;
            } // -----------------------------------------
            // if brick in miidle of path
            bInputDirections.Add(gmh.oppositeDir(lastPut));

            if (tile.name.ToLower().Contains("straight")) // if conveyor is straight
            {
                bInputDirections.Add(lastPut);
                lastPut = lastPut;
            } else if (tile.name.ToLower().Contains("eli")) // if conveyor is an elivator (going up or down)
            {
                if (tile.name.ToLower().Contains("elip")) // if conbveruor is a pipe
                {
                    bOutputDirections.Add(lastPut);
                    lastPut = lastPut;
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
                if (gmh.oppositeDir(lastPut) == tileDir)
                {
                    bOutputDirections.Add(gmh.nextDir(tileDir));
                    lastPut = gmh.nextDir(tileDir);
                } else {
                    bOutputDirections.Add(gmh.prevDir(tileDir));
                    lastPut = gmh.prevDir(tileDir);
                }
            }
            Bricks newBrick = new Bricks(bTile, bCordinates, bInputDirections, bOutputDirections, bBelt);
            newBelt.subCordinates.Add(pathIndex, newBrick);
        }
    }
}
