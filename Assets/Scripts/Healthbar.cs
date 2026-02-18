using UnityEngine;

public class Healthbar : MonoBehaviour
{
    private void OnEnable()
    {
        Entity.OnFlipped += HandleRotDefault;
    }

    private void OnDisable()
    {
        Entity.OnFlipped -= HandleRotDefault;
    }

    private void HandleRotDefault() => transform.rotation = Quaternion.identity;
}
