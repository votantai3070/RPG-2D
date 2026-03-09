using UnityEngine;
public class Object_Buff : MonoBehaviour
{
    [SerializeField] private BuffEffectData[] buffs;
    private Player player;

    [Header("Buff info")]
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private string buffSourceName = "Buff Object";
    [Space]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = initialPosition + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null && collision.CompareTag("Player"))
            player = collision.GetComponent<Player>();

        if (player.playerStats.CanApplyBuffOf(buffSourceName))
        {
            player.playerStats.ApplyBuff(buffs, buffDuration, buffSourceName);
            Destroy(gameObject);
        }
    }
}
