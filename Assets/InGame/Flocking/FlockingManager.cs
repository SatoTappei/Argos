using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������Ǘ�����}�l�[�W���[
/// </summary>
public class FlockingManager : MonoBehaviour
{
    [SerializeField] Fish _prefab;
    [SerializeField] int _amount = 20;
    [Header("�����͈�")]
    [SerializeField] Vector3 _range = new Vector3(5, 5, 5);

    /// <summary>
    /// �������������i�[���Ă����z��
    /// ������Start()�ł�����Q�Ƃ���
    /// </summary>
    Fish[] _fishes;

    public Fish[] Fishes { get => _fishes; }

    void Awake()
    {
        _fishes = new Fish[_amount];
        for (int i = 0; i < _amount; i++)
        {
            // ���̃I�u�W�F�N�g�𒆐S�Ɏw�肳�ꂽ�͈͓��Ƀ����_����������
            Vector3 pos = transform.position + new Vector3(Random.Range(-_range.x, _range.x),
                                                           Random.Range(-_range.y, _range.y),
                                                           Random.Range(-_range.z, _range.z));
            _fishes[i] = Instantiate(_prefab, pos, Quaternion.identity);
        }
    }
}
