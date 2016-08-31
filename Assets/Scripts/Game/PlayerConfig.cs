using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable/Player/Config")]
public class PlayerConfig : ScriptableObject {
    [SerializeField]
    private int _actualWorld;
    [SerializeField]
    private List<Vector3> _unlockedWorldsList;

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

    public List<Vector3> UnlockedWorldsList
    {
        get
        {
            return _unlockedWorldsList;
        }

        set
        {
            _unlockedWorldsList = value;
        }
    }

    public Vector3 GetActualWorldData()
    {
        foreach (Vector3 worldData in _unlockedWorldsList)
        {
            if (worldData.x == ActualWorld)
                return worldData;
        }

        return new Vector3();
    }

    public float GetActualLevel()
    {
        foreach (Vector3 worldData in _unlockedWorldsList)
        {
            if (worldData.x == ActualWorld)
                return worldData.y;
        }

        return -1;
    }

    public void SetActualLevel(int level, int points = 0)
    {
        for(int i = 0; i < UnlockedWorldsList.Count; i++)
        {
            if (UnlockedWorldsList[i].x == ActualWorld)
                UnlockedWorldsList[i] = new Vector3(UnlockedWorldsList[i].x, level, points);
        }
    }

    public float GetActualScore()
    {
        foreach (Vector3 worldData in _unlockedWorldsList)
        {
            if (worldData.x == ActualWorld)
                return worldData.z;
        }

        return -1;
    }

    public void SetActualScore(int points)
    {
        for (int i = 0; i < UnlockedWorldsList.Count; i++)
        {
            if (UnlockedWorldsList[i].x == ActualWorld)
                UnlockedWorldsList[i] = new Vector3(UnlockedWorldsList[i].x, UnlockedWorldsList[i].y, points);
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
