using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoadHelper : MonoBehaviour
{
    [Header("���H")]
    [SerializeField] GameObject _roadStraight;
    [SerializeField] GameObject _roadCorner;
    [SerializeField] GameObject _road3way;
    [SerializeField] GameObject _road4way;
    [SerializeField] GameObject _roadEnd;

    /// <summary>���W���L�[�ɂ��������A�������W��2�x���������ĂȂ����߂Ɏg�p����</summary>
    Dictionary<Vector3Int, GameObject> _roadDic = new Dictionary<Vector3Int, GameObject>();
    /// <summary>�ォ�瓹�H���C���ł���悤�ɂ���n�b�V���Z�b�g</summary>
    HashSet<Vector3Int> _fixRoadCandidates = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPos() => _roadDic.Keys.ToList();

    public void PlaceStreetPos(Vector3Int startPos, Vector3Int dir, int length)
    {
        // �㉺�ɐڑ������ꍇ�͉�]�����Đݒu����
        Quaternion rot = Quaternion.identity;
        if (dir.x == 0)
        {
            rot = Quaternion.Euler(0, 90, 0);
        }
        // ����length�ɂ���Ĉʒu�̒������o����
        for(int i = 0; i < length; i++)
        {
            // ���������ۂߍ����Int�^�ɐ��`����
            // dir�ɂ͓��H�̒��S���W��������̍��W������?
            Vector3Int pos = Vector3Int.RoundToInt(startPos + dir * i);

            // �������W�ɕ�����A�N�Z�X���邽�ߎ����ɕۑ�����
            // ���H�����݂��Ă��邩�`�F�b�N����
            if (_roadDic.ContainsKey(pos)) continue;

            // �����ƒǉ�
            GameObject road = Instantiate(_roadStraight, pos, rot, transform);
            _roadDic.Add(pos, road);

            // ���H�̒[����(�ڑ���)�͐��`����K�v�����邽�߃n�b�V���Z�b�g�ɒǉ�����
            // �^��:�ڑ�����鑤�̓������`���Ȃ��Ƃ����Ȃ��C�����邪�ǂ�����̂��H
            if (i == 0 || i == length - 1)
            {
                _fixRoadCandidates.Add(pos);
            }
        }
    }

    public void FixRoad()
    {
        // ���H�̒[�����̃n�b�V���Z�b�g�𑖍�����
        foreach (Vector3Int pos in _fixRoadCandidates)
        {
            // ���H�̒[�����Ɛ������ꂽ���H�S���̍��W��n���ă��X�g���Ԃ��Ă���
            // �������ꂽ���H�̒���pos�ɗאڂ������H�̍��W�̃��X�g���Ԃ��Ă���
            List<Direction> neighbourDirs = PlacementHelper.FindNeighbour(pos, _roadDic.Keys);

            Quaternion rot = Quaternion.identity;

            if (neighbourDirs.Count == 1)
            {
                Destroy(_roadDic[pos]);
                // �E��������Ȃ̂ŉE�̏ꍇ�̔���͂��Ȃ��Ă���
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
                // 2�ӏ��ɐڑ�����Ă���ꍇ�A���E�������͏㉺�ɐڑ�����Ă���̂͐^�������ȓ��H�Ȃ̂Œ[�܂�
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
                // 4�ӏ��ɐڑ�����Ă���ꍇ�͏\���H�Ȃ̂ŉ�]�̕K�v�Ȃ�
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
