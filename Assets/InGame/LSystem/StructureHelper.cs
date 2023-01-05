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
        // ���H�e�̊J���Ă���ӏ��Ɠ��H����������̎����^
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpaceAroundRoad(roadPos);
        // �T�C�Y���傫��������z�u���邽�߂ɊJ���Ă���y�n���u���b�N���郊�X�g
        List<Vector3Int> blockedPosList = new List<Vector3Int>();
        foreach (KeyValuePair<Vector3Int, Direction> freeSpot in freeEstateSpots)
        {
            // �T�C�Y���傫�������Ŗ��܂邽�߁A�����ɂ̓T�C�Y1�̌��������ĂȂ�
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
            
            // �S�Ă̌����̎�ނ𒲂ׂ�
            for (int i = 0; i < _buildingTypes.Length; i++)
            {
                // �H�H�H Quantity == -1 ��_buildingTypes�̍Ō��\���B
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
                // �܂��ݒu����]�T������Ȃ��
                if (_buildingTypes[i].IsBuildingAvailable())
                {
                    // �����̃T�C�Y�`�F�b�N
                    if (_buildingTypes[i].SizeRequired > 1)
                    {
                        // �����̔����̃T�C�Y���傫���Ĉ�ԏ�����������Ԃ�
                        int halfSize = Mathf.CeilToInt(_buildingTypes[i].SizeRequired / 2.0f);
                        // ���̌�����u���̂ɕK�v�ȋ󂫒n�����X�g�Ɋi�[���ĕԂ��Ă����
                        List<Vector3Int> tempPosBlocked = new List<Vector3Int>();
                        if (VerifyBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPosList, ref tempPosBlocked))
                        {
                            // ������ݒu����̂ɕK�v�ȃu���b�N����ׂ����W�����X�g�ɒǉ�����
                            blockedPosList.AddRange(tempPosBlocked);
                            GameObject building = SpawnPrefab(_buildingTypes[i].GetPrefab(), freeSpot.Key, rot);
                            _structuresDic.Add(freeSpot.Key, building);
                            // ���d�v:�u���b�N�����󂫒n���Ώۂ̌����������Ă���Ƃ����f�[�^��o�^����
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
        // ���H���㉺�ɔz�u����Ă���ꍇ�͌����̍��E�ɋ󂫒n���K�v
        if (freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            dir = Vector3Int.right;
        }
        else
        {
            dir = new Vector3Int(0, 0, 1);
        }
        // halfSize�̕������ׂ��u���b�N���Ă���
        for (int i = 1; i <= halfSize; i++)
        {
            Vector3Int pos1 = freeSpot.Key + dir * i;
            Vector3Int pos2 = freeSpot.Key - dir * i;
            // �u���b�N����ׂ��ꏊ�����������󂫒n�ł͂Ȃ��ꍇ��false��Ԃ�
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

    /// <summary>�A�j���[�V������ǉ����邽�߂ɐ������\�b�h�Ƃ��ĕ����Ă���</summary>
    private GameObject SpawnPrefab(GameObject prefab, Vector3Int pos, Quaternion rot)
    {
        GameObject newStructure = Instantiate(prefab, pos, rot);
        return newStructure;
    }

    Dictionary<Vector3Int, Direction> FindFreeSpaceAroundRoad(List<Vector3Int> roadPos)
    {
        // ���W�ɑΉ���������̎���
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (Vector3Int pos in roadPos)
        {
            List<Direction> neighbourDirs = PlacementHelper.FindNeighbour(pos, roadPos);
            // 4�����ɑ΂��Ē��ׂ�
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                // ���̕����ɐڑ�����Ă��Ȃ����
                if (!neighbourDirs.Contains(dir))
                {
                    // �d���`�F�b�N����̂�L���̓����̂悤��2�ӏ�����Ă���Ƃ�����Ȃ����߁H
                    Vector3Int newPos = pos + PlacementHelper.GetOffsetFromDirection(dir);
                    if (freeSpaces.ContainsKey(newPos))
                    {
                        continue;
                    }
                    // �����^�ɂ��̈ʒu�Ɠ��H�����������ǉ�����
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
