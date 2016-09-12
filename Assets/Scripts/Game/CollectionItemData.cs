using System.Collections.Generic;
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
    private bool _new = true;
    [SerializeField]
    private List<ItemPlacementData> _placement = new List<ItemPlacementData>();
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

    public List<ItemPlacementData> PlacementData
    {
        get
        {
            return _placement;
        }

        set
        {
            _placement = value;
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

    public bool IsNew
    {
        get
        {
            return _new;
        }

        set
        {
            _new = value;
        }
    }
}
