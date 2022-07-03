using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMisc : MonoBehaviour
{
    public static string[] gameStates = new string[] {"build", "move", "break", "select"};
    public static string gameState = "build";

    public static void Build() {
        Debug.Log("Build");
        General.Instance.SetGameMode("build");
    }

    public static void Move() {
        Debug.Log("Move");
        General.Instance.SetGameMode("move");
    }

    public static void Break() {
        Debug.Log("Break");
        General.Instance.SetGameMode("break");
    }

    public static void Select() {
        Debug.Log("Select");
        General.Instance.SetGameMode("select");
    }
}
