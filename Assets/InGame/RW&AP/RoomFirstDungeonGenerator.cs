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

        // �e�����̐^�񒆂̍��W�����X�g�ɒǉ����Ă���
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (BoundsInt room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        // ��L�̃n�b�V���Z�b�g �����̒��S���W��n���Đڑ����Ă��炤
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        _tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, _tilemapVisualizer);
    }

    HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        // �e�����ɑ΂��ď���������
        for (int i = 0; i < roomList.Count; i++)
        {
            BoundsInt roomBounds = roomList[i];
            Vector2Int roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            HashSet<Vector2Int> roomFloor = RunRandomWalk(RandomWalkParams, roomCenter);

            // �����̊e�}�X���ɏ�������
            foreach (Vector2Int pos in roomFloor)
            {
                // �����̓����A�ǂ���Ȃ��Ƃ���Ȃ�
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
        // �����_���ȕ����̒��S�ɑ΂��ď���������
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        Vector2Int currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        // �����œn���ꂽ�����̒��S���X�g����I�񂾒��S���폜����
        roomCenters.Remove(currentRoomCenter);

        // ��L�̒��S�̃��X�g��������胋�[�v
        while (roomCenters.Count > 0)
        {
            // �ł��߂����S��������
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            // �ł��߂����S�𒆐S���X�g����폜����
            roomCenters.Remove(closest);
            // �ڑ����ꂽ��L�̐���
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            // ���݂̒��S���X�V
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        // ��L�ƂȂ���W���i�[����n�b�V���Z�b�g
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        // ���ݑI�𒆂̕����̒��S���n�b�V���Z�b�g�Ɋi�[����
        Vector2Int pos = currentRoomCenter;
        corridor.Add(pos);
        
        // ��L��ړI�̕����܂ŐL�΂�
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
        // ��ԋ߂������̒��S��T��
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
        // �S�Ă̕����̍\���̂ɑ΂��ď���������
        foreach (BoundsInt room in roomList)
        {
            // �c���S���̃}�X�ɑ΂��ď�������
            // �I�t�Z�b�g�̒l��傫������ƕ������m�̋���������Ă���
            for (int col = _offset; col < room.size.x - _offset; col++)
            {
                for (int row = _offset; row < room.size.y - _offset; row++)
                {
                    // �����̒[���� + �c���̃T�C�Y �ŕ����̋t�[�����̈ʒu
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }
}
