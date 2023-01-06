using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAStar : MonoBehaviour
{
    public class Node
    {
        public bool walkable;
        public Vector3 worldPos;
        public int gridX;
        public int gridY;
        public Node parent;

        // 実コスト&推定コスト
        public int gCost;
        public int hCost;

        public Node(bool walkable, Vector3 worldPos, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPos = worldPos;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int fCost => gCost + hCost;
    }

    [SerializeField] LayerMask _walkableMask;
    [SerializeField] Vector2 _gridWorldPos;
    [SerializeField] float _nodeRadius;
   
    Node[,] _grid;
    float _nodeDistance;
    int _gridSizeX;
    int _gridSizeY;

    void Start()
    {
        _nodeDistance = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldPos.x / _nodeDistance);
        _gridSizeY = Mathf.RoundToInt(_gridWorldPos.y / _nodeDistance);
        CreateGrid();
    }

    void CreateGrid()
    {
        _grid = new Node[_gridSizeX, _gridSizeY];
        // オフセット？
        Vector3 bottomLeft = transform.position - Vector3.right * _gridWorldPos.x / 2 - Vector3.forward * _gridWorldPos.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDistance + _nodeRadius) +
                    Vector3.forward * (y * _nodeDistance + _nodeRadius);
                // 球形のコライダーがヒットしたら歩ける箇所
                // レイヤーマスクによって障害物のある場所との区別を付ける
                bool walkable = !Physics.CheckSphere(worldPoint, _nodeRadius, _walkableMask);

                _grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // 周囲8マスを調べる
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX * x;
                int checkY = node.gridY * y;

                // グリッド内なら追加する(例外対策)
                if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    neighbours.Add(_grid[checkX, checkY]);
                }
            }

        return neighbours;
    }

    public Node NodeFromWalkablePoint(Vector3 worldPos)
    {

        float percentX = (worldPos.x + _gridWorldPos.x / 2) / _gridWorldPos.x;
        float percentY = (worldPos.y + _gridWorldPos.y / 2) / _gridWorldPos.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // グリッド全体の何％進んだかで位置を判定している
        int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
        return _grid[x, y];
    }
}
