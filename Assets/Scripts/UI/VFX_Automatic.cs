using UnityEngine;

public class VFX_Automatic : MonoBehaviour
{
    [Header("Position Offset")]
    [SerializeField] private float minXOffset = -0.3f;
    [SerializeField] private float maxXOffset = 0.3f;
    [Space]
    [SerializeField] private float minYOffset = -0.3f;
    [SerializeField] private float maxYOffset = 0.3f;
    [Space]
    [SerializeField] private bool isRandomizePos = true;

    [Header("Rotation Offset")]
    [SerializeField] private float minRotZOffset = 0f;
    [SerializeField] private float maxRotZOffset = 360f;
    [SerializeField] private bool isRandomizeRotZ = true;

    [Header("Automatic Destroy")]
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private bool isAutomaticDestroy = true;

    private void Start()
    {
        if (isRandomizePos)
            GenerationPosOffset();
        if (isRandomizeRotZ)
            GenerationRotOffset();

        if (isAutomaticDestroy)
            AutomaticDestroy();
    }
    private void AutomaticDestroy()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void GenerationPosOffset()
    {
        float xOffset = Random.Range(minXOffset, maxXOffset);
        float yOffset = Random.Range(minYOffset, maxYOffset);

        transform.position += new Vector3(xOffset, yOffset);
    }

    private void GenerationRotOffset()
    {
        float rotZOffset = Random.Range(minRotZOffset, maxRotZOffset);

        transform.rotation = Quaternion.Euler(0, 0, rotZOffset);
    }
}
