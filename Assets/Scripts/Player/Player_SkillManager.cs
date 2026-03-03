using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash skillDash { get; private set; }
    public Skill_Shard skillShard { get; private set; }
    public Skill_ThrowSword skillThrowSword { get; private set; }
    public Skill_TimeEcho skillTimeEcho { get; private set; }
    public Skill_DomainExpansion skillDomain { get; private set; }

    private Skill_Base[] allSkills;

    private void Awake()
    {
        skillDash = GetComponentInChildren<Skill_Dash>();
        skillShard = GetComponentInChildren<Skill_Shard>();
        skillThrowSword = GetComponentInChildren<Skill_ThrowSword>();
        skillTimeEcho = GetComponentInChildren<Skill_TimeEcho>();
        skillDomain = GetComponentInChildren<Skill_DomainExpansion>();

        allSkills = GetComponentsInParent<Skill_Base>();
    }

    public void ReduceAllSkillCooldownBy(float amount)
    {
        foreach (var skill in allSkills)
            skill.ReduceCooldownBy(amount);
    }

    public Skill_Base GetSkillByType(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Dash: return skillDash;
            case SkillType.TimeShard: return skillShard;
            case SkillType.SwordThrow: return skillThrowSword;
            case SkillType.TimeEcho: return skillTimeEcho;
            case SkillType.DomainExpansion: return skillDomain;

            default:
                Debug.Log($"Skill type {skillType} is not implement yet");
                return null;
        }
    }
}
