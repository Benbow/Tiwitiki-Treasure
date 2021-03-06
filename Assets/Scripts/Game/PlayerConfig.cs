﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable/Player/Config")]
public class PlayerConfig : ScriptableObject {
    [SerializeField]
    private int _actualWorld;
    [SerializeField]
    private bool _tuto;
    [SerializeField]
    private List<WorldGameData> _worldsList;

    public int ActualWorld
    {
        get
        {
            return _actualWorld;
        }

        set
        {
            _actualWorld = value;
        }
    }

    public List<WorldGameData> WorldsList
    {
        get
        {
            return _worldsList;
        }

        set
        {
            _worldsList = value;
        }
    }

    public bool Tuto
    {
        get
        {
            return _tuto;
        }

        set
        {
            _tuto = value;
        }
    }

    public WorldGameData GetActualWorldData()
    {
        foreach (WorldGameData worldData in _worldsList)
        {
            if (worldData.WorldId == ActualWorld)
                return worldData;
        }

        return null;
    }

    public WorldGameData GetWorldDataById(int worldId)
    {
        foreach (WorldGameData worldData in _worldsList)
        {
            if (worldData.WorldId == worldId)
                return worldData;
        }

        return null;
    }


    public float GetActualLevel()
    {
        foreach (WorldGameData worldData in _worldsList)
        {
            if (worldData.WorldId == ActualWorld)
                return worldData.ActualLevel;
        }

        return -1;
    }

    public void SetActualLevel(int level, int points = 0)
    {
        for(int i = 0; i < WorldsList.Count; i++)
        {
            if (WorldsList[i].WorldId == ActualWorld)
            {
                WorldsList[i].ActualLevel = level;
                WorldsList[i].StarBarPoints = points;
            }
        }
    }

    public float GetActualScore()
    {
        foreach (WorldGameData worldData in _worldsList)
        {
            if (worldData.WorldId == ActualWorld)
                return worldData.StarBarPoints;
        }

        return -1;
    }

    public void SetActualScore(int points)
    {
        for (int i = 0; i < WorldsList.Count; i++)
        {
            if (WorldsList[i].WorldId == ActualWorld)
                WorldsList[i].StarBarPoints = points;
        }
    }

    public void SetActualAttemps(int actual)
    {
        for (int i = 0; i < WorldsList.Count; i++)
        {
            if (WorldsList[i].WorldId == ActualWorld)
                WorldsList[i].AttemptsActual = actual;
        }
    }

    public int GetActualAttempts()
    {
        foreach (WorldGameData worldData in _worldsList)
        {
            if (worldData.WorldId == ActualWorld)
                return worldData.AttemptsActual;
        }

        return -1;
    }

    public void SetMaxAddAttemps(int addMax)
    {
        for (int i = 0; i < WorldsList.Count; i++)
        {
            if (WorldsList[i].WorldId == ActualWorld)
                WorldsList[i].AttemptsMaxAdder = addMax;
        }
    }

    public int GetMaxAddAttempts()
    {
        foreach (WorldGameData worldData in _worldsList)
        {
            if (worldData.WorldId == ActualWorld)
                return worldData.AttemptsMaxAdder;
        }

        return -1;
    }




    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetSave()
    {
        ActualWorld = 1;
        Tuto = true;
        foreach (WorldGameData worldData in WorldsList)
        {
            worldData.ActualLevel = 1;
            worldData.AttemptsActual = 10;
            worldData.StarBarPoints = 0;
            worldData.AttemptsMaxAdder = 0;

            worldData.IsUnlocked = (worldData.WorldId == 1) ? true : false;

            foreach (CollectionItemData itemData in worldData.Collections)
            {
                itemData.IsNew = true;
                itemData.Owned = false;
            }
        }
    }
}
