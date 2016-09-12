using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

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
    private bool _isPlaying = false;
    private bool _isPaused = false;
    public Image ValuePointsBar;
    public Text ValuePointsText;
    public GameObject TextPoints;
    public GameObject TextAttempts;
    public GameObject NewIllusPopup;

    // Temporary Point gesture
    private int _pointsdEarned = 0;
    double levelStarAmount;

    //Utility
    private float _timerReplay = -1;
    private float _timerReplayDelay = 0;
    private GameObject _treasureChest;
    private bool _reachNewLevel;

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
    void Start() {
        DOTween.Init();

        MyPlayerConfig = GlobalManager.instance.PlayerConf;

        // star bar points
        UpdateAttempts();
        UpdatePointsReward(); // points jauge rewards initilization
        MapManager.instance.GenerateMap();
    }

    // Update is called once per frame
    void Update()
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

    //Click tile function
    public void OnTileCliked(TileSprite tile)
    {
        if (!IsPaused)
        {
            if (IsPlaying)
            {
                IsPlaying = false;
                MyPlayerConfig.SetActualAttemps(MyPlayerConfig.GetActualAttempts() - 1);
                UpdateAttempts();

                if (SoluceMapPos == tile.TileCoord)
                {
                    Found(tile.gameObject);
                }
                else
                {
                    Fail(tile.gameObject);
                }

                //Update the star Bar
                UpdatePointsUi();

                if(!_reachNewLevel)
                    MapManager.instance.OutroMapAnimation();

                if (!_reachNewLevel && MyPlayerConfig.GetActualAttempts() > 0)
                    StartDelayedReplay(3);
                else if (!_reachNewLevel && MyPlayerConfig.GetActualAttempts() <= 0)
                {
                    SceneManager.LoadScene(0);
                    //Lose Attempts gesture
                }
            }
        }
    }


    //Called if the tile clicked is the wrong one
    public void Fail(GameObject tile)
    {
        //Get the points win
        _pointsdEarned = MyGameConfig.JaugePoints[MyGameConfig.JaugePoints.Count - 1];
        MyPlayerConfig.SetActualScore((int)MyPlayerConfig.GetActualScore() + _pointsdEarned);

        //change le sprite de la case
        tile.GetComponent<SpriteRenderer>().sprite = MapManager.instance.LoseSprite;

        //Animation Chest appear
        ChestAppear(tile, MyGameConfig.JaugePoints.Count, false);


        ArrowTimer.transform.localPosition = _ArrowFailPosition;
    }

    //Called when the tile cliked is the good one
    public void Found(GameObject tile)
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
        //GamesReady();
    }

    //Function to reset the jauge bar points timer
    public void ResetTimerPoints()
    {
        _timerPoint = 0;
        ArrowTimer.transform.DOLocalMove(_ArrowStartPosition, 0.5f).SetEase(Ease.OutCubic);
    }

    //Update the star bar points
    public void UpdatePointsUi()
    {
        levelStarAmount = LevelStarFormula();

        // if scores is bigger, manage level changing
        if (MyPlayerConfig.GetActualScore() >= levelStarAmount)
        {
            _reachNewLevel = true;
            ValuePointsText.text = (int)levelStarAmount + " / " + (int)levelStarAmount;
            ValuePointsBar.DOFillAmount(1, Constant._TimerFillStarBar).OnComplete(ProcedeNextLevel);
        }
        else // if not scores is bigger, just update the star bar
        {
            ValuePointsText.text = (int)MyPlayerConfig.GetActualScore() + " / " + (int)levelStarAmount;
            ValuePointsBar.DOFillAmount(MyPlayerConfig.GetActualScore() / (float)levelStarAmount, 0.5f);
        }
    }


    //Manage Attempts text and save
    public void UpdateAttempts()
    {
        int actualAttempts = MyPlayerConfig.GetActualAttempts();
        int maxAttempts = MyPlayerConfig.GetMaxAddAttempts() + MapManager.instance.MapConfigs.maxAttemptsBase;

        TextAttempts.GetComponent<Text>().text = actualAttempts + " / " + maxAttempts;
    } 

    //After the animation of the complete star bar, change Level
    public void ProcedeNextLevel()
    {
        
        NewCollectPopup();


        /*int reste = (int)MyPlayerConfig.GetActualScore() - (int)levelStarAmount;
        MyPlayerConfig.SetActualScore(0);
        ValuePointsBar.fillAmount = 0;
        MyPlayerConfig.SetActualLevel((int)MyPlayerConfig.GetActualLevel() + 1, reste);*/

        //UpdatePointsUi();

        //TODO ANIMATION NEW COLLECTIONS
        
        //StartDelayedReplay(3);
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
        _treasureChest.transform.SetParent(MapManager.instance.MapParent.transform);
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

    //Choose a new illus and appear popup !
    public void NewCollectPopup()
    {
        NewIllusPopup.GetComponent<BoxCollider>().enabled = true;
        Image bg = NewIllusPopup.GetComponent<Image>();
        SpriteRenderer cardBack = NewIllusPopup.GetComponentsInChildren<SpriteRenderer>()[0];
        cardBack.sprite = cardBack.GetComponent<NewCollecItem>().SpriteBack;
        cardBack.GetComponent<NewCollecItem>().Illus.DOFade(0, 0);


        Sequence collectPopup = DOTween.Sequence();
        collectPopup.Append(bg.DOFade(0.55f,0.3f).SetEase(Ease.OutCirc));
        collectPopup.AppendInterval(0.3f);
        collectPopup.Append(cardBack.transform.DOLocalMoveY(3f, 1.5f));
        collectPopup.Insert(0.65f, cardBack.transform.DOLocalRotate(new Vector3(0,3600, 0), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutQuint));

        collectPopup.Play();
    }

    //launch the game again after removing the popup
    public void ContinueAftertPopupAnim()
    {
        // Remove Popup
        NewIllusPopup.GetComponent<BoxCollider>().enabled = false;
        Image bg = NewIllusPopup.GetComponent<Image>();
        SpriteRenderer cardBack = NewIllusPopup.GetComponentsInChildren<SpriteRenderer>()[0];

        Sequence collectPopup = DOTween.Sequence();
        collectPopup.Append(cardBack.transform.DOLocalMoveY(-24f, 0.5f).SetEase(Ease.InCubic));
        collectPopup.AppendInterval(0.3f);
        collectPopup.Append(bg.DOFade(0, 0.3f).SetEase(Ease.InCirc)).OnComplete(() => AfterPopupReady(true));
        //collectPopup.AppendCallback(AfterPopupReady);

        collectPopup.Play();
    }

    public void AfterPopupReady(bool replay = true)
    {
        //Modify playing variable
        _reachNewLevel = false;
        int reste = (int)MyPlayerConfig.GetActualScore() - (int)levelStarAmount;
        MyPlayerConfig.SetActualScore(0);
        ValuePointsBar.fillAmount = 0;
        MyPlayerConfig.SetActualLevel((int)MyPlayerConfig.GetActualLevel() + 1, reste);
        if (replay) {
            UpdatePointsUi();
            MapManager.instance.OutroMapAnimation();
            StartDelayedReplay(3);
        }
    }

    public void GamesReady()
    {
        ResetTimerPoints();
        IsPlaying = true;
    }
}
