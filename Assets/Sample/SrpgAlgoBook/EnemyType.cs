using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    // ���ʂ̓G
    struct Normal
    {
        // ���G�͈�
        float range;
        // �Ώۂɍs���s���̌����̔{��
        float targetMag;
        // �����{���A�S�Ă̓G���j�b�g����Ȃ�ׂ����ꂽ�ʒu�Ɉړ����悤�Ƃ���
        float distMag;
    }

    // ���񂷂�G
    struct Patrol
    {
        // �����̔z��A������΋��_�h�q�����郆�j�b�g�ɂȂ�
        Vector2[] poses;
        // �A�Ҍ��E�A�G��[�ǂ����Ȃ��悤�ɂ���
        int returnLimit;
        // ����{���A�ǂꂾ������D�悷�邩
        float ptrlMag;
    }

    // ���Έʒu�Ɉړ�����G
    struct Reflection
    {
        // �Ώ̂���̑��Έʒu�A�I�t�Z�b�g�̗l�Ȃ���
        Vector2 pos;
        // �Ώۃ��j�b�g
        Transform target;
    }
}
