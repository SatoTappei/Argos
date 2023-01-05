using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoadHelper : MonoBehaviour
{
    [Header("道路")]
    [SerializeField] GameObject _roadStraight;
    [SerializeField] GameObject _roadCorner;
    [SerializeField] GameObject _road3way;
    [SerializeField] GameObject _road4way;
    [SerializeField] GameObject _roadEnd;

    /// <summary>座標をキーにした辞書、同じ座標に2度建物を建てないために使用する</summary>
    Dictionary<Vector3Int, GameObject> _roadDic = new Dictionary<Vector3Int, GameObject>();
    /// <summary>後から道路を修正できるようにするハッシュセット</summary>
    HashSet<Vector3Int> _fixRoadCandidates = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPos() => _roadDic.Keys.ToList();

    public void PlaceStreetPos(Vector3Int startPos, Vector3Int dir, int length)
    {
        // 上下に接続される場合は回転させて設置する
        Quaternion rot = Quaternion.identity;
        if (dir.x == 0)
        {
            rot = Quaternion.Euler(0, 90, 0);
        }
        // 引数lengthによって位置の調整が出来る
        for(int i = 0; i < length; i++)
        {
            // 少数部を丸め込んでInt型に成形する
            // dirには道路の中心座標から向きの座標が入る?
            Vector3Int pos = Vector3Int.RoundToInt(startPos + dir * i);

            // 同じ座標に複数回アクセスするため辞書に保存して
            // 道路が存在しているかチェックする
            if (_roadDic.ContainsKey(pos)) continue;

            // 生成と追加
            GameObject road = Instantiate(_roadStraight, pos, rot, transform);
            _roadDic.Add(pos, road);

            // 道路の端っこ(接続部)は成形する必要があるためハッシュセットに追加する
            // 疑問:接続される側の道も成形しないといけない気がするがどうするのか？
            if (i == 0 || i == length - 1)
            {
                _fixRoadCandidates.Add(pos);
            }
        }
    }

    public void FixRoad()
    {
        // 道路の端っこのハッシュセットを走査する
        foreach (Vector3Int pos in _fixRoadCandidates)
        {
            // 道路の端っこと生成された道路全部の座標を渡してリストが返ってくる
            // 生成された道路の中にposに隣接した道路の座標のリストが返ってくる
            List<Direction> neighbourDirs = PlacementHelper.FindNeighbour(pos, _roadDic.Keys);

            Quaternion rot = Quaternion.identity;

            if (neighbourDirs.Count == 1)
            {
                Destroy(_roadDic[pos]);
                // 右向きが基準なので右の場合の判定はしなくていい
                if (neighbourDirs.Contains(Direction.Down))
                {
                    rot = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirs.Contains(Direction.Left))
                {
                    rot = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirs.Contains(Direction.Up))
                {
                    rot = Quaternion.Euler(0, -90, 0);
                }
                _roadDic[pos] = Instantiate(_roadEnd, pos, rot, transform);
            }
            else if(neighbourDirs.Count == 2)
            {
                // 2箇所に接続されている場合、左右もしくは上下に接続されているのは真っ直ぐな道路なので端折る
                if (neighbourDirs.Contains(Direction.Up) && neighbourDirs.Contains(Direction.Down) ||
                    neighbourDirs.Contains(Direction.Right) && neighbourDirs.Contains(Direction.Left))
                {
                    continue;
                }

                Destroy(_roadDic[pos]);
                if (neighbourDirs.Contains(Direction.Up) && neighbourDirs.Contains(Direction.Right))
                {
                    rot = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirs.Contains(Direction.Right) && neighbourDirs.Contains(Direction.Down))
                {
                    rot = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirs.Contains(Direction.Down) && neighbourDirs.Contains(Direction.Left))
                {
                    rot = Quaternion.Euler(0, -90, 0);
                }
                _roadDic[pos] = Instantiate(_roadCorner, pos, rot, transform);
            }
            else if(neighbourDirs.Count == 3)
            {
                Destroy(_roadDic[pos]);
                if (neighbourDirs.Contains(Direction.Right) &&
                    neighbourDirs.Contains(Direction.Down) &&
                    neighbourDirs.Contains(Direction.Left))
                {
                    rot = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirs.Contains(Direction.Down) && 
                         neighbourDirs.Contains(Direction.Left) &&
                         neighbourDirs.Contains(Direction.Up))
                {
                    rot = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirs.Contains(Direction.Left) && 
                         neighbourDirs.Contains(Direction.Up) &&
                         neighbourDirs.Contains(Direction.Right))
                {
                    rot = Quaternion.Euler(0, -90, 0);
                }
                _roadDic[pos] = Instantiate(_road3way, pos, rot, transform);
            }
            else
            {
                // 4箇所に接続されている場合は十字路なので回転の必要なし
                Destroy(_roadDic[pos]);
                _roadDic[pos] = Instantiate(_road4way, pos, rot, transform);
            }
        }
    }

    public void Reset()
    {
        foreach(GameObject item in _roadDic.Values)
        {
            Destroy(item);
        }
        _roadDic.Clear();
        _fixRoadCandidates = new HashSet<Vector3Int>();
    }
}
