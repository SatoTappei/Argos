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
        // �R���|�[�l���g��n���Ďg���Ă��炤
        WallGenerator.CreateWalls(floorPosSet, _tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO param, Vector2Int pos)
    {
        Vector2Int currentPos = pos;
        HashSet<Vector2Int> floorPosSet = new HashSet<Vector2Int>();
        // �w�肵���񐔕������_���E�H�[�N�œ����O�Ղ����̂����R���N�V�����Ɋi�[����
        for (int i = 0; i < param.Iteration; i++)
        {
            HashSet<Vector2Int> path = ProcGeneArgo.SimpleRandomWalk(currentPos, param.WalkLength);
            // �p�X��1�̃n�b�V���Z�b�g�Ƀ}�[�W����
            floorPosSet.UnionWith(path);
            // ���������_���Ɏ}�����ꂳ���邩�ǂ���
            if (param.StartRandomlyEachIteration)
                currentPos = floorPosSet.ElementAt(Random.Range(0, floorPosSet.Count));
        }
        return floorPosSet;
    }
}
