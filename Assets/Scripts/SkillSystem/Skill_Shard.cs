using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float destinationTime = 2;

    [Header("Moving Shard upgrade")]
    [SerializeField] private float speed = 2;

    [Header("Moving Shard upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges = 0;
    [SerializeField] private bool isRecharging;

    public override void TryUseSkill()
    {
        if (!CanBeUsedSkill())
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            SkillShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            SkillMoveToEnemy();

        if (Unlocked(SkillUpgradeType.Shard_TripleCast))
            SkillShardMulticast();
    }

    private void SkillShardMulticast()
    {
        if (currentCharges <= 0)
            return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(speed);
        currentCharges--;

        if (!isRecharging)
            StartCoroutine(ShardChargedCo());
    }

    private IEnumerator ShardChargedCo()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharges++;
        }

        isRecharging = false;
    }

    private void SkillMoveToEnemy()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(speed);
        SetSkillCooldown();
    }

    private void SkillShardRegular()
    {
        CreateShard();
        SetSkillCooldown();
    }

    public void CreateShard()
    {
        if (upgradeType == SkillUpgradeType.None)
            return;

        GameObject shardClone = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shardClone.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(destinationTime);
    }
}
