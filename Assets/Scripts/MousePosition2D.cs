using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class MousePosition2D : MonoBehaviour
{
    public Vector3Int location;
    
    public Tilemap map;
    public static Tile tile;
    public Tile selectTileBlock;
    public Tile selectTileSlab;

    // NOTE: availableZ is a blacklist of z cordinates that you cant place blocks on
    public List<int> availableZ = new List<int>(); // make private
    private Vector3Int lastLocation;
    private Vector3Int selectorLocation;
    private Vector3Int selectorLocation2;

    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();


    private void Start() {
        Debug.Log("test1");
        MousePosition2D.tile = GetTileByName("simple_grass_block");
        Debug.Log(MousePosition2D.tile.name);
        //tile = pgScript.tilePick;
    }

    

    public Tile GetTileByName(string key) {
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


    private bool minimumZ(Vector3Int loc) { // returns true if given locations z value is the bare minimum allowed (if all z values under is filled/underground) 
        for (int i = loc.z-1; i > -1; i--)
        {
            if (!availableZ.Contains(i))
            {
                return false;
            }
        }
        return true;
    }

    private void Update() {
        /*populateGrid pgScript = new populateGrid();
        Debug.Log(pgScript.tilePick2);
        tile = GetTileByName(pgScript.tilePick2);//*/
        // update grid position
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // update mouseWorld position
        
        location.x = map.WorldToCell(mouseWorldPos).x; // update grid x location
        location.y = map.WorldToCell(mouseWorldPos).y; // update grid y location

        if (Input.GetKeyDown(KeyCode.R))
        {
            /*GameSenceHandler gmh = new GameSenceHandler();
            BoundsInt cellBox = new BoundsInt(gmh.GetDirV3("SW", selectorLocation), gmh.makeV3Int(3, 3, 4));
            tileBox = map.GetTilesBlock(cellBox);
            tileBoxCheck = tileBox.Select(s => s == null).ToArray();//*/
            rotateBrick();
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // scroll up z pos
        {
            do
            {
                location.z += 1;
                
            } while (availableZ.Contains(location.z));
            if (availableZ.Count == 0)
            {
                location.z += 1;
            }
            
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // scroll down z pos
        {   
            if (!minimumZ(location))
            {
                do
                {
                    location.z -= 1;
                } while (availableZ.Contains(location.z));
            }

            if (availableZ.Count == 0)
            {
                if (location.z != 0)
                {
                    location.z -= 1;
                }
            }
        }


        //Debug.Log(Input.mousePosition);
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
            buildPath();
            if (availableZ.Count != 0)
            {
                if (MousePosition2D.tile.name.StartsWith("conveyor") && MousePosition2D.tile.name.Contains("straight"))
                //if (true)
                {
                    updatePath();
                } else {
                    placeBlock();
                }
                //return;
            }
            return;
        }

        if (map.HasTile(selectorLocation2))
        {
            //if (map.GetTile(selectorLocation2).name.Contains(tile.name+"sb"))
            if (map.GetTile(selectorLocation2).name.ToLower().Contains("selectorbox"))
            {
                if (Input.GetMouseButtonDown(1)) // cancel path build
                {
                    map.SetTile(selectorLocation2, null);
                    selectorLocation2 = Vector3Int.zero;
                }
                return;
            }
        }
        

        
        
        placeSelectorBox();
    }

    private void placeSelectorBox(bool update = false, string buildingBlock = "default", int loc = 1) {

        if (lastLocation != location || update == true) // runs if pointer have moved to another grid square or if the z position has changed
        {
            

            if (loc == 1)
            {
                if (map.GetTile(selectorLocation)) { // if selectorLocation has a tile
                    if (map.GetTile(selectorLocation).name.Contains("SelectorBox")) { // if tile is selectorbox
                        map.SetTile(selectorLocation, null); // clear grid location
                    }
                }
            } else if (loc == 2)
            {
                if (map.GetTile(selectorLocation2)) { // if selectorLocation2 has a tile
                    if (map.GetTile(selectorLocation2).name.Contains("SelectorBox")) { // if tile is selectorbox
                        map.SetTile(selectorLocation2, null); // clear grid location
                    }
                }
            }
            lastLocation.z = location.z;
            updateZ(buildingBlock);
            
            Vector3Int baseLocation = location;
            baseLocation.z = 0;
            if (availableZ.Count == 0)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    location.z = 0;
                }
            }

            if (!map.HasTile(location)) // in case of a bug or when using control. otherwise the selector shouldn't land on a used location.
            {
                if (!(loc == 2 && location == selectorLocation))
                {
                    if (buildingBlock == "default")
                    {
                        if (getBrickType(buildingBlock) == "block") // chooses which type of block selector to use
                        {
                            map.SetTile(location, GetTileByName("selectorBoxBlock"));
                        } else {
                            map.SetTile(location, GetTileByName("selectorBoxSlab"));
                        }
                    } else {
                        map.SetTile(location, GetTileByName(buildingBlock/*+"sb"*/));
                    }
                }
                
                if (loc == 1)
                {
                    selectorLocation = location; // updates the selectors location
                } else if (loc == 2 && !(location == selectorLocation))
                {
                    selectorLocation2 = location; // updates the selectors location 2
                }
                
            }
             
        }

        
        if (lastLocation.x != location.x || lastLocation.y != location.y)
        {
            if (availableZ.Count == 0)
            {   
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    location.z = 0;
                }
                
            }
            lastLocation = location;
        }
    }

    private string getBrickType(string mode = "default") {
        if (mode == "default")
        {
            if (MousePosition2D.tile.name.ToLower().Contains("block"))
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

    private void updateZ(string buildingBlock = "default") {
        
        availableZ.Clear();
        // ------------------------------- Creates availableZ list of taken z cordiantes -------------------------------
        Vector3Int tempLocation = location; // temp variable to change without chaneing the real one
        for (int i = 0; i < 21; i++) // loops from 0 to 20. uses i as z value
        {
            tempLocation.z = i; // sets i as z value
            if (map.GetTile(tempLocation)) { // if tempLocation is not empty
                availableZ.Add(i); // add taken Z cordiante to list
            }
        }
        tempLocation.z = 0;
        if (availableZ.Count == 0)
        {
            return;
        }
        // ----------------------------------------------------------------------------------------------------------------------------
        
        // ------------------------------- Sorts in aditional z positions. ex blocks that takes two but only ocupies one.
        // append unavailable z cordiantes to list. - makes blocks ocupie two spaces
        List<int> aditionalZ = new List<int>();
        foreach (var z in availableZ)
        {
            tempLocation.z = z;
            if (availableZ.Contains(z+1) == false) // if z+1 is clear
            {
                if (map.GetTile(tempLocation).name.ToLower().Contains("block")) // if it is a block
                {
                    aditionalZ.Add(z+1); // add block space to availableZ
                }
            }

        }
        availableZ.AddRange(aditionalZ);
        aditionalZ.Clear();
        // if building brick is a block, it removes single available z cordiantes between two ocupied ones.
        foreach (var z in availableZ) // add single z layers if blocktype is block
        {
            tempLocation.z = z;
            if (getBrickType(buildingBlock) == "block")
            {
                if (!availableZ.Contains(z+1))
                {
                    if (availableZ.Contains(z+2))
                    {
                        aditionalZ.Add(z+1);
                    }
                }
            }
            
        }
        
        
        availableZ.AddRange(aditionalZ);
        aditionalZ.Clear();
        // removes the available empty space under all the tiles by adding them to the list.
        for (int i = 0; i < availableZ[0]; i++) // add empty tiles under the map
        {
            aditionalZ.Add(i);
        }
        availableZ.AddRange(aditionalZ);
        aditionalZ.Clear();

        // if building brick is a block, it removes available z cordiantes under ocupied ones.
        foreach (var z in availableZ) // add single z layers if blocktype is block
        {
            tempLocation.z = z;
            if (getBrickType(buildingBlock) == "block")
            {
                if (z > 0)
                {
                    if (!availableZ.Contains(z-1))
                    {
                        aditionalZ.Add(z-1);
                    }
                }
            }
            
        }
        // remove unavailable 3x3 spaces 
        if (MousePosition2D.tile.name.Contains("3x3"))
        {
            availableZ.AddRange(aditionalZ);
            aditionalZ.Clear();
            for (int i = 0; i < 21; i++)
            {
                if (!availableZ.Contains(i))
                {
                    tempLocation.z = i;

                    BoundsInt cellBox = new BoundsInt(GameSenceHandler.GetDirV3("SWD", tempLocation), GameSenceHandler.makeV3Int(3, 3, 1));
                    TileBase[] tileBox = map.GetTilesBlock(cellBox);
                    List<bool> tileBoxCheck = new List<bool>();
                    foreach (var item in tileBox)
                    {
                        if (item == null)
                        {
                            tileBoxCheck.Add(false);
                        } else {
                            if (item.name.Contains("block"))
                            {
                                tileBoxCheck.Add(true);
                            } else {
                                tileBoxCheck.Add(false);
                            }
                        }
                    }
                    //tileBoxCheck = tileBox.Select(s => s.name.Contains("block")).ToArray();
                    if (tileBoxCheck.Contains(true))
                    {
                        aditionalZ.Add(i);
                    } else {
                        cellBox = new BoundsInt(GameSenceHandler.GetDirV3("SW", tempLocation), GameSenceHandler.makeV3Int(3, 3, 4));
                        tileBox = map.GetTilesBlock(cellBox);
                        tileBox = tileBox.Where((source, index) =>index != 16).ToArray();
                        bool[] tileBoxCheck2 = tileBox.Select(s => !(s == null)).ToArray();
                        if (tileBoxCheck2.Contains(true))
                        {
                            aditionalZ.Add(i);
                        }
                    }
                    
                }
                
            }
        }
        availableZ.AddRange(aditionalZ);
        availableZ.Sort();
        // ------------------------------- End of finalizing availableZ list -------------------------------



        // ctrl hold down stay on height
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Debug.Log("1");
            if (!map.HasTile(location))
            {   
                Debug.Log("2");
                Vector3Int tmpVec = location;
                tmpVec.z -= 1;
                if (map.HasTile(tmpVec))
                {
                    Debug.Log("ground");
                    if (!map.GetTile(tmpVec).name.Contains("block"))
                    {
                        Debug.Log("ground block");
                        return;
                    }
                } else {
                    return;
                }
                
            } else if (!minimumZ(location)) {
                Debug.Log("ig");
                //fix going in ground blocks
                return;
            }
            return;
        }



        //
        int lastZ = 0;
        foreach (var z in availableZ) // finds the first available position from bottom to top and sets that as the z position in the next if statement.
            {
                if (lastZ != z)
                {
                    break;
                }
                lastZ += 1;
            }
        if (location.z < lastZ) // if current z pos is under the minimum amount
        {
            location.z = lastZ;
        }
        if (lastLocation.x != location.x || lastLocation.y != location.y) // if change of grid placement (mouse position on grid)
        {
            location.z = lastZ;
        }
        
    }

    private void rotateBrick() {
        char[] directions = new char[4]{'N', 'E', 'S', 'W'};
        string currentBrickName = MousePosition2D.tile.name;
        char brickNameDirection = currentBrickName[0];
        if (!directions.Contains(brickNameDirection)) // if current brick cant be rotated
        {
            return;
        }
        int dirIndex = directions.ToList().FindIndex(c => c == brickNameDirection);
        int newDirIndex = dirIndex;
        string newTileName = "";
        for (int i = 0; i != 1;)
        {
            if (newDirIndex + 1 == 4)
            {
                newDirIndex = 0;
            } else {
                newDirIndex += 1;
            }
            newTileName = directions[newDirIndex].ToString() + currentBrickName.Substring(1);
            string[] assetFiles = Directory.GetFiles("Assets/Tiles/Assets/"); // Gets string array of the tile assets file path
            assetFiles = assetFiles.Select(s => s.ToLowerInvariant()).ToArray(); // to lowercase
            bool[] assetFilesCheck = assetFiles.Select(s => s.Contains(newTileName.ToLower())).ToArray();
            if (assetFilesCheck.Contains(true))
            {
                i = 1;
            }
        }
        
        Debug.Log(newTileName);
        MousePosition2D.tile = GetTileByName(newTileName);
    }

    
    public void placeBlock() {
        if (Input.GetMouseButtonDown(0))
        {
            map.SetTile(selectorLocation, MousePosition2D.tile);

            if (!Input.GetKey(KeyCode.LeftControl)) {
                placeSelectorBox(true); // for updating the solector box on top of the place object
            }
            
        }
    }

    public void updatePath() {
        placeSelectorBox(false, "selectorboxblock", 2);
        // use algoritm to build selector box path

        // in algoritm when looking at cordinates to se if they are taken. use x & y and take updateZ():s calculation for availableZ and make it to a new function.
    }


    public void buildPath() {
        if (Input.GetMouseButtonDown(0))
        {
            // replace selector boxes with real ones
        }
    }


}
