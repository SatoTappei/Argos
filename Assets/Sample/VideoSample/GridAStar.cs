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

        // ���R�X�g&����R�X�g
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
            // �����̃m�[�h�ƁA���̃m�[�h��fCost���r����
            int compare = fCost.CompareTo(other.fCost);
            // �����Ȃ�h�R�X�g�Ŕ�r����
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }

            // �������ق����D�悵�����̂ő召�̔�r���ʂ𔽓]������
            return -compare;
        }
    }

    [SerializeField] LayerMask _unwalkableMask;
    [SerializeField] Vector2 _gridWorldPos;
    [SerializeField] float _nodeRadius;
    // �C���X�y�N�^��Œn�`�̎�ނ�ݒ肷��
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

        // �r�b�g�a�Ōv�Z����
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
        // �I�t�Z�b�g�H
        Vector3 bottomLeft = transform.position - Vector3.right * _gridWorldPos.x / 2 - Vector3.forward * _gridWorldPos.y / 2;

        for (int x = 0; x < _gridSizeX; x++)
            for (int y = 0; y < _gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDistance + _nodeRadius) +
                    Vector3.forward * (y * _nodeDistance + _nodeRadius);
                // ���`�̃R���C�_�[���q�b�g�����������ӏ�
                // ���C���[�}�X�N�ɂ���ď�Q���̂���ꏊ�Ƃ̋�ʂ�t����
                bool walkable = !Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableMask);

                int movementPenalty = 0;

                // raycast�Œn�`�̃y�i���e�B�𔻒�
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

        // �u���[�������邱�Ƃɂ��G�[�W�F���g�����H�̒[������̂�h��
        BlurPenaltyMap(3);
    }

    // �O���b�h�u���[(�v�׋�)
    void BlurPenaltyMap(int blurSize)
    {
        // �u���[��������͈͂�1�}�X�Ȃ�J�[�l���T�C�Y��3*3�̃{�b�N�X�ɂȂ�
        int kernelSize = blurSize * 2 + 1;
        // �J�[�l���͈̔͂�3*3�Ȃ�1,5*5�Ȃ�2�̂悤�ɒ�������ǂꂭ�炢�g������Ă��邩
        int kernelExtents = (kernelSize - 1) / 2;

        // �u���[�����������͈́A����Ȃ�O���b�h�S��
        // �c�ɂ����镪�Ɖ��ɂ����镪��2�p�ӂ���
        int[,] penaltiesHorizontalPass = new int[_gridSizeX, _gridSizeY];
        int[,] penaltiesVerticalPass = new int[_gridSizeX, _gridSizeY];

        // ��ԏォ�牡�����Ƀu���[�������Ă���
        for(int y = 0; y < _gridSizeY; y++)
        {
            // �J�[�l���͈̔�(�g�����ꂽ��)�������[�v����
            for(int x = -kernelExtents; x <= kernelExtents; x++)
            {
                // x��0����J�[�l���͈̔͂ŋ��ݍ��� ��:3*3�Ȃ�0~1
                // �͈͂̊O1�}�X��x=0�Ɠ����l�Ȃ̂�clamp�ŋ��ݍ���ł���
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                // �������̃u���[�̔z��̈�ԍ����̗�ɑ�������ł���
                penaltiesHorizontalPass[0, y] += _grid[sampleX, y].movementPenalty;
            }
            for(int x = 1; x < _gridSizeX; x++)
            {
                // x                1����O���b�h�̉���
                // kernelExtents-1  �͈�-1(3*3�Ȃ�0,5*5�Ȃ�1)
                // 0����O���b�h�̉����ŋ���
                // �O��2�}�X�ɂ͊g���ł��Ȃ��̂�5*5�̏ꍇ��z�肵�ĕ��G�����Ă���H
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, _gridSizeX);
                // 2�ȏ�̒l��0����O���b�h�̉���-1�ŋ��݂���
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, _gridSizeX - 1);
                
                // ���̈����Z���Ă���H
                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] -
                    _grid[removeIndex, y].movementPenalty + _grid[addIndex, y].movementPenalty;
            }
        }

        // ��ԏォ��c�����Ƀu���[�������Ă���
        // �������̃u���[�ɑ�������ł���
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

        // ����8�}�X�𒲂ׂ�
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX * x;
                int checkY = node.gridY * y;

                // �O���b�h���Ȃ�ǉ�����(��O�΍�)
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

        // �O���b�h�S�̂̉����i�񂾂��ňʒu�𔻒肵�Ă���
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
