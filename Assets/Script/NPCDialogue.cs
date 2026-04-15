using UnityEditor.Build.Content;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class NPCDialogue1 : MonoBehaviour
{
    public RecordMultipleAudios recordMultipleAudios;

    public TextMeshProUGUI DialogueText1;

    public GameObject DialoguePanel1; 

    //uses an array so we can have multiple sentences in the dialogue

    public string[] Sentences;

    private int Index = 0;
    public float DialogueSpeed;

    private void Update()
    {
        if (recordMultipleAudios.currentPhase == RecordMultipleAudios.GamePhase.Dialogue1)
        {
            NextSentence(); 
        }
    }

    void NextSentence()
    {
        // subract one because arrays contains zeros and we dont need that
        if(Index <= Sentences.Length - 1)
        {
            DialogueText1.text = "";
            StartCoroutine(WriteSentence());
        }

        else if(Keyboard.current.fKey.wasPressedThisFrame)
        {
            
            DialoguePanel1.SetActive(false);
            //once sentences are finished it calls for the dialogue finished an advance method defined in record multiple audios script to start the audios
            recordMultipleAudios.DialogueFinishedAndAdvance(); 
        }
    }

    IEnumerator WriteSentence()
    {
        //creates the type writer effect
        foreach(char character in Sentences[Index].ToCharArray())
        {
            DialogueText1.text += character;
            yield return new WaitForSeconds(DialogueSpeed); 
            Index++; 
        }
    }

    void OnDialogueFinished()
    {
        recordMultipleAudios.DialogueFinishedAndAdvance();
    }
}
