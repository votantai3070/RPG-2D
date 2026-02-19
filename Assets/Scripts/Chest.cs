using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Animator anim;
    private Rigidbody2D rb;

    private Coroutine openChestCo;
    [SerializeField] private Vector3 knockback = new(0, 3);
    [SerializeField] private float openChestDuration = .5f;
    private bool isOpen;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool TakeDamaged(int damage, Transform damageDealer)
    {
        if (isOpen)
            return false;

        OpenChest();

        return true;
    }

    private void OpenChest()
    {
        if (openChestCo != null)
            StopCoroutine(openChestCo);

        openChestCo = StartCoroutine(OpenChestCo());
    }

    private IEnumerator OpenChestCo()
    {
        isOpen = false;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(openChestDuration);

        anim.SetBool("OpenChest", true);
        rb.linearVelocity = new(0, rb.linearVelocityY);
        isOpen = true;
    }
}
