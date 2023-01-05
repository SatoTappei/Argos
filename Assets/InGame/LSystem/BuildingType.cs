using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingType
{
    [SerializeField] GameObject[] _prefabs;
    // �ݒu�ɕK�v�ȃT�C�Y
    [SerializeField] int _sizeRequired;
    // �ݒu�\�ȗʁH
    // (BuildingTyp�N���X�̔z��̈�ԍŌ�ɂ͂��ꂪ-1�̃C���X�^���X��z�u���Ȃ��Ă͂����Ȃ�(EOF�I�ȁH)
    [SerializeField] int _quantity = -1;
    // ���ɐݒu������
    [SerializeField] int _quantityAlreadyPlaced; 

    public int SizeRequired => _sizeRequired;
    public int Quantity => _quantity;
    public int QuantityAlreadyPlaced => _quantityAlreadyPlaced;

    public GameObject GetPrefab()
    {
        // ���̃��\�b�h���ĂԂ̂͐ݒu����ꍇ�Ȃ̂�
        // �����Őݒu���������C���N�������g����
        _quantityAlreadyPlaced++;

        if (_prefabs.Length > 1)
        {
            int r = UnityEngine.Random.Range(0, _prefabs.Length);
            return _prefabs[r];
        }
        return _prefabs[0];
    }

    /// <returns>�܂��ݒu�\��</returns>
    public bool IsBuildingAvailable()
    {
        return _quantityAlreadyPlaced < _quantity;
    }

    public void Reset()
    {
        _quantityAlreadyPlaced = 0;
    }
}
