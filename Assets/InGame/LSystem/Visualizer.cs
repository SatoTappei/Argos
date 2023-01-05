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
                    _roadHelper.PlaceStreetPos(Vector3Int.RoundToInt(tempPos), 
                                               Vector3Int.RoundToInt(dir), 
                                               length);
                    Length -= 2;             
                    posList.Add(currentPos);
                    break;
                case EncodingLetters.TurnRight:
                    // �߂�l��Quaternion�^����Vector3�������鎖��Vector3�^�ɂł���
                    // �p�x��������̃x�N�g��(1,0,0)�Ƃ�(0,1,0)�Ƃ������܂�H
                    dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
                    break;
                case EncodingLetters.TurnLeft:
                    dir = Quaternion.AngleAxis(-angle, Vector3.up) * dir;
                    break;
            }
        }
        _roadHelper.FixRoad();
        // �N���XA�̃��\�b�h�ɃN���XB�̃��\�b�h�̖߂�l�������ɂ��Ă���
        // �����(Presenter)�I��
        _structureHelper.PlaceStructureAroundRoad(_roadHelper.GetRoadPos());
    }
}
