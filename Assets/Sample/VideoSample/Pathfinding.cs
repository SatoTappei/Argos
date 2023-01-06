using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = GridAStar.Node;

public class Pathfinding : MonoBehaviour
{
    GridAStar _grid;

    void Awake()
    {
        _grid = GetComponent<GridAStar>();
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.NodeFromWalkablePoint(startPos);
        Node targetNode = _grid.NodeFromWalkablePoint(targetPos);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // 一番コストが低い
            Node current = openSet[0];
            // 開いたノードのリストを走査する
            for(int i = 1; i < openSet.Count; i++)
            {
                // コストが低い もしくは 推定コストが低ければ
                if (openSet[i].fCost < current.fCost ||
                    openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    // 選択中のノードを更新する
                    current = openSet[i];
                }
            }

            // 開いたノードのリストから削除
            openSet.Remove(current);
            // 閉じたノードのリストに追加
            closedSet.Add(current);

            // 到着していたら処理を抜ける
            if (current == targetNode) return;

            // 周囲8マスを調べる
            foreach (Node neighbour in _grid.GetNeighbours(current))
            {
                // 歩けないマス もしくは 閉じたマスに含まれていた(同じマスを2回調べることになる)ら
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // 現在のマスからの最短距離(壁を含まない直線距離)をコストとする
                int cost = current.gCost + GetDistance(current, neighbour);
                // コストが低いもしくは開けたノードのリストに含まれていないなら
                if(cost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // 最初の1回(ノードを開くとき)は必ず設定するが
                    // 2回目以降はより低コストになったときに更新される
                    neighbour.gCost = cost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // 最短距離を返す、斜め移動して横/縦移動する計算
        // 斜め移動だと約1.4倍速いので10倍して14と10になった？
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
