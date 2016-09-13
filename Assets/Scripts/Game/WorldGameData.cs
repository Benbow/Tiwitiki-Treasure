using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldGameData {
    [SerializeField]
    private int _worldId;
    [SerializeField]
    private string _worldName;
    [SerializeField]
    private bool _isUnlocked = false;
    [SerializeField]
    private int _starBarPoints = 0;
    [SerializeField]
    private int _actualLevel = 1;
    [SerializeField]
    private int _attemptsMaxAdder = 0;
    [SerializeField]
    private int _attemptsActual = 0;
    [SerializeField]
    private int _layersCount = 1;
    [SerializeField]
    private MapConfig _mapConfig;
    [SerializeField]
    private List<CollectionItemData> _collections = new List<CollectionItemData>();

    public int WorldId
    {
        get
        {
            return _worldId;
        }

        set
        {
            _worldId = value;
        }
    }

    public int StarBarPoints
    {
        get
        {
            return _starBarPoints;
        }

        set
        {
            _starBarPoints = value;
        }
    }

    public int ActualLevel
    {
        get
        {
            return _actualLevel;
        }

        set
        {
            _actualLevel = value;
        }
    }

    public int AttemptsMaxAdder
    {
        get
        {
            return _attemptsMaxAdder;
        }

        set
        {
            _attemptsMaxAdder = value;
        }
    }

    public int AttemptsActual
    {
        get
        {
            return _attemptsActual;
        }

        set
        {
            _attemptsActual = value;
        }
    }

    public List<CollectionItemData> Collections
    {
        get
        {
            return _collections;
        }

        set
        {
            _collections = value;
        }
    }

    public bool IsUnlocked
    {
        get
        {
            return _isUnlocked;
        }

        set
        {
            _isUnlocked = value;
        }
    }

    public string WorldName
    {
        get
        {
            return _worldName;
        }

        set
        {
            _worldName = value;
        }
    }

    public MapConfig MapConfig
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

    public int LayersCount
    {
        get
        {
            return _layersCount;
        }

        set
        {
            _layersCount = value;
        }
    }

    public float GetPercentageCollection()
    {
        float count = 0;
        foreach (CollectionItemData item in Collections)
        {
            if (item.Owned)
                count++;
        }
        if(Collections.Count != 0)
            return count / (float)Collections.Count;

        return 0;
    }
}
