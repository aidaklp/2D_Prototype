using UnityEngine;

public class DialogueLocalization : MonoBehaviour
{
    [Header("Dialogue Text")]
    [TextArea(3, 10)]
    public string[] englishSentences;

    [TextArea(3, 10)]
    public string[] dutchSentences;

    [Header("Dialogue Script Reference")]
    public MonoBehaviour dialogueScript;

    private void Awake()
    {
        string[] selectedSentences;

        // Check selected language
        if (LanguageManager.Instance.currentLanguage ==
            LanguageManager.Language.Dutch)
        {
            selectedSentences = dutchSentences;
        }
        else
        {
            selectedSentences = englishSentences;
        }

        // Apply to correct dialogue script
        if (dialogueScript is NPCDialogue1 npc1)
        {
            npc1.Sentences = selectedSentences;
        }
        else if (dialogueScript is NPCDialogue2 npc2)
        {
            npc2.Sentences = selectedSentences;
        }
        else if (dialogueScript is NPCDialogue3 npc3)
        {
            npc3.Sentences = selectedSentences;
        }
    }
}