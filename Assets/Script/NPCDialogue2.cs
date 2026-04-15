using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections; 

public class NPCDialogue2 : MonoBehaviour
{
   







    public TextMeshProUGUI DialogueText2;

    public GameObject DialoguePanel2;
    public GameObject MoneyGenerationPanel; 

    //uses an array so we can have multiple sentences in the dialogue

    public string[] Sentences;

    private int Index = 0;
    public float DialogueSpeed;

    private bool isTyping = false;

    private bool hasStarted = false;

    private bool hasEnded = false; 

    private void Start()
    {
        DialoguePanel2.SetActive(true);
        NextSentence();
        hasStarted =true;

    }

    private void Update()
    {
      

        

          if ( Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (hasEnded)
            {
                DialoguePanel2.SetActive(false);
                MoneyGenerationPanel.SetActive(true);
            }
            else
            {
                NextSentence();
            }
            

        }
    }

    void NextSentence()
    {
        if (isTyping) return; 
        // subract one because arrays contains zeros and we dont need that
        if (Index <= Sentences.Length - 1)
        {
            DialogueText2.text = "";
            StartCoroutine(WriteSentence());
        }



    }

    IEnumerator WriteSentence()
    {
        isTyping = true; 
        //creates the type writer effect
        foreach (char character in Sentences[Index].ToCharArray())
        {
            DialogueText2.text += character;
            yield return new WaitForSeconds(DialogueSpeed);
            
        }
        Index++;

        // Check if that was the last sentence
        if (Index > Sentences.Length - 1)
        {
            hasEnded = true;
        }

        isTyping = false; 
    }

    
}


