using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    // 依存性の注入
    // 床のコレクションと一緒に
    // TilemapVisualizerを代入してこのメソッド内で呼び出している
    public static void CreateWalls(HashSet<Vector2Int> floorPosSet, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> basicWallPositions = FindWallsInDirections(floorPosSet, Direction2D._cardinalDirectionsList);
        HashSet<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPosSet, Direction2D._diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPosSet);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPosSet);
    }

    static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer,
        HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPosSet)
    {
        foreach (Vector2Int pos in cornerWallPositions)
        {
            string neighbourBinaryType = "";
            foreach (Vector2Int dir in Direction2D._eightDirectionsList)
            {
                Vector2Int neighbourPos = pos + dir;
                if (floorPosSet.Contains(neighbourPos))
                {
                    neighbourBinaryType += "1";
                }
                else
                {
                    neighbourBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(pos, neighbourBinaryType);
        }
    }

    static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, 
        HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPosSet)
    {
        // 全ての壁を走査する
        foreach (Vector2Int pos in basicWallPositions)
        {
            string neighboursBinaryType = "";
            // 上下左右
            foreach (Vector2Int dir in Direction2D._cardinalDirectionsList)
            {
                Vector2Int neighbourPos = pos + dir;
                // 壁の指定された方向が床だったら
                if (floorPosSet.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(pos, neighboursBinaryType);
        }
    }

    static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPosSet, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPosSet = new HashSet<Vector2Int>();
        // 各座標に対して各方向について調べる
        foreach (Vector2Int pos in floorPosSet)
        {
            foreach (Vector2Int dir in directionList)
            {
                // 床のコレクションに調べた方向の座標がない場合、そこは壁になるのでハッシュセットに追加する
                Vector2Int neighbourPos = pos + dir;
                if (!floorPosSet.Contains(neighbourPos))
                {
                    wallPosSet.Add(neighbourPos);
                }
            }
        }

        return wallPosSet;
    }
}
