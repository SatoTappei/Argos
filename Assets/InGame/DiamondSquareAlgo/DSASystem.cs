using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_�C�A�����h�E�X�N�G�A�E�A���S���Y��
/// </summary>
public class DSASystem : MonoBehaviour
{
    readonly int Side = 129;

    [SerializeField] GameObject[] _blocks;

    void Start()
    {
        // 2^n+1 * 2^n+1 �̃O���b�h�����
        int[,] array = new int[Side, Side];
        // �^�񒆂̍�����1�ɂ���
        array[Side / 2, Side / 2] = 1;

        // ���S����̈ړ���
        int move = 0;
        // �㉺���E���}�b�v�̒[�ɂԂ�������I��


        for (int i = 0; i < Side; i++)
        {
            for(int j = 0; j < Side; j++)
            {
                Instantiate(_blocks[0], new Vector3(j, i, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
