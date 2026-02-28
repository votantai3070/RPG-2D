using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash skillDash { get; private set; }
    public Skill_Shard skillShard { get; private set; }
    public Skill_ThrowSword skillThrowSword { get; private set; }

    private void Awake()
    {
        skillDash = GetComponentInChildren<Skill_Dash>();
        skillShard = GetComponentInChildren<Skill_Shard>();
        skillThrowSword = GetComponentInChildren<Skill_ThrowSword>();
    }

    public Skill_Base GetSkillByType(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Dash:
                return skillDash;

            case SkillType.TimeShard:
                return skillShard;

            default:
                Debug.Log($"Skill type {skillType} is not implement yet");
                return null;
        }
    }
}
