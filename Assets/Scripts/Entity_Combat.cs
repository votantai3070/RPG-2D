using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private Transform attackPoint;

    public void PerformAttack()
    {
        foreach (var hit in AttackHits())
        {
            hit.GetComponent<Entity_Health>().TakeDamaged(damage, transform);
        }
    }

    private Collider2D[] AttackHits()
    {
        return Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
