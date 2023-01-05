using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ぐるぐる自動生成、失敗作
/// </summary>
public class Tatamikomi : MonoBehaviour
{
    /// <summary>地形成生成用の高さの最大値</summary>
    readonly int MaxHeight = 100;
    
    /// 各地形の高さのボーダー
    readonly int MountainBorder = 90;
    readonly int HillBorder = 70;
    readonly int ForestBorder = 50;
    readonly int GrassBorder = 30;
    readonly int SeaBorder = 20;
    readonly int DeepSeaBorder = 1;

    /// <summary>渦巻き状にループする際の方向の順番</summary>
    readonly Vector2Int[] dirs =
        {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down,
        };

    [Header("マップの設定")]
    [SerializeField] int _height = 100;
    [SerializeField] int _width = 150;
    [Header("高さの減少量")]
    [SerializeField] int _decrease = 3;
    [Header("地形の高さの基準になる頂点数")]
    [SerializeField] int _vtxLength = 10;
    [Header("地形画像")]
    [SerializeField] GameObject[] _blocks;

    void Start()
    {
        int[,] array = new int[_height, _width];

        // 全ての頂点を同時に操作したいので一度配列に格納する
        Vector2Int[] vertices = new Vector2Int[_vtxLength];
        for (int i = 0; i < _vtxLength; i++)
        {
            int rx = Random.Range(0, array.GetLength(1));
            int ry = Random.Range(0, array.GetLength(0));

            // 頂点の高さを決めておく
            array[ry, rx] = MaxHeight;
            vertices[i] = new Vector2Int(rx, ry);
        }

        // ループ毎に低くしていくので最大値をセットしておく
        int baseHeight = MaxHeight;
        // 渦巻き状に調べる際の1辺の長さ
        // 2辺調べる毎にインクリメントされる
        int side = 1;

        // 頂点を参照するので、頂点の座標をコピーして別途座標の配列を用意する
        Vector2Int[] posArr = new Vector2Int[_vtxLength];
        for (int i = 0; i < posArr.Length; i++)
            posArr[i] = new Vector2Int(vertices[i].x, vertices[i].y);

        int c = 0;

        while (true)
        {
            // ぐるっと1周した場合に高さを変更した回数
            // これが0の場合はもう高さを変更する箇所が残っていないとみなし
            // ループから抜ける。
            // ただし、一つでも高さの変更をする箇所があればその箇所のためだけに
            // 全ループが回ってしまうので効率は非常に悪い
            int count = 0;

            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < side; k++)
                {
                    // 各頂点から並列で進める事で、境界線の違和感を消す
                    for (int l = 0; l < posArr.Length; l++)
                    {
                        int vx = posArr[l].x + dirs[j].x;
                        int vy = posArr[l].y + dirs[j].y;

                        // 操作する座標がマップの配列内に収まっており
                        // 操作する座標は初期値であれば
                        if (0 <= vx && vx < array.GetLength(1) &&
                            0 <= vy && vy < array.GetLength(0) &&
                            array[vy, vx] == 0)
                        {
                            array[vy, vx] = baseHeight;
                            count++;
                        }

                        // 高さの変更を行っていない場合でも次の座標へ進む
                        posArr[l].x = vx;
                        posArr[l].y = vy;
                    }
                }

                // 2辺に対して処理をしたので辺の長さをインクリメントする
                if (j == 1 || j == 3) side++;
            }

            // 高さを減らす
            baseHeight -= _decrease;
            baseHeight = Mathf.Max(1, baseHeight);

            if (count == 0) break;
        }

        // 配列をSpriteに変換する
        for (int i = 0; i < array.GetLength(0); i++)
            for (int j = 0; j < array.GetLength(1); j++)
            {
                int h = array[i, j];
                int index = 5;

                if      (MountainBorder <= h) index = 0; // 山
                else if (HillBorder <= h)     index = 1; // 丘
                else if (ForestBorder <= h)   index = 2; // 森
                else if (GrassBorder <= h)    index = 3; // 草原
                else if (SeaBorder <= h)      index = 4; // 海(浅い)
                else if (DeepSeaBorder <= h)  index = 5; // 海(深い)

                GameObject go = Instantiate(_blocks[index], new Vector2(j, i), Quaternion.identity);
                go.isStatic = true;
            }
    }
}
