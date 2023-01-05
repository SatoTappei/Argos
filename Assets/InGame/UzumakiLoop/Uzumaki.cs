using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// 渦巻き状にループするfor文
/// </summary>
public class Uzumaki : MonoBehaviour
{
    struct Block
    {
        int x;
        int z;
        GameObject go;

        public Block(int x, int z, GameObject go)
        {
            this.x = x;
            this.z = z;
            this.go = go;
        }

        public GameObject Go { get => go; set => go = value; }
    }

    [SerializeField] GameObject _prefab;
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] float _wait;

    async UniTaskVoid Start()
    {
        Block[,] array = new Block[_height, _width];

        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                GameObject go = Instantiate(_prefab, new Vector3(j, 0, i), Quaternion.identity);
                array[i, j] = new Block(j, i, go);
            }
        }

        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: this.GetCancellationTokenOnDestroy());

        // ランダムな座標から渦巻き状に変化させていく
        int cx = Random.Range(0, _width);
        int cz = Random.Range(0, _height);

        // ブロックを非表示にしていく
        Process(array[cz, cx].Go);

        await UniTask.Delay(System.TimeSpan.FromSeconds(_wait), cancellationToken: this.GetCancellationTokenOnDestroy());

        // 右下左上
        (int x, int z)[] dirs =
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1),
        };

        // 1辺の長さ
        int side = 1;

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < side; j++)
                {
                    // 操作する座標は上下左右で変わるので方向は配列で指定する
                    int px = cx + dirs[i].x;
                    int pz = cz + dirs[i].z;

                    // 配列内なら処理をする
                    if (0 <= px && px < _width &&
                        0 <= pz && pz < _height)
                    {
                        Process(array[pz, px].Go);
                    }

                    // 次に処理をするマスを更新する
                    cx = px;
                    cz = pz;

                    await UniTask.Delay(System.TimeSpan.FromSeconds(_wait), cancellationToken: this.GetCancellationTokenOnDestroy());
                }

                // 2辺処理する度に辺の長さが1増える
                if (i == 1 || i == 3)
                {
                    side++;
                }
            }
        }
    }

    /// <summary>渡されたオブジェクトに対して何らかの処理をする</summary>
    void Process(GameObject go)
    {
        go.SetActive(false);
    }
}