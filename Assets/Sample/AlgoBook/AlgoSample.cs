using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoSample : MonoBehaviour
{
    // ステージデータ
    struct StageData
    {
        // マップのサイズ:横×縦の要素を持つ配列
        int[] lndArr;
        // 街の座標:x,yの位置に街があるという配列
        Vector2[] twnArr;
        // マスがどの街に所属するか:各マスで判定するのでlndArrと同じ要素数を持つ
        int[] areaArr;
        // 登場する国の数
        int cntrySz;
        // 各国のデフォルトの街の配列:国の数分の長さ
        int[] cntryInitTwnArr;
        // 1街当たりの獲得軍資金
        int twnFund;
        // 各国の初期軍資金:国の数だけ
        int[] strtFundArr;
        // マップのイベント種類:何種類のイベントが発生するか？
        int mapEvnt;
        // マップの画像ID(実際のプレイには影響しないはず)
        int mapImgId;
    }

    // プレイデータ
    struct PlayData
    {
        // 各街の現状
        townData[] twnArr;
        // ユニットのデータ
        UnitData[] unitArr;
        // 建物のデータ
        BuildData[] bldArr;
    }

    // 街のデータの構造体
    struct townData
    {
        // 国番号
        int cntry;
        // 資金
        int fund;
        // 耐久度
        int hp;
        // 最大耐久度
        int hpMax;
    }

    // ユニットデータ
    struct UnitData
    {
        // ユニットの種類
        int untType;
        // 所属国
        int cntry;
        // 初期座標
        Vector2 pos;
        // 現在耐久力
        int hp;
        // 最大耐久力
        int maxHp;
        // 生成ステージ時間
        float geneTime;
        // 到着ステージ時間
        float goalTime;
        // 最後に移動したステージ時間
        float lstMvTime;
        // 最後に攻撃したステージ時間
        float lstAtkTime;
        // 現在のピクセル位置(画像を表示するために必要？)
        Vector2 pxPos;

        // 移動経路の配列
        // 生成時に移動経路を計算しており、移行はその経路に沿って移動する
        // 移動コスト表のキャッシュを持つことで計算量を削減している
        Vector2[] routeArr;
        // 経路参照位置:移動経路配列の何番にいるか(かもしれない)
        int routePos;
        // その経路参照位置になってからの時間の進捗:次の位置に移動するまでのカウント？
        float routePrgrs;
        // ユニットの画像の向き(2Dなので)
        int imgDrc;
        // 目標街
        int tgtTwn;
        // 目標街の座標
        Vector2 tgtTwnPos;
    }

    // 建物のデータ
    struct BuildData
    {
        // 建物の種類
        int bldType;
        // 所属国
        int cntry;
        // 座標
        Vector2 pos;
        // 耐久力
        int hp;
        // 最大耐久力
        int maxHp;
        // 強化HP
        int enhancedHp;
        // 強化射程
        int enhancedRng;
        // 生成されたステージ時間
        float genTime;
        // 最後にユニットを生産したステージ時間
        float lstActTime;
        // 最後に攻撃したステージ時間
        float lstAtkTime;
    }
}
