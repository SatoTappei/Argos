using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    //Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    //PathRequest currentPathRequest;
    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    Pathfinding pathfinding;

    // �p�X�𕡐������ɒT���Ȃ��悤�ɐ��䂷��t���O
    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        // �}���`�X���b�h����
        ThreadStart threadStart = delegate
        {
            //instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        //PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
        //// �L���[�ɐV�����p�X��ǉ�����
        //instance.pathRequestQueue.Enqueue(newRequest);
        //instance.TryProcesNext();
    }

    void TryProcesNext()
    {
        //// �p�X�����̏������ł͂Ȃ� ���� �p�X�̃L���[��0���傫�����
        //if (!isProcessingPath && pathRequestQueue.Count > 0)
        //{
        //    // �擪�̃p�X���擾
        //    currentPathRequest = pathRequestQueue.Dequeue();
        //    // �p�X�̏������t���O�𗧂Ă�
        //    isProcessingPath = true;
        //    pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        //}
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        //// �p�X�̏������I������̂Ńp�X�̔z��ƌ��ʂ������ɃR�[���o�b�N���s
        //currentPathRequest.callBack(path, success);
        //// �p�X�̏������t���O��܂�
        //isProcessingPath = false;
        //// ���̃p�X�̏���
        //TryProcesNext();
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callBack;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callBack)
    {
        pathStart = _start;
        pathEnd = _end;
        callBack = _callBack;
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}