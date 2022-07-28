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
    public Bricks linkedBrick;

    public GameItem storage;

    public Bricks(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Bricks linkBrick = null)
    {
        tile = cTile;
        cordinates = coords;
        inputDirections = inputDir;
        outputDirections = outputDir;
        directions = dir;
        belt = cBelt;
        linkedBrick = linkBrick;
        General.bricks[cordinates] = this;

        if (tile != null)
        {
            if (tile.name.ToLower().Contains("slant"))
            {
                directions.Remove(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString()));
                directions.Add("U");

                linkedBrick = new Bricks(null, GlobalMethods.GetDirV3("U", cordinates), new List<string>() { "D", GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString()) }, new List<string>(), new List<string>(), belt, this);

                if (inputDirections != null && outputDirections != null)
                {
                    if (inputDirections[0] == tile.name[0].ToString())
                    {
                        outputDirections.RemoveAt(0);
                        outputDirections.Add("U");
                        linkedBrick.inputDirections.Add("D");
                        linkedBrick.outputDirections.Add(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString()));
                    }
                    else
                    {
                        inputDirections.RemoveAt(0);
                        inputDirections.Add("U");
                        linkedBrick.inputDirections.Add(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString()));
                        linkedBrick.outputDirections.Add("D"); // Not working ?
                    }
                }
            }

            if (belt == null && tile.name.ToLower().Contains("conveyor"))
            {
                belt = new Belt(new List<Bricks>() {this});
            } else if (belt != null && tile.name.ToLower().Contains("conveyor")) {
                belt.AddToBelt(this);
                Debug.Log(belt.subCordinates.Count.ToString());
            }

            if (tile.name.ToLower().Contains("slant")) {
                linkedBrick.belt.AddToBelt(linkedBrick);
                Debug.Log(belt.subCordinates.Count.ToString());

                Debug.Log(linkedBrick.outputDirections.Count.ToString()); // should not be 0 FIX!!!
                Debug.Log(linkedBrick.inputDirections.Count.ToString()); // should not be 0 FIX!!!
            }
        }
        
    }
}