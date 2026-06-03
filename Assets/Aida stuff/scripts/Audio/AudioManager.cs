using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Settings")]
    public float maxBoost = 2f;

    float masterVolume = 1f;
    float micVolume = 1f;

    AudioSource player1;
    AudioSource player2;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        masterVolume = PlayerPrefs.GetFloat("MasterVol", 1f);
        micVolume = PlayerPrefs.GetFloat("MicVol", 1f);

        ApplyMaster(masterVolume);
        ApplyMic(micVolume);
    }

    // MASTER VOL
    public void SetMaster(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVol", value);
        ApplyMaster(value);
    }

    void ApplyMaster(float value)
    {
        AudioListener.volume = value;
    }

    // PLAYBACK VOL
    public void SetMic(float value)
    {
        micVolume = value;
        PlayerPrefs.SetFloat("MicVol", value);
        ApplyMic(value);
    }

    void ApplyMic(float value)
    {
        float normalized = value / maxBoost;

        float gain = Mathf.Pow(normalized, 2f) * maxBoost;

        if (player1 != null)
            player1.volume = gain;

        if (player2 != null)
            player2.volume = gain;
    }

    // SCENE SWITCH
    public void RegisterSceneSources(AudioSource p1, AudioSource p2)
    {
        player1 = p1;
        player2 = p2;

        ApplyMic(micVolume);
    }

    public float GetMaster() => masterVolume;
    public float GetMic() => micVolume;
}