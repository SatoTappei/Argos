using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEasy : MonoBehaviour
{
    FlockingManager _manager;

    void Start()
    {
        _manager = FindObjectOfType<FlockingManager>();
    }

    void Update()
    {
        //GameObject[] fishes = _manager.Fishes;

        // 群れの平均の座標を求める
        //Vector3 avePos = Vector3.zero;
        //for (int i = 0; i < fishes.Length; i++)
        //{
        //    avePos += fishes[i].transform.position;
        //}
        //avePos /= fishes.Length;
    }
}
