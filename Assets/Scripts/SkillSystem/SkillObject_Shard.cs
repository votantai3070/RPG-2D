using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;
    public Skill_Shard shardManager;

    [SerializeField] private GameObject vfxPrefab;
    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed, Transform newTarget = null)
    {
        target = newTarget == null ? FindClosestTarget() : newTarget;
        this.speed = speed;
    }


    public void SetupShard(Skill_Shard shardManager)
    {
        this.shardManager = shardManager;
        playerStats = shardManager.player.entityStats;
        damageScale = shardManager.damageScaleData;
        player = shardManager.player;

        float detinationTime = shardManager.GetDetonationTime();

        Invoke(nameof(ShardExplosion), detinationTime);
    }

    public void SetupShard(Skill_Shard shardManager, float detinationTime, bool canMove, float shardSpeed, Transform target)
    {
        this.shardManager = shardManager;
        playerStats = shardManager.player.entityStats;
        damageScale = shardManager.damageScaleData;

        Invoke(nameof(ShardExplosion), detinationTime);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed, target);
    }

    public void ShardExplosion()
    {
        DamageEnemiesInRadius(transform, checkDamageRadius);
        GameObject vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        SpriteRenderer sr = vfx.GetComponentInChildren<SpriteRenderer>();

        Debug.Log("Current elemental: " + currentElement);

        sr.color = shardManager.player.vfx.GetElementColorVfx(currentElement);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        ShardExplosion();
    }
}
