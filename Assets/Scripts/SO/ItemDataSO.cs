using UnityEngine;

[CreateAssetMenu(fileName = "Material data - ", menuName = "RPG Setup/Item Data/Material Item")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
}
