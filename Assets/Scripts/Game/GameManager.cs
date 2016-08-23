using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private GameObject[,] _gameMap;
    private List<GameObject> _gameSoluce = new List<GameObject>();

    private Vector2 _soluceMapPos = new Vector2();

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

    public List<GameObject> GameSoluce
    {
        get
        {
            return _gameSoluce;
        }

        set
        {
            _gameSoluce = value;
        }
    }

    public Vector2 SoluceMapPos
    {
        get
        {
            return _soluceMapPos;
        }

        set
        {
            _soluceMapPos = value;
        }
    }

    void Awake()
    {
        instance = this;
    }

        // Use this for initialization
    void Start () {
        MapManager.instance.GenerateMap();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
