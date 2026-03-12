using UnityEngine;

[CreateAssetMenu(fileName = "List of data - ", menuName = "RPG Setup/Item Data/Item List")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;
}
