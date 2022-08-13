using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SelectInspecter : MonoBehaviour
{

    public GameObject brickNameObject;
    public GameObject brickInputsObject;
    public GameObject brickOutputsObject;
    public Text brickName;
    public Text brickInputs;
    public Text brickOutputs;
    public static string brickNameSelected;
    public static string brickInputsSelected;
    public static string brickOutputsSelected;

    void Start() {
        brickName = brickNameObject.GetComponent<Text>();
        brickInputs = brickInputsObject.GetComponent<Text>();
        brickOutputs = brickOutputsObject.GetComponent<Text>();
    }

    void Update() {
        brickName.text = SelectInspecter.brickNameSelected;
        brickInputs.text = "Inputs: [ "+SelectInspecter.brickInputsSelected+" ]";
        brickOutputs.text = "Outputs: [ "+SelectInspecter.brickOutputsSelected+" ]";
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
                    brickNameSelected = "Belt";
                    Controller.Instance.UseWindow(Controller.Instance.selectInspector);
                    return;
                }
            }
            //Display Brick Info
            //brickName.text = inspectedBrick.tile.name;
            brickNameSelected = inspectedBrick.tile.name;

            brickInputsSelected = "";
            if (inspectedBrick.inputDirections != null)
            {
                foreach (var dir in inspectedBrick.inputDirections)
                {
                    if (inspectedBrick.inputDirections.LastIndexOf(dir) != 0 && inspectedBrick.inputDirections.Last() != dir)
                    {
                        brickInputsSelected += ", ";
                    }
                    brickInputsSelected += dir.ToString();
                } 
            }
            
            brickOutputsSelected = "";
            if (inspectedBrick.outputDirections != null)
            {
                foreach (var dir in inspectedBrick.outputDirections)
                {
                    if (inspectedBrick.outputDirections.LastIndexOf(dir) != 0 && inspectedBrick.outputDirections.Last() != dir)
                    {
                        brickOutputsSelected += ", ";
                    }
                    brickOutputsSelected += dir.ToString();
                } 
            }
            
            
            Controller.Instance.UseWindow(Controller.Instance.selectInspector);
            return;
        }
        else
        {
            Debug.Log("No brick at " + cordinate);
        }
    }
}
