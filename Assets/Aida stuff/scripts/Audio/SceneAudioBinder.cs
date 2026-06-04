using UnityEngine;

public class SceneAudioBinder : MonoBehaviour
{
    public AudioSource player1;
    public AudioSource player2;

    void Start()
    {
        AudioManager.Instance.RegisterSceneSources(player1, player2);
    }
}