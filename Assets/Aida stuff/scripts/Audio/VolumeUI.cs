using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    public GameObject panel;

    public Slider masterSlider;
    public Slider micSlider;

    public Text masterText;
    public Text micText;

    void Start()
    {
        masterSlider.minValue = 0;
        masterSlider.maxValue = 1;

        micSlider.minValue = 0;
        micSlider.maxValue = AudioManager.Instance.maxBoost;

        masterSlider.value = AudioManager.Instance.GetMaster();
        micSlider.value = AudioManager.Instance.GetMic();

        masterSlider.onValueChanged.AddListener(OnMaster);
        micSlider.onValueChanged.AddListener(OnMic);

        UpdateText();

        panel.SetActive(false);
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    void OnMaster(float v)
    {
        AudioManager.Instance.SetMaster(v);
        UpdateText();
    }

    void OnMic(float v)
    {
        AudioManager.Instance.SetMic(v);
        UpdateText();
    }

    void UpdateText()
    {
        if (masterText)
            masterText.text = Mathf.RoundToInt(masterSlider.value * 100) + "%";

        if (micText)
            micText.text = Mathf.RoundToInt((micSlider.value / AudioManager.Instance.maxBoost) * 100) + "%";
    }
}