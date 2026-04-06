using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("SFX")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;
    [Space]
    [SerializeField] private float soundDistance = 15;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(attackHit, audioSource, soundDistance);
    }

    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX(attackMiss, audioSource, soundDistance);
    }
}
