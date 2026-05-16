using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public abstract class NPCDialogueBase : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    //lines of dialogue
    public string[] sentences;
    public float dialogueSpeed;

    protected int index = 0;
    protected bool isTyping = false;

    //stop typing if skip
    protected Coroutine typingCoroutine;

    protected virtual void Start()
    {
        index = 0;

        dialoguePanel.SetActive(true);

        NextSentence();
    }

    protected virtual void Update()
    {
        if (!dialoguePanel.activeSelf) return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            // if text is still typing, skip to full sentence
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                NextSentence();
            }
        }
    }

    void SkipTyping()
    {
        // stop the typing effect immediately
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // instantly show full sentence
        dialogueText.text = sentences[index];

        isTyping = false;
        index++;
    }

    protected void NextSentence()
    {
        //finish dialogue
        if (index >= sentences.Length)
        {
            StartCoroutine(EndDialogueNextFrame());
            return;
        }

        dialogueText.text = "";

        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;

        // type letter by letter for effect
        foreach (char c in sentences[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }

        // move forward after finishing sentence
        index++;
        isTyping = false;
    }

    IEnumerator EndDialogueNextFrame()
    {
        // small delay so UI doesn't glitch when switching states
        yield return null;
        EndDialogue();
    }

    // each NPC decides what happens when dialogue ends
    protected abstract void EndDialogue();
}