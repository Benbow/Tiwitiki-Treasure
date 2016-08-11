using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TileSprite
{
    public Sprite TileImage;
    public Tiles TileType;
    public Vector2 TileCoord;

    public TileSprite()
    {
        TileImage = new Sprite();
        TileType = Tiles.Empty;
        TileCoord = new Vector2();
    }

    public TileSprite( Sprite image, Tiles tile, Vector2 coord)
    {
        TileImage = image;
        TileType = tile;
        TileCoord = coord;
    }
}