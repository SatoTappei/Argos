using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGene : MonoBehaviour
{
    // �}�b�v�����p�p�����[�^
    struct GenMap
    {
        // �y�n�𐶐�����ۂ̊�_�̐�
        int lndPnt;
        // �����͈̔�(�����y�n��臒l,���ʂ̓y�n��臒l)
        (int h, int m) heightRng;
        // �e�n�`(�X�A�R�A���n�A����)�̋��x(���x�Ƃ́H)
        int frst;
        int mntn;
        int pln;
        int wtr;
    }

    // �y�n�̐��� => �X�̐��� => �G���A����

    // �y�n�̐����c�R�A�X�A���n�A����
    // �X�̐����c���܂�߂Â��߂��Ȃ��悤�ɔz�u
    // �G���A�����c�X�̈ʒu�ɉ����Ċe�}�X���ǂ̊X�ɏ������邩

    // 1.�y�n�̐���
    // �y�n�𐶐�����ۂ̊�_�̐��������[�v����
    // ��ƂȂ���W�𗐐��Ō��肷��
    // ���̍��W�̍����𗐐��Ō��肷��
        // ������0~heightRng.h�̏ꍇ�͐Xor�R
        // ������heightRng.h + 1 ~ heightRng.m�̏ꍇ�͕��n
        // ����������ȉ��Ȃ琅��ɂȂ�
    // �s��:��_������͂̃}�X���L���Ă������@
    //      �l���ɐL�΂��H�Ƃ͉���

    // 2.�X�̐���
    // �͔C������
    // �����_���ȍ��W�ɊX�����
    // �߂��ɊX�����݂������蒼��
    // �������[�v�΍�ŕK�������ő吔�̊X�����������Ƃ͌���Ȃ�

    // 3.�G���A����
    // �}�b�v�̑S�}�X�ƊX�̍��W���g���đS�Ẵ}�X�ɑ΂��鑍������łǂ̊X����ԋ߂������ׂ�
}
