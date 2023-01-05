using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���w�֘A���[�e�B���e�B
/// </summary>
public class MathUtil
{
    /// <summary>
    /// ���W��0�`360�x�͈̔͂Ɏ��߂�
    /// </summary>
    /// <param name="angle">��]�p</param>
    /// <returns>�ԊҌ�̉�]�p</returns>
    public static Vector3 AngleRange360(Vector3 angle)
    {
        angle.x = Mathf.Repeat(angle.x, 360f);
        angle.y = Mathf.Repeat(angle.y, 360f);
        angle.z = Mathf.Repeat(angle.z, 360f);
        return angle;
    }

    /// <summary>
    /// ���W��0�`360�x�͈̔͂Ɏ��߂�
    /// </summary>
    /// <param name="angle">��]�p</param>
    /// <returns>�ԊҌ�̉�]�p</returns>
    public static Vector2 AngleRange360(Vector2 angle)
    {
        angle.x = Mathf.Repeat(angle.x, 360f);
        angle.y = Mathf.Repeat(angle.y, 360f);
        return angle;
    }

    /// <summary>
    /// �����̌�������
    /// �Q�l�Fhttp://www5d.biglobe.ne.jp/~tomoya03/shtml/algorithm/Hougan.htm
    /// </summary>
    /// <param name="lp11">��1�̎n�_</param>
    /// <param name="lp12">��1�̏I�_</param>
    /// <param name="lp21">��2�̎n�_</param>
    /// <param name="lp22">��2�̏I�_</param>
    /// <returns>
    /// 0�F��1��ɐ�2�����݂���ifloat�Ȃ̂łقږ����j
    /// -�F��1�Ɛ�2�͌�������
    /// +�F��1�Ɛ�2�͌������Ȃ�
    /// </returns>
    public static int CheckCrossLine(Vector2 lp11, Vector2 lp12, Vector2 lp21, Vector2 lp22)
    {
        return (int)(((lp11.x - lp12.x) * (lp21.y - lp11.y) + (lp11.y - lp12.y) * (lp11.x - lp21.x)) *
                     ((lp11.x - lp12.x) * (lp22.y - lp11.y) + (lp11.y - lp12.y) * (lp11.x - lp22.x)));
    }

    /// <summary>
    /// ��`���ɓ_�����݂��邩���肷��
    /// ��`����ɓ_������ꍇ���܂�
    /// </summary>
    /// <param name="pos">������W</param>
    /// <param name="rect">��`</param>
    /// <returns>true�F��`���ɓ_������</returns>
    public static bool IsInsideRect(Vector2 pos, Rect rect)
    {
        return ((rect.xMin <= pos.x) && (rect.xMax >= pos.x) &&
                (rect.yMin <= pos.y) && (rect.yMax >= pos.y));
    }

    /// <summary>
    /// �O�p�`���ɓ_�����݂��邩���]����
    /// </summary>
    /// <param name="pos">������W</param>
    /// <param name="tpos">�O�p�`�i3���_�z��j</param>
    /// <returns>true�F�O�p�`���ɓ_������</returns>
    public static bool IsInsideTriangle(Vector2 pos, Vector2[] tpos)
    {
        //�O�p�`�̂R�_��������ɂ���ꍇ�͓���Ȃ�����
        if ((tpos[0].x - tpos[2].x) * (tpos[0].y - tpos[1].y) ==
            (tpos[0].x - tpos[1].x) * (tpos[0].y - tpos[2].y))
        {
            return false;
        }

        //�e�ӂƓ_�̌���������s���A1�_���܂����Ŋm�F����_�����Ԑ�����������ꍇ��
        //�O�p�`���ɓ_�͑��݂��Ȃ��A���A�O�p�`�̒�����ɓ_������ꍇ�͊܂ނƔ��肷��
        if (CheckCrossLine(tpos[0], tpos[1], tpos[2], pos) < 0) { return false; }
        if (CheckCrossLine(tpos[0], tpos[2], tpos[1], pos) < 0) { return false; }
        if (CheckCrossLine(tpos[1], tpos[2], tpos[0], pos) < 0) { return false; }
        return true;
    }

    /// <summary>
    /// 2�_�̌�_�����߂�
    /// �Q�l�Fhttp://www.h4.dion.ne.jp/~zero1341/t/03.htm
    /// </summary>
    /// <param name="lp11">��1�̎n�_</param>
    /// <param name="lp12">��1�̏I�_</param>
    /// <param name="lp21">��2�̎n�_</param>
    /// <param name="lp22">��2�̏I�_</param>
    /// <param name="pcross">�v�Z������_</param>
    /// <returns>
    /// 0�F�����͕��s
    /// -�F�͈͓��Ō�������
    /// +�F��������Ō�������
    /// </returns>
    public static int GetCrossPoint(Vector2 lp11, Vector2 lp12, Vector2 lp21, Vector2 lp22, ref Vector2 pcross)
    {
        // �p�����[�^�\�L�̒l�ɕϊ�����
        float x1 = lp11.x;
        float y1 = lp11.y;
        float f1 = lp12.x - lp11.x;
        float g1 = lp12.y - lp11.y;

        float x2 = lp21.x;
        float y2 = lp21.y;
        float f2 = lp22.x - lp21.x;
        float g2 = lp22.y - lp21.y;

        // det�̌v�Z
        float det = f2 * g1 - f1 * g2;
        if (det == 0)
        {
            //���s�Ō����Ȃ�
            return 0;
        }

        // ��_�ɂ�����p�����[�^
        float dx = x2 - x1;
        float dy = y2 - y1;
        float t1 = (f2 * dy - g2 * dx) / det;
        float t2 = (f1 * dy - g1 * dx) / det;

        // ��_�̍��W
        pcross.x = x1 + f1 * t1;
        pcross.y = y1 + g1 * t1;

        //�͈͓��i2�_��[�Ƃ�������j�Ō������邩�m�F
        if (0 <= t1 && t1 <= 1 && 0 <= t2 && t2 <= 1)
        {
            return -1;
        }
        return 1;
    }

    /// <summary>
    /// 2�~�̌�_�����߂�
    /// </summary>
    /// <param name="pos1">�~1�̒��S</param>
    /// <param name="radius1">�~1�̔��a</param>
    /// <param name="pos2">�~2�̒��S</param>
    /// <param name="radius2">�~2�̔��a</param>
    /// <param name="poslist">���߂�ꂽ��_�̊i�[��A2�v�f����z���p��</param>
    /// <returns>
    /// ��_�̐�������
    /// 0�F��_������
    /// 1�F2�~�͐ڂ���i��_��1�j
    /// 2�F2�~�͌����i��_��2�j
    /// </returns>
    public static int GetCrossPointRounds(Vector2 pos1, float radius1, Vector2 pos2, float radius2, Vector2[] poslist)
    {
        // �����`�F�b�N
        if ((radius1 == 0) || (radius2 == 0)) { return 0; }

        // 2�_�Ԃ̋��������߂�
        float dist = Vector2.Distance(pos1, pos2);

        // �����̃`�F�b�N
        if (dist > radius1 + radius2) { return 0; }         //�~���͂��Ȃ�
        if (dist < Mathf.Abs(radius1 - radius2)) { return 0; }  //�~������

        // cos�Ƃ����߂�
        float cosval = (radius1 * radius1 + dist * dist - radius2 * radius2) / (2.0f * radius1 * dist);

        // �p�x�Ƃ����߂�
        float rad = Mathf.Acos(cosval);

        // 2�~�̒��S�_��X���W�AY���W�̍������߂�
        float cx = pos2.x - pos1.x;
        float cy = pos2.y - pos1.y;

        // 2�~�̒��S�_��X���̂Ȃ��p�x�����߂�
        float rad_origin = 0.0f;
        if (cx == 0)
        {
            if (cy > 0)
            {
                // 90�x
                rad_origin = Mathf.PI / 2.0f;
            }
            else if (cy < 0)
            {
                // 270�x
                rad_origin = Mathf.PI * 1.5f;
            }
            else
            {
                // ��_�Ȃ�
                return 0;
            }
        }
        else
        {
            // Arctan���g�p���Ċp�x�����߂�
            rad_origin = Mathf.Atan(cy / cx);

            //cx�����̏ꍇ�A���߂�p�x��180�x������
            if (cx < 0) { rad_origin += Mathf.PI; }
        }

        // ��_���擾����
        poslist[0].x = pos1.x + radius1 * Mathf.Cos(rad_origin + rad);
        poslist[0].y = pos1.y + radius1 * Mathf.Sin(rad_origin + rad);
        poslist[1].x = pos1.x + radius1 * Mathf.Cos(rad_origin - rad);
        poslist[1].y = pos1.y + radius1 * Mathf.Sin(rad_origin - rad);

        //2�_�͐ڂ���i�قړ����j
        if (Mathf.Abs((radius1 + radius2) - dist) <= float.MinValue)
        {   //FLT_EPSILON
            return 1;
        }
        return 2;
    }

    /// <summary>
    /// 2D�|���S�����ɓ_�����݂��邩�m�F����
    /// </summary>
    /// <param name="pos">�m�F����_</param>
    /// <param name="poly">2D�|���S��</param>
    /// <param name="start">�J�n�C���f�b�N�X</param>
    /// <param name="end">�I���C���f�b�N�X</param>
    /// <returns>true�F�|���S�����ɓ_�����݂���</returns>
    public static bool IsInsidePoly(Vector2 pos, Vector2[] poly, int start, int end)
    {
        int crossings = 0;
        Vector2 point0 = new Vector2();
        Vector2 point1 = new Vector2();
        bool checkX0, checkY0, checkY1;

        point0.x = poly[end].x;
        point0.y = poly[end].y;

        checkY0 = (point0.y >= pos.y);    //�����̐�̓_Y�Ǝw����WY�̔�r

        for (int i = start; i <= end; i++)
        {
            point1.x = poly[i].x;
            point1.y = poly[i].y;

            checkY1 = (point1.y >= pos.y);
            if (checkY0 != checkY1)
            {
                checkX0 = (point0.x >= pos.x);
                if (checkX0 == (point1.x >= pos.x))
                {
                    if (checkX0)
                    {
                        crossings += (checkY0 ? -1 : 1);
                    }
                }
                else
                {
                    if ((point1.x - (point1.y - pos.y)
                          * (point0.x - point1.x) / (point0.y - point1.y)) >= pos.x)
                    {
                        crossings += (checkY0 ? -1 : 1);
                    }
                }
            }
            checkY0 = checkY1;

            point0.x = point1.x;
            point0.y = point1.y;
        }

        //crossings��0�ȊO�ł���Α��p�`���Ɏw��_�͑��݂���Ƃ݂Ȃ�
        return (crossings != 0);
    }
    //�ȗ��`�F�S�̂�ΏۂƂ���
    public static bool IsInsidePoly(Vector2 pos, Vector2[] poly)
    {
        return IsInsidePoly(pos, poly, 0, poly.Length - 1);
    }

    /// <summary>
    /// �����Ɠ_�̋�������ԋ߂��_�Ƌ������擾
    /// </summary>
    /// <param name="linep1">���̓_1</param>
    /// <param name="linep2">���̓_2</param>
    /// <param name="pos">�m�F����_</param>
    /// <param name="calcpos">����̓_</param>
    /// <returns>����̓_�Ƃ̋���</returns>
    public static float GetNearPointOnLine(Vector3 linep1, Vector3 linep2, Vector3 pos, out Vector3 calcpos)
    {
        float dist1 = Vector3.Distance(linep1, pos);
        float dist2 = Vector3.Distance(linep2, pos);
        float dist = dist1 + dist2;
        if (dist <= 0)
        {
            //linep1 == linep2 == pos����������
            calcpos = pos;
            return 0.0f;
        }
        //linep1����ɋ����X�P�[�����o��
        float t = dist1 / dist;
        /*���L�ł��ǂ���Lerp���y
        Vector3 vec = linep2 - linep1;
        vec.x *= t;
        vec.y *= t;
        vec.z *= t;
        calcpos = linep1 + vec;
        */
        calcpos = Vector3.Lerp(linep1, linep2, t);
        return Vector3.Distance(pos, calcpos);
    }

    /// <summary>
    /// �����Ɠ_�̋�������ԋ߂��_�Ƌ������擾
    /// </summary>
    /// <param name="linep1">���̓_1</param>
    /// <param name="linep2">���̓_2</param>
    /// <param name="pos">�m�F����_</param>
    /// <param name="calcpos">����̓_</param>
    /// <returns>����̓_�Ƃ̋���</returns>
    public static float GetNearPointOnLine2D(Vector2 linep1, Vector2 linep2, Vector2 pos, out Vector2 calcpos)
    {
        float dist1 = Vector2.Distance(linep1, pos);
        float dist2 = Vector2.Distance(linep2, pos);
        float dist = dist1 + dist2;
        if (dist <= 0)
        {
            //linep1 == linep2 == pos����������
            calcpos = pos;
            return 0.0f;
        }
        //linep1����ɋ����X�P�[�����o��
        float t = dist1 / dist;
        calcpos = Vector2.Lerp(linep1, linep2, t);
        return Vector2.Distance(pos, calcpos);
    }

    //�����Ɠ_�̋�������ԋ߂��_���擾
    public static Vector3 GetPointOnLine(Vector3 linep1, Vector3 linep2, Vector3 pos)
    {
        float dist1 = Vector3.Distance(linep1, pos);
        float dist2 = Vector3.Distance(linep2, pos);
        float dist = dist1 + dist2;
        if (dist <= 0)
        {
            return pos;
        }
        float t = dist1 / dist;
        return Vector3.Lerp(linep1, linep2, t);
    }

    //�����Ɠ_�̋���
    public static float GetDistancePointAndLine(Vector3 linep1, Vector3 linep2, Vector3 pos)
    {
        float dist1 = Vector3.Distance(linep1, pos);
        float dist2 = Vector3.Distance(linep2, pos);
        float dist = dist1 + dist2;
        if (dist <= 0)
        {
            return 0.0f;
        }
        float t = dist1 / dist;
        return Vector3.Distance(pos, Vector3.Lerp(linep1, linep2, t));
    }
}