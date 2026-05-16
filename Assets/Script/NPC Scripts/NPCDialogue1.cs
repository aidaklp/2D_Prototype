using UnityEngine;
using System.Collections;

public class NPCDialogue1 : NPCDialogueBase
{
    public RecordMultipleAudios recordMultipleAudios;

    protected override void EndDialogue()
    {
        StartCoroutine(Advance());
    }

    IEnumerator Advance()
    {
        // wait one frame so Unity doesn't choke on UI state change
        yield return null;

        dialoguePanel.SetActive(false);

        recordMultipleAudios.DialogueFinishedAndAdvance();
    }
}