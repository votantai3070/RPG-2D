using System.Collections;
using UnityEngine;

public class VFX_Automatic : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Position Offset")]
    [SerializeField] private float minXOffset = -0.3f;
    [SerializeField] private float maxXOffset = 0.3f;
    [Space]
    [SerializeField] private float minYOffset = -0.3f;
    [SerializeField] private float maxYOffset = 0.3f;
    [Space]
    [SerializeField] private bool isRandomizePos = true;

    [Header("Fade effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Rotation Offset")]
    [SerializeField] private float minRotZOffset = 0f;
    [SerializeField] private float maxRotZOffset = 360f;
    [SerializeField] private bool isRandomizeRotZ = true;

    [Header("Automatic Destroy")]
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private bool isAutomaticDestroy = true;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
            StartCoroutine(FadeCo());

        if (isRandomizePos)
            GenerationPosOffset();
        if (isRandomizeRotZ)
            GenerationRotOffset();

        if (isAutomaticDestroy)
            AutomaticDestroy();
    }

    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;

        while (targetColor.a > 0)
        {
            targetColor.a -= (fadeSpeed * Time.deltaTime);
            sr.color = targetColor;
            yield return null;
        }

        sr.color = targetColor;
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
