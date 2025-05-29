using System;
using UnityEngine;


[Serializable]
public class Rooms
{
    public string rNumber;
    public int cost;
    public GameObject prefab;

    public Rooms(string _rNumber, int _cost, GameObject _prefab)
    {
        rNumber = _rNumber;
        cost = _cost;
        prefab = _prefab;
    }
}
