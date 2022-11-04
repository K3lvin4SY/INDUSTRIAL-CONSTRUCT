using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SelectInspecter : MonoBehaviour
{

    public Image SelectedBrickImg;
    public GameObject beltBtn;
    public GameObject backBtn;
    public GameObject NorthBtn;
    public GameObject EastBtn;
    public GameObject SouthBtn;
    public GameObject WestBtn;
    public GameObject NextBtn;
    public GameObject PrevBtn;
    public GameObject SelectedBrickImgBg;
    public GameObject brickNameObject;
    public GameObject brickPlaceObject;
    public GameObject brickCordianteObject;
    public GameObject brickStorageObject;
    public GameObject brickInputsObject;
    public GameObject brickOutputsObject;
    public GameObject brickPowerObject;
    public GameObject craftingObject;
    public Image brickPowerLblImg;
    public Image brickPowerBtnImg;
    private Text brickName;
    private Text brickInputs;
    private Text brickOutputs;
    private Text brickCordiante;
    private Text brickStorage;
    private Text brickBeltPlace;
    private static string brickNameSelected;
    private static string brickInputsSelected;
    private static string brickCordianteSelected;
    private static string brickOutputsSelected;
    private static string brickPlaceSelected;
    private static Sprite brickSpriteSelected;
    private static string brickType;
    public static Bricks brickSelected;
    public static Fabricator fabricatorSelected;

    public Sprite powerLblOn;
    public Sprite powerLblOff;
    public Sprite powerBtnOn;
    public Sprite powerBtnOff;

    void Start() {
        brickName = brickNameObject.GetComponent<Text>();
        brickInputs = brickInputsObject.GetComponent<Text>();
        brickOutputs = brickOutputsObject.GetComponent<Text>();
        brickBeltPlace = brickPlaceObject.GetComponent<Text>();
        brickCordiante = brickCordianteObject.GetComponent<Text>();
        brickStorage = brickStorageObject.GetComponent<Text>();
    }

    void Update() {
        brickName.text = SelectInspecter.brickNameSelected;
        brickInputs.text = SelectInspecter.brickInputsSelected;
        brickOutputs.text = SelectInspecter.brickOutputsSelected;
        brickBeltPlace.text = SelectInspecter.brickPlaceSelected;
        if (SelectInspecter.brickType == "miner")
        {
            brickBeltPlace.text = "";
        } else if (SelectInspecter.brickType == "converter")
        {
            brickBeltPlace.text = "Storage1: "+SelectInspecter.brickSelected.inStorage.Count.ToString();
            brickStorage.text = "Storage2: "+SelectInspecter.brickSelected.outStorage.Count.ToString();
        }
        if (SelectInspecter.brickType == "conveyor")
        {
            brickStorage.text = SelectInspecter.brickSelected.GetItem();
        } else {
            if (SelectInspecter.brickType != "converter")
            {
                brickStorage.text = "";
            }
        }
        brickCordiante.text = SelectInspecter.brickCordianteSelected;

        if (brickSpriteSelected != null) {
            SelectedBrickImg.sprite = brickSpriteSelected;
            SelectedBrickImgBg.SetActive(true);
        } else {
            SelectedBrickImgBg.SetActive(false);
        }

        if (brickSelected.powerOn)
        {
            brickPowerLblImg.sprite = powerLblOn;
            brickPowerBtnImg.sprite = powerBtnOn;
        } else {
            brickPowerLblImg.sprite = powerLblOff;
            brickPowerBtnImg.sprite = powerBtnOff;
        }

        if (SelectInspecter.brickType == "miner" || SelectInspecter.brickType == "converter")
        {
            int loopNum = 0;
            foreach (Transform child in craftingObject.transform) {
                if (loopNum >= 3)
                {
                    if (brickSelected.crafting["output"].Count == 1 && brickSelected.crafting["output"][0] != null)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<Image>().sprite = GlobalMethods.GetSpriteByName(brickSelected.crafting["output"][0]);
                    } else {
                        child.gameObject.SetActive(false);
                    }
                    
                } else {
                    if (brickSelected.crafting["input"].Count-1 >= loopNum && brickSelected.crafting["input"][0] != null)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<Image>().sprite = GlobalMethods.GetSpriteByName(brickSelected.crafting["input"][loopNum]);
                    } else {
                        child.gameObject.SetActive(false);
                    }
                    
                }
                loopNum++;
            }
        } else {
            foreach (Transform child in craftingObject.transform) {
                child.gameObject.SetActive(false);
            }
        }

        /* Checking if the brick type is a belt. */
        if (SelectInspecter.brickType == "belt") {
            string temptext = "";
            foreach (var item in brickSelected.belt.storage)
            {
                temptext += item+", ";
            }
            brickBeltPlace.text = temptext;

            beltBtn.SetActive(false);
            backBtn.SetActive(true);
            NextBtn.SetActive(NextBtnChecker());
            PrevBtn.SetActive(PrevBtnChecker());
            /*/
            if (brickSelected.belt.isBrick(brickSelected) != null)
            {
                if (brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected)) == null) {
                    if (brickSelected.belt.isBrick(brickSelected) == "first&last")
                    {
                        NextBtn.SetActive(false);
                        PrevBtn.SetActive(false);
                    } else if (brickSelected.belt.isBrickLast(brickSelected)) {
                        NextBtn.SetActive(false);
                        PrevBtn.SetActive(true);
                    } else {
                        PrevBtn.SetActive(false);
                        NextBtn.SetActive(true);
                    }
                    
                } else {
                    NextBtn.SetActive(true);
                    PrevBtn.SetActive(true);
                }
            }//*/
            

            NorthBtn.SetActive(false);
            EastBtn.SetActive(false);
            SouthBtn.SetActive(false);
            WestBtn.SetActive(false);
            brickPowerObject.SetActive(false);
            craftingObject.SetActive(false);
        } else if (SelectInspecter.brickType == "conveyor") {
            beltBtn.SetActive(true);
            backBtn.SetActive(false);
            NextBtn.SetActive(NextBtnChecker());
            PrevBtn.SetActive(PrevBtnChecker());

            NorthBtn.SetActive(false);
            EastBtn.SetActive(false);
            SouthBtn.SetActive(false);
            WestBtn.SetActive(false);
            brickPowerObject.SetActive(false);
            craftingObject.SetActive(false);
        } else if (SelectInspecter.brickType == "miner") {
            beltBtn.SetActive(false);
            backBtn.SetActive(false);
            NextBtn.SetActive(false);
            PrevBtn.SetActive(false);

            NorthBtn.SetActive(false);
            EastBtn.SetActive(false);
            SouthBtn.SetActive(false);
            WestBtn.SetActive(false);
            brickPowerObject.SetActive(true);
            craftingObject.SetActive(true);
        } else if (SelectInspecter.brickType == "converter") {
            beltBtn.SetActive(false);
            backBtn.SetActive(false);
            NextBtn.SetActive(false);
            PrevBtn.SetActive(false);

            NorthBtn.SetActive(false);
            EastBtn.SetActive(false);
            SouthBtn.SetActive(false);
            WestBtn.SetActive(false);
            brickPowerObject.SetActive(false);
            craftingObject.SetActive(true);
        } else if (SelectInspecter.brickType == "fabricator") {
            beltBtn.SetActive(false);
            backBtn.SetActive(false);
            NextBtn.SetActive(false);
            PrevBtn.SetActive(false);

            NorthBtn.SetActive(false);
            EastBtn.SetActive(false);
            SouthBtn.SetActive(false);
            WestBtn.SetActive(false);
            brickPowerObject.SetActive(false);
            craftingObject.SetActive(true);
        } else {
            beltBtn.SetActive(false);
            backBtn.SetActive(false);
            NextBtn.SetActive(false);
            PrevBtn.SetActive(false);
            brickPowerObject.SetActive(false);
            craftingObject.SetActive(false);

            if (brickSelected.directions.Contains("N")) {
                NorthBtn.SetActive(DirBtnChecker("N"));
            } else {
                NorthBtn.SetActive(false);
            }
            if (brickSelected.directions.Contains("E")) {
                EastBtn.SetActive(DirBtnChecker("E"));
            } else {
                EastBtn.SetActive(false);
            }
            if (brickSelected.directions.Contains("S")) {
                SouthBtn.SetActive(DirBtnChecker("S"));
            } else {
                SouthBtn.SetActive(false);
            }
            if (brickSelected.directions.Contains("W")) {
                WestBtn.SetActive(DirBtnChecker("W"));
            } else {
                WestBtn.SetActive(false);
            }
        }
    }
    public static void InspectAtCordiante(Vector3Int cordinate) // move this to other file, maybe
    {
        Debug.Log("Inspecting at " + cordinate);
        if (General.bricks.ContainsKey(cordinate))
        {
            Bricks inspectedBrick = General.bricks[cordinate];
            if (inspectedBrick.belt != null)
            {
                if (inspectedBrick.belt.selected)
                {
                    //Display Belt Info
                    LoadBelt(inspectedBrick.belt);

                    brickSelected = inspectedBrick;
                    Controller.Instance.UseWindow(Controller.Instance.selectInspector);
                    return;
                }
            }
            //Display Brick Info
            //brickName.text = inspectedBrick.tile.name;
            
            LoadBrick(inspectedBrick);
            Controller.Instance.UseWindow(Controller.Instance.selectInspector);
            return;
        }
        else
        {
            Debug.Log("No brick at " + cordinate);
        }
    }

    private static string GetName(string brickName) {
        brickName = brickName.ToLower();
        string newBrickName = "";
        if (brickName.Contains("conveyor"))
        {
            newBrickName += "Conveyor ";
            if (brickName.Contains("straight"))
            {
                newBrickName += "Straight";
            } else if (brickName.Contains("eli"))
            {
                newBrickName += "Elivator";
            } else if (brickName.Contains("bend"))
            {
                newBrickName += "Turn";
            } else if (brickName.Contains("slant"))
            {
                newBrickName += "Tilt";
            }
        } else if (brickName.Contains("merger")) {
            newBrickName += "Merger";
        } else if (brickName.Contains("splitter")) {
            newBrickName += "Splitter";
        } else if (brickName.Contains("miner")) {
            newBrickName += "Miner";
        } else if (brickName.Contains("constructor")) {
            newBrickName += "Constructor";
        } else if (brickName.Contains("smelter")) {
            newBrickName += "Smelter";
        } else if (brickName.Contains("fabricator")) {
            newBrickName += "Fabricator";
        } else {
            newBrickName += "Unknown";
        }

        return newBrickName;
    }

    private static void LoadBrick(dynamic brick) {
        // if try to load tile empty brick
        if (brick.tile == null)
        {
            brick = brick.linkedBrick;
        }

        // Name
        brickNameSelected = GetName(brick.tile.name);

        // Input
        brickInputsSelected = "";
        if (brick.inputDirections != null)
        {
            foreach (var dir in brick.inputDirections)
            {
                if (brick.inputDirections.LastIndexOf(dir) != 0 )//&& brick.inputDirections.Last() != dir)
                {
                    brickInputsSelected += ", ";
                }
                brickInputsSelected += dir.ToString();
            }
            brickInputsSelected = "Inputs: [ "+brickInputsSelected+" ]";
        } else {
            brickInputsSelected = "Inputs: [ None ]";
        }
        
        // Output
        brickOutputsSelected = "";
        if (brick.outputDirections != null)
        {
            foreach (var dir in brick.outputDirections)
            {
                if (brick.outputDirections.LastIndexOf(dir) != 0 )//&& brick.outputDirections.Last() != dir)
                {
                    brickOutputsSelected += ", ";
                }
                brickOutputsSelected += dir.ToString();
            }
            brickOutputsSelected = "Outputs: [ "+brickOutputsSelected+" ]";
        } else {
            brickOutputsSelected = "Outputs: [ None ]";
        }

        // Belt place
        brickPlaceSelected = "";
        if (brick.belt != null)
        {
            List<Bricks> tmpSubCordinates = brick.belt.subCordinates;
            brickPlaceSelected = "Belt Placement: "+(tmpSubCordinates.FindIndex(x => x == brick)+1).ToString();
        }
        
        brickSpriteSelected = brick.tile.sprite;

        if (brick.tile.name.ToLower().Contains("conveyor"))
        {
            brickType = "conveyor";
        } else if (brick.tile.name.ToLower().Contains("fabricator"))
        {
            brickType = "fabricator";
        } else if (brick.tile.name.ToLower().Contains("miner"))
        {
            brickType = "miner";
        } else if (brick.tile.name.ToLower().Contains("smelter") || brick.tile.name.ToLower().Contains("constructor"))
        {
            brickType = "converter";
        } else {
            brickType = "brick";
        }

        // cordiantes
        brickCordianteSelected = "X: " + brick.cordinates.x.ToString() + "\nY: " + brick.cordinates.y.ToString() + "\nZ: " + brick.cordinates.z.ToString();
        
        // update brickSelected
        brickSelected = brick;
    }

    private static void LoadBelt(Belt belt) {
        brickNameSelected = "Belt";

        brickInputsSelected = "Length: " + belt.subCordinates.Count.ToString();

        brickOutputsSelected = "";

        brickSpriteSelected = null;

        brickPlaceSelected = "";

        brickCordianteSelected = "";

        brickType = "belt";
    }

    public static void BeltBtnTrigger() {
        if (brickType == "conveyor") {
            LoadBelt(brickSelected.belt);
        }
    }

    public static void BackBtnTrigger() {
        if (brickType == "belt") {
            LoadBrick(brickSelected);
        }
    }

    public static void NextBtnTrigger() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected.belt != null) {
                Debug.Log("belt1");
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-2] == brickSelected && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-1] == brickSelected.linkedBrick)
                {
                    Debug.Log("s1");
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected.linkedBrick) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), true, true);
                        LoadBrick(connectionBrick);
                        Debug.Log("2n2");
                    }
                } else if (brickSelected.belt.subCordinates/*.Where(b => b.tile != null).ToList()*/.Last() != brickSelected)
                {
                    Debug.Log("p2");
                    Bricks nextBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) + 1];
                    LoadBrick(nextBrick);
                } else {
                    Debug.Log("p3");
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected), true, true);
                        LoadBrick(connectionBrick);
                        Debug.Log("2n");
                    }
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(true, true, true) != null)
            {
                Bricks connectedBrick = brickSelected.belt.getConnectingEdgeBrick(true, true, true);
                if (connectedBrick.belt != null)
                {
                    if (connectedBrick.tile == null)
                    {
                        brickSelected = connectedBrick.belt.subCordinates[1];
                    } else {
                        brickSelected = connectedBrick;
                    }
                    LoadBelt(connectedBrick.belt);
                } else {
                    LoadBrick(connectedBrick);
                }
            }
        }
        Debug.Log("end4");
    }

    public static void PrevBtnTrigger() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected.belt != null) {
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[1] == brickSelected && brickSelected.belt.subCordinates[0] == brickSelected.linkedBrick)
                {
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected.linkedBrick) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), false, true);
                        LoadBrick(connectionBrick);
                        Debug.Log("2n");
                    }
                } else if (brickSelected.belt.subCordinates/*.Where(b => b.tile != null).ToList()*/.First() != brickSelected)
                {
                    Bricks prevBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) - 1];
                    LoadBrick(prevBrick);
                    Debug.Log("1p");
                } else {
                    //go to previous brick
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected), false, true);
                        LoadBrick(connectionBrick);
                        Debug.Log("2p");
                    }
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(false, false, true) != null)
            {
                Bricks connectedBrick = brickSelected.belt.getConnectingEdgeBrick(false, false, true);
                if (connectedBrick.belt != null)
                {
                    if (connectedBrick.tile == null)
                    {
                        brickSelected = connectedBrick.belt.subCordinates[1];
                    } else {
                        brickSelected = connectedBrick;
                    }
                    LoadBelt(connectedBrick.belt);
                } else {
                    LoadBrick(connectedBrick);
                }
            }
        }
    }

    private bool PrevBtnChecker() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected.belt != null) {
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[1] == brickSelected && brickSelected.belt.subCordinates[0] == brickSelected.linkedBrick)
                {
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected.linkedBrick) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), false, true);
                        if (connectionBrick == null)
                        {
                            return false;
                        } else {
                            return true;
                        }
                    }
                } else if (brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().First() != brickSelected)
                {
                    return true;
                } else {
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected), false, true);
                        if (connectionBrick != null)
                        {
                            return true;
                        }
                    }
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(false, false, true) != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool NextBtnChecker() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected.belt != null) {
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-2] == brickSelected && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-1] == brickSelected.linkedBrick)
                {
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected.linkedBrick) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), true, true);
                        if (connectionBrick == null)
                        {
                            return false;
                        } else {
                            return true;
                        }
                    }
                } else if (brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().Last() != brickSelected)
                {
                    return true;
                } else {
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected), true, true);
                        if (connectionBrick != null)
                        {
                            return true;
                        }
                    }
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(true, true, true) != null)
            {
                return true;
            }
        }
        return false;
    }

    public static void NorthBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.GetDirV3("N", brickSelected.cordinates)))
        {
            LoadBrick(General.bricks[GlobalMethods.GetDirV3("N", brickSelected.cordinates)]);
        }
    }

    public static void WestBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.GetDirV3("W", brickSelected.cordinates)))
        {
            LoadBrick(General.bricks[GlobalMethods.GetDirV3("W", brickSelected.cordinates)]);
        }
    }

    public static void EastBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.GetDirV3("E", brickSelected.cordinates)))
        {
            LoadBrick(General.bricks[GlobalMethods.GetDirV3("E", brickSelected.cordinates)]);
        }
    }

    public static void SouthBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.GetDirV3("S", brickSelected.cordinates)))
        {
            LoadBrick(General.bricks[GlobalMethods.GetDirV3("S", brickSelected.cordinates)]);
        }
    }

    private bool DirBtnChecker(string dir) {
        if (General.bricks.ContainsKey(GlobalMethods.GetDirV3(dir, brickSelected.cordinates)))
        {
            foreach (var dir2 in General.bricks[GlobalMethods.GetDirV3(dir, brickSelected.cordinates)].directions)
            {
                if (GlobalMethods.GetDirV3(dir2, General.bricks[GlobalMethods.GetDirV3(dir, brickSelected.cordinates)].cordinates) == brickSelected.cordinates)
                {
                    return true;
                }
            }
            
        }
        return false;
    }

    public static void PowerBtnTrigger() {
        if (brickSelected.crafting["output"].Count >= 1)
        {
            brickSelected.powerOn = !brickSelected.powerOn;
            if (brickSelected.powerOn)
            {
                brickSelected.changeTileTag("animated");
            } else {
                brickSelected.changeTileTag(null);
            }
        }
    }
    
}
