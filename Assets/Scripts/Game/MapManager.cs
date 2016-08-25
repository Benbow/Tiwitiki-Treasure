using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    public static MapManager instance;

    private MapManager() { }

    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapManager();
            }
            return instance;
        }
    }

    public MapConfig MapConfigs
    {
        get
        {
            return _mapConfig;
        }

        set
        {
            _mapConfig = value;
        }
    }

    public GameObject MapParent
    {
        get
        {
            return _mapParent;
        }

        set
        {
            _mapParent = value;
        }
    }

    //MapConfig
    private GameObject _mapParent;
    public const float ratioTiles = 1.28f;
    #if UNITY_EDITOR
    [ReadOnly]
    #endif
    public Vector2 solucePosRatio = new Vector2(0, 5);
    public GameObject EmptyTile;
    public GameObject TargetSoluce;
    public List<Color> SpriteColors = new List<Color>();
    public Sprite WinSprite;
    public Sprite LoseSprite;
    public GameObject Star;
    

    //soluce config
    private GameObject _mapBg;
    

    [SerializeField]
    MapConfig _mapConfig;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateMap()
    {
        //Map, GameObject who contain all the map
       MapParent = new GameObject();
        MapParent.transform.parent = transform;
        MapParent.name = "Map";

        //Array that contains all the map tile
        GameManager.instance.GameMap = new GameObject[(int)_mapConfig.MapSize.y, (int)_mapConfig.MapSize.x];

        //choisir les tiles
        float tilesNumber = _mapConfig.MapSize.x * _mapConfig.MapSize.y;
        float emptyTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageEmpty / 100f));
        float smallTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageSmall / 100f));
        float mediumTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageMedium / 100f));
        float bigTilesNumber = Mathf.Floor(tilesNumber * (_mapConfig.percentageBig / 100f));

        //Add empty tiles if percentage number is not reached
        emptyTilesNumber += tilesNumber - (emptyTilesNumber + smallTilesNumber + mediumTilesNumber + bigTilesNumber) ; 

        //generer la liste des tiles a poser
        List<GameObject> shuffleBagSprites = new List<GameObject>();
        List<GameObject> smallTiles = _mapConfig.GetTilesType("TilesSmall");
        List<GameObject> mediumTiles = _mapConfig.GetTilesType("TilesMedium");
        List<GameObject> bigTiles = _mapConfig.GetTilesType("TilesBig");

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

        //remplissage du shufflebag en omettant les tiles deselectionner
        for (int i = 0; i < emptyTilesNumber; i++)
        {
            shuffleBagSprites.Add(EmptyTile);
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
                newTiles.transform.parent = MapParent.transform;
                GameManager.instance.GameMap[i, j] = newTiles;
                shuffleBagSprites.RemoveAt(rand);
            }
        }

        //poser le background
        GameObject newBg = (GameObject)GameObject.Instantiate(_mapConfig.TileBg);
        _mapBg = newBg;
        newBg.transform.position = new Vector3(_mapConfig.MapSize.x/2f + ratioTiles/2f, -_mapConfig.MapSize.y/2f - ratioTiles/2f, _mapConfig.MapSize.x);
        newBg.transform.localScale = new Vector3(11, 11, 1);
        newBg.name = "Background";
        newBg.transform.parent = MapParent.transform;


        //verifier les carres vides et les supprimer
        for (int i = 1; i < _mapConfig.MapSize.y - 1; i++)
        {
            for (int j = 1; j < _mapConfig.MapSize.x - 1; j++)
            {
                if (GameManager.instance.GameMap[j - 1, i - 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j, i - 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j + 1, i - 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j - 1, i].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j, i].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j + 1, i].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j - 1, i + 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j, i + 1].GetComponent<TileSprite>().TileType == Tiles.Empty &&
                    GameManager.instance.GameMap[j + 1, i + 1].GetComponent<TileSprite>().TileType == Tiles.Empty)
                {
                    Debug.Log("TEST");
                    int randNumberModif = Random.Range(1, 4);
                    Debug.Log("Number modif = " + randNumberModif);
                    for (int number = 1; number <= randNumberModif; number++)
                    {
                        int randX = Random.Range(-1, 2);
                        int randY = Random.Range(-1, 2);
                        int randType = Random.Range(1, 4);

                        GameObject.Destroy(GameManager.instance.GameMap[j + randX, i + randY]);
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
                        newTiles.transform.parent = MapParent.transform;
                        GameManager.instance.GameMap[i + randY, j + randX] = newTiles;
                        Debug.Log("X = " + (i + randY) + "| Y = " + (j + randX) + "| Type = " + newTiles.GetComponent<TileSprite>().TileType);
                    }
                }
            }
        }

        //temporary move Camera
        MapParent.transform.position = new Vector3(-4, 5.3f, _mapConfig.MapSize.y);
        MapParent.transform.localScale = new Vector3(0.7f, 0.7f, 1);

        GenerateSoluce();
        
    }

    public void GenerateSoluce()
    {
        GameObject soluceObject = new GameObject();
        soluceObject.transform.parent = transform;
        soluceObject.name = "Solution";

        //create game Soluce
        int randTargetX = Random.Range(1, (int)_mapConfig.MapSize.x - 1);
        int randTargetY = Random.Range(1, (int)_mapConfig.MapSize.y - 1);
        GameManager.instance.SoluceMapPos = new Vector2(randTargetY, randTargetX);

        //create Background
        GameObject soluceBg = (GameObject)GameObject.Instantiate(_mapBg);
        soluceBg.transform.parent = soluceObject.transform;
        soluceBg.name = "SoluceBg";
        soluceBg.transform.localScale = new Vector3(3, 3, 1);
        soluceBg.transform.localPosition = new Vector3(solucePosRatio.x, -solucePosRatio.y-0.1f, 1.5f);

        //Select soluce Tiles
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                GameObject soluceTile = (GameObject)GameObject.Instantiate(GameManager.instance.GameMap[randTargetY + i, randTargetX + j]);
                soluceTile.transform.parent = soluceObject.transform;
                soluceTile.transform.localPosition = new Vector3(solucePosRatio.x + (j * ratioTiles), -solucePosRatio.y - (i * ratioTiles), 1-i/10f);
                Destroy(soluceTile.GetComponent<BoxCollider2D>());
                GameManager.instance.GameSoluce.Add(soluceTile);
            }
        }

        int randMoveX = 0;
        int randMoveY = 0;
        if (!_mapConfig.CentralTreasureLocation)
        {
            randMoveX = Random.Range(-1, 2);
            randTargetX = randTargetX + randMoveX;
            randMoveY = Random.Range(-1, 2);
            randTargetY = randTargetY + randMoveY;
            GameManager.instance.SoluceMapPos = new Vector2(randTargetY, randTargetX);
        }

        GameObject soluceTarget = (GameObject)GameObject.Instantiate(TargetSoluce);
        soluceTarget.name = "Treasure location";
        soluceTarget.transform.localPosition = new Vector3(solucePosRatio.x + (randMoveX * ratioTiles), -solucePosRatio.y - 0.1f - (randMoveY * ratioTiles), 1.3f);
        soluceTarget.transform.parent = soluceObject.transform;


        soluceObject.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        soluceObject.transform.localPosition = new Vector3(0, -2f, 0);

    }

    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(40, 430, 100, 90), "Test menu");

        // Make the first button
        if (GUI.Button(new Rect(50, 460, 80, 20), "New Map"))
        {
            RebuildMap();
        }

        // Make the second button.
        if (GUI.Button(new Rect(50, 490, 80, 20), "New World"))
        {
            Object[] mapConfs = Resources.LoadAll("Scriptable/Map/Conf");
            MapConfig newMapConf;
            if (mapConfs.Length > 1)
            {
                do
                {
                    newMapConf = (MapConfig)mapConfs[Random.Range(0, mapConfs.Length)];
                } while (newMapConf.WorldName == _mapConfig.WorldName);
                _mapConfig = newMapConf;
            }
            RebuildMap();
        }
    }

    public void RebuildMap()
    {
        Destroy(this.transform.Find("Map").gameObject);
        Destroy(this.transform.Find("Solution").gameObject);
        GenerateMap();
    }
}
