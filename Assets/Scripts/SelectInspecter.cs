using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectInspecter : MonoBehaviour
{

    public static Text brickName;
    public static void InspectAtCordiante(Vector3Int cordinate)
    {
        Debug.Log("Inspecting at " + cordinate);
        if (General.bricks.ContainsKey(cordinate))
        {
            Bricks inspectedBrick = General.bricks[cordinate];
            if (inspectedBrick.belt != null)
            {
                if (inspectedBrick.belt.selected)
                {
                    Controller.Instance.UseWindow(Controller.Instance.selectInspector);
                    //Display Belt Info
                    return;
                }
            }
            Controller.Instance.UseWindow(Controller.Instance.selectInspector);
            //Display Brick Info
            brickName.text = inspectedBrick.tile.name;
            return;
        }
        else
        {
            Debug.Log("No brick at " + cordinate);
        }
    }
}
