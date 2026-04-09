using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "QUEST DATABASE", menuName = "RPG Setup/ Quest Data/ Quest Database")]
public class QuestDatabaseSO : ScriptableObject
{
    public QuestDataSO[] allQuests;

    public QuestDataSO GetQuestById(string id)
    {
        return allQuests.FirstOrDefault(quest => quest != null && quest.questSaveId == id);
    }


#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all QuestDataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestDataSO");

        allQuests = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<QuestDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

}
