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

                linkedBrick = new Bricks(null, GlobalMethods.GetDirV3("U", cordinates), new List<string>() { "D", GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString() }, null, null, belt, this);

                //Debug.Log(inputDirections);
                //Debug.Log(outputDirections);
                if (inputDirections != null && outputDirections != null)
                {
                    Debug.Log("!!!PASS!!!");
                    linkedBrick.outputDirections = new List<string>();
                    linkedBrick.inputDirections = new List<string>();
                    if (inputDirections[0] == tile.name[0].ToString())
                    {
                        outputDirections.RemoveAt(0);
                        outputDirections.Add("U");
                        linkedBrick.inputDirections.Add("D");
                        linkedBrick.outputDirections.Add(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString());
                    }
                    else
                    {
                        inputDirections.RemoveAt(0);
                        inputDirections.Add("U");
                        linkedBrick.inputDirections.Add(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString());
                        linkedBrick.outputDirections.Add("D"); // Not working ?
                    }
                }
            }

            if (belt == null && tile.name.ToLower().Contains("conveyor"))
            {
                Debug.Log("!!!NEW BELT!!!");
                belt = new Belt(new List<Bricks>() {this});
                /*
                if (linkedBrick != null)
                {
                    belt.AddToBelt(linkedBrick); // adds linkedBrick to belt
                }//*/
            } else if (belt != null && tile.name.ToLower().Contains("conveyor")) {
                if (linkedBrick == null)
                {
                    belt.AddToBelt(this);
                }

                if (linkedBrick != null)
                {
                    if (tile != null)
                    {
                        //Debug.Log("!!!Belt Add SUCCESS!!!");
                        if (belt.AddToBeltCheck(this))
                        {
                            belt.AddToBelt(this);
                            belt.AddToBelt(linkedBrick);
                        } else {
                            belt.AddToBelt(linkedBrick);
                            belt.AddToBelt(this);
                        }
                    }
                }
                /*else if (linkedBrick.tile == null)
                {
                    belt.AddToBelt(this);
                    /*
                    if (linkedBrick.tile.name.ToLower().Contains("slant"))
                    {
                        belt.AddToBelt(this); 
                    } 
                }*/
                
                //Debug.Log(belt.subCordinates.Count.ToString());
                Debug.Log("Belt Length: "+belt.subCordinates.Count.ToString());
            }

            /*
            if (tile.name.ToLower().Contains("slant")) {
                //linkedBrick.belt.AddToBelt(linkedBrick);
                //Debug.Log("Belt Length: "+belt.subCordinates.Count.ToString());

                //Debug.Log(linkedBrick.outputDirections.Count.ToString()); // should not be 0 FIX!!!
                //Debug.Log(linkedBrick.inputDirections.Count.ToString()); // should not be 0 FIX!!!
            }//*/

            if (GlobalMethods.isBrickNotExcludedType(this.tile.name, "conveyor"))
            {
                GlobalMethods.GetBelt(tile.name, cordinates, true).assignDirection(this);
            }
        }
        
    }

    public void changeTileState(string state) {

    }
}