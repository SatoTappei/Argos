using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingType
{
    [SerializeField] GameObject[] _prefabs;
    // 設置に必要なサイズ
    [SerializeField] int _sizeRequired;
    // 設置可能な量？
    // (BuildingTypクラスの配列の一番最後にはこれが-1のインスタンスを配置しなくてはいけない(EOF的な？)
    [SerializeField] int _quantity = -1;
    // 既に設置した量
    [SerializeField] int _quantityAlreadyPlaced; 

    public int SizeRequired => _sizeRequired;
    public int Quantity => _quantity;
    public int QuantityAlreadyPlaced => _quantityAlreadyPlaced;

    public GameObject GetPrefab()
    {
        // このメソッドを呼ぶのは設置する場合なので
        // ここで設置した数をインクリメントする
        _quantityAlreadyPlaced++;

        if (_prefabs.Length > 1)
        {
            int r = UnityEngine.Random.Range(0, _prefabs.Length);
            return _prefabs[r];
        }
        return _prefabs[0];
    }

    /// <returns>まだ設置可能か</returns>
    public bool IsBuildingAvailable()
    {
        return _quantityAlreadyPlaced < _quantity;
    }

    public void Reset()
    {
        _quantityAlreadyPlaced = 0;
    }
}
