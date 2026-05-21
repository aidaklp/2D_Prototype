using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject volumePanel;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider micSlider;

    [Header("Slider Labels")]
    public Text masterValueText;
    public Text micValueText;

    [Header("Audio Sources")]
    public AudioSource micAudioSource;
    public AudioSource metronomeAudioSource;

    private bool panelOpen = false;

    void Start()
    {
        masterSlider.minValue = 0f;
        masterSlider.maxValue = 1f;
        micSlider.minValue = 0f;
        micSlider.maxValue = 1f;

        masterSlider.value = PlayerPrefs.GetFloat("MasterVol", 1f);
        micSlider.value = PlayerPrefs.GetFloat("MicVol", 1f);

        ApplyMaster(masterSlider.value);
        ApplyMic(micSlider.value);

        masterSlider.onValueChanged.AddListener(ApplyMaster);
        micSlider.onValueChanged.AddListener(ApplyMic);

        volumePanel.SetActive(false);
    }

    void ApplyMaster(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVol", value);
        if (masterValueText != null)
            masterValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    void ApplyMic(float value)
    {
        if (micAudioSource != null)
            micAudioSource.volume = value;
        PlayerPrefs.SetFloat("MicVol", value);
        if (micValueText != null)
            micValueText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void TogglePanel()
    {
        panelOpen = !panelOpen;
        volumePanel.SetActive(panelOpen);
    }

    public void ClosePanel()
    {
        panelOpen = false;
        volumePanel.SetActive(false);
    }

    void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveListener(ApplyMaster);
        micSlider.onValueChanged.RemoveListener(ApplyMic);
    }
}