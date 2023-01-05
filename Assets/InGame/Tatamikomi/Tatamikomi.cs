using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���邮�鎩�������A���s��
/// </summary>
public class Tatamikomi : MonoBehaviour
{
    /// <summary>�n�`�������p�̍����̍ő�l</summary>
    readonly int MaxHeight = 100;
    
    /// �e�n�`�̍����̃{�[�_�[
    readonly int MountainBorder = 90;
    readonly int HillBorder = 70;
    readonly int ForestBorder = 50;
    readonly int GrassBorder = 30;
    readonly int SeaBorder = 20;
    readonly int DeepSeaBorder = 1;

    /// <summary>�Q������Ƀ��[�v����ۂ̕����̏���</summary>
    readonly Vector2Int[] dirs =
        {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down,
        };

    [Header("�}�b�v�̐ݒ�")]
    [SerializeField] int _height = 100;
    [SerializeField] int _width = 150;
    [Header("�����̌�����")]
    [SerializeField] int _decrease = 3;
    [Header("�n�`�̍����̊�ɂȂ钸�_��")]
    [SerializeField] int _vtxLength = 10;
    [Header("�n�`�摜")]
    [SerializeField] GameObject[] _blocks;

    void Start()
    {
        int[,] array = new int[_height, _width];

        // �S�Ă̒��_�𓯎��ɑ��삵�����̂ň�x�z��Ɋi�[����
        Vector2Int[] vertices = new Vector2Int[_vtxLength];
        for (int i = 0; i < _vtxLength; i++)
        {
            int rx = Random.Range(0, array.GetLength(1));
            int ry = Random.Range(0, array.GetLength(0));

            // ���_�̍��������߂Ă���
            array[ry, rx] = MaxHeight;
            vertices[i] = new Vector2Int(rx, ry);
        }

        // ���[�v���ɒႭ���Ă����̂ōő�l���Z�b�g���Ă���
        int baseHeight = MaxHeight;
        // �Q������ɒ��ׂ�ۂ�1�ӂ̒���
        // 2�Ӓ��ׂ閈�ɃC���N�������g�����
        int side = 1;

        // ���_���Q�Ƃ���̂ŁA���_�̍��W���R�s�[���ĕʓr���W�̔z���p�ӂ���
        Vector2Int[] posArr = new Vector2Int[_vtxLength];
        for (int i = 0; i < posArr.Length; i++)
            posArr[i] = new Vector2Int(vertices[i].x, vertices[i].y);

        int c = 0;

        while (true)
        {
            // �������1�������ꍇ�ɍ�����ύX������
            // ���ꂪ0�̏ꍇ�͂���������ύX����ӏ����c���Ă��Ȃ��Ƃ݂Ȃ�
            // ���[�v���甲����B
            // �������A��ł������̕ύX������ӏ�������΂��̉ӏ��̂��߂�����
            // �S���[�v������Ă��܂��̂Ō����͔��Ɉ���
            int count = 0;

            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < side; k++)
                {
                    // �e���_�������Ői�߂鎖�ŁA���E���̈�a��������
                    for (int l = 0; l < posArr.Length; l++)
                    {
                        int vx = posArr[l].x + dirs[j].x;
                        int vy = posArr[l].y + dirs[j].y;

                        // ���삷����W���}�b�v�̔z����Ɏ��܂��Ă���
                        // ���삷����W�͏����l�ł����
                        if (0 <= vx && vx < array.GetLength(1) &&
                            0 <= vy && vy < array.GetLength(0) &&
                            array[vy, vx] == 0)
                        {
                            array[vy, vx] = baseHeight;
                            count++;
                        }

                        // �����̕ύX���s���Ă��Ȃ��ꍇ�ł����̍��W�֐i��
                        posArr[l].x = vx;
                        posArr[l].y = vy;
                    }
                }

                // 2�ӂɑ΂��ď����������̂ŕӂ̒������C���N�������g����
                if (j == 1 || j == 3) side++;
            }

            // ���������炷
            baseHeight -= _decrease;
            baseHeight = Mathf.Max(1, baseHeight);

            if (count == 0) break;
        }

        // �z���Sprite�ɕϊ�����
        for (int i = 0; i < array.GetLength(0); i++)
            for (int j = 0; j < array.GetLength(1); j++)
            {
                int h = array[i, j];
                int index = 5;

                if      (MountainBorder <= h) index = 0; // �R
                else if (HillBorder <= h)     index = 1; // �u
                else if (ForestBorder <= h)   index = 2; // �X
                else if (GrassBorder <= h)    index = 3; // ����
                else if (SeaBorder <= h)      index = 4; // �C(��)
                else if (DeepSeaBorder <= h)  index = 5; // �C(�[��)

                GameObject go = Instantiate(_blocks[index], new Vector2(j, i), Quaternion.identity);
                go.isStatic = true;
            }
    }
}
