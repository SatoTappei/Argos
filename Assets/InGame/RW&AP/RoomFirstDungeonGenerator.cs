using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] int _minRoomWidth = 4;
    [SerializeField] int _minRoomHeight = 4;
    [SerializeField] int _dungeonWidth = 20;
    [SerializeField] int _dungeonHeight = 20;
    [Range(0, 10)]
    [SerializeField] int _offset = 1;
    [SerializeField] bool _randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    void CreateRooms()
    {
        List<BoundsInt> roomList = ProcGeneArgo.BinarySpacePartitioning(new BoundsInt((Vector3Int)_startPos,
            new Vector3Int(_dungeonWidth, _dungeonHeight, 0)), _minRoomWidth, _minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        if (_randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomList);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);
        }

        // 各部屋の真ん中の座標をリストに追加していく
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (BoundsInt room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // 回廊のハッシュセット 部屋の中心座標を渡して接続してもらう
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        _tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, _tilemapVisualizer);
    }

    HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        // 各部屋に対して処理をする
        for (int i = 0; i < roomList.Count; i++)
        {
            BoundsInt roomBounds = roomList[i];
            Vector2Int roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            HashSet<Vector2Int> roomFloor = RunRandomWalk(RandomWalkParams, roomCenter);

            // 部屋の各マス毎に処理する
            foreach (Vector2Int pos in roomFloor)
            {
                // 部屋の内側、壁じゃないところなら
                if (pos.x >= (roomBounds.xMin + _offset) && pos.x <= (roomBounds.xMax - _offset) &&
                    pos.y >= (roomBounds.yMin + _offset) && pos.y <= (roomBounds.yMax - _offset))
                {
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }

    HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        // ランダムな部屋の中心に対して処理をする
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        Vector2Int currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        // 引数で渡された部屋の中心リストから選んだ中心を削除する
        roomCenters.Remove(currentRoomCenter);

        // 回廊の中心のリストがある限りループ
        while (roomCenters.Count > 0)
        {
            // 最も近い中心を見つける
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            // 最も近い中心を中心リストから削除する
            roomCenters.Remove(closest);
            // 接続された回廊の生成
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            // 現在の中心を更新
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        // 回廊となる座標を格納するハッシュセット
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        // 現在選択中の部屋の中心をハッシュセットに格納する
        Vector2Int pos = currentRoomCenter;
        corridor.Add(pos);
        
        // 回廊を目的の部屋まで伸ばす
        while (pos.y != destination.y)
        {
            if (destination.y > pos.y)
            {
                pos += Vector2Int.up;
            }
            else if (destination.y < pos.y)
            {
                pos += Vector2Int.down;
            }
            corridor.Add(pos);
        }
        while (pos.x != destination.x)
        {
            if (destination.x > pos.x)
            {
                pos += Vector2Int.right;
            }
            else if (destination.x < pos.x)
            {
                pos += Vector2Int.left;
            }
            corridor.Add(pos);
        }

        return corridor;
    }

    Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        // 一番近い部屋の中心を探す
        foreach (Vector2Int pos in roomCenters)
        {
            float currentDistance = Vector2.Distance(pos, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = pos;
            }
        }
        return closest;
    }

    HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        // 全ての部屋の構造体に対して処理をする
        foreach (BoundsInt room in roomList)
        {
            // 縦横全部のマスに対して処理する
            // オフセットの値を大きくすると部屋同士の距離が離れていく
            for (int col = _offset; col < room.size.x - _offset; col++)
            {
                for (int row = _offset; row < room.size.y - _offset; row++)
                {
                    // 部屋の端っこ + 縦横のサイズ で部屋の逆端っこの位置
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }
}
