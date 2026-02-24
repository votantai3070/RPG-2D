using UnityEngine;
using static Skill_DataSO;

public class Skill_Base : MonoBehaviour
{
    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    private void Awake()
    {
        lastTimeUsed -= cooldown;
    }

    public void SetSkillUpgrades(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    public bool CanBeUsedSkill()
    {
        if (OnCoolDown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;
    }

    private bool OnCoolDown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillCooldown() => lastTimeUsed = Time.time;
    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;
}
