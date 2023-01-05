using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダイアモンド・スクエア・アルゴリズム
/// </summary>
public class DSASystem : MonoBehaviour
{
    readonly int Side = 129;

    [SerializeField] GameObject[] _blocks;

    void Start()
    {
        // 2^n+1 * 2^n+1 のグリッドを作る
        int[,] array = new int[Side, Side];
        // 真ん中の高さを1にする
        array[Side / 2, Side / 2] = 1;

        // 中心からの移動量
        int move = 0;
        // 上下左右がマップの端にぶつかったら終了


        for (int i = 0; i < Side; i++)
        {
            for(int j = 0; j < Side; j++)
            {
                Instantiate(_blocks[0], new Vector3(j, i, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        
    }
}
