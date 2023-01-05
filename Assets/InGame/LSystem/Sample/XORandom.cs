using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �����N���X
/// XorShift�ɂ�闐�������N���X
/// </summary>
public class XORandom
{
    private const ulong DefSeedX = 123456789;
    private const ulong DefSeedY = 362436069;
    private const ulong DefSeedZ = 521288629;
    private const ulong DefSeedW = 88675123;

    private ulong seedX;
    private ulong seedY;
    private ulong seedZ;
    private ulong seedW;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="seed1">�V�[�h�l1</param>
    /// <param name="seed2">�V�[�h�l2</param>
    /// <param name="seed3">�V�[�h�l3</param>
    /// <param name="seed4">�V�[�h�l4</param>
    public XORandom(int seed1, int seed2, int seed3, int seed4)
    {
        this.Srand(seed1, seed2, seed3, seed4);
    }
    public XORandom(int seed) : this((int)DefSeedX, (int)DefSeedY, (int)DefSeedZ, seed) { }
    public XORandom() : this(DateTime.Now.Millisecond) { }

    /// <summary>
    /// �����V�[�h�l�̃Z�b�g
    /// </summary>
    /// <param name="seed1">�V�[�h�l1</param>
    /// <param name="seed2">�V�[�h�l2</param>
    /// <param name="seed3">�V�[�h�l3</param>
    /// <param name="seed4">�V�[�h�l4</param>
    public void Srand(int seed1, int seed2, int seed3, int seed4)
    {
        seedX = (ulong)seed1;
        seedY = (ulong)seed2;
        seedZ = (ulong)seed3;
        seedW = (ulong)seed4;
    }

    /// <summary>
    /// �ȈՔŗ����V�[�h�l�Z�b�g
    /// �V�[�h4�ȊO�Ƀf�t�H���g�l�𗘗p���܂��B
    /// </summary>
    /// <param name="seed">�V�[�h�l</param>
    public void Srand(int seed)
    {
        this.Srand((int)DefSeedX, (int)DefSeedY, (int)DefSeedZ, seed);
    }

    /// <summary>
    /// ���ԁi�~���b�j���V�[�h�l�ɗ��p���܂�
    /// </summary>
    public void Srand()
    {
        this.Srand(DateTime.Now.Millisecond);
    }

    /// <summary>
    /// xorshift�ɂ�闐���擾
    /// </summary>
    /// <returns>�������������l</returns>
    public ulong NextULong()
    {
        ulong t = (seedX ^ (seedX << 11));
        seedX = seedY;
        seedY = seedZ;
        seedZ = seedW;
        return (seedW = (seedW ^ (seedW >> 19)) ^ (t ^ (t >> 8)));
    }

    /// <summary>
    /// long�ł̒l�擾
    /// </summary>
    /// <returns></returns>
    public long NextLong()
    {
        return (long)NextULong();
    }

    /// <summary>
    /// uint�ł̒l�擾
    /// </summary>
    /// <returns></returns>
    public uint NextUInt()
    {
        return (uint)NextULong();
    }

    /// <summary>
    /// int�ł̗����l�擾
    /// </summary>
    /// <returns></returns>
    public int NextInt()
    {
        return (int)NextULong();
    }

    /// <summary>
    /// �w��͈͂ł̗����ԋp
    /// </summary>
    /// <param name="min">�ŏ��l</param>
    /// <param name="max">�ő�l</param>
    /// <returns>�w��͈̗͂����iRandom.Range���lmax�l���܂܂Ȃ��j</returns>
    public int Range(int min, int max)
    {
        int val = max - min;
        if (val <= 0)
        {
            return min;
        }
        return min + Mathf.Abs(this.NextInt()) % val;
    }

    //�����ԋp�Łimax���܂܂Ȃ��j
    public float Range(float min, float max)
    {
        return min + (max - min) * ((float)Range(0, 0x7fffffff) / 0x7fffffff);
    }

    #region ---- static use ----
    /// <summary>
    /// static�p�C���X�^���X
    /// </summary>
    private static XORandom rand = new XORandom();

    /// <summary>
    /// �ȈՓI�ɗ��p���闐��������
    /// </summary>
    /// <param name="min">�ŏ��l</param>
    /// <param name="max">�ő�l</param>
    /// <returns>�w��͈̗͂���</returns>
    public static int RandRange(int min, int max)
    {
        return rand.Range(min, max);
    }

    //�����Łimax���܂܂Ȃ��j
    public static float RandRange(float min, float max)
    {
        return rand.Range(min, max);
    }
    #endregion
}