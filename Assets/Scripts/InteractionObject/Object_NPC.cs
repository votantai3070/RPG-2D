using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;
    protected Animator anim;

    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactTooltip;
    private bool facingRight = true;

    [Header("Floaty Tooltip")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindAnyObjectByType<UI>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        startPosition = interactTooltip.transform.position;
        interactTooltip.SetActive(false);
    }

    private void Update()
    {
        HandleFlip();
        HandleTooltipFloat();
    }

    private void HandleTooltipFloat()
    {
        if (interactTooltip.activeSelf)
        {
            float yOffet = Mathf.Sin(Time.time + floatSpeed) * floatRange;
            interactTooltip.transform.position = startPosition + new Vector3(0, yOffet);
        }

    }

    private void HandleFlip()
    {
        if (player == null || npc == null)
            return;

        if (npc.position.x > player.position.x && facingRight)
        {
            npc.Rotate(0, 180, 0);
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && facingRight == false)
        {
            npc.Rotate(0, 180, 0);
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactTooltip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
        interactTooltip.SetActive(false);
    }
}
