using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;

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
    //Config
    [SerializeField]
    private GameConfig _gameConfig;
    [SerializeField]
    private PlayerConfig _playerConfig;

    // Relative to Map
    private GameObject[,] _gameMap;
    private List<GameObject> _gameSoluce = new List<GameObject>();

    private Vector2 _soluceMapPos = new Vector2();

    // Timer Points gesture
    private const float _DistanceTimer = 2.75f;
    private const float _SpeedTimer = 0.180f;
    private Vector3 _ArrowStartPosition = new Vector3(0.9f, 1.75f, -1);
   // private Vector3 _ArrowStopPosition = new Vector3(0.9f, -1.3f, -1);
    private Vector3 _ArrowFailPosition = new Vector3(0.9f, -1.8f, -1);
    public GameObject ArrowTimer;
    private float _timerPoint = 0;
    private bool _isPlaying = true;
    private bool _isPaused = false;
    public Image ValuePointsBar;
    public Text ValuePointsText;
    public GameObject TextPoints;

    // Temporary Point gesture
    private int _pointsdEarned = 0;
    double levelStarAmount;

    //Utility
    private float _timerReplay = -1;
    private float _timerReplayDelay = 0;
    private GameObject _treasureChest;

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

    public bool IsPlaying
    {
        get
        {
            return _isPlaying;
        }

        set
        {
            _isPlaying = value;
        }
    }

    public GameConfig MyGameConfig
    {
        get
        {
            return _gameConfig;
        }

        set
        {
            _gameConfig = value;
        }
    }

    public PlayerConfig MyPlayerConfig
    {
        get
        {
            return _playerConfig;
        }

        set
        {
            _playerConfig = value;
        }
    }

    public bool IsPaused
    {
        get
        {
            return _isPaused;
        }

        set
        {
            _isPaused = value;
        }
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        DOTween.Init();

        MapManager.instance.GenerateMap();
        UpdatePointsUi(true);
        UpdatePointsReward();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsPaused)
        {
            //Move the arrow points
            if (IsPlaying)
            {
                _timerPoint += Time.deltaTime;
                if (_timerPoint < 11)
                {
                    ArrowTimer.transform.Translate(Vector3.down * _SpeedTimer * Time.deltaTime);
                }
            }


            //Only for delayed Replay function
            if (_timerReplay >= 0)
            {
                _timerReplay += Time.deltaTime;
                if (_timerReplay > _timerReplayDelay)
                {
                    _timerReplay = -1;
                    Replay();
                }
            }
        }
	}

    //Called if the tile clicked is the wrong one
    public void Fail(GameObject tile)
    {
        if (!IsPaused)
        {
            if (IsPlaying)
            {
                //Get the points win
                _pointsdEarned = MyGameConfig.JaugePoints[MyGameConfig.JaugePoints.Count - 1];
                MyPlayerConfig.SetActualScore((int)MyPlayerConfig.GetActualScore() + _pointsdEarned);

                //change le sprite de la case
                tile.GetComponent<SpriteRenderer>().sprite = MapManager.instance.LoseSprite;

                //Animation Chest appear
                ChestAppear(tile, MyGameConfig.JaugePoints.Count, false);


                ArrowTimer.transform.localPosition = _ArrowFailPosition;

                //Update the star Bar
                UpdatePointsUi();
            }
        }
    }

    //Called when the tile cliked is the good one
    public void Found(GameObject tile)
    {
        if (!IsPaused)
        {
            if (IsPlaying)
            {
                int pointsMilestone = Mathf.CeilToInt(_timerPoint / 2f);
                if (pointsMilestone > 6)
                    pointsMilestone = 6;

                //Get the points win
                _pointsdEarned = MyGameConfig.JaugePoints[pointsMilestone - 1];
                MyPlayerConfig.SetActualScore((int)MyPlayerConfig.GetActualScore() + _pointsdEarned);
                // Change le sprite de la tile
                tile.GetComponent<SpriteRenderer>().sprite = MapManager.instance.WinSprite;
                tile.GetComponent<SpriteRenderer>().color = MapManager.instance.SpriteColors[pointsMilestone - 1];

                //Animation Chest appear
                ChestAppear(tile, pointsMilestone, true);

                //Update the star Bar
                UpdatePointsUi();
            }
        }
    }

    //Function to restart a new map, with a potential delay
    public void StartDelayedReplay(float delay = 0)
    {
        IsPlaying = false;
        _timerReplayDelay = delay;
        _timerReplay = 0;
    }

    //Actual replay function called after the delay
    public void Replay()
    {
        Destroy(_treasureChest);
        MapManager.instance.RebuildMap();
        ResetTimerPoints();
        IsPlaying = true;
    }

    //Function to reset the jauge bar points timer
    public void ResetTimerPoints()
    {
        _timerPoint = 0;
        ArrowTimer.transform.localPosition = _ArrowStartPosition;
    }

    //Update the star bar points
    public void UpdatePointsUi(bool startCall = false)
    {   
        levelStarAmount = LevelStarFormula();

        // if scores is bigger, manage level changing
        if (MyPlayerConfig.GetActualScore() >= levelStarAmount)
        {
            ValuePointsText.text = (int)levelStarAmount + " / " + (int)levelStarAmount;
            ValuePointsBar.DOFillAmount(1, 0.5f).OnComplete(ProcedeNextLevel);
        }
        else // if not scores is bigger, just update the star bar
        {
            ValuePointsText.text = (int)MyPlayerConfig.GetActualScore() + " / " + (int)levelStarAmount;
            ValuePointsBar.DOFillAmount(MyPlayerConfig.GetActualScore() / (float)levelStarAmount, 0.5f);

            //Next map
            if (!startCall)
            {
                StartDelayedReplay(3);
            }
        }
    }

    //After the animation of the complete star bar, change Level
    public void ProcedeNextLevel()
    {
        int reste = (int)MyPlayerConfig.GetActualScore() - (int)levelStarAmount;
        MyPlayerConfig.SetActualScore(0);
        ValuePointsBar.fillAmount = 0;
        MyPlayerConfig.SetActualLevel((int)MyPlayerConfig.GetActualLevel() + 1, reste);

        UpdatePointsUi();

        //TODO ANIMATION NEW COLLECTIONS
        Debug.Log("WOUHOUUUU A NEW CARD !!");
        
        StartDelayedReplay(3);
    }

    //Update the text in the jauge
    public void UpdatePointsReward()
    {
        Text[] texts = TextPoints.GetComponentsInChildren<Text>();
        for (int i = 0; i < MyGameConfig.JaugePoints.Count; i++)
        {
            texts[i].text = MyGameConfig.JaugePoints[i].ToString();
        }
    }

    //Made the appearance of the chest
    private void ChestAppear(GameObject tile, int pointsMilestone, bool isChestVisible)
    {
        //Apparait le coffre
        _treasureChest = (GameObject)GameObject.Instantiate(MapManager.instance.MapConfigs.TreasureChest);
        _treasureChest.transform.parent = MapManager.instance.MapParent.transform;
        _treasureChest.transform.localPosition = tile.transform.localPosition;
        if (!isChestVisible)
        {
            _treasureChest.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
        //Change le Texte
        _treasureChest.GetComponentInChildren<TextMesh>().text = "+ " + _pointsdEarned;
        _treasureChest.GetComponentInChildren<TextMesh>().color = MapManager.instance.SpriteColors[pointsMilestone - 1];
    }

    // get with formula the initial amount of stars required for this world
    private double WorldInitialStarFormula()
    {
        double worldInitial = _gameConfig.InitialValue;
        if (MyPlayerConfig.ActualWorld > 1)
            worldInitial = _gameConfig.InitialValue * (Math.Pow(1 + _gameConfig.RatioWorld, MyPlayerConfig.ActualWorld - 1));
        return worldInitial;
    }

    // get with formula the amount of stars required for this level
    private double LevelStarFormula()
    {
        double levelInit = WorldInitialStarFormula();
        if (MyPlayerConfig.GetActualLevel() > 1)
            levelInit = WorldInitialStarFormula() * (Math.Pow(1 + _gameConfig.RatioLevel, MyPlayerConfig.GetActualLevel() - 1));
        else if (MyPlayerConfig.GetActualLevel() < 0)
            Debug.LogError("Level not found " + levelInit);
        return levelInit;
    }
}
