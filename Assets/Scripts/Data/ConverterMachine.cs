using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverterMachine : MonoBehaviour
{
    public Dictionary<int, Bricks> bricks; // 0 is output, 1 is oposite side input & rest is also input
    //public Vector3Int cordinates;
    //int maxOutStorage = 999, maxInStorage = 999;
    public Dictionary<GameItem, int> outStorage;
    public Dictionary<GameItem, int> inStorage;

    public List<char> directions;
    public List<char> inputDirections;
    public List<char> outputDirections;

    /*public void ConvertToBrick()
    {
        // convert to brick
    }

    //public List<Vector3Int> GetMachineBricks//*/

    void LookForItems()
    {
        Debug.Log(Time.time);
    }
}
