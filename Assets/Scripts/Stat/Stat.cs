using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public float GetValue() => baseValue;

    public float SetValue(float newValue)
    {
        baseValue = newValue;
        return baseValue;
    }
}
