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
            // ��ԃR�X�g���Ⴂ
            Node current = openSet[0];
            // �J�����m�[�h�̃��X�g�𑖍�����
            for(int i = 1; i < openSet.Count; i++)
            {
                // �R�X�g���Ⴂ �������� ����R�X�g���Ⴏ���
                if (openSet[i].fCost < current.fCost ||
                    openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    // �I�𒆂̃m�[�h���X�V����
                    current = openSet[i];
                }
            }

            // �J�����m�[�h�̃��X�g����폜
            openSet.Remove(current);
            // �����m�[�h�̃��X�g�ɒǉ�
            closedSet.Add(current);

            // �������Ă����珈���𔲂���
            if (current == targetNode) return;

            // ����8�}�X�𒲂ׂ�
            foreach (Node neighbour in _grid.GetNeighbours(current))
            {
                // �����Ȃ��}�X �������� �����}�X�Ɋ܂܂�Ă���(�����}�X��2�񒲂ׂ邱�ƂɂȂ�)��
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // ���݂̃}�X����̍ŒZ����(�ǂ��܂܂Ȃ���������)���R�X�g�Ƃ���
                int cost = current.gCost + GetDistance(current, neighbour);
                // �R�X�g���Ⴂ�������͊J�����m�[�h�̃��X�g�Ɋ܂܂�Ă��Ȃ��Ȃ�
                if(cost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // �ŏ���1��(�m�[�h���J���Ƃ�)�͕K���ݒ肷�邪
                    // 2��ڈȍ~�͂���R�X�g�ɂȂ����Ƃ��ɍX�V�����
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

        // �ŒZ������Ԃ��A�΂߈ړ����ĉ�/�c�ړ�����v�Z
        // �΂߈ړ����Ɩ�1.4�{�����̂�10�{����14��10�ɂȂ����H
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
