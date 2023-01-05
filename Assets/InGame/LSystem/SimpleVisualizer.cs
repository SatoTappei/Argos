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
        // �ŏ���1�����������鉻����
        string sequence = _lSystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    void VisualizeSequence(string sequence)
    {
        // �X��`�悷����W�Ƃ��Ďg��
        Stack<AgentParams> savePoints = new Stack<AgentParams>();
        Vector3 currentPos = Vector3.zero;

        // ���H��`�悷��̂Ɏg��
        Vector3 dir = Vector3.forward;
        Vector3 tempPos = Vector3.zero;

        // ���_��stack�Ƀv�b�V�����Ă���͍̂ŏ��̃G�[�W�F���g�̍ŏ��̃|�C���g�����_������H
        posList.Add(currentPos);

        // ��������𑖍�����
        foreach (char letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.Save:
                    // �I�u�W�F�N�g�������q���g�p����������
                    // �R���X�g���N�^���Ăяo�����A�e�v���p�e�B�ɏ����l��ݒ肵�Ă���
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
                        throw new System.Exception("�X�^�b�N���ɃZ�[�u���ꂽ�|�C���g������܂���");
                    }
                    break;
                case EncodingLetters.Draw:
                    tempPos = currentPos;
                    currentPos += dir * length;
                    DrawLine(currentPos, tempPos,Color.red);
                    Length -= 2;             // ���� -2 ?
                    posList.Add(currentPos);
                    break;
                case EncodingLetters.TurnRight:
                    // �߂�l��Quaternion�^����Vector3�������鎖��Vector3�^�ɂł���
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
