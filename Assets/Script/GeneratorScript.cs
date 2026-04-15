using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 



public class GeneratorScript : MonoBehaviour
{
    // defining variables for each type of genreation/list

    [SerializeField] private TextAsset BPM;
    [SerializeField] private TextAsset Genre;
    [SerializeField] private TextAsset Key;

    //defining our game object (will be used to store the game object containers where the word will appear)
    [SerializeField] private GameObject BPMContainer;
    [SerializeField] private GameObject genreContainer;
    [SerializeField] private GameObject keyContainer;

    //storing the generating button
    [SerializeField] private Button generateButton;

    private TextMeshProUGUI[] genreText;
    private TextMeshProUGUI[] keyText;
    private TextMeshProUGUI[] BPMText;

    //stores the font
    [SerializeField] private TMP_FontAsset musicalFont;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            GenerateBox(BPMContainer, i);
            GenerateBox(genreContainer, i);
            GenerateBox(keyContainer, i);

        }

        genreText = genreContainer.GetComponentsInChildren<TextMeshProUGUI>();
        BPMText = BPMContainer.GetComponentsInChildren<TextMeshProUGUI>();
        keyText = keyContainer.GetComponentsInChildren<TextMeshProUGUI>();
    }


    private void GenerateBox(GameObject container, int value)
    {
        // dollar sign meaning anything that will be put withing the squiggly brackets will be what the variables holding and not the variable name
        GameObject box = new GameObject($"Box{value}");

        //defines where the text will spawn by storing the transform of the word containers
        box.transform.parent = container.transform;

        box.transform.localPosition = new Vector3(0, -1100 + (value * 100), 0);
        box.transform.localScale = new Vector3(1, 1, 1);

        //allows the box image to display text
        box.AddComponent<TextMeshProUGUI>();

        //if a word is too long to fit in the box it will go outside the box instead of going under it onto a new line
        box.GetComponent<TextMeshProUGUI>().textWrappingMode = TextWrappingModes.NoWrap;

        //makes the text in the middle
        box.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        //changes text to black 
        box.GetComponent<TextMeshProUGUI>().color = Color.black;

        //changes the font
        box.GetComponent<TextMeshProUGUI>().font = musicalFont;





    }

    public void Generate()
    {
        //resets the boxes 
        genreContainer.transform.localPosition = new Vector3(genreContainer.transform.localPosition.x, 1100);
        BPMContainer.transform.localPosition = new Vector3(BPMContainer.transform.localPosition.x, 1100);
        keyContainer.transform.localPosition = new Vector3(keyContainer.transform.localPosition.x, 1100);

        generateButton.interactable = false;

        for (int i = 0; i < 24; i++) {
            genreText[i].text = ReturnWord(Genre).ToUpper();
            BPMText[i].text = ReturnWord(BPM).ToUpper();
            keyText[i].text = ReturnWord(Key).ToUpper();
        }

        //saves the varaibles somwehere elese than this scrip so the stored variables will still exist nd can be reffrenced outside of this scene
        GeneratorData.Instance.FinalBPM = BPMText[9].text;
        GeneratorData.Instance.FinalGenre = genreText[9].text;
        GeneratorData.Instance.FinalKey = keyText[9].text;

        StartCoroutine(AnimateRoll(0, genreContainer));
        StartCoroutine(AnimateRoll(0.5f, BPMContainer));
        StartCoroutine(AnimateRoll(1f, keyContainer, loadNextScene : true)); // last one triggers scene change

        string RequirementsOutput = $"{Genre} {BPM} {Key}";
        Debug.Log("Outputted requirement: {GeneratorData.Instance.FinalGenre} {GeneratorData.Instance.FinalBPM} {GeneratorData.Instance.FinalKey}");
    }

    private string ReturnWord(TextAsset file)
    {
        //generates ranom number and will search for the word associated with that number 
        string[] lines = file.text.Split("\n");
        
        string line = lines[Random.Range(0, lines.Length - 1)];

        return line.Substring(0, line.Length - 1); 

    }
    private IEnumerator AnimateRoll(float delay, GameObject container, bool loadNextScene = false)
    {
        //this is so the Bpm can come down a little after the genre
        yield return new WaitForSecondsRealtime(delay);

        float targetY = 200f;

        while (container.transform.localPosition.y > targetY)
        {
            container.transform.localPosition = new Vector3(
                container.transform.localPosition.x,
                container.transform.localPosition.y - 4f,
                0f
            );
            yield return new WaitForSecondsRealtime(0.005f);
        }
        
        // snap exactly to target so it's perfectly centered
        container.transform.localPosition = new Vector3(
            container.transform.localPosition.x,
            targetY,
            0f
        );


        if (loadNextScene)
        {
            yield return new WaitForSecondsRealtime(3f);
            SceneManager.LoadScene(1);
        }

    }
}