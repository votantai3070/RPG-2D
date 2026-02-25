using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float destinationTime = 2;

    protected override void TryUseSkill()
    {
        base.TryUseSkill();

        if (Unlocked(SkillUpgradeType.Shard))
            SkilShardRegular();
    }

    private void SkilShardRegular()
    {
        CreateShard();
        SetSkillCooldown();
    }

    public void CreateShard()
    {
        if (upgradeType == SkillUpgradeType.None)
            return;

        GameObject shardClone = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shardClone.GetComponent<SkillObject_Shard>().SetupShard(destinationTime);
    }
}
