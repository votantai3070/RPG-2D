using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();

    private float finalValue;
    private bool isDirty = true;

    public float GetValue()
    {
        if (isDirty)
        {
            finalValue = GetFinalValue();
            isDirty = false;
        }

        return finalValue;
    }

    public void AddModifier(float value, string sourceName)
    {
        StatModifier modifier = new StatModifier(value, sourceName);
        modifiers.Add(modifier);
        isDirty = true;
    }

    public void RemoveModifier(string sourceName)
    {
        modifiers.RemoveAll(modifier => modifier.sourceName == sourceName);
        isDirty = true;
    }

    public float GetFinalValue()
    {
        float finalValue = baseValue;
        foreach (StatModifier modifier in modifiers)
        {
            finalValue += modifier.value;
        }
        return finalValue;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}

[Serializable]
public class StatModifier
{
    public string sourceName;
    public float value;

    public StatModifier(float value, string sourceName)
    {
        this.value = value;
        this.sourceName = sourceName;
    }
}
