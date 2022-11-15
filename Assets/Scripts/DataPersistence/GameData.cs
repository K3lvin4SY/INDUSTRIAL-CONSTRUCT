using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static Dictionary<Vector3Int, dynamic> bricks = new Dictionary<Vector3Int, dynamic>();
    public static Dictionary<Vector3Int, dynamic> tickers = new Dictionary<Vector3Int, dynamic>();
    
    public GameData() {
        
    }
}
