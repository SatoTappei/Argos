using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProcGeneArgo
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int length)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        Vector2Int prevPos = startPos;

        // �����_���ȕ����ɐi���1�}�X�����W���i�[���Ă���
        for (int i = 0; i < length; i++)
        {
            Vector2Int newPos = prevPos + Direction2D.GetRandomCardinalDirection();
            path.Add(newPos);
            prevPos = newPos;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int length)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        Vector2Int dir = Direction2D.GetRandomCardinalDirection();
        Vector2Int currentPos = startPos;
        // ������CurrentPos��������ƑO��̍Ō�̃}�X�Ȃ̂ŏd������
        corridor.Add(currentPos);

        // ��L�̃��X�g�Ɏw�肳�ꂽ�����A�}�X���̍��W��ǉ����Ă���
        for (int i = 0; i < length; i++)
        {
            currentPos += dir;
            corridor.Add(currentPos);
        }
        return corridor;
    }

    // ��敪���@
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        // �������̎l�p�`���L���[�ɓ˂�����
        roomsQueue.Enqueue(spaceToSplit);
        // �L���[�̒��g�����ɏ�������
        while (roomsQueue.Count > 0)
        {
            // �����ŃL���[������o���Ă���̂ŕ������������͏�����
            BoundsInt room = roomsQueue.Dequeue();
            // ������₪�ŏ��T�C�Y���傫��������
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                // ���X�̊m���ŏc�������͉�������D�悷��
                if (UnityEngine.Random.value < 0.5f)
                {
                    // �����̃T�C�Y��2�{�A�܂蕪���ł���T�C�Y�Ȃ�
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    // �c�ɂ����ɂ������ł���T�C�Y�Ȃ畔���̃��X�g�ɒǉ�����H
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    // �����̃T�C�Y��2�{�A�܂蕪���ł���T�C�Y�Ȃ�
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    // �c�ɂ����ɂ������ł���T�C�Y�Ȃ畔���̃��X�g�ɒǉ�����H
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        // �����ӏ��͕����̒[����1�}�X�����͈̔͂̂ǂ���
        int xSplit = Random.Range(1, room.size.x);
        // ����1�̍\���́A���̕����̍��[���番���ʒu�܂�
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.min.y, room.min.z));
        // ����2�̍\���́A�����ʒu���猳�̕����̃T�C�Y���番���ʒu���������傫��
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
                                        new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
                                        new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> _cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    public static List<Vector2Int> _diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1)
    };

    public static List<Vector2Int> _eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1)
    };

    /// <returns>�J�[�f�B�i�����X�g�̃����_���ȗv�f��Ԃ�</returns>
    public static Vector2Int GetRandomCardinalDirection()
    {
        return _cardinalDirectionsList[Random.Range(0, _cardinalDirectionsList.Count)];
    }
}