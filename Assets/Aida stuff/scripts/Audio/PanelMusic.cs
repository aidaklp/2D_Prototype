using UnityEngine;

public class PanelMusic : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float volume = 0.5f;
    [SerializeField] private bool loop = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = musicClip;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        if (audioSource != null && musicClip != null)
        {
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}