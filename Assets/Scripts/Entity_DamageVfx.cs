using System.Collections;
using UnityEngine;

public class Entity_DamageVfx : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("Crit Damage Vfx")]
    [SerializeField] private GameObject critDamageVfx;
    [SerializeField] private Color critDamageColor = Color.white;

    [Header("Impact Vfx")]
    [SerializeField] private GameObject hitImpactVfx;
    [SerializeField] private Color impactColor = Color.white;

    [Header("Damage Vfx")]
    [SerializeField] private Material damagedMat;
    private Material originalMat;
    private Coroutine damageVfxCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = sr.material;
    }

    public void GetImapctVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critDamageVfx : hitImpactVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        vfx.GetComponentInChildren<SpriteRenderer>().color = isCrit ? critDamageColor : impactColor;

        RotateVFX(isCrit);
    }

    public void DamageVfx(float duration)
    {
        if (damageVfxCo != null)
            StopCoroutine(damageVfxCo);

        damageVfxCo = StartCoroutine(DamageVfxCo(duration));
    }

    private IEnumerator DamageVfxCo(float duration)
    {
        sr.material = damagedMat;
        yield return new WaitForSeconds(duration);
        sr.material = originalMat;
    }

    private void RotateVFX(bool isCrit)
    {
        if (entity != null)
            if (entity.faceDir == -1 && isCrit)
                transform.Rotate(0, 180, 0);
    }
}
