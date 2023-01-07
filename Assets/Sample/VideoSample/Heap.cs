using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 爆速コレクション
/// </summary>
/// <typeparam name="T"></typeparam>
public class Heap<T> where T: IHeapItem<T>
{
    T[] _items;
    int _currentItemCount;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        SortUp(item);
        _currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentItemCount--;
        // 先頭のアイテムをケツに
        _items[0] = _items[_currentItemCount];
        _items[0].HeapIndex = 0;
        SortDown(_items[0]);

        return firstItem;
    }

    public void UpdateItem(T item) => SortUp(item);

    public int Count => _currentItemCount;

    public bool Contains(T item) => Equals(_items[item.HeapIndex], item);

    void SortDown(T item)
    {
        while (true)
        {
            // ヒープ上の添え字の2倍から後ろ二つ
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // 配列の範囲内なら
            if (childIndexLeft < _currentItemCount)
            {
                swapIndex = childIndexLeft;

                // 配列の範囲内なら
                if (childIndexRight < _currentItemCount)
                {
                    // 子2つを比較して左の方が小さければ
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // 引数で渡されたアイテムと上の条件分岐で設定した左か右の子を比較
                if (item.CompareTo(_items[swapIndex]) < 0)
                {
                    Swap(item, _items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        // ヒープの真ん中？
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = _items[parentIndex];
            // 比較した結果こっちが上なら
            if (item.CompareTo(parentItem) > 0)
            {
                // 指定されたアイテムと真ん中のアイテムを交換する
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        // 交換しているがtempに保存しなくていいの？
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;

        // アイテムAとBのヒープ上の添え字を入れ替える
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}