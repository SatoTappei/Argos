using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成されたお魚を操作するコンポーネント
/// </summary>
public class Fish : MonoBehaviour
{
    readonly float JustBefore = 1.0f;

    [Header("魚の設定")]
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
        // Awakeでの生成のみに対応しているので追加で生成とかしないこと。
        _fishes = FindObjectOfType<FlockingManager>().Fishes;

        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    void Update()
    {
        // 前方に移動させる
        transform.Translate(0, 0, Time.deltaTime * _speed);

        // 群れの中心座標
        Vector3 groupCenter = Vector3.zero;
        Vector3 AvoidVec = Vector3.zero;
        float groupoSpeed = 0;
        int groupSize = 0;

        // 生成された全ての魚に対して判定する
        foreach(Fish fish in _fishes)
        {
            // 自身との判定は端折る
            if (fish == this) continue;
            // 自身と一定以上離れている魚は群れではないので端折る
            float dist = (fish.transform.position - transform.position).sqrMagnitude;
            if (dist > _range) continue;

            // 後に群れの中心座標を計算するために自身の座標を加算する
            groupCenter += fish.transform.position;
            // 群れのサイズを増やす
            groupSize++;

            // 自身が群れの他の魚とぶつかる直前なら
            // 群れの回避ベクトルに対象の魚と逆向きのベクトルを加算する
            if (dist < JustBefore)
                AvoidVec += (transform.position - fish.transform.position);

            // 群れの平均速度を計算するために対象の魚の速度を加算する
            groupoSpeed += fish._speed;
        }

        // 群れのサイズが1以上なら
        if (groupSize > 0)
        {
            // 群れの中心座標を計算する
            groupCenter /= groupSize;
            // 自身の速度を群れの平均速度に合わせる
            _speed = groupoSpeed / groupSize;

            // 群れの中心座標に群れの回避ベクトルを足した座標に向かうベクトルを計算する
            Vector3 dir = groupCenter + AvoidVec - transform.position;
            if (dir != Vector3.zero)
            {
                // 魚を回転させる
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                      Quaternion.LookRotation(dir), 
                                                      _rotSpeed * Time.deltaTime);
            }
        }
    }
}
