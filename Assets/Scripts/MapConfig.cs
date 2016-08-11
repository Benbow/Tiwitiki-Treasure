using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName ="Scriptable/Maps/Config")]
public class MapConfig : ScriptableObject
{
    public Vector2 MapSize;
    public int NumberOfDifferentTiles;
    public string WorldName;
    public GameObject TilesPrefab;

    public int percentageEmpty;
    public int percentageSmall;
    public int percentageMedium;
    public int percentageBig;

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