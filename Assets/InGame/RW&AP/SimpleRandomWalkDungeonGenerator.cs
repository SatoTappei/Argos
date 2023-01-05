using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField] SimpleRandomWalkSO _randomWalkParams;

    protected SimpleRandomWalkSO RandomWalkParams => _randomWalkParams;
    protected Vector2Int StartPos { get => _startPos; set => _startPos = value; }

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosSet = RunRandomWalk(_randomWalkParams, _startPos);
        _tilemapVisualizer.Clear();
        _tilemapVisualizer.PaintFloorTiles(floorPosSet);
        // コンポーネントを渡して使ってもらう
        WallGenerator.CreateWalls(floorPosSet, _tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO param, Vector2Int pos)
    {
        Vector2Int currentPos = pos;
        HashSet<Vector2Int> floorPosSet = new HashSet<Vector2Int>();
        // 指定した回数分ランダムウォークで得た軌跡を合体させコレクションに格納する
        for (int i = 0; i < param.Iteration; i++)
        {
            HashSet<Vector2Int> path = ProcGeneArgo.SimpleRandomWalk(currentPos, param.WalkLength);
            // パスを1つのハッシュセットにマージする
            floorPosSet.UnionWith(path);
            // 道をランダムに枝分かれさせるかどうか
            if (param.StartRandomlyEachIteration)
                currentPos = floorPosSet.ElementAt(Random.Range(0, floorPosSet.Count));
        }
        return floorPosSet;
    }
}
