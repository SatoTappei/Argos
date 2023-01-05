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

        // ランダムな方向に進んで1マスずつ座標を格納していく
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
        // ここでCurrentPosを代入すると前回の最後のマスなので重複する
        corridor.Add(currentPos);

        // 回廊のリストに指定された方向、マス分の座標を追加していく
        for (int i = 0; i < length; i++)
        {
            currentPos += dir;
            corridor.Add(currentPos);
        }
        return corridor;
    }

    // 区域分割法
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        // 部屋候補の四角形をキューに突っ込む
        roomsQueue.Enqueue(spaceToSplit);
        // キューの中身を順に処理する
        while (roomsQueue.Count > 0)
        {
            // ここでキューから取り出しているので分割した部屋は消える
            BoundsInt room = roomsQueue.Dequeue();
            // 部屋候補が最小サイズより大きかったら
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                // 半々の確率で縦もしくは横分割を優先する
                if (UnityEngine.Random.value < 0.5f)
                {
                    // 部屋のサイズの2倍、つまり分割できるサイズなら
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    // 縦にも横にも分割できるサイズなら部屋のリストに追加する？
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    // 部屋のサイズの2倍、つまり分割できるサイズなら
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    // 縦にも横にも分割できるサイズなら部屋のリストに追加する？
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
        // 分割箇所は部屋の端から1マス内側の範囲のどこか
        int xSplit = Random.Range(1, room.size.x);
        // 部屋1の構造体、元の部屋の左端から分割位置まで
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.min.y, room.min.z));
        // 部屋2の構造体、分割位置から元の部屋のサイズから分割位置を引いた大きさ
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

    /// <returns>カーディナルリストのランダムな要素を返す</returns>
    public static Vector2Int GetRandomCardinalDirection()
    {
        return _cardinalDirectionsList[Random.Range(0, _cardinalDirectionsList.Count)];
    }
}