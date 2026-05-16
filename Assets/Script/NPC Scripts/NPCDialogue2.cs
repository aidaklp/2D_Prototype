using UnityEngine;
using System.Collections;

public class NPCDialogue2 : NPCDialogueBase
{
    // panel that appears after dialogue ends
    public GameObject MoneyGenerationPanel;

    protected override void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        StartCoroutine(ShowNext());
    }

    IEnumerator ShowNext()
    {
        yield return null;

        // show next gameplay screen
        MoneyGenerationPanel.SetActive(true);
    }
}