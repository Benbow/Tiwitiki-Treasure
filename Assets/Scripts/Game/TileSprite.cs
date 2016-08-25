using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TileSprite : MonoBehaviour
{
    [SerializeField]
    private Tiles _tileType;
    private Vector2 _tileCoord;
   

    public TileSprite()
    {
        _tileType = Tiles.Empty;
        _tileCoord = new Vector2();
    }

    public TileSprite(Tiles tile, Vector2 coord)
    {
        _tileType = tile;
        _tileCoord = coord;
    }

    public Tiles TileType
    {
        get
        {
            return _tileType;
        }

        set
        {
            _tileType = value;
        }
    }

    public Vector2 TileCoord
    {
        get
        {
            return _tileCoord;
        }

        set
        {
            _tileCoord = value;
        }
    }

    public void OnMouseDown()
    {
        if (GameManager.instance.SoluceMapPos == this.TileCoord)
        {
            GameManager.instance.Found(this.gameObject);
        }
        else
        {
            GameManager.instance.Fail(this.gameObject);
        }
    }

}