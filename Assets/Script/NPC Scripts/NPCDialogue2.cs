using UnityEngine;
using System.Collections;

public class NPCDialogue2 : NPCDialogueBase
{
    public GameObject MoneyGenerationPanel;

    protected override void EndDialogue()
    {
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        // wait one frame so Unity doesn't break coroutine blah blah
        yield return null;

        dialoguePanel.SetActive(false);

        MoneyGenerationPanel.SetActive(true);
    }
}