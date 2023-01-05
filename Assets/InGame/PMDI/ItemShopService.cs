using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���프���T�[�r�X�̃C���^�[�t�F�[�X
/// </summary>
public interface IWeaponTradeService
{
    string GetWeaponTrade();
}

/// <summary>
/// �A�C�e���V���b�v�̃T�[�r�X
/// </summary>
public class ItemShopService
{
    readonly IWeaponTradeService _weaponTradeService;

    // �{�ԗp�̈��������̃R���X�g���N�^
    // ���̃N���X�𐶐�����Ǝ����ŕK�v�ȃC���^�[�t�F�[�X�����N���X�𐶐�����
    // ���g�̃C���^�[�t�F�[�X�������Ɏ��R���X�g���N�^���Ă�ő�����Ă����
    public ItemShopService() : this(new WeaponTradeService()) { }

    // �e�X�g�p�̃R���X�g���N�^
    // �������͕K�v�ȃC���^�[�t�F�[�X���������Ă���N���X���蓮�œn���Ă��K�v������
    public ItemShopService(IWeaponTradeService service)
    {
        _weaponTradeService = service;
    }

    public string GetItemShop()
    {
        string weapon = _weaponTradeService.GetWeaponTrade();

        return "�������镐��: " + weapon;
    }
}

/// <summary>
/// ���프���̃T�[�r�X(�{�ԗp)
/// </summary>
public class WeaponTradeService : IWeaponTradeService
{
    public string GetWeaponTrade()
    {
        return "�ǂ��̂邬";
    }
}

/// <summary>
/// ���프���̃T�[�r�X(�e�X�g�p)
/// </summary>
public class WeaponTradeServiceTest : IWeaponTradeService
{
    public string GetWeaponTrade()
    {
        return "�e�X�g�p����";
    }
}

/// <summary>
/// �N���C�A���g
/// </summary>
public class Client
{
    public void Process()
    {
        // �e�X�g���̓e�X�g�p�N���X�𐶐����Ē�������
        WeaponTradeServiceTest wtst = new WeaponTradeServiceTest();
        ItemShopService service = new ItemShopService(wtst);
        string trade = service.GetItemShop();

        // �{�ԗp�͂���Ȋ����Ŏ����Œ������Ă����
        ItemShopService service2 = new ItemShopService();
        string trade2 = service2.GetItemShop();
    }
}