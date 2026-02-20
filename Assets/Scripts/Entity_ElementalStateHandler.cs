using UnityEngine;

public class Entity_ElementalStateHandler : MonoBehaviour
{
    private Entity entity;

    [SerializeField] private ElementType currentElement;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Start()
    {
        currentElement = ElementType.None;
    }

    public void SetElement(ElementType element)
    {
        currentElement = element;
    }

    public void ApplyBurnEffect(float duration, int damage)
    {

    }

    public void ApplyChilledEffect(float duration, float chillMultiplier)
    {
        entity.TryEnterChillEffect(duration, chillMultiplier);
    }

    public void ApplyShockEffect(float duration, float shockMultiplier)
    {
        entity.TryEnterShockEffect(duration, shockMultiplier);
    }

    public void ApplyBurnedEffect(float duration, float fireDamage, float scaleFactor = 1)
    {
        entity.TryEnterBurnEffect(duration, fireDamage, scaleFactor);
    }

    public bool ApplyElementalVfx(ElementType element)
    {
        if (currentElement == element)
            return false;

        return currentElement == ElementType.None;
    }
}
