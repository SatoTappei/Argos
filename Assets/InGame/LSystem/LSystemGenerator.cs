using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LSystemGenerator : MonoBehaviour
{
    // �ċA�I�ɓ��H��L�΂��p�x��90�x�ɂ��邱�ƂŊX���ۂ��Ȃ�
    // �����̒u�������̃��[����K�p������J��Ԃ�Algorithm
    // �t���N�^���\��(�����悤�Ȍ`���J��Ԃ����)�ł���B
    // [F]--F
    // l[+F][-F] Save,�E������,�O�ɐi��,�߂�,�Z�[�u,��������,�O�ɐi��,�߂�
    // [�cSave ]�cLoad ���W�Ɖ�]��ۑ�

    [SerializeField] Rule[] _rules;
    [SerializeField] string _rootSentence;
    [Range(0, 10)]
    [SerializeField] int _iterationLimit = 1;
    [SerializeField] bool _randomIgnore = true;
    [Range(0, 1)]
    [SerializeField] float _chanceToIgnore = 0.3f;

    void Start()
    {
        // Sentence�������\�b�h���Ă�Ō��ʂ����O�ɕ\��
        Debug.Log(GenerateSentence());
    }

    // �����̕�������Z���e���X�𐶐����ĕԂ�
    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = _rootSentence;
        }
        // �����̕������ċA�I�ɐ���������
        return GrowRecursive(word);
    }

    /// <summary>�����̕������ċA�I�ɐ��������郁�\�b�h</summary>
    string GrowRecursive(string word, int iterationIndex = 0)
    {
        // �����𒴂����炱��ȏ㐬�������Ȃ��ŕ������Ԃ�
        if (iterationIndex >= _iterationLimit)
        {
            return word;
        }

        // �����̕�����ɑ΂��čċA�I�ȏ���
        // ����foreach�̒��g���ċA�I�ɏ��������
        StringBuilder builder = new StringBuilder();
        foreach (char c in word)
        {
            // ������r���_�[�ɒǉ����ă��[����K�p���ď�������
            builder.Append(c);
            ProcessRulesRecursivelly(builder, c, iterationIndex);
        }

        // �ċA������ɕ�����ɂ��ĕԂ�
        return builder.ToString();
    }

    /// <summary>
    /// �ċA����������
    /// </summary>
    /// <param name="builder">������r���_�[��n��(�ˑ����̒���)</param>
    /// <param name="c"></param>
    /// <param name="iterationIndex"></param>
    void ProcessRulesRecursivelly(StringBuilder builder, char c, int iterationIndex)
    {
        // �S���[����K�p����
        foreach (Rule rule in _rules)
        {
            // �����̕��������[���K�p�����ƈ�v���Ă�����
            if (rule.Letter == c.ToString())
            {
                // �����_�����𖳎�����t���O�������Ă���ꍇ
                // �J��Ԃ��񐔂�1�ȏ�̏ꍇ
                // �m���ł���ȍ~�̃G�b�W�̐���������Ȃ�
                if (_randomIgnore && iterationIndex > 1)
                {
                    if (Random.value < _chanceToIgnore)
                    {
                        return;
                    }
                }

                // ������ɒǉ�����(�ċA����)
                // ���[���ɂ���Ēu��������������A�Y�����ԍ�+1
                builder.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
            }
        }
    }
}
