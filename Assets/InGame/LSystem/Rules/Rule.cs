using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Rule : ScriptableObject
{
    [SerializeField] string _letter;
    [SerializeField] string[] _results = null;
    [SerializeField] bool _randomResult = false;

    public string Letter => _letter;

    public string GetResult()
    {
        if (_randomResult)
        {
            int randomIndex = Random.Range(0, _results.Length);
            return _results[randomIndex];
        }
        return _results[0];
    }
}
