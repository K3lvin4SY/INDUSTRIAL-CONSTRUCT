using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityToolbag;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{

    public static General Instance;
    public static Dictionary<Vector3Int, dynamic> bricks = new Dictionary<Vector3Int, dynamic>();
    public static Dictionary<Vector3Int, dynamic> tickers = new Dictionary<Vector3Int, dynamic>();
    public Vector3Int location;
    private int controlZ; // the z value of the height when control is held down
    public Sprite selectedSprite;
    
    public Tilemap map;
    public static Tile tile;

    // NOTE: availableZ is a blacklist of z cordinates that you cant place blocks on
    public List<int> availableZ = new List<int>(); // make private

    // NOTE: occupiedZ is a list of z cordinates that player intrecactable blocks are placed on
    public List<int> occupiedZ = new List<int>(); // make private
    private Vector3Int lastLocation;
    private Vector3Int selectorLocation;
    private Vector3Int selectorLocation2;
    public string gameState;

    public GameObject gridMap;
    public GameObject audioEmitter;
    Vector3 offset;


    private void Start() {
        General.Instance = this;
        gameState = "select";
        GlobalMethods.loadAllAssets();
        populateGrid.Instance.Populate();
        InvokeRepeating("tick", 0f, 1.5f);  //0s delay, repeat every 1.5s
        Debug.Log("test1");
        General.tile = GlobalMethods.getTileByName("E-conveyor_Straight_slab");
        Debug.Log(General.tile);
        Debug.Log(General.tile.name);
        //tile = pgScript.tilePick;
    }

    public void SetGameMode(string mode) {
        gameState = mode;

        // reset
        RemoveSelectorBoxes();
    }
    

    private void tick() {
        var watch = new System.Diagnostics.Stopwatch();
        var watch2 = new System.Diagnostics.Stopwatch();
        watch.Start();
        foreach (var (cord, ticker) in General.tickers)
        {
            if (ticker.tile != null && ticker.tile.name.ToLower().Contains("miner"))
            {
                watch2.Start();
                ticker.GenerateItem();
                watch2.Stop();
                Debug.Log($"Execution Time2: {watch2.ElapsedMilliseconds} ms");
            } else if (ticker.tile != null && ticker.tile.name.ToLower().Contains("fabricator"))
            {
                watch2.Start();
                ticker.convertItem();
                watch2.Stop();
                Debug.Log($"Execution Time2: {watch2.ElapsedMilliseconds} ms");
            }
        }
        watch.Stop();
        Debug.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
        Debug.Log("--------------");
    }


    private bool minimumZ(Vector3Int loc) { // returns true if given locations z value is the bare minimum allowed (if all z values under is filled/underground) 
        //Debug.Log("active");
        for (int i = loc.z-1; i > -1; i--)
        {
            if (gameState == "build") {
                if (!availableZ.Contains(i))
                {
                    return false;
                }
            } else if (gameState == "select" || gameState == "break") {
                if (occupiedZ.Contains(i))
                {
                    return false;
                }
            }
            
        }
        return true;
    }

    public void UpdateControlZ() {
        controlZ = location.z;
    }

    private bool maximumZ(Vector3Int loc) {
        for (int i = loc.z+1; i < 21; i++)
        {
            if (gameState == "build") {
                if (!availableZ.Contains(i))
                {
                    return false;
                }
            } else if (gameState == "select" || gameState == "break") {
                if (occupiedZ.Contains(i))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void Update() {
        /*populateGrid pgScript = new populateGrid();
        Debug.Log(pgScript.tilePick2);
        tile = getTileByName(pgScript.tilePick2);//*/
        // update grid position
        Vector2 mouseWorldPos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition); // update mouseWorld position
        Vector3 mouseWorldPos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        location.x = map.WorldToCell(mouseWorldPos2).x; // update grid x location
        location.y = map.WorldToCell(mouseWorldPos2).y; // update grid y location


        if (map.HasTile(selectorLocation2))
        {
            //if (map.GetTile(selectorLocation2).name.Contains(tile.name+"sb"))
            if (map.GetTile(selectorLocation2).name.ToLower().Contains("selectorbox") || map.GetTile(selectorLocation2).name.ToLower().Contains("selectorredbox"))
            {
                if (Input.GetMouseButtonDown(1)) // cancel path build
                {
                    map.SetTile(selectorLocation2, null);
                    selectorLocation2 = Vector3Int.zero;
                }
                return;
            }
        }
        

        
        if (gameState == "build")
        {
            placeSelectorBox();
        }

        if (gameState == "select" || gameState == "break")
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mouseWorldPos3);
            //Debug.Log(targetObject);
            markSelectedTile();
            //add
        }

        if (gameState == "move")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D targetObject = Physics2D.OverlapPoint(mouseWorldPos3);
                if (targetObject)
                {
                    gridMap = targetObject.transform.gameObject;
                    offset = gridMap.transform.position - mouseWorldPos3;
                }
            }
            if (gridMap)
            {
                gridMap.transform.position = mouseWorldPos3 + offset;
            }
            if (Input.GetMouseButtonUp(0) && gridMap)
            {
                gridMap = null;
            }
        }
        
    }
    //*
    public void RemoveSelectorBoxes() {

        if (map.GetTile(selectorLocation)) { // if selectorLocation has a tile
            if (map.GetTile(selectorLocation).name.Contains("SelectorBox") || map.GetTile(selectorLocation).name.Contains("SelectorRedBox")) { // if tile is selectorbox
                map.SetTile(selectorLocation, null); // clear grid location
            } else if (map.GetTile(selectorLocation).name.ToLower().Contains("lected")) { // if tile is marked
                map.SetTile(selectorLocation, GlobalMethods.getTileByName(GlobalMethods.removeTagFromBlockName(map.GetTile(selectorLocation).name))); // turn back to original tile
            }
        }
        if (map.GetTile(selectorLocation2)) { // if selectorLocation2 has a tile
            if (map.GetTile(selectorLocation2).name.Contains("SelectorBox") || map.GetTile(selectorLocation2).name.Contains("SelectorRedBox")) { // if tile is selectorbox
                map.SetTile(selectorLocation2, null); // clear grid location
            }
        }
    }//*/

    private void markSelectedTile(bool update = false) {
        string markTag;
        if (gameState == "select")
        {
            markTag = "selected";
        } else {
            markTag = "breaklected";
        }
        if (lastLocation != location || update == true) // runs if pointer have moved to another grid square or if the z position has changed
        {
            //reseting the old selection
            if (map.GetTile(selectorLocation)) { // if selectorLocation has a tile
                if (map.GetTile(selectorLocation).name.Contains("lected")) { // if tile has been marked
                    if (General.bricks.ContainsKey(selectorLocation)) // temporary if statement for brick not in bricks dictionary - REMOVE WHEN GAME SAVE is finished or WHEN ALL BRICKS IS in the bricks dictionary
                    {
                        if (General.bricks[selectorLocation].tile.name.ToLower().Contains("fabricator"))
                        {
                            General.bricks[selectorLocation].fabricator.deSelect();
                        } else if (General.bricks[selectorLocation] is Conveyor)
                        {
                            General.bricks[selectorLocation].belt.Deselect();
                            General.bricks[selectorLocation].resetTileTag(); // turn back to original tile
                        } else {
                            General.bricks[selectorLocation].resetTileTag(); // turn back to original tile
                        }
                    } else {
                        map.SetTile(selectorLocation, GlobalMethods.getTileByName(GlobalMethods.removeTagFromBlockName(map.GetTile(selectorLocation).name))); // turn back to original tile
                    }
                    
                        
                    
                    
                }
            }

            lastLocation.z = location.z;
            updateZ();

            if (map.HasTile(location)) {
                TileBase tile = map.GetTile(location);
                Tile tmpTile = GlobalMethods.getTileByName(GlobalMethods.addTagToBlockName(tile.name, markTag));
                Debug.Log(GlobalMethods.addTagToBlockName(tile.name, markTag));
                if (GlobalMethods.isBrickPlayerEditable(tmpTile.name)) {
                    if (General.bricks.ContainsKey(location))
                    {
                        // add tag via bricks class
                        General.bricks[location].changeTileTag(markTag, temp: true);
                        if (tile.name.ToLower().Contains("fabricator"))
                        {
                            FabricatorComponent fc = General.bricks[location];
                            fc.fabricator.select(markTag);
                        }
                    } else {
                        map.SetTile(location, tmpTile); // set tile to selected
                    }
                    if (Controller.shiftPressed)
                    {
                        if (General.bricks.ContainsKey(location)) // temporary if statement for brick not in bricks dictionary - REMOVE WHEN GAME SAVE is finished or WHEN ALL BRICKS IS in the bricks dictionary
                        {
                            if (General.bricks[location] is Conveyor)
                            {
                                General.bricks[location].belt.Select(markTag);
                            }
                        }
                        
                    }
                    selectedSprite = tmpTile.sprite; // update selected sprite
                    selectorLocation = location; // update selectorLocation to current grid location
                } else {
                    selectedSprite = null; // update selected sprite
                }
            } else {
                selectedSprite = null; // update selected sprite
            }
            

            if (lastLocation.x != location.x || lastLocation.y != location.y)
            {
                lastLocation = location;
            }


        }
    }
    private void placeSelectorBox(bool update = false, string buildingBlock = "default", int loc = 1) {

        if (lastLocation != location || update == true) // runs if pointer have moved to another grid square or if the z position has changed
        {
            

            if (loc == 1)
            {
                
                if (map.GetTile(selectorLocation)) { // if selectorLocation has a tile
                    //Debug.Log(map.GetTile(selectorLocation).name);
                    if (map.GetTile(selectorLocation).name.Contains("SelectorBox") || map.GetTile(selectorLocation).name.Contains("SelectorRedBox")) { // if tile is selectorbox
                        
                        map.SetTile(selectorLocation, null); // clear grid location
                    }
                }
            } else if (loc == 2)
            {
                if (map.GetTile(selectorLocation2)) { // if selectorLocation2 has a tile
                    if (map.GetTile(selectorLocation2).name.Contains("SelectorBox") || map.GetTile(selectorLocation).name.Contains("SelectorRedBox")) { // if tile is selectorbox
                        map.SetTile(selectorLocation2, null); // clear grid location
                    }
                }
            }
            lastLocation.z = location.z;
            
            updateZ(buildingBlock);
            
            
            if (!availableZ.Contains(controlZ) && Input.GetKey(KeyCode.LeftControl))
            {
                Debug.Log(controlZ);
                location.z = controlZ;
            }

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
                        if (GlobalMethods.arePathsColliding(General.tile.name, location) || GlobalMethods.willCollitionOccur(General.tile.name, location))
                        {
                            map.SetTile(location, GlobalMethods.getTileByName(GlobalMethods.addTagToBlockName(General.tile.name, "SelectorRedBox"))); // set tile to selectorredbox
                        } else
                        {
                            map.SetTile(location, GlobalMethods.getTileByName(GlobalMethods.addTagToBlockName(General.tile.name, "SelectorBox"))); // set tile to selectorbox
                        }
                        
                    } else {
                        map.SetTile(location, GlobalMethods.getTileByName(buildingBlock/*+"sb"*/));
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

    

    private void updateZ(string buildingBlock = "default") {
        
        availableZ.Clear();
        occupiedZ.Clear();
        // ------------------------------- Creates availableZ list of taken z cordiantes -------------------------------
        Vector3Int tempLocation = location; // temp variable to change without chaneing the real one
        for (int i = 0; i < 21; i++) // loops from 0 to 20. uses i as z value
        {
            tempLocation.z = i; // sets i as z value
            if (map.GetTile(tempLocation)) { // if tempLocation is not empty
                availableZ.Add(i); // add taken Z cordiante to list
                if (GlobalMethods.isBrickPlayerEditable(map.GetTile(tempLocation).name)) //if tile is conveyor
                {
                    occupiedZ.Add(i); // add taken Z cordiante to list
                }
                
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
            if (GlobalMethods.getBrickType(buildingBlock) == "block")
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
            if (GlobalMethods.getBrickType(buildingBlock) == "block")
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
        if (General.tile.name.Contains("3x3"))
        {
            availableZ.AddRange(aditionalZ);
            aditionalZ.Clear();
            for (int i = 0; i < 21; i++)
            {
                if (!availableZ.Contains(i))
                {
                    tempLocation.z = i;

                    BoundsInt cellBox = new BoundsInt(GlobalMethods.getDirV3("SWD", tempLocation), GlobalMethods.makeV3Int(3, 3, 1));
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
                        cellBox = new BoundsInt(GlobalMethods.getDirV3("SW", tempLocation), GlobalMethods.makeV3Int(3, 3, 4));
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
            if (!map.HasTile(location))
            {   
                Vector3Int tmpVec = location;
                tmpVec.z -= 1;
                if (map.HasTile(tmpVec))
                {
                    Debug.Log(map.GetTile(tmpVec).name);
                    if (!map.GetTile(tmpVec).name.Contains("block"))
                    {
                        Debug.Log("ground block");
                        return;
                    } else {

                    }
                } else {
                    return;
                }
                
            } else if (!minimumZ(location)) {
                Debug.Log("ig");
                //fix going in ground blocks
                return;
            }
            //return;
        }



        //
        int lastZ = 0;
        if (gameState == "build") {
            
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
        } else if (gameState == "select" || gameState == "break") {
            if (occupiedZ.Count > 0) {
                lastZ = occupiedZ.Last();
            } else {
                lastZ = -10;
            }
            if (lastLocation.x != location.x || lastLocation.y != location.y) // if change of grid placement (mouse position on grid)
            {
                location.z = lastZ;
            }
        }
        
    }

    public void scrollWheelZPos(int direction) {
        if (gameState == "build")
        {
            if (!minimumZ(location) || direction == 1)
            {
                do
                {
                    location.z += direction;
                    
                } while (availableZ.Contains(location.z));
            }
            
            if (availableZ.Count == 0)
            {
                if (location.z != 0 || direction == 1)
                {
                    location.z += direction;
                }
            }
        } else if (gameState == "select" || gameState == "break") {
            if ((!minimumZ(location) && direction == -1) || (direction == 1 && !maximumZ(location)))
            {
                do
                {
                    location.z += direction;

                } while (!occupiedZ.Contains(location.z));
            }
            
            /*if (occupiedZ.Count == 0)
            {
                if (location.z != 0 || direction == 1)
                {
                    location.z += direction;
                }
            }//*/
        } else if (gameState == "move") {
            Vector3 scaleChange = new Vector3(0.2f, 0.2f, 0.2f);
            if (direction == 1)
            {
                if (map.transform.localScale.x < 10f)
                {
                    map.transform.localScale += scaleChange;
                } else {
                    map.transform.localScale = new Vector3(10f, 10f, 10f);
                }
            } else {
                if (map.transform.localScale.x > 0.2f)
                {
                    map.transform.localScale -= scaleChange;
                } else {
                    map.transform.localScale = scaleChange;
                }
                
            }
        }

        
    }

    public void mouseClick() {
        if (gameState == "build")
        {
            buildPath();
            if (availableZ.Count != 0)
            {
                if (General.tile.name.StartsWith("conveyor") && General.tile.name.Contains("straight"))
                //if (true)
                {
                    updatePath();
                } else {
                    placeBlock();
                }
                //return;
            }
            return;
        } else if (gameState == "move") {

        } else if (gameState == "select") {
            if (map.HasTile(selectorLocation))
            {
                if (map.GetTile(selectorLocation).name.ToLower().Contains("selected"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SelectInspecter.inspectAtCordiante(selectorLocation);
                    }
                }
            }
            
        } else if (gameState == "break") {
            if (map.HasTile(selectorLocation))
            {
                if (map.GetTile(selectorLocation).name.ToLower().Contains("lected"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //break block at selectorLocation
                        if (General.bricks.ContainsKey(selectorLocation))
                        {
                            General.bricks[selectorLocation].destroy();
                        }
                    }
                }
            }
            
        }
    }

    public void rotateBrick() {
        if (gameState == "build")
        {
            char[] directions = new char[4]{'N', 'E', 'S', 'W'};
            string currentBrickName = General.tile.name;
            char brickNameDirection = currentBrickName[0];
            if (!directions.Contains(brickNameDirection)) // if current brick cant be rotated
            {
                return;
            }
            int dirIndex = directions.ToList().FindIndex(c => c == brickNameDirection);
            int newDirIndex = dirIndex;
            string newTileName = "";
            while (true)
            {
                if (newDirIndex + 1 == 4)
                {
                    newDirIndex = 0;
                } else {
                    newDirIndex += 1;
                }
                newTileName = directions[newDirIndex].ToString() + currentBrickName.Substring(1);
                if (GlobalMethods.getTileNames().Contains(newTileName.ToLower()))
                {
                    break;
                }
            }
            
            Debug.Log(newTileName);
            General.tile = GlobalMethods.getTileByName(newTileName);
            placeSelectorBox(update: true);
        }
    }

    
    public void placeBlock() {
        if (Input.GetMouseButtonDown(0))
        {
            if (!map.GetTile(selectorLocation).name.Contains("SelectorRedBox")) {

                if (map.GetTile(selectorLocation).name.ToLower().Contains("fabricator"))
                {
                        audioEmitter.GetComponents<AudioSource>()[7].Play();
                    new Fabricator(General.tile, selectorLocation, GlobalMethods.getDirections(General.tile.name), GlobalMethods.getInputDirections(General.tile.name, selectorLocation), GlobalMethods.getOutputDirections(General.tile.name, selectorLocation));
                } else {
                    map.SetTile(selectorLocation, General.tile);
                    Map.updateColumn(selectorLocation);
                    if (map.GetTile(selectorLocation).name.ToLower().Contains("conveyor"))
                    {
                        audioEmitter.GetComponents<AudioSource>()[5].Play();
                        new Conveyor(General.tile, selectorLocation, GlobalMethods.getDirections(General.tile.name), GlobalMethods.getInputDirections(General.tile.name, selectorLocation), GlobalMethods.getOutputDirections(General.tile.name, selectorLocation), GlobalMethods.getBelt(General.tile.name, selectorLocation));
                    } else if (map.GetTile(selectorLocation).name.ToLower().Contains("splitter"))
                    {
                        audioEmitter.GetComponents<AudioSource>()[6].Play();
                        new Splitter(General.tile, selectorLocation, GlobalMethods.getDirections(General.tile.name), GlobalMethods.getInputDirections(General.tile.name, selectorLocation), GlobalMethods.getOutputDirections(General.tile.name, selectorLocation), GlobalMethods.getBelt(General.tile.name, selectorLocation));
                    } else if (map.GetTile(selectorLocation).name.ToLower().Contains("merger"))
                    {
                        audioEmitter.GetComponents<AudioSource>()[6].Play();
                        new Merger(General.tile, selectorLocation, GlobalMethods.getDirections(General.tile.name), GlobalMethods.getInputDirections(General.tile.name, selectorLocation), GlobalMethods.getOutputDirections(General.tile.name, selectorLocation), GlobalMethods.getBelt(General.tile.name, selectorLocation));
                    } else if (map.GetTile(selectorLocation).name.ToLower().Contains("miner"))
                    {
                        audioEmitter.GetComponents<AudioSource>()[6].Play();
                        new Miner(General.tile, selectorLocation, GlobalMethods.getDirections(General.tile.name), GlobalMethods.getInputDirections(General.tile.name, selectorLocation), GlobalMethods.getOutputDirections(General.tile.name, selectorLocation), GlobalMethods.getBelt(General.tile.name, selectorLocation));
                    } else if (map.GetTile(selectorLocation).name.ToLower().Contains("smelter") || map.GetTile(selectorLocation).name.ToLower().Contains("constructer"))
                    {
                        audioEmitter.GetComponents<AudioSource>()[6].Play();
                        new Converter(General.tile, selectorLocation, GlobalMethods.getDirections(General.tile.name), GlobalMethods.getInputDirections(General.tile.name, selectorLocation), GlobalMethods.getOutputDirections(General.tile.name, selectorLocation), GlobalMethods.getBelt(General.tile.name, selectorLocation));
                    }
                }

                
                
                if (!Input.GetKey(KeyCode.LeftControl)) {
                    placeSelectorBox(true); // for updating the solector box on top of the place object
                }
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
