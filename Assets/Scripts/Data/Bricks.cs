using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

//[CreateAssetMenu(fileName = "GameSence", menuName = "Game/Bricks")]
public class Bricks// : ScriptableObject
{
    public Belt belt;
    public Tile tile;
    public Vector3Int cordinates;
    public List<string> directions;
    public List<string> inputDirections;
    public List<string> outputDirections;

    public GameItem storage;

    public Bricks(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt)
    {
        tile = cTile;
        cordinates = coords;
        inputDirections = inputDir;
        outputDirections = outputDir;
        directions = dir;
        belt = cBelt;
        General.bricks[cordinates] = this;
        if (belt == null && tile.name.ToLower().Contains("conveyor"))
        {
            belt = new Belt(new Dictionary<Vector3Int, Tile>() { { cordinates, tile } });
        } else if (belt != null && tile.name.ToLower().Contains("conveyor")) {
            belt.AddToBelt(this);
        }
        
    }
}