using UnityEngine;

public class Automatic_Vfx : MonoBehaviour
{
    [SerializeField] private float minXOffset = -0.3f;
    [SerializeField] private float maxXOffset = 0.3f;
    [SerializeField] private float minYOffset = -0.3f;
    [SerializeField] private float maxYOffset = 0.3f;

    private void Start()
    {
        GenerationPosOffset();
        GenerationRotOffset();

        AutomaticDestroy();
    }
    private void AutomaticDestroy()
    {
        Destroy(gameObject, 1);
    }

    private void GenerationPosOffset()
    {
        float xOffset = Random.Range(minXOffset, maxXOffset);
        float yOffset = Random.Range(minYOffset, maxYOffset);

        transform.position += new Vector3(xOffset, yOffset);
    }

    private void GenerationRotOffset()
    {
        float rotZOffset = Random.Range(0, 360);

        transform.rotation = Quaternion.Euler(0, 0, rotZOffset);
    }
}
