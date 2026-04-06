using UnityEngine;

public class AudioRangeController : MonoBehaviour
{
    private AudioSource source;
    private Transform player;


    [SerializeField] private float minDistanceToHearSound = 12;
    [SerializeField] private bool showGizmo;
    private float maxVolume;

    private void Start()
    {
        player = Player.instance.transform;
        source = GetComponent<AudioSource>();

        maxVolume = source.volume;
    }

    private void Update()
    {
        if (player == null || source == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float t = Mathf.Clamp01(1 - (distanceToPlayer / minDistanceToHearSound));

        float targetVolume = Mathf.Lerp(0, maxVolume, t * t);
        source.volume = Mathf.Lerp(source.volume, targetVolume, Time.deltaTime * 3); // Smoothly transition the volume
    }

    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, minDistanceToHearSound);
        }
    }
}
