using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoSample : MonoBehaviour
{
    // �X�e�[�W�f�[�^
    struct StageData
    {
        // �}�b�v�̃T�C�Y:���~�c�̗v�f�����z��
        int[] lndArr;
        // �X�̍��W:x,y�̈ʒu�ɊX������Ƃ����z��
        Vector2[] twnArr;
        // �}�X���ǂ̊X�ɏ������邩:�e�}�X�Ŕ��肷��̂�lndArr�Ɠ����v�f��������
        int[] areaArr;
        // �o�ꂷ�鍑�̐�
        int cntrySz;
        // �e���̃f�t�H���g�̊X�̔z��:���̐����̒���
        int[] cntryInitTwnArr;
        // 1�X������̊l���R����
        int twnFund;
        // �e���̏����R����:���̐�����
        int[] strtFundArr;
        // �}�b�v�̃C�x���g���:����ނ̃C�x���g���������邩�H
        int mapEvnt;
        // �}�b�v�̉摜ID(���ۂ̃v���C�ɂ͉e�����Ȃ��͂�)
        int mapImgId;
    }

    // �v���C�f�[�^
    struct PlayData
    {
        // �e�X�̌���
        townData[] twnArr;
        // ���j�b�g�̃f�[�^
        UnitData[] unitArr;
        // �����̃f�[�^
        BuildData[] bldArr;
    }

    // �X�̃f�[�^�̍\����
    struct townData
    {
        // ���ԍ�
        int cntry;
        // ����
        int fund;
        // �ϋv�x
        int hp;
        // �ő�ϋv�x
        int hpMax;
    }

    // ���j�b�g�f�[�^
    struct UnitData
    {
        // ���j�b�g�̎��
        int untType;
        // ������
        int cntry;
        // �������W
        Vector2 pos;
        // ���ݑϋv��
        int hp;
        // �ő�ϋv��
        int maxHp;
        // �����X�e�[�W����
        float geneTime;
        // �����X�e�[�W����
        float goalTime;
        // �Ō�Ɉړ������X�e�[�W����
        float lstMvTime;
        // �Ō�ɍU�������X�e�[�W����
        float lstAtkTime;
        // ���݂̃s�N�Z���ʒu(�摜��\�����邽�߂ɕK�v�H)
        Vector2 pxPos;

        // �ړ��o�H�̔z��
        // �������Ɉړ��o�H���v�Z���Ă���A�ڍs�͂��̌o�H�ɉ����Ĉړ�����
        // �ړ��R�X�g�\�̃L���b�V���������ƂŌv�Z�ʂ��팸���Ă���
        Vector2[] routeArr;
        // �o�H�Q�ƈʒu:�ړ��o�H�z��̉��Ԃɂ��邩(��������Ȃ�)
        int routePos;
        // ���̌o�H�Q�ƈʒu�ɂȂ��Ă���̎��Ԃ̐i��:���̈ʒu�Ɉړ�����܂ł̃J�E���g�H
        float routePrgrs;
        // ���j�b�g�̉摜�̌���(2D�Ȃ̂�)
        int imgDrc;
        // �ڕW�X
        int tgtTwn;
        // �ڕW�X�̍��W
        Vector2 tgtTwnPos;
    }

    // �����̃f�[�^
    struct BuildData
    {
        // �����̎��
        int bldType;
        // ������
        int cntry;
        // ���W
        Vector2 pos;
        // �ϋv��
        int hp;
        // �ő�ϋv��
        int maxHp;
        // ����HP
        int enhancedHp;
        // �����˒�
        int enhancedRng;
        // �������ꂽ�X�e�[�W����
        float genTime;
        // �Ō�Ƀ��j�b�g�𐶎Y�����X�e�[�W����
        float lstActTime;
        // �Ō�ɍU�������X�e�[�W����
        float lstAtkTime;
    }
}
