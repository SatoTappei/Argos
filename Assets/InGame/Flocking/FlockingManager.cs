using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// お魚を管理するマネージャー
/// </summary>
public class FlockingManager : MonoBehaviour
{
    [SerializeField] Fish _prefab;
    [SerializeField] int _amount = 20;
    [Header("生成範囲")]
    [SerializeField] Vector3 _range = new Vector3(5, 5, 5);

    /// <summary>
    /// 生成した魚を格納しておく配列
    /// 魚側はStart()でこれを参照する
    /// </summary>
    Fish[] _fishes;

    public Fish[] Fishes { get => _fishes; }

    void Awake()
    {
        _fishes = new Fish[_amount];
        for (int i = 0; i < _amount; i++)
        {
            // このオブジェクトを中心に指定された範囲内にランダム生成する
            Vector3 pos = transform.position + new Vector3(Random.Range(-_range.x, _range.x),
                                                           Random.Range(-_range.y, _range.y),
                                                           Random.Range(-_range.z, _range.z));
            _fishes[i] = Instantiate(_prefab, pos, Quaternion.identity);
        }
    }
}
