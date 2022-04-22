using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GameSence", menuName = "Game/Bricks")]
public class Bricks : ScriptableObject
{
    public Belt belt;
    public Tile tile;
    public Vector3Int cordinates;
    public List<char> directions;
    public List<char> inputDirections;
    public List<char> outputDirections;

    public GameItem storage;

    public void Creation(Tile cTile, Vector3Int coords, List<char> inputDir, List<char> outputDir)
    {
        Bricks newBrick = new Bricks();
        newBrick.tile = cTile;
        newBrick.cordinates = coords;
    }
}