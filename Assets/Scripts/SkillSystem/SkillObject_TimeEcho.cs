using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private GameObject onDeadVfx;
    [SerializeField] private LayerMask whatIsGround;
    private Skill_TimeEcho timeEchoManager;

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        StopHorizontalMovement();
    }

    public void HandleDie()
    {
        Instantiate(onDeadVfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void SetupTimeEcho(Skill_TimeEcho timeEchoManager)
    {
        this.timeEchoManager = timeEchoManager;
        Invoke(nameof(HandleDie), timeEchoManager.GetTimeEchoDuration());
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.linearVelocity = new(0, rb.linearVelocityY);
    }
}
