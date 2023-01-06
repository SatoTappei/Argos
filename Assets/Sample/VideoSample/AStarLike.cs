using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarLike
{
    struct Node
    {
        Vector2Int pos;
        int fCost;

        public Node(int x, int y, int f)
        {
            pos = new Vector2Int(x, y);
            fCost = f;
        }

        public Vector2Int Pos => pos;
        public int FCost => fCost;
    }

    // 開いた/閉じたノードのリスト
    List<Node> _openList = new List<Node>();
    List<Node> _closeList = new List<Node>();

    void Proc(Vector2Int target)
    {
        // 最初の位置のノードを作成する
        Node start = new Node(0, 0, 0);
        // 開いたノードのリストに追加する
        _openList.Add(start);

        while (true)
        {
            // Fコストが一番低いノードを選択
            Node current = _openList.OrderBy(n => n.FCost).FirstOrDefault();
            // 開いたノードのリストから削除
            _openList.Remove(current);
            // 閉じたノードのリストに追加
            _closeList.Add(current);

            // 到着していたら処理を抜ける
            if (target == current.Pos) return;

            // currentの周囲8マスを調べる
            for (int i = 0; i < 8; i++)
            {
                // そのマスが移動不可能もしくは閉じたノードのリストに含まれていたら
                bool canMove = true;

                if (canMove)
                {
                    continue;
                }
                // 通れるマスなら
                else
                {
                    // コストが低いマスもしくはまだ開いてないノードなら
                    bool isLow = true;
                    if (isLow)
                    {
                        // Fコストを設定する
                        // 辿るために隣のノードの親に自身を設定する

                        // そのノードが既に開いたノードのリストに含まれていなかったら
                        bool isOpen = false;
                        if (isOpen)
                        {
                            // 開いたノードのリストに追加する
                        }
                    }
                }
            }
        }
    }
}
