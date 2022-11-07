using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Conveyor : Bricks
{
    public Belt belt;
    public Conveyor linkedBrick;
    public Conveyor(Tile cTile, Vector3Int coords, List<string> dir, List<string> inputDir, List<string> outputDir, Belt cBelt = null, Conveyor linkBrick = null) : base(cTile, coords, dir, inputDir, outputDir) {
        belt = cBelt;
        linkedBrick = linkBrick;
        General.bricks[coords] = this;

        if (tile != null)
        {
            if (getName().Contains("slant")) // creates an invicibale brick if it is a slant brick
            {
                directions.Remove(GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString()));
                directions.Add("U");

                linkedBrick = new Conveyor(null, GlobalMethods.GetDirV3("U", cordinates), new List<string>() { "D", GlobalMethods.NextTileDir(tile.name, tile.name[0].ToString())[0].ToString() }, null, null, belt, this);
                General.bricks[GlobalMethods.GetDirV3("U", cordinates)] = linkedBrick;
                //Debug.Log(inputDirections);
                //Debug.Log(outputDirections);
                if (inputDirections != null || outputDirections != null)
                {
                    Debug.Log("!!!PASS!!!");
                    linkedBrick.changeOutputDir(new List<string>());
                    linkedBrick.changeInputDir(new List<string>());
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

            if (belt == null) // if its a new conveyor it creates a belt for itself
            {
                Debug.Log("!!!NEW BELT!!!");
                belt = new Belt(new List<Conveyor>() {this});
                //*
                if (linkedBrick != null)
                {
                    belt.AddToBelt(linkedBrick); // adds linkedBrick to belt
                }//*/
            } else if (belt != null) {
                if (linkedBrick == null)
                {
                    belt.AddToBelt(this);
                }

                if (linkedBrick != null) // if conveyor plack has a linked brick it adds the linked brick to the belt
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
        }

        if (tile == null || ((inputDirections != null || outputDirections != null) && tile.name.ToLower().Contains("conveyor"))) // here only conveyor bricks with direction passes through
        {
            if (tile == null)
            {
                //return;
            }
            Debug.Log("p1");
            //*  CODE down bellow may not be finished (its for updating a belt that is already created)
            if (belt != null) {
                if (belt.isBrick(this) != null) // if brick is in end or start of the belt
                {
                    Debug.Log("1");
                    if (belt.getConnectingEdgeBrick(belt.isBrickLast(this)) != null)
                    {
                        Debug.Log("2");
                        dynamic connectionBrick = belt.getConnectingEdgeBrick(belt.isBrickLast(this));
                        if (connectionBrick.inputDirections != null || connectionBrick.outputDirections != null) { // if the brick has direction
                            connectionBrick = belt.getConnectingEdgeBrick(false == belt.isBrickLast(this)); // change the connection brick to the other brick on the oteher side of the belt
                        }
                        if (connectionBrick == null)
                        {
                            connectionBrick = belt.getConnectingEdgeBrick(belt.isBrickLast(this));
                        }
                        if (connectionBrick.tile != null)
                        {
                            Debug.Log(connectionBrick.tile.name);
                        }
                        if (connectionBrick is Conveyor) // old --> (connectionBrick.belt == null)
                        {
                            connectionBrick.belt.assignDirection(this);
                            Debug.Log(connectionBrick.belt.subCordinates.Count.ToString());
                        }
                    }
                }
                Debug.Log("!!!PASSINATION!!!");//*/
                //Debug.Log(GlobalMethods.GetBelt("", cordinates, true, directions).subCordinates.Count.ToString());

                //GlobalMethods.GetBelt("", cordinates, true, directions).assignDirection(this);
            } else {
                Debug.Log("!!!LOOK IN TO PROBLEM!!!");
                Debug.Log(tile);
            }
            
        }


        // This last code for fixing when you connect a belt to an already placed dir brick
        if (inputDirections != null || outputDirections != null)
        {
            if (belt.noDirection())
            {
                Debug.Log("p21");
                changeInputDir(null);
                changeOutputDir(null);
                belt.assignDirection(this);
            }
        }
        
        if (belt != null)
        {
            if (belt.faltyDirection())
            {
                Debug.Log(inputDirections);
                //belt.fixFaltyDirection();
            }
        }
    }



    public string GetItem(bool raw = false, int offset = 0) {
        if (belt != null)
        {
            int index = belt.subCordinates.IndexOf(this);
            if (index+offset > belt.storage.Count-1)
            {
                offset = 0;
            }
            if (belt.storage[index+offset] != null)
            {
                if (raw)
                {
                    return belt.storage[index+offset];
                } else {
                    return "Item: " + belt.storage[index+offset];
                }
            }
        }
        if (raw)
        {
            return null;
        } else {
            return "Item: None";
        }
    }

    public override void receiveItem(string item)
    {
        if (belt != null)
        {
            belt.receiveItem(item);
        }
    }

    public override bool ifStorageFull(string item)
    {
        return belt.ifStorageFull(item);
    }

    public override void changeOutputDir(List<string> dirs) {
        outputDirections = dirs;
        if (tile != null && !tile.name.ToLower().Contains("eli"))
        {
            if (outputDirections != null)
            {
                string dir = outputDirections[0][0].ToString();
                if (dir == "D" || dir == "U")
                {
                    dir = GlobalMethods.oppositeDir(inputDirections[0]);
                }
                changeTileTag("animated+"+dir);
            }
        }
    }
}
