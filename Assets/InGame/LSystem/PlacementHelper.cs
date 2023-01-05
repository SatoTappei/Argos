using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<Direction> FindNeighbour(Vector3Int pos, ICollection<Vector3Int> collection)
    {
        List<Direction> neighbourDirs = new List<Direction>();
        // ���W+�����̃x�N�g�����R���N�V�����ɑ��݂���Ȃ�
        // ���W�ƕ�����g�ݍ��킹�Đݒu���A�ݒu��̍��W���R���N�V�����Ɋi�[����Ă��邩��o����
        if (collection.Contains(pos + Vector3Int.right))
        {
            neighbourDirs.Add(Direction.Right);
        }
        if (collection.Contains(pos - Vector3Int.right))
        {
            neighbourDirs.Add(Direction.Left);
        }
        if (collection.Contains(pos + new Vector3Int(0, 0, 1)))
        {
            neighbourDirs.Add(Direction.Up);
        }
        if (collection.Contains(pos - new Vector3Int(0, 0, 1)))
        {
            neighbourDirs.Add(Direction.Down);
        }

        return neighbourDirs;
    }

    public static Vector3Int GetOffsetFromDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, 1);
            case Direction.Down:
                return new Vector3Int(0, 0, -1);
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new System.Exception("������������܂���: " + dir);
    }

    public static Direction GetReverseDir(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                break;
        }
        throw new System.Exception("������������܂���: " + dir);
    }
}
