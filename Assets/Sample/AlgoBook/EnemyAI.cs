using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // �GAI�̎v�l���@
    // �����̊X�𒆐S�ɂ��ĉQ������Ɍ�����z�u���Ă���
    // 4�T�ڈڍs�͓����z�u���J��Ԃ�
    // �e�X�͎����̗̒n���ɂ���G���j�b�g�𐔂��ď��Ȃ���΍U���n�̌����𑽂���Ζh��n�̌��������Ă�
    
    // �Q������Ƀ��[�v������
    // �T���I������
        // 1�����T���������ʁA�S�Ẵ}�X���G���A�O�̏ꍇ
        // 1�����T���������ʁA�z�u�\�}�X������(�R�����Ȃǂ̔z�u�R�X�g���s��)�ꍇ
    void Uzumaki()
    {
        int w = 11;
        int h = 8;
        // �����ƕ��ő傫����
        int sz = Mathf.Max(w, h);
        // �ӂ̐�
        int side = 4;
        // �e�ӂ̒���
        int sideLength = 0;

        // �}�b�v�̑傫�����J��Ԃ�
        for(int i = 0; i < sz; i++)
        {
            // �e�ӂ̍ő�����𑝂₷
            sideLength += 2;

            // �l�p�`�Ȃ̂�4��J��Ԃ�
            for (int j = 0; j < side; j++)
            {
                // 1�ӂ�1�}�X�ɑ΂��ď���������
                for (int k = 0; k < sideLength; k++)
                {

                }
            }
        }
    }
}
