using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory; // itemSaveId -> stackSize
    public SerializableDictionary<string, int> storageItems;
    public SerializableDictionary<string, int> storageMaterials;

    public SerializableDictionary<string, ItemType> equipedItems; // itemSaveId -> slotType;

    public int skillPoint;
    public SerializableDictionary<string, bool> skillTreeUI; // skill name -> is unlocked
    public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades; // skillType -> upgradeType

    public SerializableDictionary<string, bool> unlockedCheckpoints; // check point id -> is unlocked
    public SerializableDictionary<string, Vector3> inScencePortals; // scene name -> portal position

    public SerializableDictionary<string, bool> completedQuests; // quest save id -> complete status
    public SerializableDictionary<string, int> activeQuests; // active quest save id -> current progress

    public string portalDestinationSceneName;
    public bool returningFromTown;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();

        equipedItems = new SerializableDictionary<string, ItemType>();

        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();

        unlockedCheckpoints = new SerializableDictionary<string, bool>();
        inScencePortals = new SerializableDictionary<string, Vector3>();

        completedQuests = new SerializableDictionary<string, bool>();
        activeQuests = new SerializableDictionary<string, int>();
    }
}
