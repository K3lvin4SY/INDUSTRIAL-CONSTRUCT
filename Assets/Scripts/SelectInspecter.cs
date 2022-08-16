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
    public GameObject brickInputsObject;
    public GameObject brickOutputsObject;
    private Text brickName;
    private Text brickInputs;
    private Text brickOutputs;
    private Text brickBeltPlace;
    private static string brickNameSelected;
    private static string brickInputsSelected;
    private static string brickOutputsSelected;
    private static string brickPlaceSelected;
    private static Sprite brickSpriteSelected;
    private static string brickType;
    private static Bricks brickSelected;

    void Start() {
        brickName = brickNameObject.GetComponent<Text>();
        brickInputs = brickInputsObject.GetComponent<Text>();
        brickOutputs = brickOutputsObject.GetComponent<Text>();
        brickBeltPlace = brickPlaceObject.GetComponent<Text>();
    }

    void Update() {
        brickName.text = SelectInspecter.brickNameSelected;
        brickInputs.text = SelectInspecter.brickInputsSelected;
        brickOutputs.text = SelectInspecter.brickOutputsSelected;
        brickBeltPlace.text = SelectInspecter.brickPlaceSelected;

        if (brickSpriteSelected != null) {
            SelectedBrickImg.sprite = brickSpriteSelected;
            SelectedBrickImgBg.SetActive(true);
        } else {
            SelectedBrickImgBg.SetActive(false);
        }

        /* Checking if the brick type is a belt. */
        if (SelectInspecter.brickType == "belt") {
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
        } else if (SelectInspecter.brickType == "conveyor") {
            beltBtn.SetActive(true);
            backBtn.SetActive(false);
            NextBtn.SetActive(NextBtnChecker());
            PrevBtn.SetActive(PrevBtnChecker());

            NorthBtn.SetActive(false);
            EastBtn.SetActive(false);
            SouthBtn.SetActive(false);
            WestBtn.SetActive(false);
        } else {
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

    private static void LoadBrick(Bricks brick) {
        if (brick.tile == null)
        {
            brick = brick.linkedBrick;
        }
        brickNameSelected = brick.tile.name;

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

        brickPlaceSelected = "";
        if (brick.belt != null)
        {
            brickPlaceSelected = "Belt Placement: "+(brick.belt.subCordinates.FindIndex(x => x == brick)+1).ToString();
        }
        
        brickSpriteSelected = brick.tile.sprite;

        if (brick.tile.name.ToLower().Contains("conveyor"))
        {
            brickType = "conveyor";
        } else {
            brickType = "brick";
        }
        
        brickSelected = brick;
    }

    private static void LoadBelt(Belt belt) {
        brickNameSelected = "Belt";

        brickInputsSelected = "Length: " + belt.subCordinates.Count.ToString();

        brickOutputsSelected = "";

        brickSpriteSelected = null;

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
                if (brickSelected.tile.name.ToLower().Contains("slant") && brickSelected.belt.subCordinates[brickSelected.belt.subCordinates.Count-2] == brickSelected)
                {
                    //go to next brick
                    if (brickSelected.belt.isBrick(brickSelected) != null)
                    {
                        Bricks connectionBrick = brickSelected.linkedBrick.belt.getConnectingEdgeBrick(brickSelected.linkedBrick.belt.isBrickLast(brickSelected.linkedBrick), true, true);
                        LoadBrick(connectionBrick);
                        Debug.Log("2n");
                    }
                } else if (brickSelected.belt.subCordinates/*.Where(b => b.tile != null).ToList()*/.Last() != brickSelected)
                {
                    Bricks nextBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) + 1];
                    LoadBrick(nextBrick);
                } else {
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
    }

    public static void PrevBtnTrigger() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected.belt != null) {
                if (brickSelected.belt.subCordinates/*.Where(b => b.tile != null).ToList()*/.First() != brickSelected)
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
                if (brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().First() != brickSelected)
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
                if (brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().Last() != brickSelected)
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
    
}
