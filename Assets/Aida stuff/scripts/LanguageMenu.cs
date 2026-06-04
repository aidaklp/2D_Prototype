using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour
{
    public GameObject languagePanel;

    public Button englishButton;
    public Button dutchButton;

    public Sprite selectedSprite;
    public Sprite normalSprite;

    private void Start()
    {
        UpdateVisuals();
    }

    public void ToggleLanguagePanel()
    {
        languagePanel.SetActive(!languagePanel.activeSelf);
    }

    public void SelectEnglish()
    {
        LanguageManager.Instance.SetEnglish();
        UpdateVisuals();
    }

    public void SelectDutch()
    {
        LanguageManager.Instance.SetDutch();
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (LanguageManager.Instance.currentLanguage ==
            LanguageManager.Language.English)
        {
            englishButton.image.sprite = selectedSprite;
            dutchButton.image.sprite = normalSprite;
        }
        else
        {
            englishButton.image.sprite = normalSprite;
            dutchButton.image.sprite = selectedSprite;
        }
    }
}