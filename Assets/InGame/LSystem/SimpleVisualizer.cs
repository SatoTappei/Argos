using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVisualizer : MonoBehaviour
{
    public enum EncodingLetters
    {
        Unknown = '1',
        Save = '[',
        Load = ']',
        Draw = 'F',
        TurnRight = '+',
        TurnLeft = '-',
    }

    [SerializeField] LSystemGenerator _lSystem;
    [SerializeField] GameObject _prefab;
    [SerializeField] Material _lineMat;
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
                    DrawLine(currentPos, tempPos,Color.red);
                    Length -= 2;             // 長さ -2 ?
                    posList.Add(currentPos);
                    break;
                case EncodingLetters.TurnRight:
                    // 戻り値はQuaternion型だがVector3をかける事でVector3型にできる
                    dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                    break;
                case EncodingLetters.TurnLeft:
                    dir = Quaternion.AngleAxis(-angle, Vector3.up) * dir;
                    break;
            }
        }

        foreach (Vector3 pos in posList)
        {
            Instantiate(_prefab, pos, Quaternion.identity);
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("line");
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.material = _lineMat;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
