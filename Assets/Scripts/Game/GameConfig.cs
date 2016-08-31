using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable/Game/Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField]
    private List<int> _jaugePoints = new List<int>(); // Save the differents points value earned by the player, system to change if gameplay change as well
    [SerializeField]
    private float _ratioWorld; // ratio for the evolution of the starting point of a world, base on the first one
    [SerializeField]
    private float _ratioLevel; // ratio for the evolution of the points of a level, based on the first one
    [SerializeField]
    private int _initialValue; // intial value for the first world and the first level

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
