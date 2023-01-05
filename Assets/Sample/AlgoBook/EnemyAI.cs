using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // 敵AIの思考方法
    // 自分の街を中心にして渦巻き状に建物を配置していく
    // 4週目移行は同じ配置を繰り返す
    // 各街は自分の領地内にいる敵ユニットを数えて少なければ攻撃系の建物を多ければ防御系の建物を建てる
    
    // 渦巻き状にループさせる
    // 探索終了条件
        // 1周分探索した結果、全てのマスがエリア外の場合
        // 1周分探索した結果、配置可能マスが無い(軍資金などの配置コストが不足)場合
    void Uzumaki()
    {
        int w = 11;
        int h = 8;
        // 高さと幅で大きい方
        int sz = Mathf.Max(w, h);
        // 辺の数
        int side = 4;
        // 各辺の長さ
        int sideLength = 0;

        // マップの大きさ分繰り返す
        for(int i = 0; i < sz; i++)
        {
            // 各辺の最大歩数を増やす
            sideLength += 2;

            // 四角形なので4回繰り返す
            for (int j = 0; j < side; j++)
            {
                // 1辺の1マスに対して処理をする
                for (int k = 0; k < sideLength; k++)
                {

                }
            }
        }
    }
}
