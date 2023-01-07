using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = GridAStar.Node;
using System;

public class Pathfinding : MonoBehaviour
{
    //PathRequestManager _requestManager;
    GridAStar _grid;

    void Awake()
    {
       // _requestManager = GetComponent<PathRequestManager>();
        _grid = GetComponent<GridAStar>();
    }

    //public void StartFindPath(Vector3 startPos,Vector3 targetPos)
    //{
    //    StartCoroutine(FindPath(startPos, targetPos));
    //}

    //public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    //{
    //    Vector3[] wayPoints = new Vector3[0];
    //    bool pathSuccess = false;

    //    // ���[���h���W���O���b�h�̍��W�ɕϊ����Ă���
    //    Node startNode = _grid.NodeFromWalkablePoint(startPos);
    //    Node targetNode = _grid.NodeFromWalkablePoint(targetPos);

    //    List<Node> openSet = new List<Node>();
    //    List<Node> closedSet = new List<Node>();
    //    openSet.Add(startNode);

    //    while (openSet.Count > 0)
    //    {
    //        // ��ԃR�X�g���Ⴂ
    //        Node current = openSet[0];
    //        // �J�����m�[�h�̃��X�g�𑖍�����
    //        for(int i = 1; i < openSet.Count; i++)
    //        {
    //            // �R�X�g���Ⴂ �������� ����R�X�g���Ⴏ���
    //            if (openSet[i].fCost < current.fCost ||
    //                openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
    //            {
    //                // �I�𒆂̃m�[�h���X�V����
    //                current = openSet[i];
    //            }
    //        }

    //        // �J�����m�[�h�̃��X�g����폜
    //        openSet.Remove(current);
    //        // �����m�[�h�̃��X�g�ɒǉ�
    //        closedSet.Add(current);

    //        // �������Ă����珈���𔲂���
    //        if (current == targetNode)
    //        {
    //            pathSuccess = true;
    //            RetracePath(startNode, targetNode);
    //            yield break;
    //        }

    //        // ����8�}�X�𒲂ׂ�
    //        foreach (Node neighbour in _grid.GetNeighbours(current))
    //        {
    //            // �����Ȃ��}�X �������� �����}�X�Ɋ܂܂�Ă���(�����}�X��2�񒲂ׂ邱�ƂɂȂ�)��
    //            if(!neighbour.walkable || closedSet.Contains(neighbour))
    //            {
    //                continue;
    //            }

    //            // ���݂̃}�X����̍ŒZ����(�ǂ��܂܂Ȃ���������)���R�X�g�Ƃ���
    //            // �y�i���e�B�R�X�g�𑫂�
    //            int cost = current.gCost + GetDistance(current, neighbour) + neighbour.movementPenalty;
    //            // �R�X�g���Ⴂ�������͊J�����m�[�h�̃��X�g�Ɋ܂܂�Ă��Ȃ��Ȃ�
    //            if(cost < neighbour.gCost || !openSet.Contains(neighbour))
    //            {
    //                // �ŏ���1��(�m�[�h���J���Ƃ�)�͕K���ݒ肷�邪
    //                // 2��ڈȍ~�͂���R�X�g�ɂȂ����Ƃ��ɍX�V�����
    //                neighbour.gCost = cost;
    //                neighbour.hCost = GetDistance(neighbour, targetNode);
    //                neighbour.parent = current;

    //                if (!openSet.Contains(neighbour))
    //                    openSet.Add(neighbour);
    //                //else
    //                //    openSet.UpdateItem(neighbour);
    //            }
    //        }
    //    }
    //    yield return null;

    //    if (pathSuccess)
    //    {
    //        wayPoints = RetracePath(startNode, targetNode);
    //    }
    //    _requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    //}

    public void FindPath(PathRequest request, Action<PathRequest> callback)
    {
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        // ���[���h���W���O���b�h�̍��W�ɕϊ����Ă���
        Node startNode = _grid.NodeFromWalkablePoint(request.pathStart);
        Node targetNode = _grid.NodeFromWalkablePoint(request.pathEnd);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // ��ԃR�X�g���Ⴂ
            Node current = openSet[0];
            // �J�����m�[�h�̃��X�g�𑖍�����
            for (int i = 1; i < openSet.Count; i++)
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
            if (current == targetNode)
            {
                pathSuccess = true;
                RetracePath(startNode, targetNode);
                //yield break;
            }

            // ����8�}�X�𒲂ׂ�
            foreach (Node neighbour in _grid.GetNeighbours(current))
            {
                // �����Ȃ��}�X �������� �����}�X�Ɋ܂܂�Ă���(�����}�X��2�񒲂ׂ邱�ƂɂȂ�)��
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // ���݂̃}�X����̍ŒZ����(�ǂ��܂܂Ȃ���������)���R�X�g�Ƃ���
                // �y�i���e�B�R�X�g�𑫂�
                int cost = current.gCost + GetDistance(current, neighbour) + neighbour.movementPenalty;
                // �R�X�g���Ⴂ�������͊J�����m�[�h�̃��X�g�Ɋ܂܂�Ă��Ȃ��Ȃ�
                if (cost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // �ŏ���1��(�m�[�h���J���Ƃ�)�͕K���ݒ肷�邪
                    // 2��ڈȍ~�͂���R�X�g�ɂȂ����Ƃ��ɍX�V�����
                    neighbour.gCost = cost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    //else
                    //    openSet.UpdateItem(neighbour);
                }
            }
        }

        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        //_requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
        //callback(new PathRequest(wayPoints, pathSuccess, request.callBack));
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // �ڕW�̍��W����X�^�[�g�܂ōċA�I�Ƀm�[�h�����ǂ��Ă���
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);
        return wayPoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            // 1�O�̃E�F�C�|�C���g���獡�̃E�F�C�|�C���g�������Č������o��
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            // ������������ = �����Ȃ�Ƃ��Ȃ���K�v�������
            if(directionNew != directionOld)
            {
                wayPoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }
        return wayPoints.ToArray();
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
