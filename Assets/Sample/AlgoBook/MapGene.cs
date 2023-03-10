using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGene : MonoBehaviour
{
    // マップ生成用パラメータ
    struct GenMap
    {
        // 土地を生成する際の基準点の数
        int lndPnt;
        // 高さの範囲(高い土地の閾値,普通の土地の閾値)
        (int h, int m) heightRng;
        // 各地形(森、山、平地、水上)の強度(強度とは？)
        int frst;
        int mntn;
        int pln;
        int wtr;
    }

    // 土地の生成 => 街の生成 => エリア分け

    // 土地の生成…山、森、平地、水上
    // 街の生成…あまり近づき過ぎないように配置
    // エリア分け…街の位置に応じて各マスがどの街に所属するか

    // 1.土地の生成
    // 土地を生成する際の基準点の数だけループを回す
    // 基準となる座標を乱数で決定する
    // その座標の高さを乱数で決定する
        // 高さが0~heightRng.hの場合は森or山
        // 高さがheightRng.h + 1 ~ heightRng.mの場合は平地
        // 高さがそれ以下なら水上になる
    // 不明:基準点から周囲のマスを広げていく方法
    //      四方に伸ばす？とは何か

    // 2.街の生成
    // 力任せ実装
    // ランダムな座標に街を作る
    // 近くに街が存在したらやり直し
    // 無限ループ対策で必ずしも最大数の街が生成されるとは限らない

    // 3.エリア分け
    // マップの全マスと街の座標を使って全てのマスに対する総当たりでどの街が一番近いか調べる
}
