using System.Collections;
using UnityEngine;

public class Entity_DamageVfx : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Impact Vfx")]
    [SerializeField] private GameObject impactVfx;
    [SerializeField] private Color impactColor = Color.white;

    [Header("Damage Vfx")]
    [SerializeField] private Material damagedMat;
    private Material originalMat;
    private Coroutine damageVfxCo;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void GetImapctVfx(Transform target)
    {
        GameObject vfx = Instantiate(impactVfx, target.position, Quaternion.identity);

        vfx.GetComponentInChildren<SpriteRenderer>().color = impactColor;
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

}
