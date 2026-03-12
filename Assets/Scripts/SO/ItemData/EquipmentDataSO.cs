using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment data - ", menuName = "RPG Setup/Item Data/Equipment Item")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("Item modifiers")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}