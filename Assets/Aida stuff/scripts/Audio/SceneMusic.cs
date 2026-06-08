using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] public float volume = 0.5f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.volume = volume;

        audioSource.Play();
    }
}