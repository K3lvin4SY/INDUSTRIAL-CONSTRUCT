using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Belt// : ScriptableObject
{
    public List<Bricks> subCordinates;
    //public Dictionary<int, Bricks> subCordinates;
    public List<GameItem> storage;
    

    

    /// <summary>
    /// It takes a dictionary of Vector3Ints and Tiles, and returns a dictionary of Vector3Ints and
    /// Tiles
    /// It Creates the belt from a given dictionary of Vector3Ints and Tiles
    /// </summary>
    /// <param name="path">The path that the player is currently on.</param>
    public Belt(List<Bricks> path) {
        subCordinates = new List<Bricks>();
        foreach (var brick in path)
        {
            subCordinates.Add(brick);
        }
    }

    public void AddToBelt(Bricks brick)
    {
        foreach (var dir in subCordinates[0].directions)
        {
            if (GlobalMethods.GetDirV3(dir, subCordinates[0].cordinates) == brick.cordinates)
            {
                // in begining of belt
                subCordinates.Insert(0, brick);
                return;
            }
        }

        foreach (var dir in subCordinates.Last().directions)
        {
            if (GlobalMethods.GetDirV3(dir, subCordinates.Last().cordinates) == brick.cordinates)
            {
                // in end of belt
                subCordinates.Add(brick);
                return;
            }
        }
        Debug.Log("!!!ERROR!!!");
    }

    public void updateDirection(string tileName, Vector3Int loc) {
        
    }

}
