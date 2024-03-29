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
    public static dynamic brickSelected;
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

        if (brickSelected is Miner)
        {
            if (brickSelected.powerOn)
            {
                brickPowerLblImg.sprite = powerLblOn;
                brickPowerBtnImg.sprite = powerBtnOn;
            } else {
                brickPowerLblImg.sprite = powerLblOff;
                brickPowerBtnImg.sprite = powerBtnOff;
            }
        }
        

        if (SelectInspecter.brickType == "miner" || SelectInspecter.brickType == "converter" || SelectInspecter.brickType == "fabricator")
        {
            dynamic craftingHandler;
            if (brickSelected.tile.name.ToLower().Contains("fabricator"))
            {
                craftingHandler = fabricatorSelected;
            } else {
                craftingHandler = brickSelected;
            }
            int loopNum = 0;
            foreach (Transform child in craftingObject.transform) {
                if (loopNum >= 3)
                {
                    if (craftingHandler.crafting["output"].Count == 1 && craftingHandler.crafting["output"][0] != null)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<Image>().sprite = GlobalMethods.getSpriteByName(craftingHandler.crafting["output"][0]);
                    } else {
                        child.gameObject.SetActive(false);
                    }
                    
                } else {
                    if (craftingHandler.crafting["input"].Count-1 >= loopNum && craftingHandler.crafting["input"][0] != null)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<Image>().sprite = GlobalMethods.getSpriteByName(craftingHandler.crafting["input"][loopNum]);
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
    public static void inspectAtCordiante(Vector3Int cordinate) // move this to other file, maybe
    {
        Debug.Log("Inspecting at " + cordinate);
        if (General.bricks.ContainsKey(cordinate))
        {
            dynamic inspectedBrick = General.bricks[cordinate];
            if (inspectedBrick is Conveyor)
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
            
            loadBrick(inspectedBrick);
            Controller.Instance.UseWindow(Controller.Instance.selectInspector);
            return;
        }
        else
        {
            Debug.Log("No brick at " + cordinate);
        }
    }

    private static string getName(string brickName) {
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
        } else if (brickName.Contains("constructer")) {
            newBrickName += "Constructer";
        } else if (brickName.Contains("smelter")) {
            newBrickName += "Smelter";
        } else if (brickName.Contains("fabricator")) {
            newBrickName += "Fabricator";
        } else {
            newBrickName += "Unknown";
        }

        return newBrickName;
    }

    private static void loadBrick(dynamic brick) {
        Bricks orginalBrick = brick;
        bool isFabricator = false;
        // if try to load tile empty brick
        if (brick.tile == null)
        {
            brick = brick.linkedBrick;
        }
        // load fabricator
        if (brick.tile.name.ToLower().Contains("fabricator"))
        {
            brick = brick.fabricator;
            isFabricator = true;
        }
        // Name
        brickNameSelected = getName(brick.tile.name);

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
        if (!isFabricator && brick is Conveyor)
        {
            List<Conveyor> tmpSubCordinates = brick.belt.subCordinates;
            brickPlaceSelected = "Belt Placement: "+(tmpSubCordinates.FindIndex(x => x == brick)+1).ToString();
        }
        
        brickSpriteSelected = brick.tile.sprite;

        if (brick.tile.name.ToLower().Contains("conveyor"))
        {
            brickType = "conveyor";
        } else if (brick.tile.name.ToLower().Contains("fabricator"))
        {
            brickType = "fabricator";
            fabricatorSelected = brick;
        } else if (brick.tile.name.ToLower().Contains("miner"))
        {
            brickType = "miner";
        } else if (brick.tile.name.ToLower().Contains("smelter") || brick.tile.name.ToLower().Contains("constructer"))
        {
            brickType = "converter";
        } else {
            brickType = "brick";
        }

        // cordiantes
        brickCordianteSelected = brick.cordinates.x.ToString() + " :X\n" + brick.cordinates.y.ToString() + " :Y\n" + brick.cordinates.z.ToString() + " :Z";
        
        // update brickSelected
        brickSelected = orginalBrick;
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
            loadBrick(brickSelected);
        }
    }

    public static void NextBtnTrigger() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected is Conveyor) {
                Debug.Log("belt1");
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-2] == brickSelected && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-1] == brickSelected.linkedBrick)
                {
                    Debug.Log("s1");
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected.linkedBrick) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), true, true);
                        loadBrick(connectionBrick);
                        Debug.Log("2n2");
                    }
                } else if (brickSelected.belt.subCordinates/*.Where(b => b.tile != null).ToList()*/[brickSelected.belt.subCordinates.Count-1] != brickSelected)
                {
                    Debug.Log("p2");
                    Conveyor tmpBrickSelected = brickSelected;
                    Conveyor nextBrick = tmpBrickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[tmpBrickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) + 1];
                    loadBrick(nextBrick);
                } else {
                    Debug.Log("p3");
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected), true, true);
                        loadBrick(connectionBrick);
                        Debug.Log("2n");
                    }
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(true, true, true) != null)
            {
                dynamic connectedBrick = brickSelected.belt.getConnectingEdgeBrick(true, true, true);
                if (connectedBrick is Conveyor)
                {
                    if (connectedBrick.tile == null)
                    {
                        brickSelected = connectedBrick.belt.subCordinates[1];
                    } else {
                        brickSelected = connectedBrick;
                    }
                    LoadBelt(connectedBrick.belt);
                } else {
                    loadBrick(connectedBrick);
                }
            }
        }
        Debug.Log("end4");
    }

    public static void PrevBtnTrigger() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected is Conveyor) {
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[1] == brickSelected && brickSelected.belt.subCordinates[0] == brickSelected.linkedBrick)
                {
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected.linkedBrick) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), false, true);
                        loadBrick(connectionBrick);
                        Debug.Log("2n");
                    }
                } else if (brickSelected.belt.subCordinates/*.Where(b => b.tile != null).ToList()*/[0] != brickSelected)
                {
                    Conveyor tmpBrickSelected = brickSelected;
                    Conveyor prevBrick = tmpBrickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[tmpBrickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) - 1];
                    loadBrick(prevBrick);
                    Debug.Log("1p");
                } else {
                    //go to previous brick
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.belt.getConnectingEdgeBrick(brickSelected.belt.isBrickLast(brickSelected), false, true);
                        loadBrick(connectionBrick);
                        Debug.Log("2p");
                    }
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(false, false, true) != null)
            {
                dynamic connectedBrick = brickSelected.belt.getConnectingEdgeBrick(false, false, true);
                if (connectedBrick is Conveyor)
                {
                    if (connectedBrick.tile == null)
                    {
                        brickSelected = connectedBrick.belt.subCordinates[1];
                    } else {
                        brickSelected = connectedBrick;
                    }
                    LoadBelt(connectedBrick.belt);
                } else {
                    loadBrick(connectedBrick);
                }
            }
        }
    }

    private bool PrevBtnChecker() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected is Conveyor) {
                Conveyor tmpBrickSelected = brickSelected;
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
                } else if (tmpBrickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().First() != brickSelected)
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
            if (brickSelected is Conveyor) {
                Conveyor tmpBrickSelected = brickSelected;
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
                } else if (tmpBrickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().Last() != brickSelected)
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
        if (General.bricks.ContainsKey(GlobalMethods.getDirV3("N", brickSelected.cordinates)))
        {
            loadBrick(General.bricks[GlobalMethods.getDirV3("N", brickSelected.cordinates)]);
        }
    }

    public static void WestBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.getDirV3("W", brickSelected.cordinates)))
        {
            loadBrick(General.bricks[GlobalMethods.getDirV3("W", brickSelected.cordinates)]);
        }
    }

    public static void EastBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.getDirV3("E", brickSelected.cordinates)))
        {
            loadBrick(General.bricks[GlobalMethods.getDirV3("E", brickSelected.cordinates)]);
        }
    }

    public static void SouthBtnTrigger() {
        if (General.bricks.ContainsKey(GlobalMethods.getDirV3("S", brickSelected.cordinates)))
        {
            loadBrick(General.bricks[GlobalMethods.getDirV3("S", brickSelected.cordinates)]);
        }
    }

    private bool DirBtnChecker(string dir) {
        if (General.bricks.ContainsKey(GlobalMethods.getDirV3(dir, brickSelected.cordinates)))
        {
            foreach (var dir2 in General.bricks[GlobalMethods.getDirV3(dir, brickSelected.cordinates)].directions)
            {
                if (GlobalMethods.getDirV3(dir2, General.bricks[GlobalMethods.getDirV3(dir, brickSelected.cordinates)].cordinates) == brickSelected.cordinates)
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
