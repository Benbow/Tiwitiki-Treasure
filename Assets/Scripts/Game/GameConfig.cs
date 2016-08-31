using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable/Game/Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField]
    private List<int> _jaugePoints = new List<int>();
    [SerializeField]
    private float _ratioWorld;
    [SerializeField]
    private float _ratioLevel;
    [SerializeField]
    private int _initialValue;

    public List<int> JaugePoints
    {
        get
        {
            return _jaugePoints;
        }
    }

    public float RatioWorld
    {
        get
        {
            return _ratioWorld;
        }
    }

    public float RatioLevel
    {
        get
        {
            return _ratioLevel;
        }
    }

    public int InitialValue
    {
        get
        {
            return _initialValue;
        }
    }
}
