using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[CreateAssetMenu(fileName = "GameSence", menuName = "Game/Bricks")]
public class Bricks// : ScriptableObject
{
    public Belt belt;
    public Tile tile;
    public Vector3Int cordinates;
    public List<string> directions;
    public List<string> inputDirections;
    public List<string> outputDirections;

    public GameItem storage;

    public Bricks(Tile cTile, Vector3Int coords, List<string> inputDir, List<string> outputDir, Belt cBelt)
    {
        tile = cTile;
        cordinates = coords;
        inputDirections = inputDir;
        outputDirections = outputDir;
        belt = cBelt;
    }
}