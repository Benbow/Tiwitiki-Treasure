using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    //MapConfig
    public const float ratioTiles = 1.28f;

    [SerializeField]
    MapConfig _mapConfig;


	// Use this for initialization
	void Start ()
    {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GenerateMap()
    {
        //choisir les tiles
        float tilesNumber = _mapConfig.MapSize.x * _mapConfig.MapSize.y;
        float emptyTilesNumber = Mathf.Floor(tilesNumber * ((float)_mapConfig.percentageEmpty / 100f));
        float smallTilesNumber = Mathf.Floor(tilesNumber * ((float)_mapConfig.percentageSmall / 100f));
        float mediumTilesNumber = Mathf.Floor(tilesNumber * ((float)_mapConfig.percentageMedium / 100f));
        float bigTilesNumber = Mathf.Floor(tilesNumber * ((float)_mapConfig.percentageBig / 100f));

        emptyTilesNumber += (emptyTilesNumber + smallTilesNumber + mediumTilesNumber + bigTilesNumber) - tilesNumber;

        Debug.Log(tilesNumber + " / " + emptyTilesNumber + " / " + smallTilesNumber + " / " + mediumTilesNumber + " / " + bigTilesNumber);

        //generer la liste des tiles a poser
        List<GameObject> shuffleBagSprites = new List<GameObject>();
        List<GameObject> emptyTiles = _mapConfig.GetTilesType("TilesEmpty");
        List<GameObject> smallTiles = _mapConfig.GetTilesType("TilesSmall");
        List<GameObject> mediumTiles = _mapConfig.GetTilesType("TilesMedium");
        List<GameObject> bigTiles = _mapConfig.GetTilesType("TilesBig");

        for (int i = 0; i < emptyTilesNumber; i++)
        {
            shuffleBagSprites.Add(emptyTiles[Random.Range(0, emptyTiles.Count)]);
        }
        for (int i = 0; i < smallTilesNumber; i++)
        {
            shuffleBagSprites.Add(smallTiles[Random.Range(0, smallTiles.Count)]);
        }
        for (int i = 0; i < mediumTilesNumber; i++)
        {
            shuffleBagSprites.Add(mediumTiles[Random.Range(0, mediumTiles.Count)]);
        }
        for (int i = 0; i < bigTilesNumber; i++)
        {
            shuffleBagSprites.Add(bigTiles[Random.Range(0, bigTiles.Count)]);
        }

        Debug.Log(shuffleBagSprites.Count);

        //poser les tiles
        for (int i = 0; i < _mapConfig.MapSize.y; i++)
        {
            for (int j = 0; j < _mapConfig.MapSize.x; j++)
            {
                int rand = Random.Range(0, shuffleBagSprites.Count);
                GameObject newTiles = (GameObject)GameObject.Instantiate(shuffleBagSprites[rand]);
                newTiles.transform.position = new Vector3(i * ratioTiles, j * ratioTiles, -j);
                shuffleBagSprites.RemoveAt(rand);
            }
        }

        //verifier les carres vides et les supprimer

    }
}
