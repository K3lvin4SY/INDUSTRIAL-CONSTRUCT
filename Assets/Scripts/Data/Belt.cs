using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt : ScriptableObject
{
    public Dictionary<int, Bricks> subCordinates;
    public List<GameItem> storage;
    private char[] directions = new char[4]{'N', 'E', 'S', 'W'};

    private char oppositeDir(char dir)
    {
        if (dir == 'N')
        {
            return 'S';
        } else if (dir == 'S') {
            return 'N';
        } else if (dir == 'E') {
            return 'W';
        } else if (dir == 'W') {
            return 'E';
        } else if (dir == 'U') {
            return 'D';
        } else if (dir == 'D') {
            return 'U';
        }
        return 'U';
    }

    private char nextDir(char dir) {
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex += 1;
        if (dirIndex == 4)
        {
            dirIndex = 0;
        }
        return directions[dirIndex];
    }

    private char prevDir(char dir) {
        int dirIndex = directions.ToList().FindIndex(c => c == dir);
        dirIndex -= 1;
        if (dirIndex == -1)
        {
            dirIndex = 3;
        }
        return directions[dirIndex];
    }

    Vector3Int makeV3Int(int x, int y, int z)
    {
        Vector3Int newV3I = new Vector3Int(x, y, z);
        return newV3I;
    }

    public Vector3Int GetDirV3(char dir, Vector3Int coords) {
        Dictionary<char, Vector3Int> dirConvertDic = new Dictionary<char, Vector3Int>();
        dirConvertDic.Add('N', makeV3Int(0, 1, 0)); // North
        dirConvertDic.Add('E', makeV3Int(1, 0, 0)); // East
        dirConvertDic.Add('S', makeV3Int(0, -1, 0)); // South
        dirConvertDic.Add('W', makeV3Int(-1, 0, 0)); // West
        dirConvertDic.Add('U', makeV3Int(0, 0, 1)); // Up
        dirConvertDic.Add('D', makeV3Int(0, 0, -1)); // Down
        Vector3Int combineCoords = coords + dirConvertDic[dir];
        return combineCoords;
    }

    public void Creation(Dictionary<Vector3Int, Tile> path) {
        int indexNumber = -1;
        int pathIndex = -1;
        char lastPut = 'X';
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
            
            Bricks newBrick = new Bricks();
            newBrick.tile = tile;
            newBrick.cordinates = coords;
            newBrick.belt = newBelt;
            if (lastPut == 'X') // if this is first in list (since if X isn't vhanged it is the first in the list)
            {
                if (!path.Keys.ToList().Contains(GetDirV3(tile.name[0], coords)) || !path.Keys.ToList().Contains(GetDirV3(tile.name[0], makeV3Int(coords.x, coords.y, coords.z-1)))) // if path connected to dir of path begining
                {
                    newBrick.inputDirections.Add(tile.name[0]);
                    if (tile.name.ToLower().Contains("bend"))
                    {
                        newBrick.outputDirections.Add(nextDir(tile.name[0]));
                        lastPut = nextDir(tile.name[0]);
                    } else if (tile.name.ToLower().Contains("straight"))
                    {
                        newBrick.outputDirections.Add(oppositeDir(tile.name[0]));
                    }
                    
                } else
                {
                    if (tile.name.ToLower().Contains("bend"))
                    {
                        newBrick.inputDirections.Add(nextDir(tile.name[0]));
                    
                    } else if (tile.name.ToLower().Contains("straight"))
                    {
                        newBrick.inputDirections.Add(oppositeDir(tile.name[0]));
                    }
                    newBrick.outputDirections.Add(tile.name[0]);
                    lastPut = tile.name[0];
                    
                }
                break;
            } // -----------------------------------------
            // if brick in miidle of path
            newBrick.inputDirections.Add(oppositeDir(lastPut));

            if (tile.name.ToLower().Contains("straight")) // if conveyor is straight
            {
                newBrick.inputDirections.Add(lastPut);
                lastPut = lastPut;
            } else if (tile.name.ToLower().Contains("eli")) // if conveyor is an elivator (going up or down)
            {
                if (tile.name.ToLower().Contains("elip")) // if conbveruor is a pipe
                {
                    newBrick.outputDirections.Add(lastPut);
                    lastPut = lastPut;
                } else { // all elivator entrencases
                    if (path.Values.ToList()[indexNumber-1].name.Contains("elip")) // if converor before was a pipe
                    {
                        newBrick.outputDirections.Add(tile.name[0]);
                        lastPut = tile.name[0];
                    } else {
                        newBrick.outputDirections.Add('U');
                        lastPut = 'U';
                    }
                }

                
            } else if (tile.name.ToLower().Contains("bend")) // if conveoyor is a bend
            {
                if (oppositeDir(lastPut) == tile.name[0])
                {
                    newBrick.outputDirections.Add(nextDir(tile.name[0]));
                    lastPut = nextDir(tile.name[0]);
                } else {
                    newBrick.outputDirections.Add(prevDir(tile.name[0]));
                    lastPut = prevDir(tile.name[0]);
                }
            }
            newBelt.subCordinates.Add(pathIndex, newBrick);
        }
    }
}
