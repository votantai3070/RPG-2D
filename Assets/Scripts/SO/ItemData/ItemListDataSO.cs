using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "List of data - ", menuName = "RPG Setup/Item Data/Item List")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;

    public ItemDataSO GetItemData(string saveId)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveId == saveId);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-fill with all ItemDataSO")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");

        itemList = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
