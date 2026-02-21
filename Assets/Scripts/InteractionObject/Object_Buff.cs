using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    [SerializeField] private Buff[] buffs;
    private Player player;

    [Header("Buff info")]
    [SerializeField] private float buffDuration = 5f;
    private bool canBuff = true;
    [SerializeField] private float buffAmount = 10f;
    [SerializeField] private string buffSourceName = "Buff Object";
    [Space]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 initialPosition;
    private Coroutine buffCo;

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

    private void Buff()
    {
        if (!canBuff)
            return;

        if (buffCo != null)
            StopCoroutine(buffCo);

        buffCo = StartCoroutine(BuffCo());
    }

    private IEnumerator BuffCo()
    {
        canBuff = false;
        ApplyBuff(true);

        yield return new WaitForSeconds(buffDuration);

        ApplyBuff(false);
        canBuff = true;
    }

    private void ApplyBuff(bool enable)
    {
        foreach (var buff in buffs)
        {
            if (enable)
                player.entityStat.GetStatByType(buff.type).AddModifier(buff.value, buffSourceName);
            else
                player.entityStat.GetStatByType(buff.type).RemoveModifier(buffSourceName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null && collision.CompareTag("Player"))
            player = collision.GetComponent<Player>();

        if (collision.CompareTag("Player"))
        {
            Buff();
        }
    }
}
