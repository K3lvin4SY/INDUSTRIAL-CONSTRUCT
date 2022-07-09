using System.Numerics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSenceHandler : MonoBehaviour
{

    
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