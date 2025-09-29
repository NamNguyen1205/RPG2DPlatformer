using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    public float GetValue()
    {
        return baseValue;
    }

    //buff or items affecting base value
    // all calculations done here
}
