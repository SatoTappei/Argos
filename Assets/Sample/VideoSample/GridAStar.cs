using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAStar : MonoBehaviour
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector3 worldPos;
        public int gridX;
        public int gridY;
        public int movementPenalty;
        public Node parent;
        int heapIndex;

        // 実コスト&推定コスト
        public int gCost;
        public int hCost;

        public Node(bool walkable, Vector3 worldPos, int gridX, int gridY, int penalty)
        {
            this.walkable = walkable;
            this.worldPos = worldPos;
            this.gridX = gridX;
            this.gridY = gridY;
            movementPenalty = penalty;
        }

        public int fCost => gCost + hCost;

        public int HeapIndex { get => heapIndex; set => heapIndex = value; }

        public int CompareTo(Node other)
        {
            // 引数のノードと、このノードのfCostを比較する
            int compare = fCost.CompareTo(other.fCost);
            // 同じならhコストで比較する
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }

            // 小さいほうが優先したいので大小の比較結果を反転させる
            return -compare;
        }
    }

    [SerializeField] LayerMask _unwalkableMask;
    [SerializeField] Vector2 _gridWorldPos;
    [SerializeField] float _nodeRadius;
    // インスペクタ上で地形の種類を設定する
    [SerializeField] TerrainType[] _walkableRegions;
    [SerializeField] int _obstacleProximityPenalty = 10;
    LayerMask _walkableMask;
    Dictionary<int, int> walkableRegionsDic = new Dictionary<int, int>();
   
    Node[,] _grid;
    float _nodeDistance;
    int _gridSizeX;
    int _gridSizeY;
    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    public int MaxSize => _gridSizeX * _gridSizeY;

    void Awake()
    {
        _nodeDistance = _nodeRadius * 2;
        _gridSizeX = Mathf.RoundToInt(_gridWorldPos.x / _nodeDistance);
        _gridSizeY = Mathf.RoundToInt(_gridWorldPos.y / _nodeDistance);

        // ビット和で計算する
        foreach(TerrainType region in _walkableRegions)
        {
            _walkableMask.value |= region.terrainMask.value;
            walkableRegionsDic.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }
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
                bool walkable = !Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableMask);

                int movementPenalty = 0;

                // raycastで地形のペナルティを判定
                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, _walkableMask))
                {
                    walkableRegionsDic.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }


                if (!walkable)
                {
                    movementPenalty += _obstacleProximityPenalty;
                }

                _grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }

        // ブラーをかけることによりエージェントが道路の端を歩くのを防ぐ
        BlurPenaltyMap(3);
    }

    // グリッドブラー(要勉強)
    void BlurPenaltyMap(int blurSize)
    {
        // ブラーをかける範囲が1マスならカーネルサイズは3*3のボックスになる
        int kernelSize = blurSize * 2 + 1;
        // カーネルの範囲は3*3なら1,5*5なら2のように中央からどれくらい拡張されているか
        int kernelExtents = (kernelSize - 1) / 2;

        // ブラーをかけたい範囲、今回ならグリッド全体
        // 縦にかける分と横にかける分で2つ用意する
        int[,] penaltiesHorizontalPass = new int[_gridSizeX, _gridSizeY];
        int[,] penaltiesVerticalPass = new int[_gridSizeX, _gridSizeY];

        // 一番上から横方向にブラーをかけていく
        for(int y = 0; y < _gridSizeY; y++)
        {
            // カーネルの範囲(拡張された分)だけループを回す
            for(int x = -kernelExtents; x <= kernelExtents; x++)
            {
                // xを0からカーネルの範囲で挟み込む 例:3*3なら0~1
                // 範囲の外1マスはx=0と同じ値なのでclampで挟み込んでいる
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                // 横方向のブラーの配列の一番左側の列に足しこんでいく
                penaltiesHorizontalPass[0, y] += _grid[sampleX, y].movementPenalty;
            }
            for(int x = 1; x < _gridSizeX; x++)
            {
                // x                1からグリッドの横幅
                // kernelExtents-1  範囲-1(3*3なら0,5*5なら1)
                // 0からグリッドの横幅で挟む
                // 外側2マスには拡張できないので5*5の場合を想定して複雑化している？
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, _gridSizeX);
                // 2以上の値を0からグリッドの横幅-1で挟みこむ
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, _gridSizeX - 1);
                
                // 何故引き算している？
                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] -
                    _grid[removeIndex, y].movementPenalty + _grid[addIndex, y].movementPenalty;
            }
        }

        // 一番上から縦方向にブラーをかけていく
        // 横方向のブラーに足しこんでいく
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            _grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < _gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, _gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, _gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[y, y-1] -
                    penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];

                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                _grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
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

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldPos.x, 1, _gridWorldPos.y));

        if (_grid != null)
        {
            foreach(Node n in _grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (_nodeDistance - .1f));
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
