using UnityEngine;
using System.Collections;

public class NPCDialogue3 : NPCDialogueBase
{
    // handles rating screen / results
    public RatingFlow ratingFlow;

    protected override void EndDialogue()
    {
        StartCoroutine(ShowResults());
    }

    IEnumerator ShowResults()
    {
        yield return null;

        // show final results screen
        ratingFlow.ShowResults();
    }
}