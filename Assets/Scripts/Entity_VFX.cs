using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    private Color originalColor;

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

    [Header("Elemental Vfx")]
    [SerializeField] private Color chillVfx;
    [SerializeField] private Color fireVfx;
    [SerializeField] private Color lightningVfx;
    private Coroutine elementalVfxCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMat = sr.material;
        originalColor = sr.color;
        chillVfx = new Color(0.5f, 0.5f, 1f, 0.5f); // Light blue with some transparency
        fireVfx = new Color(1f, 0.5f, 0.5f, 0.5f); // Light red with some transparency
        lightningVfx = new Color(1f, 1f, 0.5f, 0.5f); // Light yellow with some transparency
    }

    public void GetImapctVfx(Transform target, bool isCrit)
    {
        GameObject hitPrefab = isCrit ? critDamageVfx : hitImpactVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);

        vfx.GetComponentInChildren<SpriteRenderer>().color = isCrit ? critDamageColor : impactColor;

        RotateVFX(isCrit);
    }

    public void ElementVfx(float duration, ElementType element)
    {
        if (element == ElementType.None)
            return;

        if (elementalVfxCo != null)
            StopCoroutine(elementalVfxCo);

        Color elementColor = Color.white;


        switch (element)
        {
            case ElementType.Ice:
                elementColor = chillVfx;
                break;
            case ElementType.Fire:
                elementColor = fireVfx;
                break;
            case ElementType.Lightning:
                elementColor = lightningVfx;
                break;
        }

        elementalVfxCo = StartCoroutine(ElementVfxCo(duration, elementColor));
    }

    private IEnumerator ElementVfxCo(float duration, Color effectColor)
    {
        float elapsed = 0f;
        float interval = 0.2f;

        bool toggle = false;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        while (elapsed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        sr.color = originalColor;
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
