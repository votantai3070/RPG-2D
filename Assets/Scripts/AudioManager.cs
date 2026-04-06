using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioDatabaseSO audioDatabase;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    private Transform player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5)
    {
        if (player == null)
            player = Player.instance.transform;

        var data = audioDatabase.Get(soundName);
        if (data == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{soundName}' not found in database.");
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null) return;

        float maxVolume = data.maxVolume;
        float distanceToPlayer = Vector3.Distance(player.position, sfxSource.transform.position);

        float t = Mathf.Clamp01(1 - (distanceToPlayer / minDistanceToHearSound));

        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);  // Use a quadratic curve for smoother falloff
        sfxSource.pitch = Random.Range(0.95f, 1.05f); // Add slight pitch variation for more natural sound
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDatabase.Get(soundName);
        if (data == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{soundName}' not found in database.");
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.05f); // Add slight pitch variation for more natural sound
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
