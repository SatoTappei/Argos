using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器売買サービスのインターフェース
/// </summary>
public interface IWeaponTradeService
{
    string GetWeaponTrade();
}

/// <summary>
/// アイテムショップのサービス
/// </summary>
public class ItemShopService
{
    readonly IWeaponTradeService _weaponTradeService;

    // 本番用の引数無しのコンストラクタ
    // このクラスを生成すると自動で必要なインターフェースを持つクラスを生成して
    // 自身のインターフェースを引数に取るコンストラクタを呼んで代入してくれる
    public ItemShopService() : this(new WeaponTradeService()) { }

    // テスト用のコンストラクタ
    // こっちは必要なインターフェースを実装しているクラスを手動で渡してやる必要がある
    public ItemShopService(IWeaponTradeService service)
    {
        _weaponTradeService = service;
    }

    public string GetItemShop()
    {
        string weapon = _weaponTradeService.GetWeaponTrade();

        return "売買する武器: " + weapon;
    }
}

/// <summary>
/// 武器売買のサービス(本番用)
/// </summary>
public class WeaponTradeService : IWeaponTradeService
{
    public string GetWeaponTrade()
    {
        return "どうのつるぎ";
    }
}

/// <summary>
/// 武器売買のサービス(テスト用)
/// </summary>
public class WeaponTradeServiceTest : IWeaponTradeService
{
    public string GetWeaponTrade()
    {
        return "テスト用武器";
    }
}

/// <summary>
/// クライアント
/// </summary>
public class Client
{
    public void Process()
    {
        // テスト時はテスト用クラスを生成して注入する
        WeaponTradeServiceTest wtst = new WeaponTradeServiceTest();
        ItemShopService service = new ItemShopService(wtst);
        string trade = service.GetItemShop();

        // 本番用はこんな感じで自動で注入してくれる
        ItemShopService service2 = new ItemShopService();
        string trade2 = service2.GetItemShop();
    }
}