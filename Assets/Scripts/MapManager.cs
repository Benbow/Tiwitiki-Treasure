﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    //MapConfig
    public const float ratioTiles = 1.28f;

    private GameObject[,] _gameMap;

    [SerializeField]
    MapConfig _mapConfig;

    public GameObject[,] GameMap
    {
        get
        {
            return _gameMap;
        }

        set
        {
            _gameMap = value;
        }
    }

    public GameObject tile(int x, int y)
    {
        return _gameMap[x, y];
    }


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
        //Map, GameObject who contain all the map
        GameObject map = new GameObject();
        map.transform.parent = transform;
        map.name = "Map";

        //Array that contains all the map tile
        GameMap = new GameObject[(int)_mapConfig.MapSize.y, (int)_mapConfig.MapSize.x];

        //choisir les tiles
        float tilesNumber = _mapConfig.MapSize.x * _mapConfig.MapSize.y;
        float emptyTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageEmpty / 100f));
        float smallTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageSmall / 100f));
        float mediumTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageMedium / 100f));
        float bigTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageBig / 100f));

        //Add empty tiles if percentage number is not reached
        emptyTilesNumber += tilesNumber - (emptyTilesNumber + smallTilesNumber + mediumTilesNumber + bigTilesNumber) ; 

        Debug.Log(tilesNumber + " / " + emptyTilesNumber + " / " + smallTilesNumber + " / " + mediumTilesNumber + " / " + bigTilesNumber);

        //generer la liste des tiles a poser
        List<GameObject> shuffleBagSprites = new List<GameObject>();
        List<GameObject> emptyTiles = _mapConfig.GetTilesType("TilesEmpty");
        List<GameObject> smallTiles = _mapConfig.GetTilesType("TilesSmall");
        List<GameObject> mediumTiles = _mapConfig.GetTilesType("TilesMedium");
        List<GameObject> bigTiles = _mapConfig.GetTilesType("TilesBig");
        List<GameObject> backgroundTiles = _mapConfig.GetTilesType("Background");

        //Avec Gestion du nombre de tiles differentes
        int tilesRemovedNumber = (smallTiles.Count + mediumTiles.Count + bigTiles.Count) - _mapConfig.NumberOfDifferentTiles;
        //n'accepte pas si le nombre de tiles retirer est superieur a la moitié du nombre de tiles differentes
        if (tilesRemovedNumber < 0 || tilesRemovedNumber > Mathf.FloorToInt((smallTiles.Count + mediumTiles.Count + bigTiles.Count)/2)) 
            tilesRemovedNumber = 0;
        List<int> removedTileId = new List<int>();
        for (int i = 0; i < tilesRemovedNumber; i++)
        {
            int randId = Random.Range(0, smallTiles.Count + mediumTiles.Count + bigTiles.Count);
            if (!removedTileId.Contains(randId))
                removedTileId.Add(randId);
            else
                i--;
        }

        foreach (int i in removedTileId)
        {
            Debug.Log("Removed id : " + i);
        }

        //remplissage du shufflebag en omettant les tiles deselectionner
        for (int i = 0; i < emptyTilesNumber; i++)
        {
            shuffleBagSprites.Add(emptyTiles[Random.Range(0, emptyTiles.Count)]);
        }
        for (int i = 0; i < smallTilesNumber; i++)
        {
            int tileId = Random.Range(0, smallTiles.Count);
            if (!removedTileId.Contains(tileId))
                shuffleBagSprites.Add(smallTiles[tileId]);
            else
                i--;
        }
        for (int i = 0; i < mediumTilesNumber; i++)
        {
            int rand = Random.Range(0, mediumTiles.Count);
            int tileId = rand + smallTiles.Count;
            if (!removedTileId.Contains(tileId))
                shuffleBagSprites.Add(mediumTiles[rand]);
            else
                i--;
        }
        for (int i = 0; i < bigTilesNumber; i++)
        {
            int rand = Random.Range(0, bigTiles.Count);
            int tileId = rand + smallTiles.Count + mediumTiles.Count;
            if (!removedTileId.Contains(tileId))
                shuffleBagSprites.Add(bigTiles[rand]);
            else
                i--;
        }

        Debug.Log(shuffleBagSprites.Count);

        //poser les tiles
        for (int i = 0; i < _mapConfig.MapSize.y; i++)
        {
            for (int j = 0; j < _mapConfig.MapSize.x; j++)
            {
                int rand = Random.Range(0, shuffleBagSprites.Count);
                GameObject newTiles = (GameObject)GameObject.Instantiate(shuffleBagSprites[rand]);
                newTiles.transform.position = new Vector3(j * ratioTiles, i * (-ratioTiles), -i);
                newTiles.GetComponent<TileSprite>().TileCoord = new Vector2(i, j);
                newTiles.name = i + "x" + j + " " + newTiles.GetComponent<TileSprite>().TileType;
                newTiles.transform.parent = map.transform;
                GameMap[i, j] = newTiles;
                shuffleBagSprites.RemoveAt(rand);
            }
        }

        //poser le background
        int randBg = Random.Range(0, backgroundTiles.Count);
        GameObject newBg = (GameObject)GameObject.Instantiate(backgroundTiles[randBg]);
        newBg.transform.position = new Vector3(_mapConfig.MapSize.x/2f + ratioTiles/2f, -_mapConfig.MapSize.y/2f - ratioTiles/2f, _mapConfig.MapSize.x);
        newBg.transform.localScale = new Vector3(11, 11, 1);
        newBg.name = "Background";
        newBg.transform.parent = map.transform;

        //verifier les carres vides et les supprimer
        for (int i = 1; i < _mapConfig.MapSize.y - 1; i++)
        {
            for (int j = 1; j < _mapConfig.MapSize.x - 1; j++)
            {
                if (GameMap[j - 1, i - 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j, i - 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j + 1, i - 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j - 1, i].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j, i].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j + 1, i].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j - 1, i + 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j, i + 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameMap[j + 1, i + 1].GetComponent<TileSprite>().TileType == Tiles.Empty)
                {
                    Debug.Log("TEST");
                    int randNumberModif = Random.Range(1, 4);
                    Debug.Log("Number modif = " + randNumberModif);
                    for (int number = 1; number <= randNumberModif; number++)
                    {
                        int randX = Random.Range(-1, 2);
                        int randY = Random.Range(-1, 2);
                        int randType = Random.Range(1, 4);

                        GameObject.Destroy(GameMap[j + randX, i + randY]);
                        GameObject newTiles = new GameObject();
                        switch (randType)
                        {
                            case 1:
                                newTiles = (GameObject)GameObject.Instantiate(smallTiles[Random.Range(0, smallTiles.Count)]);
                                newTiles.GetComponent<TileSprite>().TileType = Tiles.Small;
                                break;
                            case 2:
                                newTiles = (GameObject)GameObject.Instantiate(mediumTiles[Random.Range(0, mediumTiles.Count)]);
                                newTiles.GetComponent<TileSprite>().TileType = Tiles.Medium;
                                break;
                            case 3:
                                newTiles = (GameObject)GameObject.Instantiate(bigTiles[Random.Range(0, bigTiles.Count)]);
                                newTiles.GetComponent<TileSprite>().TileType = Tiles.Big;
                                break;
                        }
                        newTiles.transform.position = new Vector3((j + randX) * ratioTiles, (i + randY) * (-ratioTiles), -i);
                        newTiles.GetComponent<TileSprite>().TileCoord = new Vector2(i + randY, j + randX);
                        newTiles.name = i + "x" + j + " " + newTiles.GetComponent<TileSprite>().TileType;
                        newTiles.transform.parent = map.transform;
                        GameMap[i + randY, j + randX] = newTiles;
                        Debug.Log("X = " + (i + randY) + "| Y = " + (j + randX) + "| Type = " + newTiles.GetComponent<TileSprite>().TileType);
                    }
                }
            }
        }

        //temporary move Camera
        map.transform.position = new Vector3(-4, 6, _mapConfig.MapSize.y);
        map.transform.localScale = new Vector3(0.7f, 0.7f, 1);
    }

    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(40, 410, 100, 90), "Test menu");

        // Make the first button
        if (GUI.Button(new Rect(50, 440, 80, 20), "New Map"))
        {
            RebuildMap();
        }

        // Make the second button.
        if (GUI.Button(new Rect(50, 470, 80, 20), "New World"))
        {
            MapConfig[] mapConfs = Resources.FindObjectsOfTypeAll<MapConfig>();
            MapConfig newMapConf;
            do
            {
                newMapConf = mapConfs[Random.Range(0, mapConfs.Length)];
            } while (newMapConf.WorldName == _mapConfig.WorldName);

            _mapConfig = newMapConf;
            RebuildMap();
        }
    }

    void RebuildMap()
    {
        Destroy(this.transform.Find("Map").gameObject);
        GenerateMap();
    }
}
