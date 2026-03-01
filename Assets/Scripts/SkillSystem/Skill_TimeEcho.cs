using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    public override void TryUseSkill()
    {
        if (!CanBeUsedSkill()) return;

        CreateTimeEcho();
    }

    private void CreateTimeEcho()
    {
        GameObject timeEcho = Instantiate(timeEchoPrefab, transform.position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupTimeEcho(this);
    }

    public float GetTimeEchoDuration() => timeEchoDuration;
}
