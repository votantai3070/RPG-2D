using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash skillDash { get; private set; }


    private void Awake()
    {
        skillDash = GetComponentInChildren<Skill_Dash>();
    }

    public Skill_Base GetSkillByType(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Dash:
                return skillDash;

            default:
                Debug.Log($"Skill type {skillType} is not implement yet");
                return null;
        }
    }
}
