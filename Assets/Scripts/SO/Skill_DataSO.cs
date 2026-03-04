using UnityEngine;

[CreateAssetMenu(fileName = "Skill - ", menuName = "RPG Setup/Skill")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Description")]
    public string displayName;
    [TextArea(3, 10)]
    public string description;
    public Sprite icon;

    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockedByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;

    [System.Serializable]
    public class UpgradeData
    {
        public SkillUpgradeType upgradeType;
        public float cooldown;
        public DamageScaleData damageScale;
    }
}
