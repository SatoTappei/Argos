using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] int _corridorLength = 14;
    [SerializeField] int _corridorCount = 5;
    [Range(0.0f, 1)]
    [SerializeField] float _roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPosSet = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPosSet = new HashSet<Vector2Int>();

        CreateCorridors(floorPosSet, potentialRoomPosSet);

        HashSet<Vector2Int> roomPosSet = CreateRooms(potentialRoomPosSet);
        
        // 通路の出口が部屋になっていない通路を見つける
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPosSet);

        CreateRoomsAtDeadEnd(deadEnds, roomPosSet);

        floorPosSet.UnionWith(roomPosSet);

        _tilemapVisualizer.PaintFloorTiles(floorPosSet);
        WallGenerator.CreateWalls(floorPosSet, _tilemapVisualizer);
    }

    void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloorSet)
    {
        foreach (Vector2Int pos in deadEnds)
        {
            // 回廊の先に部屋が生成されていないが他の部屋に接続されてしまっている場合は除く
            if (!roomFloorSet.Contains(pos))
            {
                // 部屋を生成する
                HashSet<Vector2Int> room = RunRandomWalk(RandomWalkParams, pos);
                roomFloorSet.UnionWith(room);
            }
        }
    }

    List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPosSet)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (Vector2Int pos in floorPosSet)
        {
            // 隣接するマスに床がない = 回廊の端が部屋になっていない
            int neighboursCount = 0;
            foreach(Vector2Int dir in Direction2D._cardinalDirectionsList)
            {
                if (floorPosSet.Contains(pos + dir))
                    neighboursCount++;
            }
            if (neighboursCount == 1)
                deadEnds.Add(pos);
        }
        return deadEnds;
    }

    HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPosSet)
    {
        HashSet<Vector2Int> roomPosSet = new HashSet<Vector2Int>();
        // 部屋を生成する確率分、生成する部屋を増減させる
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPosSet.Count * _roomPercent);

        List<Vector2Int> roomToCreate = potentialRoomPosSet.OrderBy(x => Guid.NewGuid())
                                                           .Take(roomToCreateCount)
                                                           .ToList();

        // 回廊の先からランダムウォークで部屋の生成を行う
        foreach(Vector2Int roomPos in roomToCreate)
        {
            HashSet<Vector2Int> roomFloor = RunRandomWalk(RandomWalkParams, roomPos);
            roomPosSet.UnionWith(roomFloor);
        }
        return roomPosSet;
    }

    void CreateCorridors(HashSet<Vector2Int> floorPosSet, HashSet<Vector2Int> potentialRoomPosSet)
    {
        Vector2Int currentPos = _startPos;
        potentialRoomPosSet.Add(currentPos);

        for (int i = 0; i < _corridorCount; i++)
        {
            // 1本の回廊をマスのリストで取得する
            List<Vector2Int> corridor = ProcGeneArgo.RandomWalkCorridor(currentPos, _corridorLength);
            // 現在の座標を回廊の出口に変更する
            currentPos = corridor[corridor.Count - 1];
            // 回廊の出口を部屋の候補として追加する
            potentialRoomPosSet.Add(currentPos);
            // 床として認識する座標のハッシュセットとがっちゃんこする
            floorPosSet.UnionWith(corridor);
        }
    }
}
