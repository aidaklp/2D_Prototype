using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections; 

public class NPCDialogue3: MonoBehaviour
{
  



    public TextMeshProUGUI DialogueText3;
    public GameObject DialoguePanel3;


    //reffrences the rating flow script
    public RatingFlow ratingFlow;
  


    //uses an array so we can have multiple sentences in the dialogue

    public string[] Sentences;

    private int Index = 0;
    public float DialogueSpeed;

    private bool isTyping = false;

    private bool hasStarted = false;

    private bool hasEnded = false;



    private void OnEnable()
    {
        Debug.Log("Dialogue3 enabled");
        Index = 0;
        hasEnded = false;
        NextSentence();
    }

    private void Update()
    {

        if (!DialoguePanel3.activeSelf) return;  



        if ( Keyboard.current.fKey.wasPressedThisFrame )
        {
            Debug.Log("F pressed, hasEnded: " + hasEnded + " Index: " + Index + " Sentences.Length: " + Sentences.Length);

            if (hasEnded)
            {
                Debug.Log("Calling ShowResults");
                ratingFlow.ShowResults();
         
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
            DialogueText3.text = "";
            StartCoroutine(WriteSentence());
        }



    }

    IEnumerator WriteSentence()
    {
        isTyping = true; 
        //creates the type writer effect
        foreach (char character in Sentences[Index].ToCharArray())
        {
            DialogueText3.text += character;
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


