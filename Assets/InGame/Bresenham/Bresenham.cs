using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ブレゼンハムアルゴリズム
/// ある直線をはさみ込むようにして存在する2つの点のうちどちらが直線に近いかで打つ点を判定するアルゴリズム
/// </summary>
public class Bresenham : MonoBehaviour
{
    [SerializeField] GameObject _block;
    [Header("2点を指定(第1象限、傾き <= 1)")]
    [SerializeField] Vector2Int _pos1 = new Vector2Int(0, 0);
    [SerializeField] Vector2Int _pos2 = new Vector2Int(29,6);

    void Start()
    {
        /* 解説 */
        // 本来の線の近似である必要がある
        // 整数値しかとれないので例えば点xにおける本来のyが1.4だとしたら
        // 2もしくは1に点の候補がある。
        // 2点の中間点 f(x0+1, y0+1/2) の値で次に横いくか斜めいくかがきまる
        // 式:D = [A(x0+1) + B(y0+1/2) + C] - [Ax0 + By0 + C]
        //    D = A + 1/2B
        // 小数部分を消すために全部2倍すると
        // 2D = 2A + B

        int dx = _pos2.x - _pos1.x; // Δx
        int dy = _pos2.y - _pos1.y; // Δy
        // 現在のyの値、必ず整数になる
        int y = _pos1.y;         
        // 誤差
        float err = 0;
        // 傾き
        float m = Mathf.Abs(dy / (1.0f * dx)); 

        for (int x = _pos1.x; x <= _pos2.x; x++)
        {
            // 誤差が0.5以上なら斜め方向に伸ばす
            if (err >= 0.5f)
            {
                // 小数点の部分だけあればいいので1を引く
                err--;

                y++;
            }

            // 誤差に傾きを足し込んでいく
            err += m;
            
            Instantiate(_block, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
