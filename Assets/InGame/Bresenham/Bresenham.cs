using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �u���[���n���A���S���Y��
/// ���钼�����͂��ݍ��ނ悤�ɂ��đ��݂���2�̓_�̂����ǂ��炪�����ɋ߂����őł_�𔻒肷��A���S���Y��
/// </summary>
public class Bresenham : MonoBehaviour
{
    [SerializeField] GameObject _block;
    [Header("2�_���w��(��1�ی��A�X�� <= 1)")]
    [SerializeField] Vector2Int _pos1 = new Vector2Int(0, 0);
    [SerializeField] Vector2Int _pos2 = new Vector2Int(29,6);

    void Start()
    {
        /* ��� */
        // �{���̐��̋ߎ��ł���K�v������
        // �����l�����Ƃ�Ȃ��̂ŗႦ�Γ_x�ɂ�����{����y��1.4���Ƃ�����
        // 2��������1�ɓ_�̌�₪����B
        // 2�_�̒��ԓ_ f(x0+1, y0+1/2) �̒l�Ŏ��ɉ��������΂߂����������܂�
        // ��:D = [A(x0+1) + B(y0+1/2) + C] - [Ax0 + By0 + C]
        //    D = A + 1/2B
        // �����������������߂ɑS��2�{�����
        // 2D = 2A + B

        int dx = _pos2.x - _pos1.x; // ��x
        int dy = _pos2.y - _pos1.y; // ��y
        // ���݂�y�̒l�A�K�������ɂȂ�
        int y = _pos1.y;         
        // �덷
        float err = 0;
        // �X��
        float m = Mathf.Abs(dy / (1.0f * dx)); 

        for (int x = _pos1.x; x <= _pos2.x; x++)
        {
            // �덷��0.5�ȏ�Ȃ�΂ߕ����ɐL�΂�
            if (err >= 0.5f)
            {
                // �����_�̕�����������΂����̂�1������
                err--;

                y++;
            }

            // �덷�ɌX���𑫂�����ł���
            err += m;
            
            Instantiate(_block, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
