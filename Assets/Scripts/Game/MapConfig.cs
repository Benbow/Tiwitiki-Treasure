using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName ="Scriptable/Maps/Config")]
public class MapConfig : ScriptableObject
{   
    #if UNITY_EDITOR
    [ReadOnly]
    #endif
    public Vector2 MapSize = new Vector2(10, 10);
    [Range(0, 25)]
    public int NumberOfDifferentTiles;
    public string WorldName;
    public GameObject TilesPrefab;
    public GameObject TileBg;
    public GameObject TreasureChest;
    [Range(0, 100)]
    public int percentageEmpty;
    [Range(0, 100)]
    public int percentageSmall;
    [Range(0, 100)]
    public int percentageMedium;
    [Range(0, 100)]
    public int percentageBig;

    public bool CentralTreasureLocation = true;

    public List<GameObject> GetTilesType(string type)
    {
        List<GameObject> returnList = new List<GameObject>();
        Transform empty = TilesPrefab.transform.Find(type);
        foreach (Transform child in empty)
        {
            returnList.Add(child.gameObject);
        }

        return returnList;
    }
}