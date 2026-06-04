using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [TextArea(2, 5)]
    public string englishText;

    [TextArea(2, 5)]
    public string dutchText;

    private TextMeshProUGUI textComponent;

    private LanguageManager.Language lastLanguage;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ApplyLanguage();
        CacheLanguage();
    }

    private void Update()
    {
        // detect language change without needing other scripts
        if (LanguageManager.Instance == null)
            return;

        if (LanguageManager.Instance.currentLanguage != lastLanguage)
        {
            ApplyLanguage();
            CacheLanguage();
        }
    }

    private void CacheLanguage()
    {
        if (LanguageManager.Instance != null)
        {
            lastLanguage = LanguageManager.Instance.currentLanguage;
        }
    }

    public void ApplyLanguage()
    {
        if (LanguageManager.Instance == null || textComponent == null)
            return;

        if (LanguageManager.Instance.currentLanguage ==
            LanguageManager.Language.Dutch)
        {
            textComponent.text = dutchText;
        }
        else
        {
            textComponent.text = englishText;
        }
    }
}