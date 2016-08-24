using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    // Temporary Point gesture
    private int _pointsdEarned = 0;
    private const int _MaxPoints = 1000;
    public Image ValuePointsBar;
    public Text ValuePointsText;

    //Utility
    private float _timerReplay = -1;
    private float _timerReplayDelay = 0;

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

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        MapManager.instance.GenerateMap();
        UpdatePoints();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IsPlaying)
        {
            _timerPoint += Time.deltaTime;
            if (_timerPoint < 11)
            {
                ArrowTimer.transform.Translate(Vector3.down * _SpeedTimer * Time.deltaTime);
            }
        }

        if (_timerReplay >= 0)
        {
            _timerReplay += Time.deltaTime;
            if(_timerReplay > _timerReplayDelay)
            {
                _timerReplay = -1;
                Replay();
            }

        }
	}

    public void Fail()
    {
        if (IsPlaying)
        {
            _pointsdEarned += 5;
            Debug.Log("OOOOO Sorry, you lose, here take 5 stars ! Points :" + _pointsdEarned);
            UpdatePoints();
            ArrowTimer.transform.localPosition = _ArrowFailPosition;
            IsPlaying = false;
            StartDelayedReplay(3);
        }
    }

    public void Found()
    {
        if (IsPlaying)
        {
            int pointsMilestone = Mathf.CeilToInt(_timerPoint / 2f);
            switch (pointsMilestone)
            {
                case 1:
                    _pointsdEarned += 1000;
                    break;
                case 2:
                    _pointsdEarned += 500;
                    break;
                case 3:
                    _pointsdEarned += 250;
                    break;
                case 4:
                    _pointsdEarned += 100;
                    break;
                case 5:
                    _pointsdEarned += 50;
                    break;
                default:
                    _pointsdEarned += 10;
                    break;
            }
            Debug.Log("Congrats ! you earned " + _pointsdEarned);
            UpdatePoints();
            IsPlaying = false;
            StartDelayedReplay(3);
        }
    }

    public void StartDelayedReplay(float delay = 0)
    {
        _timerReplayDelay = delay;
        _timerReplay = 0;
    }

    public void Replay()
    {
        MapManager.instance.RebuildMap();
        ResetTimerPoints();
        IsPlaying = true;
    }

    public void ResetTimerPoints()
    {
        _timerPoint = 0;
        ArrowTimer.transform.localPosition = _ArrowStartPosition;
    }

    public void UpdatePoints()
    {
        ValuePointsText.text = _pointsdEarned + " / " + _MaxPoints;
        ValuePointsBar.fillAmount = (float)_pointsdEarned / (float)_MaxPoints;
    }
}
