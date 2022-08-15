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
    public GameObject brickInputsObject;
    public GameObject brickOutputsObject;
    private Text brickName;
    private Text brickInputs;
    private Text brickOutputs;
    private static string brickNameSelected;
    private static string brickInputsSelected;
    private static string brickOutputsSelected;
    private static Sprite brickSpriteSelected;
    private static string brickType;
    private static Bricks brickSelected;

    void Start() {
        brickName = brickNameObject.GetComponent<Text>();
        brickInputs = brickInputsObject.GetComponent<Text>();
        brickOutputs = brickOutputsObject.GetComponent<Text>();
    }

    void Update() {
        brickName.text = SelectInspecter.brickNameSelected;
        brickInputs.text = SelectInspecter.brickInputsSelected;
        brickOutputs.text = SelectInspecter.brickOutputsSelected;

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

            NorthBtn.SetActive(true);
            EastBtn.SetActive(true);
            SouthBtn.SetActive(true);
            WestBtn.SetActive(true);
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
                if (brickSelected.belt.subCordinates.Last() != brickSelected)
                {
                    Bricks nextBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) + 1];
                    LoadBrick(nextBrick);
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(true) != null)
            {
                Bricks connectedBrick = brickSelected.belt.getConnectingEdgeBrick(true);
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
                if (brickSelected.belt.subCordinates.First() != brickSelected)
                {
                    Bricks prevBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) - 1];
                    LoadBrick(prevBrick);
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(false) != null)
            {
                Bricks connectedBrick = brickSelected.belt.getConnectingEdgeBrick(false);
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
                if (brickSelected.belt.subCordinates.First() != brickSelected)
                {
                    return true;
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(false) != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool NextBtnChecker() {
        if (brickType == "brick" || brickType == "conveyor") {
            if (brickSelected.belt != null) {
                if (brickSelected.belt.subCordinates.Last() != brickSelected)
                {
                    return true;
                }
            }
        } else if (brickType == "belt") {
            if (brickSelected.belt.getConnectingEdgeBrick(true) != null)
            {
                return true;
            }
        }
        return false;
    }

    public static void NorthBtnTrigger() {

    }

    public static void WestBtnTrigger() {
        
    }

    public static void EastBtnTrigger() {
        
    }

    public static void SouthBtnTrigger() {
        
    }
    
}
