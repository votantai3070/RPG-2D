using UnityEngine;
using static Skill_DataSO;

public class Skill_Base : MonoBehaviour
{
    public Player player { get; private set; }
    public Player_SkillManager skillManager { get; private set; }
    public DamageScaleData damageScaleData { get; private set; }

    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;


    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        skillManager = GetComponentInParent<Player_SkillManager>();
        damageScaleData = new DamageScaleData();

        lastTimeUsed -= cooldown;
    }

    public void SetSkillUpgrades(Skill_DataSO skillData)
    {
        UpgradeData upgrade = skillData.upgradeData;
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScale;


        player.ui.ingame.GetSkillSlot(skillType).SetupSkillSlot(skillData);
        ResetCooldown();
    }

    public virtual void TryUseSkill()
    {

    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    public virtual bool CanBeUsedSkill()
    {
        if (upgradeType == SkillUpgradeType.None)
            return false;

        if (OnCoolDown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;
    }

    protected bool OnCoolDown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillCooldown()
    {
        player.ui.ingame.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;
    }
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed += cooldownReduction;
    public void ResetCooldown()
    {
        player.ui.ingame.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }
}
