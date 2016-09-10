using UnityEngine;

[System.Serializable]
public class CollectionItemData {

    /*[SerializeField]
    private int _itemId;
    [SerializeField]
    private int _worldId;*/
    [SerializeField]
    private Sprite _collectionSprite;
    [SerializeField]
    private bool _owned = false;
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private string _name;

    /*public int ItemId
    {
        get
        {
            return _itemId;
        }

        set
        {
            _itemId = value;
        }
    }

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
    }*/

    public Sprite CollectionSprite
    {
        get
        {
            return _collectionSprite;
        }

        set
        {
            _collectionSprite = value;
        }
    }

    public bool Owned
    {
        get
        {
            return _owned;
        }

        set
        {
            _owned = value;
        }
    }

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

    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
        }
    }
}
