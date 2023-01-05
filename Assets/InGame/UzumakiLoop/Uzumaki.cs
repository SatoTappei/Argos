using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q������Ƀ��[�v����for��
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

        // �����_���ȍ��W����Q������ɕω������Ă���
        int cx = Random.Range(0, _width);
        int cz = Random.Range(0, _height);

        // �u���b�N���\���ɂ��Ă���
        Process(array[cz, cx].Go);

        await UniTask.Delay(System.TimeSpan.FromSeconds(_wait), cancellationToken: this.GetCancellationTokenOnDestroy());

        // �E������
        (int x, int z)[] dirs =
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1),
        };

        // 1�ӂ̒���
        int side = 1;

        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < side; j++)
                {
                    // ���삷����W�͏㉺���E�ŕς��̂ŕ����͔z��Ŏw�肷��
                    int px = cx + dirs[i].x;
                    int pz = cz + dirs[i].z;

                    // �z����Ȃ珈��������
                    if (0 <= px && px < _width &&
                        0 <= pz && pz < _height)
                    {
                        Process(array[pz, px].Go);
                    }

                    // ���ɏ���������}�X���X�V����
                    cx = px;
                    cz = pz;

                    await UniTask.Delay(System.TimeSpan.FromSeconds(_wait), cancellationToken: this.GetCancellationTokenOnDestroy());
                }

                // 2�ӏ�������x�ɕӂ̒�����1������
                if (i == 1 || i == 3)
                {
                    side++;
                }
            }
        }
    }

    /// <summary>�n���ꂽ�I�u�W�F�N�g�ɑ΂��ĉ��炩�̏���������</summary>
    void Process(GameObject go)
    {
        go.SetActive(false);
    }
}