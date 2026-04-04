using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioDatabaseSO audioDatabase;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

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

    public void PlaySFX(string soundName, AudioSource sfxSource)
    {
        var data = audioDatabase.Get(soundName);
        if (data == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{soundName}' not found in database.");
            return;
        }

        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.clip = clip;
        sfxSource.PlayOneShot(clip);
    }
}
