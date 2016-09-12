using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemPlacementData  {

    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private Vector2 _size;
    [SerializeField]
    private Vector3 _scaling = new Vector3(1,1,1);

    public Vector3 Position
    {
        get
        {
            return _position;
        }

        set
        {
            _position = value;
        }
    }

    public Vector2 Size
    {
        get
        {
            return _size;
        }

        set
        {
            _size = value;
        }
    }

    public Vector3 Scaling
    {
        get
        {
            return _scaling;
        }

        set
        {
            _scaling = value;
        }
    }
}
