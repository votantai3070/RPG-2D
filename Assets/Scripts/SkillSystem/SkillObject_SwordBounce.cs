using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    private float bounceSpeed;
    private float bounceTime;
    private List<Transform> chooseBefore = new();
    private Collider2D[] enemies;
    private Transform nextEnemy;

    protected override void Update()
    {
        HandleBounce();
        HandleComeback();
    }

    public override void SetupSword(Skill_ThrowSword swordManger, Vector2 direction)
    {
        anim?.SetTrigger("Spin");
        base.SetupSword(swordManger, direction);

        bounceSpeed = swordManger.bounceSpeed;
        bounceTime = swordManger.bounceTime;
    }

    private void HandleBounce()
    {
        if (nextEnemy == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, nextEnemy.position, bounceSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextEnemy.position) < .75f)
        {
            DamageEnemiesInRadius(transform, 1);
            BounceToNextEnemy();

            if (bounceTime <= 0 || nextEnemy == null)
            {
                nextEnemy = null;
                GetSwordBackToPlayer();
            }
        }
    }

    private void BounceToNextEnemy()
    {
        nextEnemy = GetNextEnemy();
        bounceTime--;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemies == null)
        {
            enemies = GetEnemyAround(transform, 10);
            rb.simulated = false;
        }

        DamageEnemiesInRadius(transform, 1);

        if (enemies.Length <= 1 || bounceTime <= 0)
            GetSwordBackToPlayer();
        else
            nextEnemy = GetNextEnemy();
    }

    private Transform GetNextEnemy()
    {
        List<Transform> validEnemies = GetValidEnemies();

        int randomEnemy = Random.Range(0, validEnemies.Count);

        Transform nextToEnemy = validEnemies[randomEnemy];
        chooseBefore.Add(nextToEnemy);

        return nextToEnemy;
    }

    private List<Transform> GetValidEnemies()
    {
        List<Transform> validEnemies = new();
        List<Transform> aliveEnemies = GetAliveEnemies();

        foreach (var enemy in aliveEnemies)
        {
            if (enemy.CompareTag("Enemy") && !chooseBefore.Contains(enemy))
                validEnemies.Add(enemy);
        }

        if (validEnemies.Count > 0)
            return validEnemies;
        else
        {
            chooseBefore.Clear();
            return aliveEnemies;
        }
    }

    private List<Transform> GetAliveEnemies()
    {
        List<Transform> aliveEnemies = new();

        foreach (var enemy in enemies)
        {
            if (enemy != null)
                aliveEnemies.Add(enemy.transform);
        }

        return aliveEnemies;
    }
}
