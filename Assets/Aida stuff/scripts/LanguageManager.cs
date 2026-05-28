using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language
    {
        English,
        Dutch
    }

    public Language currentLanguage = Language.English;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEnglish()
    {
        currentLanguage = Language.English;
    }

    public void SetDutch()
    {
        currentLanguage = Language.Dutch;
    }
}