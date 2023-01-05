using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Param_")]
public class SimpleRandomWalkSO : ScriptableObject
{
    [SerializeField] int _iteration = 10;
    [SerializeField] int _walkLength = 10;
    [SerializeField] bool _startRandomlyEachIteration = true;

    public int Iteration => _iteration;
    public int WalkLength => _walkLength;
    public bool StartRandomlyEachIteration => _startRandomlyEachIteration;
}
