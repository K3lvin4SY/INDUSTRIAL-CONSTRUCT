using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{

    public Tilemap smallMap;
    public Tilemap bigMap;
    public static Map Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        for (int x = -15; x < 15; x++)
        {
            for (int y = -15; y < 15; y++)
            {
                Map.updateColumn(new Vector3Int(x, y, 0));
            }
        }
    }

    public static void updateColumn(Vector3Int cord) {
        Tilemap bigMap = Map.Instance.bigMap;
        Tilemap smallMap = Map.Instance.smallMap;
        int tileZ = 0;
        for (int i = 0; i < 20; i++)
        {
            Vector3Int generatedCord = new Vector3Int(cord.x, cord.y, i);
            TileBase tile = bigMap.GetTile(generatedCord);
            smallMap.SetTile(generatedCord, null);
            if (tile != null)
            {
                // place tile here
                string flatTileName = "";
                string tileName = tile.name.ToLower();
                string dir = tileName[0].ToString().ToUpper();

                if (tileName.Contains("grass"))
                {
                    flatTileName = "Grass"+((Mathf.Abs(cord.x*cord.y) % 3)+1).ToString();
                } else if (tileName.Contains("conveyor")) {
                    if (tileName.Contains("straight"))
                    {
                        flatTileName = dir+"_Straight";
                    } else if (tileName.Contains("slant"))
                    {
                        if (dir == "S" || dir == "W")
                        {
                            dir = GlobalMethods.oppositeDir(dir);
                        }
                        flatTileName = dir+"_Straight";
                    } else if (tileName.Contains("bend"))
                    {
                        flatTileName = dir+"_Bend";
                    } else if (tileName.Contains("eli"))
                    {
                        flatTileName = dir+"_Eli";
                    }

                    flatTileName += "_Conveyor";
                } else if (tileName.Contains("sand")) {
                    flatTileName = "Sand";
                } else if (tileName.Contains("dirt")) {
                    flatTileName = "Dirt";
                } else if (tileName.Contains("merger")) {
                    flatTileName = dir+"_Merger";
                } else if (tileName.Contains("splitter")) {
                    flatTileName = dir+"_Splitter";
                } else if (tileName.Contains("miner")) {
                    flatTileName = dir+"_Miner";
                } else if (tileName.Contains("smelter")) {
                    flatTileName = dir+"_Smelter";
                }

                smallMap.SetTile(new Vector3Int(cord.x, cord.y, tileZ), GlobalMethods.GetFlatTileByName(flatTileName));
                tileZ++;
            }
        }
    }
}
