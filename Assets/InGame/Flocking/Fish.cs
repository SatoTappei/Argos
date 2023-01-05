using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ꂽ�����𑀍삷��R���|�[�l���g
/// </summary>
public class Fish : MonoBehaviour
{
    readonly float JustBefore = 1.0f;

    [Header("���̐ݒ�")]
    [Range(1.0f, 50.0f)]
    [SerializeField] float _minSpeed;
    [Range(1.0f, 50.0f)]
    [SerializeField] float _maxSpeed;
    [Range(1.0f, 100.0f)]
    [SerializeField] float _range;
    [Range(0.0f, 50.0f)]
    [SerializeField] float _rotSpeed;

    Fish[] _fishes;
    float _speed;

    void Start()
    {
        // Awake�ł̐����݂̂ɑΉ����Ă���̂Œǉ��Ő����Ƃ����Ȃ����ƁB
        _fishes = FindObjectOfType<FlockingManager>().Fishes;

        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    void Update()
    {
        // �O���Ɉړ�������
        transform.Translate(0, 0, Time.deltaTime * _speed);

        // �Q��̒��S���W
        Vector3 groupCenter = Vector3.zero;
        Vector3 AvoidVec = Vector3.zero;
        float groupoSpeed = 0;
        int groupSize = 0;

        // �������ꂽ�S�Ă̋��ɑ΂��Ĕ��肷��
        foreach(Fish fish in _fishes)
        {
            // ���g�Ƃ̔���͒[�܂�
            if (fish == this) continue;
            // ���g�ƈ��ȏ㗣��Ă��鋛�͌Q��ł͂Ȃ��̂Œ[�܂�
            float dist = (fish.transform.position - transform.position).sqrMagnitude;
            if (dist > _range) continue;

            // ��ɌQ��̒��S���W���v�Z���邽�߂Ɏ��g�̍��W�����Z����
            groupCenter += fish.transform.position;
            // �Q��̃T�C�Y�𑝂₷
            groupSize++;

            // ���g���Q��̑��̋��ƂԂ��钼�O�Ȃ�
            // �Q��̉���x�N�g���ɑΏۂ̋��Ƌt�����̃x�N�g�������Z����
            if (dist < JustBefore)
                AvoidVec += (transform.position - fish.transform.position);

            // �Q��̕��ϑ��x���v�Z���邽�߂ɑΏۂ̋��̑��x�����Z����
            groupoSpeed += fish._speed;
        }

        // �Q��̃T�C�Y��1�ȏ�Ȃ�
        if (groupSize > 0)
        {
            // �Q��̒��S���W���v�Z����
            groupCenter /= groupSize;
            // ���g�̑��x���Q��̕��ϑ��x�ɍ��킹��
            _speed = groupoSpeed / groupSize;

            // �Q��̒��S���W�ɌQ��̉���x�N�g���𑫂������W�Ɍ������x�N�g�����v�Z����
            Vector3 dir = groupCenter + AvoidVec - transform.position;
            if (dir != Vector3.zero)
            {
                // ������]������
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                      Quaternion.LookRotation(dir), 
                                                      _rotSpeed * Time.deltaTime);
            }
        }
    }
}
