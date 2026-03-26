using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Player_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 1;

    [Header("Moving Shard upgrade")]
    [SerializeField] private float speed = 2;

    [Header("Moving Shard upgrade")]
    [SerializeField] private int maxCharges = 3;
    [SerializeField] private int currentCharges = 0;
    [SerializeField] private bool isRecharging;

    [Header("Teleport Shard upgrade")]
    [SerializeField] private float shardExistDuration = 10;

    [Header("Health Rewind Shard upgrade")]
    [SerializeField] private float savedHealthPercent;

    protected override void Awake()
    {
        base.Awake();

        playerHealth = GetComponentInParent<Player_Health>();
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill())
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            SkillShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            SkillMoveToEnemy();

        if (Unlocked(SkillUpgradeType.Shard_TripleCast))
            SkillShardMulticast();

        if (Unlocked(SkillUpgradeType.Shard_Teleport))
            SkillShardTeleport();

        if (Unlocked(SkillUpgradeType.Shard_TeleportAndHeal))
            SkillShardHealRewind();
    }

    private void SkillShardHealRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void SkillShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPos = currentShard.transform.position;
        Vector3 playerPos = player.transform.position;

        currentShard.transform.position = playerPos;
        currentShard.ShardExplosion();

        player.TeleportPlayer(shardPos);
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
        SetSkillOnCooldown();
    }

    private void SkillShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        if (upgradeType == SkillUpgradeType.None)
            return;

        GameObject shardClone = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shardClone.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportAndHeal))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard(Transform target = null, bool shardCanMove = false)
    {
        bool canMove = shardCanMove ? shardCanMove :
            Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_TripleCast);


        GameObject shardClone = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shardClone.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, speed, target);
    }

    public void CreateDomainShard(Transform target)
    {

    }

    public float GetDetonationTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportAndHeal))
            return shardExistDuration;

        return detonateTime;
    }

    private void ForceCooldown()
    {
        if (!OnCooldown())
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
