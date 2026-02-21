using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/Skill")]
public class Skill_SO : ScriptableObject
{
    public int cost;

    [Header("Skill Description")]
    public string displayName;
    [TextArea(3, 10)]
    public string description;
    public Sprite icon;
}
