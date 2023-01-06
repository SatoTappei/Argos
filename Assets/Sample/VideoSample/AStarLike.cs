using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarLike
{
    struct Node
    {
        Vector2Int pos;
        int fCost;

        public Node(int x, int y, int f)
        {
            pos = new Vector2Int(x, y);
            fCost = f;
        }

        public Vector2Int Pos => pos;
        public int FCost => fCost;
    }

    // �J����/�����m�[�h�̃��X�g
    List<Node> _openList = new List<Node>();
    List<Node> _closeList = new List<Node>();

    void Proc(Vector2Int target)
    {
        // �ŏ��̈ʒu�̃m�[�h���쐬����
        Node start = new Node(0, 0, 0);
        // �J�����m�[�h�̃��X�g�ɒǉ�����
        _openList.Add(start);

        while (true)
        {
            // F�R�X�g����ԒႢ�m�[�h��I��
            Node current = _openList.OrderBy(n => n.FCost).FirstOrDefault();
            // �J�����m�[�h�̃��X�g����폜
            _openList.Remove(current);
            // �����m�[�h�̃��X�g�ɒǉ�
            _closeList.Add(current);

            // �������Ă����珈���𔲂���
            if (target == current.Pos) return;

            // current�̎���8�}�X�𒲂ׂ�
            for (int i = 0; i < 8; i++)
            {
                // ���̃}�X���ړ��s�\�������͕����m�[�h�̃��X�g�Ɋ܂܂�Ă�����
                bool canMove = true;

                if (canMove)
                {
                    continue;
                }
                // �ʂ��}�X�Ȃ�
                else
                {
                    // �R�X�g���Ⴂ�}�X�������͂܂��J���ĂȂ��m�[�h�Ȃ�
                    bool isLow = true;
                    if (isLow)
                    {
                        // F�R�X�g��ݒ肷��
                        // �H�邽�߂ɗׂ̃m�[�h�̐e�Ɏ��g��ݒ肷��

                        // ���̃m�[�h�����ɊJ�����m�[�h�̃��X�g�Ɋ܂܂�Ă��Ȃ�������
                        bool isOpen = false;
                        if (isOpen)
                        {
                            // �J�����m�[�h�̃��X�g�ɒǉ�����
                        }
                    }
                }
            }
        }
    }
}
