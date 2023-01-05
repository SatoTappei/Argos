using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] Tilemap _floorTilemap;
    [SerializeField] Tilemap _wallTilemap;
    [SerializeField] TileBase _floorTile;
    [SerializeField] TileBase _wallTop;
    [SerializeField] TileBase _wallSideRight;
    [SerializeField] TileBase _wallSideLeft;
    [SerializeField] TileBase _wallBottom;
    [SerializeField] TileBase _wallFull;
    [SerializeField] TileBase _wallInCornerDownLeft;
    [SerializeField] TileBase _wallInCornerDownRight;
    [SerializeField] TileBase _wallDiaCornerDownRight;
    [SerializeField] TileBase _wallDiaCornerDownLeft;
    [SerializeField] TileBase _wallDiaCornerUpRight;
    [SerializeField] TileBase _wallDiaCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPosColl)
    {
        PaintTiles(floorPosColl, _floorTilemap, _floorTile);
    }

    void PaintTiles(IEnumerable<Vector2Int> posColl, Tilemap tilemap, TileBase tile)
    {
        foreach (Vector2Int pos in posColl)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    internal void PaintSingleBasicWall(Vector2Int pos, string binaryType)
    {
        // 周囲のマスの判定を文字列にしたバイナリをint型に変換
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        // そのタイプの壁を設置するための条件が複数あるので判定する
        if (WallTypeHelper.wallTop.Contains(typeAsInt))
        {
            tile = _wallTop;
        }
        else if (WallTypeHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = _wallSideRight;
        }
        else if (WallTypeHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = _wallSideLeft;
        }
        else if (WallTypeHelper.wallBottm.Contains(typeAsInt))
        {
            tile = _wallBottom;
        }

        if (tile != null)
            PaintSingleTile(_wallTilemap, tile, pos);
    }

    internal void PaintSingleCornerWall(Vector2Int pos, string neighbourBinaryType)
    {
        //throw new NotImplementedException();
    }

    void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }

    public void Clear()
    {
        _floorTilemap.ClearAllTiles();
        _wallTilemap.ClearAllTiles();
    }
}
