using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SimpleVisualizer;

public class Visualizer : MonoBehaviour
{
    [SerializeField] LSystemGenerator _lSystem;
    [SerializeField] RoadHelper _roadHelper;
    [SerializeField] StructureHelper _structureHelper;
    [SerializeField] int _roadLength = 8;
    List<Vector3> posList = new List<Vector3>();

    int length = 8;
    float angle = 90;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    void Start()
    {
        CreateTown();
    }

    public void CreateTown()
    {
        length = _roadLength;
        // 最初の1文字を見える化する
        string sequence = _lSystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    void VisualizeSequence(string sequence)
    {
        // 街を描画する座標として使う
        Stack<AgentParams> savePoints = new Stack<AgentParams>();
        Vector3 currentPos = Vector3.zero;

        // 道路を描画するのに使う
        Vector3 dir = Vector3.forward;
        Vector3 tempPos = Vector3.zero;

        // 原点をstackにプッシュしているのは最初のエージェントの最初のポイントが原点だから？
        posList.Add(currentPos);

        // 文字列内を走査する
        foreach (char letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.Save:
                    // オブジェクト初期化子を使用した初期化
                    // コンストラクタを呼び出さず、各プロパティに初期値を設定していく
                    savePoints.Push(new AgentParams
                    {
                        Pos = currentPos,
                        Dir = dir,
                        Length = Length
                    });
                    break;
                case EncodingLetters.Load:
                    if (savePoints.Count > 0)
                    {
                        AgentParams param = savePoints.Pop();
                        currentPos = param.Pos;
                        dir = param.Dir;
                        Length = param.Length;
                    }
                    else
                    {
                        throw new System.Exception("スタック内にセーブされたポイントがありません");
                    }
                    break;
                case EncodingLetters.Draw:
                    tempPos = currentPos;
                    currentPos += dir * length;
                    _roadHelper.PlaceStreetPos(Vector3Int.RoundToInt(tempPos), 
                                               Vector3Int.RoundToInt(dir), 
                                               length);
                    Length -= 2;             
                    posList.Add(currentPos);
                    break;
                case EncodingLetters.TurnRight:
                    // 戻り値はQuaternion型だがVector3をかける事でVector3型にできる
                    // 角度から方向のベクトル(1,0,0)とか(0,1,0)とかが求まる？
                    dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                    break;
                case EncodingLetters.TurnLeft:
                    dir = Quaternion.AngleAxis(-angle, Vector3.up) * dir;
                    break;
            }
        }
        _roadHelper.FixRoad();
        // クラスAのメソッドにクラスBのメソッドの戻り値を引数にしている
        // 仲介役(Presenter)的な
        _structureHelper.PlaceStructureAroundRoad(_roadHelper.GetRoadPos());
    }
}
