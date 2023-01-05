using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureHelper : MonoBehaviour
{
    [SerializeField] BuildingType[] _buildingTypes;
    [SerializeField] GameObject[] _naturePrefab;
    [SerializeField] bool _randomNaturePlacement = false;
    [Range(0, 1)]
    [SerializeField] float randomNaturePlacementThreshold = 0.3f;
    [SerializeField] Dictionary<Vector3Int, GameObject> _structuresDic = new Dictionary<Vector3Int, GameObject>();
    [SerializeField] Dictionary<Vector3Int, GameObject> _naturesDic = new Dictionary<Vector3Int, GameObject>();

    public void PlaceStructureAroundRoad(List<Vector3Int> roadPos)
    {
        // 道路脇の開いている箇所と道路がある方向の辞書型
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpaceAroundRoad(roadPos);
        // サイズが大きい建物を配置するために開いている土地をブロックするリスト
        List<Vector3Int> blockedPosList = new List<Vector3Int>();
        foreach (KeyValuePair<Vector3Int, Direction> freeSpot in freeEstateSpots)
        {
            // サイズが大きい建物で埋まるため、ここにはサイズ1の建物を建てない
            if (blockedPosList.Contains(freeSpot.Key))
            {
                continue;
            }

            Quaternion rot = Quaternion.identity;
            switch (freeSpot.Value)
            {
                case Direction.Up:
                    rot = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.Down:
                    rot = Quaternion.Euler(0, -90, 0);
                    break;
                case Direction.Right:
                    rot = Quaternion.Euler(0, 180, 0);
                    break;
            }
            
            // 全ての建物の種類を調べる
            for (int i = 0; i < _buildingTypes.Length; i++)
            {
                // ？？？ Quantity == -1 は_buildingTypesの最後を表す。
                if (_buildingTypes[i].Quantity == -1)
                {
                    if (_randomNaturePlacement)
                    {
                        if (UnityEngine.Random.value < randomNaturePlacementThreshold)
                        {
                            GameObject nature = SpawnPrefab(_naturePrefab[UnityEngine.Random.Range(0, _naturePrefab.Length)],
                                                            freeSpot.Key, rot);
                            _naturesDic.Add(freeSpot.Key, nature);
                            break;
                        }
                    }

                    GameObject building = SpawnPrefab(_buildingTypes[i].GetPrefab(), freeSpot.Key, rot);
                    _structuresDic.Add(freeSpot.Key, building);
                    break;
                }
                // まだ設置する余裕があるならば
                if (_buildingTypes[i].IsBuildingAvailable())
                {
                    // 建物のサイズチェック
                    if (_buildingTypes[i].SizeRequired > 1)
                    {
                        // 建物の半分のサイズより大きくて一番小さい整数を返す
                        int halfSize = Mathf.CeilToInt(_buildingTypes[i].SizeRequired / 2.0f);
                        // この建物を置くのに必要な空き地をリストに格納して返してくれる
                        List<Vector3Int> tempPosBlocked = new List<Vector3Int>();
                        if (VerifyBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPosList, ref tempPosBlocked))
                        {
                            // 建物を設置するのに必要なブロックするべき座標をリストに追加する
                            blockedPosList.AddRange(tempPosBlocked);
                            GameObject building = SpawnPrefab(_buildingTypes[i].GetPrefab(), freeSpot.Key, rot);
                            _structuresDic.Add(freeSpot.Key, building);
                            // ☆重要:ブロックした空き地も対象の建物が立っているというデータを登録する
                            foreach (Vector3Int pos in tempPosBlocked)
                            {
                                _structuresDic.Add(pos, building);
                            }
                        }
                    }
                    else
                    {
                        GameObject building = SpawnPrefab(_buildingTypes[i].GetPrefab(), freeSpot.Key, rot);
                        _structuresDic.Add(freeSpot.Key, building);
                    }
                    break;
                }
            }
            //Instantiate(_prefab, freeSpot.Key, rot, transform);
        }
    }

    private bool VerifyBuildingFits(int halfSize,
                                    Dictionary<Vector3Int, Direction> freeEstateSpots,
                                    KeyValuePair<Vector3Int, Direction> freeSpot,
                                    List<Vector3Int> blockedPosList,
                                    ref List<Vector3Int> tempPosBlocked)
    {
        Vector3Int dir = Vector3Int.zero;
        // 道路が上下に配置されている場合は建物の左右に空き地が必要
        if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            dir = Vector3Int.right;
        }
        else
        {
            dir = new Vector3Int(0, 0, 1);
        }
        // halfSizeの分だけ隣もブロックしていく
        for (int i = 1; i <= halfSize; i++)
        {
            Vector3Int pos1 = freeSpot.Key + dir * i;
            Vector3Int pos2 = freeSpot.Key - dir * i;
            // ブロックするべき場所がそもそも空き地ではない場合はfalseを返す
            if (!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2) ||
                blockedPosList.Contains(pos1) || blockedPosList.Contains(pos2))
            {
                return false;
            }
            tempPosBlocked.Add(pos1);
            tempPosBlocked.Add(pos2);
        }
        return true;
    }

    /// <summary>アニメーションを追加するために生成メソッドとして分けている</summary>
    private GameObject SpawnPrefab(GameObject prefab, Vector3Int pos, Quaternion rot)
    {
        GameObject newStructure = Instantiate(prefab, pos, rot);
        return newStructure;
    }

    Dictionary<Vector3Int, Direction> FindFreeSpaceAroundRoad(List<Vector3Int> roadPos)
    {
        // 座標に対応する方向の辞書
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (Vector3Int pos in roadPos)
        {
            List<Direction> neighbourDirs = PlacementHelper.FindNeighbour(pos, roadPos);
            // 4方向に対して調べる
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                // その方向に接続されていなければ
                if (!neighbourDirs.Contains(dir))
                {
                    // 重複チェックするのはL字の内側のような2箇所被っているところを省くため？
                    Vector3Int newPos = pos + PlacementHelper.GetOffsetFromDirection(dir);
                    if (freeSpaces.ContainsKey(newPos))
                    {
                        continue;
                    }
                    // 辞書型にこの位置と道路がある方向を追加する
                    freeSpaces.Add(newPos, PlacementHelper.GetReverseDir(dir));
                }

            }
        }
        return freeSpaces;
    }

    public void Reset()
    {
        foreach (GameObject item in _structuresDic.Values)
        {
            Destroy(item);
        }
        _structuresDic.Clear();
        foreach (var buildingType in _buildingTypes)
        {
            buildingType.Reset();
        }
    }
}
