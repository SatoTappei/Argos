using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w���p�[�N���X
/// ���������������Ă���_�ł̓��[�e�B���e�B�N���X�Ɠ�������
/// ������̓I�u�W�F�N�g�Ƃ��Ĉ����̂ł��I�u�W�F�N�g�w���I�c�炵��
/// </summary>
public class AgentParams
{
    [SerializeField] Vector3 _pos;
    [SerializeField] Vector3 _dir;
    [SerializeField] int _length;

    public Vector3 Pos { get => _pos; set => _pos = value; }
    public Vector3 Dir { get => _dir; set => _dir = value; }
    public int Length { get => _length; set => _length = value; }
}
