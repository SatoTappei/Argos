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

    // パスを複数同時に探さないように制御するフラグ
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
        // マルチスレッド処理
        ThreadStart threadStart = delegate
        {
            //instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        //PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
        //// キューに新しいパスを追加する
        //instance.pathRequestQueue.Enqueue(newRequest);
        //instance.TryProcesNext();
    }

    void TryProcesNext()
    {
        //// パス検索の処理中ではない かつ パスのキューが0より大きければ
        //if (!isProcessingPath && pathRequestQueue.Count > 0)
        //{
        //    // 先頭のパスを取得
        //    currentPathRequest = pathRequestQueue.Dequeue();
        //    // パスの処理中フラグを立てる
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
        //// パスの処理が終わったのでパスの配列と結果を引数にコールバック実行
        //currentPathRequest.callBack(path, success);
        //// パスの処理中フラグを折る
        //isProcessingPath = false;
        //// 次のパスの処理
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