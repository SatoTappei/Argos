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
        
        // �ʘH�̏o���������ɂȂ��Ă��Ȃ��ʘH��������
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
            // ��L�̐�ɕ�������������Ă��Ȃ������̕����ɐڑ�����Ă��܂��Ă���ꍇ�͏���
            if (!roomFloorSet.Contains(pos))
            {
                // �����𐶐�����
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
            // �אڂ���}�X�ɏ����Ȃ� = ��L�̒[�������ɂȂ��Ă��Ȃ�
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
        // �����𐶐�����m�����A�������镔���𑝌�������
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPosSet.Count * _roomPercent);

        List<Vector2Int> roomToCreate = potentialRoomPosSet.OrderBy(x => Guid.NewGuid())
                                                           .Take(roomToCreateCount)
                                                           .ToList();

        // ��L�̐悩�烉���_���E�H�[�N�ŕ����̐������s��
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
            // 1�{�̉�L���}�X�̃��X�g�Ŏ擾����
            List<Vector2Int> corridor = ProcGeneArgo.RandomWalkCorridor(currentPos, _corridorLength);
            // ���݂̍��W����L�̏o���ɕύX����
            currentPos = corridor[corridor.Count - 1];
            // ��L�̏o���𕔉��̌��Ƃ��Ēǉ�����
            potentialRoomPosSet.Add(currentPos);
            // ���Ƃ��ĔF��������W�̃n�b�V���Z�b�g�Ƃ�������񂱂���
            floorPosSet.UnionWith(corridor);
        }
    }
}
