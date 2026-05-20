//using UnityEditor.Build.Content;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
//using Unity.VisualScripting;


public class NPCDialogue1 : MonoBehaviour
{
    public RecordMultipleAudios recordMultipleAudios;

    public TextMeshProUGUI DialogueText1;

    public GameObject DialoguePanel1; 

    //uses an array so we can have multiple sentences in the dialogue

    public string[] Sentences;

    private int Index = 0;
    public float DialogueSpeed;

    //helps prevent f from being spamed while the text is typing
    private bool isTyping = false;

    // will allow a check for the first sentence to be triggered before the next
    private bool hasStarted = false;



    private void Update()
    {
        if (recordMultipleAudios.currentPhase == RecordMultipleAudios.GamePhase.Dialogue1)
        {
            if (!hasStarted) {
                hasStarted = true;
                NextSentence();
                    }

            if (Keyboard.current.fKey.wasPressedThisFrame) {
                NextSentence();
            }
            //checks if e is pressed and if so calls on the Skip all dialogue method
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                SkipAllDialogue();
            }
        }
    }

    void NextSentence()
    {
        if (isTyping) return;


        // subract one because arrays contains zeros and we dont need that
        if(Index <= Sentences.Length - 1)
        {
            DialogueText1.text = "";
            StartCoroutine(WriteSentence());
        }

        else 
        {
            
            DialoguePanel1.SetActive(false);
            //once sentences are finished it calls for the dialogue finished an advance method defined in record multiple audios script to start the audios
            recordMultipleAudios.DialogueFinishedAndAdvance(); 
        }
    }

    IEnumerator WriteSentence()
    {
        isTyping = true; 

        //creates the type writer effect
        foreach(char character in Sentences[Index].ToCharArray())
        {
            DialogueText1.text += character;
            yield return new WaitForSeconds(DialogueSpeed); 
        
        }
        Index++;
        isTyping =false;
    }

    void OnDialogueFinished()
    {
        recordMultipleAudios.DialogueFinishedAndAdvance();
    }

    void SkipAllDialogue()
    {
        //stops all coroutines but used to stop the write sentence coroutine
        StopAllCoroutines ();
        //sets is typing back to false
        isTyping = false;

        //this jumps the index to the end of the array to now tll the code that there are not more sentences left to type  out 
        Index = Sentences.Length;
        //closes the dialogua pannel
        DialoguePanel1.SetActive(false);
        //reffrences the dialogue finished and advance method from the record multiple audios script
        recordMultipleAudios.DialogueFinishedAndAdvance();
    }
}
