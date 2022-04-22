using System.Numerics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSenceHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3Int makeV3Int(int x, int y, int z)
    {
        Vector3Int newV3I = new Vector3Int(x, y, z);
        return newV3I;
    }

    public Vector3Int GetDirV3(string dir, Vector3Int coords, int distance = 1) {
        Dictionary<string, Vector3Int> dirConvertDic = new Dictionary<string, Vector3Int>();
        dirConvertDic.Add("N", makeV3Int(0, 1*distance, 0));
        dirConvertDic.Add("E", makeV3Int(1*distance, 0, 0));
        dirConvertDic.Add("S", makeV3Int(0, -1*distance, 0));
        dirConvertDic.Add("W", makeV3Int(-1*distance, 0, 0));
        dirConvertDic.Add("U", makeV3Int(0, 0, 1*distance)); // Up
        dirConvertDic.Add("D", makeV3Int(0, 0, -1*distance)); // Down
        Vector3Int combineCoords = coords + dirConvertDic[dir[0].ToString()];
        foreach (var dirChar in dir)
        {
            if (dirChar == dir[0])
            {
                continue;
            }
            combineCoords += dirConvertDic[dirChar.ToString()];
        }
        return combineCoords;
    }

    public bool IsConnectionPossible(Bricks brick, Vector3Int placementCordinates, List<char> placementDirections)
    {
        foreach (var dir in brick.directions)
        {
            if (placementDirections.Contains(dir))
            {
                if (placementCordinates == GetDirV3(dir.ToString(), brick.cordinates))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}

/*
public class GeneratorMachine
{
    public Bricks brick;
    int maxOutStorage = 999;
    public GameItem outStorage;
    public List<char> inputDirections;
}

public class ConverterMachine
{
    public Dictionary<Port, Bricks> bricks; // 0 is output, 1 is oposite side input & rest is also input
    //public Vector3Int cordinates;
    int maxOutStorage = 999, maxInStorage = 999;
    public Dictionary<GameItem, int> outStorage;
    public Dictionary<GameItem, int> inStorage;

    public List<char> directions;
    public List<char> inputDirections;
    public List<char> outputDirections;

    /*public void ConvertToBrick()
    {
        // convert to brick
    }

    //public List<Vector3Int> GetMachineBricks//

    void LookForItems()
    {
        Debug.Log(Time.time);
    }

}

public class GameItem
{
    public string name;
    public int amount = 1;

}

public class Merger
{
    public Bricks brick;
}

public class Splitter
{
    public Bricks brick;
}

public class Belt
{
    public Vector3Int cordinates;
    public Dictionary<int, Bricks> subCordinates;

    
}

public class Bricks
{
    public Belt belt;
    public Vector3Int cordinates;
    public BrickType type;

    public List<char> directions;
    public List<char> inputDirections;
    public List<char> outputDirections;

    public GameItem storage;

}

public class BrickType
{
    public bool conveyer = false;
    public bool generator = false;
    public bool converter = false;
    public bool merger = false;
    public bool splitter = false;
}

public class Port
{
    public string port; // input, output || null
}

public class MaterialSource
{
    public string material;
    public Vector3Int cordinates;
}*/