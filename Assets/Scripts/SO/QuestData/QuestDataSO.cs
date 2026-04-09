using UnityEditor;
using UnityEngine;

public enum RewardType
{
    Merchant, Blacksmith, None
}

[CreateAssetMenu(fileName = "Quest - ", menuName = "RPG Setup/ Quest Data/ New Quest")]
public class QuestDataSO : ScriptableObject
{
    public string questSaveId;
    [Space]
    public string questName;
    [TextArea(5, 10)] public string questDescription;
    [TextArea(5, 10)] public string questGoal;

    public string questTargetId; // Enemy name, item name, etc.
    public int requiredAmount;

    [Header("Rewards")]
    public RewardType rewardType;
    public Inventory_Item[] rewardItems;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(questSaveId))
        {
            string path = AssetDatabase.GetAssetPath(this);
            questSaveId = AssetDatabase.AssetPathToGUID(path);
        }
#endif
    }
}
