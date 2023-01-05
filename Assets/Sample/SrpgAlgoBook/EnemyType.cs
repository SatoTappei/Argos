using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    // 普通の敵
    struct Normal
    {
        // 索敵範囲
        float range;
        // 対象に行う行動の効率の倍率
        float targetMag;
        // 距離倍率、全ての敵ユニットからなるべく離れた位置に移動しようとする
        float distMag;
    }

    // 巡回する敵
    struct Patrol
    {
        // 巡回先の配列、無ければ拠点防衛をするユニットになる
        Vector2[] poses;
        // 帰還限界、敵を深追いしないようにする
        int returnLimit;
        // 巡回倍率、どれだけ巡回優先するか
        float ptrlMag;
    }

    // 相対位置に移動する敵
    struct Reflection
    {
        // 対称からの相対位置、オフセットの様なもの
        Vector2 pos;
        // 対象ユニット
        Transform target;
    }
}
