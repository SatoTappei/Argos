using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    // �ˑ����̒���
    // ���̃R���N�V�����ƈꏏ��
    // TilemapVisualizer�������Ă��̃��\�b�h���ŌĂяo���Ă���
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
        // �S�Ă̕ǂ𑖍�����
        foreach (Vector2Int pos in basicWallPositions)
        {
            string neighboursBinaryType = "";
            // �㉺���E
            foreach (Vector2Int dir in Direction2D._cardinalDirectionsList)
            {
                Vector2Int neighbourPos = pos + dir;
                // �ǂ̎w�肳�ꂽ����������������
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
        // �e���W�ɑ΂��Ċe�����ɂ��Ē��ׂ�
        foreach (Vector2Int pos in floorPosSet)
        {
            foreach (Vector2Int dir in directionList)
            {
                // ���̃R���N�V�����ɒ��ׂ������̍��W���Ȃ��ꍇ�A�����͕ǂɂȂ�̂Ńn�b�V���Z�b�g�ɒǉ�����
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
