using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ヘルパークラス
/// 処理だけを持っている点ではユーティリティクラスと同じだが
/// こちらはオブジェクトとして扱うのでよりオブジェクト指向的…らしい
/// </summary>
public class AgentParams
{
    [SerializeField] Vector3 _pos;
    [SerializeField] Vector3 _dir;
    [SerializeField] int _length;

    public Vector3 Pos { get => _pos; set => _pos = value; }
    public Vector3 Dir { get => _dir; set => _dir = value; }
    public int Length { get => _length; set => _length = value; }
}
