using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SelectInspecter : MonoBehaviour
{

    public Image SelectedBrickImg;
    public GameObject beltBtn;
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
        } else {
            beltBtn.SetActive(true);
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
                if (brick.inputDirections.LastIndexOf(dir) != 0 && brick.inputDirections.Last() != dir)
                {
                    brickInputsSelected += ", ";
                }
                brickInputsSelected += dir.ToString();
            }
            brickInputsSelected = "Inputs: [ "+brickInputsSelected+" ]";
        }
        
        brickOutputsSelected = "";
        if (brick.outputDirections != null)
        {
            foreach (var dir in brick.outputDirections)
            {
                if (brick.outputDirections.LastIndexOf(dir) != 0 && brick.outputDirections.Last() != dir)
                {
                    brickOutputsSelected += ", ";
                }
                brickOutputsSelected += dir.ToString();
            }
            brickOutputsSelected = "Outputs: [ "+brickOutputsSelected+" ]";
        }
        
        brickSpriteSelected = brick.tile.sprite;

        brickType = "brick";
        
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
        if (brickType == "brick") {
            LoadBelt(brickSelected.belt);
        }
    }

    public static void NextBtnTrigger() {
        if (brickType == "brick") {
            if (brickSelected.belt != null) {
                if (brickSelected.belt.subCordinates.Last() != brickSelected)
                {
                    Bricks nextBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) + 1];
                    LoadBrick(nextBrick);
                }
            }
        }
    }

    public static void PrevBtnTrigger() {
        if (brickType == "brick") {
            if (brickSelected.belt != null) {
                if (brickSelected.belt.subCordinates.First() != brickSelected)
                {
                    Bricks prevBrick = brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList()[brickSelected.belt.subCordinates.Where(b => b.tile != null).ToList().FindIndex(x => x == brickSelected) - 1];
                    LoadBrick(prevBrick);
                }
            }
        }
    }
    
}
